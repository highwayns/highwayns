<?php
if(!defined('IN_HIGHWAY'))
{
die('Access Denied!');
}
error_reporting(E_ERROR );
define('HIGHWAY_ROOT_PATH', str_replace('install/include/common.inc.php', '', str_replace('\\', '/', __FILE__)));
session_cache_limiter('private, must-revalidate');
ini_set('session.save_handler', 'files');
session_save_path(HIGHWAY_ROOT_PATH.'data/sessions/');
session_start();
require_once (HIGHWAY_ROOT_PATH.'install/include/common.fun.php');
if (!empty($_GET))
{
$_GET  = install_addslashes_deep($_GET);
}
if (!empty($_POST))
{
$_POST = install_addslashes_deep($_POST);
}
$_COOKIE   = install_addslashes_deep($_COOKIE);
$_REQUEST  = install_addslashes_deep($_REQUEST);
PHP_VERSION > '5.1'?date_default_timezone_set("PRC"):'';
 $timestamp = time();
 header("Content-Type:text/html;charset=".HIGHWAY_CHARSET);
 $php_self = isset($_SERVER['PHP_SELF']) ? $_SERVER['PHP_SELF'] : $_SERVER['SCRIPT_NAME'];
 $url = $php_self."?".$_SERVER['QUERY_STRING'];
if (!file_exists(HIGHWAY_ROOT_PATH.'install/'))
{
echo "“install”フォルダーが存在しません！";
exit();
}
if (!is_readable(HIGHWAY_ROOT_PATH.'install/') || !is_writable(HIGHWAY_ROOT_PATH.'install/') || !is_readable(HIGHWAY_ROOT_PATH.'install/compile/') || !is_writable(HIGHWAY_ROOT_PATH.'install/compile/'))
{
exit("“install”フォルダー及びサブフォルダー下読み書き状態を（777）に設定して<br />インストール説明をご覧ください！");
} 
 require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
 require_once(HIGHWAY_ROOT_PATH.'include/template_lite/class.template.php');
 $install_smarty = new Template_Lite();
 $install_smarty -> reserved_template_varname = "smarty";
 $install_smarty->cache = false;
 $install_smarty->template_dir = HIGHWAY_ROOT_PATH.'install/templates';
 $install_smarty->compile_dir = HIGHWAY_ROOT_PATH.'install/compile';
 $install_smarty->left_delimiter = "{#";
 $install_smarty->right_delimiter = "#}";
 $need_check_dirs = array(
                    'data',
                    'data/comads',
					'data/backup',
					'data/certificate',
					'data/images',
					'data/images/thumb',
					'data/link',
					'data/logo',					
                    'data/photo',
					'data/photo/thumb',
 					'data/hrtools',
					'data/sessions',
					'data/credent_photo',
					'data/word',
					'temp/caches',
					'temp/templates_c',		
					'temp/backup_templates',			
					'templates',	
					'admin/statement',			
					'install'
                    );
?>
