<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'qq_set';
$smarty->assign('act',$act);
$smarty->assign('navlabel',$act);
$smarty->assign('pageheader',"第三方アカウント登録");	
if($act == 'qq_set')
{
	check_permissions($_SESSION['admin_purview'],"set_qqconnect");	
	get_token();	
	$smarty->assign('config',$_CFG);
	$smarty->display('openconnect/admin_qqconnect.htm');
}
elseif($act == 'set_qq_save')
{
	check_permissions($_SESSION['admin_purview'],"set_qqconnect");	
	check_token();
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('config')." SET value='$v' WHERE name='$k'")?adminmsg('設定更新失敗', 1):"";
	}
	refresh_cache('config');
	write_log("QQ登録設定", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
elseif($act == 'sina_set')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"set_sinaconnect");
	$smarty->assign('config',$_CFG);
	$smarty->display('openconnect/admin_sinaconnect.htm');
}
elseif($act == 'set_sina_save')
{
	check_permissions($_SESSION['admin_purview'],"set_sinaconnect");	
	check_token();
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('config')." SET value='$v' WHERE name='$k'")?adminmsg('設定更新失敗', 1):"";
	}
	refresh_cache('config');
	write_log("第三方登録sinaを設定", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
elseif($act == 'taobao_set')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"set_taobaoconnect");
	$smarty->assign('config',$_CFG);
	$smarty->display('openconnect/admin_taobaoconnect.htm');
}
elseif($act == 'set_taobao_save')
{
	check_permissions($_SESSION['admin_purview'],"set_taobaoconnect");	
	check_token();
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('config')." SET value='$v' WHERE name='$k'")?adminmsg('設定更新失敗', 1):"";
	}
	refresh_cache('config');
	write_log("taobao登録設定", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
?>
