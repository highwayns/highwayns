<?php

 if(!defined('IN_HIGHWAY'))
 {
 	die('Access Denied!');
 }
//生成支付代码
function get_code($order, $payment)
    {
	global $_CFG;
	if (!is_array($order) ||!is_array($payment))  return false;
        $data_mid  = trim($payment['partnerid']);//商户编号
        $data_oid   = $order['oid'];//订单号
        $data_amount  = intval($order['v_amount']); //支付金额  
        $data_moneytype  = 'CNY';//币种
        $data_key  = trim($payment['ytauthkey']);//MD5密钥
        $data_url = $order['v_url'];//返回url,地址应为绝对路径,带有http协议
		$data_remark1 = $order['remark1'];//备注1
        $MD5KEY =$data_amount.$data_moneytype.$data_oid.$data_mid.$data_url.$data_key;
        $MD5KEY = strtoupper(md5($MD5KEY)); //md5加密拼凑串,注意顺序不能变
        $def_url  = '<form name="E_FORM"  method="post" action="https://pay3.chinabank.com.cn/PayGate" target="_blank">';
        $def_url .= "<input type=HIDDEN name='v_mid' value='".$data_mid."'>";//商户编号
        $def_url .= "<input type=HIDDEN name='v_oid' value='".$data_oid."'>";//订单号
        $def_url .= "<input type=HIDDEN name='v_amount' value='".$data_amount."'>"; //支付金额  
        $def_url .= "<input type=HIDDEN name='v_moneytype'  value='".$data_moneytype."'>";//币种
        $def_url .= "<input type=HIDDEN name='v_url'  value='".$data_url."'>";//返回url,地址应为绝对路径,带有http协议
        $def_url .= "<input type=HIDDEN name='v_md5info' value='".$MD5KEY."'>"; //md5加密拼凑串
        $def_url .= "<input type=HIDDEN name='remark1' value='".$remark1."'>";//备注
        $def_url .= "</form>";
		$def_url .= "<input type=\"button\" name=\"imageField\" class=\"but130lan intrgration_but\" value=\"确认支付\"  onclick=\"document.E_FORM.submit()\"/>";
        return $def_url;
    }
function respond()
{
$payment        = get_payment_info('chinabank');
$v_oid          = trim($_POST['v_oid']);
$v_pmode        = trim($_POST['v_pmode']);
$v_pstatus      = trim($_POST['v_pstatus']);
$v_pstring      = trim($_POST['v_pstring']);
$v_amount       = trim($_POST['v_amount']);
$v_moneytype    = trim($_POST['v_moneytype']);
$remark1        = trim($_POST['remark1']);
$remark2        = trim($_POST['remark2']);
$v_md5str       = trim($_POST['v_md5str']);
$key = $payment['ytauthkey'];

$md5string=strtoupper(md5($v_oid.$v_pstatus.$v_amount.$v_moneytype.$key));
 /* 检查秘钥是否正确 */
if ($v_md5str==$md5string)
{
if ($v_pstatus == '20')
{
/* 改变订单状态 */
if (!order_paid($v_oid)) return false;
return true;
}
}
else
{
return false;
}
}
//获取必须字符
function pay_info()
{
$arr['p_introduction']="銀聯簡単紹介：";
$arr['notes']="銀聯オンライン説明：";
$arr['partnerid']="銀聯オンライン店舗コード：";
$arr['ytauthkey']="銀聯オンラインMD5 キー：";
$arr['fee']="銀聯手続き費用：";
return $arr;
}
?>
