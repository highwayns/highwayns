<?php
 
define('IN_HIGHWAY', true);
require_once(dirname(dirname(__FILE__)).'/include/plus.common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'HW_jobslist';
if (strcasecmp(HIGHWAY_DBCHARSET,"utf8")!=0)
{
$_GET['key']=utf8_to_gbk($_GET['key']);
}
unset($_GET['act']);
$_GET=array_map("rawurlencode",$_GET);
$url=url_rewrite($act,$_GET);
unset($_GET);
exit($url);
?>
