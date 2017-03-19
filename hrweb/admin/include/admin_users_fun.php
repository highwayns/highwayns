<?php
 /*
 * 74cms 管理中心 管理员账户相关函数
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
 function get_admin_list($offset,$perpage,$get_sql= '')
{
	global $db;
	$row_arr = array();
	if(isset($offset)&&!empty($perpage))
	{
	$limit=" LIMIT ".$offset.','.$perpage;
	}
	$result = $db->query("SELECT * FROM ".table('admin')." ".$get_sql."  ".$limit);
	while($row = $db->fetch_array($result))
	{
	$row_arr[] = $row;
	}
	return $row_arr;
}
function del_users($id,$purview='')
{
	global $db;
	$return=0;
	if ($purview<>"all") return false;
	if (!$db->query("Delete from ".table('admin')." WHERE admin_id=".intval($id)." AND purview<>'all' ")) return false;
	$return=$return+$db->affected_rows();	
	//填写管理员日志
	write_log("后台删除id为".$id."的管理员", $_SESSION['admin_name'],3);
	return $return;
}
function get_admin_log($offset,$perpage,$get_sql= '')
{
	global $db;
	$limit=" LIMIT ".$offset.','.$perpage;
	$sql="SELECT * FROM ".table('admin_log')." ".$get_sql." order BY log_id DESC ".$limit;
	return $db->getall($sql);
}
//从ID获取账户信息
function get_admin_account($admin_id)
{
	global $db;
	$admin_id=intval($admin_id);
	$account = $db->getone("SELECT * FROM ".table('admin')." WHERE admin_id = '$admin_id'");
	return $account;
}
//获取随机字符串
 function randstr($length=6)
{
	$hash='';
	$chars= 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz@#!~?:-=';   
	$max=strlen($chars)-1;   
	mt_srand((double)microtime()*1000000);   
	for($i=0;$i<$length;$i++)   {   
	$hash.=$chars[mt_rand(0,$max)];   
	}   
	return $hash;   
}
?>