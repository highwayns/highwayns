<?php
 /*
 * 74cms 合作帐号登录
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'qq_set';
$smarty->assign('act',$act);
$smarty->assign('navlabel',$act);
$smarty->assign('pageheader',"第三方帐号登录");	
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
	!$db->query("UPDATE ".table('config')." SET value='$v' WHERE name='$k'")?adminmsg('更新设置失败', 1):"";
	}
	refresh_cache('config');
	write_log("设置第三方登录QQ", $_SESSION['admin_name'],3);
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
	!$db->query("UPDATE ".table('config')." SET value='$v' WHERE name='$k'")?adminmsg('更新设置失败', 1):"";
	}
	refresh_cache('config');
	write_log("设置第三方登录sina", $_SESSION['admin_name'],3);
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
	!$db->query("UPDATE ".table('config')." SET value='$v' WHERE name='$k'")?adminmsg('更新设置失败', 1):"";
	}
	refresh_cache('config');
	write_log("设置第三方登录taobao", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
?>