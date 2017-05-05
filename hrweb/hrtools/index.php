<?php
define('IN_HIGHWAY', true);
$alias="HW_hrtoolsindex";
require_once(dirname(__FILE__).'/../include/common.inc.php');
if ($_PLUG['hrtools']['p_install']==1)
{
	$link[0]['text'] = "トップに戻る";
	$link[0]['href'] = $_CFG['site_dir'];
	showmsg("管理者このモジュールを閉じた!",1,$link);
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
}
$smarty->display($mypage['tpl'],$cached_id);
unset($smarty);
?>
