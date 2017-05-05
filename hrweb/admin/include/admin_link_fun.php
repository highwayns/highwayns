<?php
 if(!defined('IN_HIGHWAY'))
 {
 	die('Access Denied!');
 }
function get_links($offset, $perpage, $get_sql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT l.*,c.categoryname FROM ".table('link')." AS l ".$get_sql.$limit);
	while($row = $db->fetch_array($result))
	{
	$row_arr[] = $row;
	}
	return $row_arr;	
}
function get_links_one($id)
{
	global $db;
	$sql = "select * from ".table('link')." where link_id=".intval($id)."";
	$link=$db->getone($sql);
	return $link;
}
function del_link($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('link')." WHERE link_id IN (".$sqlin.")")) return false;
		$return=$return+$db->affected_rows();
		//填写管理员日志
		write_log("削除idは".$sqlin."の相互リンク", $_SESSION['admin_name'],3);
	}
	return $return;
}
function get_link_category()
{
	global $db;
	$sql = "select * from ".table('link_category')."";
	$info=$db->getall($sql);
	return $info;
}
function get_link_category_name($val)
{
	global $db;
	$sql = "select * from ".table('link_category')." where c_alias='".$val."'";
	$category=$db->getone($sql);
	return $category;
}
function del_category($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('link_category')." WHERE id IN (".$sqlin.")  AND c_sys<>1")) return false;
		$return=$return+$db->affected_rows();
		//填写管理员日志
		write_log("相互リンク分類削除", $_SESSION['admin_name'],3);
	}
	return $return;
}
?>
