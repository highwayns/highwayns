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
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/fun_wap.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
$smarty->cache = false;
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$id = intval($_GET['id']);
$row=resume_one($id);
wap_weixin_logon($_GET['from']);
if(intval($_SESSION["uid"])>0){
	$sql="select * from ".table("company_down_resume")." where company_uid=$_SESSION[uid] and resume_id=".$id;
	$down_resume=$db->getone($sql);
	$smarty->assign('down_resume',$down_resume);
	$time=time();
	$jobs_sql="select * from ".table("jobs")." where uid=$_SESSION[uid] and display=1 and deadline>$time ";
	$jobs_row=$db->getall($jobs_sql);
	$smarty->assign('jobs_row',$jobs_row);
}
$smarty->assign('show',$row);
$smarty->assign('goback',$_SERVER["HTTP_REFERER"]);
$smarty->display("wap/wap-resume-show.html");
?>