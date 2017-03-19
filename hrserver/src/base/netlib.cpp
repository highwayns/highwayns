#include "netlib.h"
#include "util.h"

#define __LIBEVENT__

#ifndef __LIBEVENT__

#include "BaseSocket.h"
#include "EventDispatch.h"

int netlib_init()
{
	int ret = NETLIB_OK;
#ifdef _WIN32
	WSADATA wsaData;
	WORD wReqest = MAKEWORD(1, 1);
	if (WSAStartup(wReqest, &wsaData) != 0)
	{
		ret = NETLIB_ERROR;
	}
#endif

	return ret;
}

int netlib_destroy()
{
	int ret = NETLIB_OK;
#ifdef _WIN32
	if (WSACleanup() != 0)
	{
		ret = NETLIB_ERROR;
	}
#endif

	return ret;
}

int netlib_listen(
		const char*	server_ip, 
		uint16_t	port,
		callback_t	callback,
		void*		callback_data)
{
	auto spSocket = sp_CBaseSocket(new CBaseSocket());
	if (!spSocket)
		return NETLIB_ERROR;

	int ret =  spSocket->Listen(server_ip, port, callback, callback_data);
	// if (ret == NETLIB_ERROR)
	// 	delete pSocket;
	return ret;
}

net_handle_t netlib_connect(
		const char* server_ip, 
		uint16_t	port, 
		callback_t	callback, 
		void*		callback_data)
{
	auto spSocket = sp_CBaseSocket(new CBaseSocket());
	if (!spSocket)
		return NETLIB_INVALID_HANDLE;

	net_handle_t handle = spSocket->Connect(server_ip, port, callback, callback_data);
	// if (handle == NETLIB_INVALID_HANDLE)
	// 	delete pSocket;
	return handle;
}

int netlib_send(net_handle_t handle, void* buf, int len)
{
	auto spSocket = FindBaseSocket(handle);
	if (!spSocket)
	{
		return NETLIB_ERROR;
	}
	int ret = spSocket->Send(buf, len);
	// pSocket->ReleaseRef();
	return ret;
}

int netlib_recv(net_handle_t handle, void* buf, int len)
{
	auto spSocket = FindBaseSocket(handle);
	if (!spSocket)
		return NETLIB_ERROR;

	int ret = spSocket->Recv(buf, len);
	// pSocket->ReleaseRef();
	return ret;
}

int netlib_close(net_handle_t handle)
{
	auto spSocket = FindBaseSocket(handle);
	if (!spSocket)
		return NETLIB_ERROR;

	int ret = spSocket->Close();
	// pSocket->ReleaseRef();
	return ret;
}

int netlib_option(net_handle_t handle, int opt, void* optval)
{
	auto spSocket = FindBaseSocket(handle);
	if (!spSocket)
		return NETLIB_ERROR;

	if ((opt >= NETLIB_OPT_GET_REMOTE_IP) && !optval)
		return NETLIB_ERROR;

	switch (opt)
	{
	case NETLIB_OPT_SET_CALLBACK:
		spSocket->SetCallback((callback_t)optval);
		break;
	case NETLIB_OPT_SET_CALLBACK_DATA:
		spSocket->SetCallbackData(optval);
		break;
	case NETLIB_OPT_GET_REMOTE_IP:
		*(string*)optval = spSocket->GetRemoteIP();
		break;
	case NETLIB_OPT_GET_REMOTE_PORT:
		*(uint16_t*)optval = spSocket->GetRemotePort();
		break;
	case NETLIB_OPT_GET_LOCAL_IP:
		*(string*)optval = spSocket->GetLocalIP();
		break;
	case NETLIB_OPT_GET_LOCAL_PORT:
		*(uint16_t*)optval = spSocket->GetLocalPort();
		break;
	case NETLIB_OPT_SET_SEND_BUF_SIZE:
		spSocket->SetSendBufSize(*(uint32_t*)optval);
		break;
	case NETLIB_OPT_SET_RECV_BUF_SIZE:
		spSocket->SetRecvBufSize(*(uint32_t*)optval);
		break;
	}

	// pSocket->ReleaseRef();
	return NETLIB_OK;
}

int netlib_register_timer(callback_t callback, void* user_data, uint64_t interval)
{
	CEventDispatch::Instance()->AddTimer(callback, user_data, interval);
	return 0;
}

