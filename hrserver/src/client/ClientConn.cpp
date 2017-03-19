/*================================================================
 *   Copyright (C) 2014 All rights reserved.
 *
 *   文件名称：ClentConn.cpp
 *   创 建 者：Zhang Yuanhao
 *   邮    箱：bluefoxah@gmail.com
 *   创建日期：2014年12月30日
 *   描    述：
 *
 ================================================================*/
#include "ClientConn.h"
#include "ServInfo.h"
#include "EncDec.h"
#include "ClientWorker.h"

using namespace std;

static ConnMap_sp_t s_client_conn_map;
static CServInfo<CClientConn>* s_serv_info_list;

void client_conn_timer_callback(void* callback_data, uint8_t msg, uint32_t handle, void* pParam)
{
    uint64_t cur_time = get_tick_count();
    for (auto it = s_client_conn_map.begin(); it != s_client_conn_map.end(); ) {
        auto it_old = it;
        it++;
        auto conn = it_old->second;
        conn->OnTimer(cur_time);
    }
}

CClientConn::CClientConn(string ip, uint16_t port) : m_bOpen(false), m_ip(ip), m_port(port)
{
    m_pSeqAlloctor = CSeqAlloctor::getInstance();
    m_handle = 0;
}

CClientConn::~CClientConn()
{
     logd("destruct clientconn"); 
}

void CClientConn::Register(string name, string passwd)
{
    RUNTIME_TRACE;
    m_username = name;
    m_passwd = passwd;
    m_state = CONN_STATE_REG;
	m_handle = netlib_connect(m_ip.c_str(), m_port, imconn_callback_sp, (void*)&s_client_conn_map);
	if (m_handle != NETLIB_INVALID_HANDLE) {
        s_client_conn_map.insert(make_pair(m_handle, shared_from_this()));
	} else {
	    throw netex("connect to %s:%d failed", m_ip.c_str(), m_port);
	}
}

void CClientConn::Login(string name, string passwd)
{
    m_username = name;
    m_passwd = passwd;
    m_state = CONN_STATE_LOGIN;
    m_handle = netlib_connect(m_ip.c_str(), m_port, imconn_callback_sp, (void*)&s_client_conn_map);
	if (m_handle != NETLIB_INVALID_HANDLE) {
        s_client_conn_map.insert(make_pair(m_handle, shared_from_this()));
	} else {
	    throw netex("connect to %s:%d failed", m_ip.c_str(), m_port);
	}
}

uint32_t CClientConn::OnRegister()
{
    RUNTIME_TRACE;
    CImPdu pdu;
    IM::Login::IMRegisterReq msg;
    msg.set_user_name(m_username);
    char md5passwd[33];
    CMd5::MD5_Calculate(m_passwd.c_str(), m_passwd.length(), md5passwd);
    msg.set_password(string(md5passwd));
    msg.set_online_status(IM::BaseDefine::USER_STATUS_ONLINE);
    msg.set_client_type(IM::BaseDefine::CLIENT_TYPE_WINDOWS);
    msg.set_client_version("1.0");
    
    IM::BaseDefine::UserInfo* pUser = msg.mutable_user_info();
    pUser->set_user_gender(0);
    pUser->set_department_id(1);
    pUser->set_user_nick_name(m_username);
    pUser->set_user_domain(m_username);
    pUser->set_avatar_url("xxx/ooo");
    pUser->set_email("test@test.com");
    pUser->set_user_tel("110");
    pUser->set_user_real_name(m_username);
    pUser->set_status(0);
    
    pdu.SetPBMsg(&msg);
    pdu.SetServiceId(IM::BaseDefine::SID_LOGIN);
    pdu.SetCommandId(IM::BaseDefine::CID_LOGIN_REQ_USERREG);
    uint32_t nSeqNo = m_pSeqAlloctor->getSeq(ALLOCTOR_PACKET);
    pdu.SetSeqNum(nSeqNo);
    SendPdu(&pdu);
    return nSeqNo;
}

