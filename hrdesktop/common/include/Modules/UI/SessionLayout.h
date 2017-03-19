/*******************************************************************************
 *  @file      SessionLayout.h 2014\12\31 14:05:29 $
 *  @author    ���<dafo@mogujie.com>
 *  @brief     
 ******************************************************************************/
#ifndef SESSIONLAYOUT_6BC9730E_47F6_4BCB_936D_AC034AA10DFF_H__
#define SESSIONLAYOUT_6BC9730E_47F6_4BCB_936D_AC034AA10DFF_H__

#include "DuiLib/UIlib.h"
#include "GlobalDefine.h"
#include "Modules/ModuleObserver.h"
#include "UIIMEdit.h"
#include <memory>
/******************************************************************************/
const UInt32 INIT_FOCUS_TIMER_ID = 10088;
const UInt8	FETCH_MSG_COUNT_PERTIME = 10;

using namespace DuiLib;
class MessageEntity;
class EmotionDialog;
namespace module
{
	struct IHttpOperation;
}
/**
* The class <code>SessionLayout</code>
*
*/
class SessionLayout :public CHorizontalLayoutUI, public INotifyUI, public CWebBrowserEventHandler
{
public:
    /** @name Constructors and Destructor*/
    //@{
    /**
     * Constructor 
     */
	SessionLayout(const std::string& sId, CPaintManagerUI& paint_manager);
    /**
     * Destructor
     */
    virtual ~SessionLayout();
    //@}

public:
	virtual void DoInit();
	virtual void DoEvent(TEventUI& event);
	virtual void Notify(TNotifyUI& msg);
	void OnWindowInitialized(TNotifyUI& msg);
	void DocmentComplete(IDispatch *pDisp, VARIANT *&url);//�򿪻Ự�Ѿ������������ʼ����ɣ�����δ����Ϣ
	virtual HRESULT STDMETHODCALLTYPE TranslateUrl(
		/* [in] */ DWORD dwTranslate,
		/* [in] */ OLECHAR __RPC_FAR *pchURLIn,
		/* [out] */ OLECHAR __RPC_FAR *__RPC_FAR *ppchURLOut);
	virtual void NewWindow2(VARIANT_BOOL *&Cancel, BSTR bstrUrl);

	//������Ŷ�������
	BOOL StopPlayingAnimate(std::string& sAudioPlayingID);
	BOOL StartPlayingAnimate(std::string sAudioPlayingID);
public:
	/**@name MKO*/
	//@{
	void MKOForEmotionModuleCallBack(const std::string& keyId, MKO_TUPLE_PARAM mkoParam);
	void MKOForGroupModuleCallBack(const std::string& keyId, MKO_TUPLE_PARAM mkoParam);
	//@}

public:
	void SendMsg();
	void UpdateRunTimeMsg();	//����δ����Ϣ
	void UpdateSendMsgShortcut();//���¿�ݼ�
	void UpdateBottomLayout();
	void FreshGroupMemberAvatar(IN const std::string& sID);		//ˢ��Ⱥ��Ա������״̬
	void FreshAllGroupMemberAvatar();
	void DoDisplayHistoryMsgToIE(std::vector<MessageEntity>& msgList, BOOL scrollBottom);

	void OnSendImageCallback(std::shared_ptr<void> param);//����ͼƬ����
    void OnFinishScreenCapture(__in LPCTSTR lpFilePath);
    UInt32 GetGroupItemCnt(void);
private:
	/**
	 * չʾȺ��Ա
	 *
	 * @return  void
	 * @exception there is no any exception to throw.
	 */
	void _UpdateGroupMembersList();
	void _AddGroupMemberToList(IN const std::string& sID,IN const BOOL bCreator);
	void _UpdateSearchRsultList(IN const std::vector<std::string>& nameList);
    void _LoadFirstOpenedMsg(void);//���ص�һ�δ򿪶Ի�������Ϣ������δ����Ϣ�������ʷ��Ϣ��
	BOOL _DisplayUnreadMsg();
	void _DisplayHistoryMsgToIE(UInt32 nMsgCount, BOOL scrollBottom);
	BOOL _DisplayMsgToIE(IN MessageEntity msg);
	void _DisplaySysMsg(IN CString strID);
	void _SendSessionMsg(IN MixedMsg mixMsg);//Ͷ����Ϣ
	void _SendImage(CString& csFilePath);
	void _CreateMenu(IN const TNotifyUI& msg);
	void _GetGroupNameListByShortName(IN const CString& sShortName, OUT std::vector<string>& nameList);

	/**
	 * �����Ѷ�ȷ��
	 *
	 * @param   IN MessageEntity msg
	 * @return  void
	 * @exception there is no any exception to throw.
	 */
	void _AsynSendReadAck(IN MessageEntity& msg);	
	void _DafoNetWorkPicMsg(OUT MixedMsg& mixMsg);//���ʵ�����Ϣ��̬����
public:
	CPaintManagerUI&	m_paint_manager;

	CWebBrowserUI*		m_pWebBrowser;//������ʾ��
    BOOL                m_bDocumentReady = FALSE;
	UIIMEdit*			m_pInputRichEdit;

	CContainerUI*		m_pRightContainer;
	CListUI*			m_pGroupMemberList;
	CEditUI*			m_pSearchEdit;
	CListUI*			m_pSearchResultList;

	CTextUI*			m_pSendDescription;	// ctrl+enter /enter

	CButtonUI*			m_pBtnSendMsg;
	CButtonUI*			m_pBtnClose;

	CButtonUI*			m_pBtnEmotion;		//����
	CButtonUI*			m_pBtnSendImage;	//����ͼƬ
    CButtonUI*			m_pBtnScreenShot;	//����
	CButtonUI*			m_pBtnshock;
	CButtonUI*			m_pBtnsendfile;
	CButtonUI*			m_pBtnbanGroupMsg;
	CButtonUI*			m_pBtndisplayGroupMsg;
	CButtonUI*			m_pBtnadduser;//����������Ա/����������

	CHorizontalLayoutUI*	m_bottomLayout;

	std::string				m_sId;								//�ỰId
	std::vector<MixedMsg>	m_SendingMixedMSGList;				//���ڷ��͵�ͼ�Ļ�����Ϣ
	time_t					m_tShakeWindow;
	CString					m_csTobeTranslateUrl;				//IE�ؼ�ת��URL

	BOOL					m_bGroupSession;					//��Ϊ����״̬�ж��ã�Ⱥ�ǲ���Ҫ��״̬��
	BOOL					m_bWritingTimerExist;				//���������timer�Ƿ����
	module::IHttpOperation*	m_pSendImgHttpOper = 0;
};
/******************************************************************************/
#endif// SESSIONLAYOUT_6BC9730E_47F6_4BCB_936D_AC034AA10DFF_H__
