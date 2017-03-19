<?php
 /*
 * 74cms 管理中心 友情链接 数据调用函数
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
 if(!defined('IN_QISHI'))
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
		write_log("后台删除id为".$sqlin."的友情链接", $_SESSION['admin_name'],3);
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
		write_log("后台删除友情链接分类", $_SESSION['admin_name'],3);
	}
	return $return;
}
?>