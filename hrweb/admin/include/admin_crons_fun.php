<?php
 /*
 * 74cms 计划任务
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
function get_crons($offset, $perpage, $get_sql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT * FROM ".table('crons')." ".$get_sql.$limit);
	while($row = $db->fetch_array($result))
	{
		switch ($row['weekday'])
		{
		case "-1":
		  $row['weekdaycn']="";
		  break;
		case 0:
		  $row['weekdaycn']="每周日";
		  break;
		case 1:
		   $row['weekdaycn']="每周一";
		  break;
		case 2:
		   $row['weekdaycn']="每周二";
		  break;
		case 3:
		   $row['weekdaycn']="每周三";
		  break;
		case 4:
		   $row['weekdaycn']="每周四";
		  break;
		case 5:
		   $row['weekdaycn']="每周五";
		  break;
		case 6:
		   $row['weekdaycn']="每周六";
		  break;
		default:
		 $row['weekdaycn']="";
		}
	
	$row_arr[] = $row;
	}
	return $row_arr;	
}
function del_crons($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('crons')." WHERE  cronid  IN (".$sqlin.") AND admin_set<>1 ")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
function get_crons_one($id)
{
	global $db;
	$sql = "select * from ".table('crons')." where cronid=".intval($id)."";
	return $db->getone($sql);
}
?>