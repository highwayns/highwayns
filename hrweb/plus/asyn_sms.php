<?php
 /*
 * 74cms SMS
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
ignore_user_abort(true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
require_once(QISHI_ROOT_PATH.'include/fun_user.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_GET['act']) ? trim($_GET['act']) : '';
$uid=intval($_GET['uid']);
$key=trim($_GET['key']);
if (empty($uid) || empty($key))
{
exit("error");
}
$asyn_userkey=asyn_userkey($uid);
if ($asyn_userkey<>$key)
{
exit("error");
}
$SMSconfig=get_cache('sms_config');
$SMStemplates=get_cache('sms_templates');
$userinfo=get_user_inid($uid);
if ($SMSconfig['open']!="1")
{
exit("error");
}
if($act == 'jobs_apply')
{   $jobs_uid=intval($_GET['jobs_uid']);
	$comuser=get_user_inid($jobs_uid);
	if (empty($comuser['mobile']) || $comuser['mobile_audit']=='0')
	{
	exit("error");
	}
	else
	{
	$templates=label_replace($SMStemplates['set_applyjobs']);
	exit(send_sms($comuser['mobile'],$templates));
	}
}
elseif($act == 'set_invite')
{
	$templates=label_replace($SMStemplates['set_invite']);
	exit(send_sms($_GET['mobile'],$templates));
}
elseif($act == 'set_order')
{
	$templates=label_replace($SMStemplates['set_order']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_payment')
{
	$templates=label_replace($SMStemplates['set_payment']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_editpwd')
{
	$templates=label_replace($SMStemplates['set_editpwd']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_jobsallow')
{
	$templates=label_replace($SMStemplates['set_jobsallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_jobsnotallow')
{
	$templates=label_replace($SMStemplates['set_jobsnotallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_licenseallow')
{
	$templates=label_replace($SMStemplates['set_licenseallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_licensenotallow')
{
	$templates=label_replace($SMStemplates['set_licensenotallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_addmap')
{
	$templates=label_replace($SMStemplates['set_addmap']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_resumeallow'){
	$templates=label_replace($SMStemplates['set_resumeallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_resumenotallow'){
	$templates=label_replace($SMStemplates['set_resumenotallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_teaallow'){
	$templates=label_replace($SMStemplates['set_teaallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_teanotallow'){
	$templates=label_replace($SMStemplates['set_teanotallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_couallow'){
	$templates=label_replace($SMStemplates['set_couallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_counotallow'){
	$templates=label_replace($SMStemplates['set_counotallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_applycou'){
	$templates=label_replace($SMStemplates['set_applycou']);
	send_sms($_GET['mobile'],$templates);
}
elseif($act == 'set_downapp'){
	$templates=label_replace($SMStemplates['set_downapp']);
	send_sms($_GET['mobile'],$templates);
}
elseif($act == 'set_hunallow'){
	$templates=label_replace($SMStemplates['set_hunallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_hunnotallow'){
	$templates=label_replace($SMStemplates['set_hunnotallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_hunjobsallow'){
	$templates=label_replace($SMStemplates['set_hunjobsallow']);
	send_sms($userinfo['mobile'],$templates);
}
elseif($act == 'set_hunjobsnotallow'){
	$templates=label_replace($SMStemplates['set_hunjobsnotallow']);
	send_sms($userinfo['mobile'],$templates);
}

?>