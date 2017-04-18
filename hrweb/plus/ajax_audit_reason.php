<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'jobs_reason'; 
$id=intval($_GET['id']);
if($act=='jobs_reason'){
	$column="jobs_id";
}elseif($act=='resume_reason'){
	$column="resume_id";
}elseif($act=='company_reason'){
	$column="company_id";
}
if ($id)
{
	$result = $db->getone("SELECT * FROM ".table('audit_reason')." WHERE `{$column}`={$id} ORDER BY id DESC LIMIT 1");
	if(empty($result) && $column=='company_id'){exit('现在提交认证资料，认证通过后可增加信息的可信度，还可能额外赠送积分哦！');}
	exit($result['reason']);
}
?>
