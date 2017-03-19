<?php
 /*
 * 74cms 会员注册
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
$alias="QS_login";
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
require_once(QISHI_ROOT_PATH.'include/fun_user.php');
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
	setcookie("QS[uid]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
	setcookie('QS[username]',"", time() - 3600,$QS_cookiepath, $QS_cookiedomain);
	setcookie('QS[password]',"", time() - 3600,$QS_cookiepath, $QS_cookiedomain);
	setcookie("QS[utype]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
	header("Location:".url_rewrite('QS_login'));
	}
}
//激活账户
elseif ($act=='activate')
{
	if (defined('UC_API')){
				include_once(QISHI_ROOT_PATH.'uc_client/client.php');
				if($data = uc_get_user($_SESSION['activate_username']))
				{
				unset($_SESSION['uid']);
				unset($_SESSION['username']);
				unset($_SESSION['utype']);
				unset($_SESSION['uqqid']);
				setcookie("QS[uid]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
				setcookie("QS[username]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
				setcookie("QS[password]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
				setcookie("QS[utype]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);		
				$smarty->assign('activate_email',$data[2]);
				$smarty->assign('activate_username',$_SESSION['activate_username']);
				}
				else
				{
				showmsg('激活失败，用户名错误！',0);
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
			$link[0]['text'] = "进入会员中心";
			$link[0]['href'] = $login_url['qs_login'];
			$link[1]['text'] = "网站首页";
			$link[1]['href'] = $_CFG['site_dir'];
			$_SESSION['activate_username']="";
			showmsg('激活成功，即将进入会员中心！',2,$link);
			exit(); 
		}
		else
		{
			if ($activateinfo==-10)
			{
			$html="密码输入错误";
			}
			elseif($activateinfo==-1)
			{
			$html="激活会员类型丢失";
			}
			elseif($activateinfo==-2)
			{
			$html="电子邮箱有重复";
			}
			elseif($activateinfo==-3)
			{
			$html="手机有重复";
			}
			elseif($activateinfo==-4)
			{
			$html="用户名有重复";
			}
			else
			{
			$html="原因未知";
			}
			unset($_SESSION['uid']);
			unset($_SESSION['username']);
			unset($_SESSION['utype']);
			unset($_SESSION['uqqid']);
			setcookie("QS[uid]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
			setcookie("QS[username]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
			setcookie("QS[password]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
			setcookie("QS[utype]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
			unset($_SESSION['activate_username']);
			unset($_SESSION['activate_email']);
			unset($_SESSION["openid"]);
			$link[0]['text'] = "重新登录";
			$link[0]['href'] = url_rewrite('QS_login');
			showmsg("激活失败，原因：{$html}",0,$link);
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
	if ($_CFG['closereg']=='1')showmsg("网站暂停会员注册，请稍后再次尝试！",1);
	if(intval($_GET['type'])==3 && $_PLUG['hunter']['p_install']==1){
		showmsg("管理员已关闭猎头模块,禁止注册！",1);
	}
	if(intval($_GET['type'])==4 && $_PLUG['train']['p_install']==1){
		showmsg("管理员已关闭培训模块,禁止注册！",1);
	}
	$smarty->assign('title','会员注册 - '.$_CFG['site_name']);
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
		$link[0]['text'] = "注册失败,重新注册";
		$link[0]['href'] = "?act=reg";
		showmsg("注册失败，非正常链接",0,$link);
	}
	$sqlarr['utype']=$_POST['utype']?intval($_POST['utype']):showmsg('请选择会员类型');
	$sqlarr['mobile']=$_POST['mobile']?trim($_POST['mobile']):showmsg('请输入手机号');
	$sqlarr['reg_type']=1;
	$token=substr(md5(mt_rand(100000, 999999)), 8,16);
	$_SESSION['reg_token']=$token;
	$smarty->assign('token',$token);
	$smarty->assign('title','会员注册 - '.$_CFG['site_name']);
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
			$link[0]['text'] = "重新注册";
			$link[0]['href'] = "?act=reg";
			showmsg("注册失败,链接过期",0,$link);
		}
		$key_str=substr(md5($email.$time),8,16);
		if($key_str!=$key)
		{
			$link[0]['text'] = "重新注册";
			$link[0]['href'] = "?act=reg";
			showmsg("注册失败,key错误",0,$link);
		}
		$token=substr(md5(mt_rand(100000, 999999)), 8,16);
		$_SESSION['reg_token']=$token;
		$smarty->assign('token',$token);
		$sqlarr['utype']=$_GET['utype']?intval($_GET['utype']):showmsg('请选择会员类型');
		$sqlarr['email']=$_GET['email']?trim($_GET['email']):showmsg('请输入邮箱');
	}
	else
	{
		if(empty($_POST['token']) || $_POST['token']!=$_SESSION['reg_token'])
		{
			$link[0]['text'] = "注册失败,重新注册";
			$link[0]['href'] = "?act=reg";
			showmsg("注册失败，非正常链接",0,$link);
		}
		$sqlarr['utype']=$_POST['utype']?intval($_POST['utype']):showmsg('请选择会员类型');
		$sqlarr['email']=$_POST['email']?trim($_POST['email']):showmsg('请输入邮箱');
		$token=substr(md5(mt_rand(100000, 999999)), 8,16);
		$_SESSION['reg_token']=$token;
		$smarty->assign('token',$token);
	}
	$sqlarr['reg_type']=2;
	$smarty->assign('sqlarr',$sqlarr);
	$smarty->assign('title','会员注册 - '.$_CFG['site_name']);
	$smarty->display('user/reg-step2.htm');
}
// 保存注册信息
elseif($act =="reg_step3")
{
	global $db,$QS_pwdhash,$_CFG,$timestamp;
	if(empty($_POST['token']) || $_POST['token']!=$_SESSION['reg_token'])
	{
		$link[0]['text'] = "注册失败,重新注册";
		$link[0]['href'] = "?act=reg";
		showmsg("注册失败，非正常链接",0,$link);
	}
	unset($_SESSION['reg_token']);
	// 注册信息
	$reg_type=$_POST['reg_type']?intval($_POST['reg_type']):showmsg("注册方式错误");
	$member_type=$_POST['utype']?intval($_POST['utype']):showmsg("选择注册会员");
	$password=$_POST['password']?trim($_POST['password']):showmsg("请输入密码");
	if($reg_type==1)
	{
		$mobile=$_POST['mobile']?trim($_POST['mobile']):showmsg("注册手机号丢失");
		$rst=user_register($reg_type,$password,$member_type,"",$mobile,false);
	}
	else
	{
		$email=$_POST['email']?trim($_POST['email']):showmsg("注册邮箱号丢失");
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
				$utype_cn='企业'; 
				break;
			case '2':
				$utype_cn='个人'; 
				break;
			case '3':
				$utype_cn='猎头'; 
				break;
			case '4':
				$utype_cn='培训'; 
				break;
		}
		dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid=".$user['uid']."&key=".asyn_userkey($user['uid'])."&sendemail=".$email."&sendusername=".$user['username']."&sendpassword=".$password."&utype=".$utype_cn."&act=reg");
		}
		$user['uc_url']=$login_js['uc_login'];
		$user['url']=$login_js['qs_login'];
		$smarty->assign('title','会员注册 - '.$_CFG['site_name']);
		$smarty->assign('user',$user);
		setcookie("isFirstReg",1, time()+3600*24);
		$smarty->display('user/reg-step3.htm');
	}
	else
	{
		$link[0]['text'] = "注册失败,重新注册";
		$link[0]['href'] = "?act=reg";
		showmsg("注册失败",0,$link);
	}
}
unset($smarty);
?>