void CClientConn::OnConfirm()
{
    // log("%s client on confirm ", typeid(*this).name());
    switch (m_state) {
        case CONN_STATE_LOGIN:
            OnLogin();
            break;
        case CONN_STATE_REG:
            OnRegister();
            break;
    }
}

void CClientConn::OnClose()
{
    log("onclose from handle=%d\n", m_handle);
    Close();
}

void CClientConn::OnWrite()
{
    // log("in conn on write");
}

void CClientConn::OnTimer(uint64_t curr_tick)
{
    if (curr_tick > m_last_send_tick + CLIENT_HEARTBEAT_INTERVAL) {
        CImPdu cPdu;
        IM::Other::IMHeartBeat msg;
        cPdu.SetPBMsg(&msg);
        cPdu.SetServiceId(IM::BaseDefine::SID_OTHER);
        cPdu.SetCommandId(IM::BaseDefine::CID_OTHER_HEARTBEAT);
        uint32_t nSeqNo = m_pSeqAlloctor->getSeq(ALLOCTOR_PACKET);
        cPdu.SetSeqNum(nSeqNo);
        SendPdu(&cPdu);
    }
    
    if (curr_tick > m_last_recv_tick + CLIENT_TIMEOUT) {
        log("conn to msg_server timeout\n");
        Close();
    }
}

uint32_t CClientConn::OnLogin()
{
    CImPdu cPdu;
    IM::Login::IMLoginReq msg;
    msg.set_user_name(m_username);
    char md5passwd[33];
    CMd5::MD5_Calculate(m_passwd.c_str(), m_passwd.length(), md5passwd);
    msg.set_password(string(md5passwd));
    msg.set_online_status(IM::BaseDefine::USER_STATUS_ONLINE);
    msg.set_client_type(IM::BaseDefine::CLIENT_TYPE_WINDOWS);
    msg.set_client_version("1.0");
    cPdu.SetPBMsg(&msg);
    cPdu.SetServiceId(IM::BaseDefine::SID_LOGIN);
    cPdu.SetCommandId(IM::BaseDefine::CID_LOGIN_REQ_USERLOGIN);
    uint32_t nSeqNo = m_pSeqAlloctor->getSeq(ALLOCTOR_PACKET);
    cPdu.SetSeqNum(nSeqNo);
    SendPdu(&cPdu);
    return nSeqNo;
}

uint32_t CClientConn::getUser(uint32_t nUserId, uint32_t nTime)
{
    CImPdu cPdu;
    IM::Buddy::IMAllUserReq msg;
    msg.set_user_id(nUserId);
    msg.set_latest_update_time(nTime);
    cPdu.SetPBMsg(&msg);
    cPdu.SetServiceId(IM::BaseDefine::SID_BUDDY_LIST);
    cPdu.SetCommandId(IM::BaseDefine::CID_BUDDY_LIST_ALL_USER_REQUEST);
    uint32_t nSeqNo = m_pSeqAlloctor->getSeq(ALLOCTOR_PACKET);
    cPdu.SetSeqNum(nSeqNo);
    SendPdu(&cPdu);
    return nSeqNo;
}

uint32_t CClientConn::getUserInfo(uint32_t nUserId, list<uint32_t>& lsUserId)
{
    CImPdu cPdu;
    IM::Buddy::IMUsersInfoReq msg;
    msg.set_user_id(nUserId);
    for (auto it=lsUserId.begin(); it!=lsUserId.end(); ++it) {
        msg.add_user_id_list(*it);
    }
    cPdu.SetPBMsg(&msg);
    cPdu.SetServiceId(IM::BaseDefine::SID_BUDDY_LIST);
    cPdu.SetCommandId(IM::BaseDefine::CID_BUDDY_LIST_USER_INFO_REQUEST);
    uint32_t nSeqNo = m_pSeqAlloctor->getSeq(ALLOCTOR_PACKET);
    cPdu.SetSeqNum(nSeqNo);
    SendPdu(&cPdu);
    return nSeqNo;
}