int netlib_delete_timer(callback_t callback, void* user_data)
{
	CEventDispatch::Instance()->RemoveTimer(callback, user_data);
	return 0;
}

int netlib_add_loop(callback_t callback, void* user_data)
{
	CEventDispatch::Instance()->AddLoop(callback, user_data);
	return 0;
}

void netlib_eventloop(uint32_t wait_timeout)
{
	CEventDispatch::Instance()->StartDispatch(wait_timeout);
}

void netlib_stop_event()
{
    CEventDispatch::Instance()->StopDispatch();
}

bool netlib_is_running()
{
    return CEventDispatch::Instance()->isRunning();
}

#else

using namespace std;

static unordered_map<net_handle_t, struct event*> g_read_event_map;
static unordered_map<net_handle_t, struct event*> g_write_event_map;
static unordered_map<callback_t, struct event*> g_timer_map;
struct event_base* g_libevent_base;

static string _GetRemoteIP(net_handle_t hd);
static uint16_t _GetRemotePort(net_handle_t hd);
static string _GetLocalIP(net_handle_t hd);
static uint16_t _GetLocalPort(net_handle_t hd);
static void _SetSendBufSize(net_handle_t hd, uint32_t send_size);
static void _SetRecvBufSize(net_handle_t hd, uint32_t recv_size);

static int _GetErrorCode();
static void _SetNonblock(SOCKET fd);
static bool _IsBlock(int error_code);
static void _SetReuseAddr(SOCKET fd);
static void _SetNoDelay(SOCKET fd);
static void _SetAddr(const char* ip, const uint16_t port, sockaddr_in* pAddr);

void netlib_onconfirm(evutil_socket_t fd, short what, void *arg);
void netlib_onread(evutil_socket_t fd, short what, void *arg);
void netlib_onwrite(evutil_socket_t fd, short what, void *arg);
void netlib_onaccept(evutil_socket_t fd, short what, void *arg);
void netlib_ontimer(evutil_socket_t fd, short what, void* arg);

void netlib_check_write_error(net_handle_t fd, int* error, socklen_t* len);
void netlib_set_onconnect_event(net_handle_t handle, callback_t callback, void* cbdata);

struct EvtArg {
	callback_t callback;
	void* cbdata;
	
	EvtArg(callback_t c, void* d) : callback(c), cbdata(d) {}
	~EvtArg() {}
};

struct EvtArg2 {
	callback_t callback;
	void* cbdata;
	struct event* evt;
	
	EvtArg2(callback_t c, void* d, struct event* e) : callback(c), cbdata(d), evt(e) {}
	~EvtArg2() {}
};

static int _GetErrorCode()
{
#ifdef _WIN32
	return WSAGetLastError();
#else
	return errno;
#endif
}

static void _SetNonblock(SOCKET fd)
{
#ifdef _WIN32
	u_long nonblock = 1;
	int ret = ioctlsocket(fd, FIONBIO, &nonblock);
#else
	int ret = fcntl(fd, F_SETFL, O_NONBLOCK | fcntl(fd, F_GETFL));
#endif
	if (ret == SOCKET_ERROR)
	{
		log("_SetNonblock failed, err_code=%d", _GetErrorCode());
	}
}

static bool _IsBlock(int error_code)
{
#ifdef _WIN32
	return ( (error_code == WSAEINPROGRESS) || (error_code == WSAEWOULDBLOCK) );
#else
	return ( (error_code == EINPROGRESS) || (error_code == EWOULDBLOCK) );
#endif
}

static void _SetReuseAddr(SOCKET fd)
{
	int reuse = 1;
	int ret = setsockopt(fd, SOL_SOCKET, SO_REUSEADDR, (char*)&reuse, sizeof(reuse));
	if (ret == SOCKET_ERROR)
	{
		log("_SetReuseAddr failed, err_code=%d", _GetErrorCode());
	}
}

static void _SetNoDelay(SOCKET fd)
{
	int nodelay = 1;
	int ret = setsockopt(fd, IPPROTO_TCP, TCP_NODELAY, (char*)&nodelay, sizeof(nodelay));
	if (ret == SOCKET_ERROR)
	{
		log("_SetNoDelay failed, err_code=%d", _GetErrorCode());
	}
}

