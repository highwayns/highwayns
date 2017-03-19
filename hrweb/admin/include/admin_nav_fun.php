<?php
 /*
 * 74cms 管理中心 网站设置 数据调用函数
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
//获取导航所有
function get_nav($alias=NULL)
{
	global $db;
	if ($alias) $wheresql=" WHERE alias='".trim($alias)."' ";
	$result = $db->query("SELECT * FROM ".table('navigation').$wheresql." order BY display desc,navigationorder desc,id asc");
	while($row = $db->fetch_array($result))
	{
	$category=get_nav_cat_one($row['alias']);
	$row['categoryname']=$category['categoryname'];
	$row_arr[] = $row;
	}
	return $row_arr;
}
//获取导航（单个）
function get_nav_one($id)
{
	global $db;
	$id=intval($id);
	$sql = "select * from ".table('navigation')." where id=".$id;
	$category_one=$db->getone($sql);
	if ($category_one['systemclass']=="1")
	{
	$category_one['url']=url_rewrite($category_one['module'],$category_one['module_page']);
	}
	$category_one['url_str']=cut_str($category_one['url'],12,0);
	return $category_one;
}
//删除导航栏目
function del_navigation($del_id)
{
	global $db;
	if (!is_array($del_id))$del_id=array($del_id);
	$sqlin=implode(",",$del_id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
	if (!$db->query("Delete from ".table('navigation')." WHERE id IN (".$sqlin.")")) return false;
	return true;
	}
	return false;
}
//获取所有导航分类
function get_nav_cat()
{
	global $db;
	$sql = "select * from ".table('navigation_category');
	$list=$db->getall($sql);
	return $list;
}
//获取单个导航分类
function get_nav_cat_one($alias)
{
	global $db;
	$sql = "select * from ".table('navigation_category')." where alias='".trim($alias)."'";
	return $db->getone($sql);
}
function del_nav_cat($id)
{
	global $db;
	if (!$db->query("Delete from ".table('navigation_category')." WHERE id=".intval($id)." AND admin_set<>1")) return false;
	return true;
}
?>