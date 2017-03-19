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
function get_memberslog_list($offset,$perpage,$sql= '')
{
	global $db;
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT * FROM ".table('members_log')." ".$sql.$limit);
	while($row = $db->fetch_array($result))
	{
	$row_arr[] = $row;
	}
	return $row_arr;
}
function del_memberslog($id)
{
	global $db;
	$delnum=0;
	if (!is_array($id)) $id=array($id);
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		$db->query("Delete from ".table('members_log')." WHERE log_id IN (".$sqlin.")");
		$delnum=$db->affected_rows();
	}
	return $delnum;
}
function pidel_memberslog($log_type,$starttime,$endtime)
{
	global $db;
	$delnum=0;
	$starttime=intval($starttime);
	$endtime=intval($endtime);
	if (!is_array($log_type)) $l_type=array($log_type);
	$sqlin=implode(",",$log_type);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin) && $starttime && $endtime)
	{
		$db->query("Delete from ".table('members_log')." WHERE log_addtime>{$starttime} and  log_addtime<{$endtime}  and log_type IN (".$sqlin.")");
		$delnum=$db->affected_rows();
	}
	return $delnum;
}
?>