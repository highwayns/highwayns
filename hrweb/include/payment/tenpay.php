<?php
/**
 * 74cms 财付通支付插件
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/

 if(!defined('IN_QISHI'))
 {
 	die('Access Denied!');
 }
//生成支付代码
function get_code($order, $payment)
    {
	global $_CFG;
	if (!is_array($order) ||!is_array($payment))  return false;
	$bargainor_id = trim($payment['partnerid']);////商户编号
	$key=trim($payment['ytauthkey']);//MD5密钥
	$return_url=$order['v_url'];//返回url,地址应为绝对路径,带有http协议
	//date_default_timezone_set(PRC);
    $strDate = date("Ymd");
    $strTime = date("His");
    $randNum = rand(1000, 9999);//4位随机数
    $strReq = $strTime . $randNum;//10位序列号,可以自行调整。	
    $transaction_id = $bargainor_id . $strDate . $strReq;/* 财付通交易单号，规则为：10位商户号+8位时间（YYYYmmdd)+10位流水号 */
	$sp_billno = $order['oid'];//订单号 商家订单号,长度若超过32位，取前32位。财付通只记录商家订单号，不保证唯一。
	//$total_fee ="1";/* 商品价格（包含运费），以分为单位 */
	$total_fee =intval($order['v_amount'])*100;/* 商品价格（包含运费），以分为单位 */
	$desc = "订单号：" . $transaction_id;
	/* 创建支付请求对象 */
    $reqHandler = new PayRequestHandler();
    $reqHandler->init();
    $reqHandler->setKey($key);
	$reqHandler->setParameter("bargainor_id", $bargainor_id);			//商户号
	$reqHandler->setParameter("sp_billno", $sp_billno);					//商户订单号
	$reqHandler->setParameter("transaction_id", $transaction_id);		//财付通交易单号
	$reqHandler->setParameter("total_fee", $total_fee);					//商品总金额,以分为单位
	$reqHandler->setParameter("return_url", $return_url);				//返回处理地址
	$reqHandler->setParameter("desc", $desc);	//商品名称
	$reqHandler->setParameter("spbill_create_ip", $_SERVER['REMOTE_ADDR']);
	$reqUrl = $reqHandler->getRequestURL();
	$def_url  ="<a href=\"".$reqUrl."\" target=\"_blank\"><img src=\"".$_CFG['site_template']."images/25.gif\" border=\"0\"/></a>";
	$def_url  ="<input type=\"button\" class=\"but130lan intrgration_but\" value=\"确认支付\"  onclick=\"javascript:window.open('".$reqUrl."')\"/>";
	 return $def_url;
    }
/**
 * 响应操作
*/
function respond()
{
$payment= get_payment_info('tenpay');
$key = $payment['ytauthkey'];
/* 创建支付应答对象 */
$resHandler = new PayResponseHandler();
$resHandler->setKey($key);
	if($resHandler->isTenpaySign())
	{
	//商户单号
	$sp_billno = $resHandler->getParameter("sp_billno");
	//财付通交易单号
	$transaction_id = $resHandler->getParameter("transaction_id");
	//金额,以分为单位
	$total_fee = $resHandler->getParameter("total_fee");
	$pay_result = $resHandler->getParameter("pay_result");
		if( "0" == $pay_result ) 
		{
		return order_paid($sp_billno);
		}
		else
		{
		return false;
		}
	}
	else
	{
	return false;
	}
}
function pay_info()
{
$arr['p_introduction']="财付通简短描述：";
$arr['notes']="财付通详细描述：";
$arr['partnerid']="财付通商户编号：";
$arr['ytauthkey']="财付通MD5 密钥：";
$arr['fee']="财付通交易手续费：";
return $arr;
}
//----------------------------------------------------
//财付通自带class
class PayRequestHandler extends RequestHandler {
	
	function __construct() {
		$this->PayRequestHandler();
	}
	
	function PayRequestHandler() {
		//默认支付网关地址
		$this->setGateURL("https://www.tenpay.com/cgi-bin/v1.0/pay_gate.cgi");	
	}
	
	/**
	*@Override
	*初始化函数，默认给一些参数赋值，如cmdno,date等。
	*/
	function init() {
		//任务代码
		$this->setParameter("cmdno", "1");
		
		//日期
		$this->setParameter("date",  date("Ymd"));
		
		//商户号
		$this->setParameter("bargainor_id", "");
		
		//财付通交易单号
		$this->setParameter("transaction_id", "");
		
		//商家订单号
		$this->setParameter("sp_billno", "");
		
		//商品价格，以分为单位
		$this->setParameter("total_fee", "");
		
		//货币类型
		$this->setParameter("fee_type",  "1");
		
		//返回url
		$this->setParameter("return_url",  "");
		
		//自定义参数
		$this->setParameter("attach",  "");
		
		//用户ip
		$this->setParameter("spbill_create_ip",  "");
		
		//商品名称
		$this->setParameter("desc",  "");
		
		//银行编码
		$this->setParameter("bank_type",  "0");
		
		//字符集编码
		$this->setParameter("cs",  "gbk");
		
		//摘要
		$this->setParameter("sign",  "");
		
	}
	
