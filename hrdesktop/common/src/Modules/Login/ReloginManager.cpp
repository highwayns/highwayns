/******************************************************************************* 
 *  @file      ReloginManager.cpp 2013\9\4 16:44:21 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief   
 ******************************************************************************/

#include "stdafx.h"
#include "ReloginManager.h"
#include "LoginOperation.h"
#include "Modules/IUserListModule.h"
#include "Modules/ISysConfigModule.h"
#include "Modules/ILoginModule.h"
#include "Modules/ISessionModule.h"
#include "Modules/IMiscModule.h"
#include "Modules/ITcpClientModule.h"
#include "Modules/IMessageModule.h"
#include "ProtocolBuffer/IM.Buddy.pb.h"
#include "utility/Multilingual.h"
#include "network/ImCore.h"
/******************************************************************************/
// -----------------------------------------------------------------------------
//  ReloginManager: Public, Constructor

ReloginManager::ReloginManager()
:m_secondCount(3)
,m_bDoReloginNow(FALSE)
{

}

// -----------------------------------------------------------------------------
//  ReloginManager: Public, Destructor

ReloginManager::~ReloginManager()
{
}
// -----------------------------------------------------------------------------
// public   
void ReloginManager::startReloginTimer(UInt32 second)
{
    if(second > 15)
        second = 15;
	module::ITimerEvent* pTimer = 0;
	module::getEventManager()->scheduleTimerWithLambda(second, FALSE,
		[=]()
	{
		doRelogin();
	}
	, &pTimer);
    m_secondCount = second;
}
/******************************************************************************/
// -----------------------------------------------------------------------------
// public   
void ReloginManager::forceRelogin()
{
    doRelogin();
}

// -----------------------------------------------------------------------------
// private   
void ReloginManager::doRelogin()
{
    try
    {
        if(m_bDoReloginNow)
        {
            LOG__(APP,_T("is doing Relogin now..."));
            return;
        }

        //������������ δ���͵� ��Ϣ�Ѷ�ȷ�� Operation����������ܻ���������Ϣ��
        imcore::IMLibCoreClearOperationByName(imcore::OPERATION_NAME_MSG_READ_ACK);

        LoginParam param;
		module::TTConfig* pCfg = module::getSysConfigModule()->getSystemConfig();
		param.mySelectedStatus = pCfg->myselectStatus;
		param.csUserName = pCfg->userName;
		param.password = pCfg->password;
		param.csUserName.Trim();

		LoginOperation* pOperation = new LoginOperation(BIND_CALLBACK_1(ReloginManager::OnOperationCallback), param);
		imcore::IMLibCoreStartOperation(pOperation);
        m_bDoReloginNow = TRUE;
    }
    catch (...)
    {
		module::getTcpClientModule()->shutdown();
        startReloginTimer(++m_secondCount);
        LOG__(ERR, _T("relogin unknown exception"));
        m_bDoReloginNow = FALSE;
    }
}

void ReloginManager::OnOperationCallback(std::shared_ptr<void> param)
{
	m_bDoReloginNow = FALSE;
	LoginParam* pLoginParam = (LoginParam*)param.get();
	if (LOGIN_OK == pLoginParam->result)
	{
		LOG__(ERR, _T("ReloginManager regloin success!!!"));

		module::getSessionModule()->setTime(pLoginParam->serverTime);

		//֪ͨ�������ͻ��˳�ʼ�����,��ȡ��֯�ܹ���Ϣ��Ⱥ�б�
		module::getLoginModule()->notifyLoginDone();

		//��մ��ڿͻ��˵�δ����Ϣ����Ϊ������ֻὫ�����Ϣ�͹����������ظ�
		module::getMessageModule()->removeAllMessage();

		//��ȡ�����ϵ�Ự
		UInt32 updateTime = module::getSessionModule()->getGlobalUpdateTime();
		LOG__(APP, _T("IMRecentContactSessionReq after relogin done, local update time = %d"), updateTime);
		IM::Buddy::IMRecentContactSessionReq imRecentContactSessionReq;
		imRecentContactSessionReq.set_user_id(module::getSysConfigModule()->userId());
		imRecentContactSessionReq.set_latest_update_time(updateTime);
		module::getTcpClientModule()->sendPacket(IM::BaseDefine::ServiceID::SID_BUDDY_LIST
			, IM::BaseDefine::BuddyListCmdID::CID_BUDDY_LIST_RECENT_CONTACT_SESSION_REQUEST
			, &imRecentContactSessionReq);

		//֪ͨ�����Ѿ��ָ����������Խ��и��ֲ�����
		module::getLoginModule()->asynNotifyObserver(module::KEY_LOGIN_RELOGINOK, pLoginParam->mySelectedStatus);
	}
	else
	{
		LOG__(ERR, _T("ReloginManager regloin failed!!!"));
		module::getTcpClientModule()->shutdown();

		//TCP\IP��֤tokenʧЧ,�������»�ȡtoken��task
		//if (LOGIN_TOKEN_FAILED == pLoginParam->result)
		{
			//�����ʱ��ȡtoken�Ķ�ʱ��
		}
		if (IM::BaseDefine::REFUSE_REASON_VERSION_TOO_OLD == pLoginParam->result)
		{
			CString csTip = util::getMultilingual()->getStringById(_T("STRID_WEBLOGINFORM_TIP_VERSION_TOOOLD"));
			CString csTitle = module::getMiscModule()->getAppTitle();
			::MessageBox(0, csTip, csTitle, MB_OK | MB_ICONINFORMATION);
			module::getMiscModule()->quitTheApplication();
		}
		else
		{
			startReloginTimer(++m_secondCount);
		}
	}
}
