<?php
 /*
 * 74cms 管理中心 插件管理
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
//获取插件列表
function get_plug($type="")
{
	global $db;
	if (!empty($type)) $wheresql="  WHERE p_install=".intval($type)."  ";
	$sql = "select * from ".table('plug')." ".$wheresql;
	$list=$db->getall($sql);
	return $list;
}
//卸载插件
function uninstall_plug($id)
{
global $db;
if (!intval($id)) return false;
$sql= "UPDATE ".table('plug')." SET p_install='1' WHERE id='$id'";
if (!$db->query($sql))return false;
return true;
}
//安装插件
function install_plug($id)
{
global $db;
if (!intval($id)) return false;
$sql= "UPDATE ".table('plug')." SET p_install='2' WHERE id='$id'";
if (!$db->query($sql))return false;
return true;
}
?>