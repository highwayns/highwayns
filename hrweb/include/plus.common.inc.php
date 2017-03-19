<?php
 /*
 * 74cms plus共用配置文件
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
if(!defined('IN_QISHI')) exit('Access Denied!');
define('QISHI_ROOT_PATH', dirname(dirname(__FILE__)).'/');
error_reporting(E_ERROR);
require_once(QISHI_ROOT_PATH.'data/config.php');
ini_set('session.save_handler', 'files');
session_save_path(QISHI_ROOT_PATH.'data/sessions/');
session_start();
header("Content-Type:text/html;charset=".QISHI_CHARSET);
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
require_once(QISHI_ROOT_PATH.'include/common.fun.php');
PHP_VERSION > '5.1'?date_default_timezone_set("PRC"):'';
$timestamp = time();
$online_ip=getip();
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
$ip_address=convertip($online_ip);
$_CFG=get_cache('config');
$_PAGE=get_cache('page');
$_NAV=get_cache('nav');
$_CFG['wap_domain'] = $_CFG['wap_domain']==""?$_CFG['site_domain'].$_CFG['site_dir']."m":$_CFG['wap_domain'];
$_CFG['version']=QISHI_VERSION;
$_CFG['web_logo']=$_CFG['web_logo']?$_CFG['web_logo']:'logo.gif';
$_CFG['upfiles_dir']=$_CFG['site_dir']."data/".$_CFG['updir_images']."/";
$_CFG['site_template']=$_CFG['site_dir'].'templates/'.$_CFG['template_dir'];
execution_crons();
?>