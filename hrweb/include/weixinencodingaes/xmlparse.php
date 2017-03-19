<?php
include_once "errorCode.php";

/**
 * XMLParse class
 *
 * 鎻愪緵鎻愬彇娑堟伅鏍煎紡涓殑瀵嗘枃鍙婄敓鎴愬洖澶嶆秷鎭牸寮忕殑鎺ュ彛.
 */
class XMLParse
{

	/**
	 * 鎻愬彇鍑簒ml鏁版嵁鍖呬腑鐨勫姞瀵嗘秷鎭?	 * @param string $xmltext 寰呮彁鍙栫殑xml瀛楃涓?	 * @return string 鎻愬彇鍑虹殑鍔犲瘑娑堟伅瀛楃涓?	 */
	public function extract($xmltext)
	{
		try {
			$xml = new DOMDocument();
			$xml->loadXML($xmltext);
			$array_e = $xml->getElementsByTagName('Encrypt');
			$array_a = $xml->getElementsByTagName('ToUserName');
			$encrypt = $array_e->item(0)->nodeValue;
			$tousername = $array_a->item(0)->nodeValue;
			return array(0, $encrypt, $tousername);
		} catch (Exception $e) {
			//print $e . "\n";
			return array(ErrorCode::$ParseXmlError, null, null);
		}
	}

	/**
	 * 鐢熸垚xml娑堟伅
	 * @param string $encrypt 鍔犲瘑鍚庣殑娑堟伅瀵嗘枃
	 * @param string $signature 瀹夊叏绛惧悕
	 * @param string $timestamp 鏃堕棿鎴?	 * @param string $nonce 闅忔満瀛楃涓?	 */
	public function generate($encrypt, $signature, $timestamp, $nonce)
	{
		$format = "<xml>
<Encrypt><![CDATA[%s]]></Encrypt>
<MsgSignature><![CDATA[%s]]></MsgSignature>
<TimeStamp>%s</TimeStamp>
<Nonce><![CDATA[%s]]></Nonce>
</xml>";
		return sprintf($format, $encrypt, $signature, $timestamp, $nonce);
	}

}


?>