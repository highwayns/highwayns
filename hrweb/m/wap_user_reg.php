<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'reg';
$smarty->caching = false;
if ($act == 'reg')
{
$smarty->display("wap/wap_reg.html");
}
elseif ($act=='form')
{
	if ($_CFG['closereg']=='1')WapShowMsg("ウェブ会員登録停止しています，後程試してください！",1);
	if (intval($_GET['type'])==0)WapShowMsg("登録タイプ選択してください！",1);
	if(intval($_GET['type'])>2){
		WapShowMsg("会員タイプ不正，再選択してください！",1);
	}
	$smarty->assign('type',$_GET['type']);
	$captcha=get_cache('captcha');
	$smarty->assign('verify_userreg',$captcha['verify_userreg']);
	$smarty->display('wap/reg_form.html');
}
elseif ($act == 'do_reg')
{
	require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
	require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
	require_once(HIGHWAY_ROOT_PATH.'include/fun_user.php');
	$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
	$username = isset($_POST['username'])?trim($_POST['username']):"";
	$password = isset($_POST['password'])?trim($_POST['password']):"";
	$member_type =intval($_POST['utype']);
	$email = isset($_POST['email'])?trim($_POST['email']):"";
	if (empty($username)||empty($password)||empty($member_type)||empty($email))
	{
	$err="情報不完全";
	}
	elseif (strlen($username)<6 || strlen($username)>18)
	{
	$err="ユーザ名長さは6-18です";
	}
	elseif (strlen($password)<6 || strlen($password)>18)
	{
	$err="パスワード長さ为6-18个字符";
	}
	elseif ($password<>$_POST['password1'])
	{
	$err="入力のパスワードは一致しません";
	}
	elseif (empty($email) || !ereg("^[-a-zA-Z0-9_\.]+\@([0-9A-Za-z][0-9A-Za-z-]+\.)+[A-Za-z]{2,5}$",$email))
	{
	$err="電子メールボックスフォーマットエラー";
	}
	if (get_user_inusername($username))
	{
	$err="ユーザ名既に存在します";
	}
	if (get_user_inemail($email))
	{
	$err="メールボックスが既に存在します";
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
			header("location:".$login_js['hw_login']);
		}
	}
	else
	{
	header("location:wap_user_reg.php");
	}
}
?>
