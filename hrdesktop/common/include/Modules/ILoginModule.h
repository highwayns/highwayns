/*******************************************************************************
 *  @file      ILoginModule.h 2014\7\17 19:38:12 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief   
 ******************************************************************************/

#ifndef ILOGINMODULE_2BB11F1D_3E16_44D3_92FB_6CAE04D48B6B_H__
#define ILOGINMODULE_2BB11F1D_3E16_44D3_92FB_6CAE04D48B6B_H__

#include "GlobalDefine.h"
#include "Modules/IModuleInterface.h"
#include "Modules/ModuleDll.h"
#include "Modules/ModuleBase.h"
#include <string>
/******************************************************************************/
NAMESPACE_BEGIN(module)
const std::string MODULE_LOING_PREFIX = "login";

//KEYID
const std::string KEY_LOGIN_KICKOUT = MODULE_LOING_PREFIX + "kickout";				//�߳�����
const std::string KEY_LOGIN_RELOGINOK = MODULE_LOING_PREFIX + "reloginok";			//relogin�ɹ�֪ͨ

/**
 * The class <code>ILoginModule</code> 
 *
 */
class MODULE_API ILoginModule : public module::ModuleBase
							   ,public module::IPduPacketParse
{
public:
	virtual BOOL showLoginDialog() = 0;
	/**
	 * ��½�ɹ�����ʼ��ȡ��֯�ܹ���Ⱥ��Ϣ
	 *
	 * @return  void
	 * @exception there is no any exception to throw.
	 */
	virtual void notifyLoginDone() = 0;
	/**
	 * ���������ӿ�
	 *
	 * @param   BOOL bForce �Ƿ�ǿ������
	 * @return  void
	 * @exception there is no any exception to throw.
	 */	
	virtual void relogin(BOOL bForce) = 0;
	virtual BOOL isOfflineByMyself()const = 0;
	virtual void setOfflineByMyself(BOOL b) = 0;
};

MODULE_API ILoginModule* getLoginModule();

NAMESPACE_END(module)
/******************************************************************************/
#endif// ILOGINMODULE_2BB11F1D_3E16_44D3_92FB_6CAE04D48B6B_H__