	/**
	*@Override
	*创建签名
	*/
	function createSign() {
		$cmdno = $this->getParameter("cmdno");
		$date = $this->getParameter("date");
		$bargainor_id = $this->getParameter("bargainor_id");
		$transaction_id = $this->getParameter("transaction_id");
		$sp_billno = $this->getParameter("sp_billno");
		$total_fee = $this->getParameter("total_fee");
		$fee_type = $this->getParameter("fee_type");
		$return_url = $this->getParameter("return_url");
		$attach = $this->getParameter("attach");
		$spbill_create_ip = $this->getParameter("spbill_create_ip");
		$key = $this->getKey();
		
		$signPars = "cmdno=" . $cmdno . "&" .
				"date=" . $date . "&" .
				"bargainor_id=" . $bargainor_id . "&" .
				"transaction_id=" . $transaction_id . "&" .
				"sp_billno=" . $sp_billno . "&" .
				"total_fee=" . $total_fee . "&" .
				"fee_type=" . $fee_type . "&" .
				"return_url=" . $return_url . "&" .
				"attach=" . $attach . "&";
		
		if($spbill_create_ip != "") {
			$signPars .= "spbill_create_ip=" . $spbill_create_ip . "&";
		}
		
		$signPars .= "key=" . $key;
		
		$sign = strtolower(md5($signPars));
		
		$this->setParameter("sign", $sign);
		
		//debug信息
		$this->_setDebugInfo($signPars . " => sign:" . $sign);
		
	}

}
class PayResponseHandler extends ResponseHandler {
	
	/**
	*@Override
	*/
	function isTenpaySign() {
		$cmdno = $this->getParameter("cmdno");
		$pay_result = $this->getParameter("pay_result");
		$date = $this->getParameter("date");
		$transaction_id = $this->getParameter("transaction_id");
		$sp_billno = $this->getParameter("sp_billno");
		$total_fee = $this->getParameter("total_fee");		
		$fee_type = $this->getParameter("fee_type");
		$attach = $this->getParameter("attach");
		$key = $this->getKey();
		
		$signPars = "";
		//组织签名串
		$signPars = "cmdno=" . $cmdno . "&" .
				"pay_result=" . $pay_result . "&" .
				"date=" . $date . "&" .
				"transaction_id=" . $transaction_id . "&" .
				"sp_billno=" . $sp_billno . "&" .
				"total_fee=" . $total_fee . "&" .
				"fee_type=" . $fee_type . "&" .
				"attach=" . $attach . "&" .
				"key=" . $key;
				
		$sign = strtolower(md5($signPars));
		
		$tenpaySign = strtolower($this->getParameter("sign"));
		
		//debug信息
		$this->_setDebugInfo($signPars . " => sign:" . $sign .
				" tenpaySign:" . $this->getParameter("sign"));
		
		return $sign == $tenpaySign;
		
	}
	
}
class RequestHandler {
	
	/** 网关url地址 */
	var $gateUrl;
	
	/** 密钥 */
	var $key;
	
	/** 请求的参数 */
	var $parameters;
	
	/** debug信息 */
	var $debugInfo;
	
	function __construct() {
		$this->RequestHandler();
	}
	
	function RequestHandler() {
		$this->gateUrl = "https://www.tenpay.com/cgi-bin/v1.0/pay_gate.cgi";
		$this->key = "";
		$this->parameters = array();
		$this->debugInfo = "";
	}
	
	/**
	*初始化函数。
	*/
	function init() {
		//nothing to do
	}
	
	/**
	*获取入口地址,不包含参数值
	*/
	function getGateURL() {
		return $this->gateUrl;
	}
	
	/**
	*设置入口地址,不包含参数值
	*/
	function setGateURL($gateUrl) {
		$this->gateUrl = $gateUrl;
	}
	
	/**
	*获取密钥
	*/
	function getKey() {
		return $this->key;
	}
	
	/**
	*设置密钥
	*/
	function setKey($key) {
		$this->key = $key;
	}
	
	/**
	*获取参数值
	*/
	function getParameter($parameter) {
		return $this->parameters[$parameter];
	}
	
	/**
	*设置参数值
	*/
	function setParameter($parameter, $parameterValue) {
		$this->parameters[$parameter] = $parameterValue;
	}
	
	/**
	*获取所有请求的参数
	*@return array
	*/
	function getAllParameters() {
		return $this->parameters;
	}
	
