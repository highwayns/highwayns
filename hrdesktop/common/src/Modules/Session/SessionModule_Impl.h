/*******************************************************************************
 *  @file      SessionModule_Impl.h 2014\12\31 11:41:27 $
 *  @author    ���<dafo@mogujie.com>
 *  @brief     
 ******************************************************************************/

#ifndef SESSIONMODULE_IMPL_414A6CFB_C817_43C4_9C73_7E965E7317C7_H__
#define SESSIONMODULE_IMPL_414A6CFB_C817_43C4_9C73_7E965E7317C7_H__

#include "Modules/ISessionModule.h"
/******************************************************************************/
class SyncTimeTimer;

/**
 * The class <code>SessionModule_Impl</code> 
 *
 */
class MessageEntity;
class SessionModule_Impl final : public module::ISessionModule
{
public:
    /** @name Constructors and Destructor*/

    //@{
    /**
     * Constructor 
     */
	SessionModule_Impl();
    /**
     * Destructor
     */
    virtual ~SessionModule_Impl() = default;
    //@}
	virtual void onPacket(imcore::TTPBHeader& header, std::string& pbBody);
public:
	virtual DuiLib::CControlUI* createMainDialogControl(
		IN LPCTSTR pstrClass,IN DuiLib::CPaintManagerUI& paintManager);

	virtual BOOL startup();

	/**@name ͬ��������ʱ��*/
	//@{
	virtual UInt32 getTime()const;
	virtual void setTime(UInt32 time);
	virtual void startSyncTimeTimer();
	//@}

	/**@name SessionEntityManagerί�ɰ�װ*/
	//@{
	virtual void setSessionEntity(IN const module::SessionEntity& sessionInfo);
	virtual void getRecentSessionList(OUT std::vector<std::string>& vecRecentSession);
	virtual BOOL getSessionEntityBySId(IN const std::string& sId, OUT module::SessionEntity& sessionEntity);
	virtual void updateSessionEntityByMsg(IN const MessageEntity& msg);
	virtual void deleteSessionEntity(IN const std::string& sessionId);
	virtual UInt32 getGlobalUpdateTime()const;	
	virtual void setGlobalUpdateTime(IN const UInt32 updateTime)const;
	//@}

private:
	/**@name �������˲��*/
	//@{
	void _sessionMsgData(IN std::string& pbBody);
	void _sessionMsgACK(IN const UInt16 seqNo, IN std::string& pbBody);
	void _sessionMsgTimeResponse(IN std::string& pbBody);
	void _sessionMsgUnreadCntResponse(IN std::string& pbBody);
	void _sessionMsgReadNotify(IN std::string& pbBody);
	void _sessionUnReadMsgListResponse(IN std::string& pbBody);
	void _sessionHistoryMsgListResponse(IN UInt16 reserved, IN std::string& pbBody);
	//@}
	BOOL _checkMsgFromStranger(IN MessageEntity& msg);//��Ϣ��Դ��ID�Ǵ��ڵ�ǰ�ỰID�б��У������ڣ���Ҫȥ��ȡ
	BOOL _banGroupMSG(IN MessageEntity msg);//Ⱥ��Ϣ����
	BOOL _prase2LocalMsg(OUT MessageEntity& msg);//�����ɱ��ؿ�չʾ����Ϣ
private:
	SyncTimeTimer*              m_pSyncTimer;
};
/******************************************************************************/
#endif// SESSIONMODULE_IMPL_414A6CFB_C817_43C4_9C73_7E965E7317C7_H__
