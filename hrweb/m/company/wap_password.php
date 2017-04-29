<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_company.php');
$smarty->cache = false;
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'password_edit';
if ($_SESSION['uid']=='' || $_SESSION['username']==''||intval($_SESSION['utype'])==2)
{
	header("Location: ../wap_login.php");
}
elseif ($act == 'password_edit')
{
	$uid = intval($_SESSION['uid']);
	//total里面存的是这个会员站内信的内容 
	$smarty->assign('total',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='1'"));
	$smarty->assign('title','修改パスワード - 企業会員センター - '.$_CFG['site_name']);
	$smarty->display("wap/company/wap-password.html");
	
}
elseif ($act == 'save_password')
{	
	require_once(HIGHWAY_ROOT_PATH.'include/fun_user.php');
	$arr['username']=$_SESSION['username'];
	$arr['oldpassword']=trim($_POST['oldpassword'])?trim($_POST['oldpassword']):exit('旧パスワードを入力してください！');
	$arr['password']=trim($_POST['password'])?trim($_POST['password']):exit('新パスワードを入力してください！');
	if ($arr['password']!=trim($_POST['password1'])) exit('パスワードが一致しません，再度入力してください！');
	//fun_user.php中edit_password()修改密码的方法
	$info=edit_password($arr);
	if ($info==-1) exit('旧パスワード間違いました！');
	if ($info==$_SESSION['username']){
		//发送邮件
		$mailconfig=get_cache('mailconfig');
		if ($mailconfig['set_editpwd']=="1" && $user['email_audit']=="1")
		{
		dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid=".$_SESSION['uid']."&key=".asyn_userkey($_SESSION['uid'])."&act=set_editpwd&newpassword=".$arr['password']);
		}
		//邮件发送完毕
		//sms
		$sms=get_cache('sms_config');
		if ($sms['open']=="1" && $sms['set_editpwd']=="1"  && $user['mobile_audit']=="1")
		{
		dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_sms.php?uid=".$_SESSION['uid']."&key=".asyn_userkey($_SESSION['uid'])."&act=set_editpwd&newpassword=".$arr['password']);
		}
		//sms
		//往会员日志表里记录
		write_memberslog($_SESSION['uid'],2,1004 ,$_SESSION['username'],"修改密码");
		exit('パスワード更新失敗！');
	 }
}

?>
