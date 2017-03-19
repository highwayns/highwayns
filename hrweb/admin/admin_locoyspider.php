<?php
 /*
 * 74cms 火车头采集器设置
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
require_once(ADMIN_ROOT_PATH.'include/admin_locoyspider_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'set';
check_permissions($_SESSION['admin_purview'],"locoyspider");
	require_once(ADMIN_ROOT_PATH.'include/admin_article_fun.php');
$show=get_cache('locoyspider');
$smarty->assign('show',$show);
$smarty->assign('pageheader',"火车头采集");
if($act=="set")
{	
	get_token();
	$smarty->assign('navlabel',"set");
	$smarty->display('locoyspider/admin_locoyspider.htm');
}
elseif($act=="set_news")
{	
	get_token();
	$smarty->assign('navlabel',"set_news");
	$smarty->display('locoyspider/admin_locoyspider_news.htm');
}
elseif($act=="set_company")
{	
	get_token();
	$smarty->assign('navlabel',"set_company");
	$smarty->display('locoyspider/admin_locoyspider_company.htm');
}
elseif($act=="set_jobs")
{	
	get_token();
	$smarty->assign('navlabel',"set_jobs");
	$smarty->display('locoyspider/admin_locoyspider_jobs.htm');
}
elseif($act=="set_user")
{	
	get_token();
	$smarty->assign('navlabel',"set_user");
	$smarty->display('locoyspider/admin_locoyspider_user.htm');
}
elseif($act == 'set_save')
{
	check_token();
	if (intval($_POST['search_threshold'])>100 || intval($_POST['search_threshold'])==0) unset($_POST['search_threshold']);
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('locoyspider')." SET value='$v' WHERE name='$k' LIMIT 1")?adminmsg('更新失败', 1):"";
	}
	refresh_cache('locoyspider');
	write_log("设置火车头配置", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
?>