static void _SetAddr(const char* ip, const uint16_t port, sockaddr_in* pAddr)
{
	memset(pAddr, 0, sizeof(sockaddr_in));
	pAddr->sin_family = AF_INET;
	pAddr->sin_port = htons(port);
	pAddr->sin_addr.s_addr = inet_addr(ip);
	if (pAddr->sin_addr.s_addr == INADDR_NONE)
	{
		hostent* host = gethostbyname(ip);
		if (host == NULL)
		{
			log("gethostbyname failed, ip=%s", ip);
			return;
		}

		pAddr->sin_addr.s_addr = *(uint32_t*)host->h_addr;
	}
}

int netlib_init()
{
	int ret = NETLIB_OK;
#ifdef _WIN32
	WSADATA wsaData;
	WORD wReqest = MAKEWORD(1, 1);
	if (WSAStartup(wReqest, &wsaData) != 0) {
		ret = NETLIB_ERROR;
	}
#endif
	g_libevent_base = event_base_new();
	return ret;
}

int netlib_destroy()
{
	int ret = NETLIB_OK;
#ifdef _WIN32
	if (WSACleanup() != 0) {
		ret = NETLIB_ERROR;
	}
#endif
	event_base_free(g_libevent_base);
	return ret;
}

net_handle_t netlib_connect(
		const char* server_ip, 
		uint16_t	port, 
		callback_t	callback, 
		void*		callback_data)
{
	net_handle_t sock = socket(AF_INET, SOCK_STREAM, 0);
	if (sock == INVALID_SOCKET) {
		loge("socket failed, err_code=%d", _GetErrorCode());
		return NETLIB_INVALID_HANDLE;
	}
	
	_SetNonblock(sock);
	_SetNoDelay(sock);
	
	sockaddr_in serv_addr;
	_SetAddr(server_ip, port, &serv_addr);
	int ret = connect(sock, (sockaddr*)&serv_addr, sizeof(serv_addr));
	if ( (ret == SOCKET_ERROR) && (!_IsBlock(_GetErrorCode())) ) {
		loge("connect failed, err_code=%d", _GetErrorCode());
		closesocket(sock);
		return NETLIB_INVALID_HANDLE;
	}
	
	auto evtArg2 = new EvtArg2(callback, callback_data, NULL);
	evtArg2->evt = event_new(g_libevent_base, sock, EV_WRITE, netlib_onconfirm, evtArg2);
	event_add(evtArg2->evt, NULL);

	return sock;
}

int netlib_close(net_handle_t handle)
{
	auto it = g_read_event_map.find(handle);
	if (it != g_read_event_map.end()) {
		auto ev = it->second;
		g_read_event_map.erase(it);
		event_free(ev);
	}
	
	auto it2 = g_write_event_map.find(handle);
	if (it2 != g_write_event_map.end()) {
		auto ev = it2->second;
		g_write_event_map.erase(it2);
		event_free(ev);
	}
	
	closesocket(handle);
	return 0;
}


int netlib_listen(
		const char*	server_ip, 
		uint16_t	port,
		callback_t	callback,
		void*		callback_data)
{
	auto sock = socket(AF_INET, SOCK_STREAM, 0);
	if (sock == INVALID_SOCKET) {
		printf("socket failed, err_code=%d\n", _GetErrorCode());
		return NETLIB_ERROR;
	}
	
	_SetReuseAddr(sock);
	_SetNonblock(sock);
	
	sockaddr_in serv_addr;
	_SetAddr(server_ip, port, &serv_addr);
	int ret = bind(sock, (sockaddr*)&serv_addr, sizeof(serv_addr));
	if (ret == SOCKET_ERROR) {
		loge("bind failed, err_code=%d", _GetErrorCode());
		closesocket(sock);
		return NETLIB_ERROR;
	}
	
	ret = listen(sock, 64);
	if (ret == SOCKET_ERROR) {
		loge("listen failed, err_code=%d", _GetErrorCode());
		closesocket(sock);
		return NETLIB_ERROR;
	}

	auto evtArg = new EvtArg(callback, callback_data);
	struct event* ev = event_new(g_libevent_base, sock, EV_READ|EV_PERSIST, netlib_onaccept, evtArg);
	event_add(ev, NULL);
	
	return NETLIB_OK;
	
}


