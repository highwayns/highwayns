<?php
 /*
 * 74cms 管理中心 支付方式
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
//获取支付方式列表
function get_payment($type="")
{
	global $db;
	if (!empty($type)) $wheresql="  WHERE p_install=".intval($type)."  ";
	$sql = "select * from ".table('payment')." ".$wheresql." order BY listorder desc";
	$list=$db->getall($sql);
	return $list;
}
//获取单条支付方式
function get_payment_one($name){
global $db;
$sql = "select * from ".table('payment')." WHERE typename='".$name."'";
$info=$db->getone($sql);
return $info;
}
//卸载支付方式
function uninstall_payment($id)
{
global $db;
if (!intval($id)) return false;
$sql= "UPDATE ".table('payment')." SET p_install='1' WHERE id='$id'";
if (!$db->query($sql))return false;
write_log("卸载id为".$id."的支付方式", $_SESSION['admin_name'],3);
return true;
}
//修改支付列表排序
function edit_payment_listorder($id,$eid)
{
global $db;
if (!intval($id) || !intval($eid)) return false;
$sql= "UPDATE ".table('payment')." SET listorder='$eid' WHERE id='$id'";
if (!$db->query($sql))return false;
return true;
}
?>