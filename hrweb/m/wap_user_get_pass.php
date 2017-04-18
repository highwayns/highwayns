﻿<?php
define('IN_HIGHWAY', true);
$alias="HW_login";
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
unset($dbhost,$dbuser,$dbpass,$dbname);
$smarty->cache = false;
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'enter';
if ($act=='enter')
{
	$smarty->assign('title','找回密码 - '.$_CFG['site_name']);
	$captcha=get_cache('captcha');
	$smarty->assign('verify_getpwd',$captcha['verify_getpwd']);
	$smarty->assign('sms',get_cache('sms_config'));
	$smarty->assign('step',"1");
	$smarty->display('wap/wap-alter-password.html');
}
//找回密码第2步
elseif ($act=='get_pass')
{

	$captcha=get_cache('captcha');
	$postcaptcha = trim($_POST['postcaptcha']);
	$postusername=trim($_POST['username'])?trim($_POST['username']):exit('请填写用户名');
	if (empty($_POST['email']) || !preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$_POST['email']))
	{
	echo '电子邮箱格式错误！';
	}
	require_once(HIGHWAY_ROOT_PATH.'include/fun_user.php');
	$userinfo=get_user_inusername($postusername);
	if (empty($userinfo) || $userinfo['email']<>$_POST['email'])
	{
	echo '用户名或注册邮箱填写错误';
	}
	else
	{
			
			$mailconfig=get_cache('mailconfig');
			$arr['username']=$userinfo['username'];
			$arr['password'] = rand(100000,999999).randstr();
				if (smtp_mail($userinfo['email'],"找回密码","您的新密码为：".$arr['password']))
				{
					$md5password=md5(md5($arr['password']).$userinfo['pwd_hash'].$HW_pwdhash);
					if (!$db->query( "UPDATE ".table('members')." SET password = '$md5password'  WHERE uid='{$userinfo['uid']}'"))
					{
					echo '密码修改失败';
					}
					echo '密码修改成功请查看您的邮箱';
				}
				else
				{
					echo '邮件发送失败，请联系网站管理员';
				}
	}
}
unset($smarty);
?>
