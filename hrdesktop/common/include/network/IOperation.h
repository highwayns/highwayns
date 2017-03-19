/*******************************************************************************
 *  @file      IOperation.h 2014\7\16 19:10:09 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief	   �첽����ִ�е�λ
 ******************************************************************************/

#ifndef IOPERATION123_D8CF1B19_B95A_4A75_82FB_7572A1BB9A30_H__
#define IOPERATION123_D8CF1B19_B95A_4A75_82FB_7572A1BB9A30_H__

#include "GlobalDefine.h"
#include "network/networkdll.h"
#include <memory>
/******************************************************************************/
NAMESPACE_BEGIN(imcore)

/**
 * The class <code>IOperation</code> 
 *
 */
struct NETWORK_DLL IOperation
{
public:
	virtual void process() = 0;
//private:
	/**
	* �������������ͷ��Լ�
	*
	* @return  void
	* @exception there is no any exception to throw.
	*/
	virtual void release() = 0;
};

NAMESPACE_END(imcore)
/******************************************************************************/
#endif// IOPERATION123_D8CF1B19_B95A_4A75_82FB_7572A1BB9A30_H__
