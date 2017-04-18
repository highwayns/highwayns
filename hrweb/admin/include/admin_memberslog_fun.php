<?php
if(!defined('IN_HIGHWAY'))
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
