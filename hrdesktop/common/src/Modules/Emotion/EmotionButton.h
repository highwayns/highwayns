#pragma once
#include "cxImage/cxImage/ximage.h"
#include "DuiLib/UIlib.h"
#include "GlobalDefine.h"
#include <map>
using namespace DuiLib;

class CEmotionButton : public CControlUI ,public INotifyUI
{
public:
	CEmotionButton(void);
	~CEmotionButton(void);
public:
	virtual void DoPaint(HDC hDC, const RECT& rcPaint);
	virtual void DoEvent(TEventUI& event);
	virtual void Notify(TNotifyUI& msg);
	void SetPage(int page);
	int GetCurrentPage();
	int GetPageCount();
public:
	//���ڴ�С
	int QQFACEDLG_WIDTH;
	int QQFACEDLG_HEIGHT;

	//�������Ͻ�����
	int CELLS_LEFT;
	int CELLS_TOP;
	int CELLS_RIGHT;	//(6  + 29*15)
	int CELLS_BOTTOM;	//(28 + 29*8)

	int CELLSIZE; //ÿ������= 29*30����ͼƬ�ߴ���24*24��

	int CELLCOUNT_LINE;	//ÿ��
	int CELLCOUNT_COLUMN;	//ÿ��
	int CELLCOUNT_PAGE; //ÿҳ120������

	const int TIMER_SHOWGIF = 101;	//��ʱ��-��ʾ��̬GIF
	int curSel, lastSel; //��ǰCell����һ��Cell������Cell�н�����
	int curPage;
	int frameCount;		//֡����
	int curFrame;		//��ǰ��ʾ�Ķ���֡
	TRACKMOUSEEVENT m_tme;

	HDC		m_hMemDCBkGnd;		//�����ڴ�DC
	HDC		m_hMemDC;			//�ڴ�DC
	HBITMAP m_hMemBitmapBkGnd;	//����λͼ
	HBITMAP m_hMemBitmap;		//�ڴ�λͼ

	//��ȡĳ��Cell����ɫ���ο�ѡ��ʱ���ƣ�
	void GetBlueRect(int cellIndex, LPRECT pRect);

	int  GetCellIndex(int x, int y);

	//�ͷ�ͼƬ��Դ
	void FreeImages();

	//��ȡ�����ļ�������·����
	void GetFaceFolderPath(TCHAR* path, TCHAR* folderName);

	int LoadImages(LPTSTR folder);
	int _LoadImages(IN CString folder);

	void SwitchPage(int curPage);

	CxImage* GetSelectedImage(int curPage, int curSel);
	//��ͼ��ı߿�
	void UpdateSelectedFace(int curPage, int curSel, int curFrame, int pvstatus);

	//������������ͼ����
	RECT	rcLeft, rcRight;
	int m_ImageCount;
	int m_PageCount; //ҳ�� 
	int pvstatus;		//��ǰ��ʾ�Ǹ����Σ�-1-����ʾ��0-��࣬1-�Ҳ�
	CxImage* m_CxImages;

	//����ͼ״̬
	enum _PVStatus
	{
		Hide = 0,
		Left = 1,
		Right = 2,
	};

	//����ͼλ����θı䣬��4λ��֮ǰ��ʾ״̬����4λ���µ���ʾ״̬
	enum _PosChangeType
	{
		NoChange = 0,
		HideToLeft = 0x01,
		HideToRight = 0x02,
		LeftToHide = 0x10,
		LeftToRight = 0x12,
		RightToHide = 0x20,
		RightToLeft = 0x21,
	};

	std::map<UInt32, CString>	m_mapImagePath;
};
