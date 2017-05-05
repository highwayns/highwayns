<?php
if(!defined('IN_HIGHWAY'))
{
 die('Access Denied!');
}
//微招聘列表
function get_simple_list($offset, $perpage, $sql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT {$offset},{$perpage} ";
	$result = $db->query("SELECT * FROM ".table('simple')." {$sql} {$limit}");
	while($row = $db->fetch_array($result))
	{
	$row_arr[] = $row;
	}
	return $row_arr;
}
function simple_del($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('simple')." WHERE id IN (".$sqlin.")")) return false;
		$return=$return+$db->affected_rows();
		//填写管理员日志
		write_log("削除idは".$sqlin."の募集 , 削除件数".$return."行", $_SESSION['admin_name'],3);
	}
	return $return;
}
function simple_refresh($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("update  ".table('simple')." SET refreshtime='".time()."'  WHERE id IN (".$sqlin.")")) return false;
		$return=$return+$db->affected_rows();
		//填写管理员日志
		write_log("更新idは".$sqlin."の微募集 , 更新件数".$return."行", $_SESSION['admin_name'],3);
	}
	return $return;
}
//微招聘审核
function simple_audit($id,$audit)
{
	global $db;
	if (!is_array($id))$id=array($id);
	$return=0;
	$sqlin=implode(",",$id);	
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("update  ".table('simple')." SET audit='".intval($audit)."'  WHERE id IN (".$sqlin.")")) return false;
		$return=$return+$db->affected_rows();
		//填写管理员日志
		write_log("審査idは".$sqlin."の微募集 , 審査件数".$return."行", $_SESSION['admin_name'],3);
	}
	return $return;
}
?>
