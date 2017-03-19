<?php
 /*
 * 74cms 支付方式
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
require_once(ADMIN_ROOT_PATH.'include/admin_pay_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"site_payment");
$smarty->assign('pageheader',"支付方式");
if($act == 'list')
{	
	get_token();
	$smarty->assign('payment',get_payment());
	$smarty->display('pay/admin_payment_list.htm');
}
elseif($act == 'uninstall_payment')
{
	check_token();
	uninstall_payment($_GET['id'])?adminmsg('成功卸载', 2):adminmsg('卸载失败', 1);
}
elseif($act == 'action_payment')
{
	get_token();
	$payment=get_payment_one($_GET['name']);
	if (!$payment) adminmsg('获取失败', 1);
	require_once("../include/payment/".$payment['typename'].".php");
	$smarty->assign('show',$payment);
	$smarty->assign('pay',pay_info());
	$smarty->display('pay/admin_payment_action.htm');
}
elseif($act == 'save_payment')
{
	check_token();
	$setsqlarr['id']=intval($_POST['id']);
	$setsqlarr['listorder']=intval($_POST['listorder']);
	$setsqlarr['p_introduction']=trim($_POST['p_introduction']);
	$setsqlarr['notes']=trim($_POST['notes']);
	$setsqlarr['partnerid']=trim($_POST['partnerid']);
	$setsqlarr['ytauthkey']=trim($_POST['ytauthkey']);
	$setsqlarr['fee']=floatval($_POST['fee']);
	$setsqlarr['parameter1']=trim($_POST['parameter1']);
	$setsqlarr['parameter2']=trim($_POST['parameter2']);
	$setsqlarr['parameter3']=trim($_POST['parameter3']);
	$setsqlarr['p_install']=2;
	$wheresql=" id=".$setsqlarr['id']." ";
	$link[0]['text'] = "返回支付方式列表";
	$link[0]['href'] = '?';
	!$db->updatetable(table('payment'), $setsqlarr,$wheresql)?adminmsg('保存失败！', 1):adminmsg('保存成功！', 2,$link);
}
?>