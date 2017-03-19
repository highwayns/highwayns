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
//获取说明页分类
function get_explain_category()
{
	global $db;
	$sql = "select * from ".table('explain_category')."  order BY category_order desc,id asc";
	$category_list=$db->getall($sql);
	return $category_list;
}
function get_explain_category_one($id)
{
	global $db;
	$sql = "select * from ".table('explain_category')." where id=".intval($id)." LIMIT 1";
	$category=$db->getone($sql);
	return $category;
}
 //读取说明页
function get_explain($offset, $perpage, $sql= '')
{
	global $db;
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT e.*,c.categoryname FROM ".table('explain')." AS e ".$sql.$limit);
	while($row = $db->fetch_array($result)){
	$tit_color = $row['tit_color'] ? "color:".$row['tit_color'].";" : '';
	$tit_b = $row['tit_b']>0 ? "font-weight:bold;" : '';
	$tit_style= $tit_color || $tit_b ? "style=\"".$tit_color.$tit_b."\""  : '';
	if (!empty($row['is_url']) && $row['is_url']!='http://')
	{
	$row['url'] = $row['is_url'];
	}
	else
	{
	$row['url'] = url_rewrite('QS_explainshow',array('id'=>$row['id']));
	}
	$row['url_title'] ="<a href=\"".$row['url']."\" target=\"_blank\" ".$tit_style.">".$row['title']."</a> ";
	$row_arr[] = $row;
	}
	return $row_arr;
}
function del_explain($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('explain')." WHERE id IN (".$sqlin.") ")) return false;
		$return=$return+$db->affected_rows();
		write_log("删除id为".$sqlin."的说明页,共删除".$return."行", $_SESSION['admin_name'],3);
	}
	return $return;
}
function del_category($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('explain_category')." WHERE id IN (".$sqlin.")  AND admin_set<>1")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
?>