<?php
if(!defined('IN_HIGHWAY'))
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
