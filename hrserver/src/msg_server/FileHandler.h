/*
 * FileHandler.h
 *
 *  Created on: 2013-12-17
 *      Author: ziteng@mogujie.com
 */

#ifndef FILEHANDLER_H_
#define FILEHANDLER_H_

#include "ImPduBase.h"
#include "MsgConn.h"

class CMsgConn;

class CFileHandler
{
public:
	virtual ~CFileHandler() {}

	static CFileHandler* getInstance();

    void HandleClientFileRequest(SpCMsgConn pMsgConn, CImPdu* pPdu);
    void HandleClientFileHasOfflineReq(SpCMsgConn pMsgConn, CImPdu* pPdu);
    void HandleClientFileAddOfflineReq(SpCMsgConn pMsgConn, CImPdu* pPdu);
    void HandleClientFileDelOfflineReq(SpCMsgConn pMsgConn, CImPdu* pPdu);
    void HandleFileHasOfflineRes(CImPdu* pPdu);
    void HandleFileNotify(CImPdu* pPdu);
private:
	CFileHandler() {}

private:
	static CFileHandler* s_handler_instance;
};


#endif /* FILEHANDLER_H_ */
