/*
 * imconn.cpp
 *
 *  Created on: 2013-6-5
 *      Author: ziteng@mogujie.com
 */

#include "imconn.h"

//static uint64_t g_send_pkt_cnt = 0;		// 发送数据包总数
//static uint64_t g_recv_pkt_cnt = 0;		// 接收数据包总数
#include "Lock.h"
static CLock s_send_lock;

static sp_CImConn FindImConnSp(ConnMap_sp_t* imconn_map_sp, net_handle_t handle)
{
	sp_CImConn spConn;
	auto iter = imconn_map_sp->find(handle);
	if (iter != imconn_map_sp->end()) {
		spConn = iter->second;
		return spConn;
	} else {
		return nullptr;
	}
}

void imconn_callback_sp(void* callback_data, uint8_t msg, uint32_t handle, void* pParam)
{
	NOTUSED_ARG(handle);
	NOTUSED_ARG(pParam);
	auto imconn_map_sp = (ConnMap_sp_t*)callback_data;
	
	auto pConn = FindImConnSp(imconn_map_sp, handle);
	if (!pConn)
		return;

	// log("msg=%d, handle=%d ", msg, handle);

	switch (msg)
	{
	case NETLIB_MSG_CONFIRM:
		pConn->OnConfirm();
		break;
	case NETLIB_MSG_READ:
		pConn->OnRead();
		break;
	case NETLIB_MSG_WRITE:
		pConn->OnWrite();
		break;
	case NETLIB_MSG_CLOSE:
		pConn->OnClose();
		break;
	default:
		loge("!!!imconn_callback error msg: %d ", msg);
		break;
	}
}


static CImConn* FindImConn(ConnMap_t* imconn_map, net_handle_t handle)
{
	CImConn* pConn = NULL;
	ConnMap_t::iterator iter = imconn_map->find(handle);
	if (iter != imconn_map->end())
	{
		pConn = iter->second;
		pConn->AddRef();
	}

	return pConn;
}


void imconn_callback(void* callback_data, uint8_t msg, uint32_t handle, void* pParam)
{
	NOTUSED_ARG(handle);
	NOTUSED_ARG(pParam);

	if (!callback_data)
		return;

	ConnMap_t* conn_map = (ConnMap_t*)callback_data;
	CImConn* pConn = FindImConn(conn_map, handle);
	if (!pConn)
		return;

	// logt("msg=%d, handle=%d ", msg, handle);

	switch (msg)
	{
	case NETLIB_MSG_CONFIRM:
		pConn->OnConfirm();
		break;
	case NETLIB_MSG_READ:
		pConn->OnRead();
		break;
	case NETLIB_MSG_WRITE:
		pConn->OnWrite();
		break;
	case NETLIB_MSG_CLOSE:
		pConn->OnClose();
		break;
	default:
		log("!!!imconn_callback error msg: %d ", msg);
		break;
	}
	pConn->ReleaseRef();
}

//////////////////////////
CImConn::CImConn()
{
	//log("CImConn::CImConn ");

	m_busy = false;
	m_handle = NETLIB_INVALID_HANDLE;
	m_recv_bytes = 0;

	m_last_send_tick = m_last_recv_tick = get_tick_count();
}

CImConn::~CImConn()
{
	//log("CImConn::~CImConn, handle=%d ", m_handle);
}

int CImConn::Send(void* data, int len)
{
	// CAutoLock autoLock(&s_send_lock);
	m_last_send_tick = get_tick_count();
//	++g_send_pkt_cnt;

	if (m_busy)
	{
		m_out_buf.Write(data, len);
		return len;
	}

	int offset = 0;
	int remain = len;
	while (remain > 0) {
		int send_size = remain;
		if (send_size > NETLIB_MAX_SOCKET_BUF_SIZE) {
			send_size = NETLIB_MAX_SOCKET_BUF_SIZE;
		}

		int ret = netlib_send(m_handle, (char*)data + offset , send_size);
		if (ret <= 0) {
			if (errno == EAGAIN || errno == EWOULDBLOCK) {
				ret = 0;
			} else {
				log("close on error=%d", errno);
				OnClose();
				return -1;
			}
		}

		offset += ret;
		remain -= ret;
	}

	if (remain > 0)
	{
		m_out_buf.Write((char*)data + offset, remain);
		m_busy = true;
		log("send busy, remain=%d ", m_out_buf.GetWriteOffset());
	}
    else
    {
        OnWriteCompelete();
    }

	return len;
}

void CImConn::OnRead()
{
	for (;;)
	{
		uint32_t free_buf_len = m_in_buf.GetAllocSize() - m_in_buf.GetWriteOffset();
		if (free_buf_len < READ_BUF_SIZE)
			m_in_buf.Extend(READ_BUF_SIZE);

		int ret = netlib_recv(m_handle, m_in_buf.GetBuffer() + m_in_buf.GetWriteOffset(), READ_BUF_SIZE);
		if (ret == 0) {
			log("close on netlib_recv=0");
			OnClose();
			return;
		} else if (ret < 0) {
			if (errno == EAGAIN || errno == EWOULDBLOCK) {
				break;
			} else {
				log("close on error=%d", errno);
				OnClose();
				return;
			}
		}

		m_recv_bytes += ret;
		m_in_buf.IncWriteOffset(ret);

		m_last_recv_tick = get_tick_count();
	}

    CImPdu* pPdu = NULL;
	try
    {
		while ( ( pPdu = CImPdu::ReadPdu(m_in_buf.GetBuffer(), m_in_buf.GetWriteOffset()) ) )
		{
            uint32_t pdu_len = pPdu->GetLength();
            
			HandlePdu(pPdu);

			m_in_buf.Read(NULL, pdu_len);
			delete pPdu;
            pPdu = NULL;
//			++g_recv_pkt_cnt;
		}
	} catch (CPduException& ex) {
		log("!!!catch exception, sid=%u, cid=%u, err_code=%u, err_msg=%s, close the connection ",
				ex.GetServiceId(), ex.GetCommandId(), ex.GetErrorCode(), ex.GetErrorMsg());
        if (pPdu) {
            delete pPdu;
            pPdu = NULL;
        }
        OnClose();
	}
}

void CImConn::OnWrite()
{
	// CAutoLock autoLock(&s_send_lock);
	if (!m_busy)
		return;

	while (m_out_buf.GetWriteOffset() > 0) {
		int send_size = m_out_buf.GetWriteOffset();
		if (send_size > NETLIB_MAX_SOCKET_BUF_SIZE) {
			send_size = NETLIB_MAX_SOCKET_BUF_SIZE;
		}

		int ret = netlib_send(m_handle, m_out_buf.GetBuffer(), send_size);
		if (ret <= 0) {
			if (errno == EAGAIN || errno == EWOULDBLOCK) {
				break;
			} else {
				log("close on error=%d", errno);
				OnClose();
				return;
			}
		}

		m_out_buf.Read(NULL, ret);
	}

	if (m_out_buf.GetWriteOffset() == 0) {
		m_busy = false;
	}

	log("onWrite, remain=%d ", m_out_buf.GetWriteOffset());
}



