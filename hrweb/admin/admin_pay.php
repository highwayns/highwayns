<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_pay_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"site_payment");
$smarty->assign('pageheader',"支払方式");
if($act == 'list')
{	
	get_token();
	$smarty->assign('payment',get_payment());
	$smarty->display('pay/admin_payment_list.htm');
}
elseif($act == 'uninstall_payment')
{
	check_token();
	uninstall_payment($_GET['id'])?adminmsg('アンインストール成功', 2):adminmsg('アンインストール失敗', 1);
}
elseif($act == 'action_payment')
{
	get_token();
	$payment=get_payment_one($_GET['name']);
	if (!$payment) adminmsg('取得失敗', 1);
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
	$link[0]['text'] = "支払方式一覧に戻る";
	$link[0]['href'] = '?';
	!$db->updatetable(table('payment'), $setsqlarr,$wheresql)?adminmsg('保存失敗！', 1):adminmsg('保存成功！', 2,$link);
}
?>
