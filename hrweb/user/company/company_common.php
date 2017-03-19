<?php
/*
 * 74cms 企业会员中心
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
if(!defined('IN_QISHI')) die('Access Denied!');
$page_select="user";
require_once(dirname(dirname(dirname(__FILE__))).'/include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
require_once(QISHI_ROOT_PATH.'include/fun_company.php');
	$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
	if((empty($_SESSION['uid']) || empty($_SESSION['username']) || empty($_SESSION['utype'])) &&  $_COOKIE['QS']['username'] && $_COOKIE['QS']['password'] && $_COOKIE['QS']['uid'])
	{
		require_once(QISHI_ROOT_PATH.'include/fun_user.php');
		if(check_cookie($_COOKIE['QS']['uid'],$_COOKIE['QS']['username'],$_COOKIE['QS']['password']))
		{
		update_user_info($_COOKIE['QS']['uid'],false,false);
		header("Location:".get_member_url($_SESSION['utype']));
		}
		else
		{
		unset($_SESSION['uid'],$_SESSION['username'],$_SESSION['utype'],$_SESSION['uqqid'],$_SESSION['activate_username'],$_SESSION['activate_email'],$_SESSION["openid"]);
		setcookie("QS[uid]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		setcookie('QS[username]',"", time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		setcookie('QS[password]',"", time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		setcookie("QS[utype]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		}
	}
	if ($_SESSION['uid']=='' || $_SESSION['username']=='' || intval($_SESSION['uid'])===0)
	{
		header("Location: ".url_rewrite('QS_login')."?act=logout");
		exit();
	}
	elseif ($_SESSION['utype']!='1') 
	{
	$link[0]['text'] = "会员中心";
	$link[0]['href'] = url_rewrite('QS_login');
	showmsg('您访问的页面需要 企业会员 登录！',1,$link);
	}
	$act = !empty($_GET['act']) ? trim($_GET['act']) : 'index';
	$smarty->cache = false;
	$user=get_user_info($_SESSION['uid']);
	if(intval($user['consultant'])>0){
		$consultant = get_consultant($user['consultant']);
		$smarty->assign('consultant',$consultant);
	}
	if ($user['status']=="2" && $act!='index' && $act!='user_status'  && $act!='user_status_save') 
	{
		$link[0]['text'] = "返回会员中心首页";
		$link[0]['href'] = 'company_index.php?act=';
	exit(showmsg('您的账号处于暂停状态，请联系管理员设为正常后进行操作！',1,$link));	
	}
	elseif (empty($user))
	{
	unset($_SESSION['utype'],$_SESSION['uid'],$_SESSION['username']);
	header("Location:".url_rewrite('QS_login')."?url=".$_SERVER["REQUEST_URI"]);
	exit();
	}


	if ($_CFG['login_com_audit_email'] && $user['email_audit']=="0" && $act!='authenticate' && $act!='user_email' && $act!='user_mobile')
	{
		$link[0]['text'] = "认证邮箱";
		$link[0]['href'] = 'company_user.php?act=authenticate';
		$link[1]['text'] = "网站首页";
		$link[1]['href'] = $_CFG['site_dir'];
		showmsg('您的邮箱未认证，认证后才能进行其他操作！',1,$link,true,6);
		exit();
	}
	$sms=get_cache('sms_config');
	if ($_CFG['login_com_audit_mobile'] && $user['mobile_audit']=="0" && $act!='authenticate' && $act!='user_mobile' && $act!='user_email' && $sms['open']=="1")
	{
		$link[0]['text'] = "认证手机";
		$link[0]['href'] = 'company_user.php?act=authenticate';
		$link[1]['text'] = "网站首页";
		$link[1]['href'] = $_CFG['site_dir'];
		showmsg('您的手机未认证，认证后才能进行其他操作！',1,$link,true,6);
		exit();
	}
	$smarty->assign('sms',$sms);
	$smarty->assign('promotion_category',get_promotion_category());
	$company_profile=get_company($_SESSION['uid']);
	if (!empty($company_profile))
	{
		$company_profile = array_map("addslashes",$company_profile);
		$smarty->assign('company_url',url_rewrite('QS_companyshow',array('id'=>$company_profile['id'])));
		// 企业是否完善资料
		$cominfo_flge=true;
		$array=array("companyname","nature","trade","district","scale","address","contact","telephone","email","contents");
		foreach ($company_profile as $key => $value){
			if(in_array($key,$array) && empty($value))
			{
				$cominfo_flge=false;
				break;
			}
		}
		$smarty->assign('cominfo_flge',$cominfo_flge);
	}	
	else
	{
		$cominfo_flge=false;
		$smarty->assign('cominfo_flge',$cominfo_flge);
	}
	$smarty->assign('userindexurl','company_index.php');
	if ($_SESSION['handsel_userlogin'])
	{
	//第一次登录提示
	$smarty->assign('handsel_userlogin',$_SESSION['handsel_userlogin']);
	unset($_SESSION['handsel_userlogin']);
	}
?>