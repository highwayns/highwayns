<?php
 /*
 * 74cms 模块管理
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
require_once(ADMIN_ROOT_PATH.'include/admin_plug_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"site_plug");
$smarty->assign('pageheader',"模块管理");
if($act == 'list')
{	
	get_token();
	$smarty->assign('plug',get_plug());
	$smarty->display('plug/admin_plug_list.htm');
}
elseif($act == 'uninstall_plug')
{
	check_token();
	if(uninstall_plug($_GET['id'])){
		refresh_plug_cache();
		adminmsg('关闭成功', 2);
	}else{
		adminmsg('关闭失败', 1);
	}

			
}
elseif($act == 'install_plug')
{
	check_token();
	if(install_plug($_GET['id'])){
		refresh_plug_cache();
		adminmsg('开启成功', 2);
	}else{
		adminmsg('开启失败', 1);
	}
}
?>