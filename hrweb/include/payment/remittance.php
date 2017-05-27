<?php

 if(!defined('IN_HIGHWAY'))
 {
 	die('Access Denied!');
 }
 //获取安装代码
function pay_info()
{
$arr['p_introduction']="振込説明：";
$arr['notes']="振り込み説明：";
$arr['fee']="支払い費用：";
return $arr;
}
?>
