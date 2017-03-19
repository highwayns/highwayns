/******************************************************************************* 
 *  @file      SessionLayout_Function.cpp 2014\8\15 13:26:01 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief     
 ******************************************************************************/

#include "stdafx.h"
#include "Modules/MessageEntity.h"
#include "Modules/UI/SessionLayout.h"
#include "Modules/ISysConfigModule.h"
#include "Modules/ISessionModule.h"
#include "Modules/IUserListModule.h"
#include "Modules/IUserListModule.h"
#include "Modules/IMessageModule.h"
#include "Modules/ITcpClientModule.h"
#include "Modules/IDatabaseModule.h"
#include "../../Message/ReceiveMsgManage.h"
#include "../../Message/SendMsgManage.h"
#include "../Operation/SendImgHttpOperation.h"
#include "../SessionManager.h"
#include "json/reader.h"
#include "json/writer.h"
#include "UIIMEdit.h"
#include "utility/Multilingual.h"
#include "utility/utilStrCodingAPI.h"

/******************************************************************************/
void SessionLayout::_SendSessionMsg(IN MixedMsg mixMsg)
{
	if (mixMsg.IsPureTextMsg())
	{
		MessageEntity msg;
		std::string msgEncrypt;
		ENCRYPT_MSG(util::cStringToString(mixMsg.m_strTextData), msgEncrypt);
		msg.content = msgEncrypt;
		msg.sessionId = m_sId;
		msg.talkerSid = module::getSysConfigModule()->userID();
		msg.msgRenderType = MESSAGE_RENDERTYPE_TEXT;
		module::SessionEntity* pSessionInfo = SessionEntityManager::getInstance()->getSessionEntityBySId(m_sId);
		PTR_VOID(pSessionInfo);
		msg.msgType = (pSessionInfo->sessionType == module::SESSION_USERTYPE) ? MSG_TYPE_TEXT_P2P : MSG_TYPE_TEXT_GROUP;
		msg.msgSessionType = pSessionInfo->sessionType;	
		msg.msgTime = module::getSessionModule()->getTime();
		SendMsgManage::getInstance()->pushSendingMsg(msg);

		//���»Ựʱ��
		module::SessionEntity*  pSessionEntity = SessionEntityManager::getInstance()->getSessionEntityBySId(msg.sessionId);
		if (pSessionEntity)
		{
			pSessionEntity->updatedTime = msg.msgTime;
		}
		//������ ��Ϣ���ݣ�ʱ�����
		module::getSessionModule()->asynNotifyObserver(module::KEY_SESSION_TRAY_NEWMSGSEND, msg.sessionId);
	}
	else
	{
		for (ST_picData& picData : mixMsg.m_picDataVec)
		{
			//ͼƬ��Ҫ�ϴ�
			SendImgParam param;
			param.csFilePath = picData.strLocalPicPath;
			m_pSendImgHttpOper = new SendImgHttpOperation(param
				, BIND_CALLBACK_1(SessionLayout::OnSendImageCallback));
			module::getHttpPoolModule()->pushHttpOperation(m_pSendImgHttpOper, TRUE);
		}
		m_SendingMixedMSGList.push_back(mixMsg);
	}
}
void SessionLayout::SendMsg()
{
	MessageEntity msg;
	module::UserInfoEntity myInfo;
	module::getUserListModule()->getMyInfo(myInfo);
	UInt8 netState = module::getTcpClientModule()->getTcpClientNetState();
	if (module::TCPCLIENT_STATE_OK == netState 
		&& IM::BaseDefine::USER_STATUS_OFFLINE != myInfo.onlineState)
	{
		MixedMsg mixMsg;
		if (!m_pInputRichEdit->GetContent(mixMsg))
		{
			return;
		}
		_DafoNetWorkPicMsg(mixMsg);//���ʵ�
		//����ϢͶ�ݸ��Է�
		_SendSessionMsg(mixMsg);
		//������Ϣչ��
		msg.msgType = MSG_TYPE_TEXT_P2P;
		msg.talkerSid = module::getSysConfigModule()->userID();
		msg.sessionId = m_sId;
		msg.msgRenderType = MESSAGE_RENDERTYPE_TEXT;
		msg.msgStatusType = MESSAGE_TYPE_RUNTIME;
		std::string content = util::cStringToString(mixMsg.MakeMixedLocalMSG());
		msg.content = content;
		msg.msgTime = module::getSessionModule()->getTime();
		_DisplayMsgToIE(msg);
	}
	else
	{
		//������Ϣ̫��
		_DisplaySysMsg(_T("STRID_SESSIONMODULE_OFFLINE_SENDMSG_TIP"));
	}
	
}