uint32_t CClientConn::sendMessage(uint32_t nFromId, uint32_t nToId, IM::BaseDefine::MsgType nType, const string& strMsgData)
{
    CImPdu cPdu;
    IM::Message::IMMsgData msg;
    msg.set_from_user_id(nFromId);
    msg.set_to_session_id(nToId);
    msg.set_msg_id(0);
    msg.set_create_time(time(NULL));
    msg.set_msg_type(nType);
    msg.set_msg_data(strMsgData);
    cPdu.SetPBMsg(&msg);
    cPdu.SetServiceId(IM::BaseDefine::SID_MSG);
    cPdu.SetCommandId(IM::BaseDefine::CID_MSG_DATA);
    uint32_t nSeqNo = m_pSeqAlloctor->getSeq(ALLOCTOR_PACKET);
    cPdu.SetSeqNum(nSeqNo);
    SendPdu(&cPdu);
    return nSeqNo;
}

uint32_t CClientConn::getUnreadMsgCnt(uint32_t nUserId)
{
    CImPdu cPdu;
    IM::Message::IMUnreadMsgCntReq msg;
    msg.set_user_id(nUserId);
    cPdu.SetPBMsg(&msg);
    cPdu.SetServiceId(IM::BaseDefine::SID_MSG);
    cPdu.SetCommandId(IM::BaseDefine::CID_MSG_UNREAD_CNT_REQUEST);
    uint32_t nSeqNo = m_pSeqAlloctor->getSeq(ALLOCTOR_PACKET);
    cPdu.SetSeqNum(nSeqNo);
    SendPdu(&cPdu);
    return nSeqNo;
}


uint32_t CClientConn::getRecentSession(uint32_t nUserId, uint32_t nLastTime)
{
    CImPdu cPdu;
    IM::Buddy::IMRecentContactSessionReq msg;
    msg.set_user_id(nUserId);
    msg.set_latest_update_time(nLastTime);
    cPdu.SetPBMsg(&msg);
    cPdu.SetServiceId(IM::BaseDefine::SID_BUDDY_LIST);
    cPdu.SetCommandId(IM::BaseDefine::CID_BUDDY_LIST_RECENT_CONTACT_SESSION_REQUEST);
    uint32_t nSeqNo = m_pSeqAlloctor->getSeq(ALLOCTOR_PACKET);
    cPdu.SetSeqNum(nSeqNo);
    SendPdu(&cPdu);
    return nSeqNo;
}

uint32_t CClientConn::getMsgList(uint32_t nUserId, IM::BaseDefine::SessionType nType, uint32_t nPeerId, uint32_t nMsgId, uint32_t nMsgCnt)
{
    CImPdu cPdu;
    IM::Message::IMGetMsgListReq msg;
    msg.set_user_id(nUserId);
    msg.set_session_type(nType);
    msg.set_session_id(nPeerId);
    msg.set_msg_id_begin(nMsgId);
    msg.set_msg_cnt(nMsgCnt);
    cPdu.SetPBMsg(&msg);
    cPdu.SetServiceId(IM::BaseDefine::SID_MSG);
    cPdu.SetCommandId(IM::BaseDefine::CID_MSG_LIST_REQUEST);
    uint32_t nSeqNo = m_pSeqAlloctor->getSeq(ALLOCTOR_PACKET);
    cPdu.SetSeqNum(nSeqNo);
    SendPdu(&cPdu);
    return nSeqNo;
}

