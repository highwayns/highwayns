<?php
 /*
 * 74cms 
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
function get_syslog_list($offset,$perpage,$sql= '')
{
	global $db;
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT * FROM ".table('syslog')." ".$sql.$limit);
	while($row = $db->fetch_array($result))
	{
	$row['l_page']=urldecode($row['l_page']);
	$row['l_str'] = str_replace('<p>','<br />',$row['l_str']);
	$row['l_str'] = str_replace('</p>','',$row['l_str']);
	$row_arr[] = $row;
	}
	return $row_arr;
}
function del_syslog($id)
{
	global $db;
	$delnum=0;
	if (!is_array($id)) $id=array($id);
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		$db->query("Delete from ".table('syslog')." WHERE l_id IN (".$sqlin.")");
		$delnum=$db->affected_rows();
	}
	return $delnum;
}
function pidel_syslog($l_type,$starttime,$endtime)
{
	global $db;
	$delnum=0;
	$starttime=intval($starttime);
	$endtime=intval($endtime);
	if (!is_array($l_type)) $l_type=array($l_type);
	$sqlin=implode(",",$l_type);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin) && $starttime && $endtime)
	{
		$db->query("Delete from ".table('syslog')." WHERE l_time>{$starttime} and  l_time<{$endtime} and l_type IN (".$sqlin.")");
		$delnum=$db->affected_rows();
	}
	return $delnum;
}
?>