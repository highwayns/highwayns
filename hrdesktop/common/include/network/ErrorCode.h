/*******************************************************************************
 *  @file      ErrorCode.h 2012\8\16 22:21:34 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief	   IMCORE���ش�����Ķ���,ͨ��Э��Ĵ����붨����PortErrorCode.h
 ******************************************************************************/

#ifndef ERRORCODE_4A19179B_20B8_4BF7_AD9A_2468C3BF9AB7_H__
#define ERRORCODE_4A19179B_20B8_4BF7_AD9A_2468C3BF9AB7_H__

#include "GlobalDefine.h"
/******************************************************************************/
NAMESPACE_BEGIN(imcore)

typedef UInt32  IMCoreErrorCode;

//�������룬error code flag
const IMCoreErrorCode IMCORE_FLAG					= 0x000000U; // �������������
const IMCoreErrorCode IMCORE_WORK_FLAG				= 0x010000U; // ��������̨����opertaion,event�����Ĵ���

//�������
const IMCoreErrorCode IMCORE_OK									= IMCORE_FLAG | 0x00U;   //һ��OK
const IMCoreErrorCode IMCORE_ALLOC_ERROR						= IMCORE_FLAG | 0x01U;   //�ڴ�������
const IMCoreErrorCode IMCORE_INVALID_HWND_ERROR					= IMCORE_FLAG | 0x02U;   //��Ч�Ĵ��ھ��
const IMCoreErrorCode IMCORE_ARGUMENT_ERROR						= IMCORE_FLAG | 0x03U;   //�߼���������
const IMCoreErrorCode IMCORE_FILE_OPEN_ERROR					= IMCORE_FLAG | 0x04U;   //�ļ���ʧ��
const IMCoreErrorCode IMCORE_FILE_READ_ERROR					= IMCORE_FLAG | 0x05U;   //�ļ���ȡʧ��
const IMCoreErrorCode IMCORE_FILE_WRITE_ERROR					= IMCORE_FLAG | 0x06U;   //�ļ���ȡʧ��
const IMCoreErrorCode IMCORE_FILE_SYSTEM_ERROR					= IMCORE_FLAG | 0x07U;   //�ļ�δ֪�쳣

//opertaion event����
const IMCoreErrorCode IMCORE_WORK_INTERNEL_ERROR				= IMCORE_WORK_FLAG | 0x01;   //worker�ڲ�����
const IMCoreErrorCode IMCORE_WORK_PUSHOPERTION_ERROR			= IMCORE_WORK_FLAG | 0x02;   //opertaion startʧ��
const IMCoreErrorCode IMCORE_WORK_POSTMESSAGE_ERROR				= IMCORE_WORK_FLAG | 0x03;   //
const IMCoreErrorCode IMCORE_WORK_TIMER_INEXISTENCE_ERROR		= IMCORE_WORK_FLAG | 0x04;   //Timer������

NAMESPACE_END(imcore)
/******************************************************************************/
#endif// ERRORCODE_4A19179B_20B8_4BF7_AD9A_2468C3BF9AB7_H__