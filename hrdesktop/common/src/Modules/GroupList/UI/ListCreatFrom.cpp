/******************************************************************************* 
 *  @file      ListCreatFrom.cpp 2014\8\12 15:01:27 $
 *  @author    ���<dafo@mogujie.com>
 *  @brief      ���ڴ���Ⱥ���ڵ��û�����Դ�б�
 ******************************************************************************/

#include "stdafx.h"
#include "ListCreatFrom.h"



/******************************************************************************/

Node* ListCreatFrom::AddNode(const IMListItemInfo& item, Node* parent /*= NULL*/, int index /*= 0*/)
{

	return __super(item,parent,index);
}
