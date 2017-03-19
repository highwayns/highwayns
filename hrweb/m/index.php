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
$emergency_jobs = $db->getall("SELECT * FROM ".table('jobs')." WHERE `emergency`=1 ORDER BY `refreshtime` DESC LIMIT 5");	
foreach ($emergency_jobs as $key => $value) {
	$emergency_jobs[$key]['url'] = wap_url_rewrite("wap-jobs-show",array("id"=>$value["id"]));
	if (!empty($value['highlight']))
	{
	$emergency_jobs[$key]['jobs_name']="<span style=\"color:{$value['highlight']}\">{$value['jobs_name']}</span>";
	}
}
$smarty->assign('emergency_jobs',$emergency_jobs);
$smarty->display("wap/wap.html");
?>