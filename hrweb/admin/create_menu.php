<?php
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$access_token = get_access_token();
if(empty($access_token)){
	adminmsg("access_token获取失败！",1);
}
$jsonmenu = get_weixin_json_menu();
$url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=".$access_token;
$result = https_request($url, $jsonmenu);
$result_arr = json_decode($result,true);
if($result_arr['errmsg']=='ok'){
  adminmsg("同步菜单成功！",2);
}else{
  adminmsg("同步菜单失败，请检查代码！",1);
}
?>