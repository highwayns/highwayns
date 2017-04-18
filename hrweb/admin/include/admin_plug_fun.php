<?php
 if(!defined('IN_HIGHWAY'))
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
