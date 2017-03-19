<?php
 /*
 * 74cms 生成HTML
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
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'set';
$smarty->assign('act',$act);
$smarty->assign('pageheader',"百度链接提交");
if($act == 'set')
{
$smarty->assign('data',get_cache('baidu_submiturl'));
$smarty->display('baidu_submiturl/admin_baidu_submiturl_set.htm');
}
elseif($act == 'setsave')
{
	$_POST['token']=trim($_POST['token']);
	$_POST['addcompany']=intval($_POST['addcompany']);
	$_POST['addjob']=intval($_POST['addjob']);
	$_POST['addresume']=intval($_POST['addresume']);
	$_POST['addcompanynews']=intval($_POST['addcompanynews']);
	$_POST['addarticle']=intval($_POST['addarticle']);
	$_POST['addexplain']=intval($_POST['addexplain']);
	$_POST['addnotice']=intval($_POST['addnotice']);
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('baidu_submiturl')." SET value='{$v}' WHERE name='{$k}'")?adminmsg('保存失败', 1):"";
	}
	refresh_cache('baidu_submiturl');
	write_log("修改百度链接提交配置", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
?>