uint32_t CClientConn::sendMsgAck(uint32_t nUserId, uint32_t nPeerId, IM::BaseDefine::SessionType nType, uint32_t nMsgId)
{
    CImPdu cPdu;
    IM::Message::IMMsgDataReadAck msg;
    msg.set_user_id(nUserId);
    msg.set_session_id(nPeerId);
    msg.set_session_type(nType);
    msg.set_msg_id(nMsgId);
    cPdu.SetPBMsg(&msg);
    cPdu.SetServiceId(IM::BaseDefine::SID_MSG);
    cPdu.SetCommandId(IM::BaseDefine::CID_MSG_READ_ACK);
    uint32_t nSeqNo = m_pSeqAlloctor->getSeq(ALLOCTOR_PACKET);
    cPdu.SetSeqNum(nSeqNo);
    SendPdu(&cPdu);
    return nSeqNo;
}

void CClientConn::Close()
{
	if (m_handle != NETLIB_INVALID_HANDLE) {
	    log("close conn handle=%d", m_handle);
		netlib_close(m_handle);
        s_client_conn_map.erase(m_handle);
	}
}

void CClientConn::HandlePdu(CImPdu* pPdu)
{
    log("pdu %d", pPdu->GetCommandId());
	switch (pPdu->GetCommandId()) {
        case IM::BaseDefine::CID_OTHER_HEARTBEAT:
            _HandleHeartBeat(pPdu);
    		break;
    	case IM::BaseDefine::CID_LOGIN_RES_USERREG:
    	    _HandleUserRegResponse(pPdu);
    	    break;
        case IM::BaseDefine::CID_LOGIN_RES_USERLOGIN:
            _HandleLoginResponse(pPdu);
    		break; 
        case IM::BaseDefine::CID_BUDDY_LIST_ALL_USER_RESPONSE:
            _HandleUser(pPdu);
            break;
        case IM::BaseDefine::CID_BUDDY_LIST_USER_INFO_RESPONSE:
            _HandleUserInfo(pPdu);
            break;
        case IM::BaseDefine::CID_MSG_DATA_ACK:
            _HandleSendMsg(pPdu);
            break;
        case IM::BaseDefine::CID_MSG_UNREAD_CNT_RESPONSE:
            _HandleUnreadCnt(pPdu);
            break;
        case IM::BaseDefine::CID_BUDDY_LIST_RECENT_CONTACT_SESSION_RESPONSE:
            _HandleRecentSession(pPdu);
            break;
        case IM::BaseDefine::CID_MSG_LIST_RESPONSE:
            _HandleMsgList(pPdu);
            break;
        case IM::BaseDefine::CID_MSG_DATA:
            _HandleMsgData(pPdu);
            break;
        default:
		log("wrong msg_type=%d\n", pPdu->GetCommandId());
		break;
	}
}

void CClientConn::_HandleHeartBeat(CImPdu* pPdu)
{
    log("server heartbeat response");
}

void CClientConn::_HandleUserRegResponse(CImPdu* pPdu)
{
    IM::Login::IMRegisterRes msg;
    CHECK_PB_PARSE_MSG(msg.ParseFromArray(pPdu->GetBodyData(), pPdu->GetBodyLength()));
    string name = msg.user_name();
    uint32_t ret = msg.result_code();
    string strRet = msg.result_string();
    if (ret) {
        loge("Register username=%s failed errno=%d, errinfo=%s",
            name.c_str(), ret, strRet.c_str());
        return;
    }
}

void CClientConn::_HandleLoginResponse(CImPdu* pPdu)
{
    IM::Login::IMLoginRes msgResp;
    uint32_t nSeqNo = pPdu->GetSeqNum();
    CHECK_PB_PARSE_MSG(msgResp.ParseFromArray(pPdu->GetBodyData(), pPdu->GetBodyLength()));
    uint32_t ret = msgResp.result_code();
    string strRet = msgResp.result_string();
    if (ret) {
        loge("login username=%s failed errno=%d, errinfo=%s",
            m_username.c_str(), ret, strRet.c_str());
        return;
    }
    m_bOpen = true;
    IM::BaseDefine::UserInfo cUser = msgResp.user_info();
    log("%s login successful", m_username.c_str());
}

