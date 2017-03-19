//===========================================================================================
//  2014.7.28 redrain�޸ģ�QQ��491646717�������ʻ���bug2����ϵ��
//	�޸ĵĴ����DoEvent���ֵ��߼�����͸��������� �옷ÿһ�죨Ⱥ�ǳ� �����裬QQ��848861075 ���Ĵ���
//  ����˵���������������¿ṷ���������������ֲ��ŵĲ���ʱ�õ�CSliderUI�ؼ�����̨����Ƶ���ȥ����CSliderUI����Ϊ
//  CSliderUI����Ϊ��ṷ�ĺܲ�һ�����м���ȱ��
//	����1��ֻ��ͨ�����CSliderUI��ĳ��λ�ò��ܴ���valuechanged��Ϣ���޷�ͨ����������ȥ���������bug������
//  ����2�����CSliderUI��ĳ��λ�ã�����굯��ʱ����Ÿı�λ�ã����������������갴��ʱ�͸ı���
//  ����3����̨�д���һֱ����SetValue�����ı们���λ��ʱ�����������������ͻ�������ڻ����һֱ��������
//  ����4�����黬���������޷�֪ͨ���������ڸı䣬������������ı�ʱ��ͨ��������һ�߻���һ�߾͸ı��������������ǻ�����ɺ��ٸı䣬
//       Ϊ���������һ���µ���ϢDUI_MSGTYPE_VALUECHANGED_MOVE���������Ϣ�Ķ���ŵ�UIDefine.h�ļ���
//		 #define DUI_MSGTYPE_VALUECHANGED_MOVE      (_T("movevaluechanged"))
//		 ����Ч�ʿ��ǣ�Ҫ��CSliderUI���������Ϣ��Ӧ����������sendmoveΪ�棬Ĭ��Ϊ��
//
//	���޸ĵĴ������ͨ�������ַ�����2014.7.28 redrain���������ң������Ҳ鿴Դ��
//	�˴��޸Ĳ���Ӱ��ؼ�ԭ�е����ԣ��Ҹ���ˮƽ���ޣ�������κ����⣬������ϵ��
//===========================================================================================
#ifndef __UISLIDER_H__
#define __UISLIDER_H__

#pragma once

namespace DuiLib
{
	class UILIB_API CSliderUI : public CProgressUI
	{
	public:
		CSliderUI();

		LPCTSTR GetClass() const;
		UINT GetControlFlags() const;
		LPVOID GetInterface(LPCTSTR pstrName);

		void SetEnabled(bool bEnable = true);

		int GetChangeStep();
		void SetChangeStep(int step);
		void SetThumbSize(SIZE szXY);
		RECT GetThumbRect() const;
		LPCTSTR GetThumbImage() const;
		void SetThumbImage(LPCTSTR pStrImage);
		LPCTSTR GetThumbHotImage() const;
		void SetThumbHotImage(LPCTSTR pStrImage);
		LPCTSTR GetThumbPushedImage() const;
		void SetThumbPushedImage(LPCTSTR pStrImage);

		void DoEvent(TEventUI& event);//2014.7.28 redrain
		void SetAttribute(LPCTSTR pstrName, LPCTSTR pstrValue);
		void PaintStatusImage(HDC hDC);

		void SetValue(int nValue);//2014.7.28 redrain
		void SetCanSendMove(bool bCanSend);//2014.7.28 redrain
		bool GetCanSendMove() const;//2014.7.28 redrain
	protected:
		SIZE m_szThumb;
		UINT m_uButtonState;
		int m_nStep;

		CDuiString m_sThumbImage;
		CDuiString m_sThumbHotImage;
		CDuiString m_sThumbPushedImage;

		CDuiString m_sImageModify;
		bool	   m_bSendMove;//2014.7.28 redrain
	};
}

#endif // __UISLIDER_H__