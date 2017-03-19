// DuiLibEx.h : DuiLibEx DLL ����ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������


// CDuiLibExApp
// �йش���ʵ�ֵ���Ϣ������� DuiLibEx.cpp
//

class CDuiLibExApp : public CWinApp
{
public:
	CDuiLibExApp();

// ��д
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
