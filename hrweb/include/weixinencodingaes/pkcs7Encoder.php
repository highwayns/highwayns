<?php

include_once "errorCode.php";

class PKCS7Encoder
{
	public static $block_size = 32;

	function decode($text)
	{

		$pad = ord(substr($text, -1));
		if ($pad < 1 || $pad > 32) {
			$pad = 0;
		}
		return substr($text, 0, (strlen($text) - $pad));
	}

}

class Prpcrypt
{
	public $key;

	function Prpcrypt($k)
	{
		$this->key = base64_decode($k . "=");
	}

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
