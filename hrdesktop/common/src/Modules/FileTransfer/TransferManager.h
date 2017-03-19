 /*******************************************************************************
 *  @file      TransferManager.h 2014\9\3 11:19:29 $
 *  @author    ���<dafo@mogujie.com>
 *  @brief     �ļ��������
 ******************************************************************************/

#ifndef TRANSFERMANAGER_3046D4C9_3E8D_4C36_93B4_092651F5B66F_H__
#define TRANSFERMANAGER_3046D4C9_3E8D_4C36_93B4_092651F5B66F_H__

#include "GlobalDefine.h"
#include "network/Lock.h"
#include <list>
#include <map>

/******************************************************************************/
class TransferFile;
class FileTransferUIThread;

class TransferFileEntity
{
public:
	TransferFileEntity();
public:
	UInt32				nPort;
	UInt32				nFileSize;
	UInt32				nClientMode;			//CLIENT_REALTIME_SENDER = 1,CLIENT_REALTIME_RECVER,CLIENT_OFFLINE_UPLOAD,CLIENT_OFFLINE_DOWNLOAD
	UInt32				nProgress;				//�������
	TransferFile*		pFileObject;			//�����ļ�
	std::string			sFromID;
	std::string         sTaskID;				//�ļ�Ψһ��ʾ��
	std::string         sToID;
	std::string         sFileName;
	std::string			sIP;
	std::string         sPathOfflineFileOnSev;	//�����ļ�����ʱ���������ļ��������ϵ�λ��

	UInt32		        time;            //���ļ�����ʱ��
private:
	CString				sSavePath;				//����ʱ�����Ϊ�ĵ�ַ

public:
	CString getSaveFilePath();					//�����ı����ļ�·��
	CString getSaveFloderFilePath();			//������ļ���λ��
	void setSaveFilePath(const CString& sPath);

	CString getRealFileName();
};

typedef std::map<std::string, TransferFileEntity>     TransferFileMap;//Ⱥ�б�

class TransferFileEntityManager
{
public:
	~TransferFileEntityManager();
	static TransferFileEntityManager* getInstance();
	BOOL pushTransferFileEntity(IN  TransferFileEntity& FileInfo);
	BOOL getFileInfoByTaskId(IN const std::string& sID, OUT TransferFileEntity& FileInfo);
	BOOL DeleteFileInfoByTaskId(IN const std::string& sID);
	void getAllTransferFileEntityFileID(std::list<std::string>& fileIdList);
	BOOL updateFileInfoBysTaskID(IN const TransferFileEntity& info);
	BOOL openFileByFileID(IN const std::string& sID);
	BOOL openFileFolderByTaskID(IN const std::string& sID);
	BOOL kickMapFileItemToVecFile(IN std::string& sfId);
	void pushTransferFileEntityToVec(IN  TransferFileEntity& FileInfo);
	BOOL checkIfIsSending(IN  CString sFilePath);//���ڴ�����ļ������ٴδ���

public:
	/**@name �������file socket��Դ���߳����*/
	//@{
	BOOL startup();
	void shutdown();
	/**
	* ��UI thread���첽�����ļ�����socket
	*
	* @param   std::string & taskId �ļ���������id
	* @return  void
	* @exception there is no any exception to throw.
	*/
	void openFileSocketByTaskId(std::string& taskId);
	/**
	* ��UI thread�йر��ļ�����socket
	*
	* @param   std::string & taskId �ļ���������id
	* @return  void
	* @exception there is no any exception to throw.
	*/
	void closeFileSocketByTaskId(std::string& taskId);
	/**
	* ���ļ�������������ͽ����ļ�����
	*
	* @param   std::string & taskId
	* @return  BOOL
	* @exception there is no any exception to throw.
	*/
	BOOL acceptFileTransfer(const std::string& taskId);//�����ļ�����
	BOOL rejectFileTransfer(const std::string& taskId);//�ܾ��ļ�����
	BOOL cancelFileTransfer(const std::string& taskId);//ȡ���ļ�����
	FileTransferUIThread* getFileTransferThread();
	//@}

private:
	TransferFileEntityManager();

private:
	CLock					m_lock;
	TransferFileMap						m_MapFile;
	std::vector<TransferFileEntity>		m_VecFinishedFile;			//�Ѿ���������ļ�,�κβ��������ļ����ᱻ�ӵ�����
	FileTransferUIThread*				m_fileUIThread;
};
/******************************************************************************/
#endif// TRANSFERMANAGER_3046D4C9_3E8D_4C36_93B4_092651F5B66F_H__
