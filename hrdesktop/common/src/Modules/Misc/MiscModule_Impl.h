/*******************************************************************************
 *  @file      MiscModule.h 2014\8\4 9:56:39 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief     һЩ�Ƚ��ӵĹ��õĽӿں���ȫ���ŵ�����
 ******************************************************************************/

#ifndef MISCMODULE_A3EEE277_68C9_4F13_96CD_1C0AB4033F12_H__
#define MISCMODULE_A3EEE277_68C9_4F13_96CD_1C0AB4033F12_H__

#include "Modules/IMiscModule.h"
#include "Modules/ModuleBase.h"
/******************************************************************************/

/**
 * The class <code>һЩ�Ƚ��ӵĹ��õĽӿں���</code> 
 *
 */
class MiscModule_Impl : public module::IMiscModule
{
public:
    /** @name Constructors and Destructor*/

    //@{
    /**
     * Constructor 
     */
    MiscModule_Impl();
    /**
     * Destructor
     */
    virtual ~MiscModule_Impl();
    //@}
	virtual void release();

public:
	virtual CString getUsersDir();
	virtual CString getTTCommonAppdata();
	virtual CString getCurAccountDir();
	virtual CString getDownloadDir();
	virtual CString getDownloadDirByToday();
	virtual CString getDataDir();
	virtual CString getDefaultAvatar();
	virtual CString getUserTempDir();
	virtual CString getDumpDir();
	virtual CString getFileRecvDir();//�ļ����䱣���·��
	virtual CString getTTCommonAppdataUserDir();//��ϵͳ����Ŀ¼��
	virtual CString GetAudioFileSavedPath();	//�����ļ�����Ŀ¼
	virtual CString getEmotionFilesDir();
	virtual CString makeShortTimeDescription(IN UInt32 Intime);
	
	virtual void playSysConfigSound();	//�������õ�����
	virtual void playSound(const CString& path);
	virtual void quitTheApplication();
	virtual CString getAppTitle();
	virtual void asynOpenWebBrowser(CString& url, Int32 nFlag = SW_SHOWMAXIMIZED);
	virtual void doProcess1(IN std::string& pData, OUT std::string& pOutData);
	virtual void doProcess2(IN std::string& pData, OUT std::string& pOutData);
};
/******************************************************************************/
#endif// MISCMODULE_A3EEE277_68C9_4F13_96CD_1C0AB4033F12_H__
