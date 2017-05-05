<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_company.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$smarty->cache = false;
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'index';
if ($_SESSION['uid']=='' || $_SESSION['username']==''||intval($_SESSION['utype'])==2)
{
	header("Location: ../wap_login.php");
}
elseif ($act == 'index')
{
	$smarty->cache = false;
	$uid=intval($_SESSION["uid"]);
	$wheresql=" select * from ".table("jobs")." where uid=$_SESSION[uid] union all select * from ".table("jobs_tmp")." where uid=$uid ";
	$row=get_jobs($offset,$perpage,$wheresql,true);
	$smarty->assign('row',$row);
	$smarty->display("wap/company/wap-job-index.html");	
}
// 发布职位
elseif($act=="jobs_add")
{
	$smarty->cache = false;
	$company_info=get_company(intval($_SESSION['uid']));
	if($company_info['companyname'])
	{
		$smarty->assign('company_info',$company_info);
		if ($_CFG['operation_mode']=="3")
		{
			$setmeal=get_user_setmeal(intval($_SESSION['uid']));
			if (($setmeal['endtime']>time() || $setmeal['endtime']=="0") &&  $setmeal['jobs_ordinary']>0)
			{
			$smarty->assign('setmeal',$setmeal);
			$smarty->assign('add_mode',2);
			}
			elseif($_CFG['setmeal_to_points']=="1")
			{
			$smarty->assign('points_total',get_user_points(intval($_SESSION['uid'])));
			$smarty->assign('points',get_cache('points_rule'));
			$smarty->assign('add_mode',1);
			}
			else
			{
			$smarty->assign('setmeal',$setmeal);
			$smarty->assign('add_mode',2);
			}
		}
		elseif ($_CFG['operation_mode']=="2")
		{
			$setmeal=get_user_setmeal(intval($_SESSION['uid']));
			$smarty->assign('setmeal',$setmeal);
			$smarty->assign('add_mode',2);
		}
		elseif ($_CFG['operation_mode']=="1")
		{
			$smarty->assign('points_total',get_user_points(intval($_SESSION['uid'])));
			$smarty->assign('points',get_cache('points_rule'));
			$smarty->assign('add_mode',1);
		}
		$smarty->display("wap/company/wap-create-job.html");
	}else{
		header("Location: wap_user.php?act=company_info_add");
	}
}
// 保存职位
elseif($act=="jobs_add_save")
{
	$smarty->cache = false;
	$company_info=get_company(intval($_SESSION['uid']));
	$company_info = array_map("addslashes", $company_info);
	$_POST=array_map("utf8_to_gbk", $_POST);
	$add_mode=trim($_POST['add_mode']);
	if ($add_mode=='1')
	{
					$points_rule=get_cache('points_rule');
					$user_points=get_user_points($_SESSION['uid']);
					$total=0;
					if ($points_rule['jobs_add']['type']=="2" && $points_rule['jobs_add']['value']>0)
					{
					$total=$points_rule['jobs_add']['value'];
					}
					if ($points_rule['jobs_daily']['type']=="2" && $points_rule['jobs_daily']['value']>0)
					{
					$total=$total+($days*$points_rule['jobs_daily']['value']);
					}
					if ($total>$user_points)
					{
					exit("貴方の".$_CFG['points_byname']."ポイント不足，振込してください！");
					}
					$setsqlarr['setmeal_deadline']=0;
	}
	elseif ($add_mode=='2')
	{
				$setmeal=get_user_setmeal($_SESSION['uid']);
				if ($setmeal['endtime']<time() && $setmeal['endtime']<>"0")
				{					
					exit("サービス期限切れた，再申し込みしてください");
				}
				if ($setmeal['jobs_ordinary']<=0)
				{
					exit("配布された職位は最大制限を超えました，サービスコースアップグレードしてください！");
				}
				$setsqlarr['setmeal_deadline']=$setmeal['endtime'];
				$setsqlarr['setmeal_id']=$setmeal['setmeal_id'];
				$setsqlarr['setmeal_name']=$setmeal['setmeal_name'];
	}
	$setsqlarr['add_mode']=intval($add_mode);
	$setsqlarr['uid']=intval($_SESSION['uid']);
	$setsqlarr['companyname']=$company_info['companyname'];
	$setsqlarr['company_id']=$company_info['id'];
	$setsqlarr['company_addtime']=$company_info['addtime'];
	$setsqlarr['company_audit']=$company_info['audit'];
	$setsqlarr['jobs_name']=!empty($_POST['jobs_name'])?trim($_POST['jobs_name']):exit('職位名を入力してください！');
	$setsqlarr['nature']=intval($_POST['nature'])?trim($_POST['nature']):exit('職位性質を選択してください!');
	$setsqlarr['nature_cn']=trim($_POST['nature_cn'])?trim($_POST['nature_cn']):exit('職位性質を選択してください!');
	$setsqlarr['topclass']=intval($_POST['topclass']);
	$setsqlarr['category']=!empty($_POST['category'])?intval($_POST['category']):exit('職業種類を選択してください！');
	$setsqlarr['subclass']=intval($_POST['subclass']);
	$setsqlarr['category_cn']=trim($_POST['category_cn']);
	$setsqlarr['amount']=intval($_POST['amount'])?intval($_POST['amount']):exit('募集人数を入力してください!');
	$setsqlarr['district']=!empty($_POST['district'])?intval($_POST['district']):exit('仕事地区を選択してください！');
	$setsqlarr['sdistrict']=intval($_POST['sdistrict']);
	$setsqlarr['district_cn']=trim($_POST['district_cn']);
	$setsqlarr['wage']=intval($_POST['wage'])?intval($_POST['wage']):exit('給料選択してください！');		
	$setsqlarr['wage_cn']=trim($_POST['wage_cn']);
	// $setsqlarr['negotiable']=intval($_POST['negotiable']);
	// $setsqlarr['tag']=trim($_POST['tag']);
	$setsqlarr['sex']=intval($_POST['sex'])?intval($_POST['sex']):exit("性別を選択してください!");
	$setsqlarr['sex_cn']=trim($_POST['sex_cn']);
	$setsqlarr['education']=intval($_POST['education'])?intval($_POST['education']):exit('学歴要求を入力してください！');		
	$setsqlarr['education_cn']=trim($_POST['education_cn']);
	$setsqlarr['experience']=intval($_POST['experience'])?intval($_POST['experience']):exit('仕事経験を選択してください！');		
	$setsqlarr['experience_cn']=trim($_POST['experience_cn']);
	$setsqlarr['graduate']=intval($_POST['graduate']);
	// $setsqlarr['age']=trim($_POST['minage'])."-".trim($_POST['maxage']);
	$setsqlarr['contents']=!empty($_POST['contents'])?trim($_POST['contents']):exit('職位説明を入力してください！');
	check_word($_CFG['filter'],$_POST['contents'])?exit($_CFG['filter_tips']):'';
	$setsqlarr['trade']=$company_info['trade'];
	$setsqlarr['trade_cn']=$company_info['trade_cn'];
	$setsqlarr['scale']=$company_info['scale'];
	$setsqlarr['scale_cn']=$company_info['scale_cn'];
	$setsqlarr['street']=$company_info['street'];
	$setsqlarr['street_cn']=$company_info['street_cn'];
	$setsqlarr['addtime']=time();
	$setsqlarr['deadline']=strtotime("".intval($_CFG['company_add_days'])." day");
	$setsqlarr['refreshtime']=time();
	$setsqlarr['key']=$setsqlarr['jobs_name'].$company_info['companyname'].$setsqlarr['category_cn'].$setsqlarr['district_cn'].$setsqlarr['contents'];
	require_once(HIGHWAY_ROOT_PATH.'include/splitword.class.php');
	$sp = new SPWord();
	$setsqlarr['key']="{$setsqlarr['jobs_name']} {$company_info['companyname']} ".$sp->extracttag($setsqlarr['key']);
	$setsqlarr['key']=$sp->pad($setsqlarr['key']);
	$setsqlarr['tpl']=$company_info['tpl'];
	$setsqlarr['map_x']=$company_info['map_x'];
	$setsqlarr['map_y']=$company_info['map_y'];
	if ($company_info['audit']=="1")
	{
	$setsqlarr['audit']=intval($_CFG['audit_verifycom_addjob']);
	}
	else
	{
	$setsqlarr['audit']=intval($_CFG['audit_unexaminedcom_addjob']);
	}
	$setsqlarr_contact['contact']=!empty($_POST['contact'])?trim($_POST['contact']):exit('連絡先を入力してください！');
	$setsqlarr_contact['telephone']=!empty($_POST['telephone'])?trim($_POST['telephone']):exit('連絡電話入力してください！');
	check_word($_CFG['filter'],$_POST['telephone'])?exit($_CFG['filter_tips']):'';

	$setsqlarr_contact['contact_show']=1;
	$setsqlarr_contact['email_show']=1;
	$setsqlarr_contact['telephone_show']=1;
	$setsqlarr_contact['address_show']=1;
	//添加职位信息
	$pid=$db->inserttable(table('jobs'),$setsqlarr,1);
	if(empty($pid)){
		exit("err");
	}
	//添加联系方式
	$setsqlarr_contact['pid']=$pid;
	if(!$db->inserttable(table('jobs_contact'),$setsqlarr_contact))exit("連絡先エラー");
	if ($add_mode=='1')
	{
		if ($points_rule['jobs_add']['value']>0)
		{
		report_deal($_SESSION['uid'],$points_rule['jobs_add']['type'],$points_rule['jobs_add']['value']);
		$user_points=get_user_points($_SESSION['uid']);
		$operator=$points_rule['jobs_add']['type']=="1"?"+":"-";
		write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"職位を配布しました：<strong>{$setsqlarr['jobs_name']}</strong>，({$operator}{$points_rule['jobs_add']['value']})，(残る:{$user_points})",1,1001,"職位配布","{$operator}{$points_rule['jobs_add']['value']}","{$user_points}");
		}
		if (intval($_POST['days'])>0 && $points_rule['jobs_daily']['value']>0)
		{
		$points_day=intval($_POST['days'])*$points_rule['jobs_daily']['value'];
		report_deal($_SESSION['uid'],$points_rule['jobs_daily']['type'],$points_day);
		$user_points=get_user_points($_SESSION['uid']);
		$operator=$points_rule['jobs_daily']['type']=="1"?"+":"-";
		write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"配布職位:<strong>{$_POST['jobs_name']}</strong>，有效期限は$_POST['days']}日，({$operator}{$points_day})，(残る:{$user_points})",1,1001,"職位配布","{$operator}{$points_day}","{$user_points}");
		}
	}	
	elseif ($add_mode=='2')
	{
		action_user_setmeal($_SESSION['uid'],"jobs_ordinary");
		$setmeal=get_user_setmeal($_SESSION['uid']);
		write_memberslog($_SESSION['uid'],1,9002,$_SESSION['username'],"普通職位配布:<strong>{$_POST['jobs_name']}</strong>，配布可能普通職位件数:<strong>{$setmeal['jobs_ordinary']}</strong>件",2,1001,"職位配布","1","{$setmeal['jobs_ordinary']}");
	}
	$searchtab['id']=$pid;
	$searchtab['uid']=$setsqlarr['uid'];
	$searchtab['recommend']=$setsqlarr['recommend'];
	$searchtab['emergency']=$setsqlarr['emergency'];
	$searchtab['nature']=$setsqlarr['nature'];
	$searchtab['sex']=$setsqlarr['sex'];
	$searchtab['topclass']=$setsqlarr['topclass'];
	$searchtab['category']=$setsqlarr['category'];
	$searchtab['subclass']=$setsqlarr['subclass'];
	$searchtab['trade']=$setsqlarr['trade'];
	$searchtab['district']=$setsqlarr['district'];
	$searchtab['sdistrict']=$setsqlarr['sdistrict'];	
	$searchtab['street']=$company_info['street'];
	$searchtab['education']=$setsqlarr['education'];
	$searchtab['experience']=$setsqlarr['experience'];
	$searchtab['wage']=$setsqlarr['wage'];
	$searchtab['refreshtime']=$setsqlarr['refreshtime'];
	$searchtab['scale']=$setsqlarr['scale'];	
	$searchtab['setmeal_id']=$setsqlarr['setmeal_id'];
	//
	$db->inserttable(table('jobs_search_wage'),$searchtab);
	$db->inserttable(table('jobs_search_scale'),$searchtab);
	//
	$searchtab['map_x']=$setsqlarr['map_x'];
	$searchtab['map_y']=$setsqlarr['map_y'];
	$db->inserttable(table('jobs_search_rtime'),$searchtab);

	unset($searchtab['map_x'],$searchtab['map_y']);
	//
	$searchtab['stick']=$setsqlarr['stick'];
	$db->inserttable(table('jobs_search_stickrtime'),$searchtab);

	unset($searchtab['stick']);
	//
	$searchtab['click']=$setsqlarr['click'];
	$db->inserttable(table('jobs_search_hot'),$searchtab);

	unset($searchtab['click']);
	//
	$searchtab['key']=$setsqlarr['key'];
	$searchtab['likekey']=$setsqlarr['jobs_name'].','.$setsqlarr['companyname'];
	$searchtab['map_x']=$setsqlarr['map_x'];
	$searchtab['map_y']=$setsqlarr['map_y'];
	$db->inserttable(table('jobs_search_key'),$searchtab);
	unset($searchtab);
	distribution_jobs($pid,$_SESSION['uid']);
	write_memberslog($_SESSION['uid'],1,2001,$_SESSION['username'],"職位配布：{$setsqlarr['jobs_name']}");
	baidu_submiturl(url_rewrite('HW_jobsshow',array('id'=>$pid)),'addjob');
	echo $pid;
}
// 职位发布成功页面
elseif($act=="addjobs_save_succeed")
{
	$smarty->cache = false;
	$jobs_id=intval($_GET["id"]);
	$jobs=jobs_one($jobs_id);
	$sql="select * from ".table("resume")." as r left join ".table("resume_jobs")." as rj on rj.pid=r.id where rj.category=$jobs[category] limit 5 ";
	$resume_list=$db->getall($sql);
	foreach ($resume_list as $key => $val) {
		$val['age']=date("Y")-$val['birthdate'];
		$resume_list[$key]=$val;
	}
	$smarty->assign('resume_list',$resume_list);
	$smarty->display("wap/company/wap-create-job-success.html");
}
// 职位修改 页面
elseif($act=="jobs_edit")
{
	$smarty->cache = false;
	$jobs=get_jobs_one($_GET['id'],$_SESSION['uid']);
	if($jobs){
		$jobs['contents'] = strip_tags(htmlspecialchars_decode($jobs['contents'],ENT_QUOTES));
	}
	$company_info=get_company($_SESSION['uid']);
	$smarty->assign('company_info',$company_info);
	$smarty->assign('jobs',$jobs);
	$smarty->display("wap/company/wap-jobs_edit.html");
}
elseif($act=="jobs_edit_save")
{
	$smarty->cache = false;
	$company_info=get_company($_SESSION['uid']);
	$company_info = array_map("addslashes", $company_info);
	$id=intval($_POST['id']);
	$days=intval($_POST['days']);
	$_POST=array_map("utf8_to_gbk", $_POST);
	// var_dump($_POST);die;
	$add_mode=trim($_POST['add_mode']);
	if ($add_mode=='1')
	{
		$points_rule=get_cache('points_rule');
		$user_points=get_user_points($_SESSION['uid']);
		$total=0;
		if($points_rule['jobs_edit']['type']=="2" && $points_rule['jobs_edit']['value']>0)
		{
		$total=$points_rule['jobs_edit']['value'];
		}
		if($points_rule['jobs_daily']['type']=="2" && $points_rule['jobs_daily']['value']>0)
		{
		$total=$total+($days*$points_rule['jobs_daily']['value']);
		}
		if ($total>$user_points)
		{
		exit("貴方の".$_CFG['points_byname']."ポイント不足，振込してください！");
		}
	}
	elseif ($add_mode=='2')
	{
		$setmeal=get_user_setmeal($_SESSION['uid']);
		if ($setmeal['endtime']<time() && $setmeal['endtime']<>"0")
		{					
			exit("コース期限切れた，再申し込みしてください");
		}
	}
	$setsqlarr['jobs_name']=!empty($_POST['jobs_name'])?trim($_POST['jobs_name']):exit('職位名を入力してください！');
	$setsqlarr['nature']=intval($_POST['nature'])?trim($_POST['nature']):exit('職位性質を選択してください!');
	$setsqlarr['nature_cn']=trim($_POST['nature_cn'])?trim($_POST['nature_cn']):exit('職位性質を選択してください!');
	$setsqlarr['topclass']=intval($_POST['topclass']);
	$setsqlarr['category']=!empty($_POST['category'])?intval($_POST['category']):exit('職業種類を選択してください！');
	$setsqlarr['subclass']=intval($_POST['subclass']);
	$setsqlarr['category_cn']=trim($_POST['category_cn']);
	$setsqlarr['amount']=intval($_POST['amount'])?intval($_POST['amount']):exit('募集人数を入力してください!');
	$setsqlarr['district']=!empty($_POST['district'])?intval($_POST['district']):exit('仕事地区を選択してください！');
	$setsqlarr['sdistrict']=intval($_POST['sdistrict']);
	$setsqlarr['district_cn']=trim($_POST['district_cn']);
	$setsqlarr['wage']=intval($_POST['wage'])?intval($_POST['wage']):exit('給料選択してください！');		
	$setsqlarr['wage_cn']=trim($_POST['wage_cn']);
	// $setsqlarr['negotiable']=intval($_POST['negotiable']);
	// $setsqlarr['tag']=trim($_POST['tag']);
	$setsqlarr['sex']=intval($_POST['sex'])?intval($_POST['sex']):exit("性別を選択してください!");
	$setsqlarr['sex_cn']=trim($_POST['sex_cn']);
	$setsqlarr['education']=intval($_POST['education'])?intval($_POST['education']):exit('学歴要求を入力してください！');		
	$setsqlarr['education_cn']=trim($_POST['education_cn']);
	$setsqlarr['experience']=intval($_POST['experience'])?intval($_POST['experience']):exit('仕事経験を選択してください！');		
	$setsqlarr['experience_cn']=trim($_POST['experience_cn']);
	$setsqlarr['graduate']=intval($_POST['graduate']);
	// $setsqlarr['age']=trim($_POST['minage'])."-".trim($_POST['maxage']);
	$setsqlarr['contents']=!empty($_POST['contents'])?trim($_POST['contents']):exit('職位説明を入力してください！');
	check_word($_CFG['filter'],$_POST['contents'])?exit($_CFG['filter_tips']):'';
	if ($add_mode=='1'){
		$setsqlarr['setmeal_deadline']=0;
		$setsqlarr['add_mode']=1;
	}elseif($add_mode=='2'){
		$setmeal=get_user_setmeal($_SESSION['uid']);
		$setsqlarr['setmeal_deadline']=$setmeal['endtime'];
		$setsqlarr['setmeal_id']=$setmeal['setmeal_id'];
		$setsqlarr['setmeal_name']=$setmeal['setmeal_name'];
		$setsqlarr['add_mode']=2;
	}
	$setsqlarr['deadline']=strtotime("".intval($_CFG['company_add_days'])." day");
	$setsqlarr['key']=$setsqlarr['jobs_name'].$company_info['companyname'].$setsqlarr['category_cn'].$setsqlarr['district_cn'].$setsqlarr['contents'];
	require_once(HIGHWAY_ROOT_PATH.'include/splitword.class.php');
	$sp = new SPWord();
	$setsqlarr['key']="{$setsqlarr['jobs_name']} {$company_info['companyname']} ".$sp->extracttag($setsqlarr['key']);
	$setsqlarr['key']=$sp->pad($setsqlarr['key']);
	if ($company_info['audit']=="1")
	{
	$_CFG['audit_verifycom_editjob']<>"-1"?$setsqlarr['audit']=intval($_CFG['audit_verifycom_editjob']):'';
	}
	else
	{
	$_CFG['audit_unexaminedcom_editjob']<>"-1"?$setsqlarr['audit']=intval($_CFG['audit_unexaminedcom_editjob']):'';
	}
	$setsqlarr_contact['contact']=!empty($_POST['contact'])?trim($_POST['contact']):exit('連絡先を入力してください！');
	$setsqlarr_contact['telephone']=!empty($_POST['telephone'])?trim($_POST['telephone']):exit('連絡電話入力してください！');
	check_word($_CFG['filter'],$_POST['telephone'])?exit($_CFG['filter_tips']):'';

	$setsqlarr_contact['contact_show']=1;
	$setsqlarr_contact['email_show']=1;
	$setsqlarr_contact['telephone_show']=1;
	$setsqlarr_contact['address_show']=1;

	if (!$db->updatetable(table('jobs'), $setsqlarr," id='{$id}' AND uid='{$_SESSION['uid']}' ")) exit("err");
	if (!$db->updatetable(table('jobs_tmp'), $setsqlarr," id='{$id}' AND uid='{$_SESSION['uid']}' ")) exit("err");
	if (!$db->updatetable(table('jobs_contact'), $setsqlarr_contact," pid='{$id}' ")){
		exit("err");
	}
	if ($add_mode=='1')
	{
		if ($points_rule['jobs_edit']['value']>0)
		{
		report_deal($_SESSION['uid'],$points_rule['jobs_edit']['type'],$points_rule['jobs_edit']['value']);
		$user_points=get_user_points($_SESSION['uid']);
		$operator=$points_rule['jobs_edit']['type']=="1"?"+":"-";
		write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"職位変更：<strong>{$setsqlarr['jobs_name']}</strong>，({$operator}{$points_rule['jobs_edit']['value']})，(残る:{$user_points})",1,1002,"募集情報変更","{$operator}{$points_rule['jobs_edit']['value']}","{$user_points}");
		}
		if ($days>0 && $points_rule['jobs_daily']['value']>0)
		{
		$points_day=intval($_POST['days'])*$points_rule['jobs_daily']['value'];
		report_deal($_SESSION['uid'],$points_rule['jobs_daily']['type'],$points_day);
		$user_points=get_user_points($_SESSION['uid']);
		$operator=$points_rule['jobs_daily']['type']=="1"?"+":"-";
		write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"延長職位({$_POST['jobs_name']})有效期間は{$_POST['days']}日，({$operator}{$points_day})，(残る:{$user_points})",1,1002,"募集情報変更","{$operator}{$points_day}","{$user_points}");
		}
	}
	$searchtab['nature']=$setsqlarr['nature'];
	$searchtab['sex']=$setsqlarr['sex'];
	$searchtab['topclass']=$setsqlarr['topclass'];
	$searchtab['category']=$setsqlarr['category'];
	$searchtab['subclass']=$setsqlarr['subclass'];
	$searchtab['district']=$setsqlarr['district'];
	$searchtab['sdistrict']=$setsqlarr['sdistrict'];
	$searchtab['education']=$setsqlarr['education'];
	$searchtab['experience']=$setsqlarr['experience'];
	$searchtab['wage']=$setsqlarr['wage'];
	//
	$db->updatetable(table('jobs_search_wage'),$searchtab," id='{$id}' AND uid='{$_SESSION['uid']}' ");
	$db->updatetable(table('jobs_search_rtime'),$searchtab," id='{$id}' AND uid='{$_SESSION['uid']}' ");
	$db->updatetable(table('jobs_search_stickrtime'),$searchtab," id='{$id}' AND uid='{$_SESSION['uid']}' ");
	$db->updatetable(table('jobs_search_hot'),$searchtab," id='{$id}' AND uid='{$_SESSION['uid']}' ");
	$db->updatetable(table('jobs_search_scale'),$searchtab," id='{$id}' AND uid='{$_SESSION['uid']}'");
	$searchtab['key']=$setsqlarr['key'];
	$searchtab['likekey']=$setsqlarr['jobs_name'].','.$company_profile['companyname'];
	$db->updatetable(table('jobs_search_key'),$searchtab," id='{$id}' AND uid='{$_SESSION['uid']}' ");
	unset($searchtab);
	// distribution_jobs($id,$_SESSION['uid']);
	write_memberslog($_SESSION['uid'],$_SESSION['utype'],2002,$_SESSION['username'],"職位修正：{$setsqlarr['jobs_name']}，職位ID：{$id}");
	exit("ok");
}
// 职位刷新
elseif($act=="jobs_refresh")
{
	$smarty->cache = false;
	$id=intval($_POST['id']);
	//积分模式
	if($_CFG['operation_mode']=='1'){
		//限制刷新时间
		//最经一次的刷新时间
		$refrestime=get_last_refresh_date($_SESSION['uid'],"1001");
		$duringtime=time()-$refrestime['max(addtime)'];
		$refresh_time = get_today_refresh_times($_SESSION['uid'],"1001");
		if($_CFG['com_pointsmode_refresh_time']!=0&&($refresh_time['count(*)']>=$_CFG['com_pointsmode_refresh_time']))
		{
		exit("毎日最大更新件数".$_CFG['com_pointsmode_refresh_time']."回,今日最大更新回数を超えた！");	
		}
		elseif($duringtime<=$space){
		exit($_CFG['com_pointsmode_refresh_space']."分以内、職位再度検索不可！");
		}
		else 
		{	
			$points_rule=get_cache('points_rule');
			if($points_rule['jobs_refresh']['value']>0)
			{
				$user_points=get_user_points($_SESSION['uid']);
				$total_point=$jobs_num*$points_rule['jobs_refresh']['value'];
				if ($total_point>$user_points && $points_rule['jobs_refresh']['type']=="2")
				{
						$link[0]['text'] = "前頁に戻る";
						$link[0]['href'] = 'javascript:history.go(-1)';
						$link[1]['text'] = "即時振込";
						$link[1]['href'] = 'company_service.php?act=order_add';
				showmsg("貴方様の".$_CFG['points_byname']."ポイント足りない，振込してください！",0,$link);
				}
				report_deal($_SESSION['uid'],$points_rule['jobs_refresh']['type'],$total_point);
				$user_points=get_user_points($_SESSION['uid']);
				$operator=$points_rule['jobs_refresh']['type']=="1"?"+":"-";
				write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"{$jobs_num}件職位更新済み，({$operator}{$total_point})，(残る:{$user_points})",1,1003,"職位更新","{$operator}{$total_point}","{$user_points}");
				write_refresh_log($_SESSION['uid'],1001);	
				refresh_jobs($id,intval($_SESSION['uid']))?exit("ok"):exit("err");
			}
			write_refresh_log($_SESSION['uid'],1001);
			refresh_jobs($id,intval($_SESSION['uid']))?exit("ok"):exit("err");
		}
	}	
	//套餐模式
	elseif($_CFG['operation_mode']=='2') 
	{
		//限制刷新时间
		//最经一次的刷新时间
		$setmeal=get_user_setmeal($_SESSION['uid']);
		if (empty($setmeal))
		{					
			exit("サービス有効にしていません，有効にしてくださ");
		}
		elseif ($setmeal['endtime']<time() && $setmeal['endtime']<>"0")
		{					
			exit("サービス期限切れた，再申し込みしてください");
		}
		else
		{
			$refrestime=get_last_refresh_date($_SESSION['uid'],"1001");
			$duringtime=time()-$refrestime['max(addtime)'];
			$space = $setmeal['refresh_jobs_space']*60;
			$refresh_time = get_today_refresh_times($_SESSION['uid'],"1001");
			if($setmeal['refresh_jobs_time']!=0&&($refresh_time['count(*)']>=$setmeal['refresh_jobs_time']))
			{
			exit("毎日最大更新件数".$setmeal['refresh_jobs_time']."回,今日最大更新回数を超えた！");
			}
			elseif($duringtime<=$space){
			exit($setmeal['refresh_jobs_space']."分以内、職位再度検索不可！");	
			}
			write_refresh_log($_SESSION['uid'],1001);
			refresh_jobs($id,intval($_SESSION['uid']))?exit("ok"):exit("err");
		}
	}
	//混合模式
	elseif($_CFG['operation_mode']=='3') 
	{
		//限制刷新时间
		//最经一次的刷新时间
		$setmeal=get_user_setmeal($_SESSION['uid']);
		$refrestime=get_last_refresh_date($_SESSION['uid'],"1001");
		$duringtime=time()-$refrestime['max(addtime)'];
		$space = $setmeal['refresh_jobs_space']*60;
		$refresh_time = get_today_refresh_times($_SESSION['uid'],"1001");
		if($setmeal['refresh_jobs_time']!=0&&($refresh_time['count(*)']>=$setmeal['refresh_jobs_time']))
		{
		exit("毎日最大更新件数".$setmeal['refresh_jobs_time']."回,今日最大更新回数を超えた！");
		}
		elseif($duringtime<=$space){
		exit($setmeal['refresh_jobs_space']."分以内、職位再度検索不可！");	
		}
		else
		{
			$points_rule=get_cache('points_rule');
			if($points_rule['jobs_refresh']['value']>0)
			{
				$user_points=get_user_points($_SESSION['uid']);
				$total_point=$jobs_num*$points_rule['jobs_refresh']['value'];
				if ($total_point>$user_points && $points_rule['jobs_refresh']['type']=="2")
				{
				exit("貴方様の".$_CFG['points_byname']."ポイント足りない，振込してください！");
				}
				report_deal($_SESSION['uid'],$points_rule['jobs_refresh']['type'],$total_point);
				$user_points=get_user_points($_SESSION['uid']);
				$operator=$points_rule['jobs_refresh']['type']=="1"?"+":"-";
				write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"{$jobs_num}件職位更新済み，({$operator}{$total_point})，(残る:{$user_points})",1,1003,"職位更新","{$operator}{$total_point}","{$user_points}");
				write_refresh_log($_SESSION['uid'],1001);
				refresh_jobs($id,intval($_SESSION['uid']))?exit("ok"):exit("err");
			}
		}
	}
}
// 暂停 职位 
elseif($act=="jobs_pause")
{
	$smarty->cache = false;
	$id=intval($_POST['id']);
	$uid=intval($_SESSION["uid"]);
	activate_jobs($id,2,$_SESSION['uid']);
	exit("ok");
}
// 暂停职位 恢复
elseif($act=="jobs_regain")
{
	$smarty->cache = false;
	$id=intval($_POST['id']);
	$uid=intval($_SESSION["uid"]);
	$jobs_num= $db->get_total("select count(*) num from ".table("jobs")." where uid=$_SESSION[uid] and audit=1 and display=1 ");
	$jobs_tmp_num= $db->get_total("select count(*) num from ".table("jobs_tmp")." where uid=$_SESSION[uid] and audit<>3 and display=1 ");
	$com_jobs_num=$jobs_num+$jobs_tmp_num;
	$setmeal= get_user_setmeal($_SESSION['uid']);
	if ($com_jobs_num>=$setmeal['jobs_ordinary'])
	{
		exit("表示された職位は設定制限を超えました，サービスコースをアップグレードして，又は募集しない職位を閉じる！");
	}else
	{
		activate_jobs($id,1,$_SESSION['uid']);
		exit("ok");
	}
}
// 删除职位 
elseif($act=="jobs_del")
{
	$smarty->cache = false;
	$id=intval($_POST['id']);
	del_jobs($id,intval($_SESSION['uid']))?exit("ok"):exit("err");
}
?>
