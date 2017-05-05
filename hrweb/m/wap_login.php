<?php
define('IN_HIGHWAY', true);
$alias="HW_login";
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_user.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
unset($dbhost,$dbuser,$dbpass,$dbname);
$smarty->caching = false;
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'login';
if($act == 'logout')
{
	unset($_SESSION['uid']);
	unset($_SESSION['username']);
	unset($_SESSION['utype']);
	setcookie("QS[uid]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie("QS[username]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie("QS[password]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie("QS[utype]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	unset($_SESSION['activate_username']);
	unset($_SESSION['activate_email']);
	header("location:index.php"); 
}
elseif($act == 'weixin_login'){
	$openid = trim($_GET['openid']);
	$uid = intval($_GET['uid']);
	$event_key = intval($_GET['event_key']);
	weixin_login($openid,$uid,$event_key);

	
	$smarty->display('wap/scan/scan_success.html');
}
elseif(!$_SESSION['uid'] && !$_SESSION['username'] && !$_SESSION['utype'] &&  $_COOKIE['QS']['username'] && $_COOKIE['QS']['password'] )
{
	if(check_cookie($_COOKIE['QS']['username'],$_COOKIE['QS']['password']))
	{
	update_user_info($_COOKIE['QS']['username'],false,false);
			if($_SESSION['utype']==2)	header("location:personal/wap_user.php");
			if($_SESSION['utype']==1)	header("location:company/wap_user.php");
	}
	else
	{
	setcookie("QS[uid]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie('QS[username]',"", time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie('QS[password]',"", time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie("QS[utype]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	header("location:index.php"); 
	}
}
elseif ($_SESSION['username'] && $_SESSION['utype'] )
{
			if($_SESSION['utype']==2)	header("location:personal/wap_user.php");
			if($_SESSION['utype']==1)	header("location:company/wap_user.php");
}
elseif ($act=='login')
{
	$_SESSION['url'] = $_SERVER['HTTP_REFERER'];
	$smarty->display('wap/wap_login.html');
}
elseif ($act == 'do_login')
{
	require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
	if($_POST['username']=="ユーザ名/携帯番号/メール" || $_POST['password']==""|| $_POST['username']=="" ){
		$smarty->assign('err',"ユーザパスワードを入力してください");
		$smarty->display('wap/wap_login.html');
	}else{
		$username=isset($_POST['username'])?trim($_POST['username']):"";
		$password=isset($_POST['password'])?trim($_POST['password']):"";
		$expire=isset($_POST['expire'])?intval($_POST['expire']):"";
		if ($username && $password)
		{
			if (wap_user_login($username,$password))
			{	
					if(!empty($_SESSION['url'])){
						header("location:".$_SESSION['url']);
						unset($_SESSION['url']);
						die;
					}
				if($_SESSION['utype']==2)	header("location:personal/wap_user.php");
				if($_SESSION['utype']==1)	header("location:company/wap_user.php");
			}
			else
			{
				$smarty->caching = false;
				$smarty->assign('err',"ユーザ登録失敗，ユーザ名或パスワードエラー");
				$smarty->display('wap/wap_login.html');
			}		
		}
	}

}
elseif($act == 'waiting_weixin_login'){
	$event_key = $_SESSION['scene_id'];
	$content = "";
	if(file_exists(HIGHWAY_ROOT_PATH."data/weixin/".($event_key%10).'/'.$event_key.".txt")){
		$content = file_get_contents(HIGHWAY_ROOT_PATH."data/weixin/".($event_key%10).'/'.$event_key.".txt");
	}	
	$uid = intval($content);
	if($uid>0){
		global $HW_cookiepath,$HW_cookiedomain;
		$u=get_user_by_uid($uid);
		if (!empty($u))
		{
			unset($_SESSION['uid']);
			unset($_SESSION['username']);
			unset($_SESSION['utype']);
			unset($_SESSION['uqqid']);
			setcookie("QS[uid]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
			setcookie("QS[username]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
			setcookie("QS[password]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
			setcookie("QS[utype]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
			unset($_SESSION['activate_username']);
			unset($_SESSION['activate_email']);
			
			$_SESSION['uid']=$u['uid'];
			$_SESSION['username']=$u['username'];
			$_SESSION['utype']=$u['utype'];
			$_SESSION['uqqid']="1";
			setcookie('QS[uid]',$u['uid'],0,$HW_cookiepath,$HW_cookiedomain);
			setcookie('QS[username]',$u['username'],0,$HW_cookiepath,$HW_cookiedomain);
			setcookie('QS[password]',$u['password'],0,$HW_cookiepath,$HW_cookiedomain);
			setcookie('QS[utype]',$u['utype'], 0,$HW_cookiepath,$HW_cookiedomain);
			unlink(HIGHWAY_ROOT_PATH."data/weixin/".($event_key%10).'/'.$event_key.".txt");
		}
		exit("1");
	}
}
function weixin_login($openid,$uid,$event_key){
	global $HW_cookiepath,$HW_cookiedomain,$_CFG;
	$u=get_user_by_weixinopenid($openid,$uid);
	if (!empty($u))
	{
		if(file_exists(HIGHWAY_ROOT_PATH."data/weixin/".($event_key%10).'/'.$event_key.".txt")){
			ini_set('session.save_handler', 'files');
			session_save_path(HIGHWAY_ROOT_PATH.'data/sessions/');
			session_start();
			$fp = @fopen(HIGHWAY_ROOT_PATH . 'data/weixin/'.($event_key%10).'/'.$event_key.'.txt', 'wb+');
			@fwrite($fp, $uid);
			@fclose($fp);
			$find = array("http://","/wap");
			$replace = array("");
			$HW_cookiedomain = str_replace($find,$replace,$_CFG['wap_domain']);
			unset($_SESSION['uid']);
			unset($_SESSION['username']);
			unset($_SESSION['utype']);
			unset($_SESSION['uqqid']);
			setcookie("QS[uid]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
			setcookie("QS[username]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
			setcookie("QS[password]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
			setcookie("QS[utype]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
			unset($_SESSION['activate_username']);
			unset($_SESSION['activate_email']);
			
			$_SESSION['uid']=$u['uid'];
			$_SESSION['username']=$u['username'];
			$_SESSION['utype']=$u['utype'];
			$_SESSION['uqqid']="1";
			setcookie('QS[uid]',$u['uid'],0,$HW_cookiepath,$HW_cookiedomain);
			setcookie('QS[username]',$u['username'],0,$HW_cookiepath,$HW_cookiedomain);
			setcookie('QS[password]',$u['password'],0,$HW_cookiepath,$HW_cookiedomain);
			setcookie('QS[utype]',$u['utype'], 0,$HW_cookiepath,$HW_cookiedomain);
		}
	}
}
function get_user_by_weixinopenid($openid,$uid){
	global $db;
	$usinfo = $db->getone("select * from ".table('members')." where weixin_openid='".$openid."' and uid='".$uid."'");
	return $usinfo;
}
function get_user_by_uid($uid){
	global $db;
	$usinfo = $db->getone("select * from ".table('members')." where uid='".$uid."'");
	return $usinfo;
}
unset($smarty);
?>
