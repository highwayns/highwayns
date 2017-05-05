<?php
define('IN_HIGHWAY', true);
$alias="HW_login";
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_user.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
unset($dbhost,$dbuser,$dbpass,$dbname);
$smarty->cache = false;
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'reg';
$smarty->assign('header_nav',"reg");
$SMSconfig=get_cache('sms_config');
$smarty->assign('SMSconfig',$SMSconfig);
if(!$_SESSION['uid'] && !$_SESSION['username'] && !$_SESSION['utype'] &&  $_COOKIE['QS']['username'] && $_COOKIE['QS']['password'] )
{
	if(check_cookie($_COOKIE['QS']['uid'],$_COOKIE['QS']['username'],$_COOKIE['QS']['password']))
	{
	update_user_info($_COOKIE['QS']['uid'],false,false);
	header("Location:".get_member_url($_SESSION['utype']));
	}
	else
	{
	setcookie("QS[uid]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie('QS[username]',"", time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie('QS[password]',"", time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	setcookie("QS[utype]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
	header("Location:".url_rewrite('HW_login'));
	}
}
//激活账户
elseif ($act=='activate')
{
	if (defined('UC_API')){
				include_once(HIGHWAY_ROOT_PATH.'uc_client/client.php');
				if($data = uc_get_user($_SESSION['activate_username']))
				{
				unset($_SESSION['uid']);
				unset($_SESSION['username']);
				unset($_SESSION['utype']);
				unset($_SESSION['uqqid']);
				setcookie("QS[uid]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
				setcookie("QS[username]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
				setcookie("QS[password]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
				setcookie("QS[utype]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);		
				$smarty->assign('activate_email',$data[2]);
				$smarty->assign('activate_username',$_SESSION['activate_username']);
				}
				else
				{
				showmsg('Active失敗，ユーザ名エラー！',0);
				}
				$smarty->display('user/activate.htm');
	}
}
elseif ($act=='activate_save')
{
		$activateinfo=activate_user($_SESSION['activate_username'],$_POST['pwd'],$_POST['act_email'],$_POST['member_type'],$_POST['mobile']);
		if($activateinfo>0)
		{
			$login_url=user_login($_SESSION['activate_username'],$_POST['pwd'],1,false);
			$link[0]['text'] = "会員中心へ";
			$link[0]['href'] = $login_url['hw_login'];
			$link[1]['text'] = "ウェブ首页";
			$link[1]['href'] = $_CFG['site_dir'];
			$_SESSION['activate_username']="";
			showmsg('Active成功，会員センターへ！',2,$link);
			exit(); 
		}
		else
		{
			if ($activateinfo==-10)
			{
			$html="パスワード入力エラー";
			}
			elseif($activateinfo==-1)
			{
			$html="Active会員タイプ失った";
			}
			elseif($activateinfo==-2)
			{
			$html="電子メールボックス重複あり";
			}
			elseif($activateinfo==-3)
			{
			$html="携帯番号重複";
			}
			elseif($activateinfo==-4)
			{
			$html="ユーザ名重複しました";
			}
			else
			{
			$html="原因未知";
			}
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
			unset($_SESSION["openid"]);
			$link[0]['text'] = "再登録";
			$link[0]['href'] = url_rewrite('HW_login');
			showmsg("Active失敗，原因：{$html}",0,$link);
			exit();
		}
}
elseif ($_SESSION['username'] && $_SESSION['utype'] &&  $_COOKIE['QS']['username'] && $_COOKIE['QS']['password'])
{
	header("Location:".get_member_url($_SESSION['utype']));
}
// 注册第一步
elseif ($act=='reg')
{
	if ($_CFG['closereg']=='1')showmsg("ウェブ会員登録停止しています，後程試してください！",1);
	if(intval($_GET['type'])==3 && $_PLUG['hunter']['p_install']==1){
		showmsg("管理者は人材紹介モジュールを無効にしました,登録禁止！",1);
	}
	if(intval($_GET['type'])==4 && $_PLUG['train']['p_install']==1){
		showmsg("管理者訓練モジュールを閉じた,登録禁止！",1);
	}
	$smarty->assign('title','会員登録 - '.$_CFG['site_name']);
	$token=substr(md5(mt_rand(100000, 999999)), 8,16);
	$_SESSION['reg_token']=$token;
	$smarty->assign('token',$token);
	$captcha=get_cache('captcha');
	$smarty->assign('verify_userreg',$captcha['verify_userreg']);
	$smarty->display('user/reg-step1.htm');
}
// 注册第二步 通过手机
elseif($act =="reg_step2")
{
	global $_CFG;
	if(empty($_POST['token']) || $_POST['token']!=$_SESSION['reg_token'])
	{
		$link[0]['text'] = "登録失敗,再登録";
		$link[0]['href'] = "?act=reg";
		showmsg("登録失敗，リンク正しくない",0,$link);
	}
	$sqlarr['utype']=$_POST['utype']?intval($_POST['utype']):showmsg('会員タイプ選択');
	$sqlarr['mobile']=$_POST['mobile']?trim($_POST['mobile']):showmsg('携帯番号を入力してください');
	$sqlarr['reg_type']=1;
	$token=substr(md5(mt_rand(100000, 999999)), 8,16);
	$_SESSION['reg_token']=$token;
	$smarty->assign('token',$token);
	$smarty->assign('title','会員登録 - '.$_CFG['site_name']);
	$smarty->assign('sqlarr',$sqlarr);
	$smarty->display('user/reg-step2.htm');
}
// 通过邮箱
elseif($act =="reg_step2_email")
{
	global $_CFG;
	if($_CFG['check_reg_email']=="1")
	{
		$email=$_GET['email']?trim($_GET['email']):"";
		$key=$_GET['key']?trim($_GET['key']):"";
		$time=$_GET['time']?trim($_GET['time']):"";

		$end_time=$time+24*3600;
		if($end_time<time())
		{
			$link[0]['text'] = "再登録";
			$link[0]['href'] = "?act=reg";
			showmsg("登録失敗,リンク期間きれ",0,$link);
		}
		$key_str=substr(md5($email.$time),8,16);
		if($key_str!=$key)
		{
			$link[0]['text'] = "再登録";
			$link[0]['href'] = "?act=reg";
			showmsg("登録失敗,keyエラー",0,$link);
		}
		$token=substr(md5(mt_rand(100000, 999999)), 8,16);
		$_SESSION['reg_token']=$token;
		$smarty->assign('token',$token);
		$sqlarr['utype']=$_GET['utype']?intval($_GET['utype']):showmsg('会員タイプ選択');
		$sqlarr['email']=$_GET['email']?trim($_GET['email']):showmsg('メールを入力してください');
	}
	else
	{
		if(empty($_POST['token']) || $_POST['token']!=$_SESSION['reg_token'])
		{
			$link[0]['text'] = "登録失敗,再登録";
			$link[0]['href'] = "?act=reg";
			showmsg("登録失敗，リンク正しくない",0,$link);
		}
		$sqlarr['utype']=$_POST['utype']?intval($_POST['utype']):showmsg('会員タイプ選択');
		$sqlarr['email']=$_POST['email']?trim($_POST['email']):showmsg('メールを入力してください');
		$token=substr(md5(mt_rand(100000, 999999)), 8,16);
		$_SESSION['reg_token']=$token;
		$smarty->assign('token',$token);
	}
	$sqlarr['reg_type']=2;
	$smarty->assign('sqlarr',$sqlarr);
	$smarty->assign('title','会員登録 - '.$_CFG['site_name']);
	$smarty->display('user/reg-step2.htm');
}
// 保存注册信息
elseif($act =="reg_step3")
{
	global $db,$HW_pwdhash,$_CFG,$timestamp;
	if(empty($_POST['token']) || $_POST['token']!=$_SESSION['reg_token'])
	{
		$link[0]['text'] = "登録失敗,再登録";
		$link[0]['href'] = "?act=reg";
		showmsg("登録失敗，リンク正しくない",0,$link);
	}
	unset($_SESSION['reg_token']);
	// 注册信息
	$reg_type=$_POST['reg_type']?intval($_POST['reg_type']):showmsg("登録方式エラー");
	$member_type=$_POST['utype']?intval($_POST['utype']):showmsg("選択登録会員");
	$password=$_POST['password']?trim($_POST['password']):showmsg("パスワードを入力してください");
	if($reg_type==1)
	{
		$mobile=$_POST['mobile']?trim($_POST['mobile']):showmsg("登録携帯番号が失った");
		$rst=user_register($reg_type,$password,$member_type,"",$mobile,false);
	}
	else
	{
		$email=$_POST['email']?trim($_POST['email']):showmsg("登録メールボックスが失った");
		$rst=user_register($reg_type,$password,$member_type,$email,"",$uc_reg=true);
	}
	if($rst>0)
	{
		$user=get_user_inid($rst);

		// 企业信息
		if($member_type==1 && !empty($com_setarr))
		{
			$com_setarr['uid']=intval($rst);
			$com_setarr['audit']=intval($_CFG['audit_add_com']);
			$com_setarr['addtime']=$timestamp;
			$com_setarr['refreshtime']=$timestamp;
			$db->inserttable(table('company_profile'),$com_setarr);
		}	
		$login_js=user_login($user['username'],$password);
		$mailconfig=get_cache('mailconfig');
		if ($mailconfig['set_reg']=="1")
		{
		switch ($user['utype']) {
			case '1':
				$utype_cn='企業'; 
				break;
			case '2':
				$utype_cn='個人'; 
				break;
			case '3':
				$utype_cn='ヘッドハンター'; 
				break;
			case '4':
				$utype_cn='訓練'; 
				break;
		}
		dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid=".$user['uid']."&key=".asyn_userkey($user['uid'])."&sendemail=".$email."&sendusername=".$user['username']."&sendpassword=".$password."&utype=".$utype_cn."&act=reg");
		}
		$user['uc_url']=$login_js['uc_login'];
		$user['url']=$login_js['hw_login'];
		$smarty->assign('title','会員登録 - '.$_CFG['site_name']);
		$smarty->assign('user',$user);
		setcookie("isFirstReg",1, time()+3600*24);
		$smarty->display('user/reg-step3.htm');
	}
	else
	{
		$link[0]['text'] = "登録失敗,再登録";
		$link[0]['href'] = "?act=reg";
		showmsg("登録失敗",0,$link);
	}
}
unset($smarty);
?>