void netlib_onaccept(evutil_socket_t fd, short what, void *arg)
{
	sockaddr_in peer_addr;
	socklen_t addr_len = sizeof(sockaddr_in);
	char ip_str[64];

	while ( (fd = accept(fd, (sockaddr*)&peer_addr, &addr_len)) != INVALID_SOCKET )
	{
		_SetNoDelay(fd);
		_SetNonblock(fd);
		
		auto evtArg = (EvtArg*)arg;
		evtArg->callback(evtArg->cbdata, NETLIB_MSG_CONNECT, (net_handle_t)fd, NULL);
	}	
}

void netlib_check_write_error(net_handle_t fd, int* error, socklen_t* len)
{
#ifdef _WIN32
	getsockopt(fd, SOL_SOCKET, SO_ERROR, (char*)error, len);
#else
	getsockopt(fd, SOL_SOCKET, SO_ERROR, (void*)error, len);
#endif	
}

void netlib_onconfirm(evutil_socket_t fd, short what, void *arg)
{
	auto evtArg2 = (EvtArg2*)arg;
	int error = 0;
	socklen_t len = sizeof(error);
	netlib_check_write_error((net_handle_t)fd, &error, &len);
	if (error) {
		evtArg2->callback(evtArg2->cbdata, NETLIB_MSG_CLOSE, (net_handle_t)fd, NULL);
	} else {
		event_free(evtArg2->evt);
		auto arg = new EvtArg(evtArg2->callback, evtArg2->cbdata);
		struct event* evread = event_new(g_libevent_base, fd, EV_READ|EV_PERSIST|EV_ET, netlib_onread, arg);
		struct event* evwrite = event_new(g_libevent_base, fd, EV_WRITE|EV_PERSIST|EV_ET, netlib_onwrite, arg);
		event_add(evread, NULL);
		event_add(evwrite, NULL);
		g_read_event_map[fd] = evread;
		g_write_event_map[fd] = evwrite;
		evtArg2->callback(evtArg2->cbdata, NETLIB_MSG_CONFIRM, (net_handle_t)fd, NULL);
	}
}

void netlib_onread(evutil_socket_t fd, short what, void *arg)
{
	auto evtArg = (EvtArg*)arg;
	evtArg->callback(evtArg->cbdata, NETLIB_MSG_READ, (net_handle_t)fd, NULL);
}

void netlib_onwrite(evutil_socket_t fd, short what, void *arg)
{
	auto evtArg = (EvtArg*)arg;
	evtArg->callback(evtArg->cbdata, NETLIB_MSG_WRITE, (net_handle_t)fd, NULL);
}

void netlib_set_onconnect_event(net_handle_t handle, callback_t callback, void* cbdata)
{
	auto arg = new EvtArg(callback, cbdata);
	struct event* evread = event_new(g_libevent_base, handle, EV_READ|EV_PERSIST|EV_ET, netlib_onread, arg);
	struct event* evwrite = event_new(g_libevent_base, handle, EV_WRITE|EV_PERSIST|EV_ET, netlib_onwrite, arg);
	event_add(evread, NULL);
	event_add(evwrite, NULL);
	g_read_event_map[handle] = evread;
	g_write_event_map[handle] = evwrite;
}

string _GetRemoteIP(net_handle_t hd)
{
	struct sockaddr_in sa;
	socklen_t len = sizeof(sa);
	if (!getpeername(hd, (struct sockaddr*)&sa, &len)) {
		return inet_ntoa(sa.sin_addr);
	} else {
		return "";
	}
}

uint16_t _GetRemotePort(net_handle_t hd)
{
	struct sockaddr_in sa;
	socklen_t len = sizeof(sa);
	if (!getpeername(hd, (struct sockaddr*)&sa, &len)) {
		return ntohs(sa.sin_port);
	} else {
		return 0;
	}	
}

string _GetLocalIP(net_handle_t hd)
{
	struct sockaddr_in sa;
	socklen_t len = sizeof(sa);
	if (!getsockname(hd, (struct sockaddr*)&sa, &len)) {
		return inet_ntoa(sa.sin_addr);
	} else {
		return "";
	}	
}

uint16_t _GetLocalPort(net_handle_t hd)
{
	struct sockaddr_in sa;
	socklen_t len = sizeof(sa);
	if (!getsockname(hd, (struct sockaddr*)&sa, &len)) {
		return ntohs(sa.sin_port);
	} else {
		return 0;
	}	
}

