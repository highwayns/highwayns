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

require_once(dirname(__FILE__).'/../../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/fun_wap.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
require_once(QISHI_ROOT_PATH.'include/fun_personal.php');
$smarty->cache = false;
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'password_edit';
if ($_SESSION['uid']=='' || $_SESSION['username']==''||intval($_SESSION['utype'])==1)
{
	header("Location: ../wap_login.php");
}
elseif ($act == 'password_edit')
{
	$uid = intval($_SESSION['uid']);
	$smarty->assign('total',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='1'"));
	$smarty->assign('title','修改密码 - 个人会员中心 - '.$_CFG['site_name']);
	$smarty->display("wap/personal/wap-password.html");

	
}
elseif ($act == 'save_password')
{	
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$arr['username']=$_SESSION['username'];
	$arr['oldpassword']=trim($_POST['oldpassword'])?trim($_POST['oldpassword']):exit('请输入旧密码！');
	$arr['password']=trim($_POST['password'])?trim($_POST['password']):exit('请输入新密码！');
	if ($arr['password']!=trim($_POST['password1'])) exit('两次输入密码不相同，请重新输入！');
	//edit_password()修改密码的方法
	$info=edit_password($arr);
	if ($info==-1) exit('旧密码输入错误，请重新输入！');
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
		if(defined('UC_API'))
		{
		include_once(QISHI_ROOT_PATH.'uc_client/client.php');
		uc_user_edit($arr['username'],$arr['oldpassword'], $arr['password']);
		}
		//往会员日志表里记录
		write_memberslog($_SESSION['uid'],2,1004 ,$_SESSION['username'],"修改密码");
		exit('密码修改成功！');
	 }
}

?>