	/**
	*获取带参数的请求URL
	*/
	function getRequestURL() {
	
		$this->createSign();
		
		$reqPar = "";
		ksort($this->parameters);
		foreach($this->parameters as $k => $v) {
			$reqPar .= $k . "=" . urlencode($v) . "&";
		}
		
		//去掉最后一个&
		$reqPar = substr($reqPar, 0, strlen($reqPar)-1);
		
		$requestURL = $this->getGateURL() . "?" . $reqPar;
		
		return $requestURL;
		
	}
		
	/**
	*获取debug信息
	*/
	function getDebugInfo() {
		return $this->debugInfo;
	}
	
	/**
	*重定向到财付通支付
	*/
	function doSend() {
		header("Location:" . $this->getRequestURL());
		exit;
	}
	
	/**
	*创建md5摘要,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。
	*/
	function createSign() {
		$signPars = "";
		ksort($this->parameters);
		foreach($this->parameters as $k => $v) {
			if("" != $v && "sign" != $k) {
				$signPars .= $k . "=" . $v . "&";
			}
		}
		$signPars .= "key=" . $this->getKey();
		
		$sign = strtolower(md5($signPars));
		
		$this->setParameter("sign", $sign);
		
		//debug信息
		$this->_setDebugInfo($signPars . " => sign:" . $sign);
		
	}	
	
	/**
	*设置debug信息
	*/
	function _setDebugInfo($debugInfo) {
		$this->debugInfo = $debugInfo;
	}

}
class ResponseHandler  {
	
	/** 密钥 */
	var $key;
	
	/** 应答的参数 */
	var $parameters;
	
	/** debug信息 */
	var $debugInfo;
	
	function __construct() {
		$this->ResponseHandler();
	}
	
	function ResponseHandler() {
		$this->key = "";
		$this->parameters = array();
		$this->debugInfo = "";
		
		/* GET */
		foreach($_GET as $k => $v) {
			$this->setParameter($k, $v);
		}
		/* POST */
		foreach($_POST as $k => $v) {
			$this->setParameter($k, $v);
		}
	}
		
	/**
	*获取密钥
	*/
	function getKey() {
		return $this->key;
	}
	
	/**
	*设置密钥
	*/	
	function setKey($key) {
		$this->key = $key;
	}
	
	/**
	*获取参数值
	*/	
	function getParameter($parameter) {
		return $this->parameters[$parameter];
	}
	
	/**
	*设置参数值
	*/	
	function setParameter($parameter, $parameterValue) {
		$this->parameters[$parameter] = $parameterValue;
	}
	
	/**
	*获取所有请求的参数
	*@return array
	*/
	function getAllParameters() {
		return $this->parameters;
	}	
	
	/**
	*是否财付通签名,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。
	*true:是
	*false:否
	*/	
	function isTenpaySign() {
		$signPars = "";
		ksort($this->parameters);
		foreach($this->parameters as $k => $v) {
			if("sign" != $k && "" != $v) {
				$signPars .= $k . "=" . $v . "&";
			}
		}
		$signPars .= "key=" . $this->getKey();
		
		$sign = strtolower(md5($signPars));
		
		$tenpaySign = strtolower($this->getParameter("sign"));
				
		//debug信息
		$this->_setDebugInfo($signPars . " => sign:" . $sign .
				" tenpaySign:" . $this->getParameter("sign"));
		
		return $sign == $tenpaySign;
		
	}
	
	/**
	*获取debug信息
	*/	
	function getDebugInfo() {
		return $this->debugInfo;
	}
	
	/**
	*显示处理结果。
	*@param $show_url 显示处理结果的url地址,绝对url地址的形式(http://www.xxx.com/xxx.php)。
	*/	
	function doShow($show_url) {
		$strHtml = "<html><head>\r\n" .
			"<meta name=\"TENCENT_ONLINE_PAYMENT\" content=\"China TENCENT\">" .
			"<script language=\"javascript\">\r\n" .
				"window.location.href='" . $show_url . "';\r\n" .
			"</script>\r\n" .
			"</head><body></body></html>";
			
		echo $strHtml;
		
		exit;
	}
	
	/**
	 * 是否财付通签名
	 * @param signParameterArray 签名的参数数组
	 * @return boolean
	 */	
	function _isTenpaySign($signParameterArray) {
	
		$signPars = "";
		foreach($signParameterArray as $k) {
			$v = $this->getParameter($k);
			if("sign" != $k && "" != $v) {
				$signPars .= $k . "=" . $v . "&";
			}			
		}
		$signPars .= "key=" . $this->getKey();
		
		$sign = strtolower(md5($signPars));
		
		$tenpaySign = strtolower($this->getParameter("sign"));
				
		//debug信息
		$this->_setDebugInfo($signPars . " => sign:" . $sign .
				" tenpaySign:" . $this->getParameter("sign"));
		
		return $sign == $tenpaySign;		
		
	
	}
	
	/**
	*设置debug信息
	*/	
	function _setDebugInfo($debugInfo) {
		$this->debugInfo = $debugInfo;
	}
	
}
?>