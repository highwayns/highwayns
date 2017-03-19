// Modules.h : Modules DLL ����ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������


// CModulesApp
// �йش���ʵ�ֵ���Ϣ������� Modules.cpp
//

class CModulesApp : public CWinApp
{
public:
	CModulesApp();

// ��д
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
