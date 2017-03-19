<?php
 /*
 * 74cms HR
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
function get_hrtools_category()
{
	global $db;
	$sql = "select * from ".table('hrtools_category')."  order BY c_order desc,c_id ASC";
	return $db->getall($sql);
}
function get_hrtools_category_one($id)
{
	global $db;
	$sql = "select * from ".table('hrtools_category')." where c_id=".intval($id)." LIMIT 1";
	return $db->getone($sql);
}
function get_hrtools($offset, $perpage, $sql= '')
{
	global $db,$_CFG;
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT * FROM ".table('hrtools')." AS h ".$sql.$limit);
	while($row = $db->fetch_array($result))
	{
		if ($row['h_strong']=="1")
		{
		$row['h_filename']="<strong>{$row['h_filename']}</strong>";
		}
		if ($row['h_color'])
		{
		$row['h_filename']="<span style=\"color:{$row['h_color']}\">{$row['h_filename']}</span>";
		}
		$row['h_fileurl']=substr($row['h_fileurl'],0,7)=="http://"?$row['h_fileurl']:$_CFG['site_dir'].$row['h_fileurl'];
	$row_arr[] = $row;
	}
	return $row_arr;
}
function del_hrtools($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('hrtools')." WHERE h_id IN (".$sqlin.") ")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
function del_hrtools_category($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('hrtools_category')." WHERE c_id IN (".$sqlin.")  AND c_adminset<>1")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
?>