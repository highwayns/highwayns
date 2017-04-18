<?php
if(!defined('IN_HIGHWAY'))
{
die('Access Denied!');
}
include_once(HIGHWAY_ROOT_PATH.'include/template_lite/class.template.php');
$smarty = new Template_Lite; 
$smarty -> cache_dir = HIGHWAY_ROOT_PATH.'temp/caches/'.$_CFG['template_dir'];
$smarty -> compile_dir =  HIGHWAY_ROOT_PATH.'temp/templates_c/'.$_CFG['template_dir'];
$smarty -> template_dir = HIGHWAY_ROOT_PATH.'templates/'.$_CFG['template_dir'];
$smarty -> reserved_template_varname = "smarty";
$smarty -> left_delimiter = "{#";
$smarty -> right_delimiter = "#}";
$smarty -> force_compile = false;
$smarty -> assign('_PLUG', $_PLUG);
$smarty -> assign('HIGHWAY', $_CFG);
$smarty -> assign('page_select',$page_select);
?>
