<?php
 /*
 * 74cms WAP
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/fun_wap.php');
require_once(QISHI_ROOT_PATH.'include/fun_company.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
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
	$wheresql=" WHERE a.company_uid='{$_SESSION['uid']}' ";
	$perpage = 5;
	$count  = 0;
	$page = empty($_GET['page'])?1:intval($_GET['page']);
	if($page<1) $page = 1;
	$start = ($page-1)*$perpage;
	$total_sql="SELECT COUNT(*) AS num FROM  ".table('company_down_resume')." as a  {$wheresql}";
	$count=$db->get_total($total_sql);
	$limit=" LIMIT {$start},{$perpage}";
	$sql="select a.*,r.title,r.fullname,r.display_name,r.education_cn,r.birthdate,r.experience_cn,r.residence from ".table("company_down_resume")." as a left join ".table("resume")." as r on a.resume_id=r.id  $wheresql order by a.down_addtime desc ".$limit;
	$row=$db->getall($sql);
	foreach ($row as $key=>$value) {
		$value["birthdate_"]=date('Y',time())-$value["birthdate"];
		/*if ($value['display_name']=="2")
		{
		$value['fullname']="N".str_pad($value['resume_id'],7,"0",STR_PAD_LEFT);
		}
		elseif ($value['display_name']=="3")
		{
		$value['fullname']=cut_str($value['fullname'],1,0,"**");
		}*/
		$row[$key]=$value;
	}
	$smarty->assign('row',$row);
	$smarty->display("wap/company/wap-download-resumes.html");	
}
elseif($act=="ajax_download_resume")
{
	$favoriteshtml="";
	$rows=intval($_GET['rows']);
	$offset=intval($_GET['offset']);
	$wheresql=" WHERE a.company_uid='{$_SESSION['uid']}' ";
	$row=$db->getall("select a.*,r.title,r.fullname,r.display_name,r.education_cn,r.birthdate,r.experience_cn,r.residence from ".table("company_down_resume")." as a left join ".table("resume")." as r on a.resume_id=r.id  $wheresql order by a.down_addtime desc limit $offset,$rows");
	if (!empty($row) && $offset<=100)
	{
		foreach($row as $list)
		{
			$list["birthdate_"]=date('Y',time())-$list["birthdate"];
			/*if ($list['display_name']=="2")
			{
			$list['fullname']="N".str_pad($list['resume_id'],7,"0",STR_PAD_LEFT);
			}
			elseif ($list['display_name']=="3")
			{
			$list['fullname']=cut_str($list['fullname'],1,0,"**");
			}*/
			$favoriteshtml.='<div class="get_resume_box" onclick="window.location.href="../wap-resume-show.php?id='.$favoriteshtml.="<div class='get_resume_box' onclick=window.location.href='../wap-resume-show.php?id={$list["resume_id"]}'
><div class='get_resume_left'><div class='name_box'><div class='name_box_l'>".$list["fullname"].'</div><div class="name_box_r">'.date("Y-m-d",$list["apply_addtime"]).'</div><div class="clear"></div></div><div class="person_detail">'.$list["education_cn"].'|'.$list["birthdate_"].'|'.$list["experience_cn"].'|'.$list["residence_cn"].'</div></div><div class="get_resume_right"><img src="../images/34.gif" alt="" /></div><div class="clear"></div></div>';
		}
		exit($favoriteshtml);
	}
	else
	{
		exit('-1');
	}
}
// 下载简历联系方式
elseif($act=="ajax_download_resume_add")
{
	$resume_id=intval($_POST["resume_id"]);
	$resume = resume_one($resume_id);
	$company_info=get_company($_SESSION['uid']);
	if($_SESSION["utype"]!=1){
		exit("企业会员请登录后查看联系方式");
	}
	else
	{
		$setmeal=get_user_setmeal($_SESSION['uid']);
		if ($_CFG['operation_mode']=="3")
		{
			if ($_CFG['setmeal_to_points']=="1")
			{
				if (empty($setmeal) || ($setmeal['endtime']<time() && $setmeal['endtime']<>"0"))
				{
				$_CFG['operation_mode']="1";
				}
				elseif ($resume['talent_']=='2' && $setmeal['download_resume_senior']<=0)
				{
				$_CFG['operation_mode']="1";
				}
				elseif ($resume['talent_']=='1' && $setmeal['download_resume_ordinary']<=0)
				{
				$_CFG['operation_mode']="1";
				}
				else
				{
				$_CFG['operation_mode']="2";
				}
			}
			else
			{
			$_CFG['operation_mode']="2";
			}
		}
		if ($_CFG['operation_mode']=="2")
		{
			if (empty($setmeal) || ($setmeal['endtime']<time() && $setmeal['endtime']<>"0"))
			{
				exit('您的服务已到期,您可以登录网页版申请服务！');
			}
			elseif ($resume['talent_']=='2' && $setmeal['download_resume_senior']<=0)
			{
				exit('您下载高级人才数量已经超出限制,您可以登录网页版申请服务！');
			}
			elseif ($resume['talent_']=='1' && $setmeal['download_resume_ordinary']<=0)
			{
				exit('你下载简历数量已经超出了限制,您可以登录网页版申请服务！');
			}
			else
			{
				exit("ok");
			}
		}
		elseif($_CFG['operation_mode']=="1")
		{
			$points_rule=get_cache('points_rule');
			$points=$resume['talent_']=='2'?$points_rule['resume_download_advanced']['value']:$points_rule['resume_download']['value'];
			$mypoints=get_user_points($_SESSION['uid']);
			if  ($mypoints<$points)
			{
				if (!empty($setmeal) && $_CFG['setmeal_to_points']=="1")
				{
					exit('你的服务已到期或超出服务条数。您可以登录网页版申请服务');
				}
				else
				{
					exit('你的'.$_CFG['points_byname'].' 不足，请充值后下载');
				}			
			}else{
				exit("ok");
			}
		}
	}
}
elseif($act=="ajax_download_resume_add_save")
{
	$resume_id=intval($_POST["resume_id"]);
	$resume = resume_one($resume_id);
	$resume = array_map("addslashes",$resume);
	$setmeal=get_user_setmeal($_SESSION['uid']);
	if ($_CFG['operation_mode']=="3")
	{
		if ($_CFG['setmeal_to_points']=="1")
		{
			if (empty($setmeal) || ($setmeal['endtime']<time() && $setmeal['endtime']<>"0"))
			{
			$_CFG['operation_mode']="1";
			}
			elseif ($resume['talent_']=='2' && $setmeal['download_resume_senior']<=0)
			{
			$_CFG['operation_mode']="1";
			}
			elseif ($resume['talent_']=='1' && $setmeal['download_resume_ordinary']<=0)
			{
			$_CFG['operation_mode']="1";
			}
			else
			{
			$_CFG['operation_mode']="2";
			}
		}
		else
		{
		$_CFG['operation_mode']="2";
		}
	}
	if ($_CFG['operation_mode']=="2")
	{	
			if ($resume_one['talent_']=='2')
			{
					if ($setmeal['download_resume_senior']>0 && add_down_resume($resume_id,$_SESSION['uid'],$resume["uid"],$resume['title']))
					{
					action_user_setmeal($_SESSION['uid'],"download_resume_senior");
					$setmeal=get_user_setmeal($_SESSION['uid']);
					write_memberslog($_SESSION['uid'],1,9002,$_SESSION['username'],"下载了 {$ruser['username']} 发布的高级简历,还可以下载 {$setmeal['download_resume_senior']} 份高级简历",2,1005,"下载高级简历","1","{$setmeal['download_resume_senior']}");
					write_memberslog($_SESSION['uid'],1,4001,$_SESSION['username'],"下载了 {$ruser['username']} 发布的简历");
					//站内信
					if($pms_notice=='1'){
						$company=$db->getone("select id,companyname  from ".table('company_profile')." where uid ={$_SESSION['uid']} limit 1");
						// $user=$db->getone("select username from ".table('members')." where uid ={$resume['uid']} limit 1");
						$resume_url=url_rewrite('QS_resume',array('id'=>$id));
						$company_url=url_rewrite('QS_companyshow',array('id'=>$company['id']));
						$message=$_SESSION['username']."下载了您发布的简历：<a href=\"{$resume_url}\" target=\"_blank\">{$resume['title']}</a>，<a href=\"$company_url\" target=\"_blank\">点击查看公司详情</a>";
						write_pmsnotice($resume['uid'],$ruser['username'],$message);
					}
					exit("ok");
					}
			}
			else
			{		
					if ($setmeal['download_resume_ordinary']>0 && add_down_resume($resume_id,$_SESSION['uid'],$resume['uid'],$resume['title']))
					{
					action_user_setmeal($_SESSION['uid'],"download_resume_ordinary");
					$setmeal=get_user_setmeal($_SESSION['uid']);
					write_memberslog($_SESSION['uid'],1,9002,$_SESSION['username'],"下载了 {$ruser['username']} 发布的普通简历,还可以下载 {$setmeal['download_resume_ordinary']} 份普通简历",2,1004,"下载普通简历","1","{$setmeal['download_resume_ordinary']}");
					write_memberslog($_SESSION['uid'],1,4001,$_SESSION['username'],"下载了 {$ruser['username']} 发布的简历");
					//站内信
					if($pms_notice=='1'){
						$company=$db->getone("select id,companyname  from ".table('company_profile')." where uid ={$_SESSION['uid']} limit 1");
						// $user=$db->getone("select username from ".table('members')." where uid ={$resume['uid']} limit 1");
						$resume_url=url_rewrite('QS_resume',array('id'=>$id));
						$company_url=url_rewrite('QS_companyshow',array('id'=>$company['id']));
						$message=$_SESSION['username']."下载了您发布的简历：<a href=\"{$resume_url}\" target=\"_blank\">{$resume['title']}</a>，<a href=\"$company_url\" target=\"_blank\">点击查看公司详情</a>";
						write_pmsnotice($resume['uid'],$ruser['username'],$message);
					}
					exit("ok");
					}
			}

	}
	elseif($_CFG['operation_mode']=="1")
	{			
				$points_rule=get_cache('points_rule');
				$points=$resume['talent_']=='2'?$points_rule['resume_download_advanced']['value']:$points_rule['resume_download']['value'];
				$ptype=$resume['talent_']=='2'?$points_rule['resume_download_advanced']['type']:$points_rule['resume_download']['type'];
				$mypoints=get_user_points($_SESSION['uid']);
				if  ($mypoints<$points)
				{
					exit("您的积分不足");
				}
				if (add_down_resume($resume_id,$_SESSION['uid'],$resume['uid'],$resume['title']))
				{
					if ($points>0)
					{
					report_deal($_SESSION['uid'],$ptype,$points);
					$user_points=get_user_points($_SESSION['uid']);
					$operator=$ptype=="1"?"+":"-";
					if($resume['talent_']=='2'){
						write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"下载了 {$ruser['username']} 发布的简历({$operator}{$points}),(剩余:{$user_points})",1,1005,"下载高级简历","{$operator}{$points}","{$user_points}");
					}elseif($resume['talent_']=='1'){
						write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"下载了 {$ruser['username']} 发布的简历({$operator}{$points}),(剩余:{$user_points})",1,1004,"下载普通简历","{$operator}{$points}","{$user_points}");
					}
					write_memberslog($_SESSION['uid'],1,4001,$_SESSION['username'],"下载了 {$ruser['username']} 发布的简历");
					//站内信
					if($pms_notice=='1'){
						$company=$db->getone("select id,companyname  from ".table('company_profile')." where uid ={$_SESSION['uid']} limit 1");
						// $user=$db->getone("select username from ".table('members')." where uid ={$resume['uid']} limit 1");
						$resume_url=url_rewrite('QS_resume',array('id'=>$id));
						$company_url=url_rewrite('QS_companyshow',array('id'=>$company['id']));
						$message=$_SESSION['username']."下载了您发布的简历：<a href=\"{$resume_url}\" target=\"_blank\">{$resume['title']}</a>，<a href=\"$company_url\" target=\"_blank\">点击查看公司详情</a>";
						write_pmsnotice($resume['uid'],$ruser['username'],$message);
					}
					}
					exit("ok");
				}
	}
}
?>