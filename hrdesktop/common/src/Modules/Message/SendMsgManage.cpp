/******************************************************************************* 
 *  @file      SendMsgManage.cpp 2014\8\7 13:52:48 $
 *  @author    ���<dafo@mogujie.com>
 *  @brief     
 ******************************************************************************/

#include "stdafx.h"
#include "SendMsgManage.h"
#include "Modules/ITcpClientModule.h"
#include "Modules/IMessageModule.h"
#include "Modules/ISysConfigModule.h"
#include "Modules/ISessionModule.h"
#include "Modules/IDatabaseModule.h"
#include "Modules/IMiscModule.h"
#include "utility/utilStrCodingAPI.h"
#include "ProtocolBuffer/IM.Message.pb.h"
#include "network/ImCore.h"
#include "network/TTPBHeader.h"

namespace
{
	const UInt8 TIMEOUT_SENDINGMSG = 5;	//���ڷ��͵���Ϣ����ʱΪ5S
	const UInt8 SENDMSG_RETRY_CNT = 1;	//
	const UInt32 SENDMSG_INTERVAL = 100;//ʱ����Ϊ100����
}

/******************************************************************************/

// -----------------------------------------------------------------------------
//  SendMsgManage: Public, Constructor

SendMsgManage::SendMsgManage()
{
	CheckSendMsgTimer* pCheckSendMsgTimer = new CheckSendMsgTimer(this);
	module::getEventManager()->scheduleTimer(pCheckSendMsgTimer, 1, TRUE);
	srand(static_cast<UInt32>(time(0)));
}

// -----------------------------------------------------------------------------
//  SendMsgManage: Public, Destructor

SendMsgManage::~SendMsgManage()
{

}

void SendMsgManage::pushSendingMsg(IN MessageEntity& msg)
{
	SendingMsg sendingMsg;
	CAutoLock autoLock(&m_lock);
	if (m_ListSendingMsg.empty())
	{
		sendingMsg.sendtime = static_cast<long>(clock());
		sendingMsg.msg = msg;
		sendingMsg.seqNo = _getSeqNo();
		sendingMsg.status = MSGSTATUS_SENDING;
		m_ListSendingMsg.push_back(sendingMsg);
		sendMessage(sendingMsg);
	}
	else
	{
		sendingMsg.sendtime = static_cast<long>(clock());
		sendingMsg.msg = msg;
		sendingMsg.seqNo = _getSeqNo();
		SendingMsg& BackMsg = m_ListSendingMsg.back();
		if (sendingMsg.sendtime - BackMsg.sendtime > SENDMSG_INTERVAL)
		{
			m_ListSendingMsg.push_back(sendingMsg);
			SendingMsgList::iterator itBegin = m_ListSendingMsg.begin();

			if (itBegin->status == MSGSTATUS_TOSEND)
			{
				itBegin->status = MSGSTATUS_SENDING;
				sendMessage(*itBegin);
			}
		}
		else
		{//����Ƶ��̫��
			module::getSessionModule()->asynNotifyObserver(module::KEY_SESSION_SENDMSG_TOOFAST, sendingMsg.msg.sessionId);//������Ϣ̫��
			return;
		}

	}

}
void SendMsgManage::getSendFailedMsgs(OUT SendingMsgList& FailedMsgList)
{
	CAutoLock autoLock(&m_lock);
	if (m_ListSendingMsg.empty())
	{
		return;
	}
	SendingMsg& itFront = m_ListSendingMsg.front();
	if (itFront.waitSeconds > TIMEOUT_SENDINGMSG
		&&itFront.retrySendCnt <= SENDMSG_RETRY_CNT)
	{
		std::copy(m_ListSendingMsg.begin(), m_ListSendingMsg.end(), std::back_inserter(FailedMsgList));
	}
	m_ListSendingMsg.clear();
}

SendMsgManage* SendMsgManage::getInstance()
{
	static SendMsgManage manager;
	return &manager;
}

UInt32 SendMsgManage::_getSeqNo(void)
{
	static UInt16 seqNo = static_cast<UInt16>(rand() % 1000);
	if (seqNo > (imcore::TTPBHEADER_RESERVED_MASK -1))
	{
		seqNo = static_cast<UInt16>(rand() % 1000);
	}
	return ++seqNo;
}

