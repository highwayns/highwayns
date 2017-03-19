/*================================================================
 *   Copyright (C) 2014 All rights reserved.
 *
 *   文件名称：ClientConn.h
 *   创 建 者：Zhang Yuanhao
 *   邮    箱：bluefoxah@gmail.com
 *   创建日期：2014年12月30日
 *   描    述：
 *
 ================================================================*/

#ifndef CLIENTCONN_H_
#define CLIENTCONN_H_

#include <iostream>
#include "SeqAlloctor.h"
#include "imconn.h"
#include "ImPduBase.h"


#include "IM.BaseDefine.pb.h"
#include "IM.Login.pb.h"
#include "IM.Other.pb.h"
#include "IM.Server.pb.h"
#include "IM.Buddy.pb.h"
#include "IM.Message.pb.h"
#include "IM.Group.pb.h"

void client_conn_register(string username, string passwd);
void client_conn_timer_callback(void* callback_data, uint8_t msg, uint32_t handle, void* pParam);


class IPacketCallback
{
public:
    IPacketCallback() {};
    virtual ~IPacketCallback() {};
    virtual void onConnect() {}
    virtual void onClose() {}
    virtual void onError(uint32_t nSeqNo, uint32_t nCmd, const string& strMsg) {}
    virtual void onLogin(uint32_t nSeqNo, uint32_t nResultCode, string& strMsg, IM::BaseDefine::UserInfo* pUser = NULL) {}
    virtual void onGetChangedUser(uint32_t nSeqNo,const list<IM::BaseDefine::UserInfo>& lsUser) {}
    virtual void onGetUserInfo(uint32_t nSeqNo,const list<IM::BaseDefine::UserInfo>& lsUser) {}
    virtual void onSendMsg(uint32_t nSeqNo, uint32_t nUserId, uint32_t nRecvId, IM::BaseDefine::SessionType nType, uint32_t nMsgId) {}
    virtual void onGetUnreadMsgCnt(uint32_t nSeqNo, uint32_t nUserId, uint32_t nTotalCnt, const list<IM::BaseDefine::UnreadInfo>& lsUnreadCnt) {}
    virtual void onGetRecentSession(uint32_t nSeqNo, uint32_t nUserId, const list<IM::BaseDefine::ContactSessionInfo>& lsSession) {}
    virtual void onGetMsgList(uint32_t nSeqNo, uint32_t nUserId, uint32_t nPeerId, IM::BaseDefine::SessionType nSessionType, uint32_t nMsgId, uint32_t nMsgCnt, const list<IM::BaseDefine::MsgInfo>& lsMsg) {}
    virtual void onRecvMsg(uint32_t nSeqNo, uint32_t nFromId, uint32_t nToId, uint32_t nMsgId, uint32_t nCreateTime, IM::BaseDefine::MsgType nMsgType, const string& strMsgData) {}
};

typedef enum {
    CONN_STATE_REG = 1,
    CONN_STATE_LOGIN
}conn_state_t;

class CClientConn : public CImConn
{
public:
	CClientConn(string ip, uint16_t port);
	virtual ~CClientConn();

	bool IsOpen() { return m_bOpen; }

    virtual void Close();
public:
    
    uint32_t getUser(uint32_t nUserId, uint32_t nTime =0);
    uint32_t getUserInfo(uint32_t nUserId, list<uint32_t>& lsUserId);
    uint32_t sendMessage(uint32_t nFromId, uint32_t nToId, IM::BaseDefine::MsgType nType, const string& strMsgData);
    uint32_t getUnreadMsgCnt(uint32_t nUserId);
    uint32_t getRecentSession(uint32_t nUserId, uint32_t nLastTime);
    uint32_t getMsgList(uint32_t nUserId, IM::BaseDefine::SessionType nType, uint32_t nPeerId, uint32_t nMsgId, uint32_t nMsgCnt);
    uint32_t sendMsgAck(uint32_t nUserId, uint32_t nPeerId, IM::BaseDefine::SessionType nType, uint32_t nMsgId);
public:
	virtual void OnConfirm();
	virtual void OnClose();
	virtual void OnTimer(uint64_t curr_tick);
	void OnWrite() override;
	
	void Register(string name, string passwd);
	void Login(string name, string passwd);
	uint32_t OnRegister();
	uint32_t OnLogin();

	virtual void HandlePdu(CImPdu* pPdu);
private:
    void _HandleHeartBeat(CImPdu* pPdu);

    void _HandleUserRegResponse(CImPdu* pPdu);
	void _HandleLoginResponse(CImPdu* pPdu);
    void _HandleUser(CImPdu* pPdu);
    void _HandleUserInfo(CImPdu* pPdu);
    void _HandleSendMsg(CImPdu* pPdu);
    void _HandleUnreadCnt(CImPdu* pPdu);
    void _HandleRecentSession(CImPdu* pPdu);
    void _HandleMsgList(CImPdu* pPdu);
    void _HandleMsgData(CImPdu* pPdu);
    
private:
	bool 		m_bOpen;
    uint32_t m_serv_idx;
    CSeqAlloctor* m_pSeqAlloctor;
    IPacketCallback* m_pCallback;
    string m_ip;
    uint16_t m_port;
    string m_username;
    string m_passwd;
    conn_state_t m_state;
};


#endif /* CLIENTCONN_H_ */
