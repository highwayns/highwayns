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
$smarty->assign('leftmenu',"recruitment");
if ($act=='apply_jobs')
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$joinsql=" LEFT JOIN  ".table('resume')." AS r  ON  a.resume_id=r.id ";
	$wheresql=" WHERE a.company_uid='{$_SESSION['uid']}' ";
	$look=intval($_GET['look']);
	if($look>0)$wheresql.=" AND a.personal_look='{$look}'";
	$state=intval($_GET['state']);
	if($state>0)
	{
		$joinsql.=" left join ".table('company_label_resume')." as l on l.resume_id=a.resume_id ";
		$wheresql.=" AND l.resume_state=$state AND l.uid={$_SESSION['uid']} ";
	}
	$jobsid=intval($_GET['jobsid']);
	if($jobsid>0){
		$wheresql.=" AND a.jobs_id='{$jobsid}' ";
		$sql="select jobs_name from ".table("jobs")." where id=".intval($_GET['jobsid'])." ";
		$row=$db->getone($sql);
		$smarty->assign('jobs_name',$row["jobs_name"]);
	}
	$perpage=10;
	$total_sql="SELECT COUNT(*) AS num FROM ".table('personal_jobs_apply')." AS a  ".$joinsql." {$wheresql} ";
	$total=$db->get_total($total_sql);
	$page = new page(array('total'=>$total, 'perpage'=>$perpage));
	$offset=($page->nowindex-1)*$perpage;
	$smarty->assign('act',$act);
	$smarty->assign('title','收到的职位申请 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('jobs_apply',get_apply_jobs($offset,$perpage,$joinsql.$wheresql));
	if($total>$perpage)
	{
		$smarty->assign('page',$page->show(3));
	}
	$smarty->assign('count',count_jobs_apply($_SESSION['uid'],0,$jobsid));
	$smarty->assign('count1',count_jobs_apply($_SESSION['uid'],1,$jobsid));
	$smarty->assign('count2',count_jobs_apply($_SESSION['uid'],2,$jobsid));
	$smarty->assign('jobs',get_auditjobs($_SESSION['uid']));
	$smarty->display('member_company/company_apply_jobs.htm');
}
elseif ($act=='set_apply_jobs')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择任何项目！",1);
	set_apply($yid,$_SESSION['uid'],2)?showmsg("设置成功！",2):showmsg("设置失败！",0);
}
elseif ($act=='apply_jobs_del')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择项目！",1);
	if ($n=del_apply_jobs($yid,$_SESSION['uid']))
	{
	showmsg("删除成功！共删除 {$n} 行",2);
	}
	else
	{
	showmsg("失败！",0);
	}
}
elseif ($act=='down_resume_list')
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$perpage=10;
	$joinsql=" LEFT JOIN  ".table('resume')." as r ON d.resume_id=r.id ";
	$wheresql=" WHERE  d.company_uid='{$_SESSION['uid']}' ";
	$settr=intval($_GET['settr']);
	$talent=intval($_GET['talent']);
	$state=intval($_GET['state']);//标记状态
	if($settr>0)
	{
	$settr_val=strtotime("-{$settr} day");
	$wheresql.=" AND d.down_addtime>{$settr_val} ";
	}
	if($talent){
		$wheresql.=" AND r.talent=1 ";
	}
	if($state>0)
	{
		$joinsql.=" left join ".table('company_label_resume')." as l on l.resume_id=d.resume_id ";
		$wheresql.=" AND l.resume_state=$state ";
	}

	$total_sql="SELECT COUNT(*) AS num FROM ".table('company_down_resume')." as d".$joinsql.$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$smarty->assign('title','已下载的简历 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('act',$act);
	$smarty->assign('list',get_down_resume($offset,$perpage,$joinsql.$wheresql));
	if($total_val>$perpage)
	{
		$smarty->assign('page',$page->show(3));
	}
	$smarty->display('member_company/company_down_resume.htm');
}
elseif ($act=='down_resume_del')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择简历！",1);
	if ($n=del_down_resume($yid,$_SESSION['uid']))
	{
	showmsg("删除成功！共删除 {$n} 行",2);
	}
	else
	{
	showmsg("失败！",0);
	}
}
elseif ($act=='perform')
{
	$id =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择简历！",1);
	if(!empty($_REQUEST['shift'])){
		$num=down_to_favorites($id,$_SESSION['uid']);
		if ($num==='full')
		{
		showmsg("人才库已满!",1);
		}
		elseif($num>0)
		{
		showmsg("添加成功，共添加 {$num} 条",2);
		}
		else
		{
		showmsg("添加失败,已经存在！",1);
		}
	}
	
}
elseif ($act=='favorites_list')
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$perpage=10;
	$joinsql=" LEFT JOIN  ".table('resume')." AS r ON  f.resume_id=r.id ";
	$wheresql= " WHERE f.company_uid='{$_SESSION['uid']}' ";
	$settr=intval($_GET['settr']);
	if($settr>0)
	{
	$settr_val=strtotime("-".$settr." day");
	$wheresql.=" AND f.favoritesa_ddtime>".$settr_val;
	}
	$total_sql="SELECT COUNT(*) AS num FROM ".table('company_favorites')." AS f ".$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$offset=($page->nowindex-1)*$perpage;
	$smarty->assign('title','企业人才库 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('act',$act);
	$smarty->assign('favorites',get_favorites($offset, $perpage,$joinsql.$wheresql));
	if($total_val>$perpage)
	{
		$smarty->assign('page',$page->show(3));
	}
	$smarty->display('member_company/company_favorites.htm');
}
elseif ($act=='favorites_del')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择简历！",1);
	if ($n=del_favorites($yid,$_SESSION['uid']))
	{
	showmsg("删除成功！共删除 {$n} 行",2);
	}
	else
	{
	showmsg("失败！",0);
	}
}
//已邀请面试列表
elseif ($act=='interview_list')
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$perpage=10;
	$joinsql=" LEFT JOIN ".table('resume')." as r ON i.resume_id=r.id ";
	$wheresql=" WHERE i.company_uid='{$_SESSION['uid']}' ";
	//面试职位 筛选
	$jobsid=intval($_GET['jobsid']);
	if($jobsid>0)
	{
		$wheresql.=" AND i.jobs_id='{$jobsid}' ";
	}
	//对方查看状态 帅选
	$look=intval($_GET['look']);
	if($look>0)$wheresql.=" AND  i.personal_look='{$look}' ";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('company_interview')." as i ".$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$resume = get_interview($offset, $perpage,$joinsql.$wheresql);
	$smarty->assign('act',$act);
	$smarty->assign('title','我发起的面试邀请 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('resume',$resume);
	$count1=count_interview($_SESSION['uid'],1,$jobsid);//未查看
	$count2=count_interview($_SESSION['uid'],2,$jobsid);//已查看
	$count=$count1+$count2;
	$smarty->assign('count',$count);
	$smarty->assign('count1',$count1);
	$smarty->assign('count2',$count2);
	$smarty->assign('filter_jobs',get_interview_jobs($_SESSION['uid']));
	if($total_val>$perpage)
	{
		$smarty->assign('page',$page->show(3));
	}
	$smarty->display('member_company/company_interview.htm');
}
//删除面试邀请信息
elseif ($act=='interview_del')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择简历！",1);
	if (del_interview($yid,$_SESSION['uid']))
	{
		showmsg("删除成功！",2);
	}
	else
	{
		showmsg("删除失败！",0);
	}
}
//收藏 简历
elseif($act == 'fav_att_resume')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择简历！",1);
	$n = add_favorites($yid,$_SESSION['uid']);
	if(intval($n) > 0)
	{
	showmsg("收藏成功！共收藏 {$n} 行",2);
	}
	else
	{
	showmsg("收藏失败！",0);
	}
}
unset($smarty);
?>