 /*******************************************************************************
 *  @file      SendImgOperation.h 2014\8\8 9:39:42 $
 *  @author    ���<dafo@mogujie.com>
 *  @brief     
 ******************************************************************************/

#ifndef SENDIMGOPERATION_9D05FD1C_6789_41D1_8F8A_2659456782F4_H__
#define SENDIMGOPERATION_9D05FD1C_6789_41D1_8F8A_2659456782F4_H__

#include "Modules/IHttpPoolModule.h"

class SendImgParam
{
public:
	enum
	{
		SENDIMG_OK = 0,             //���ͳɹ�
		SENDIMG_ERROR_UP,           //�ϴ�����
		SENDIMG_ERROR_NETWORK,      //�������
	};
public:
	CString         csFilePath;
	UInt8           m_result;
	std::string     m_pathUrl;
	std::string		m_relativePathUrl;
	UInt32			width;
	UInt32			height;
};
/******************************************************************************/

/**
 * The class <code>SendImgOperation</code> 
 *
 */
class SendImgHttpOperation : public module::IHttpOperation
{
public:
    /** @name Constructors and Destructor*/

    //@{
    /**
     * Constructor 
     */
	SendImgHttpOperation(SendImgParam& sendImgParam, module::IOperationDelegate callback);
    /**
     * Destructor
     */
    ~SendImgHttpOperation();
    //@}
private:
	BOOL _parseResponse(IN const std::string& body,OUT std::string& pathUrl);

public:
	virtual void processOpertion();
	virtual void release();

private:
	void _getImgSize(UInt32& width,UInt32& height);

private:
	SendImgParam			m_sendImgParam;
};
/******************************************************************************/
#endif// SENDIMGOPERATION_9D05FD1C_6789_41D1_8F8A_2659456782F4_H__
