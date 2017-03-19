<?php
 /*
 * 74cms 管理中心共用配置文件
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
if(!defined('IN_QISHI')) die('Access Denied!');
header("Content-Type:text/html;charset=".QISHI_CHARSET);
error_reporting(E_ERROR);
define('ADMIN_ROOT_PATH', str_replace('include/admin_common.inc.php', '', str_replace('\\', '/', __FILE__)));
define('QISHI_ROOT_PATH', dirname(ADMIN_ROOT_PATH).'/');
ini_set('session.save_handler', 'files');
session_save_path(QISHI_ROOT_PATH.'data/sessions/');
session_start();
require_once(QISHI_ROOT_PATH.'include/74cms_version.php');
require_once(ADMIN_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
unset($dbhost,$dbuser,$dbpass);
require_once(QISHI_ROOT_PATH.'include/help.class.php');
require_once(QISHI_ROOT_PATH.'include/common.fun.php');
require_once(ADMIN_ROOT_PATH.'include/admin_common.fun.php');
if(!get_magic_quotes_gpc())
{
	$_POST = admin_addslashes_deep($_POST);
	$_GET = admin_addslashes_deep($_GET);
	$_COOKIE = admin_addslashes_deep($_COOKIE);
	$_REQUEST = admin_addslashes_deep($_REQUEST);
}
$timestamp = time();
$online_ip = getip();
$ip_address=convertip($online_ip);

$_PAGE=get_cache('page');
$_NAV =get_cache('nav');
$_CFG=get_cache('config');
$_PLUG=get_cache('plug');
$_CFG['version']=QISHI_VERSION;
$_CFG['site_template']=$_CFG['site_dir'].'templates/'.$_CFG['template_dir'];
$_CFG['web_logo']=$_CFG['web_logo']?$_CFG['web_logo']:'logo.gif';
$_CFG['upfiles_dir']=$_CFG['site_dir']."data/".$_CFG['updir_images']."/";
$_CFG['thumb_dir']=$_CFG['site_dir']."data/".$_CFG['updir_thumb']."/";
$_CFG['certificate_dir']=$_CFG['site_dir']."data/".$_CFG['updir_certificate']."/";
$_CFG['certificate_train_dir']=$_CFG['site_dir']."data/".$_CFG['updir_train_certificate']."/";
$_CFG['resume_photo_dir']=$_CFG['site_dir']."data/".$_CFG['resume_photo_dir']."/";
$_CFG['resume_photo_dir_thumb']=$_CFG['site_dir']."data/".$_CFG['resume_photo_dir_thumb']."/";
$_CFG['hunter_photo_dir']=$_CFG['site_dir']."data/hunter/";
$_CFG['hunter_photo_dir_thumb']=$_CFG['site_dir']."data/hunter/thumb/";
$upfiles_dir="../data/".$_CFG['updir_images']."/";
$thumb_dir="../data/".$_CFG['updir_thumb']."/";
$certificate_dir="../data/".$_CFG['updir_certificate']."/";
$certificate_train_dir="../data/".$_CFG['updir_train_certificate']."/";
$hunter_dir="../data/hunter/";
$thumbwidth="115";
$thumbheight="85";
if (empty($_GET['perpage']))
{
$_GET['perpage']=10;
}
$perpage=intval($_GET['perpage']);
require_once(ADMIN_ROOT_PATH.'include/admin_tpl.inc.php');
date_default_timezone_set("PRC");
if(empty($_SESSION['admin_id']) && $_REQUEST['act'] != 'login' && $_REQUEST['act'] != 'do_login' && $_REQUEST['act'] != 'logout')
{
	if($_COOKIE['Qishi']['admin_id'] && $_COOKIE['Qishi']['admin_name'] && $_COOKIE['Qishi']['admin_pwd'])
	{
	
			if(check_cookie($_COOKIE['Qishi']['admin_name'],$_COOKIE['Qishi']['admin_pwd']))
			{
				update_admin_info($_COOKIE['Qishi']['admin_name'],false);
			}
			else
			{
				setcookie("Qishi[admin_id]", '', 1, $QS_cookiepath, $QS_cookiedomain);
				setcookie("Qishi[admin_name]", '', 1, $QS_cookiepath, $QS_cookiedomain);
				setcookie("Qishi[admin_pwd]", '', 1, $QS_cookiepath, $QS_cookiedomain);
				exit('<script type="text/javascript">top.location="admin_login.php?act=login";</script>');
			}
	}
	else
	{
	exit('<script type="text/javascript">top.location="admin_login.php?act=login";</script>');
	}
}
?>