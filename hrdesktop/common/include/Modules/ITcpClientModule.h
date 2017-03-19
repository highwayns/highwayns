/*******************************************************************************
 *  @file      ITcpClientModule.h 2014\7\29 13:11:43 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief     �ͻ���TCP���糤����ģ��ӿڶ���
 ******************************************************************************/

#ifndef ITCPCLIENTMODULE_66DE1A78_4D88_4416_BFC6_F4F16832ECBB_H__
#define ITCPCLIENTMODULE_66DE1A78_4D88_4416_BFC6_F4F16832ECBB_H__

#include "GlobalDefine.h"
#include "ModuleDll.h"
#include "Modules/ModuleBase.h"
#include "google/protobuf/message_lite.h"
/******************************************************************************/
class   CImPdu;
namespace IM
{
	namespace Login
	{
		class IMLoginRes;
	}
}
NAMESPACE_BEGIN(module)
const std::string MODULE_TCPCLIENT_PREFIX = "TcpClient";

//KEYID
const std::string KEY_TCPCLIENT_STATE = MODULE_TCPCLIENT_PREFIX + "State";	//TCP����״̬���

/**
* The class <code>TcpClientSocket</code>
*
*/
struct ITcpClientSocketCallback
{
	virtual void onClose(int error) = 0;
	virtual void onReceiveData(const char* data, UInt32 size) = 0;
	virtual void onParseError() = 0;
	virtual void onConnectDone() = 0;
};

enum TCPCLIENT_STATE
{
	TCPCLIENT_STATE_OK = 0,
	TCPCLIENT_STATE_DISCONNECT
};

/**
 * The class <code>�ͻ���TCP���糤����ģ�飬��TcpClientScoket��һ������</code> 
 *
 */
class MODULE_API ITcpClientModule : public module::ModuleBase
{
public:
	virtual IM::Login::IMLoginRes* doLogin(CString &linkaddr, UInt16 port
		,CString& uName, std::string& pass) = 0;
	virtual void shutdown() = 0;

	/**
	* ����Э���
	*
	* @return  void
	* @exception there is no any exception to throw.
	*/
	virtual void sendPacket(CImPdu* pdu) {} ;
	virtual void sendPacket(UInt16 moduleId, google::protobuf::MessageLite* pbBody) = 0;
	virtual void sendPacket(UInt16 moduleId, UInt16 cmdId, google::protobuf::MessageLite* pbBody) = 0;
	virtual void sendPacket(UInt16 moduleId, UInt16 cmdId, UInt16 reserved, google::protobuf::MessageLite* pbBody) = 0;
	/**
	 * �ͻ��˿������������
	 *
	 * @return  void
	 * @exception there is no any exception to throw.
	 */
	
	virtual void startHeartbeat() = 0;
	virtual UInt8 getTcpClientNetState()const = 0;
};

MODULE_API ITcpClientModule* getTcpClientModule();

NAMESPACE_END(module)
/******************************************************************************/
#endif// ITCPCLIENTMODULE_66DE1A78_4D88_4416_BFC6_F4F16832ECBB_H__
