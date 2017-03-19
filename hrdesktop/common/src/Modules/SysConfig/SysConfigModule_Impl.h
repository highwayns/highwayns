/*******************************************************************************
 *  @file      SysConfigModule_Impl.h 2014\8\4 10:56:38 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief     ϵͳ������Ϣ������ϵͳ���ú�ȫ��������Ϣ
 ******************************************************************************/

#ifndef SYSCONFIGMODULE_IMPL_9E63D68E_676C_49DB_A936_7F97A626D551_H__
#define SYSCONFIGMODULE_IMPL_9E63D68E_676C_49DB_A936_7F97A626D551_H__


#include "Modules/ISysConfigModule.h"
/******************************************************************************/

namespace module
{
	IOperationDelegate;
}
/**
 * The class <code>ϵͳ������Ϣ������ϵͳ���ú�ȫ��������Ϣ</code> 
 *
 */
class SysConfigModule_Impl final : public module::ISysConfigModule
{
public:
    /** @name Constructors and Destructor*/

    //@{
    /**
     * Constructor 
     */
    SysConfigModule_Impl();
    /**
     * Destructor
     */
    virtual ~SysConfigModule_Impl();
    //@}

public:
	virtual module::TTConfig* getSystemConfig();
	virtual BOOL saveData();
	virtual std::string userID()const;
	virtual CString UserID()const;
	virtual UInt32 userId()const;
	virtual void showSysConfigDialog(HWND hParentWnd);
	virtual BOOL showServerConfigDialog(HWND hParentWnd);
	virtual void setSysConfigDialogFlag(BOOL bIsExist);

	virtual UInt32 getUserInfoLatestUpdateTime(void);//�ܵ��û�����ʱ��
	virtual void saveUserInfoLatestUpdateTime(IN const UInt32 nLatestUpdateTime);

	virtual UInt32 getDepartmentInfoLatestUpdateTime(void);
	virtual void saveDepartmentInfoLatestUpdateTime(IN const UInt32 nLatestUpdateTime);

	virtual UInt32 getRecentSessionLatestUpdateTime(void);
	virtual void saveRecentSessionLatestUpdateTime(IN const UInt32 nLatestUpdateTime);

	//TODO ͳһͼƬ��ȡ���߼�
	virtual BOOL getImage(IN std::string sid, IN std::string url, IN BOOL bGrayScale
		, IN std::string& format, IN module::IOperationDelegate callback
		, OUT std::string& sLocalPath);
private:
	/**
	 * �������л�������
	 *
	 * @return  BOOL
	 * @exception there is no any exception to throw.
	 */	
	BOOL _loadData();
	/**
	* �������л�������
	*
	* @return  BOOL
	* @exception there is no any exception to throw.
	*/
	BOOL _saveData();
	/**
	 * �����л�
	 *
	 * @param   CArchive & ar
	 * @return  void
	 * @exception there is no any exception to throw.
	 */
	void _unMarshal(CArchive& ar);
	/**
	* ���л�
	*
	* @param   CArchive & ar
	* @return  void
	* @exception there is no any exception to throw.
	*/
	void _marshal(CArchive& ar);

private:
	module::TTConfig			m_Config;
	BOOL						m_bSysConfigDialogFlag;//ȷ����������ʵ��
};
/******************************************************************************/
#endif// SYSCONFIGMODULE_IMPL_9E63D68E_676C_49DB_A936_7F97A626D551_H__
