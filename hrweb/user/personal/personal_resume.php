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
$smarty->assign('leftmenu',"resume"); 
 
//简历列表
if ($act=='resume_list')
{
	$wheresql=" WHERE uid='".$_SESSION['uid']."' ";
	$sql="SELECT * FROM ".table('resume').$wheresql;
	$smarty->assign('title','我的简历 - 个人会员中心 - '.$_CFG['site_name']);
	$smarty->assign('act',$act);
	$total=$db->get_total("SELECT COUNT(*) AS num FROM ".table('resume')." WHERE uid='{$_SESSION['uid']}'");
	$smarty->assign('total',$total);
	$smarty->assign('resume_list',get_resume_list($sql,12,true,true,true));
	$smarty->display('member_personal/personal_resume_list.htm');
}
elseif ($act=='refresh')
{
		$resumeid = intval($_GET['id'])?intval($_GET['id']):showmsg("您没有选择简历！");
		$refrestime=get_last_refresh_date($_SESSION['uid'],"2001");
		$duringtime=time()-$refrestime['max(addtime)'];
		$space = $_CFG['per_refresh_resume_space']*60;
		$refresh_time = get_today_refresh_times($_SESSION['uid'],"2001");
		if($_CFG['per_refresh_resume_time']!=0&&($refresh_time['count(*)']>=$_CFG['per_refresh_resume_time']))
		{
		showmsg("每天最多只能刷新".$_CFG['per_refresh_resume_time']."次,您今天已超过最大刷新次数限制！",2);	
		}
		elseif($duringtime<=$space){
		showmsg($_CFG['per_refresh_resume_space']."分钟内不能重复刷新简历！",2);
		}
		else 
		{
		refresh_resume($resumeid,$_SESSION['uid'])?showmsg('操作成功！',2):showmsg('操作失败！',0);
		}
}
//删除简历
elseif ($act=='del_resume')
{
	if (intval($_GET['id'])==0)
	{
	exit('您没有选择简历！');
	}
	else
	{
	del_resume($_SESSION['uid'],intval($_GET['id']))?exit('success'):exit('fail');
	}
}
//创建简历-基本信息
elseif ($act=='make1')
{
	$uid=intval($_SESSION['uid']);
	$pid=intval($_REQUEST['pid']);
	/**
	 * 3.6优化start
	 * @var [type]
	 */
	$total=$db->get_total("SELECT COUNT(*) AS num FROM ".table('resume')." WHERE uid='{$uid}'");
	if ($total>=intval($_CFG['resume_max']))
	{
	showmsg("您最多可以创建{$_CFG['resume_max']} 份简历,已经超出了最大限制！",1);
	}
	/**
	 * 3.6优化end
	 * @var [type]
	 */
	$_SESSION['send_mobile_key']=mt_rand(100000, 999999);
	$smarty->assign('send_key',$_SESSION['send_mobile_key']);
	$smarty->assign('resume_basic',get_resume_basic($uid,$pid));
	$smarty->assign('resume_education',get_resume_education($uid,$pid));
	$smarty->assign('resume_work',get_resume_work($uid,$pid));
	$smarty->assign('resume_training',get_resume_training($uid,$pid));
	$smarty->assign('act',$act);
	$smarty->assign('pid',$pid);
	$smarty->assign('user',$user);
	$smarty->assign('userprofile',get_userprofile($_SESSION['uid']));
	$smarty->assign('title','我的简历 - 个人会员中心 - '.$_CFG['site_name']);
	$captcha=get_cache('captcha');
	$smarty->assign('verify_resume',$captcha['verify_resume']);
	$smarty->assign('go_resume_show',$_GET['go_resume_show']);
	$smarty->display('member_personal/personal_make_resume_step1.htm');
}
//创建简历 -保存基本信息、求职意向
elseif ($act=='make1_save')
{
	$captcha=get_cache('captcha');
	$postcaptcha = trim($_POST['postcaptcha']);
	if($captcha['verify_resume']=='1' && empty($postcaptcha) && intval($_REQUEST['pid'])===0)
	{
		showmsg("请填写系统验证码",1);
 	}
	if ($captcha['verify_resume']=='1' && intval($_REQUEST['pid'])===0 &&  strcasecmp($_SESSION['imageCaptcha_content'],$postcaptcha)!=0)
	{
		showmsg("系统验证码错误",1);
	}
	$setsqlarr['uid']=intval($_SESSION['uid']);
	$setsqlarr['telephone']=trim($_POST['mobile'])?trim($_POST['mobile']):showmsg('请填写手机号！',1);
	if($user['mobile_audit']!="1")
	{
		$members['mobile']=$telephone;
		$members_info['phone']=$telephone;
		$resume['telephone']=$telephone;
		$db->updatetable(table("members"),$members,array("uid"=>intval($_SESSION['uid'])));
		$db->updatetable(table("members_info"),$members_info,array("uid"=>intval($_SESSION['uid'])));
		$db->updatetable(table("resume"),$resume,array("uid"=>intval($_SESSION['uid'])));
		unset($members['mobile'],$members_info['phone'],$resume['telephone']);
	}
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):"未命名简历";
	check_word($_CFG['filter'],$_POST['title'])?showmsg($_CFG['filter_tips'],0):'';
	$setsqlarr['fullname']=trim($_POST['fullname'])?trim($_POST['fullname']):showmsg('请填写姓名！',1);
	check_word($_CFG['filter'],$_POST['fullname'])?showmsg($_CFG['filter_tips'],0):'';
	$setsqlarr['display_name']=intval($_POST['display_name']);
	$setsqlarr['sex']=trim($_POST['sex'])?intval($_POST['sex']):showmsg('请选择性别！',1);
	$setsqlarr['sex_cn']=trim($_POST['sex_cn']);
	$setsqlarr['birthdate']=intval($_POST['birthdate'])>1945?intval($_POST['birthdate']):showmsg('请正确填写出生年份',1);
	$setsqlarr['residence']=trim($_POST['residence'])?trim($_POST['residence']):showmsg('请填写现居住地！',1);
	$setsqlarr['education']=intval($_POST['education'])?intval($_POST['education']):showmsg('请选择学历',1);
	$setsqlarr['education_cn']=trim($_POST['education_cn']);
	$setsqlarr['major']=intval($_POST['major'])?intval($_POST['major']):showmsg('请选择专业',1);
	$setsqlarr['major_cn']=trim($_POST['major_cn']);
	$setsqlarr['experience']=$_POST['experience']?$_POST['experience']:showmsg('请选择工作经验',1);
	$setsqlarr['experience_cn']=trim($_POST['experience_cn']);
	$setsqlarr['email']=trim($_POST['email'])?trim($_POST['email']):showmsg('请填写邮箱！',1);
	if($user['email_audit']!="1")
	{
		$members['email']=$setsqlarr['email'];
		$members_info['email']=$setsqlarr['email'];
		$resume['email']=$setsqlarr['email'];
		$db->updatetable(table("members"),$members,array("uid"=>intval($_SESSION['uid'])));
		$db->updatetable(table("members_info"),$members_info,array("uid"=>intval($_SESSION['uid'])));
		$db->updatetable(table("resume"),$resume,array("uid"=>intval($_SESSION['uid'])));
		unset($members['email'],$members_info['email'],$resume['email']);
	}
	check_word($_CFG['filter'],$_POST['email'])?showmsg($_CFG['filter_tips'],0):'';
	$setsqlarr['email_notify']=$_POST['email_notify']=="1"?1:0;
	$setsqlarr['height']=intval($_POST['height']);
	$setsqlarr['householdaddress']=trim($_POST['householdaddress']);
	$setsqlarr['marriage']=intval($_POST['marriage']);
	$setsqlarr['marriage_cn']=trim($_POST['marriage_cn']);;
	$setsqlarr['intention_jobs']=trim($_POST['intention_jobs'])?trim($_POST['intention_jobs']):showmsg('请选择意向职位！',1);
	$setsqlarr['trade']=$_POST['trade']?trim($_POST['trade']):showmsg('请选择期望行业！',1);
	$setsqlarr['trade_cn']=trim($_POST['trade_cn']);
	$setsqlarr['district_cn']=$_POST['district_cn']?trim($_POST['district_cn']):showmsg('请选择期望工作地区！',1);
	$setsqlarr['nature']=intval($_POST['nature'])?intval($_POST['nature']):showmsg('请选择期望岗位性质！',1);
	$setsqlarr['nature_cn']=trim($_POST['nature_cn']);
	//目前状态
	$setsqlarr['current']=intval($_POST['current'])?intval($_POST['current']):showmsg('请选择目前状态！',1);
	$setsqlarr['current_cn']=trim($_POST['current_cn']);
	$setsqlarr['wage']=intval($_POST['wage'])?intval($_POST['wage']):showmsg('请选择期望薪资！',1);
	$setsqlarr['wage_cn']=trim($_POST['wage_cn']);
	$setsqlarr['refreshtime']=$timestamp;
	$setsqlarr['audit']=intval($_CFG['audit_resume']);
	$setsqlarr['resume_from_pc']=1;
	$total=$db->get_total("SELECT COUNT(*) AS num FROM ".table('resume')." WHERE uid='{$_SESSION['uid']}'");
	if ($total>=intval($_CFG['resume_max']))
	{
	showmsg("您最多可以创建{$_CFG['resume_max']} 份简历,已经超出了最大限制！",1);
	}
	else
	{
	$setsqlarr['addtime']=$timestamp;
	$pid=$db->inserttable(table('resume'),$setsqlarr,1);
	$searchtab['id'] = $pid;
	$searchtab['uid'] = $_SESSION['uid'];
	$db->inserttable(table('resume_search_key'),$searchtab);
	$db->inserttable(table('resume_search_rtime'),$searchtab);
	if (empty($pid))showmsg("保存失败！",0);
	add_resume_jobs($pid,$_SESSION['uid'],$_POST['intention_jobs_id'])?"":showmsg('保存失败！',0);
	add_resume_district($pid,$_SESSION['uid'],$_POST['district'])?"":showmsg('保存失败！',0);
	add_resume_trade($pid,$_SESSION['uid'],$_POST['trade'])?"":showmsg('保存失败！',0);
	check_resume($_SESSION['uid'],$pid);
	write_memberslog($_SESSION['uid'],2,1101,$_SESSION['username'],"创建了简历");
	
	if(!get_userprofile($_SESSION['uid'])){
		$infoarr['realname']=$setsqlarr['fullname'];
		$infoarr['sex']=$setsqlarr['sex'];
		$infoarr['sex_cn']=$setsqlarr['sex_cn'];
		$infoarr['birthday']=$setsqlarr['birthdate'];
		$infoarr['residence']=$setsqlarr['residence'];
		$infoarr['education']=$setsqlarr['education'];
		$infoarr['education_cn']=$setsqlarr['education_cn'];
		$infoarr['experience']=$setsqlarr['experience'];
		$infoarr['experience_cn']=$setsqlarr['experience_cn'];
		$infoarr['height']=$setsqlarr['height'];
		$infoarr['householdaddress']=$setsqlarr['householdaddress'];
		$infoarr['marriage']=$setsqlarr['marriage'];
		$infoarr['marriage_cn']=$setsqlarr['marriage_cn'];
		$infoarr['phone']=$setsqlarr['telephone'];
		$infoarr['email']=$setsqlarr['email'];
		$infoarr['uid']=intval($_SESSION['uid']);
		$db->inserttable(table('members_info'),$infoarr);
	}
	header("Location: ?act=edit_resume&pid=".$pid."&make=1");
	}	
}
elseif($act=='make1_succeed'){
	$pid = intval($_GET['pid']);
	$smarty->assign('pid',$pid);
	$smarty->assign('resume_basic',get_resume_basic($_SESSION['uid'],$pid));
	$smarty->assign('title','我的简历 - 个人会员中心 - '.$_CFG['site_name']);
	$smarty->display('member_personal/personal_make_resume_step1_succeed.htm');
}
elseif($act=='ajax_get_interest_jobs'){
	global $_CFG;
	$uid=intval($_SESSION['uid']);
	$pid=intval($_GET['pid']);
	$html = "";
	$interest_id = get_interest_jobs_id_by_resume($uid,$pid);
	$jobs_list = get_interest_jobs_list($interest_id);
	if(!empty($jobs_list)){
		foreach($jobs_list as $k=>$v){
			$jobs_url = url_rewrite("QS_jobsshow",array("id"=>$v['id']));
			$company_url = url_rewrite("QS_companyshow",array("id"=>$v['company_id']));
			$html='<tr>
					<td class="frist" width="117"><div class="index-line1"><a href="'.$jobs_url.'" class="underline job-link">'.$v["jobs_name"].'</a></div></td>
					<td width="228"><div class="index-line2"><a href="'.$company_url.'" class="underline com-link">'.$v["companyname"].'</a></div></td>
					<td width="139"><div class="index-line3">'.$v["district_cn"].'</div></td>
					<td width="195"><span>'.$v["wage_cn"].'</span></td>
					<td width="75">2015-06-01</td>
				</tr>';
		}
	}
	exit($html);
}
elseif($act == 'ajax_save_basic_info')
{
	$telephone=trim($_POST['mobile'])?trim($_POST['mobile']):exit('请填写手机号！');
	$resume_basic=get_resume_basic($_SESSION['uid'],$_REQUEST['pid']);
	$setsqlarr['telephone']=$telephone;
	if($user['mobile_audit']!="1")
	{
		$members['mobile']=$telephone;
		$members_info['phone']=$telephone;
		$db->updatetable(table("members"),$members,array("uid"=>intval($_SESSION['uid'])));
		$db->updatetable(table("members_info"),$members_info,array("uid"=>intval($_SESSION['uid'])));
		unset($members['mobile'],$members_info['phone']);
	}
	$setsqlarr['title']=utf8_to_gbk(trim($_POST['title']));
	$setsqlarr['fullname']=trim($_POST['fullname'])?utf8_to_gbk(trim($_POST['fullname'])):exit('请填写姓名！');
	check_word($_CFG['filter'],$setsqlarr['fullname'])?exit($_CFG['filter_tips']):'';
	$setsqlarr['display_name']=intval($_POST['display_name']);
	$setsqlarr['sex']=trim($_POST['sex'])?intval($_POST['sex']):exit('请选择性别！');
	$setsqlarr['sex_cn']=utf8_to_gbk(trim($_POST['sex_cn']));
	$setsqlarr['birthdate']=intval($_POST['birthdate'])>1945?intval($_POST['birthdate']):exit('请正确填写出生年份');
	$setsqlarr['residence']=trim($_POST['residence'])?utf8_to_gbk(trim($_POST['residence'])):exit('请填写现居住地！');
	$setsqlarr['education']=intval($_POST['education'])?intval($_POST['education']):exit('请选择学历');
	$setsqlarr['education_cn']=utf8_to_gbk(trim($_POST['education_cn']));
	$setsqlarr['major']=intval($_POST['major'])?intval($_POST['major']):exit('请选择专业');
	$setsqlarr['major_cn']=utf8_to_gbk(trim($_POST['major_cn']));
	$setsqlarr['experience']=$_POST['experience']?$_POST['experience']:exit('请选择工作经验');
	$setsqlarr['experience_cn']=utf8_to_gbk(trim($_POST['experience_cn']));
	$setsqlarr['email']=trim($_POST['email'])?utf8_to_gbk(trim($_POST['email'])):exit('请填写邮箱！');
	if($user['email_audit']!="1")
	{
		$members['email']=$setsqlarr['email'];
		$members_info['email']=$setsqlarr['email'];
		$db->updatetable(table("members"),$members,array("uid"=>intval($_SESSION['uid'])));
		$db->updatetable(table("members_info"),$members_info,array("uid"=>intval($_SESSION['uid'])));
		unset($members['email'],$members_info['email']);


	}
	check_word($_CFG['filter'],$setsqlarr['email'])?exit($_CFG['filter_tips']):'';
	$setsqlarr['email_notify']=$_POST['email_notify']=="1"?1:0;
	$setsqlarr['height']=intval($_POST['height']);
	$setsqlarr['householdaddress']=utf8_to_gbk(trim($_POST['householdaddress']));
	$setsqlarr['marriage']=intval($_POST['marriage']);
	$setsqlarr['marriage_cn']=utf8_to_gbk(trim($_POST['marriage_cn']));
	$setsqlarr['refreshtime']=$timestamp;
	$_CFG['audit_edit_resume']!="-1"?$setsqlarr['audit']=intval($_CFG['audit_edit_resume']):"";

	$db->updatetable(table('resume'),$setsqlarr, array("id"=>intval($_REQUEST['pid']),"uid"=>$_SESSION['uid']));
	
	check_resume($_SESSION['uid'],intval($_REQUEST['pid']));
	$title = utf8_to_gbk(trim($_POST['title']));
	write_memberslog($_SESSION['uid'],2,1105,$_SESSION['username'],"修改了简历({$title})");
	exit("success");
}
elseif($act == 'ajax_save_titl')
{
	$setsqlarr['uid']=intval($_SESSION['uid']);
	$setsqlarr['title']=utf8_to_gbk(trim($_POST['title']))?utf8_to_gbk(trim($_POST['title'])):exit('请填写简历名称！');
	check_word($_CFG['filter'],$setsqlarr['title'])?exit($_CFG['filter_tips']):''; 
	$db->updatetable(table('resume'),$setsqlarr," id='".intval($_POST['pid'])."'  AND uid='{$setsqlarr['uid']}'"); 
	$title = $setsqlarr['title'];
	write_memberslog($_SESSION['uid'],2,1105,$_SESSION['username'],"修改了简历({$title})"); 
	exit('success'); 
} 
elseif ($act=='ajax_save_basic')
{
	$setsqlarr['uid']=intval($_SESSION['uid']);
	 
	$setsqlarr['intention_jobs']=utf8_to_gbk(trim($_POST['intention_jobs']))?utf8_to_gbk(trim($_POST['intention_jobs'])):exit('请选择意向职位！');
	$setsqlarr['trade']=$_POST['trade']?trim($_POST['trade']):exit('请选择期望行业！');
	$setsqlarr['trade_cn']=utf8_to_gbk(trim($_POST['trade_cn']));
	$setsqlarr['district_cn']=utf8_to_gbk(trim($_POST['district_cn']))?utf8_to_gbk(trim($_POST['district_cn'])):exit('请选择期望工作地区！');
	$setsqlarr['nature']=intval($_POST['nature'])?intval($_POST['nature']):exit('请选择期望岗位性质！');
	$setsqlarr['nature_cn']=utf8_to_gbk(trim($_POST['nature_cn']));
	//目前状态
	$setsqlarr['current']=intval($_POST['current'])?intval($_POST['current']):exit('请选择目前状态！');
	$setsqlarr['current_cn']=utf8_to_gbk(trim($_POST['current_cn']));
	$setsqlarr['wage']=intval($_POST['wage'])?intval($_POST['wage']):exit('请选择期望薪资！');
	$setsqlarr['wage_cn']=utf8_to_gbk(trim($_POST['wage_cn']));
	$setsqlarr['refreshtime']=$timestamp;
	$_CFG['audit_edit_resume']!="-1"?$setsqlarr['audit']=intval($_CFG['audit_edit_resume']):"";
	$db->updatetable(table('resume'),$setsqlarr," id='".intval($_REQUEST['pid'])."'  AND uid='{$setsqlarr['uid']}'");
	add_resume_jobs(intval($_REQUEST['pid']),$_SESSION['uid'],$_POST['intention_jobs_id'])?"":showmsg('保存失败！',0);
	add_resume_district(intval($_REQUEST['pid']),$_SESSION['uid'],$_POST['district'])?"":showmsg('保存失败！',0);
	add_resume_trade(intval($_REQUEST['pid']),$_SESSION['uid'],$_POST['trade'])?"":showmsg('保存失败！',0);
	check_resume($_SESSION['uid'],intval($_REQUEST['pid']));
	$title = utf8_to_gbk(trim($_POST['title']));
	write_memberslog($_SESSION['uid'],2,1105,$_SESSION['username'],"修改了简历({$title})");
	exit("success");
}
elseif ($act=='resume_logo_save')
{	
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['param1']));
	if($resume_basic['photo_img'])
	{
		@unlink("../../data/".$_CFG['_resume_photo_dir']."/".$resume_basic['photo_img']);
		@unlink("../../data/".$_CFG['_resume_photo_dir_thumb']."/".$resume_basic['photo_img']);
	}
	$updir = date('Y/m/d');
	$savePath = "../../data/".$_CFG['_resume_photo_dir']."/".$updir;  //图片存储路径
	$savePathThumb = "../../data/".$_CFG['_resume_photo_dir_thumb']."/".$updir;  //图片存储路径
	 make_dir($savePath);
	 make_dir($savePathThumb);
	$savePicName = time();//图片存储名称
	$file_src = $savePath.'/'.$savePicName."_src.jpg";
	$filename150 = $savePath.'/'.$savePicName.".jpg"; 
	$filename50 = $savePathThumb.'/'.$savePicName.".jpg"; 
	$src=base64_decode($_POST['pic']);
	$pic1=base64_decode($_POST['pic1']);   
	$pic2=base64_decode($_POST['pic2']);
	if($src) {
		file_put_contents($file_src,$src);
	}
	file_put_contents($filename150,$pic1);
	if($pic2)file_put_contents($filename50,$pic2);
	$rs['status'] = 1;
	$rs['picUrl'] = $updir.'/'.$savePicName.".jpg";
	$setarr['photo_img']=$rs['picUrl'];
	$setarr['photo_audit']=intval($_CFG['audit_resume_photo']);
	$setarr['photo']=1;
	$db->updatetable(table("resume"),$setarr,array("uid"=>intval($_SESSION['uid']),"id"=>$resume_basic['id']));
	check_resume($_SESSION['uid'],intval($resume_basic['id']));
	print json_encode($rs);

}
elseif($act=='save_education'){
	$id=intval($_POST['id']);
	$setsqlarr['uid'] = intval($_SESSION['uid']);
	$setsqlarr['pid'] = intval($_REQUEST['pid']);
	
	if ($setsqlarr['uid']==0 || $setsqlarr['pid']==0 ) exit('简历不存在！');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['pid']));
	if (empty($resume_basic)) exit("请先填写简历基本信息！");
	$resume_education=get_resume_education($_SESSION['uid'],$_REQUEST['pid']);
	if (count($resume_education)>=6) exit('教育经历不能超过6条！');
	$school = utf8_to_gbk(trim($_POST['school']));
	$speciality = utf8_to_gbk(trim($_POST['speciality']));
	$education_cn = utf8_to_gbk(trim($_POST['education_cn']));
	$setsqlarr['school'] = $school?$school:exit("请填写学校名称！");
	check_word($_CFG['filter'],$setsqlarr['school'])?exit($_CFG['filter_tips']):'';
	$setsqlarr['speciality'] = $speciality?$speciality:exit("请填写专业名称！");
	check_word($_CFG['filter'],$setsqlarr['speciality'])?exit($_CFG['filter_tips']):'';
	$setsqlarr['education'] = intval($_POST['education'])?intval($_POST['education']):exit("请选择获得学历！");
	$setsqlarr['education_cn'] = $education_cn?$education_cn:exit("请选择获得学历！");
	// 选择至今就不判断结束时间了
	if (intval($_POST['edu_todate']) == 1) {
		if(trim($_POST['edu_start_year'])==""||trim($_POST['edu_start_month'])==""){
			exit("请选择就读时间！");
		}
		if(intval(($_POST['edu_start_year']))>intval(date('Y'))){
			exit('就读开始时间不允许大于毕业时间');
		}
		if(intval($_POST['edu_start_year']) == intval(date('Y')) && intval(($_POST['edu_start_month']))>=intval(date('m'))){
			exit('就读开始月份不允许大于毕业时间');
		}
	} else {
		if(trim($_POST['edu_start_year'])==""||trim($_POST['edu_start_month'])==""||trim($_POST['edu_end_year'])==""||trim($_POST['edu_end_month'])==""){
			exit("请选择就读时间！");
		}
		if(intval(($_POST['edu_start_year']))>intval($_POST['edu_end_year'])){
			exit('就读开始时间不允许大于毕业时间');
		}
		if(intval($_POST['edu_start_year']) == intval($_POST['edu_end_year']) && intval(($_POST['edu_start_month']))>=intval($_POST['edu_end_month'])){
			exit('就读开始月份不允许大于毕业时间');
		}
	}
	$setsqlarr['startyear'] = intval($_POST['edu_start_year']);
	$setsqlarr['startmonth'] = intval($_POST['edu_start_month']);
	$setsqlarr['endyear'] = intval($_POST['edu_end_year']);
	$setsqlarr['endmonth'] = intval($_POST['edu_end_month']);
	$setsqlarr['todate'] = intval($_POST['edu_todate']); // 至今
	if($id){
		$db->updatetable(table("resume_education"),$setsqlarr,array("id"=>$id));
		exit("success");
	}else{
		$insert_id = $db->inserttable(table("resume_education"),$setsqlarr,1);
		if($insert_id){
			check_resume($_SESSION['uid'],intval($_REQUEST['pid']));
			exit("success");
		}else{
			exit("err");
		}
	}
	
}
elseif($act=='ajax_get_education_list'){
	$pid=intval($_GET['pid']);
	$uid=intval($_SESSION['uid']);
	$education_list = get_resume_education($uid,$pid);
	$html="";
	if($education_list){
		foreach ($education_list as $key => $value) {
			// 判断结束时间是否至今
			$datehtm = '';
			if($value["todate"] == 1) {
				$datehtm = '至今';
			} else {
				$datehtm = $value["endyear"].'年'.$value["endmonth"].'月';
			}
			// ===========================
			$html.='<div class="jl1">
				 	 <div class="l1">'.$value["startyear"].'年'.$value["startmonth"].'月-'.$datehtm.'</div>
					 <div class="l2">'.$value["school"].'</div>
					 <div class="l3">'.$value["speciality"].'</div>
					 <div class="l4">'.$value["education_cn"].'</div>
					 <div class="l5">
					 <a class="edit_education" todate="'.$value["todate"].'" href="javascript:void(0);" url="?act=edit_education&id='.$value["id"].'&pid='.$pid.'"></a>
					 <a class="del_education d" href="javascript:void(0);" pid="'.$pid.'" edu_id="'.$value["id"].'" ></a><div class="clear"></div>
					 </div>
					 <div class="clear"></div>
				</div>';
		}
	}else{
		$js='<script type="text/javascript">$("#add_education").hide();$(function(){$(".but130lan_add").hover(function(){$(this).addClass("hover")},function(){$(this).removeClass("hover");})})</script>';
		$html.='<div class="noinfo" id="education_empty_box">
		 	 <div class="txt">教育经历最能体现您的学历和专业能力，快来完成它吸引企业和HR青睐吧！</div>
			 <div class="addbut">
			 	<input type="button" name="" id="empty_add_education" value="添加经历"  class="but130lan_add"/>
			 </div>
		</div>';
		$html.=$js;
	}
	
	exit($html);
}
//创建简历-修改教育经历
elseif ($act=='edit_education')
{
	$uid=intval($_SESSION['uid']);
	$pid=intval($_REQUEST['pid']);
	if ($uid==0 || $pid==0) exit('简历不存在！');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['pid']));
	if (empty($resume_basic)) exit("请先填写简历基本信息！");
	$id=intval($_GET['id'])?intval($_GET['id']):exit('参数错误！');
	$education_edit = get_resume_education_one($_SESSION['uid'],$pid,$id);
	foreach ($education_edit as $key => $value) {
		$education_edit[$key] = gbk_to_utf8($value);
	}
	$json_encode = json_encode($education_edit);
	exit($json_encode);
}
//创建简历-删除教育经历
elseif ($act=='del_education')
{
	$id=intval($_GET['id']);
	$sql="Delete from ".table('resume_education')." WHERE id='{$id}'  AND uid='".intval($_SESSION['uid'])."' AND pid='".intval($_REQUEST['pid'])."' LIMIT 1 ";
	if ($db->query($sql))
	{
	check_resume($_SESSION['uid'],intval($_REQUEST['pid']));//更新简历完成状态
	exit('删除成功！');
	}
	else
	{
	exit('删除失败！');
	}	
}
elseif($act=='save_work'){
	$id=intval($_POST['id']);
	$setsqlarr['uid'] = intval($_SESSION['uid']);
	$setsqlarr['pid'] = intval($_REQUEST['pid']);
	if ($setsqlarr['uid']==0 || $setsqlarr['pid']==0 ) exit('简历不存在！');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['pid']));
	if (empty($resume_basic)) exit("请先填写简历基本信息！");
	$resume_work=get_resume_work($_SESSION['uid'],$_REQUEST['pid']);
	if (count($resume_work)>=6) exit('工作经历不能超过6条！');

	$companyname = utf8_to_gbk(trim($_POST['companyname']));
	$jobs = utf8_to_gbk(trim($_POST['jobs']));
	$achievements = utf8_to_gbk(trim($_POST['achievements']));
	$setsqlarr['companyname'] = $companyname?$companyname:exit("请填写公司名称！");
	check_word($_CFG['filter'],$setsqlarr['companyname'])?exit($_CFG['filter_tips']):'';
	$setsqlarr['jobs'] = $jobs?$jobs:exit("请填写职位名称！");
	check_word($_CFG['filter'],$setsqlarr['jobs'])?exit($_CFG['filter_tips']):'';
	// 选择至今就不判断结束时间了
	if (intval($_POST['work_todate']) == 1) {
		if(trim($_POST['work_start_year'])==""||trim($_POST['work_start_month'])==""){
			exit("请选择任职时间！");
		}
		if(intval(($_POST['work_start_year']))>intval(date('Y'))){
			exit('工作开始时间不允许大于工作结束时间');
		}
		if(intval($_POST['work_start_year']) == intval(date('Y')) && intval(($_POST['work_start_month']))>=intval(date('m'))){
				exit('工作开始月份不允许大于工作结束时间');
		}
	} else {
		if(trim($_POST['work_start_year'])==""||trim($_POST['work_start_month'])==""||trim($_POST['work_end_year'])==""||trim($_POST['work_end_month'])==""){
			exit("请选择任职时间！");
		}
		if(intval(($_POST['work_start_year']))>intval($_POST['work_end_year'])){
			exit('工作开始时间不允许大于工作结束时间');
		}
		if(intval($_POST['work_start_year']) == intval($_POST['work_end_year']) && intval(($_POST['work_start_month']))>=intval($_POST['work_end_month'])){
				exit('工作开始月份不允许大于工作结束时间');
		}
	}
	$setsqlarr['startyear'] = intval($_POST['work_start_year']);
	$setsqlarr['startmonth'] = intval($_POST['work_start_month']);
	$setsqlarr['endyear'] = intval($_POST['work_end_year']);
	$setsqlarr['endmonth'] = intval($_POST['work_end_month']);
	$setsqlarr['achievements'] = $achievements?$achievements:exit("请填写工作职责！");
	$setsqlarr['todate'] = intval($_POST['work_todate']); // 至今
	check_word($_CFG['filter'],$setsqlarr['achievements'])?exit($_CFG['filter_tips']):'';
	
	if($id){
		$db->updatetable(table("resume_work"),$setsqlarr,array("id"=>$id));
		exit("success");
	}else{
		$insert_id = $db->inserttable(table("resume_work"),$setsqlarr,1);
		if($insert_id){
			check_resume($_SESSION['uid'],intval($_REQUEST['pid']));
			exit("success");
		}else{
			exit("err");
		}
	}
	
}
elseif($act=='ajax_get_work_list'){
	$pid=intval($_GET['pid']);
	$uid=intval($_SESSION['uid']);
	$work_list = get_resume_work($uid,$pid);
	$html="";
	if($work_list){
		foreach ($work_list as $key => $value) {
			// 判断结束时间是否至今
			$datehtm = '';
			if($value["todate"] == 1) {
				$datehtm = '至今';
			} else {
				$datehtm = $value["endyear"].'年'.$value["endmonth"].'月';
			}
			// ===========================
			$html.='<div class="jl2">
					 	 <div class="l1">'.$value["startyear"].'年'.$value["startmonth"].'月-'.$datehtm.'</div>
						 <div class="l2">'.$value["companyname"].'</div>
						 <div class="l3">'.$value["jobs"].'</div>
						 <div class="l4">
						 <a class="edit_work" todate="'.$value["todate"].'" href="javascript:void(0);" url="?act=edit_work&id='.$value["id"].'&pid='.$pid.'"></a>
						 <a class="del_work d" href="javascript:void(0);" pid="'.$pid.'" work_id="'.$value["id"].'" ></a><div class="clear"></div>
						 <div class="clear"></div>
						 </div>
						 <div class="l5">工作职责：</div>
						 <div class="l6">'.$value["achievements"].'
						 </div>
						 <div class="clear"></div>
					</div>';
		}
	}else{
		$js='<script type="text/javascript">$("#add_work").hide();$(function(){$(".but130lan_add").hover(function(){$(this).addClass("hover")},function(){$(this).removeClass("hover");})})</script>';
		$html.='<div class="noinfo" id="work_empty_box">	
			 	 <div class="txt">工作经历最能体现您丰富的阅历和出众的工作能力，是你薪酬翻倍的筹码哦HR青睐吧！</div>
				 <div class="addbut">
				 	<input type="button" name="" id="empty_add_work" value="添加经历"  class="but130lan_add"/>
				 </div>
			</div>';
		$html.=$js;
	}
	
	exit($html);
}
//创建简历-修改工作经历
elseif ($act=='edit_work')
{
	$uid=intval($_SESSION['uid']);
	$pid=intval($_REQUEST['pid']);
	if ($uid==0 || $pid==0) exit('简历不存在！');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['pid']));
	if (empty($resume_basic)) exit("请先填写简历基本信息！");
	$id=intval($_GET['id'])?intval($_GET['id']):exit('参数错误！');
	$work_edit = get_resume_work_one($_SESSION['uid'],$pid,$id);
	foreach ($work_edit as $key => $value) {
		$work_edit[$key] = gbk_to_utf8($value);
	}
	$json_encode = json_encode($work_edit);
	exit($json_encode);
}
//创建简历-删除工作经历
elseif ($act=='del_work')
{
	$id=intval($_GET['id']);
	$sql="Delete from ".table('resume_work')." WHERE id='{$id}'  AND uid='".intval($_SESSION['uid'])."' AND pid='".intval($_REQUEST['pid'])."' LIMIT 1 ";
	if ($db->query($sql))
	{
	check_resume($_SESSION['uid'],intval($_REQUEST['pid']));//更新简历完成状态
	exit('删除成功！');
	}
	else
	{
	exit('删除失败！');
	}	
}
elseif($act=='save_training'){
	$id=intval($_POST['id']);
	$setsqlarr['uid'] = intval($_SESSION['uid']);
	$setsqlarr['pid'] = intval($_REQUEST['pid']);
	if ($setsqlarr['uid']==0 || $setsqlarr['pid']==0 ) exit('简历不存在！');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['pid']));
	if (empty($resume_basic)) exit("请先填写简历基本信息！");
	$resume_training=get_resume_training($_SESSION['uid'],$_REQUEST['pid']);
	if (count($resume_training)>=6) exit('培训经历不能超过6条！');
	
	$agency = utf8_to_gbk(trim($_POST['agency']));
	$course = utf8_to_gbk(trim($_POST['course']));
	$description = utf8_to_gbk(trim($_POST['description']));
	$setsqlarr['agency'] = $agency?$agency:exit("请填写培训机构！");
	check_word($_CFG['filter'],$setsqlarr['agency'])?exit($_CFG['filter_tips']):'';
	$setsqlarr['course'] = $course?$course:exit("请填写培训课程！");
	check_word($_CFG['filter'],$setsqlarr['course'])?exit($_CFG['filter_tips']):'';
	// 选择至今就不判断结束时间了
	if (intval($_POST['training_todate']) == 1) {
		if(trim($_POST['training_start_year'])==""||trim($_POST['training_start_month'])==""){
			exit("请选择培训时间！");
		}
		if(intval(($_POST['training_start_year']))>intval(date('Y'))){
			exit('培训开始时间不允许大于培训结束时间');
		}
		if(intval($_POST['training_start_year']) == intval(date('Y')) && intval(($_POST['training_start_month']))>=intval(date('m'))){
				exit('培训开始月份不允许大于培训结束时间');
		}
	} else {
		if(trim($_POST['training_start_year'])==""||trim($_POST['training_start_month'])==""||trim($_POST['training_end_year'])==""||trim($_POST['training_end_month'])==""){
			exit("请选择培训时间！");
		}
		if(intval(($_POST['training_start_year']))>intval($_POST['training_end_year'])){
			exit('培训开始时间不允许大于培训结束时间');
		}
		if(intval($_POST['training_start_year']) == intval($_POST['training_end_year']) && intval(($_POST['training_start_month']))>=intval($_POST['training_end_month'])){
				exit('培训开始月份不允许大于培训结束时间');
		}
	}
	$setsqlarr['startyear'] = intval($_POST['training_start_year']);
	$setsqlarr['startmonth'] = intval($_POST['training_start_month']);
	$setsqlarr['endyear'] = intval($_POST['training_end_year']);
	$setsqlarr['endmonth'] = intval($_POST['training_end_month']);
	$setsqlarr['description'] = $description?$description:exit("请填写培训内容！");
	$setsqlarr['todate'] = intval($_POST['training_todate']); // 至今
	check_word($_CFG['filter'],$setsqlarr['description'])?exit($_CFG['filter_tips']):'';
	
	if($id){
		$db->updatetable(table("resume_training"),$setsqlarr,array("id"=>$id));
		exit("success");
	}else{
		$insert_id = $db->inserttable(table("resume_training"),$setsqlarr,1);
		if($insert_id){
			check_resume($_SESSION['uid'],intval($_REQUEST['pid']));
			exit("success");
		}else{
			exit("err");
		}
	} 
}
elseif($act=='ajax_get_training_list'){
	$pid=intval($_GET['pid']);
	$uid=intval($_SESSION['uid']);
	$training_list = get_resume_training($uid,$pid);
	$html="";
	if($training_list){
		foreach ($training_list as $key => $value) {
			// 判断结束时间是否至今
			$datehtm = '';
			if($value["todate"] == 1) {
				$datehtm = '至今';
			} else {
				$datehtm = $value["endyear"].'年'.$value["endmonth"].'月';
			}
			// ===========================
			$html.='<div class="jl2">
			 	 <div class="l1">'.$value["startyear"].'年'.$value["startmonth"].'月-'.$datehtm.'</div>
				 <div class="l2">'.$value["agency"].'</div>
				 <div class="l3">'.$value["course"].'</div>
				 <div class="l4">
				 <a class="edit_training" todate="'.$value["todate"].'" href="javascript:void(0);" url="?act=edit_training&id='.$value["id"].'&pid='.$pid.'"></a>
				 <a class="del_training d" href="javascript:void(0);" pid="'.$pid.'" training_id="'.$value["id"].'" ></a><div class="clear"></div>
				 </div>
				 <div class="l5">培训内容：</div>
				 <div class="l6">'.$value["description"].'</div>
				 <div class="clear"></div>
			</div>';
		}
	}else{
		$js='<script type="text/javascript">$("#add_training").hide();$(function(){$(".but130lan_add").hover(function(){$(this).addClass("hover")},function(){$(this).removeClass("hover");})})</script>';
		$html.='<div class="noinfo" id="training_empty_box">	
		 	 <div class="txt">培训经历是你勇于上进的最好的体现，快来说说令您难忘的学习经历吧！</div>
			 <div class="addbut">
			 	<input type="button" name="" id="empty_add_training" value="添加经历"  class="but130lan_add"/>
			 </div>
		</div>';
		$html.=$js;
	}
	exit($html);
}
//创建简历-修改培训经历
elseif ($act=='edit_training')
{
	$uid=intval($_SESSION['uid']);
	$pid=intval($_REQUEST['pid']);
	if ($uid==0 || $pid==0) exit('简历不存在！');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['pid']));
	if (empty($resume_basic)) exit("请先填写简历基本信息！");
	$id=intval($_GET['id'])?intval($_GET['id']):exit('参数错误！');
	$training_edit = get_resume_training_one($_SESSION['uid'],$pid,$id);
	foreach ($training_edit as $key => $value) {
		$training_edit[$key] = gbk_to_utf8($value);
	}
	$json_encode = json_encode($training_edit);
	exit($json_encode);
}
//创建简历-删除培训经历
elseif ($act=='del_training')
{
	$id=intval($_GET['id']);
	$sql="Delete from ".table('resume_training')." WHERE id='{$id}'  AND uid='".intval($_SESSION['uid'])."' AND pid='".intval($_REQUEST['pid'])."' LIMIT 1 ";
	if ($db->query($sql))
	{
	check_resume($_SESSION['uid'],intval($_REQUEST['pid']));//更新简历完成状态
	exit('删除成功！');
	}
	else
	{
	exit('删除失败！');
	}	
}
//语言
elseif($act=='ajax_get_language_list'){
	$pid=intval($_GET['pid']);
	$uid=intval($_SESSION['uid']);
	$language_list = get_resume_language($uid,$pid);
	$html="";
	if($language_list){
		foreach ($language_list as $key => $value) {
			$html.='<div class="jl2">
			 	 <div class="l1">'.$value["language_cn"].'</div>
				 <div class="l2">'.$value["level_cn"].'</div> 
				  <div class="l3"> </div> 
				 <div class="l4">
				 <a class="edit_language" href="javascript:void(0);" url="?act=edit_language&id='.$value["id"].'&pid='.$pid.'"></a>
				 <a class="del_language d" href="javascript:void(0);" pid="'.$pid.'" language_id="'.$value["id"].'" ></a><div class="clear"></div>
				 </div> 
				 <div class="clear"></div>
			</div>';
		}
	}else{
		$js='<script type="text/javascript">$("#add_language").hide();$(function(){$(".but130lan_add").hover(function(){$(this).addClass("hover")},function(){$(this).removeClass("hover");})})</script>';
		$html.='<div class="noinfo" id="language_empty_box">	
		 	 <div class="txt">语言能力是你勇于上进的最好的体现，快来说说令您难忘的语言能力吧！</div>
			 <div class="addbut">
			 	<input type="button" name="" id="empty_add_language" value="添加语言"  class="but130lan_add"/>
			 </div>
		</div>';
		$html.=$js;
	} 
	exit($html);
}
elseif($act=='save_language'){
	$id=intval($_POST['id']);
	$setsqlarr['uid'] = intval($_SESSION['uid']);
	$setsqlarr['pid'] = intval($_REQUEST['pid']);
	if ($setsqlarr['uid']==0 || $setsqlarr['pid']==0 ) exit('简历不存在！');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['pid']));
	if (empty($resume_basic)) exit("请先填写简历基本信息！");
	$resume_language=get_resume_language($_SESSION['uid'],$_REQUEST['pid']);
	if (count($resume_language)>=6) exit('语言能力不能超过6条！');
 
	$language_cn = utf8_to_gbk(trim($_POST['language_cn']));
	$language_level_cn = utf8_to_gbk(trim($_POST['language_level_cn']));
	 
	$setsqlarr['language_cn'] = $language_cn?$language_cn:exit("请填语言类型！"); 
	$setsqlarr['level_cn'] = $language_level_cn?$language_level_cn:exit("请填写语言等级！");
	 
	$setsqlarr['language'] = intval($_POST['language']);
	$setsqlarr['level'] = intval($_POST['language_level']); 
	if($id){
		$db->updatetable(table("resume_language"),$setsqlarr,array("id"=>$id));
		exit("success");
	}else{
		$insert_id = $db->inserttable(table("resume_language"),$setsqlarr,1);
		if($insert_id){
			check_resume($_SESSION['uid'],intval($_REQUEST['pid']));
			exit("success");
		}else{
			exit("err");
		}
	} 
}
//创建简历-修改培训经历
elseif ($act=='edit_language')
{
	$uid=intval($_SESSION['uid']);
	$pid=intval($_REQUEST['pid']);
	if ($uid==0 || $pid==0) exit('简历不存在！');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['pid']));
	if (empty($resume_basic)) exit("请先填写简历基本信息！");
	$id=intval($_GET['id'])?intval($_GET['id']):exit('参数错误！');
	$language_edit = get_resume_language_one($_SESSION['uid'],$pid,$id);
	foreach ($language_edit as $key => $value) {
		$language_edit[$key] = gbk_to_utf8($value);
	}
	$json_encode = json_encode($language_edit);
	exit($json_encode);
}
//创建简历-删除培训经历
elseif ($act=='del_language')
{
	$id=intval($_GET['id']);
	$sql="Delete from ".table('resume_language')." WHERE id='{$id}'  AND uid='".intval($_SESSION['uid'])."' AND pid='".intval($_REQUEST['pid'])."' LIMIT 1 ";
	if ($db->query($sql))
	{
	check_resume($_SESSION['uid'],intval($_REQUEST['pid']));//更新简历完成状态
	exit('删除成功！');
	}
	else
	{
	exit('删除失败！');
	}	
}
//证书
elseif($act=='ajax_get_credent_list'){
	$pid=intval($_GET['pid']);
	$uid=intval($_SESSION['uid']);
	$credent_list = get_resume_credent($uid,$pid);
	$html="";
	if($credent_list){
		foreach ($credent_list as $key => $value) {
			$html.='<div class="jl2">
				 <div class="l1">'.$value["name"].'</div>
			 	 <div class="l2">'.$value["year"].'年'.$value["month"].'月</div>  
				 <div class="l3"></div>  
				 <div class="l4">
				 <a class="edit_credent" href="javascript:void(0);" url="?act=edit_credent&id='.$value["id"].'&pid='.$pid.'"></a>
				 <a class="del_credent d" href="javascript:void(0);" pid="'.$pid.'" credent_id="'.$value["id"].'" ></a><div class="clear"></div>
				 </div> 
			</div>';
		}
	}else{
		$js='<script type="text/javascript">$("#add_credent").hide();$(function(){$(".but130lan_add").hover(function(){$(this).addClass("hover")},function(){$(this).removeClass("hover");})})</script>';
		$html.='<div class="noinfo" id="credent_empty_box">	
		 	 <div class="txt">证书是你勇于上进的最好的体现，快来说说令您难忘的获得的证书吧！</div>
			 <div class="addbut">
			 	<input type="button" name="" id="empty_add_credent" value="添加证书"  class="but130lan_add"/>
			 </div>
		</div>';
		$html.=$js;
	}
	exit($html);
}
elseif($act=='save_credent'){
	$id=intval($_POST['id']);
	$setsqlarr['uid'] = intval($_SESSION['uid']);
	$setsqlarr['pid'] = intval($_REQUEST['pid']);
	if ($setsqlarr['uid']==0 || $setsqlarr['pid']==0 ) exit('简历不存在！');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['pid']));
	if (empty($resume_basic)) exit("请先填写简历基本信息！");
	$resume_language=get_resume_language($_SESSION['uid'],$_REQUEST['pid']);
	if (count($resume_language)>=6) exit('证书不能超过6条！');
 
	$credent = utf8_to_gbk(trim($_POST['credent'])); 
	check_word($_CFG['filter'],$setsqlarr['credent'])?exit($_CFG['filter_tips']):'';
	$setsqlarr['name'] = $credent?$credent:exit("请填写证书名称！");
	$setsqlarr['year'] = intval($_POST['credent_year'])?intval($_POST['credent_year']):exit("请选择年！");
	$setsqlarr['month'] = intval($_POST['credent_month'])?intval($_POST['credent_month']):exit("请选择月！");

	if($id){
		$db->updatetable(table("resume_credent"),$setsqlarr,array("id"=>$id));
		exit("success");
	}else{
		$insert_id = $db->inserttable(table("resume_credent"),$setsqlarr,1);
		if($insert_id){
			check_resume($_SESSION['uid'],intval($_REQUEST['pid']));
			exit("success");
		}else{
			exit("err");
		}
	} 
} 
elseif ($act=='edit_credent')
{
	$uid=intval($_SESSION['uid']);
	$pid=intval($_REQUEST['pid']);
	if ($uid==0 || $pid==0) exit('简历不存在！');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_REQUEST['pid']));
	if (empty($resume_basic)) exit("请先填写简历基本信息！");
	$id=intval($_GET['id'])?intval($_GET['id']):exit('参数错误！');
	$credent_edit = get_resume_credent_one($_SESSION['uid'],$pid,$id);
	foreach ($credent_edit as $key => $value) {
		$credent_edit[$key] = gbk_to_utf8($value);
	}
	$json_encode = json_encode($credent_edit);
	exit($json_encode);
}
//删除证书
elseif ($act=='del_credent')
{
	$id=intval($_GET['id']); 
	$sql = "select * from ".table('resume_credent')." where id='".$id."' AND uid='".intval($_SESSION['uid'])."'  LIMIT 1 ";
	$credent_one = $db->getone($sql);
	$images = $credent_one['images'];
	@unlink('../../data/credent_photo/'.$images);
	$sql="Delete from ".table('resume_credent')." WHERE id='{$id}'  AND uid='".intval($_SESSION['uid'])."' AND pid='".intval($_REQUEST['pid'])."' LIMIT 1 ";
	if ($db->query($sql))
	{ 
	check_resume($_SESSION['uid'],intval($_REQUEST['pid']));//更新简历完成状态
	exit('删除成功！');
	}
	else
	{
	exit('删除失败！');
	}	
}//上传证书
elseif ($act=='credent_photo')
{	 
	!$_FILES['credent_photo']['name']?exit('请上传图片！'):"";
	require_once(QISHI_ROOT_PATH.'include/cut_upload.php');  
	$up_res_original="../../data/credent_photo/"; 
	$cdate = date("Y/m/d/");
	$mkdir = $up_res_original.$cdate;
	make_dir($mkdir); 
	$images =_asUpFiles($up_res_original.$cdate,"credent_photo",$_CFG['resume_photo_max'],'gif/jpg/bmp/png',true);
	$data['save_url'] = $cdate.$images;
	$images = $mkdir.$images;
	$data['url'] = $images; 
	$json_encode = json_encode($data);
	exit($json_encode);
}
////上传WORD
elseif ($act=='word_upload')
{	
	$pid=intval($_GET['pid']);
	$setsqlarr['uid'] = intval($_SESSION['uid']);
	!$_FILES['word_resume']['name']?exit('请上传文件！'):""; 
	require_once(QISHI_ROOT_PATH.'include/cut_upload.php');
	$up_res_original="../../data/word/"; 
	$cdate = date("Y/m/d/");
	$mkdir = $up_res_original.$cdate;
	make_dir($mkdir); 
	$word=get_resume_basic($setsqlarr['uid'],$pid);
	$word_resume =_asUpFiles($up_res_original.$cdate,"word_resume",2048,'doc',true);
	$setsqlarr['word_resume'] = $cdate.$word_resume;
	$db->updatetable(table("resume"),$setsqlarr,array("id"=>$pid));
	@unlink(QISHI_ROOT_PATH."data/word/".$word['word_resume']);
	$data['save_url'] = $setsqlarr['word_resume'];
	$json_encode = json_encode($data);
	exit($json_encode);
}
// 删除 word 
elseif($act == "word_del")
{
	$pid=$_POST['pid']?intval($_POST['pid']):exit("简历ID丢失");
	$uid=intval($_SESSION['uid']);
	$word=get_resume_basic($uid,$pid);
	@unlink(QISHI_ROOT_PATH."data/word/".$word['word_resume']);
	$setarr['word_resume']="";
	$db->updatetable(table("resume"),$setarr,array('uid'=>$uid,"id"=>$pid))?exit("删除成功"):exit("删除成功");
}
elseif($act == "ajax_save_specialty")
{
	$uid=intval($_SESSION['uid']);
	$pid=$_POST['pid']?intval($_POST['pid']):exit("简历ID丢失");
	$specialty=$_POST['specialty']?iconv("utf-8", "gbk", trim($_POST['specialty'])):exit("请输入自我描述");
	$setarr['specialty']=$specialty;
	$db->updatetable(table('resume'),$setarr,array("id"=>$pid,"uid"=>$uid))?exit("ok"):exit("保存失败");
}
// ajax 保存 附件图片
elseif($act == "ajax_resume_img_save")
{
	$uid=intval($_SESSION['uid']);
	$pid=$_GET['pid']?intval($_GET['pid']):exit("简历ID丢失");
	$n=$db->get_total("SELECT COUNT(*) AS num FROM ".table('resume_img')." WHERE uid=$uid and resume_id=$pid ");
	if($n>=4)
	{
		exit("-7");
	}
	require_once(QISHI_ROOT_PATH.'include/upload.php');
	!$_FILES['resume_img']['name']?exit('请上传图片！'):"";
	$datedir=date("Y/m/d/");
	$up_dir="../../data/photo/".$datedir;
	make_dir($up_dir);
	$setsqlarr['img']=_asUpFiles($up_dir,"resume_img",800,'gif/jpg/bmp/png/jpeg',true);
	if ($setsqlarr['img'])
	{
		$img_src=$up_dir.$setsqlarr['resume_img'];
		makethumb($img_src,$up_dir,600,600);
		$setsqlarr['uid']=$uid;
		$setsqlarr['resume_id']=$pid;
		$setsqlarr['addtime']=time();
		$setsqlarr['img']=$datedir.$setsqlarr['img'];
		$img_id = $db->inserttable(table('resume_img'),$setsqlarr,true);
		if ($img_id > 0)
		{
			$data['save_url'] = $setsqlarr['img'];
			$data['url'] = $setsqlarr['img'];
			$data['title'] = $setsqlarr['title'];
			$data['addtime'] = date('Y-m-d',$setsqlarr['addtime']);
			$data['id'] = $img_id; 
			$json_encode = json_encode($data);
			exit($json_encode); 
		}
		else
		{
			exit("-6");
		}
	}
	else
	{
		exit("-6");
	}
}
// 保存附件描述
elseif($act == "ajax_resume_img_title_save")
{
	$uid=intval($_SESSION['uid']);
	$img_id=$_POST['id']?intval($_POST['id']):exit("ID丢失");
	$setarr['title']=$_POST['title']?iconv("utf-8", "gbk", trim($_POST['title'])):exit("请输入备注！");
	$db->updatetable(table("resume_img"),$setarr,array("id"=>$img_id,"uid"=>$uid))?exit("添加备注成功"):exit("添加备注失败");
}
// 删除附件 图片
elseif($act== "ajax_resume_img_del")
{
	global $_CFG;
	$uid=intval($_SESSION['uid']);
	$img_id=$_POST['id']?intval($_POST['id']):exit("ID丢失");
	$row=$db->getone("select img from ".table("resume_img")." where id=$img_id and uid=$uid limit 1");
	@unlink("../../data/photo/".$row['img']);
	$db->query("delete from ".table("resume_img")." where id=$img_id and uid=$uid limit 1")?exit("删除成功"):exit("删除失败");
}
// ajax 保存特长标签
elseif($act == "ajax_save_tag")
{
	$uid=intval($_SESSION['uid']);
	$pid=$_POST['pid']?intval($_POST['pid']):exit("简历ID丢失");
	$tag=$_POST['tag']?iconv("utf-8", "gbk", trim($_POST['tag'])):"";
	$tag_cn=$_POST['tag_cn']?iconv("utf-8", "gbk", trim($_POST['tag_cn'])):"";
	$setarr['tag']=$tag;
	$setarr['tag_cn']=$tag_cn;
	add_resume_tag($pid,$uid,$tag);
	$db->updatetable(table('resume'),$setarr,array("id"=>$pid,"uid"=>$uid))?exit("ok"):exit("保存失败");
}
// 简历发布按钮 
elseif($act == "edit_resume_save")
{
	$uid=intval($_SESSION['uid']);
	$pid=$_POST['pid']?intval($_POST['pid']):showmsg("简历ID丢失",1);
	$resume_basic= get_resume_basic($uid,$pid);
	$make=intval($_POST['make']);
	check_resume($uid,$pid);
	if($make==1)
	{
		header("Location: ?act=make1_succeed&pid=".$pid);
	}
	else
	{
		header("Location: ?act=resume_list");
	}
	
}
elseif ($act=='edit_resume')
{
	$uid=intval($_SESSION['uid']);
	$pid=intval($_REQUEST['pid']);
	if($_GET['make']==1)
	{
		$title="创建简历";
	}else
	{
		$title="修改简历";
	}
	$smarty->assign('h_title',$title);
	$_SESSION['send_mobile_key']=mt_rand(100000, 999999);
	$smarty->assign('send_key',$_SESSION['send_mobile_key']);
	$smarty->assign('resume_basic',get_resume_basic($uid,$pid));
	$smarty->assign('resume_education',get_resume_education($uid,$pid));
	$smarty->assign('resume_work',get_resume_work($uid,$pid));
	$smarty->assign('resume_training',get_resume_training($uid,$pid));
	$smarty->assign('resume_language',get_resume_language($uid,$pid));
	$smarty->assign('resume_credent',get_resume_credent($uid,$pid));
	$smarty->assign('resume_img',get_resume_img($uid,$pid));

	$resume_jobs=get_resume_jobs($pid);
	if ($resume_jobs)
	{
		foreach($resume_jobs as $rjob)
		{
		$jobsid[]=$rjob['topclass'].".".$rjob['category'].".".$rjob['subclass'];
		}
		$resume_jobs_id=implode(",",$jobsid);
	}
	$smarty->assign('resume_jobs_id',$resume_jobs_id);
	$resume_district=get_resume_district($pid);
	if ($resume_district)
	{
		foreach($resume_district as $rcity)
		{
		$cityid[]=$rcity['district'].".".$rcity['sdistrict'];
		}
		$resume_district_id=implode(",",$cityid);
	}
	$smarty->assign('resume_district_id',$resume_district_id);
	$smarty->assign('act',$act);
	$smarty->assign('pid',$pid);
	$smarty->assign('user',$user);
	$smarty->assign('title','我的简历 - 个人会员中心 - '.$_CFG['site_name']);
	$captcha=get_cache('captcha');
	$smarty->assign('verify_resume',$captcha['verify_resume']);
	$smarty->assign('go_resume_show',$_GET['go_resume_show']);
	$smarty->display('member_personal/personal_resume_edit.htm');
}
elseif ($act=='save_resume_privacy')
{
	$uid=intval($_SESSION['uid']);
	$pid=intval($_REQUEST['pid']);
	$setsqlarr['display']=intval($_POST['display']);
	$setsqlarr['display_name']=intval($_POST['display_name']);
	$setsqlarr['photo_display']=intval($_POST['photo_display']);
	$wheresql=" uid='".$_SESSION['uid']."' ";
	!$db->updatetable(table('resume'),$setsqlarr," uid='{$uid}' AND  id='{$pid}'");
	$setsqlarrdisplay['display']=intval($_POST['display']);
	!$db->updatetable(table('resume_search_key'),$setsqlarrdisplay," uid='{$uid}' AND  id='{$pid}'");
	!$db->updatetable(table('resume_search_rtime'),$setsqlarrdisplay," uid='{$uid}' AND  id='{$pid}'");
	write_memberslog($_SESSION['uid'],2,1104,$_SESSION['username'],"设置简历隐私({$pid})");
}
elseif ($act=='talent_save')
{
	$uid=intval($_SESSION['uid']);
	$pid=intval($_REQUEST['pid']);
	$resume=get_resume_basic($uid,$pid);
	if ($resume['complete_percent']<$_CFG['elite_resume_complete_percent'])
	{
	showmsg("简历完整指数小于{$_CFG['elite_resume_complete_percent']}%，禁止申请！",0);
	}
	$setsqlarr['talent']=3;
	$wheresql=" uid='{$uid}' AND id='{$pid}' ";
	$db->updatetable(table('resume'),$setsqlarr,$wheresql);
	write_memberslog($uid,2,1107,$_SESSION['username'],"申请高级人才");
	showmsg('申请成功，请等待管理员审核！',2);
}
unset($smarty);
?>