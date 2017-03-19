<?php
 /*
 * 74cms 帮助
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
function get_pmssys($offset, $perpage, $sql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT * FROM ".table('pms_sys')."{$sql}  {$limit}");
	while($row = $db->fetch_array($result))
	{
	$row['message1']=cut_str($row['message'],18,0,"...");
	$row_arr[] = $row;
	}
	return $row_arr;
}
function del_pms_sys($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('pms_sys')." WHERE spmid IN ({$sqlin}) ")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
?>