<?php
/*
 * 74cms 企业会员中心
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/company_common.php');
$smarty->assign('leftmenu',"index");
if ($act=='index')
{
	$uid=intval($_SESSION['uid']);
	$smarty->assign('title','企业会员中心 - '.$_CFG['site_name']);
	//首页顶部提示信息(套餐或者积分已失效或快失效时提醒)
	$message = array();
	if ($_CFG['operation_mode']=='1' || $_CFG['operation_mode']=='3')
	{
		$my_points = get_user_points($uid);
		if($my_points < $_CFG['points_min_remind'] && intval($my_points) > 0 && !empty($_CFG['points_min_remind']))
		{
			$message[] = '提醒：您的积分不足，为避免造成不必要的麻烦，请<a href="company_service.php?act=order_add">立即充值</a>';
		}
		elseif(intval($my_points) <= 0 && !empty($_CFG['points_min_remind']))
		{
			$message[] = '提醒：您的积分已为0，为避免造成不必要的麻烦，请<a href="company_service.php?act=order_add">立即充值</a>';
		}
		$smarty->assign('points',$my_points);
	}
	if($_CFG['operation_mode']=='2' || $_CFG['operation_mode']=='3')
	{
		$my_setmeal = get_user_setmeal($uid);
		if(time()>$my_setmeal['endtime'] && $my_setmeal['endtime'] > 0 && !empty($_CFG['meal_min_remind'])){
			$message[] = '提醒：您的套餐已到期，为避免造成不必要的麻烦，请<a href="company_service.php?act=setmeal_list" target="_blank">升级套餐</a>';
		}
		elseif(($my_setmeal['endtime']-time())/86400 <=$_CFG['meal_min_remind']  && $my_setmeal['endtime'] > 0 && !empty($_CFG['meal_min_remind']))
		{
			$message[] = '提醒：您的套餐快到期，为避免造成不必要的麻烦，请<a href="company_service.php?act=setmeal_list" target="_blank">升级套餐</a>';
		}
		$smarty->assign('setmeal',$my_setmeal);
	}
	$smarty->assign('message',$message);
	$smarty->assign('company',$company_profile);
	//登录时间
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	//判断是否是自己登录的还是后台管理员登录的
	if($_SESSION['no_self'])
	{
		$smarty->assign('loginlog',get_loginlog_two($uid,'1001'));
	}
	else
	{
		$smarty->assign('loginlog',get_loginlog_one($uid,'1001'));
	}
	$smarty->assign('user',$user);
	//统计职位数
	$smarty->assign('total_audit_jobs',$db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs')." WHERE uid=".$uid));
	$smarty->assign('total_noaudit_jobs',$db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs_tmp')." WHERE uid=".$uid." AND audit=2"));
	$smarty->assign('total_nolook_resume',$db->get_total("SELECT COUNT(*) AS num FROM ".table('personal_jobs_apply')." WHERE company_uid=".$uid." AND personal_look=1"));
	$smarty->assign('total_down_resume',$db->get_total("SELECT COUNT(*) AS num FROM ".table('company_down_resume')." WHERE company_uid=".$uid));
	$smarty->assign('total_favorites_resume',$db->get_total("SELECT COUNT(*) AS num FROM ".table('company_favorites')." WHERE company_uid=".$uid));
	//推荐简历
	$smarty->assign('concern_id',get_concern_id($uid));
	//消息提醒
	$smarty->assign('msg_total1',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='2' AND `replyuid`<>'{$uid}'"));
	$smarty->assign('msg_total2',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='1' AND `replyuid`<>'{$uid}'"));
	$smarty->display('member_company/company_index.htm');
}
unset($smarty);
?>