void CClientConn::_HandleUser(CImPdu* pPdu)
{
    IM::Buddy::IMAllUserRsp msgResp;
    uint32_t nSeqNo = pPdu->GetSeqNum();
    if(msgResp.ParseFromArray(pPdu->GetBodyData(), pPdu->GetBodyLength()))
    {
        uint32_t userCnt = msgResp.user_list_size();
        printf("get %d users\n", userCnt);
        list<IM::BaseDefine::UserInfo> lsUsers;
        for(uint32_t i=0; i<userCnt; ++i)
        {
            IM::BaseDefine::UserInfo cUserInfo = msgResp.user_list(i);
            lsUsers.push_back(cUserInfo);
        }
        m_pCallback->onGetChangedUser(nSeqNo, lsUsers);
    }
    else
    {
        m_pCallback->onError(nSeqNo, pPdu->GetCommandId(), "parse pb error");
    }
}

void CClientConn::_HandleUserInfo(CImPdu* pPdu)
{
    IM::Buddy::IMUsersInfoRsp msgResp;
    uint32_t nSeqNo = pPdu->GetSeqNum();
    if(msgResp.ParseFromArray(pPdu->GetBodyData(), pPdu->GetBodyLength()))
    {
        uint32_t userCnt = msgResp.user_info_list_size();
        list<IM::BaseDefine::UserInfo> lsUser;
        for (uint32_t i=0; i<userCnt; ++i) {
            IM::BaseDefine::UserInfo userInfo = msgResp.user_info_list(i);
            lsUser.push_back(userInfo);
        }
        m_pCallback->onGetUserInfo(nSeqNo, lsUser);
    }
    else
    {
        m_pCallback->onError(nSeqNo, pPdu->GetCommandId(), "parse pb error");
    }
}

void CClientConn::_HandleSendMsg(CImPdu* pPdu)
{
    IM::Message::IMMsgDataAck msgResp;
    uint32_t nSeqNo = pPdu->GetSeqNum();
    if(msgResp.ParseFromArray(pPdu->GetBodyData(), pPdu->GetBodyLength()))
    {
        uint32_t nSendId = msgResp.user_id();
        uint32_t nRecvId = msgResp.session_id();
        uint32_t nMsgId = msgResp.msg_id();
        IM::BaseDefine::SessionType nType = msgResp.session_type();
        m_pCallback->onSendMsg(nSeqNo, nSendId, nRecvId, nType, nMsgId);
    }
    else
    {
        m_pCallback->onError(nSeqNo, pPdu->GetCommandId(), "parse pb error");
    }
}


void CClientConn::_HandleUnreadCnt(CImPdu* pPdu)
{
    IM::Message::IMUnreadMsgCntRsp msgResp;
    uint32_t nSeqNo = pPdu->GetSeqNum();
    if(msgResp.ParseFromArray(pPdu->GetBodyData(), pPdu->GetBodyLength()))
    {
        list<IM::BaseDefine::UnreadInfo> lsUnreadInfo;
        uint32_t nUserId = msgResp.user_id();
        uint32_t nTotalCnt = msgResp.total_cnt();
        uint32_t nCnt = msgResp.unreadinfo_list_size();
        for (uint32_t i=0; i<nCnt; ++i) {
            IM::BaseDefine::UnreadInfo unreadInfo = msgResp.unreadinfo_list(i);
            lsUnreadInfo.push_back(unreadInfo);
        }
        m_pCallback->onGetUnreadMsgCnt(nSeqNo, nUserId, nTotalCnt, lsUnreadInfo);
    }
    else
    {
        m_pCallback->onError(nSeqNo, pPdu->GetCommandId(), "parse pb fail");
    }
}