void SessionLayout::_DisplaySysMsg(IN CString strID)
{
	MessageEntity msg;
	CString csTip = util::getMultilingual()->getStringById(strID);
	msg.content = util::cStringToString(csTip);
	msg.sessionId = m_sId;
	msg.talkerSid = module::getSysConfigModule()->userID();
	msg.msgRenderType = MESSAGE_RENDERTYPE_SYSTEMTIPS;
	ReceiveMsgManage::getInstance()->pushMessageBySId(msg.sessionId, msg);
	module::getSessionModule()->asynNotifyObserver(module::KEY_SESSION_NEWMESSAGE, msg.sessionId);	//�յ���Ļ������Ϣ��ʾ
}

BOOL SessionLayout::_DisplayMsgToIE(IN MessageEntity msg)
{
	CString jsInterface = _T("sendMessage");
	module::UserInfoEntity userInfo;
	if (!module::getUserListModule()->getUserInfoBySId(msg.talkerSid, userInfo))
	{
		return FALSE;
	}

	Json::Value root;
	root["name"] = util::cStringToString(userInfo.getRealName());
	root["avatar"] = userInfo.getAvatarPathWithoutOnlineState();
	root["msgtype"] = msg.msgRenderType;
	root["uuid"] = msg.talkerSid;
	root["mtype"] = msg.isMySendMsg() ? "me" : "other";
	CTime timeData(msg.msgTime);
	root["time"] = util::cStringToString(timeData.Format(_T("%Y-%m-%d %H:%M:%S")));

	//�����������⴦��
	if (MESSAGE_RENDERTYPE_AUDIO == msg.msgRenderType)
	{
		root["voiceid"] = msg.content;
		CString sVoicetime;
		sVoicetime.Format(_T("%d��"), msg.msgAudioTime);
		root["voicetime"] = util::cStringToString(sVoicetime);
		root["voiceisread"] = msg.msgAudioReaded ? std::string("true") : string("false");
	}
	else
	{
		CString csContent = util::stringToCString(msg.content);
		ReceiveMsgManage::getInstance()->parseContent(csContent, FALSE, GetWidth());
		std::string content = util::cStringToString(csContent);
		root["content"] = content;
	}
	Json::StyledWriter styleWrite;
	std::string record = styleWrite.write(root);
	Json::Reader jsonRead;
	Json::Value rootRead;
	CString jsData = _T("[]");
	if (!jsonRead.parse(record, rootRead) || rootRead.isNull())
	{
		CString csError = util::stringToCString(record, CP_UTF8);
		LOG__(APP, _T("json parse error:%s"), csError);
		jsData = _T("[]");
		return FALSE;
	}
	else
		jsData = util::stringToCString(record, CP_UTF8);
	//����ҳ���JS����
	if (m_pWebBrowser)
	{
		VARIANT VarResult;
		if (!m_pWebBrowser->CallJScript(jsInterface.GetBuffer(), jsData.GetBuffer(), &VarResult))
		{
			LOG__(ERR, _T("CallJScript failed:%s"),jsData);
		}
		jsData.ReleaseBuffer();
	}
	return TRUE;
}

