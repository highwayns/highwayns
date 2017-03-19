/*******************************************************************************
 *  @file      ChangeDiscussionGrpMemberDialog.h 2015\1\14 19:38:29 $
 *  @author    ���<dafo@mogujie.com>
 *  @brief     
 ******************************************************************************/

#ifndef CHANGEDISCUSSIONGRPMEMBERDIALOG_66DC2707_07BD_4D1E_BE03_D3DA670C1E13_H__
#define CHANGEDISCUSSIONGRPMEMBERDIALOG_66DC2707_07BD_4D1E_BE03_D3DA670C1E13_H__

#include "DuiLib/UIlib.h"
#include <list>
using namespace DuiLib;
class UIIMList;

/**
 * The class <code>ChangeDiscussionGrpMemberDialog</code> 
 *
 */
class ChangeDiscussionGrpMemberDialog : public WindowImplBase
{
public:
    /** @name Constructors and Destructor*/

    //@{
    /**
     * Constructor 
     */
	ChangeDiscussionGrpMemberDialog(IN const std::string& sid);
    /**
     * Destructor
     */
	virtual ~ChangeDiscussionGrpMemberDialog();
    //@}
	DUI_DECLARE_MESSAGE_MAP()
public:
	LPCTSTR GetWindowClassName() const;
	virtual CDuiString GetSkinFile();
	virtual CDuiString GetSkinFolder();
	virtual CControlUI* CreateControl(LPCTSTR pstrClass);
	virtual void OnFinalMessage(HWND hWnd);
	virtual LRESULT HandleMessage(UINT uMsg, WPARAM wParam, LPARAM lParam);
protected:
	void OnPrepare(TNotifyUI& msg);
	void OnItemActive(TNotifyUI& msg);
	void OnItemClick(TNotifyUI& msg);
	void OnClick(TNotifyUI& msg);
	void OnTextChanged(TNotifyUI& msg);
private:
	BOOL _AddToGroupMemberList(IN std::string sid,IN const BOOL bEnableDelete = TRUE);
	void _updateSearchResultList(IN const std::vector<std::string>& nameList);
	void _refreshUIAddedNum();
	void _changeResultList(IN const std::string& sid ,IN const BOOL bAdded);
	void _sendChangeReq();
private:
	UIIMList*				m_pListCreatFrom;		//����������Դ�б�	��
	CListUI*				m_pListGroupMembers;	//���������б�	��
	CListUI*				m_pListSearchResult;	//��������б�
	CEditUI*				m_editGroupName;
	CVerticalLayoutUI*		m_searchePanel;
	CEditUI*				m_editSearch;			//������
	CTextUI*				m_TextaddNums;			//�Ѿ����˶�����
	std::string				m_currentSessionId;		//�ỰId

	std::list<std::string>  m_addedUsers;	//���ӵ��û�
	std::list<std::string>  m_deleteUsers;	//ɾ�����û�
};
/******************************************************************************/
#endif// CHANGEDISCUSSIONGRPMEMBERDIALOG_66dc2707-07bd-4d1e-be03-d3da670c1e13_H__
