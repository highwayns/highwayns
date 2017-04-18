<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
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