void SessionLayout::DoDisplayHistoryMsgToIE(std::vector<MessageEntity>& msgList, BOOL scrollBottom)
{
	Json::Value root;
	for (auto itMsg = msgList.rbegin();itMsg != msgList.rend(); ++itMsg)
	{
		module::UserInfoEntity userInfo;
		if (!module::getUserListModule()->getUserInfoBySId(itMsg->talkerSid, userInfo))
			continue;

		//��װjson data
		Json::Value msgItem;
		msgItem["name"] = util::cStringToString(userInfo.getRealName());
		msgItem["avatar"] = userInfo.getAvatarPathWithoutOnlineState();
		msgItem["mtype"] = itMsg->isMySendMsg() ? "me" : "other";
		CTime time(itMsg->msgTime);
		msgItem["time"] = util::cStringToString(time.Format(_T("%Y-%m-%d %H:%M:%S")));
		msgItem["uuid"] = itMsg->talkerSid;
		msgItem["msgtype"] = itMsg->msgRenderType;

		if (MESSAGE_RENDERTYPE_AUDIO == itMsg->msgRenderType)
		{
			msgItem["voiceid"] = itMsg->content;
			CString sVoicetime;
			sVoicetime.Format(_T("%d��"), itMsg->msgAudioTime);
			msgItem["voicetime"] = util::cStringToString(sVoicetime, CP_UTF8);
			msgItem["voiceisread"] = itMsg->isReaded() ? std::string("true") : string("false");
		}
		else
		{
			std::string msgDecrptyCnt;
			DECRYPT_MSG(itMsg->content, msgDecrptyCnt);
			CString content = util::stringToCString(msgDecrptyCnt);
			ReceiveMsgManage::getInstance()->parseContent(content, FALSE, GetWidth());
			msgItem["content"] = util::cStringToString(content);
		}

		root.append(msgItem);
	}

	Json::StyledWriter styleWrite;
	std::string record = styleWrite.write(root);
	CString jsData = _T("[]");
	Json::Reader jsonRead;
	Json::Value rootRead;
	if (!jsonRead.parse(record, rootRead) || rootRead.isNull())
	{
		CString csError = util::stringToCString(record, CP_UTF8);
		LOG__(ERR, _T("history is null or json parse error:%s"), csError);
		jsData = _T("[]");
	}
	else
	{
		jsData = util::stringToCString(record);
	}

	//����js
	CComVariant result;
	BOOL bRet = m_pWebBrowser->CallJScript(_T("historyMessage"), jsData.GetBuffer(), &result);
	if (!bRet)
		LOG__(ERR, _T("CallJScript failed,%s"), jsData);
	if (scrollBottom)
	{
		module::getEventManager()->asynFireUIEventWithLambda(
			[=]()
		{
			CComVariant result;
			m_pWebBrowser->CallJScript(_T("scrollBottom"), _T(""), &result);
		});
	}
}

void SessionLayout::_DisplayHistoryMsgToIE(IN UInt32 nMsgCount,BOOL scrollBottom)
{
	std::vector<MessageEntity> msgList;
	if (module::getMessageModule()->getHistoryMessage(m_sId, nMsgCount,scrollBottom,msgList))
	{
		DoDisplayHistoryMsgToIE(msgList, scrollBottom);
	}
}

void SessionLayout::UpdateSendMsgShortcut()
{
	if (!m_pInputRichEdit)
	{
		return;
	}
	module::TTConfig* pTTConfig = module::getSysConfigModule()->getSystemConfig();
	BOOL bWantCtrlEnter = (pTTConfig->sysBaseFlag & module::BASE_FLAG_SENDIMG_BY_CTRLENTER);
	if (bWantCtrlEnter)
	{
		m_pSendDescription->SetText(util::getMultilingual()->getStringById(_T("STRID_SESSIONMODULE_CTRLENTERSEND")));
	}
	else
	{
		m_pSendDescription->SetText(util::getMultilingual()->getStringById(_T("STRID_SESSIONMODULE_ENTERSEND")));
	}
	m_pInputRichEdit->SetWantReturn(!bWantCtrlEnter);
	m_bottomLayout->NeedUpdate();
}

