<?php
 if(!defined('IN_HIGHWAY'))
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
