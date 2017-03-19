/*******************************************************************************
 *  @file      MessageEntity.h 2014\7\25 22:59:38 $
 *  @author    ���<dafo@mogujie.com>
 *  @brief   
 ******************************************************************************/

#ifndef MESSAGEENTITY_35FE027F_F23D_4444_B013_9FCF04145DD6_H__
#define MESSAGEENTITY_35FE027F_F23D_4444_B013_9FCF04145DD6_H__

#include "GlobalDefine.h"
#include "Modules/ModuleDll.h"

/******************************************************************************/

///////////////////////////��Ϣ���͸�������һ��///////////////////////////////////////////////
enum MSG_TYPE_SERVER{
	MSG_TYPE_TEXT_P2P = 0x01,
	MSG_TYPE_AUDIO_P2P = 0x02,
	MSG_TYPE_TEXT_GROUP = 0x11,
	MSG_TYPE_AUDIO_GROUP = 0x12,
};

enum MSG_TYPE_RENDER//��Ϣ��Ⱦ����--����Ǹ�IE��ʾ�õ�
{
	MESSAGE_RENDERTYPE_TEXT = 1,            //���ı���Ϣ
	MESSAGE_RENDERTYPE_IMAGE = 2,               //��ͼƬ
	MESSAGE_RENDERTYPE_SYSTEMTIPS = 4,          //ϵͳ��ʾ
	MESSAGE_RENDERTYPE_FILETRANSFER = 5,        //�ļ���ʾ
	MESSAGE_RENDERTYPE_AUDIO = 6,               //������Ϣ
};

enum MSG_TYPE_STATUS
{
	MESSAGE_TYPE_NONE = -1,
	MESSAGE_TYPE_RUNTIME = 0,               //����ʱ��Ϣ
	MESSAGE_TYPE_OFFLINE,                   //������Ϣ
	MESSAGE_TYPE_HISTORY,                   //��ʷ��Ϣ
};
enum MSG_TYPE_FROM		//��Ϣ��Դ����
{
	MESSAGETYPE_FROM_ERROR = 0,
	MESSAGETYPE_FROM_FRIEND,				//���Ե�Ե�ĺ�����Ϣ
	MESSAGETYPE_FROM_GROUP,					//����Ⱥ����Ϣ
};

class MODULE_API MessageEntity
{
public:

    MessageEntity();
    ~MessageEntity() = default;

public:
	UInt8			msgType;            //��Ϣ����		 1.�ı���Ϣ��100.������Ϣ	MSG_TYPE_SERVER
	UInt8			msgStatusType;      //��Ϣ��״̬���� 0 ���� 1 ���ߡ�2 ��ʷ		MSG_TYPE_STATUS
	UInt8			msgRenderType;      //��Ϣ��Ⱦ����								MSG_TYPE_RENDER
	UInt8			msgSessionType;		//��Ϣ��Դ		 1.������Ϣ��2.Ⱥ��Ϣ
	UInt32          msgTime;            //��Ϣ�շ�ʱ��
	std::string     content;            //��Ϣ����
	std::string     imageId;            //ͼƬID
	std::string     talkerSid;          //��Ϣ�ķ�����
	std::string     sessionId;          //�Ự��ID
	UInt32		    msgId;				//msg ID

	//�������
	UInt8           msgAudioTime;       //������Ϣʱ��
	UInt8           msgAudioReaded;     //�Ѿ����Ź�������

public:
	BOOL isMySendMsg()const;
	BOOL isFromGroupMsg()const;
	BOOL getSenderInfo(OUT CString& senderName, OUT std::string& senderAvatartPath);//��ȡ�����ߵ����ƺ�ͷ��·��
	BOOL makeGroupSessionId();//���յ���Ϣʱ�������Ⱥ��ϢҪ����ǰ׺ "group_"
	inline  BOOL isReaded()const; //for audio msg

public:
	//Ⱥ����
	std::string getOriginSessionId();
};
/******************************************************************************/
#endif// MESSAGEENTITY_35FE027F_F23D_4444_B013_9FCF04145DD6_H__
