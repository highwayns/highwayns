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
$smarty->assign('leftmenu',"promotion");

if ($act=='tpl')
{
		if (!$cominfo_flge)
		{
		$link[0]['text'] = "填写企业资料";
		$link[0]['href'] = 'company_info.php?act=company_profile';
		showmsg("请先填写您的企业资料！",1,$link);
		}
	$smarty->assign('title','选择模版 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('comtpl',get_comtpl());
	if ($company_profile['tpl']=="")
	{
	$company_profile['tpl']=$_CFG['tpl_company'];
	}
	if($_CFG['operation_mode']=='2' || $_CFG['operation_mode']=='3'){
		$setmeal=get_user_setmeal($_SESSION['uid']);//获取会员套餐
		$smarty->assign('setmeal',$setmeal);
	}
	$smarty->assign('mytpl',$company_profile['tpl']);
	$smarty->assign('com_info',$company_profile['id']);
	$smarty->display('member_company/company_tpl.htm'); 
}
elseif ($act=='tpl_save')
{
	$seltpl=trim($_POST['tpl']);
	if ($company_profile['tpl']=="")
	{
	$company_profile['tpl']=$_CFG['tpl_company'];
	}
	if ($company_profile['tpl']==$seltpl)
	{
	showmsg("设置成功！",2);
	}
	$comtpl=get_comtpl_one($seltpl);
	if (empty($comtpl))
	{
		showmsg("模版选择错误",0);
	}
	if($_CFG['operation_mode']=='1'){
		$user_points=get_user_points($_SESSION['uid']);
		if ($comtpl['tpl_val']>$user_points)
		{
			$link[0]['text'] = "返回上一页";
			$link[0]['href'] = 'javascript:history.go(-1)';
			$link[1]['text'] = "充值积分";
			$link[1]['href'] = 'company_service.php?act=order_add';
			showmsg("你的".$_CFG['points_byname']."不够进行此次操作，请先充值！",1,$link);
		}
	}elseif($_CFG['operation_mode']=='2'||$_CFG['operation_mode']=='3'){
		$setmeal=get_user_setmeal($_SESSION['uid']);//获取会员套餐
		$link[0]['text'] = "返回上一页";
		$link[0]['href'] = 'javascript:history.go(-1)';
		$link[1]['text'] = "重新开通服务";
		$link[1]['href'] = 'company_service.php?act=setmeal_list';
		if ($setmeal['endtime']<time() && $setmeal['endtime']<>"0")
		{					
			showmsg("您的服务已经到期，请重新开通",1,$link);
		}
		if ($setmeal['change_templates']=='0')
		{
			showmsg("你的套餐{$setmeal['setmeal_name']},没有自由切换模板的权限，请尽快开通新套餐",1,$link);
		}
	}
	$setsqlarr['tpl']=$seltpl;
	$db->updatetable(table('company_profile'),$setsqlarr," uid='{$_SESSION['uid']}'");
	$db->updatetable(table('jobs'),$setsqlarr," uid='{$_SESSION['uid']}'");
	$db->updatetable(table('jobs_tmp'),$setsqlarr," uid='{$_SESSION['uid']}'");
	
 	if($_CFG['operation_mode']=='1'){
		if ($comtpl['tpl_val']>0)
		{
		report_deal($_SESSION['uid'],2,$comtpl['tpl_val']);
		$user_points=get_user_points($_SESSION['uid']);
		write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"设置企业模版：{$comtpl['tpl_name']}，(-{$comtpl['tpl_val']})，(剩余:{$user_points})",1,1022,"选择模板","-{$comtpl['tpl_val']}","{$user_points}");
		}
	}elseif($_CFG['operation_mode']=='2'||$_CFG['operation_mode']=='3'){
		write_memberslog($_SESSION['uid'],1,9002,$_SESSION['username'],"套餐：{$setmeal['setmeal_name']}，可自由切换模板，设置企业模版：{$comtpl['tpl_name']}",2,1022,"选择模板","0","0");
	}
	write_memberslog($_SESSION['uid'],1,8007,$_SESSION['username'],"设置企业模版：{$comtpl['tpl_name']}");
	showmsg("设置成功！",2);
}
unset($smarty);
?>