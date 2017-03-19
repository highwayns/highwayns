<?php
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
$act = isset($_REQUEST['act']) ? trim($_REQUEST['act']) : 'app';
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$captcha=get_cache('captcha');
$smarty->assign('verify_userlogin',$captcha['verify_userlogin']);
$smarty->display('plus/ajax_login.htm');
exit();