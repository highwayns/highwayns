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
	if(empty($result) && $column=='company_id'){exit('認定資料を提出してください，認定するとポイントを送ります！');}
	exit($result['reason']);
}
?>
