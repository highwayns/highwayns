<?php

include_once "errorCode.php";

/**
 * PKCS7Encoder class
 *
 * 鎻愪緵鍩轰簬PKCS7绠楁硶鐨勫姞瑙ｅ瘑鎺ュ彛.
 */
class PKCS7Encoder
{
	public static $block_size = 32;

	/**
	 * 瀵归渶瑕佸姞瀵嗙殑鏄庢枃杩涜濉厖琛ヤ綅
	 * @param $text 闇€瑕佽繘琛屽～鍏呰ˉ浣嶆搷浣滅殑鏄庢枃
	 * @return 琛ラ綈鏄庢枃瀛楃涓?	 */
	function encode($text)
	{
		$block_size = PKCS7Encoder::$block_size;
		$text_length = strlen($text);
		//璁＄畻闇€瑕佸～鍏呯殑浣嶆暟
		$amount_to_pad = PKCS7Encoder::$block_size - ($text_length % PKCS7Encoder::$block_size);
		if ($amount_to_pad == 0) {
			$amount_to_pad = PKCS7Encoder::block_size;
		}
		//鑾峰緱琛ヤ綅鎵€鐢ㄧ殑瀛楃
		$pad_chr = chr($amount_to_pad);
		$tmp = "";
		for ($index = 0; $index < $amount_to_pad; $index++) {
			$tmp .= $pad_chr;
		}
		return $text . $tmp;
	}

	/**
	 * 瀵硅В瀵嗗悗鐨勬槑鏂囪繘琛岃ˉ浣嶅垹闄?	 * @param decrypted 瑙ｅ瘑鍚庣殑鏄庢枃
	 * @return 鍒犻櫎濉厖琛ヤ綅鍚庣殑鏄庢枃
	 */
	function decode($text)
	{

		$pad = ord(substr($text, -1));
		if ($pad < 1 || $pad > 32) {
			$pad = 0;
		}
		return substr($text, 0, (strlen($text) - $pad));
	}

}

/**
 * Prpcrypt class
 *
 * 鎻愪緵鎺ユ敹鍜屾帹閫佺粰鍏紬骞冲彴娑堟伅鐨勫姞瑙ｅ瘑鎺ュ彛.
 */
class Prpcrypt
{
	public $key;

	function Prpcrypt($k)
	{
		$this->key = base64_decode($k . "=");
	}

	/**
	 * 瀵规槑鏂囪繘琛屽姞瀵?	 * @param string $text 闇€瑕佸姞瀵嗙殑鏄庢枃
	 * @return string 鍔犲瘑鍚庣殑瀵嗘枃
	 */
	public function encrypt($text, $appid)
	{

		try {
			//鑾峰緱16浣嶉殢鏈哄瓧绗︿覆锛屽～鍏呭埌鏄庢枃涔嬪墠
			$random = $this->getRandomStr();
			$text = $random . pack("N", strlen($text)) . $text . $appid;
			// 缃戠粶瀛楄妭搴?			$size = mcrypt_get_block_size(MCRYPT_RIJNDAEL_128, MCRYPT_MODE_CBC);
			$module = mcrypt_module_open(MCRYPT_RIJNDAEL_128, '', MCRYPT_MODE_CBC, '');
			$iv = substr($this->key, 0, 16);
			//浣跨敤鑷畾涔夌殑濉厖鏂瑰紡瀵规槑鏂囪繘琛岃ˉ浣嶅～鍏?			$pkc_encoder = new PKCS7Encoder;
			$text = $pkc_encoder->encode($text);
			mcrypt_generic_init($module, $this->key, $iv);
			//鍔犲瘑
			$encrypted = mcrypt_generic($module, $text);
			mcrypt_generic_deinit($module);
			mcrypt_module_close($module);

			//print(base64_encode($encrypted));
			//浣跨敤BASE64瀵瑰姞瀵嗗悗鐨勫瓧绗︿覆杩涜缂栫爜
			return array(ErrorCode::$OK, base64_encode($encrypted));
		} catch (Exception $e) {
			//print $e;
			return array(ErrorCode::$EncryptAESError, null);
		}
	}

	/**
	 * 瀵瑰瘑鏂囪繘琛岃В瀵?	 * @param string $encrypted 闇€瑕佽В瀵嗙殑瀵嗘枃
	 * @return string 瑙ｅ瘑寰楀埌鐨勬槑鏂?	 */
	public function decrypt($encrypted, $appid)
	{

		try {
			//浣跨敤BASE64瀵归渶瑕佽В瀵嗙殑瀛楃涓茶繘琛岃В鐮?			$ciphertext_dec = base64_decode($encrypted);
			$module = mcrypt_module_open(MCRYPT_RIJNDAEL_128, '', MCRYPT_MODE_CBC, '');
			$iv = substr($this->key, 0, 16);
			mcrypt_generic_init($module, $this->key, $iv);

			//瑙ｅ瘑
			$decrypted = mdecrypt_generic($module, $ciphertext_dec);
			mcrypt_generic_deinit($module);
			mcrypt_module_close($module);
		} catch (Exception $e) {
			return array(ErrorCode::$DecryptAESError, null);
		}


		try {
			//鍘婚櫎琛ヤ綅瀛楃
			$pkc_encoder = new PKCS7Encoder;
			$result = $pkc_encoder->decode($decrypted);
			//鍘婚櫎16浣嶉殢鏈哄瓧绗︿覆,缃戠粶瀛楄妭搴忓拰AppId
			if (strlen($result) < 16)
				return "";
			$content = substr($result, 16, strlen($result));
			$len_list = unpack("N", substr($content, 0, 4));
			$xml_len = $len_list[1];
			$xml_content = substr($content, 4, $xml_len);
			$from_appid = substr($content, $xml_len + 4);
		} catch (Exception $e) {
			//print $e;
			return array(ErrorCode::$IllegalBuffer, null);
		}
		if ($from_appid != $appid)
			return array(ErrorCode::$ValidateAppidError, null);
		return array(0, $xml_content);

	}


	/**
	 * 闅忔満鐢熸垚16浣嶅瓧绗︿覆
	 * @return string 鐢熸垚鐨勫瓧绗︿覆
	 */
	function getRandomStr()
	{

		$str = "";
		$str_pol = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
		$max = strlen($str_pol) - 1;
		for ($i = 0; $i < 16; $i++) {
			$str .= $str_pol[mt_rand(0, $max)];
		}
		return $str;
	}

}

?>