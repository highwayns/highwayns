/*
 * login_server.cpp
 *
 *  Created on: 2013-6-21
 *      Author: ziteng@mogujie.com
 */

#include <unistd.h>

#include "LoginConn.h"
#include "netlib.h"
#include "ConfigFileReader.h"
#include "version.h"
#include "HttpConn.h"
#include "ipparser.h"

IpParser* pIpParser = NULL;
string strMsfsUrl;
string strDiscovery;//发现获取地址
void client_callback(void* callback_data, uint8_t msg, uint32_t handle, void* pParam)
{
	if (msg == NETLIB_MSG_CONNECT)
	{
		auto pConn = shared_ptr<CLoginConn>(new CLoginConn());
		pConn->OnConnect2(handle, LOGIN_CONN_TYPE_CLIENT);
	}
	else
	{
		log("!!!error msg: %d ", msg);
	}
}

// this callback will be replaced by imconn_callback() in OnConnect()
void msg_serv_callback(void* callback_data, uint8_t msg, uint32_t handle, void* pParam)
{
	if (msg == NETLIB_MSG_CONNECT)
	{
		auto pConn = shared_ptr<CLoginConn>(new CLoginConn());
		pConn->OnConnect2(handle, LOGIN_CONN_TYPE_MSG_SERV);
	}
	else
	{
		log("!!!error msg: %d ", msg);
	}

}


void http_callback(void* callback_data, uint8_t msg, uint32_t handle, void* pParam)
{
    if (msg == NETLIB_MSG_CONNECT)
    {
        auto pConn = shared_ptr<CHttpConn>(new CHttpConn());
        pConn->OnConnect(handle);
    }
    else
    {
        log("!!!error msg: %d ", msg);
    }
}

void cxx_handler(int sig)
{
  print_stacktrace();
  exit(1);
}

int main(int argc, char* argv[])
{
	backup_core_file();
	will_be_daemon(argc, argv);
	// will_have_stacktrace();
	
	if ((argc == 2) && (strcmp(argv[1], "-v") == 0)) {
		printf("Server Version: LoginServer/%s\n", VERSION);
		printf("Server Build: %s %s\n", __DATE__, __TIME__);
		return 0;
	}
	// signal(SIGSEGV, cxx_handler);   // install our handler
	signal(SIGPIPE, SIG_IGN);

	CConfigFileReader config_file("loginserver.conf");

    char* http_listen_ip = config_file.GetConfigName("HttpListenIP");
    char* str_http_port = config_file.GetConfigName("HttpPort");
	char* msg_server_listen_ip = config_file.GetConfigName("MsgServerListenIP");
	char* str_msg_server_port = config_file.GetConfigName("MsgServerPort");
    char* str_msfs_url = config_file.GetConfigName("msfs");
    char* str_discovery = config_file.GetConfigName("discovery");

	if (!msg_server_listen_ip || !str_msg_server_port || !http_listen_ip
        || !str_http_port || !str_msfs_url || !str_discovery) {
		log("config item missing, exit... ");
		return -1;
	}

	uint16_t msg_server_port = atoi(str_msg_server_port);
    uint16_t http_port = atoi(str_http_port);
    strMsfsUrl = str_msfs_url;
    strDiscovery = str_discovery;
    
    
    pIpParser = new IpParser();
    
	int ret = netlib_init();

	if (ret == NETLIB_ERROR)
		return ret;

	CStrExplode msg_server_listen_ip_list(msg_server_listen_ip, ';');
	for (uint32_t i = 0; i < msg_server_listen_ip_list.GetItemCnt(); i++) {
		ret = netlib_listen(msg_server_listen_ip_list.GetItem(i), msg_server_port, msg_serv_callback, NULL);
		if (ret == NETLIB_ERROR)
			return ret;
	}
    
    CStrExplode http_listen_ip_list(http_listen_ip, ';');
    for (uint32_t i = 0; i < http_listen_ip_list.GetItemCnt(); i++) {
        ret = netlib_listen(http_listen_ip_list.GetItem(i), http_port, http_callback, NULL);
        if (ret == NETLIB_ERROR)
            return ret;
    }
    
	init_login_conn();
    init_http_conn();

    writePid();
	netlib_eventloop();

	return 0;
}