void CClientConn::_HandleRecentSession(CImPdu *pPdu)
{
    IM::Buddy::IMRecentContactSessionRsp msgResp;
    uint32_t nSeqNo = pPdu->GetSeqNum();
    if(msgResp.ParseFromArray(pPdu->GetBodyData(), pPdu->GetBodyLength()))
    {
        list<IM::BaseDefine::ContactSessionInfo> lsSession;
        uint32_t nUserId = msgResp.user_id();
        uint32_t nCnt = msgResp.contact_session_list_size();
        for (uint32_t i=0; i<nCnt; ++i) {
            IM::BaseDefine::ContactSessionInfo session = msgResp.contact_session_list(i);
            lsSession.push_back(session);
        }
        m_pCallback->onGetRecentSession(nSeqNo, nUserId, lsSession);
    }
    else
    {
        m_pCallback->onError(nSeqNo, pPdu->GetCommandId(), "parse pb error");
    }
}

void CClientConn::_HandleMsgList(CImPdu *pPdu)
{
    IM::Message::IMGetMsgListRsp msgResp;
    uint32_t nSeqNo = pPdu->GetSeqNum();
    if(msgResp.ParseFromArray(pPdu->GetBodyData(), pPdu->GetBodyLength()))
    {
        uint32_t nUserId= msgResp.user_id();
        IM::BaseDefine::SessionType nSessionType = msgResp.session_type();
        uint32_t nPeerId = msgResp.session_id();
        uint32_t nMsgId = msgResp.msg_id_begin();
        uint32_t nMsgCnt = msgResp.msg_list_size();
        list<IM::BaseDefine::MsgInfo> lsMsg;
        for(uint32_t i=0; i<nMsgCnt; ++i)
        {
            IM::BaseDefine::MsgInfo msgInfo = msgResp.msg_list(i);
            lsMsg.push_back(msgInfo);
        }
        m_pCallback->onGetMsgList(nSeqNo, nUserId, nPeerId, nSessionType, nMsgId, nMsgCnt, lsMsg);
    }
    else
    {
        m_pCallback->onError(nSeqNo, pPdu->GetCommandId(), "parse pb falied");
    }
}
void CClientConn::_HandleMsgData(CImPdu* pPdu)
{
    IM::Message::IMMsgData msg;
    uint32_t nSeqNo = pPdu->GetSeqNum();
    if(msg.ParseFromArray(pPdu->GetBodyData(), pPdu->GetBodyLength()))
    {
        // play("message.wav");
        
        uint32_t nFromId = msg.from_user_id();
        uint32_t nToId = msg.to_session_id();
        uint32_t nMsgId = msg.msg_id();
        IM::BaseDefine::MsgType nMsgType = msg.msg_type();
        uint32_t nCreateTime = msg.create_time();
        string strMsg(msg.msg_data().c_str(), msg.msg_data().length());
        
        IM::BaseDefine::SessionType nSessionType;
        if(nMsgType == IM::BaseDefine::MSG_TYPE_SINGLE_TEXT)
        {
            nSessionType = IM::BaseDefine::SESSION_TYPE_SINGLE;
        }
        else if(nMsgType == IM::BaseDefine::MSG_TYPE_SINGLE_AUDIO)
        {
            nSessionType = IM::BaseDefine::SESSION_TYPE_SINGLE;
        }
        else if(nMsgType == IM::BaseDefine::MSG_TYPE_GROUP_TEXT)
        {
            nSessionType = IM::BaseDefine::SESSION_TYPE_GROUP;
        }
        else if(nMsgType == IM::BaseDefine::MSG_TYPE_GROUP_AUDIO)
        {
            nSessionType = IM::BaseDefine::SESSION_TYPE_GROUP;
        }
        sendMsgAck(nFromId, nToId, nSessionType, nMsgId);
        m_pCallback->onRecvMsg(nSeqNo, nFromId, nToId, nMsgId, nCreateTime, nMsgType, strMsg);
    }
    else
    {
        m_pCallback->onError(nSeqNo, pPdu->GetCommandId(), "parse pb falied");
    }
}
