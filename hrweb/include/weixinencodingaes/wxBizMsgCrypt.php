<?php

/**
 * 瀵瑰叕浼楀钩鍙板彂閫佺粰鍏紬璐﹀彿鐨勬秷鎭姞瑙ｅ瘑绀轰緥浠ｇ爜.
 *
 * @copyright Copyright (c) 1998-2014 Tencent Inc.
 */


include_once "sha1.php";
include_once "xmlparse.php";
include_once "pkcs7Encoder.php";
include_once "errorCode.php";

/**
 * 1.绗笁鏂瑰洖澶嶅姞瀵嗘秷鎭粰鍏紬骞冲彴锛? * 2.绗笁鏂规敹鍒板叕浼楀钩鍙板彂閫佺殑娑堟伅锛岄獙璇佹秷鎭殑瀹夊叏鎬э紝骞跺娑堟伅杩涜瑙ｅ瘑銆? */
class WXBizMsgCrypt
{
	private $token;
	private $encodingAesKey;
	private $appId;

	/**
	 * 鏋勯€犲嚱鏁?	 * @param $token string 鍏紬骞冲彴涓婏紝寮€鍙戣€呰缃殑token
	 * @param $encodingAesKey string 鍏紬骞冲彴涓婏紝寮€鍙戣€呰缃殑EncodingAESKey
	 * @param $appId string 鍏紬骞冲彴鐨刟ppId
	 */
	public function WXBizMsgCrypt($token, $encodingAesKey, $appId)
	{
		$this->token = $token;
		$this->encodingAesKey = $encodingAesKey;
		$this->appId = $appId;
	}

	/**
	 * 灏嗗叕浼楀钩鍙板洖澶嶇敤鎴风殑娑堟伅鍔犲瘑鎵撳寘.
	 * <ol>
	 *    <li>瀵硅鍙戦€佺殑娑堟伅杩涜AES-CBC鍔犲瘑</li>
	 *    <li>鐢熸垚瀹夊叏绛惧悕</li>
	 *    <li>灏嗘秷鎭瘑鏂囧拰瀹夊叏绛惧悕鎵撳寘鎴恱ml鏍煎紡</li>
	 * </ol>
	 *
	 * @param $replyMsg string 鍏紬骞冲彴寰呭洖澶嶇敤鎴风殑娑堟伅锛寈ml鏍煎紡鐨勫瓧绗︿覆
	 * @param $timeStamp string 鏃堕棿鎴筹紝鍙互鑷繁鐢熸垚锛屼篃鍙互鐢║RL鍙傛暟鐨則imestamp
	 * @param $nonce string 闅忔満涓诧紝鍙互鑷繁鐢熸垚锛屼篃鍙互鐢║RL鍙傛暟鐨刵once
	 * @param &$encryptMsg string 鍔犲瘑鍚庣殑鍙互鐩存帴鍥炲鐢ㄦ埛鐨勫瘑鏂囷紝鍖呮嫭msg_signature, timestamp, nonce, encrypt鐨剎ml鏍煎紡鐨勫瓧绗︿覆,
	 *                      褰搑eturn杩斿洖0鏃舵湁鏁?	 *
	 * @return int 鎴愬姛0锛屽け璐ヨ繑鍥炲搴旂殑閿欒鐮?	 */
	public function encryptMsg($replyMsg, $timeStamp, $nonce, &$encryptMsg)
	{
		$pc = new Prpcrypt($this->encodingAesKey);

		//鍔犲瘑
		$array = $pc->encrypt($replyMsg, $this->appId);
		$ret = $array[0];
		if ($ret != 0) {
			return $ret;
		}

		if ($timeStamp == null) {
			$timeStamp = time();
		}
		$encrypt = $array[1];

		//鐢熸垚瀹夊叏绛惧悕
		$sha1 = new SHA1;
		$array = $sha1->getSHA1($this->token, $timeStamp, $nonce, $encrypt);
		$ret = $array[0];
		if ($ret != 0) {
			return $ret;
		}
		$signature = $array[1];

		//鐢熸垚鍙戦€佺殑xml
		$xmlparse = new XMLParse;
		$encryptMsg = $xmlparse->generate($encrypt, $signature, $timeStamp, $nonce);
		return ErrorCode::$OK;
	}


	/**
	 * 妫€楠屾秷鎭殑鐪熷疄鎬э紝骞朵笖鑾峰彇瑙ｅ瘑鍚庣殑鏄庢枃.
	 * <ol>
	 *    <li>鍒╃敤鏀跺埌鐨勫瘑鏂囩敓鎴愬畨鍏ㄧ鍚嶏紝杩涜绛惧悕楠岃瘉</li>
	 *    <li>鑻ラ獙璇侀€氳繃锛屽垯鎻愬彇xml涓殑鍔犲瘑娑堟伅</li>
	 *    <li>瀵规秷鎭繘琛岃В瀵?/li>
	 * </ol>
	 *
	 * @param $msgSignature string 绛惧悕涓诧紝瀵瑰簲URL鍙傛暟鐨刴sg_signature
	 * @param $timestamp string 鏃堕棿鎴?瀵瑰簲URL鍙傛暟鐨則imestamp
	 * @param $nonce string 闅忔満涓诧紝瀵瑰簲URL鍙傛暟鐨刵once
	 * @param $postData string 瀵嗘枃锛屽搴擯OST璇锋眰鐨勬暟鎹?	 * @param &$msg string 瑙ｅ瘑鍚庣殑鍘熸枃锛屽綋return杩斿洖0鏃舵湁鏁?	 *
	 * @return int 鎴愬姛0锛屽け璐ヨ繑鍥炲搴旂殑閿欒鐮?	 */
	public function decryptMsg($msgSignature, $timestamp = null, $nonce, $postData, &$msg)
	{
		if (strlen($this->encodingAesKey) != 43) {
			return ErrorCode::$IllegalAesKey;
		}

		$pc = new Prpcrypt($this->encodingAesKey);

		//鎻愬彇瀵嗘枃
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

		//楠岃瘉瀹夊叏绛惧悕
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

