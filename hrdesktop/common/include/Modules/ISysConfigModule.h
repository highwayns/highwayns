/*******************************************************************************
 *  @file      ISysConfigModule.h 2014\8\4 10:54:39 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief     ϵͳ������Ϣ������ϵͳ���ú�ȫ��������Ϣ
 ******************************************************************************/

#ifndef ISYSCONFIGMODULE_03995006_A398_4574_BAE1_549FE4543DD3_H__
#define ISYSCONFIGMODULE_03995006_A398_4574_BAE1_549FE4543DD3_H__

#include "GlobalDefine.h"
#include "Modules/ModuleDll.h"
#include "Modules/ModuleBase.h"
/******************************************************************************/
NAMESPACE_BEGIN(module)
const UInt16 SYSCONFIG_VERSIONI = 1;
const std::string MODULE_SYSCONFIG_PREFIX = "SysConfig";

//KEYID
const std::string KEY_SYSCONFIG_UPDATED					= MODULE_SYSCONFIG_PREFIX + "Update";//�������ø���
const std::string KEY_SYSCONFIG_SHOW_USERDETAILDIALOG	= MODULE_SYSCONFIG_PREFIX + "ShowUserDetailDialog";//չʾ�û���ϸ��Ϣ����

//BaseFlag
enum
{
	BASE_FLAG_TOPMOST = 1 << 0,						//�Ƿ�������������ǰ��
	BASE_FLAG_NOTIPWHENNEWMSG = 1 << 1,				//��������ϢʱƮ����ʾ
	BASE_FLAG_NOSOUNDWHENMSG = 1 << 2,				//�Ƿ�ر���Ϣ������ʾ
	BASE_FLAG_SENDIMG_BY_CTRLENTER = 1 << 3,        //Ctrl + Enter��������Ϣ����֮Enter
};

//MusicID
enum
{
	MUSIC_SOUND_DINGDONG =  0,		//������
	MUSIC_SOUND_KEYBOARD,      //��������
	MUSIC_SOUND_DINGLIN ,       //������
	MUSIC_SOUND_CALLMSG ,        //��������
};

struct TTConfigNeedCache
{
	UInt16			version = 0;
	BOOL			isRememberPWD = TRUE;
	CString			userName;
	std::string		password;
	CString			loginServIP = _T("http://access.teamtalk.im:8080/msg_server");				//��Դ���Է�������ַ
	Int32			sysBaseFlag = 0;			//��������
	Int32			sysSoundTypeBaseFlag = 0;	//��������
};

struct TTConfig : public TTConfigNeedCache
{
	UInt8			myselectStatus;
	UInt32			loginServPort = 0;
	CString			csUserId;
	CString			token;
	CString			fileSysAddr;
	CString			fileSysBackUpAddr;
	
	std::string		msgSevPriorIP;//��Ϣ��������ַ
	std::string		msgSevBackupIP;//��Ϣ���������õ�ַ
	UInt32			msgServPort;//��Ϣ�������˿�
	std::string		userId;
};

/**
 * The class <code>ϵͳ������Ϣ������ϵͳ���ú�ȫ��������Ϣ</code> 
 *
 */
class MODULE_API ISysConfigModule : public module::ModuleBase
{
public:
	/**
	 * ��ȡϵͳ�������ݣ��ı���������֮�������Ҫ�������������saveData()��
	 *
	 * @param   SystemConfig & cfg
	 * @return  void
	 * @exception there is no any exception to throw.
	 */	
	virtual TTConfig* getSystemConfig() = 0;
	/**
	* ��ȡ�û�ID
	*
	* @return  std::string
	* @exception there is no any exception to throw.
	*/
	virtual std::string userID()const = 0;
	virtual CString UserID()const = 0;
	virtual UInt32 userId()const = 0;
	/**
	 * ���������ݱ��浽����
	 *
	 * @return  BOOL
	 * @exception there is no any exception to throw.
	 */
	virtual BOOL saveData() = 0;
	virtual BOOL showServerConfigDialog(IN HWND hParentWnd) = 0;
	/**
	 * ��ʾϵͳ���ô���
	 *
	 * @return  void
	 * @exception there is no any exception to throw.
	 */
	virtual void showSysConfigDialog(IN HWND hParentWnd) = 0;
	/**
	* �ͷŴ��ڵ�ʱ����false��������ʱ����true
	*
	* @return  void
	* @exception there is no any exception to throw.
	*/
	virtual void setSysConfigDialogFlag(IN BOOL bIsExist) = 0;
	
	/**
	 * �˺�Ŀ¼�µ��û���Ϣ������ʱ��(��ʱ������)
	 *
	 * @param   void
	 * @return  UInt32
	 * @exception there is no any exception to throw.
	 */	
	virtual UInt32 getUserInfoLatestUpdateTime(void) = 0;
	virtual void saveUserInfoLatestUpdateTime(IN const UInt32 nLatestUpdateTime) = 0;

	virtual UInt32 getDepartmentInfoLatestUpdateTime(void) = 0;
	virtual void saveDepartmentInfoLatestUpdateTime(IN const UInt32 nLatestUpdateTime) = 0;

	virtual UInt32 getRecentSessionLatestUpdateTime(void) = 0;
	virtual void saveRecentSessionLatestUpdateTime(IN const UInt32 nLatestUpdateTime) = 0;
};

MODULE_API ISysConfigModule* getSysConfigModule();

NAMESPACE_END(module)
/******************************************************************************/
#endif// ISYSCONFIGMODULE_03995006_A398_4574_BAE1_549FE4543DD3_H__
