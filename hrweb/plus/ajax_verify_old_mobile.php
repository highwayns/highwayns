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
exit("検証コードエラー");
}
$SMSconfig=get_cache('sms_config');
if ($SMSconfig['open']!="1")
{
exit("ショートメッセージモジュール無効");
}
if ($act=="send_code")
{
		if (empty($mobile) || !preg_match("/^(13|14|15|17|18)\d{9}$/",$mobile))
		{
		exit("携帯番号エラー");
		}
		
		if ($_SESSION['send_time'] && (time()-$_SESSION['send_time'])<180)
		{
		exit("180秒後再検証してください！");
		}
		$rand=mt_rand(100000, 999999);	
		$r=captcha_send_sms($mobile,"{$_CFG['site_name']}携帯認定有難うございます,検証コードは:{$rand}");
		if ($r=="success")
		{
		$_SESSION['mobile_rand']=$rand;
		$_SESSION['send_time']=time();
		$_SESSION['verify_mobile']=$mobile;
		exit("success");
		}
		else
		{
		exit("SMS配置エラー，ウェブ管理者に連絡");
		}
}
elseif ($act=="verify_code")
{
	$verifycode=trim($_POST['verifycode']);
	if (empty($verifycode) || empty($_SESSION['mobile_rand']) || $verifycode<>$_SESSION['mobile_rand'])
	{
		exit("確認コードエラー");
	}
	else
	{
			$uid=intval($_SESSION['uid']);
			if (empty($uid))
			{
				exit("システムエラー，UID失った！");
			}
			else
			{
				exit("success");
			}
	}
}
?>
