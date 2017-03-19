 /*******************************************************************************
 *  @file      ReceiveMsgManage.h 2014\8\7 14:57:06 $
 *  @author    ���<dafo@mogujie.com>
 *  @brief     
 ******************************************************************************/

#ifndef RECEIVEMSGMANAGE_B3CDCA98_9B4E_482C_8342_7F2DF985F6D3_H__
#define RECEIVEMSGMANAGE_B3CDCA98_9B4E_482C_8342_7F2DF985F6D3_H__

#include "Modules/MessageEntity.h"
#include "Modules/IEvent.h"
#include "network/Lock.h"
#include <list>
#include <map>


typedef std::list<MessageEntity> SessionMessage_List;
typedef std::map<std::string, SessionMessage_List> SessionMessageMap;

/******************************************************************************/

/**
 * The class <code>ReceiveMsgManage</code> 
 *
 */
class ReceiveMsgManage
{
public:
    /**
     * Destructor
     */
	virtual ~ReceiveMsgManage();
	static ReceiveMsgManage* getInstance();

private:
    /**
     * Constructor 
     */
	ReceiveMsgManage();

public:
	BOOL pushMessageBySId(const std::string& sId, MessageEntity& msg);
	BOOL popMessageBySId(const std::string& sId, MessageEntity& msg);
	/**
	 * ȡ�����µ�һ����Ϣ�����ǲ�pop,��Ҫ����Ʈ����ϢԤ���͸��������ϵ����
	 *
	 * @param   const std::string & sId
	 * @param   MessageEntity & msg
	 * @return  BOOL
	 * @exception there is no any exception to throw.
	 */	
	BOOL frontMessageBySId(const std::string& sId, MessageEntity& msg);
	BOOL popMessagesBySId(IN const std::string& sId,OUT SessionMessage_List& msgList,IN MSG_TYPE_STATUS status = MESSAGE_TYPE_NONE,IN const UINT32 msgId = 0);
	UInt32 getUnReadMsgCountBySId(const std::string& sId);
	UInt32 getTotalUnReadMsgCount();
	void removeAllMessage();
	/**
	 * ����ӿ�Ŀǰ���Ǹ�������Ϣ�ڶ������������ʹ�ã�����ӷ��������յ��ظ�����Ϣ
	 *
	 * @param   const std::string & sId
	 * @return  void
	 * @exception there is no any exception to throw.
	 */	
	void removeMessageBySId(const std::string& sId);
	void parseContent(CString& content, BOOL bFloatForm, Int32 chatWidth);      //�ú����е����
private:
	SessionMessage_List* _getChatMsgListBySID(const std::string& sId);
	void _Quickchat2Fromat(OUT CString& content);//ת�� @С��� �ַ�
	void _urlReplace(CString& content);
	void _urlScan(CString& content);
	void _replaceUrlParts(CString& content);
private:
	SessionMessageMap           m_mapSessionMsg;
	CLock			m_lock;
	std::vector<CString>        m_scanUrls;

public:
	/**
	 * ��Ϣȥ��
	 *
	 * @param   IN const MessageEntity & msg
	 * @param   IN const UInt32 seqNo
	 * @return  BOOL
	 * @exception there is no any exception to throw.
	 */	
	BOOL checkIsReduplicatedMsg(IN const MessageEntity& msg, IN const UInt32 seqNo);
private:
	struct ReceiveMsg
	{
		MessageEntity msg;
		UInt32         seqNo;
	};
	typedef std::list<ReceiveMsg>			   ReceiveMsgList;
	typedef std::map<std::string, ReceiveMsgList>   ReceiveMsgMap;

	ReceiveMsgMap m_MsgMap;
	//��Ϣȥ��//end
};


//////////////////////////////////////////////////////////////////////////
class AudioMessageMananger    //������Ϣ
{
public:
	~AudioMessageMananger();
	static AudioMessageMananger* getInstance();

	BOOL playAudioMsgByAudioSid(IN const std::string& sSessionID, IN const std::string& sAID);
	BOOL autoplayNextUnReadAudioMsg();

	BOOL audioProcess();
	BOOL makeAppAudioSid(IN const UInt32 msgId, IN const std::string sSessionId , OUT std::string& sAID);
	BOOL saveAudioDataToFile(IN UCHAR* data, IN UINT32 lenth, IN std::string sFileName);
	BOOL getAudioMsgLenth(IN UCHAR* data, IN UINT32 lenth, OUT UInt8& AudioMsgLen);

	BOOL pushAudioMessageBySId(const std::string& sId, MessageEntity& msg);
	BOOL popPlayingAudioMsg();
	BOOL clearAudioMsgBySessionID(IN const std::string sSessionID);
	BOOL stopPlayingAnimate();

private:
	BOOL startPlayingAnimate(IN const std::string& sToPlayAID);

	SessionMessageMap           m_mapUnReadAudioMsg;

	std::string m_sPlayingSessionID;//��ǰ���ڲ��ŵĻỰID
	std::string m_sPlayingAID;//��ǰ���ڲ��ŻỰ������IS
};
/******************************************************************************/
#endif// RECEIVEMSGMANAGE_B3CDCA98_9B4E_482C_8342_7F2DF985F6D3_H__
