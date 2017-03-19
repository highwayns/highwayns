/*******************************************************************************
 *  @file      ReloginManager.h 2013\9\4 16:44:19 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief   ������������
 ******************************************************************************/

#ifndef RELOGINMANAGER_95607570_AE99_4D6C_B2F6_25CD0FD8A0FA_H__
#define RELOGINMANAGER_95607570_AE99_4D6C_B2F6_25CD0FD8A0FA_H__

#include "Modules/IEvent.h"
#include <memory>

/******************************************************************************/
class ReloginManager;

/**
 * The class <code>ReloginManager</code> 
 *
 */
class ReloginManager
{
    friend class ReloginTimer;
public:
    /** @name Constructors and Destructor*/

    //@{
    /**
     * Constructor 
     */
    ReloginManager();
    /**
     * Destructor
     */
    ~ReloginManager();
    //@}
	void OnOperationCallback(std::shared_ptr<void> param);

public:
    void startReloginTimer(UInt32 second);
    void forceRelogin();
    
private:
    void doRelogin();

private:
    UInt32                      m_secondCount;      //����ʱ���3�������15��
    BOOL                        m_bDoReloginNow;
};

/******************************************************************************/
#endif// RELOGINMANAGER_95607570_AE99_4D6C_B2F6_25CD0FD8A0FA_H__