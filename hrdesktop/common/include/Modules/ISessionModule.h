/*******************************************************************************
 *  @file      ISessionModule.h 2014\7\27 10:06:08 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief   
 ******************************************************************************/

#ifndef ISESSIONMODULE_070C0321_0708_4487_8028_C1D8934B709D_H__
#define ISESSIONMODULE_070C0321_0708_4487_8028_C1D8934B709D_H__

#include "GlobalDefine.h"
#include "Modules/ModuleDll.h"
#include "Modules/ModuleBase.h"
#include "Modules/IModuleInterface.h"
/******************************************************************************/
namespace DuiLib
{
	class CControlUI;
	class CPaintManagerUI;
}
class MessageEntity;
NAMESPACE_BEGIN(module)
const std::string MODULE_SESSION_PREFIX = "session";

//KEYID
const std::string KEY_SESSION_NEWMESSAGE		= MODULE_SESSION_PREFIX + "NewMessage";			//���յ���Ϣ����������ʱ��Ϣ��������Ϣ
const std::string KEY_SESSION_OPENNEWSESSION	= MODULE_SESSION_PREFIX + "OpenNewSession";     //֪ͨ��һ���µĻỰ
const std::string KEY_SESSION_SENDMSG_TOOFAST	= MODULE_SESSION_PREFIX + "SendMsgTooFast";     //������Ϣ̫��
const std::string KEY_SESSION_SENDMSG_FAILED	= MODULE_SESSION_PREFIX + "SendMsgFialed";      //������Ϣʧ��
const std::string KEY_SESSION_SHAKEWINDOW_MSG	= MODULE_SESSION_PREFIX + "ShakeWindowMsg";		//����������
const std::string KEY_SESSION_WRITING_MSG		= MODULE_SESSION_PREFIX + "WritingMsg";			//��������
const std::string KEY_SESSION_STOPWRITING_MSG	= MODULE_SESSION_PREFIX + "StopWrtingMsg";		//ֹͣ����������
const std::string KEY_SESSION_SENDMSGTIP_KEY	= MODULE_SESSION_PREFIX + "SendMsgTipKey";		//������Ϣ��ֵ�ı�
const std::string KEY_SESSION_TRAY_STARTEMOT	= MODULE_SESSION_PREFIX + "TrayStartEmot";		//��������ͼ����˸
const std::string KEY_SESSION_TRAY_STOPEMOT		= MODULE_SESSION_PREFIX + "TrayStopEmot";		//�ر�����ͼ����˸
const std::string KEY_SESSION_TRAY_NEWMSGSEND	= MODULE_SESSION_PREFIX + "TrayNewMsgSend"; 	//������һ����Ϣ�������ϵ�˸���
const std::string KEY_SESSION_TRAY_COPYDATA		= MODULE_SESSION_PREFIX + "TrayCopyData";		//�������ģ�鷢��������֪ͨ
const std::string KEY_SESSION_HISTORY_MESSAGE	= MODULE_SESSION_PREFIX + "HistoryMessage";		//���յ���ʷ��Ϣ
const std::string KEY_SESSION_MESSGEREADED_NOTIFY = MODULE_SESSION_PREFIX + "MessageReadEdNotify";//�����˶�ȡ�˲��ֵ���Ϣ
const std::string KEY_SESSION_UPDATE_RECENTLY_SESSIONLIST = MODULE_SESSION_PREFIX + "UpdateRecentlySessionList";		//��������Ự�б�����
const std::string KEY_SESSION_UPDATE_TOTAL_UNREADMSG_COUNT = MODULE_SESSION_PREFIX + "UpdateTotalUnReadMsgCount";		//������δ������

/////////////////////////////////SessionEntity/////////////////////////////////////////
enum
{
	SESSION_ERRORTYPE = 0,
	SESSION_USERTYPE,		//���˻Ự
	SESSION_GROUPTYPE,		//Ⱥ�Ự
};
class SessionEntity
{
public:
	SessionEntity();
	std::string getAvatarPath();
	UInt8 getOnlineState();
	CString getName();
	void setUpdatedTimeByServerTime();
	std::string getOriginSessionId();
	UInt32 getOriginIntegerSessionId();
public:
	UInt8								sessionType;		//SESSION_USERTYPE / SESSION_GROUPTYPE
	UInt32                              updatedTime;      //��Ϣ������ʱ��
	std::string                         sessionID;				//�ỰID
	UInt32								latestmsgId = 0;
	std::string							latestMsgContent;			//���һ����Ϣ����
	std::string							latestMsgFromId;			//�������һ����Ϣ����ԱID
	UInt8								sessionStatus;				//�Ƿ�ɾ��
};
typedef std::map<std::string, SessionEntity>    SessionEntityMap;
/////////////////////////////////ISessionModule/////////////////////////////////////////
/**
 * The class <code>ISessionModule</code> 
 *
 */
class MODULE_API ISessionModule : public module::ModuleBase
								 ,public module::IPduPacketParse
{
public:
	virtual DuiLib::CControlUI* createMainDialogControl(
		LPCTSTR pstrClass,DuiLib::CPaintManagerUI& paintManager) = 0;

	/**
	* �û�ģ���ʼ����������ر����û����ݣ�
	*
	* @return 
	* @exception there is no any exception to throw.
	*/
	virtual BOOL startup() = 0;


	/**@name ͬ��������ʱ��*/
	//@{
	virtual UInt32 getTime()const = 0;
	virtual void setTime(IN UInt32 time) = 0;
	virtual void startSyncTimeTimer() = 0;
	//@}

	/**@name SessionEntityManagerί�ɰ�װ*/
	//@{
	virtual void setSessionEntity(IN const module::SessionEntity& sessionInfo) = 0;//���»Ự��������ڣ���ᴴ��
	virtual void getRecentSessionList(OUT std::vector<std::string>& vecRecentSession) = 0;//����������û�չʾ����Ự�б�
	virtual BOOL getSessionEntityBySId(IN const std::string& sId, OUT module::SessionEntity& sessionEntity) = 0;
	virtual void updateSessionEntityByMsg(IN const MessageEntity& msg) = 0;
	virtual void deleteSessionEntity(IN const std::string& sessionId) = 0;
	virtual UInt32 getGlobalUpdateTime()const = 0;//�ӷ�������ȡ������µĻỰ�б����Ҫ�ύ���һ�θ��»Ự��ʱ��
	virtual void setGlobalUpdateTime(IN const UInt32 updateTime)const = 0;
	//@}

};

MODULE_API ISessionModule* getSessionModule();

NAMESPACE_END(module)
/******************************************************************************/
#endif// ISESSIONMODULE_070C0321_0708_4487_8028_C1D8934B709D_H__