BOOL SendMsgManage::popUpSendingMsgByAck(IN const UInt16 seqNo, IN const UInt32 msgID)
{
	CAutoLock autoLock(&m_lock);
	//�ҵ���Ӧ��seqNo�Ļ�����erase��
	SendingMsgList::iterator iter = m_ListSendingMsg.begin();
	for (; iter != m_ListSendingMsg.end(); ++iter)
	{
		if (iter->seqNo == seqNo)
		{
			//����Ϣ���浽DB
			iter->msg.msgId = msgID;
			module::getDatabaseModule()->sqlInsertMessage(iter->msg);
			//���»Ự���ڴ��DB
			module::getSessionModule()->updateSessionEntityByMsg(iter->msg);
			
			SendingMsgList::iterator iterNext = m_ListSendingMsg.erase(iter);
			if (iterNext != m_ListSendingMsg.end() && iterNext->status == MSGSTATUS_TOSEND)//����һ��
			{
				iterNext->status = MSGSTATUS_SENDING;
				sendMessage(*iterNext);
			}

			return TRUE;
		}
	}
	return FALSE;
}

void SendMsgManage::clearSendMessageList()
{
	if (!m_ListSendingMsg.empty())
	{
		m_ListSendingMsg.clear();
	}
}

void SendMsgManage::sendMessage(IN SendingMsg& sendingMsg)
{
	imcore::IMLibCoreStartOperationWithLambda(
		[=]()mutable
	{
		IM::Message::IMMsgData imMsgData;
		imMsgData.set_from_user_id(module::getSysConfigModule()->userId());
		std::string sToId = sendingMsg.msg.getOriginSessionId();
		imMsgData.set_to_session_id(util::stringToInt32(sToId));
		imMsgData.set_msg_id(0);//���͵�ʱ��û����Ϣid����0����
		imMsgData.set_create_time(sendingMsg.msg.msgTime);
		imMsgData.set_msg_type(static_cast<IM::BaseDefine::MsgType>(sendingMsg.msg.msgType));
		imMsgData.set_msg_data(sendingMsg.msg.content);
		module::getTcpClientModule()->sendPacket(IM::BaseDefine::ServiceID::SID_MSG
			, IM::BaseDefine::MessageCmdID::CID_MSG_DATA
			, sendingMsg.seqNo
			, &imMsgData);
	}
	);
}



//////////////////////////////////////////////////////////////////////////
CheckSendMsgTimer::CheckSendMsgTimer(SendMsgManage* pMsgCheck)
:m_pMsgCheck(pMsgCheck)
{

}

CheckSendMsgTimer::~CheckSendMsgTimer()
{

}

void CheckSendMsgTimer::process()
{
	if (m_pMsgCheck == nullptr)
	{
		return;
	}

	CAutoLock autoLock(&m_pMsgCheck->m_lock);
	if (m_pMsgCheck->m_ListSendingMsg.empty())
	{
		return;
	}
	SendingMsg& itFront = m_pMsgCheck->m_ListSendingMsg.front();
	if (itFront.status == MSGSTATUS_SENDING)//����Ϊ���ڷ��͵���Ϣ
	{
		++itFront.waitSeconds;
		if (itFront.waitSeconds > TIMEOUT_SENDINGMSG)//��ʱ����
		{
			if (itFront.retrySendCnt < SENDMSG_RETRY_CNT)
			{
				++itFront.retrySendCnt;
				itFront.waitSeconds = 0;
				//��Ϣ����ʧ��һ�κ�����һ��
				m_pMsgCheck->sendMessage(itFront);
			}
			else//��������׸���Ϣ����ʧ�ܣ�����Ϊ������Ϣ���ж���ʱ
			{
				module::getSessionModule()->asynNotifyObserver(module::KEY_SESSION_SENDMSG_FAILED, itFront.msg.sessionId);//������Ϣʧ��
			}
		}
	}
	else if (MSGSTATUS_TOSEND == itFront.status)//����Ϊ�ȴ�����״̬����Ϣ
	{
		itFront.status = MSGSTATUS_SENDING;
		m_pMsgCheck->sendMessage(itFront);
	}
}

void CheckSendMsgTimer::release()
{
	delete this;
}

/******************************************************************************/