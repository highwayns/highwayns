/*******************************************************************************
 *  @file      SyncTimerTimer.h 2013\9\6 14:48:43 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief   ͬ��������ʱ��timer
 ******************************************************************************/

#ifndef SYNCTIMERTIMER_33883969_3CB9_47FD_BC38_4E860C0731F8_H__
#define SYNCTIMERTIMER_33883969_3CB9_47FD_BC38_4E860C0731F8_H__

#include "Modules/IEvent.h"

/******************************************************************************/
/**
 * The class <code>SyncTimerTimer</code> 
 *
 */
class SyncTimeTimer : public module::ITimerEvent
{
public:
    /** @name Constructors and Destructor*/

    //@{
    /**
     * Constructor 
     */
    SyncTimeTimer();
    /**
     * Destructor
     */
    virtual ~SyncTimeTimer() = default;
    //@}
	virtual void process();
    virtual void release();

public:
    inline Int32 getTime()const { return m_serverTime;}
    inline void setTime(UInt32 time) { m_serverTime = time;}

private:
    UInt32                  m_serverTime;           //������ʱ��
    UInt32                  m_timeCount;            //ʱ�������
};
/******************************************************************************/
#endif// SYNCTIMERTIMER_33883969_3CB9_47FD_BC38_4E860C0731F8_H__
