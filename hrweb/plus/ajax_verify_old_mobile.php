<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : ''; 
$mobile=trim($_POST['mobile']);
$send_key=trim($_POST['send_key']);
if (empty($send_key) || $send_key<>$_SESSION['send_mobile_key'])
{
exit("效验码错误");
}
$SMSconfig=get_cache('sms_config');
if ($SMSconfig['open']!="1")
{
exit("短信模块处于关闭状态");
}
if ($act=="send_code")
{
		if (empty($mobile) || !preg_match("/^(13|14|15|17|18)\d{9}$/",$mobile))
		{
		exit("手机号错误");
		}
		
		if ($_SESSION['send_time'] && (time()-$_SESSION['send_time'])<180)
		{
		exit("请180秒后再进行验证！");
		}
		$rand=mt_rand(100000, 999999);	
		$r=captcha_send_sms($mobile,"感谢您使用{$_CFG['site_name']}手机认证,验证码为:{$rand}");
		if ($r=="success")
		{
		$_SESSION['mobile_rand']=$rand;
		$_SESSION['send_time']=time();
		$_SESSION['verify_mobile']=$mobile;
		exit("success");
		}
		else
		{
		exit("SMS配置出错，请联系网站管理员");
		}
}
elseif ($act=="verify_code")
{
	$verifycode=trim($_POST['verifycode']);
	if (empty($verifycode) || empty($_SESSION['mobile_rand']) || $verifycode<>$_SESSION['mobile_rand'])
	{
		exit("验证码错误");
	}
	else
	{
			$uid=intval($_SESSION['uid']);
			if (empty($uid))
			{
				exit("系统错误，UID丢失！");
			}
			else
			{
				exit("success");
			}
	}
}
?>