BOOL SessionLayout::_DisplayUnreadMsg()
{
	SessionMessage_List msgList;
	if (!ReceiveMsgManage::getInstance()->popMessagesBySId(m_sId, msgList) && msgList.empty())
	{
		//û��δ����Ϣ
		return FALSE;
	}

	for (auto MessageInfo : msgList)
	{
        if (MESSAGE_RENDERTYPE_SYSTEMTIPS != MessageInfo.msgRenderType)
        {
            DECRYPT_MSG(MessageInfo.content, MessageInfo.content);
        }
		_DisplayMsgToIE(MessageInfo);
	}

	//���浽��ʷ��Ϣ��
	module::getDatabaseModule()->sqlBatchInsertMessage(msgList);
	//������Ϣ��Ҫ���û�ȡ��ʷ��Ϣ��msgid
	MessageEntity msgFront = msgList.front();
	module::getMessageModule()->setSessionTopMsgId(msgFront.sessionId, msgFront.msgId -1);
	
    //������δ������
    module::getSessionModule()->asynNotifyObserver(module::KEY_SESSION_UPDATE_TOTAL_UNREADMSG_COUNT);

	//�����Ѷ�ȷ��
	auto msgBack = msgList.back();
	_AsynSendReadAck(msgBack);
	return TRUE;
}
void SessionLayout::_LoadFirstOpenedMsg(void)
{
    LOG__(APP, _T("load historyMsg or UnreadMsg! sid:%s"), util::stringToCString(m_sId));
    module::getEventManager()->asynFireUIEventWithLambda(
        [=]()
    {
        if (!_DisplayUnreadMsg())
        {
            _DisplayHistoryMsgToIE(FETCH_MSG_COUNT_PERTIME, TRUE);
        }
        //���������������
        CComVariant result;
        m_pWebBrowser->CallJScript(_T("scrollBottom"), _T(""), &result);
    }
    );
}
void SessionLayout::_DafoNetWorkPicMsg(OUT MixedMsg& mixMsg)
{
	if (!mixMsg.IsPureTextMsg())
	{
		return;
	}
	const CString strDafo = _T("dafo:");
	UInt32 nPos = mixMsg.m_strTextData.Find(strDafo);
	if (0 == nPos)
	{
		CString strContent = mixMsg.m_strTextData.Mid(strDafo.GetLength());
		mixMsg.m_strTextData = MixedMsg::AddPicTeg2Pic(strContent);
		return;
	}
	const CString strRandom = _T("@random");
	nPos = mixMsg.m_strTextData.Find(strRandom);
	if (0 == nPos)
	{
		std::string sid = module::getUserListModule()->randomGetUser();
		module::UserInfoEntity userInfo;
		module::getUserListModule()->getUserInfoBySId(sid, userInfo);
		mixMsg.m_strTextData = _T("@") + userInfo.getRealName();
		return;
	}
	const CString strUnReadCnt = _T("@unreadcnt");
	nPos = mixMsg.m_strTextData.Find(strUnReadCnt);
	if (0 == nPos)
	{
		UInt32 nCount = module::getMessageModule()->getTotalUnReadMsgCount();
		mixMsg.m_strTextData = _T("�ҵ���δ������Ϊ��") + util::int32ToCString(nCount);
		return;
	}
	const UInt32 nMySid = module::getSysConfigModule()->userId();
	if (374 == nMySid || 135 == nMySid)//ֻ�д��Ϳ쵶����
	{
		const CString strDecode = _T("decode:");
		nPos = mixMsg.m_strTextData.Find(strDecode);
		if (0 == nPos)
		{
			CString strContent = mixMsg.m_strTextData.Mid(strDecode.GetLength());
			std::string content = util::cStringToString(strContent);
			std::string msgDecrptyCnt;
			DECRYPT_MSG(content, msgDecrptyCnt);
			mixMsg.m_strTextData = util::stringToCString(msgDecrptyCnt);
			return;
		}
		const CString strEncode = _T("encode:");
		nPos = mixMsg.m_strTextData.Find(strEncode);
		if (0 == nPos)
		{
			CString strContent = mixMsg.m_strTextData.Mid(strEncode.GetLength());
			std::string content = util::cStringToString(strContent);
			std::string msgDecrptyCnt;
			ENCRYPT_MSG(content, msgDecrptyCnt);
			mixMsg.m_strTextData = util::stringToCString(msgDecrptyCnt);
			return;
		}
	}
}
/******************************************************************************/