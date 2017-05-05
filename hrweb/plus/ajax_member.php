<?php
define('IN_HIGHWAY', true);
require_once(dirname(dirname(__FILE__)).'/include/plus.common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : ''; 
if($act == 'edit_apply')
{
	$id=intval($_GET['id']);
	if ($id>0)
	{
	$setsqlarr['personal_look']=2;
	$db->updatetable(table('personal_jobs_apply'),$setsqlarr," did='".$id."' LIMIT 1");
			$sql="select m.username from ".table('personal_jobs_apply')." AS a JOIN ".table('members')." AS m ON a.personal_uid=m.uid WHERE a.did='{$id}' LIMIT 1";
			$user=$db->getone($sql);
			write_memberslog($_SESSION['uid'],1,2006,$_SESSION['username'],"{$user['username']} の職位申し込みを閲覧しました");
	exit("ok");
	}
}
elseif($act == 'edit_interview')
{
	$id=intval($_GET['id']);
	if ($id>0)
	{
	$setsqlarr['personal_look']=2;
	if ($db->updatetable(table('company_interview'),$setsqlarr," did='".$id."' LIMIT 1"))exit("ok");
	}
}
?>