void _SetSendBufSize(net_handle_t hd, uint32_t send_size)
{
	int ret = setsockopt(hd, SOL_SOCKET, SO_SNDBUF, &send_size, 4);
	if (ret == SOCKET_ERROR) {
		loge("set SO_SNDBUF failed for fd=%d", hd);
	}

	socklen_t len = 4;
	int size = 0;
	getsockopt(hd, SOL_SOCKET, SO_SNDBUF, &size, &len);
	loge("socket=%d send_buf_size=%d", hd, size);	
}

void _SetRecvBufSize(net_handle_t hd, uint32_t recv_size)
{
	int ret = setsockopt(hd, SOL_SOCKET, SO_RCVBUF, &recv_size, 4);
	if (ret == SOCKET_ERROR) {
		loge("set SO_RCVBUF failed for fd=%d", hd);
	}

	socklen_t len = 4;
	int size = 0;
	getsockopt(hd, SOL_SOCKET, SO_RCVBUF, &size, &len);
	loge("socket=%d recv_buf_size=%d", hd, size);	
}

int netlib_option(net_handle_t handle, int opt, void* optval)
{
	static callback_t cb;
	
	if ((opt >= NETLIB_OPT_GET_REMOTE_IP) && !optval)
		return NETLIB_ERROR;
		
	switch (opt) {
		case NETLIB_OPT_SET_CALLBACK:
			cb = (callback_t)optval;
			break;
		case NETLIB_OPT_SET_CALLBACK_DATA:
			netlib_set_onconnect_event(handle, cb, optval);
			break;
		case NETLIB_OPT_GET_REMOTE_IP:
			*(string*)optval = _GetRemoteIP(handle);
			break;
		case NETLIB_OPT_GET_REMOTE_PORT:
			*(uint16_t*)optval = _GetRemotePort(handle);
			break;
		case NETLIB_OPT_GET_LOCAL_IP:
			*(string*)optval = _GetLocalIP(handle);
			break;
		case NETLIB_OPT_GET_LOCAL_PORT:
			*(uint16_t*)optval = _GetLocalPort(handle);
			break;
		case NETLIB_OPT_SET_SEND_BUF_SIZE:
			_SetSendBufSize(handle, *(uint32_t*)optval);
			break;
		case NETLIB_OPT_SET_RECV_BUF_SIZE:
			_SetRecvBufSize(handle, *(uint32_t*)optval);
			break;
		default:
			break;
	}
	return NETLIB_OK;
}

int netlib_send(net_handle_t handle, void* buf, int len)
{
	return send(handle, (char*)buf, len, 0);
}

int netlib_recv(net_handle_t handle, void* buf, int len)
{
	return recv(handle, (char*)buf, len, 0);
}


int netlib_register_timer(callback_t callback, void* user_data, uint64_t interval)
{
	long int sec = interval/1000L;
	long int usec = interval%1000L;
	struct timeval t = {sec, usec};
	auto arg = new EvtArg(callback, user_data);
	struct event* ev = event_new(g_libevent_base, -1, EV_PERSIST, netlib_ontimer, arg);
	event_add(ev, &t);
	g_timer_map[callback] = ev;
	return 0;
}

int netlib_delete_timer(callback_t callback, void* user_data)
{
	auto it = g_timer_map.find(callback);
	if (it != g_timer_map.end()) {
		auto ev = it->second;
		g_timer_map.erase(it);
		event_free(ev);
	}
	return 0;
}

void netlib_ontimer(evutil_socket_t fd, short what, void* arg)
{
	EvtArg* evtArg = (EvtArg*)arg;
	evtArg->callback(evtArg->cbdata, NETLIB_MSG_TIMER, 0, NULL);
}

int netlib_add_loop(callback_t callback, void* user_data)
{
	struct timeval t = {0, 100};
	auto arg = new EvtArg(callback, user_data);
	struct event* ev = event_new(g_libevent_base, -1, EV_PERSIST, netlib_ontimer, arg);
	event_add(ev, &t);
	// g_timer_map[callback] = ev;
	return 0;
}

void netlib_eventloop(uint32_t wait_timeout)
{
	event_base_dispatch(g_libevent_base);
}

void netlib_stop_event()
{
    event_base_loopbreak(g_libevent_base);
}

bool netlib_is_running()
{
    bool ret = !event_base_got_break(g_libevent_base);
    return ret;
}

void netlib_redis_attach(redisAsyncContext *context)
{
	redisLibeventAttach(context, g_libevent_base);
}

#endif
