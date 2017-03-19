<?php 
 /*
 * 74cms 支付响应页面
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../common.inc.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$funtype=array('1'=>'include/fun_company.php',4=>'include/fun_train.php',3=>'include/fun_hunter.php');
require_once(QISHI_ROOT_PATH.$funtype[$_SESSION['utype']]);
require_once(QISHI_ROOT_PATH."include/payment/alipay.php");
	if (respond())
	{		
		$orderurl=array('1'=>'company_service.php?act=order_list',4=>'train_service.php?act=order_list',3=>'hunter_service.php?act=order_list');
		$link[0]['text'] = "查看订单";
		if($_SESSION['adv_pay']){
			$link[0]['href'] = get_member_url($_SESSION['utype'],true)."company_service.php?act=adv_order_list";
		}else{
			$link[0]['href'] = get_member_url($_SESSION['utype'],true).$orderurl[$_SESSION['utype']];
		}
		unset($_SESSION['adv_pay']);
		$link[1]['text'] = "会员中心";
		$link[1]['href'] = url_rewrite('QS_login');
		$link[2]['text'] = "网站首页";
		$link[2]['href'] = $_CFG['site_dir'];
		showmsg("付款成功！",2,$link,false);
	}
	else
	{
		$link[0]['text'] = "会员中心";
		$link[0]['href'] = get_member_url($_SESSION['utype']);
		showmsg("付款失败！请联系网站管理员",0,$link);
	}
?>
