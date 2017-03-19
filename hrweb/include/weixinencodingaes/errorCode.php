<?php

/**
 * error code 璇存槑.
 * <ul>
 *    <li>-40001: 绛惧悕楠岃瘉閿欒</li>
 *    <li>-40002: xml瑙ｆ瀽澶辫触</li>
 *    <li>-40003: sha鍔犲瘑鐢熸垚绛惧悕澶辫触</li>
 *    <li>-40004: encodingAesKey 闈炴硶</li>
 *    <li>-40005: appid 鏍￠獙閿欒</li>
 *    <li>-40006: aes 鍔犲瘑澶辫触</li>
 *    <li>-40007: aes 瑙ｅ瘑澶辫触</li>
 *    <li>-40008: 瑙ｅ瘑鍚庡緱鍒扮殑buffer闈炴硶</li>
 *    <li>-40009: base64鍔犲瘑澶辫触</li>
 *    <li>-40010: base64瑙ｅ瘑澶辫触</li>
 *    <li>-40011: 鐢熸垚xml澶辫触</li>
 * </ul>
 */
class ErrorCode
{
	public static $OK = 0;
	public static $ValidateSignatureError = -40001;
	public static $ParseXmlError = -40002;
	public static $ComputeSignatureError = -40003;
	public static $IllegalAesKey = -40004;
	public static $ValidateAppidError = -40005;
	public static $EncryptAESError = -40006;
	public static $DecryptAESError = -40007;
	public static $IllegalBuffer = -40008;
	public static $EncodeBase64Error = -40009;
	public static $DecodeBase64Error = -40010;
	public static $GenReturnXmlError = -40011;
}

?>