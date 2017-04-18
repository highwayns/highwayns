<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$smarty->cache = false;
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$downtype=$_GET['downtype']?$_GET['downtype']:"android";
$smarty->assign("downtype",$downtype);
$smarty->display("wap/download.html");
?>
