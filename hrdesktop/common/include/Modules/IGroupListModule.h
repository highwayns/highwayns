/*******************************************************************************
 *  @file      IGroupListModule.h 2014\8\6 15:29:01 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief     Ⱥ�����������ģ��
 ******************************************************************************/

#ifndef IGROUPLISTMODULE_3AD36DFC_4041_486A_A437_948E152517E8_H__
#define IGROUPLISTMODULE_3AD36DFC_4041_486A_A437_948E152517E8_H__

#include "GlobalDefine.h"
#include "Modules/ModuleDll.h"
#include "Modules/IUserListModule.h"
#include "Modules/IModuleInterface.h"
#include "Modules/ModuleBase.h"
#include <list>
#include <string>
/******************************************************************************/
NAMESPACE_BEGIN(module)
const std::string MODULE_GROUPLIST_PREFIX = "grouplist";

//KEYID
const std::string KEY_GROUPLIST_UPDATE_GROUPLIST		= MODULE_GROUPLIST_PREFIX + "GroupList";		//UI���¹̶�Ⱥ��������
const std::string KEY_GROUPLIST_UPDATE_GROUPINFO_UPDATE	= MODULE_GROUPLIST_PREFIX + "GroupInfoUpdate";//������Ⱥ��Ϣ�������ǹ̶�Ⱥ��Ҳ�����������飩
const std::string KEY_GROUPLIST_UPDATE_CREATNEWGROUP	= MODULE_GROUPLIST_PREFIX + "CreateNewGroup";	//����һ�������鷵��
const std::string KEY_GROUPLIST_UPDATE_MEMBER_CHANGED	= MODULE_GROUPLIST_PREFIX + "MembersChanged";	//��Ա�䶯
const std::string KEY_GROUPLIST_UPDATE_MYSELF_KICKED	= MODULE_GROUPLIST_PREFIX + "MySelfKicked";		//�Լ����߳���Ⱥ
const std::string KEY_GROUPLIST_UPDATE_SHIELD_SUCCEED   = MODULE_GROUPLIST_PREFIX + "GroupShieldSucceed";	//Ⱥ���γɹ�
class GroupInfoEntity
{
public:
	std::string			gId;					//ȺID
	std::string			avatarUrl;
	std::string			avatarLocalPath;        //ͷ�����سɹ���Ĵ洢·��
	std::string			creatorId;				//Ⱥ������Id
	CString				desc;					//Ⱥ����
	CString				csName;
	UInt32				type = 0;				//Ⱥ���ͣ�1���̶�Ⱥ 2,������
	UInt32				groupUpdated = 0;		//���һ�θ�����Ϣʱ��
	UInt32				version = 0;			//Ⱥ�汾
	UInt32				shieldStatus = 0;		//Ⱥ��Ϣ�����Ƿ����� 0: ������ 1: ����
	std::list<std::string>	groupMemeberList;

public:
	std::string getAvatarPath()
	{
		std::string path = avatarLocalPath;
		if (path.empty())
		{
			std::string sDataPath = util::cStringToString(module::getMiscModule()->getDefaultAvatar());
			if (1 == type)
			{
				path = sDataPath + "Groups.png";
			}
			else
			{
				path = sDataPath + "DiscussionGroups.png";
			}
		}
		else
		{
			std::string sDownPath = util::cStringToString(module::getMiscModule()->getDownloadDir());
			path = sDownPath + avatarLocalPath;
		}
		return path;
	}
};

typedef std::map<std::string, GroupInfoEntity>       GroupInfoMap;//Ⱥ�б�
typedef std::vector<std::string>					 GroupVec;//Ⱥ�б�ID,�����ϵȺ
/**
 * The class <code>Ⱥ�����������ģ��</code> 
 *
 */
class MODULE_API IGroupListModule : public module::ModuleBase
								  , public module::IPduPacketParse
{
public:
	/**
	* ��ȡ����Ⱥ����Ϣ�����ڲ���Ⱥ�б�
	*
	* @param   OUT module::GroupMap & Groups
	* @return  void
	* @exception there is no any exception to throw.
	*/
	virtual void getAllGroupListInfo(OUT module::GroupInfoMap& Groups) = 0;
	/**
	* ��ѯȺ��Ϣ
	*
	* @param   IN const std::string & sID
	* @param   OUT module::GroupInfoEntity & groupInfo
	* @return  BOOL
	* @exception there is no any exception to throw.
	*/
	
	virtual BOOL getGroupInfoBySId(IN const std::string& sID, OUT module::GroupInfoEntity& groupInfo) = 0;

	virtual BOOL deleteGroupInfoById(IN const std::string& sGroupId) = 0;
	/**
	* ��ȡȺ������Ա
	*
	* @param   IN const std::string & sID
	* @param   OUT module::VecUserInfoEntity & AddedMemberVec
	* @return  BOOL
	* @exception there is no any exception to throw.
	*/
	virtual BOOL popAddedMemberVecBySId(IN const std::string& sID, OUT module::UserInfoEntityVec& AddedMemberVec) = 0;
	/**
	* ��ȡȺ���ߵ����Ա
	*
	* @param   IN const std::string & sID
	* @param   OUT module::VecUserInfoEntity & RemovedMemberVec
	* @return  BOOL
	* @exception there is no any exception to throw.
	*/
	virtual BOOL popRemovedMemberVecBySId(IN const std::string& sID, OUT module::UserInfoEntityVec& RemovedMemberVec) = 0;

	/**
	* ��ȡĬ�ϵ�Ⱥͷ����Ϣ
	*
	* @return  CString
	* @exception there is no any exception to throw.
	*/
	virtual CString getDefaultAvatartPath() = 0;
	/**
	* ��ȡ�µ�Ⱥ��Ա
	*
	* @param   IN const std::string & groupId
	* @return  void
	* @exception there is no any exception to throw.
	*/
	virtual void tcpGetGroupInfo(IN const std::string& groupId) = 0;//��ȡ��Ⱥ����Ϣ
	virtual void tcpGetGroupsInfo(IN const module::GroupVec& VecGroupId) = 0;//��ȡ��Ⱥ����Ϣ
	virtual void tcpShieldGroup(IN const std::string& groupId, IN UInt32 shieldStatus) = 0;//����Ⱥ�����Ͳ�����
	virtual void onCreateDiscussionGrpDialog(const std::string& currentSessionId) = 0;
	virtual void onChangeDiscussionGrpMemberDialog(const std::string& currentSessionId) = 0;

	virtual void GetSearchGroupNameListByShortName(IN const CString& sShortName, OUT module::GroupVec & gidList) = 0;

	virtual std::string makeGroupSId(const std::string& sid) = 0;
	virtual std::string getOriginalSId(const std::string& sid) = 0;
	virtual BOOL startup() = 0;
};

MODULE_API IGroupListModule* getGroupListModule();

NAMESPACE_END(module)
/******************************************************************************/
#endif// IGROUPLISTMODULE_3AD36DFC_4041_486A_A437_948E152517E8_H__
