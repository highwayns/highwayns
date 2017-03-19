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
function get_help($offset, $perpage, $sql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT a.*,c.id as cid,c.categoryname as c_categoryname FROM ".table('help')." AS a {$sql}  {$limit}");
	while($row = $db->fetch_array($result))
	{
	$row['url'] = url_rewrite('QS_helpshow',array('id'=>$row['id']));
	$row_arr[] = $row;
	}
	return $row_arr;
}
function del_help($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('help')." WHERE id IN ({$sqlin}) ")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
function get_help_category()
{
	global $db;
	$sql = "select * from ".table('help_category')." where parentid=0  ORDER BY category_order desc";
	$category_list=$db->getall($sql);
	if (is_array($category_list))
	{
	foreach($category_list as $v)
	{
	$list[]=array("id"=>$v['id'],"categoryname"=>$v['categoryname'],"parentid"=>$v['parentid']);
		$sqlf = "select * from ".table('help_category')." where parentid=".$v['id']."  ORDER BY category_order desc";
		$category_f=$db->getall($sqlf);
		if (is_array($category_f))
		{
			foreach($category_f as $vs)
			{
			$list[]=array("id"=>$vs['id'],"categoryname"=>"|-".$vs['categoryname'],"parentid"=>$vs['parentid']);
			}
		}
	}
	}
	return $list;
}
function get_help_category_one($id)
{
	global $db;
	$sql = "select * from ".table('help_category')." where id=".intval($id)." LIMIT 1";
	$var=$db->getone($sql);
	return $var;
}
function get_help_parentid($val)
{
	global $db;
	$sql = "select * from ".table('help_category')." where id=".intval($val)."  LIMIT 1";
	$category=$db->getone($sql);
	return $category['parentid'];
}
function del_category($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	foreach($id as $sid)
	{
		$sql = "select * from ".table('help_category')." where id=".intval($sid)."  LIMIT 1";
		$category=$db->getone($sql);
		if ($category['parentid']=="0")
		{
			if (!$db->query("Delete from ".table('help_category')." WHERE (parentid =".intval($sid)." OR id =".intval($sid).")")) return false;
			$return=$return+$db->affected_rows();
		}
		else
		{
			if (!$db->query("Delete from ".table('help_category')." WHERE id =".intval($sid)." ")) return false;
			$return=$return+$db->affected_rows();
		}
	}
	return $return;
}
?>