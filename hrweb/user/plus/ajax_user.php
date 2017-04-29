<?php
define('IN_HIGHWAY', true);
require_once(dirname(dirname(__FILE__)).'/../include/plus.common.inc.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : '';
if($act =='do_login')
{
	$username=isset($_POST['username'])?trim($_POST['username']):"";
	$password=isset($_POST['password'])?trim($_POST['password']):"";
	$expire=isset($_POST['expire'])?intval($_POST['expire']):"";
	$account_type=1;
	if (preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$username))
	{
	$account_type=2;
	}
	elseif (preg_match("/^(13|14|15|18)\d{9}$/",$username))
	{
	$account_type=3;
	}
	$url=isset($_POST['url'])?$_POST['url']:"";
	if (strcasecmp(HIGHWAY_DBCHARSET,"utf8")!=0)
	{
	$username=utf8_to_gbk($username);
	$password=utf8_to_gbk($password);
	}
	$captcha=get_cache('captcha');
	if ($captcha['verify_userlogin']=="1")
	{
		$postcaptcha=$_POST['postcaptcha'];
		if ($captcha['captcha_lang']=="cn" && strcasecmp(HIGHWAY_DBCHARSET,"utf8")!=0)
		{
		$postcaptcha=utf8_to_gbk($postcaptcha);
		}
		if (empty($postcaptcha) || empty($_SESSION['imageCaptcha_content']) || strcasecmp($_SESSION['imageCaptcha_content'],$postcaptcha)!=0)
		{
		unset($_SESSION['imageCaptcha_content']);
		exit("errcaptcha");
		}
	}
	require_once(HIGHWAY_ROOT_PATH.'include/fun_user.php');
	if ($username && $password)
	{
		$login=user_login($username,$password,$account_type,true,$expire);
		$url=$url?$url:$login['hw_login'];
		if ($login['hw_login'])
		{
		exit($login['uc_login']."<script language=\"javascript\" type=\"text/javascript\">window.location.href=\"".$url."\";</script>");
		}
		else
		{
		exit("err");
		}
	}
	exit("err");
}
elseif ($act=='do_reg')
{
	$captcha=get_cache('captcha');
	if ($captcha['verify_userreg']=="1")
	{
		$postcaptcha=$_POST['postcaptcha'];
		if ($captcha['captcha_lang']=="cn" && strcasecmp(HIGHWAY_DBCHARSET,"utf8")!=0)
		{
		$postcaptcha=utf8_to_gbk($postcaptcha);
		}
		if (empty($postcaptcha) || empty($_SESSION['imageCaptcha_content']) || strcasecmp($_SESSION['imageCaptcha_content'],$postcaptcha)!=0)
		{
		exit("err");
		}
	}
	require_once(HIGHWAY_ROOT_PATH.'include/fun_user.php');
	$username = isset($_POST['username'])?trim($_POST['username']):exit("err");
	$password = isset($_POST['password'])?trim($_POST['password']):exit("err");
	$member_type = isset($_POST['member_type'])?intval($_POST['member_type']):exit("err");
	$email = isset($_POST['email'])?trim($_POST['email']):exit("err");
	if (strcasecmp(HIGHWAY_DBCHARSET,"utf8")!=0)
	{
	$username=utf8_to_gbk($username);
	$password=utf8_to_gbk($password);
	}
		if(defined('UC_API'))
		{
			include_once(HIGHWAY_ROOT_PATH.'uc_client/client.php');
			if (uc_user_checkname($username)<0)
			{
			exit("err");
			}
			if (uc_user_checkemail($email)<0)
			{
			exit("err");
			}
		}
	$register=user_register($username,$password,$member_type,$email);
	if ($register>0)
	{	
		$login_js=user_login($username,$password);
		$mailconfig=get_cache('mailconfig');
		if ($mailconfig['set_reg']=="1")
		{
		dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid=".$_SESSION['uid']."&key=".asyn_userkey($_SESSION['uid'])."&sendemail=".$email."&sendusername=".$username."&sendpassword=".$password."&act=reg");
		}
		$ucjs=$login_js['uc_login'];
		$qsurl=$login_js['hw_login'];
		$qsjs="<script language=\"javascript\" type=\"text/javascript\">window.location.href=\"".$qsurl."\";</script>";
		 if ($ucjs || $qsurl)
			{
			exit($ucjs.$qsjs);
			}
			else
			{
			exit("err");
			}
	}
	else
	{
	exit("err");
	}
}
elseif($act =='check_usname')
{
	require_once(HIGHWAY_ROOT_PATH.'include/fun_user.php');
	$usname=trim($_POST['usname']);
	if (strcasecmp(HIGHWAY_DBCHARSET,"utf8")!=0)
	{
	$usname=utf8_to_gbk($usname);
	}
	$user=get_user_inusername($usname);
	if (defined('UC_API'))
	{
		include_once(HIGHWAY_ROOT_PATH.'uc_client/client.php');
		if (uc_user_checkname($usname)===1 && empty($user))
		{
		exit("true");
		}
		else
		{
		exit("false");
		}
	}
	empty($user)?exit("true"):exit("false");
}
elseif($act == 'check_email')
{
	require_once(HIGHWAY_ROOT_PATH.'include/fun_user.php');
	$email=trim($_POST['email']);
	if (strcasecmp(HIGHWAY_DBCHARSET,"utf8")!=0)
	{
	$email=utf8_to_gbk($email);
	}
	$user=get_user_inemail($email);
	if (defined('UC_API'))
	{
		include_once(HIGHWAY_ROOT_PATH.'uc_client/client.php');
		if (uc_user_checkemail($email)===1 && empty($user))
		{
		exit("true");
		}
		else
		{
		exit("false");
		}
	}
	empty($user)?exit("true"):exit("false");
}
elseif ($act=="top_loginform")
{
	$contents='';
	if ($_COOKIE['QS']['username'] && $_COOKIE['QS']['password'])
	{
		$contents='よこそ&nbsp;&nbsp;<a href="{#$user_url#}" style="color:#339900">{#$username#}</a> 登録！&nbsp;&nbsp;{#$pmscount_a#}&nbsp;&nbsp;&nbsp;&nbsp;<a href="{#$user_url#}">[会員センター]</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="{#$logout_url#}" >[終了]</a>';
	}
	elseif ($_SESSION['activate_username'] && defined('UC_API'))
	{
		$contents=' &nbsp;&nbsp;アカウント {#$activate_username#} はＡｃｉｔｖｅしません！ <a href="{#$activate_url#}" style="color:#339900">Ａｃｔｉｖｅ</a>';
	}
	else
	{	
		$contents='よこそ{#$site_name#}！&nbsp;&nbsp;&nbsp;&nbsp;<a href="{#$login_url#}" >[登録]</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="{#$reg_url#}" >[無料登録]</a>';
	}
		$contents=str_replace('{#$activate_username#}',$_SESSION['activate_username'],$contents);
		$contents=str_replace('{#$site_name#}',$_CFG['site_name'],$contents);
		$contents=str_replace('{#$username#}',$_COOKIE['QS']['username'],$contents);
		$contents=str_replace('{#$pmscount#}',$_COOKIE['QS']['pmscount'],$contents);
		$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
		if ($_COOKIE['QS']['utype']=='1')
		{
		$user_url=$_CFG['site_dir']."user/company/company_index.php";
			if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/company/company_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="ショートメッセージ">メッセージ '.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		if ($_COOKIE['QS']['utype']=='2')
		{
			$user_url=$_CFG['site_dir']."user/personal/personal_index.php";
			if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/personal/personal_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="ショートメッセージ">メッセージ'.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		if ($_COOKIE['QS']['utype']=='4')
		{
			$user_url=$_CFG['site_dir']."user/train/train_index.php";
			if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/train/train_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="ショートメッセージ">メッセージ'.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		if ($_COOKIE['QS']['utype']=='3')
		{
			$user_url=$_CFG['site_dir']."user/hunter/hunter_index.php";
			if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/hunter/hunter_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="ショートメッセージ">メッセージ '.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		$contents=str_replace('{#$pmscount_a#}',$pmscount_a,$contents);
		$contents=str_replace('{#$user_url#}',$user_url,$contents);
		$contents=str_replace('{#$login_url#}',url_rewrite('HW_login'),$contents);
		$contents=str_replace('{#$logout_url#}',url_rewrite('HW_login')."?act=logout",$contents);
		$contents=str_replace('{#$reg_url#}',$_CFG['site_dir']."user/user_reg.php",$contents);
		$contents=str_replace('{#$activate_url#}',$_CFG['site_dir']."user/user_reg.php?act=activate",$contents);
		exit($contents);
}
elseif ($act=="loginform")
{
	$contents='';
	if ($_COOKIE['QS']['username'] && $_COOKIE['QS']['password'])
	{
		$tpl='../templates/'.$_CFG['template_dir']."plus/login_success.htm";
	}
	elseif ($_SESSION['activate_username'] && defined('UC_API'))
	{
		$tpl='../templates/'.$_CFG['template_dir']."plus/login_activate.htm";
	}
	else
	{
		$tpl='../templates/'.$_CFG['template_dir']."plus/login_form.htm";
	}
		$contents=file_get_contents($tpl);
		$contents=str_replace('{#$activate_username#}',$_SESSION['activate_username'],$contents);
		$contents=str_replace('{#$site_name#}',$_CFG['site_name'],$contents);
		$contents=str_replace('{#$username#}',$_COOKIE['QS']['username'],$contents);
		$contents=str_replace('{#$pmscount#}',$_COOKIE['QS']['pmscount'],$contents);
		$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
		$contents=str_replace('{#$site_dir#}',$_CFG['site_dir'],$contents);
		if ($_COOKIE['QS']['utype']=='1')
		{
			$user_url=$_CFG['site_dir']."user/company/company_index.php";
			 if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/company/company_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="ショートメッセージ">メッセージ '.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		if ($_COOKIE['QS']['utype']=='2')
		{
			$user_url=$_CFG['site_dir']."user/personal/personal_index.php";
			 if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/personal/personal_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="ショートメッセージ">メッセージ'.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		if ($_COOKIE['QS']['utype']=='4')
		{
			$user_url=$_CFG['site_dir']."user/train/train_index.php";
			 if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/train/train_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="ショートメッセージ">メッセージ'.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		if ($_COOKIE['QS']['utype']=='3')
		{
			$user_url=$_CFG['site_dir']."user/hunter/hunter_index.php";
			 if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/hunter/hunter_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="ショートメッセージ">メッセージ '.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		$contents=str_replace('{#$pmscount_a#}',$pmscount_a,$contents);
		$contents=str_replace('{#$user_url#}',$user_url,$contents);
		$contents=str_replace('{#$login_url#}',url_rewrite('HW_login'),$contents);
		$contents=str_replace('{#$logout_url#}',url_rewrite('HW_login')."?act=logout",$contents);
		$contents=str_replace('{#$reg_url#}',$_CFG['site_dir']."user/user_reg.php",$contents);
		$contents=str_replace('{#$activate_url#}',$_CFG['site_dir']."user/user_reg.php?act=activate",$contents);
		exit($contents);
}
?>
