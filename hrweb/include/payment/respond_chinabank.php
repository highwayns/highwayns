<?php 
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$funtype=array('1'=>'include/fun_company.php',4=>'include/fun_train.php',3=>'include/fun_hunter.php');
require_once(HIGHWAY_ROOT_PATH.$funtype[$_SESSION['utype']]);
require_once(HIGHWAY_ROOT_PATH."include/payment/chinabank.php");
	if (respond())
	{
		$orderurl=array('1'=>'company_service.php?act=order_list',4=>'train_service.php?act=order_list',3=>'hunter_service.php?act=order_list');
		$link[0]['text'] = "オーダー閲覧";
		$link[0]['href'] = get_member_url($_SESSION['utype'],true).$orderurl[$_SESSION['utype']];
		$link[1]['text'] = "会員中心";
		$link[1]['href'] = url_rewrite('HW_login');		
		$link[2]['text'] = "ウェブ首页";
		$link[2]['href'] = $_CFG['site_dir'];
		showmsg("支払い成功！",2,$link,false);
	}
	else
	{
		$link[0]['text'] = "会員中心";
		$link[0]['href'] = get_member_url($_SESSION['utype']);
		showmsg("支払い失敗！ウェブ管理者に連絡してください",0,$link);
	}
?>
