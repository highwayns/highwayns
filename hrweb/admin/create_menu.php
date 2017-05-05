<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$access_token = get_access_token();
if(empty($access_token)){
	adminmsg("access_token取得失敗！",1);
}
$jsonmenu = get_weixin_json_menu();
$url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=".$access_token;
$result = https_request($url, $jsonmenu);
$result_arr = json_decode($result,true);
if($result_arr['errmsg']=='ok'){
  adminmsg("メニュー同期成功！",2);
}else{
  adminmsg("メニュー同期失敗，コードチェックしてください！",1);
}
?>
