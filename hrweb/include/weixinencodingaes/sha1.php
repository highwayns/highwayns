<?php

include_once "errorCode.php";

/**
 * SHA1 class
 *
 * 璁＄畻鍏紬骞冲彴鐨勬秷鎭鍚嶆帴鍙?
 */
class SHA1
{
	/**
	 * 鐢⊿HA1绠楁硶鐢熸垚瀹夊叏绛惧悕
	 * @param string $token 绁ㄦ嵁
	 * @param string $timestamp 鏃堕棿鎴?	 * @param string $nonce 闅忔満瀛楃涓?	 * @param string $encrypt 瀵嗘枃娑堟伅
	 */
	public function getSHA1($token, $timestamp, $nonce, $encrypt_msg)
	{
		//鎺掑簭
		try {
			$array = array($encrypt_msg, $token, $timestamp, $nonce);
			sort($array, SORT_STRING);
			$str = implode($array);
			return array(ErrorCode::$OK, sha1($str));
		} catch (Exception $e) {
			//print $e . "\n";
			return array(ErrorCode::$ComputeSignatureError, null);
		}
	}

}


?>