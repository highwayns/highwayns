<?php
 /*
 * 74cms 系统设置
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
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'set';
check_permissions($_SESSION['admin_purview'],"site_set");
$smarty->assign('pageheader',"网站配置");
if($act == 'set')
{	
	get_token();
	$smarty->assign('rand',rand(1,100));
	$smarty->assign('upfiles_dir',$upfiles_dir);	
	$smarty->assign('config',get_cache('config'));
	$smarty->assign('navlabel',"set");	
	$smarty->display('set/admin_set_config.htm');
}
elseif($act == 'site_setsave')
{
	check_token();
	require_once(ADMIN_ROOT_PATH.'include/upload.php');
		if($_FILES['web_logo']['name'])
		{
			$web_logo=_asUpFiles($upfiles_dir, "web_logo", 1024*2, 'jpg/gif/png',"logo");
			if(!$db->query("UPDATE ".table('config')." SET value='$web_logo' WHERE name='web_logo'"))
			{
				//填写管理员日志
				write_log("后台设置网站LOGO失败", $_SESSION['admin_name'],3);
				adminmsg('更新站点设置失败', 1);
			}
			//填写管理员日志
			write_log("后台成功设置网站LOGO", $_SESSION['admin_name'],3);
		}

		if($_FILES['body_bgimg']['name'])
		{
			$body_bgimg=_asUpFiles($upfiles_dir, "body_bgimg", 1024*2, 'jpg/gif/png',"body_bg_img");
			if(!$db->query("UPDATE ".table('config')." SET value='$body_bgimg' WHERE name='body_bgimg'"))
			{
				//填写管理员日志
				write_log("后台设置网站背景失败", $_SESSION['admin_name'],3);
				adminmsg('更新站点设置失败', 1);
			}
			//填写管理员日志
			write_log("后台成功设置网站背景", $_SESSION['admin_name'],3);
		}
		if($_POST['set_body_bgimg_defaule']==1)
		{
			@unlink($upfiles_dir.$_CFG["body_bgimg"]);
			if(!$db->query("UPDATE ".table('config')." SET value='' WHERE name='body_bgimg'"))
			{
				//填写管理员日志
				write_log("后台设置网站默认背景失败", $_SESSION['admin_name'],3);
				adminmsg('更新站点设置失败', 1);
			}
			//填写管理员日志
			write_log("后台成功设置网站默认背景", $_SESSION['admin_name'],3);
		}
		foreach($_POST as $k => $v)
		{
		!$db->query("UPDATE ".table('config')." SET value='{$v}' WHERE name='{$k}'")?adminmsg('更新站点设置失败', 1):"";
		}
		refresh_cache('config');
		//填写管理员日志
		write_log("后台成功设置网站配置", $_SESSION['admin_name'],3);
		adminmsg("保存成功！",2);
}
elseif($act == 'map')
{
	get_token();
	$smarty->assign('config',$_CFG);
	$smarty->assign('navlabel',"map");	
	$smarty->display('set/admin_set_map.htm');
}
elseif($act == 'agreement')
{
	get_token();
	$smarty->assign('config',$_CFG);
	$smarty->assign('text',get_cache('text'));
	$smarty->assign('navlabel',"agreement");	
	$smarty->display('set/admin_set_agreement.htm');
}
elseif($act == 'set_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"mb_set");
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('config')." SET value='{$v}' WHERE name='{$k}'")?adminmsg('更新设置失败', 1):"";
	}
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('text')." SET value='{$v}' WHERE name='{$k}'")?adminmsg('更新设置失败', 1):"";
	}
	refresh_cache('config');
	refresh_cache('text');
	//填写管理员日志
	write_log("后台成功更新设置", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
elseif($act == 'search')
{
	get_token();
	$smarty->assign('pageheader',"搜索设置");
	$smarty->assign('config',$_CFG);
	$smarty->display('set/admin_set_search.htm');
}
elseif($act == 'search_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"set_search");
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('config')." SET value='{$v}' WHERE name='{$k}'")?adminmsg('更新设置失败', 1):"";
	}
	//填写管理员日志
	write_log("后台成功更新搜索设置", $_SESSION['admin_name'],3);
	refresh_cache('config');
	write_log("配置搜索设置", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
?>