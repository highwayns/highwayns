<?php
 /*
 * 74cms 审核不通过原因
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
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
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