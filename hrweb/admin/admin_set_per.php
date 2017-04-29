<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'set';
$smarty->assign('act',$act);
$smarty->assign('pageheader',"个人设置");
check_permissions($_SESSION['admin_purview'],"set_per");	
if($act == 'set')
{
	get_token();	
	$smarty->assign('config',$_CFG);
	$smarty->assign('text',get_cache('text'));
	$smarty->display('set_per/admin_set_per.htm');
}
elseif($act == 'set_save')
{
	check_token();
	//填写管理员日志
	write_log("后台更新设置", $_SESSION['admin_name'],3);
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('config')." SET value='$v' WHERE name='$k'")?adminmsg('設定更新失敗', 1):"";
	}
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('text')." SET value='$v' WHERE name='$k'")?adminmsg('設定更新失敗', 1):"";
	}
	refresh_cache('config');
	refresh_cache('text');
	adminmsg("保存成功！",2);
}
?>
