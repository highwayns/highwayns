<?php
 /*
 * 74cms 管理中心 关键词
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
function get_hotword($offset, $perpage, $wheresql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	return $db->getall("SELECT * FROM ".table('hotword').$wheresql.$limit);
}
function get_hotword_one($id)
{
	global $db;
	$sql = "select * from ".table('hotword')." where w_id=".intval($id)." LIMIT 1";
	return $db->getone($sql);
}
function get_hotword_obtainword($word)
{
	global $db;
	$sql = "select * from ".table('hotword')." where w_word='".trim($word)."'  LIMIT 1";
	return $db->getone($sql);
}
function del_hottype($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('hotword')." WHERE w_id IN (".$sqlin.") ")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
?>