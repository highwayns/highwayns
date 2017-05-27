<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/company_common.php');
$smarty->assign('leftmenu',"promotion");

if ($act=='tpl')
{
		if (!$cominfo_flge)
		{
		$link[0]['text'] = "企業資料を入力してください";
		$link[0]['href'] = 'company_info.php?act=company_profile';
		showmsg("企業資料を入力してください！",1,$link);
		}
	$smarty->assign('title','テンプレート選択 - 企業会員センター - '.$_CFG['site_name']);
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
	showmsg("設定成功！",2);
	}
	$comtpl=get_comtpl_one($seltpl);
	if (empty($comtpl))
	{
		showmsg("テンプレート選択エラー",0);
	}
	if($_CFG['operation_mode']=='1'){
		$user_points=get_user_points($_SESSION['uid']);
		if ($comtpl['tpl_val']>$user_points)
		{
			$link[0]['text'] = "前頁に戻る";
			$link[0]['href'] = 'javascript:history.go(-1)';
			$link[1]['text'] = "振込ポイント";
			$link[1]['href'] = 'company_service.php?act=order_add';
			showmsg("貴方の".$_CFG['points_byname']."ポイント足りない，振込してください！",1,$link);
		}
	}elseif($_CFG['operation_mode']=='2'||$_CFG['operation_mode']=='3'){
		$setmeal=get_user_setmeal($_SESSION['uid']);//获取会员套餐
		$link[0]['text'] = "前頁に戻る";
		$link[0]['href'] = 'javascript:history.go(-1)';
		$link[1]['text'] = "サービス再Active";
		$link[1]['href'] = 'company_service.php?act=setmeal_list';
		if ($setmeal['endtime']<time() && $setmeal['endtime']<>"0")
		{					
			showmsg("サービス期限切れた，再申し込みしてください",1,$link);
		}
		if ($setmeal['change_templates']=='0')
		{
			showmsg("このコース{$setmeal['setmeal_name']},テンプレート切り替え権限がありません，新コースを申し込みしてください",1,$link);
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
		write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"設定企業テンプレート：{$comtpl['tpl_name']}，(-{$comtpl['tpl_val']})，(残る:{$user_points})",1,1022,"テンプレート選択","-{$comtpl['tpl_val']}","{$user_points}");
		}
	}elseif($_CFG['operation_mode']=='2'||$_CFG['operation_mode']=='3'){
		write_memberslog($_SESSION['uid'],1,9002,$_SESSION['username'],"コース：{$setmeal['setmeal_name']}，テンプレート選択自由切り替え，企業テンプレート設定：{$comtpl['tpl_name']}",2,1022,"テンプレート選択","0","0");
	}
	write_memberslog($_SESSION['uid'],1,8007,$_SESSION['username'],"企業テンプレート：{$comtpl['tpl_name']}");
	showmsg("設定成功！",2);
}
unset($smarty);
?>
