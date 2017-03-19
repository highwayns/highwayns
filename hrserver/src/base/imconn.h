/*
 * imconn.h
 *
 *  Created on: 2013-6-5
 *      Author: ziteng
 */

#ifndef IMCONN_H_
#define IMCONN_H_

#include <unordered_map>
#include <memory>

#include "netlib.h"
#include "util.h"
#include "ImPduBase.h"

#define SERVER_HEARTBEAT_INTERVAL	5000
#define SERVER_TIMEOUT				30000
#define CLIENT_HEARTBEAT_INTERVAL	28000
#define CLIENT_TIMEOUT				120000
#define MOBILE_CLIENT_TIMEOUT       60000 * 5
// #define MOBILE_CLIENT_TIMEOUT       60 * 1
#define READ_BUF_SIZE	2048

class CImConn : public CRefObject, public enable_shared_from_this<CImConn>
{
public:
	CImConn();
	virtual ~CImConn();

	int Send(void* data, int len);
	virtual void OnRead();
	virtual void OnWrite();
	
	bool IsBusy() { return m_busy; }
	int SendPdu(CImPdu* pPdu) { return Send(pPdu->GetBuffer(), pPdu->GetLength()); }

	virtual void OnConnect(net_handle_t handle) { m_handle = handle; }
	virtual void OnConfirm(){}
	virtual void OnClose(){}
	virtual void OnTimer(uint64_t){}
    virtual void OnWriteCompelete(){}
	virtual void HandlePdu(CImPdu*){}
	
	virtual void SetConnName(std::string name) { m_conn_name = name; }
	virtual string& GetConnName() { return m_conn_name; }

protected:
	std::string     m_conn_name;
	net_handle_t	m_handle;
	bool			m_busy;

	string			m_peer_ip;
	uint16_t		m_peer_port;
	CSimpleBuffer	m_in_buf;
	CSimpleBuffer	m_out_buf;

	bool			m_policy_conn;
	uint32_t		m_recv_bytes;
	uint64_t		m_last_send_tick;
	uint64_t		m_last_recv_tick;
    uint64_t        m_last_all_user_tick;
};

typedef hash_map<net_handle_t, CImConn*> ConnMap_t;
typedef hash_map<uint32_t, CImConn*> UserMap_t;

using sp_CImConn = shared_ptr<CImConn>;
using ConnMap_sp_t = unordered_map<net_handle_t, sp_CImConn>;
using UserMap_sp_t = unordered_map<uint32_t, sp_CImConn>;

void imconn_callback_sp(void* callback_data, uint8_t msg, uint32_t handle, void* pParam);

void imconn_callback(void* callback_data, uint8_t msg, uint32_t handle, void* pParam);
void ReadPolicyFile();

#endif /* IMCONN_H_ */
