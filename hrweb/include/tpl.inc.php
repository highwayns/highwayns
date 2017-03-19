<?php
 /*
 * 74cms 初始化模版引擎
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
if(!defined('IN_QISHI'))
{
die('Access Denied!');
}
include_once(QISHI_ROOT_PATH.'include/template_lite/class.template.php');
$smarty = new Template_Lite; 
$smarty -> cache_dir = QISHI_ROOT_PATH.'temp/caches/'.$_CFG['template_dir'];
$smarty -> compile_dir =  QISHI_ROOT_PATH.'temp/templates_c/'.$_CFG['template_dir'];
$smarty -> template_dir = QISHI_ROOT_PATH.'templates/'.$_CFG['template_dir'];
$smarty -> reserved_template_varname = "smarty";
$smarty -> left_delimiter = "{#";
$smarty -> right_delimiter = "#}";
$smarty -> force_compile = false;
$smarty -> assign('_PLUG', $_PLUG);
$smarty -> assign('QISHI', $_CFG);
$smarty -> assign('page_select',$page_select);
?>