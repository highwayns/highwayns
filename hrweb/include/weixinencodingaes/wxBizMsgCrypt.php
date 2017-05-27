<?php



include_once "sha1.php";
include_once "xmlparse.php";
include_once "pkcs7Encoder.php";
include_once "errorCode.php";

class WXBizMsgCrypt
{
	private $token;
	private $encodingAesKey;
	private $appId;
	
	public function WXBizMsgCrypt($token, $encodingAesKey, $appId)
	{
		$this->token = $token;
		$this->encodingAesKey = $encodingAesKey;
		$this->appId = $appId;
	}
	
	public function encryptMsg($replyMsg, $timeStamp, $nonce, &$encryptMsg)
	{
		$pc = new Prpcrypt($this->encodingAesKey);
		
		//加密
		$array = $pc->encrypt($replyMsg, $this->appId);
		$ret = $array[0];
		if ($ret != 0) {
			return $ret;
		}
		
		if ($timeStamp == null) {
			$timeStamp = time();
		}
		$encrypt = $array[1];
		
		//生成安全签名
		$sha1 = new SHA1;
		$array = $sha1->getSHA1($this->token, $timeStamp, $nonce, $encrypt);
		$ret = $array[0];
		if ($ret != 0) {
			return $ret;
		}
		$signature = $array[1];
		
		//生成发送的xml
		$xmlparse = new XMLParse;
		$encryptMsg = $xmlparse->generate($encrypt, $signature, $timeStamp, $nonce);
		return ErrorCode::$OK;
	}
	
	
	public function decryptMsg($msgSignature, $timestamp = null, $nonce, $postData, &$msg)
	{
		if (strlen($this->encodingAesKey) != 43) {
			return ErrorCode::$IllegalAesKey;
		}
		
		$pc = new Prpcrypt($this->encodingAesKey);
		
		//提取密文
		$xmlparse = new XMLParse;
		$array = $xmlparse->extract($postData);
		$ret = $array[0];
		
		if ($ret != 0) {
			return $ret;
		}
		
		if ($timestamp == null) {
			$timestamp = time();
		}
		
		$encrypt = $array[1];
		$touser_name = $array[2];
		
		//验证安全签名
		$sha1 = new SHA1;
		$array = $sha1->getSHA1($this->token, $timestamp, $nonce, $encrypt);
		$ret = $array[0];
		
		if ($ret != 0) {
			return $ret;
		}
		
		$signature = $array[1];
		if ($signature != $msgSignature) {
			return ErrorCode::$ValidateSignatureError;
		}
		
		$result = $pc->decrypt($encrypt, $this->appId);
		if ($result[0] != 0) {
			return $result[0];
		}
		$msg = $result[1];
		
		return ErrorCode::$OK;
	}
	
}

