/*******************************************************************************
 *  @file      IDatabaseModule.h 2014\8\3 10:38:47 $
 *  @author    �쵶<kuaidao@mogujie.com>
 *  @brief     ����sqliteʵ�ֱ������ݴ洢ģ�飬���û���Ϣ��
 ******************************************************************************/

#ifndef IDATABASEMODULE_086C113C_CEE3_423B_81D1_D771B443A991_H__
#define IDATABASEMODULE_086C113C_CEE3_423B_81D1_D771B443A991_H__

#include "GlobalDefine.h"
#include "Modules/ModuleDll.h"
#include "Modules/ModuleBase.h"
#include "Modules/IUserListModule.h"
#include "Modules/IGroupListModule.h"
#include "Modules/ISessionModule.h"
#include <vector>
/******************************************************************************/
class MessageEntity;
class TransferFileEntity;
NAMESPACE_BEGIN(module)
struct ImImageEntity
{
	UInt32				hashcode;		//����urlPath�����hashֵ
	std::string			filename;		//ͼƬ���ش洢����
	std::string			urlPath;		//ͼƬurl
};

/**
 * The class <code>IDatabaseModule</code> 
 *
 */
class MODULE_API IDatabaseModule : public module::ModuleBase
{
public:
	/**@name ͼƬ�洢���*/
	//@{
	virtual BOOL sqlInsertImImageEntity(const ImImageEntity& entity) = 0;
	virtual BOOL sqlGetImImageEntityByHashcode(UInt32 hashcode,ImImageEntity& entity) = 0;
	virtual BOOL sqlUpdateImImageEntity(UInt32 hashcode, module::ImImageEntity& entity) = 0;
	//@}

	/**@name �����ϵ�Ự��Ϣ*/
	//@{
	virtual BOOL sqlGetRecentSessionInfoByGId(IN std::string& sId, OUT module::SessionEntity& sessionInfo) = 0;
	virtual BOOL sqlGetAllRecentSessionInfo(OUT std::vector<module::SessionEntity>& sessionList) = 0;
	virtual BOOL sqlInsertRecentSessionInfoEntity(IN const module::SessionEntity& sessionInfo) = 0;
	virtual BOOL sqlDeleteRecentSessionInfoEntity(IN const std::string& sessionId) = 0;
	virtual BOOL sqlUpdateRecentSessionInfoEntity(IN const module::SessionEntity& sessionInfo) = 0;
	virtual BOOL sqlBatchInsertRecentSessionInfos(IN std::vector<module::SessionEntity>& sessionList) = 0;
	//@}

	/**@name �û���Ϣ���*/
	//@{
	virtual BOOL sqlGetAllUsersInfo(OUT std::vector<module::UserInfoEntity>& userList) = 0;
	virtual BOOL sqlGetUserInfoBySId(IN std::string& sId, OUT module::UserInfoEntity& userInfo) = 0;
	virtual BOOL sqlInsertUserInfoEntity(IN const module::UserInfoEntity& userInfo) = 0;
	virtual BOOL sqlUpdateUserInfoEntity(std::string& sId, IN const module::UserInfoEntity& userInfo) = 0;
	virtual BOOL sqlBatchInsertUserInfos(IN module::UserInfoEntityMap& mapUserInfos) = 0;
	//@}

	/**@name ������Ϣ���*/
	//@{
	virtual BOOL sqlGetAllDepartmentInfo(OUT std::vector<module::DepartmentEntity>& departmentList) = 0;
	virtual BOOL sqlGetDepartmentBySId(IN std::string& sId, OUT module::DepartmentEntity& departmentInfo) = 0;
	virtual BOOL sqlInsertDepartmentInfoEntity(IN const module::DepartmentEntity& departmentInfo) = 0;
	virtual BOOL sqlUpdateDepartmentInfoEntity(std::string& sId, IN const module::DepartmentEntity& departmentInfo) = 0;
	virtual BOOL sqlBatchInsertDepartmentInfos(IN module::DepartmentMap& mapDepartmentInfos) = 0;
	//@}

	/**@name Ⱥ��Ϣ���*/
	//@{
	virtual BOOL sqlGetGroupInfoByGId(IN std::string& gId,OUT module::GroupInfoEntity& groupInfo) = 0;
	virtual BOOL sqlGetAllGroupInfo(OUT std::vector<module::GroupInfoEntity>& groupList) = 0;
	virtual BOOL sqlInsertOrReplaceGroupInfoEntity(IN const module::GroupInfoEntity& groupInfo) = 0;
	virtual BOOL sqlDeleteGroupInfoEntity(IN const std::string& groupId) = 0;
	virtual BOOL sqlUpdateGroupInfoEntity(std::string& sId, IN const module::GroupInfoEntity& groupInfo) = 0;
	virtual BOOL sqlBatchInsertGroupInfos(IN module::GroupInfoMap& mapGroupInfos) = 0;
	//@}

	/**@name �Ự��Ϣ���*/
	//@{
	virtual BOOL sqlInsertMessage(IN MessageEntity& msg) = 0;
	virtual BOOL sqlBatchInsertMessage(IN std::list<MessageEntity>& msgList) = 0;
	virtual BOOL sqlGetHistoryMessage(IN const std::string& sId, IN const UInt32 msgId, IN UInt32 nMsgCount
									, OUT std::vector<MessageEntity>& msgList) = 0;
	//@}

	/**@name �ļ��������*/
	//@{
	virtual BOOL sqlInsertFileTransferHistory(IN TransferFileEntity& fileInfo) = 0;
	virtual BOOL sqlGetFileTransferHistory(OUT std::vector<TransferFileEntity>& fileList) = 0;
	//@}
};

MODULE_API IDatabaseModule* getDatabaseModule();

NAMESPACE_END(module)
/******************************************************************************/
#endif// IDATABASEMODULE_086C113C_CEE3_423B_81D1_D771B443A991_H__
