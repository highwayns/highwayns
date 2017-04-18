<?php
define('IN_HIGHWAY', true);
$page_select="user";
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$text=get_cache('text');
$smarty->assign('agreement',$text['agreement']);
$smarty->display('agreement/agreement.htm');
unset($smarty);
?>
