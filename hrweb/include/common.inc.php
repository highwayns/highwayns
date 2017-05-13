<?php
if(!defined('IN_HIGHWAY')) exit('Access Denied!');
define('HIGHWAY_ROOT_PATH',dirname(dirname(__FILE__)).'/');
error_reporting(E_ERROR);
ini_set('session.save_handler', 'files');
session_save_path(HIGHWAY_ROOT_PATH.'data/sessions/');
session_start();
require_once(HIGHWAY_ROOT_PATH.'data/config.php');
header("Content-Type:text/html;charset=".HIGHWAY_CHARSET);
require_once(HIGHWAY_ROOT_PATH.'include/help.class.php');
require_once(HIGHWAY_ROOT_PATH.'include/common.fun.php');
require_once(HIGHWAY_ROOT_PATH.'include/highwayns_version.php');
$QSstarttime=exectime();
if (!empty($_GET))
{
$_GET  = help::addslashes_deep($_GET);
}
if (!empty($_POST))
{
$_POST = help::addslashes_deep($_POST);
}
$_COOKIE   = help::addslashes_deep($_COOKIE);
$_REQUEST  = help::addslashes_deep($_REQUEST);
date_default_timezone_set("PRC");
$timestamp = time();
$online_ip=getip();
$ip_address=convertip($online_ip);
$_NAV=get_cache('nav');
$_PAGE=get_cache('page');
$_CFG=get_cache('config');
$_PLUG=get_cache('plug');
$_CFG['wap_domain'] = $_CFG['wap_domain']==""?$_CFG['site_domain'].$_CFG['site_dir']."m":$_CFG['wap_domain'];
$_CFG['version']=HIGHWAY_VERSION;
$_CFG['web_logo']=$_CFG['web_logo']?$_CFG['web_logo']:'logo.gif';
$_CFG['upfiles_dir']=$_CFG['site_dir']."data/".$_CFG['updir_images']."/";
$_CFG['thumb_dir']=$_CFG['site_dir']."data/".$_CFG['updir_thumb']."/";
$_CFG['_resume_photo_dir']= $_CFG['resume_photo_dir'];
$_CFG['resume_photo_dir']=$_CFG['site_dir']."data/".$_CFG['resume_photo_dir']."/";
$_CFG['_resume_photo_dir_thumb']= $_CFG['resume_photo_dir_thumb'];
$_CFG['resume_photo_dir_thumb']=$_CFG['site_dir']."data/".$_CFG['resume_photo_dir_thumb']."/";
$_CFG['site_template']=$_CFG['site_dir'].'templates/'.$_CFG['template_dir'];
$mypage=$_PAGE[$alias];
$mypage['tag']?$page_select=$mypage['tag']:'';
require_once(HIGHWAY_ROOT_PATH.'include/tpl.inc.php');
	if ($_CFG['isclose'])
	{
				$smarty->assign('info',$_CFG['close_reason']=$_CFG['close_reason']?$_CFG['close_reason']:'サイト一時閉じる...');
				$smarty->display('warning.htm');
				exit();
	}
	if ($_CFG['filter_ip'] && check_word($_CFG['filter_ip'],$online_ip))
	{
			$smarty->assign('info',$_CFG['filter_ip_tips']);
			$smarty->display('warning.htm');
			exit();
	}
?>
