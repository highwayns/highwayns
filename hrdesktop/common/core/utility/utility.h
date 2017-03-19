// utility.h : utility DLL ����ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������


// CutilityApp
// �йش���ʵ�ֵ���Ϣ������� utility.cpp
//

class CutilityApp : public CWinApp
{
public:
	CutilityApp();

// ��д
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
