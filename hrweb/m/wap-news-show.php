<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$smarty->cache = false;
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$smarty->assign('show',news_one($_GET['id']));
$smarty->assign('goback',$_SERVER["HTTP_REFERER"]);
$smarty->display("wap/wap-news-show.html");
?>
