<?php
 /*
 * 74cms 企业推广
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_company_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"com_promotion");
$smarty->assign('pageheader',"企业推广");
if($act == 'list')
{
	get_token();
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$oderbysql=" order BY p.cp_id DESC ";
	$joinsql = " INNER JOIN ".table('jobs')." AS j INNER JOIN ".table('promotion_category')." AS c  ON p.cp_jobid=j.id AND p.cp_promotionid=c.cat_id ";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if (!empty($key) && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE j.jobs_name like '%{$key}%'";
		elseif ($key_type===2)$wheresql=" WHERE j.companyname like '%{$key}%'";
		elseif ($key_type===3)$wheresql=" WHERE j.id =".intval($key);
		elseif ($key_type===4)$wheresql=" WHERE p.cp_uid=".intval($key);
		elseif ($key_type===5)$wheresql=" WHERE p.cp_uid=".intval($key);
		$oederbysql="";
	}
	$settr=$_GET['settr'];
	if ($settr<>"")
	{
		$wheresql.=empty($wheresql)?" WHERE ":" AND  ";
		$days=intval($settr);
		$settr=strtotime($days." day");
		if ($days===0)
		{
		$wheresql.=" p.cp_endtime< ".time()." ";
		}
		else
		{
		$wheresql.=" p.cp_endtime< ".$settr." ";
		}		
	}
	$promotionid=isset($_GET['promotionid'])?intval($_GET['promotionid']):"";
	if ($promotionid>0)
	{
	$wheresql.=empty($wheresql)?" WHERE p.cp_promotionid={$promotionid} ":" AND p.cp_promotionid={$promotionid} ";
	}
	$total_sql="SELECT COUNT(*) AS num FROM ".table('promotion')." AS p ".$joinsql.$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_promotion($offset,$perpage,$joinsql.$wheresql.$oderbysql);
	$smarty->assign('navlabel',"list");
	$smarty->assign('list',$list);
	$smarty->assign('cat',get_promotion_cat(1));
	$smarty->assign('page',$page->show(3));
	$smarty->display('promotion/admin_promotion_list.htm');
}
elseif($act == 'promotion_add')
{
	get_token();
	$smarty->assign('navlabel',"add");
	$smarty->assign('list',get_promotion_cat());
	$smarty->assign('cat',get_promotion_cat(1));	
	$smarty->display('promotion/admin_promotion_add.htm');
}
elseif($act == 'promotion_save')
{
	check_token();
	$setsqlarr['cp_days']=intval($_POST['days']);
	if ($setsqlarr['cp_days']==0)
	{
		adminmsg("请填写推广天数",1);
	}
	$setsqlarr['cp_jobid']=intval($_POST['jobid']);
	$setsqlarr['cp_promotionid']=intval($_POST['promotionid']);
	if (check_promotion($setsqlarr['cp_jobid'],$setsqlarr['cp_promotionid']))
	{
		adminmsg("此职位正在执行此推广！请选择其他职位或者其他推广方案",1);
	}
	else
	{
		if ($setsqlarr['cp_promotionid']=="4")
		{
		$setsqlarr['cp_val']=!empty($_POST['val'])?$_POST['val']:adminmsg("请选择颜色",1);
		}
		$setsqlarr['cp_starttime']=time();
		$setsqlarr['cp_endtime']=strtotime("{$setsqlarr['cp_days']} day");
		$setsqlarr['cp_available']=1;
		$setsqlarr['cp_hour_cn']=trim($_POST['hour']);
		$setsqlarr['cp_hour']=intval($_POST['hour']);
		$jobs=get_jobs_one($setsqlarr['cp_jobid']);
		$setsqlarr['cp_uid']=$jobs['uid'];
		if ($db->inserttable(table('promotion'),$setsqlarr))
		{
		$u=get_user($setsqlarr['cp_uid']);
		$promotion=get_promotion_cat_one($setsqlarr['cp_promotionid']);
		write_memberslog($u['uid'],1,3004,$u['username'],"管理员增加推广：{$promotion['cat_name']},职位ID：{$setsqlarr['cp_jobid']}");
		set_job_promotion($setsqlarr['cp_jobid'],$setsqlarr['cp_promotionid'],$setsqlarr['cp_val']);
		write_log("添加推广：{$promotion['cat_name']},职位ID：{$setsqlarr['cp_jobid']}", $_SESSION['admin_name'],3);
		$link[0]['text'] = "返回列表";
		$link[0]['href'] = "?act=list";
		adminmsg("添加成功",2,$link);		
		}
	}
}
elseif($act == 'promotion_edit')
{
	get_token();
	$id=intval($_GET['id']);
	$show = get_promotion_one($id);
	$jobs = get_jobs_one($show['cp_jobid']);
	$promotion = get_promotion_cat_one($show['cp_promotionid']);
	$smarty->assign('time',time());
	$smarty->assign('show',$show);
	$smarty->assign('jobs',$jobs);
	$smarty->assign('promotion',$promotion);
	$smarty->display('promotion/admin_promotion_edit.htm');
}
elseif($act == 'promotion_edit_save')
{
	check_token();
	$setsqlarr['cp_id']=intval($_POST['id']);
	$setsqlarr['cp_promotionid']=intval($_POST['promotionid']);
	$setsqlarr['cp_hour_cn']=trim($_POST['hour']);
	$setsqlarr['cp_hour']=intval($_POST['hour']);
	$days=intval($_POST['days']);	
	if ($setsqlarr['cp_promotionid']=="4")
	{
	$setsqlarr['cp_val']=trim($_POST['val']);
	}
	if ($days>0)
	{
	$endtime=intval($_POST['endtime']);
	$setsqlarr['cp_endtime']=$endtime>time()?$endtime+($days*(60*60*24)):strtotime("".$days." day");
	}
	$wheresql=" cp_id='{$setsqlarr['cp_id']}' ";
	if ($db->updatetable(table('promotion'),$setsqlarr,$wheresql))
	{
		if ($setsqlarr['cp_promotionid']=="4")
		{
			$jobid=intval($_POST['jobid']);
		 	$db->query("UPDATE ".table('jobs')." SET highlight='{$setsqlarr['cp_val']}' WHERE id='{$jobid}' ");
			$db->query("UPDATE ".table('jobs_tmp')." SET highlight='{$setsqlarr['cp_val']}' WHERE id='{$jobid}' ");
		}
		write_log("修改推广id为".$setsqlarr['cp_id']."的推广", $_SESSION['admin_name'],3);
		$link[0]['text'] = "推广列表";
		$link[0]['href'] ="?act=list";
		adminmsg("修改成功！",2,$link);
	}	
}
elseif($act == 'promotion_del')
{
	get_token();
	if ($n=del_promotion($_POST['id']))
	{
	adminmsg("取消成功！共取消 {$n} 行",2);
	}
	else
	{
	adminmsg("取消失败！",0);
	}
}
elseif($act == 'category')
{
	get_token();
	$smarty->assign('navlabel',"category");
	$smarty->assign('list',get_promotion_cat());	
	$smarty->display('promotion/admin_promotion_category.htm');
}
elseif($act == 'edit_category')
{
	get_token();
	$id=intval($_GET['id']);
	$smarty->assign('navlabel',"category");
	$smarty->assign('show',get_promotion_cat_one($id));	
	$smarty->display('promotion/admin_promotion_category_edit.htm');
}
elseif($act=='edit_category_save')
{	
	check_token();
	$setsqlarr['cat_name']=trim($_POST['cat_name'])?trim($_POST['cat_name']):adminmsg('您没有填写方案名称！',1);
	$setsqlarr['cat_available']=intval($_POST['cat_available']);
	$setsqlarr['cat_minday']=intval($_POST['cat_minday']);
	$setsqlarr['cat_maxday']=intval($_POST['cat_maxday']);
	$setsqlarr['cat_points']=intval($_POST['cat_points']);
	$setsqlarr['cat_order']=intval($_POST['cat_order']);
	$setsqlarr['cat_notes']=trim($_POST['cat_notes']);
	$wheresql=" cat_id='".intval($_POST['id'])."'";
		if ($db->updatetable(table('promotion_category'),$setsqlarr,$wheresql))
		{
		$link[0]['text'] = "方案列表";
		$link[0]['href'] ="?act=category";
		adminmsg("修改成功！",2,$link);
		}
		else
		{
		adminmsg("修改失败！",0);
		}
}
?>