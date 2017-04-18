<?php
if(!file_exists(dirname(__FILE__).'/data/install.lock')) header("Location:install/index.php");
define('IN_HIGHWAY', true);
$alias="HW_index";
require_once(dirname(__FILE__).'/include/common.inc.php');
if(browser()=="mobile" && $_GET['iswap']==""){
	header("location:".$_CFG['wap_domain']);
}
if($mypage['caching']>0){
        $smarty->cache =true;
		$smarty->cache_lifetime=$mypage['caching'];
	}else{
		$smarty->cache = false;
	}
$cached_id=$alias.(isset($_GET['id'])?"|".(intval($_GET['id'])%100).'|'.intval($_GET['id']):'').(isset($_GET['page'])?"|p".intval($_GET['page'])%100:'');
if(!$smarty->is_cached($mypage['tpl'],$cached_id))
{
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
unset($dbhost,$dbuser,$dbpass,$dbname);
$smarty->display($mypage['tpl'],$cached_id);
}
else
{
$smarty->display($mypage['tpl'],$cached_id);
}
unset($smarty);
?>
