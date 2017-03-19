/*******************************************************************************
 *  @file      UIGroupsTreelist.h 2014\8\7 15:44:04 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief     Ⱥ��������Ự���οؼ�
 ******************************************************************************/

#ifndef UIGROUPSTREELIST_A6A09709_9687_4B3D_8924_25CA7CBC4A8F_H__
#define UIGROUPSTREELIST_A6A09709_9687_4B3D_8924_25CA7CBC4A8F_H__

#include "Modules/UI/UIIMList.h"

const CDuiString MY_DISCUSSGROUP_ITEMID = _T("myDiscussGroup_id");
const CDuiString MY_GROUP_ITEMID = _T("mygroup_id");

class GroupsListItemInfo final :public IMListItemInfo
{

};

/**
* The class <code>Ⱥ��������Ự���οؼ�</code>
*
*/
class CGroupsTreelistUI final: public UIIMList
{
public:
	
	CGroupsTreelistUI(CPaintManagerUI& paint_manager);

	Node* AddNode(const GroupsListItemInfo& item, Node* parent = NULL);//����Ⱥ
	Node* UpdateNode(const GroupsListItemInfo& groupInfo);//����Ⱥ��Ϣ
	BOOL UpdateItemBySId(const std::string& sId);
	void ClearItemMsgCount(IN const std::string& sId);//�����ʾ��δ������

	BOOL GetFirstChildItemSId(OUT std::string& sId);//�Ҹ���һ����Ч�ڵ㣬���ظ�sid���û������û����
};
/******************************************************************************/
#endif// UIGROUPSTREELIST_a6a09709-9687-4b3d-8924-25ca7cbc4a8f_H__