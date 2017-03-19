 /*******************************************************************************
 *  @file      ListCreatFrom.h 2014\8\12 15:00:54 $
 *  @author    ���<dafo@mogujie.com>
 *  @brief     ���ڴ���Ⱥ���ڵ��û�����Դ�б�
 ******************************************************************************/

#ifndef LISTCREATFROM_975EBA89_D5AE_43AC_BB2E_F682E2292925_H__
#define LISTCREATFROM_975EBA89_D5AE_43AC_BB2E_F682E2292925_H__

#include "Modules/UI/UIIMList.h"
/******************************************************************************/

/**
 * The class <code>ListCreatFrom</code> 
 *
 */
class ListCreatFrom : public UIIMList
{
public:

	Node* AddNode(const IMListItemInfo& item, Node* parent = NULL, int index = 0);
};
/******************************************************************************/
#endif// LISTCREATFROM_975EBA89_D5AE_43AC_BB2E_F682E2292925_H__
