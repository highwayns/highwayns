<?php
 /*
 * 74cms 会员中心公用函数
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
function get_user_inemail($email)
{
global $db;
$sql = "select * from ".table('members')." where email = '$email'";
return $db->getone($sql);
}
function get_user_inusername($username)
{
global $db;
$sql = "select * from ".table('members')." where username = '$username'";
return $db->getone($sql);
}
function get_user_inmobile($mobile)
{
global $db;
$sql = "select * from ".table('members')." where mobile = '$mobile'";
return $db->getone($sql);
}
function randstr($length=6)
{   
	$hash='';
	$chars= 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz@#!~?:-=';   
	$max=strlen($chars)-1;   
	mt_srand((double)microtime()*1000000);   
	for($i=0;$i<$length;$i++){   
	$hash.=$chars[mt_rand(0,$max)];   
	}   
	return $hash;   
}
?>