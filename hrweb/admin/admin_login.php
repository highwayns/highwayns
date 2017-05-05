<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'login';
if($act == 'logout')
{
	unset($_SESSION['admin_id']);
	unset($_SESSION['admin_name']);
	unset($_SESSION['admin_purview']);
	setcookie('Qishi[admin_id]',"",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie('Qishi[admin_name]',"",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie('Qishi[admin_pwd]',"",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	header("Location:?act=login");
}
elseif($act == 'login')
{
	header("Expires: Mon, 26 Jul 1997 05:00:00 GMT");
	header("Cache-Control: no-cache, must-revalidate");
	header("Pragma: no-cache");
	if(isset($_SESSION['admin_id'],$_SESSION['admin_name'],$_SESSION['admin_purview']))
	{
	header("Location: admin_index.php"); 
	}
	$smarty->assign('random',mt_rand());
	$smarty->assign("HIGHWAY_VERSION",HIGHWAY_VERSION);
	get_token();
	$captcha=get_cache('captcha');
	$smarty->assign('verify_adminlogin',$captcha['verify_adminlogin']);
	$smarty->display('sys/admin_login.htm');
}
elseif($act == 'do_login')
{
	header("Expires: Mon, 26 Jul 1997 05:00:00 GMT");
	header("Cache-Control: no-cache, must-revalidate");
	header("Pragma: no-cache");
 	$admin_name = isset($_POST['admin_name']) ? trim($_POST['admin_name']) : '';
 	$admin_pwd = isset($_POST['admin_pwd']) ? trim($_POST['admin_pwd']) : '';
	$postcaptcha = isset($_POST['postcaptcha']) ? $_POST['postcaptcha'] : '';
 	$remember = isset($_POST['rememberme']) ? intval($_POST['rememberme']) : 0;
 	if($admin_name == '')
	{
	header("Location:?act=login&err=".urlencode('ユーザ名を入力しません'));
	exit();
 	}
 	elseif($admin_pwd == '')
	{
	header("Location:?act=login&err=".urlencode('パスワードを入力してください'));
	exit();
 	}
	$captcha=get_cache('captcha');
	if(empty($postcaptcha) && $captcha['verify_adminlogin']=='1')
	{
		header("Location:?act=login&err=".urlencode('検証コードを入力してください'));
		exit();
 	}
	if ($captcha['verify_adminlogin']=='1' && strcasecmp($_SESSION['imageCaptcha_content'],$postcaptcha)!=0)
	{
		write_log("<span style=\"color:#FF0000\">検証コード入力エラー</span>",$admin_name,2);
		header("Location:?act=login&err=".urlencode('検証コード入力エラー'));
		exit();
	}
 	elseif(check_admin($admin_name,$admin_pwd))
	{
 		update_admin_info($admin_name);
		write_log("登録成功",$admin_name);
 		if($remember == 1)
		{
			$admininfo=get_admin_one($admin_name);
 			setcookie('Qishi[admin_id]', $_SESSION['admin_id'], time()+86400, $HW_cookiepath, $HW_cookiedomain);
 			setcookie('Qishi[admin_name]', $admin_name, time()+86400, $HW_cookiepath, $HW_cookiedomain);
			setcookie('Qishi[admin_pwd]', md5($admin_name.$admininfo['pwd'].$admininfo['pwd_hash'].$HW_pwdhash), time()+86400, $HW_cookiepath, $HW_cookiedomain);
 		}
 	}
	else
	{
		write_log("<span style=\"color:#FF0000\">ユーザ名或パスワードエラー</span>",$admin_name,2);
		header("Location:?act=login&err=".urlencode('ユーザ名又はパスワードエラー'));
		exit();
 	}
header("Location: admin_index.php"); 
}
?>
