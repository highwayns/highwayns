<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/company_common.php');
$smarty->assign('leftmenu',"recruitment");
if ($act=='apply_jobs')
{
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
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
	$smarty->assign('title','職位申請 - 企業会員センター - '.$_CFG['site_name']);
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
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("項目を選択してください！",1);
	set_apply($yid,$_SESSION['uid'],2)?showmsg("設定成功！",2):showmsg("設定失敗！",0);
}
elseif ($act=='apply_jobs_del')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("项目を選択してください！",1);
	if ($n=del_apply_jobs($yid,$_SESSION['uid']))
	{
	showmsg("削除成功！削除行数 {$n} ",2);
	}
	else
	{
	showmsg("失敗！",0);
	}
}
elseif ($act=='down_resume_list')
{
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
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
	$smarty->assign('title','已ダウンロード的履歴書 - 企業会員センター - '.$_CFG['site_name']);
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
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("履歴書を選択してください！",1);
	if ($n=del_down_resume($yid,$_SESSION['uid']))
	{
	showmsg("削除成功！削除行数 {$n} ",2);
	}
	else
	{
	showmsg("失敗！",0);
	}
}
elseif ($act=='perform')
{
	$id =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("履歴書を選択してください！",1);
	if(!empty($_REQUEST['shift'])){
		$num=down_to_favorites($id,$_SESSION['uid']);
		if ($num==='full')
		{
		showmsg("人材庫いっぱい!",1);
		}
		elseif($num>0)
		{
		showmsg("追加成功，追加件数 {$num} 件",2);
		}
		else
		{
		showmsg("追加失敗,既に存在します！",1);
		}
	}
	
}
elseif ($act=='favorites_list')
{
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
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
	$smarty->assign('title','企業人材一覧 - 企業会員センター - '.$_CFG['site_name']);
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
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("履歴書を選択してください！",1);
	if ($n=del_favorites($yid,$_SESSION['uid']))
	{
	showmsg("削除成功！削除行数 {$n} ",2);
	}
	else
	{
	showmsg("失敗！",0);
	}
}
//已邀请面试列表
elseif ($act=='interview_list')
{
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
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
	$smarty->assign('title','面接誘い - 企業会員センター - '.$_CFG['site_name']);
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
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("履歴書を選択してください！",1);
	if (del_interview($yid,$_SESSION['uid']))
	{
		showmsg("削除成功！",2);
	}
	else
	{
		showmsg("削除失敗！",0);
	}
}
//收藏 简历
elseif($act == 'fav_att_resume')
{
	$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:showmsg("履歴書を選択してください！",1);
	$n = add_favorites($yid,$_SESSION['uid']);
	if(intval($n) > 0)
	{
	showmsg("お気に入り成功！行数 {$n} 行",2);
	}
	else
	{
	showmsg("お気に入り失敗！",0);
	}
}
unset($smarty);
?>
