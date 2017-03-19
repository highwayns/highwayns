<?php
 /*
 * 74cms 管理中心 设置微信菜单 数据调用函数
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
function get_weixin_menu()
{
	global $db;
	$menu_list = array();
	$sql = "select * from ".table('weixin_menu')." where parentid=0 order BY menu_order desc,id asc";
	$parent_menu = $db->getall($sql);
	foreach($parent_menu as $p){
		$menu_list[$p['id']] = $p;
		$sub_menu = $db->getall("select * from ".table('weixin_menu')." where parentid=".$p['id']);
		foreach ($sub_menu as $key => $value) {
			$menu_list[$p['id']]['child_menu'][] = $value;
		}
	}
	return $menu_list;
}
function get_weixin_menu_one($id)
{
	global $db;
	$sql = "select * from ".table('weixin_menu')." WHERE id=".intval($id)." LIMIT 1";
	return $db->getone($sql);
}
function get_parent_menu()
{
	global $db;
	$sql = "select * from ".table('weixin_menu')." WHERE parentid=0";
	return $db->getall($sql);
}
function del_menu($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('weixin_menu')." WHERE id IN (".$sqlin.") ")) return false;
		$return=$return+$db->affected_rows();
		if (!$db->query("Delete from ".table('weixin_menu')." WHERE parentid IN (".$sqlin.") ")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
function get_binding_list()
{
	global $db;
	$sql = "select * from ".table('members')." where weixin_openid!='' order BY bindingtime desc";
	$binding_list = $db->getall($sql);
	return $binding_list;
}
function del_binding($uid)
{
	global $db;
	if(!is_array($uid)) $uid=array($uid);
	$return=0;
	$sqlin=implode(",",$uid);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("update ".table('members')." set weixin_openid=null,weixin_nick='',bindingtime=0 WHERE uid IN (".$sqlin.") ")) return false;
		$return=$return+$db->affected_rows();
	}
	return $return;
}
?>