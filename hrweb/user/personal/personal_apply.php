<?php
/*
 * 74cms 个人会员中心
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__) . '/personal_common.php');
$smarty->assign('leftmenu',"apply");
if ($act=='down')
{
	$perpage=10;
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$joinsql=" LEFT JOIN  ".table('company_profile')." AS c  ON d.company_uid=c.uid ";
	$wheresql=" WHERE d.resume_uid='{$_SESSION['uid']}' ";
	$resume_id =intval($_GET['resume_id']);
	if($resume_id>0)$wheresql.=" AND  d.resume_id='{$resume_id}' ";	
	$settr=intval($_GET['settr']);
	if($settr>0)
	{
	$settr_val=strtotime("-".$settr." day");
	$wheresql.=" AND d.down_addtime>".$settr_val;
	}
	$total_sql="SELECT COUNT(*) AS num from ".table('company_down_resume')." AS d {$wheresql}";
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$smarty->assign('title',"谁下载的我的简历 - 个人会员中心 - {$_CFG['site_name']}");
	$smarty->assign('mylist',get_com_downresume($offset,$perpage,$joinsql.$wheresql));
	$smarty->assign('page',$page->show(3));
	$smarty->assign('count',$total_val);
	$smarty->assign('act',$act);
	$smarty->assign('resume_list',get_auditresume_list($_SESSION['uid']));
	$smarty->display('member_personal/personal_downresume.htm');
}
//面试邀请 列表
elseif ($act=='interview')
{
	$perpage=10;
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$wheresql=" WHERE  i.resume_uid='{$_SESSION['uid']}' ";
	$look=intval($_GET['look']);
	if($look>0)
	{
	$wheresql.=" AND  i.personal_look={$look}";
	}
	$resume_id =intval($_GET['resume_id']);
	if($resume_id>0)
	{
		$wheresql.=" AND  i.resume_id='{$resume_id}' ";	
		$sql="select title from ".table("resume")." where id=".intval($_GET['resume_id'])." ";
		$row=$db->getone($sql);
		$smarty->assign('resume_title',$row["title"]);
	}
	//筛选 普通职位面试(0)  还是高级职位面试(1)
	$jobs_type=intval($_GET['jobs_type']);
	if($jobs_type != 1)
	{
		$total_sql="SELECT COUNT(*) AS num from ".table('company_interview')." AS i {$wheresql} ";
		$total_val=$db->get_total($total_sql);
		$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
		$currenpage=$page->nowindex;
		$offset=($currenpage-1)*$perpage;
		$joinsql=" LEFT JOIN  ".table('jobs')." AS j  ON  i.jobs_id=j.id ";
		$smarty->assign('interview',get_invitation($offset, $perpage,$joinsql.$wheresql));
	}
	if($total_val > $perpage)
	{
		$smarty->assign('page',$page->show(3));
	}
	$smarty->assign('title','收到的面试邀请 - 个人会员中心 - '.$_CFG['site_name']);
	$smarty->assign('act',$act);
	$count[0]=count_interview($_SESSION['uid'],$jobs_type,1);  //未看
	$count[1]=count_interview($_SESSION['uid'],$jobs_type,2);  //已看
	$count[2]=$count[0]+$count[1];
	$smarty->assign('count',$count);
	$smarty->assign('resume_list',get_interview_resumes($_SESSION['uid']));
	$smarty->display('member_personal/personal_interview.htm');
}
elseif ($act=='set_interview')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择项目！",1);
	$jobs_type=intval($_GET['jobs_type']);
	$n=set_invitation($yid,$_SESSION['uid'],2);
	if($n)
	{
		showmsg("设置成功！",2);
	}
	else
	{
		showmsg("设置失败！",0);
	}
}
elseif ($act=='interview_del')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择项目！",1);
	$jobs_type=intval($_GET['jobs_type']);
	$n=del_interview($yid,$_SESSION['uid']);
	if(intval($n) > 0)
	{
	showmsg("删除成功！共删除 {$n} 行",2);
	}
	else
	{
	showmsg("失败！",0);
	}
}
//职位收藏夹列表
elseif ($act=='favorites')
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$wheresql=" WHERE f.personal_uid='{$_SESSION['uid']}' ";
	$settr=intval($_GET['settr']);
	if($settr>0)
	{
	$settr_val=strtotime("-".$settr." day");
	$wheresql.=" AND f.addtime>".$settr_val;
	}
	$perpage=10;
	$total_sql="SELECT COUNT(*) AS num FROM ".table('personal_favorites')." AS f {$wheresql} ";
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$smarty->assign('title','职位收藏夹 - 个人会员中心 - '.$_CFG['site_name']);
	$smarty->assign('act',$act);
	$joinsql=" LEFT JOIN ".table('jobs')." as  j  ON f.jobs_id=j.id ";
	$smarty->assign('favorites',get_favorites($offset,$perpage,$joinsql.$wheresql));
	if($total_val > $perpage)
	{
		$smarty->assign('page',$page->show(3));
	}
	$smarty->display('member_personal/personal_favorites.htm');
}
elseif ($act=='del_favorites')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择项目！",1);
	if($n=del_favorites($yid,$_SESSION['uid']))
	{
		showmsg("删除成功！共删除 {$n} 行",2);
	}
	else
	{
		showmsg("删除失败！",0);
	}
}
//申请的职位列表
elseif ($act=='apply_jobs')
{
	$joinsql = '';
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$wheresql=" WHERE a.personal_uid='{$_SESSION['uid']}' ";
	$resume_id =intval($_GET['resume_id']);
	//筛选简历
	if($resume_id>0)
	{
		$wheresql.=" AND  a.resume_id='{$resume_id}' ";	
	}
	//筛选 对方是否已查看
	$aetlook=intval($_GET['aetlook']);
	if($aetlook>0)
	{
		$wheresql.=" AND a.personal_look='{$aetlook}'";
	}
	//筛选 申请时间
	$settr=intval($_GET['settr']);
	if($settr>0)
	{
	$settr_val=strtotime("-".$settr." day");
	$wheresql.=" AND a.apply_addtime>".$settr_val;
	}
	//筛选 反馈 (1->企业未查看  2->待反馈  3->合适  4->不合适  5->待定  6->未接通)
	$reply_id=intval($_GET['reply_id']);
	if($reply_id == 1)
	{
		$wheresql.=" AND a.personal_look='1' ";
	}
	elseif($reply_id == 2)
	{
		$wheresql.=" AND a.personal_look='2' AND a.is_reply=0  ";
	}
	elseif($reply_id == 3)
	{
		$wheresql.=" AND a.personal_look='2' AND a.is_reply=1 ";
	}
	elseif($reply_id == 4)
	{
		$wheresql.=" AND a.personal_look='2' AND a.is_reply=2 ";
	}
	elseif($reply_id == 5)
	{
		$wheresql.=" AND a.personal_look='2' AND a.is_reply=3 ";
	}
	elseif($reply_id == 6)
	{
		$wheresql.=" AND a.personal_look='2' AND a.is_reply=4 ";
	}
	$perpage=10;
	//筛选 普通职位(0) 和 猎头职位(1)
	$jobs_type=intval($_GET['jobs_type']);
	$total_sql="SELECT COUNT(*) AS num FROM ".table('personal_jobs_apply')." AS a {$wheresql} ";
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$joinsql.=" LEFT JOIN ".table('jobs')." AS j ON a.jobs_id=j.id ";
	$smarty->assign('jobs_apply',get_apply_jobs($offset,$perpage,$joinsql,$wheresql));
	$smarty->assign('title','已申请的职位 - 个人会员中心 - '.$_CFG['site_name']);
	$smarty->assign('act',$act);
	if($total_val > $perpage)
	{
		$smarty->assign('page',$page->show(3));
	}
	$count[0]=count_personal_jobs_apply($jobs_type,$_SESSION['uid'],1); //未查看
	$count[1]=count_personal_jobs_apply($jobs_type,$_SESSION['uid'],2); //已查看
	$count[2]=$count[0]+$count[1];
	$smarty->assign('count',$count);
	$smarty->assign('resume_list',get_apply_jobs_resumes($_SESSION['uid']));
	$smarty->display('member_personal/personal_apply_jobs.htm');
}
//删除-申请的职位列表
elseif ($act=='del_jobs_apply')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("你没有选择项目！",1);
	$jobs_type=intval($_GET['jobs_type']);
	$n=del_jobs_apply($yid,$_SESSION['uid']);
	if(intval($n) > 0)
	{
		showmsg("删除成功！",2);
	}
	else
	{
		showmsg("删除失败！",0);
	}
}
unset($smarty);
?>