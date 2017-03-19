<?php
 /*
 * 74cms 管理中心 说明页 数据调用函数
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
function get_notice($offset, $perpage, $get_explain_sql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT n.*,c.categoryname FROM ".table('notice')." AS n".$get_explain_sql.$limit);
	while($row = $db->fetch_array($result))
	{
	$tit_color = $row['tit_color'] ? "color:".$row['tit_color'].";" : '';
	$tit_b = $row['tit_b']>0 ? "font-weight:bold;" : '';
	$tit_style= $tit_color || $tit_b ? "style=\"".$tit_color.$tit_b."\""  : '';
	$url = $row['is_url']=="http://" ? url_rewrite('QS_noticeshow',array('id'=>$row['id'])): $row['is_url'];
	$row['url_title'] ="<a href=\"".$url."\" target=\"_blank\" ".$tit_style.">".$row['title']."</a> ";
	$row_arr[] = $row;
	}
	return $row_arr;
}
function get_notice_one($id)
{
	global $db;
	$sql = "select * from ".table('notice')." where id=".intval($id)." LIMIT 1";
	$notice=$db->getone($sql);
	return $notice;
}
function del_notice($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('notice')." WHERE id IN (".$sqlin.") ")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
function get_notice_category(){
	global $db;
	$sql = "select * from ".table('notice_category')."  order BY sort desc,id asc";
	return $db->getall($sql);
}
function get_notice_category_one($id)
{
	global $db;
	$sql = "select * from ".table('notice_category')." where id=".intval($id)." LIMIT 1";
	$category=$db->getone($sql);
	return $category;
}
function del_notice_category($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('notice_category')." WHERE id IN (".$sqlin.")  AND admin_set<>1")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
?>