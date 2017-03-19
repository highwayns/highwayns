<?php
function tpl_function_qishi_resume_show($params, &$smarty)
{
global $db,$_CFG,$QS_cookiepath,$QS_cookiedomain;
$arr=explode(',',$params['set']);
foreach($arr as $str)
{
$a=explode(':',$str);
	switch ($a[0])
	{
	case "简历ID":
		$aset['id'] = $a[1];
		break;
	case "列表名":
		$aset['listname'] = $a[1];
		break;
	}
}
$aset=array_map("get_smarty_request",$aset);
$aset['id']=$aset['id']?intval($aset['id']):0;
$aset['listname']=$aset['listname']?$aset['listname']:"list";
$wheresql=" WHERE  id=".$aset['id']."";
$val=$db->getone("select id,uid,display,display_name,fullname,sex,sex_cn,major_cn,birthdate,photo,photo_img,photo_display,tag_cn,refreshtime,height,marriage_cn,education_cn,experience_cn,householdaddress,residence,talent,wage_cn,nature_cn,district_cn,trade_cn,intention_jobs,current_cn,specialty,title,telephone,email,addtime,resume_from_pc from ".table('resume').$wheresql." LIMIT  1");
if(intval($_SESSION['utype'])==1){
	$company_profile = $db->getone("select companyname from ".table('company_profile')." where uid=".intval($_SESSION['uid']));
}
if ($val)
{
	if(intval($_SESSION['uid'])>0 && intval($_SESSION['utype'])==1)
	{
		// 简历处理率
		$resume_applyed = $db->getone("select count(*) num from ".table("personal_jobs_apply")." where  company_uid=".intval($_SESSION['uid'])." and resume_id=$val[id] ");
		if(!empty($resume_applyed))
		{
			$apply_see=$db->getone("select count(*) num from ".table("personal_jobs_apply")." where  company_uid=".intval($_SESSION['uid'])." and  personal_look=2 ");
			$apply_all=$db->getone("select count(*) num from ".table("personal_jobs_apply")." where  company_uid=".intval($_SESSION['uid'])." ");
			$company_info['resume_processing']=$apply_see['num']/$apply_all['num']*100;
			$db->updatetable(table("company_profile"),$company_info,array("uid"=>$_SESSION['uid']));
		}

		//查看是否已经下载过简历
		$download = $db->getone("select did from ".table("company_down_resume")." where resume_id=$val[id] and company_uid=".intval($_SESSION['uid'])." ");
		if(empty($download)){
			if ($val['display_name']=="2")
			{
				$val['fullname']="N".str_pad($val['id'],7,"0",STR_PAD_LEFT);
				$val['fullname_']=$val['fullname'];		
			}
			elseif($val['display_name']=="3")
			{
				if($val['sex']==1){
				$val['fullname']=cut_str($val['fullname'],1,0,"先生");
				}elseif($val['sex'] == 2){
				$val['fullname']=cut_str($val['fullname'],1,0,"女士");
				}
			}
			else
			{
				$val['fullname_']=$val['fullname'];
				$val['fullname']=$val['fullname'];
			}
		}
		//提示信息
		$mes_apply = $db->getone("select jobs_name,apply_addtime from ".table('personal_jobs_apply')." where `resume_id`=".$val['id']." and  `company_uid`=".intval($_SESSION['uid'])." limit 1 ");
		if($mes_apply)
		{
			$val['message'] = "应聘职位：".$mes_apply['jobs_name']." 投递时间：".date('Y-m-d',$mes_apply['apply_addtime']);
		}
		else
		{
			$val['message'] = "";
		}
	}
	else
	{
		if ($val['display_name']=="2")
		{
			$val['fullname']="N".str_pad($val['id'],7,"0",STR_PAD_LEFT);
			$val['fullname_']=$val['fullname'];		
		}
		elseif($val['display_name']=="3")
		{
			if($val['sex']==1){
			$val['fullname']=cut_str($val['fullname'],1,0,"先生");
			}elseif($val['sex'] == 2){
			$val['fullname']=cut_str($val['fullname'],1,0,"女士");
			}
		}
		else
		{
			$val['fullname_']=$val['fullname'];
			$val['fullname']=$val['fullname'];
		}
	}
	$val['education_list']=get_this_education($val['uid'],$val['id']);
	$val['work_list']=get_this_work($val['uid'],$val['id']);
	$val['training_list']=get_this_training($val['uid'],$val['id']);
	$val['language_list']=get_this_language($val['uid'],$val['id']);
	$val['credent_list']=get_this_credent($val['uid'],$val['id']);
	$val['img_list']=get_this_img($val['uid'],$val['id']);
	$val['age']=date("Y")-$val['birthdate'];

	if ($val['photo']=="1")
	{
		$download = $db->getone("select did from ".table("company_down_resume")." where resume_id=$val[id] and company_uid=".intval($_SESSION['uid'])." ");
		if(empty($download))
		{
			if($val['photo_display']=="1")
			{	
				$val['photosrc']=$_CFG['resume_photo_dir'].$val['photo_img'];
			}
			else
			{
				$val['photosrc']=$_CFG['resume_photo_dir_thumb']."no_photo_display.gif";
			}
		}
		else
		{
			$val['photosrc']=$_CFG['resume_photo_dir'].$val['photo_img'];
		}
	}
	else
	{
	$val['photosrc']=$_CFG['resume_photo_dir_thumb']."no_photo.gif";
	}

	if ($val['tag_cn'])
	{
		$tag_cn=explode(',',$val['tag_cn']);
		$val['tag_cn']=$tag_cn;
	}
	else
	{
	$val['tag_cn']=array();
	}
	$apply = $db->getone("select * from ".table('personal_jobs_apply')." where `resume_id`=".$val['id']);
	$val['jobs_name'] = $apply['jobs_name'];
	$val['apply_addtime'] = $apply['apply_addtime'];
	$val['jobs_url'] = url_rewrite('QS_jobsshow',array('id'=>$apply['jobs_id']));
	if($val['jobs_name']){
		$val['apply'] = 1;
	}else{
		$val['apply'] = 0;
	}
	/* 简历活跃度  更新时间 主动申请职位数  浏览职位数 */
	$vitality=0;
	$val['refreshtime_cn']=daterange(time(),$val['refreshtime'],'Y-m-d',"#FF3300");
	$timestr=time()-$val['refreshtime'];
	$day= intval($timestr/86400);
	if($day<3)
	{
		$vitality+=2;
	}
	else
	{
		$vitality+=1;
	}
	$time=time()-15*86400;
	$val['apply_jobs']=$db->get_total("select count(*) num from ".table("personal_jobs_apply")." where resume_id=$val[id] and apply_addtime>$time ");
	if($val['apply_jobs']>0 && $val['apply_jobs']<10)
	{
		$vitality+=1;
	}
	elseif($val['apply_jobs']>=10)
	{
		$vitality+=2;
	}
	$val['vitality']=$vitality;
	/*企业关注度 start */
	$attention=0;
	$val['com_down']=$db->get_total("select count(*) num from ".table("company_down_resume")." where resume_id=$val[id] and down_addtime>$time ");
	
	if($val['com_down']>=0 && $val['com_down']<10)
	{
		$attention+=1;
	}
	elseif($val['com_down']>=10)
	{
		$attention+=2;
	}
	$val['com_invite']=$db->get_total("select count(*) num from ".table("company_interview")." where resume_id=$val[id] and interview_addtime>$time ");
	if($val['com_invite']>0 && $val['com_invite']<10)
	{
		$attention+=1;
	}
	elseif($val['com_invite']>=10)
	{
		$attention+=2;
	}
	
	$val['attention']=$attention;
	/*企业关注度 end */
	//判断手机、微信、邮箱是否验证
	$is_audit_phone = $db->getone("SELECT mobile_audit,email_audit,weixin_openid FROM ".table('members')." WHERE uid={$val['uid']}  LIMIT 1 ");
	$val['is_audit_mobile'] = $is_audit_phone['mobile_audit'];
	$val['is_audit_email'] = $is_audit_phone['email_audit'];
	$val['is_audit_weixin'] = $is_audit_phone['weixin_openid'];
	//个人自己预览
	if($_SESSION['utype'] == '2' && $_SESSION['uid'] == $val['uid'] ){
		$val['isminesee'] = '1';
	}
}
else
{
	header("HTTP/1.1 404 Not Found"); 
	$smarty->display("404.htm");
	exit();
}
$smarty->assign($aset['listname'],$val);
}
function get_this_education($uid,$pid)
{
	global $db;
	$sql = "SELECT startyear,startmonth,endyear,endmonth,school,speciality,education_cn,todate FROM ".table('resume_education')." WHERE uid='".intval($uid)."' AND pid='".intval($pid)."' ";
	return $db->getall($sql);
}
function get_this_work($uid,$pid)
{
	global $db;
	$sql = "select startyear,startmonth,endyear,endmonth,jobs,companyname,achievements,todate from ".table('resume_work')." where uid=".intval($uid)." AND pid='".$pid."' " ;
	return $db->getall($sql);
}
function get_this_training($uid,$pid)
{
	global $db;
	$sql = "select startyear,startmonth,endyear,endmonth,agency,course,description,todate from ".table('resume_training')." where uid='".intval($uid)."' AND pid='".intval($pid)."'";
	return $db->getall($sql);
}
function get_this_language($uid,$pid)
{
	global $db;
	$sql = "select language_cn,level_cn from ".table('resume_language')." where uid='".intval($uid)."' AND pid='".intval($pid)."'";
	return $db->getall($sql);
}
function get_this_credent($uid,$pid)
{
	global $db;
	$sql = "select year,month,name,images from ".table('resume_credent')." where uid='".intval($uid)."' AND pid='".intval($pid)."'";
	return $db->getall($sql);
}
function get_this_img($uid,$pid)
{
	global $db;
	$sql = "select img from ".table('resume_img')." where uid='".intval($uid)."' AND resume_id='".intval($pid)."'";
	return $db->getall($sql);
}
?>