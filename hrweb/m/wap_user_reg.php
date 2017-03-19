<?php
 /*
 * 74cms WAP
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/fun_wap.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'reg';
$smarty->caching = false;
if ($act == 'reg')
{
$smarty->display("wap/wap_reg.html");
}
elseif ($act=='form')
{
	if ($_CFG['closereg']=='1')WapShowMsg("网站暂停会员注册，请稍后再次尝试！",1);
	if (intval($_GET['type'])==0)WapShowMsg("请选择注册类型！",1);
	if(intval($_GET['type'])>2){
		WapShowMsg("会员类型不正确，请重新选择！",1);
	}
	$smarty->assign('type',$_GET['type']);
	$captcha=get_cache('captcha');
	$smarty->assign('verify_userreg',$captcha['verify_userreg']);
	$smarty->display('wap/reg_form.html');
}
elseif ($act == 'do_reg')
{
	require_once(QISHI_ROOT_PATH.'include/fun_wap.php');
	require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
	$username = isset($_POST['username'])?trim($_POST['username']):"";
	$password = isset($_POST['password'])?trim($_POST['password']):"";
	$member_type =intval($_POST['utype']);
	$email = isset($_POST['email'])?trim($_POST['email']):"";
	if (empty($username)||empty($password)||empty($member_type)||empty($email))
	{
	$err="信息不完整";
	}
	elseif (strlen($username)<6 || strlen($username)>18)
	{
	$err="用户名长度为6-18个字符";
	}
	elseif (strlen($password)<6 || strlen($password)>18)
	{
	$err="密码长度为6-18个字符";
	}
	elseif ($password<>$_POST['password1'])
	{
	$err="两次输入的密码不同";
	}
	elseif (empty($email) || !ereg("^[-a-zA-Z0-9_\.]+\@([0-9A-Za-z][0-9A-Za-z-]+\.)+[A-Za-z]{2,5}$",$email))
	{
	$err="电子邮箱格式错误";
	}
	if (get_user_inusername($username))
	{
	$err="用户名已经存在";
	}
	if (get_user_inemail($email))
	{
	$err="电子邮箱已经存在";
	}	
	if ($err)
	{
	$smarty->assign('err',$err);
	$smarty->assign('type',$member_type);
	$smarty->display("wap/reg_form.html");
	exit();
	}	
	$register=user_register(3,$password,$member_type,$email,$mobile="",true,$username,"");
	if ($register>0)
	{
		$login_js=wap_user_login($username,$password);
		$mailconfig=get_cache('mailconfig');
		if ($mailconfig['set_reg']=="1")
		{
		dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid=".$_SESSION['uid']."&key=".asyn_userkey($_SESSION['uid'])."&sendemail=".$email."&sendusername=".$username."&sendpassword=".$password."&act=reg");
		}
		if ($login_js)
		{
			header("location:".$login_js['qs_login']);
		}
	}
	else
	{
	header("location:wap_user_reg.php");
	}
}
?>