<?php
 /*
 * 74cms ajax 会员中心
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
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
			write_memberslog($_SESSION['uid'],1,2006,$_SESSION['username'],"查看了 {$user['username']} 的职位申请");
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