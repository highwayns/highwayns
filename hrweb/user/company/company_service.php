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
$smarty->assign('leftmenu',"service");
//我的账户 -> 积分操作 
if ($act=='j_account')
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$smarty->assign('operation_mode',intval($_CFG['operation_mode']));
	//收支状态(消耗->1 赠送->2)/操作时间
	$cid=trim($_GET['cid']);
	$settr=intval($_GET['settr']);
	//套餐
	$my_setmeal = get_user_setmeal($_SESSION['uid']);
	$smarty->assign('setmeal',$my_setmeal);
	//积分
	$my_points = get_user_points(intval($_SESSION['uid']));
	$smarty->assign('points',$my_points);
	$smarty->assign('act','j_account');
	$smarty->assign('title','我的账户 - 企业会员中心 - '.$_CFG['site_name']);
	//积分消费明细
	if(trim($_GET['detail']) == '1')
	{
		$wheresql=" WHERE log_uid='{$_SESSION['uid']}' AND log_type=9001 AND log_mode=1";
		if($settr>0)
		{
			$settr_val=strtotime("-".$settr." day");
			$wheresql.=" AND log_addtime>".$settr_val;
			$smarty->assign('settr',$_GET['settr']);
		}
		if($cid == '1')
		{
			$smarty->assign('c_type',"消耗");
			$smarty->assign('cid',$_GET['cid']);
			$wheresql.=" AND log_op_used < 0 ";
		}
		elseif($cid == '2')
		{
			$smarty->assign('c_type',"赠送");
			$smarty->assign('cid',$_GET['cid']);
			$wheresql.=" AND log_op_used > 0 ";
		}
		$perpage=10;
		$total_sql="SELECT COUNT(*) AS num FROM ".table('members_log').$wheresql;
		$total_val=$db->get_total($total_sql);
		$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
		$offset=($page->nowindex-1)*$perpage;
		$smarty->assign('report',get_user_report($offset, $perpage,$wheresql));
		$smarty->assign('page',$page->show(3));
		$smarty->display('member_company/company_my_account_detail.htm');
	}
	//积分规则
	else
	{
		$smarty->assign('points_rule',get_points_rule());
		$smarty->display('member_company/company_my_account.htm');
	}
}
//我的账户 -> 套餐操作 
elseif ($act=='t_account')
{
	$settr=intval($_GET['settr']);
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$smarty->assign('operation_mode',intval($_CFG['operation_mode']));
	//积分
	$my_points = get_user_points(intval($_SESSION['uid']));
	$smarty->assign('points',$my_points);
	//套餐
	$my_setmeal = get_user_setmeal($_SESSION['uid']);
	$smarty->assign('setmeal',$my_setmeal);
	$smarty->assign('act','t_account');
	$smarty->assign('title','我的账户 - 企业会员中心 - '.$_CFG['site_name']);
	//套餐消费明细
	if(trim($_GET['detail']) == '1')
	{
		$wheresql=" WHERE log_uid='{$_SESSION['uid']}' AND log_type=9002 AND log_mode=2 ";
		if($settr>0)
		{
			$settr_val=strtotime("-".$settr." day");
			$wheresql.=" AND log_addtime>".$settr_val;
			$smarty->assign('settr',$_GET['settr']);
		}
		$perpage=10;
		$total_sql="SELECT COUNT(*) AS num FROM ".table('members_log').$wheresql;
		$total_val=$db->get_total($total_sql);
		$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
		$offset=($page->nowindex-1)*$perpage;
		$smarty->assign('report',get_user_report($offset, $perpage,$wheresql));
		$smarty->assign('page',$page->show(3));
		$smarty->display('member_company/company_my_account_package_detail.htm');
	}
	//套餐规则
	else
	{
		$smarty->assign('setmeal_rule',get_members_setmeal_rule($my_setmeal['setmeal_id']));
		$smarty->display('member_company/company_my_account_package.htm');
	}

}
elseif ($act=='order_list')
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$is_paid=trim($_GET['is_paid']);
	$pay_type=intval($_GET['pay_type']);
	$wheresql=" WHERE uid='".$_SESSION['uid']."' ";
	//订单状态
	if($is_paid<>'' && is_numeric($is_paid))
	{
		$smarty->assign('is_paid',$is_paid);
		$wheresql.=" AND is_paid='".intval($is_paid)."' ";
	}
	//订单类别
	if($pay_type > 0)
	{
		$smarty->assign('pay_type',$pay_type);
		$wheresql.=" AND pay_type='".intval($pay_type)."' ";
	}
	$perpage=10;
	$total_sql="SELECT COUNT(*) AS num FROM ".table('order').$wheresql;
	$page = new page(array('total'=>$db->get_total($total_sql), 'perpage'=>$perpage));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$smarty->assign('title','充值记录 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('act',$act);
	$smarty->assign('is_paid',$is_paid);
	$smarty->assign('payment',get_order_all($offset, $perpage,$wheresql));
	if ($total_val>$perpage)
	{
	$smarty->assign('page',$page->show(3));
	}
	$smarty->display('member_company/company_order_list.htm');
}
elseif ($act=='order_add')
{
	$smarty->assign('title','在线充值 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('payment',get_payment());
	$smarty->assign('points',get_user_points($_SESSION['uid']));
	$smarty->display('member_company/company_order_add.htm');
}
elseif ($act=='order_add_save')
{
		if (!$cominfo_flge)
		{
		$link[0]['text'] = "填写企业资料";
		$link[0]['href'] = 'company_info.php?act=company_profile';
		showmsg("请先填写您的企业资料！",1,$link);
		}
	$myorder=get_user_order($_SESSION['uid'],1);
	$order_num=count($myorder);
	if ($order_num>=5)
	{
	$link[0]['text'] = "立即查看";
	$link[0]['href'] = '?act=order_list&is_paid=1';
	showmsg("未处理的订单不能超过 5 条，请先处理后再次申请！",1,$link,true,8);
	}
	$amount=(trim($_POST['amount'])).(intval($_POST['amount']))?trim($_POST['amount']):showmsg('请填写充值金额！',1);
	($amount<$_CFG['payment_min'])?showmsg("单笔充值金额不能少于 ".$_CFG['payment_min']." 元！",1):'';
	$payment_name=empty($_POST['payment_name'])?showmsg("请选择付款方式！",1):$_POST['payment_name'];
	$paymenttpye=get_payment_info($payment_name);
	if (empty($paymenttpye)) showmsg("支付方式错误！",0);
	$fee=number_format(($amount/100)*$paymenttpye['fee'],1,'.','');//手续费
	$order['oid']= strtoupper(substr($paymenttpye['typename'],0,1))."-".date('ymd',time())."-".date('His',time());//订单号
	$order['v_url']=$_CFG['site_domain'].$_CFG['site_dir']."include/payment/respond_".$paymenttpye['typename'].".php";
	$order['v_amount']=$amount+$fee; 
	$points=$amount*$_CFG['payment_rate'];
	$order_id=add_order($_SESSION['uid'],4,$order['oid'],$amount,$payment_name,"充值积分:".$points,$timestamp,$points,'',1);
		if ($order_id)
			{
			header("location:?act=payment&order_id=".$order_id);
			}
			else
			{
			showmsg("添加订单失败！",0);
			}
}
elseif ($act=='payment')
{
	$setmeal = get_user_setmeal($_SESSION['uid']);
	if ($setmeal['endtime']>0){
		$setmeal_endtime=sub_day($setmeal['endtime'],time());
	}else{
		$setmeal_endtime="无限期";
	}
	$smarty->assign('user_setmeal',$setmeal);
	$smarty->assign('setmeal_endtime',$setmeal_endtime);
	$smarty->assign('payment',get_payment());
	$order_id=intval($_GET['order_id']);
	$myorder=get_order_one($_SESSION['uid'],$order_id);
	$payment=get_payment_info($myorder['payment_name']);
	if (empty($payment)) showmsg("支付方式错误！",0);
	$fee=number_format(($myorder['amount']/100)*$payment['fee'],1,'.','');//手续费
	$order['oid']=$myorder['oid'];//订单号
	$order['v_url']=$_CFG['site_domain'].$_CFG['site_dir']."include/payment/respond_".$payment['typename'].".php";
	$order['v_amount']=$myorder['amount']+$fee;
	if ($myorder['payment_name']!='remittance')//假如是非线下支付，
	{
		require_once(QISHI_ROOT_PATH."include/payment/".$payment['typename'].".php");
		$payment_form=get_code($order,$payment);
		if (empty($payment_form)) showmsg("在线支付参数错误！",0);
	}
	$smarty->assign('points',get_user_points($_SESSION['uid']));
	$smarty->assign('title','付款 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('fee',$fee);
	$smarty->assign('amount',$myorder['amount']);
	$smarty->assign('oid',$order['oid']);
	$smarty->assign('byname',$payment);
	$smarty->assign('payment_form',$payment_form);
	$smarty->display('member_company/company_order_pay.htm');
}
elseif ($act=='order_del')
{
	$link[0]['text'] = "返回上一页";
	$link[0]['href'] = '?act=order_list';
	$id=intval($_GET['id']);
	del_order($_SESSION['uid'],$id)?showmsg('取消成功！',2,$link):showmsg('取消失败！',1);
}
elseif ($act=='setmeal_list')
{
	$setmeal = get_user_setmeal($_SESSION['uid']);
	if ($setmeal['endtime']>0){
		$setmeal_endtime=sub_day($setmeal['endtime'],time());
	}else{
		$setmeal_endtime="无限期";
	}
	$smarty->assign('user_setmeal',$setmeal);
	$smarty->assign('setmeal_endtime',$setmeal_endtime);
	$smarty->assign('title','服务列表 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('setmeal',get_setmeal());
	$smarty->display('member_company/company_setmeal_list.htm');
}
elseif ($act=='setmeal_order_add')
{
	$setmealid = intval($_GET['setmealid'])?intval($_GET['setmealid']):showmsg("请选择服务套餐！",1);
	$setmeal = get_user_setmeal($_SESSION['uid']);
	if ($setmeal['endtime']>0){
		$setmeal_endtime=sub_day($setmeal['endtime'],time());
	}else{
		$setmeal_endtime="无限期";
	}
	$smarty->assign('user_setmeal',$setmeal);
	$smarty->assign('setmeal_endtime',$setmeal_endtime);
	$smarty->assign('title','申请服务 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('setmeal',get_setmeal_one($setmealid));
	$smarty->assign('payment',get_payment());
	$smarty->display('member_company/company_order_add_setmeal.htm');
}
elseif ($act=='setmeal_order_add_save')
{
		if (!$cominfo_flge)
		{
		$link[0]['text'] = "填写企业资料";
		$link[0]['href'] = 'company_info.php?act=company_profile';
		showmsg("请先填写您的企业资料！",1,$link);
		}
	$myorder=get_user_order($_SESSION['uid'],1);
	$order_num=count($myorder);
	if ($order_num>=5)
	{
	$link[0]['text'] = "立即查看";
	$link[0]['href'] = '?act=order_list&is_paid=1';
	showmsg("未处理的订单不能超过 5 条，请先处理后再次申请！",1,$link,true,8);
	}
	$setmeal=get_setmeal_one($_POST['setmealid']);
	if ($setmeal && $setmeal['apply']=="1")
	{
		$payment_name=empty($_POST['payment_name'])?showmsg("请选择付款方式！",1):$_POST['payment_name'];
		$paymenttpye=get_payment_info($payment_name);
		if (empty($paymenttpye)) showmsg("支付方式错误！",0);
		$fee=number_format(($setmeal['expense']/100)*$paymenttpye['fee'],1,'.','');//手续费
		$order['oid']= strtoupper(substr($paymenttpye['typename'],0,1))."-".date('ymd',time())."-".date('His',time());//订单号
		$order['v_url']=$_CFG['site_domain'].$_CFG['site_dir']."include/payment/respond_".$paymenttpye['typename'].".php";
		$order['v_amount']=$setmeal['expense']+$fee;//金额
		$order_id=add_order($_SESSION['uid'],1,$order['oid'],$setmeal['expense'],$payment_name,"开通服务:".$setmeal['setmeal_name'],$timestamp,"",$setmeal['id'],1);
			if ($order_id)
			{
				if ($order['v_amount']==0)//0元套餐
				{
					if (order_paid($order['oid']))
					{
						$link[0]['text'] = "查看订单";
						$link[0]['href'] = 'company_service.php?act=order_list';
						$link[1]['text'] = "会员中心首页";
						$link[1]['href'] = 'company_index.php?act=';
						showmsg("操作成功，系统已为您开通了服务！",2,$link);	
					}
				}
				header("Location:?act=payment&order_id=".$order_id."");//付款页面
			}
			else
			{
			showmsg("添加订单失败！",0);
			}
	}
	else
	{
	showmsg("添加订单失败！",0);
	}
}
elseif ($act=='feedback')
{
	$smarty->assign('title','用户反馈 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('feedback',get_feedback($_SESSION['uid']));
	$smarty->display('member_company/company_feedback.htm');
}
elseif ($act=='feedback_save')
{
	$get_feedback=get_feedback($_SESSION['uid']);
	if (count($get_feedback)>=5) 
	{
	showmsg('反馈信息不能超过5条！',1);
	exit();
	}
	$setsqlarr['infotype']=intval($_POST['infotype']);
	$setsqlarr['feedback']=trim($_POST['feedback'])?trim($_POST['feedback']):showmsg("请填写内容！",1);
	$setsqlarr['uid']=$_SESSION['uid'];
	$setsqlarr['usertype']=$_SESSION['utype'];
	$setsqlarr['username']=$_SESSION['username'];
	$setsqlarr['addtime']=$timestamp;
	write_memberslog($_SESSION['uid'],1,7001,$_SESSION['username'],"添加了反馈信息");
	!$db->inserttable(table('feedback'),$setsqlarr)?showmsg("添加失败！",0):showmsg("添加成功，请等待管理员回复！",2);
}
elseif ($act=='del_feedback')
{
	$id=intval($_GET['id']);
	del_feedback($id,$_SESSION['uid'])?showmsg('删除成功！',2):showmsg('删除失败！',1);
}
unset($smarty);
?>