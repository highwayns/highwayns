<?php
 /*
 * 74cms 发送邮件
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
ignore_user_abort(true);
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
require_once(QISHI_ROOT_PATH.'include/fun_user.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_GET['act']) ? trim($_GET['act']) : '';
$uid=intval($_GET['uid']);
$key=trim($_GET['key']);
if (empty($uid) || empty($key))
{
 exit("error");
}
$asyn_userkey=asyn_userkey($uid);
if ($asyn_userkey<>$key)exit("error");
$mailconfig=get_cache('mailconfig');
$mail_templates=get_cache('mail_templates');
//发送注册邮件
if($act == 'reg'){
	if ($_GET['sendemail'] && $_GET['sendusername'] && $_GET['sendpassword'] && $mailconfig['set_reg']=="1")
	{
			$userinfo=get_user_inid($uid);
			if ($userinfo['username']==$_GET['sendusername'] && $userinfo['email']==$_GET['sendemail'])
			{ 
			$templates=label_replace($mail_templates['set_reg']);
			$templates_title=label_replace($mail_templates['set_reg_title']);
			smtp_mail($_GET['sendemail'],$templates_title,$templates);
			}
	}
}
//申请职位发送邮件
elseif($act == 'jobs_apply')
{   
	global $_CFG;
	$templates=label_replace($mail_templates['set_applyjobs']);
	$templates_title=label_replace($mail_templates['set_applyjobs_title']);
	// 申请职位发送邮件 简历信息
	require_once(QISHI_ROOT_PATH.'include/fun_personal.php');
	$resume_id=intval($_GET['resume_id']);
	$resume_basic=get_resume_basic($uid,$resume_id);
	if($resume_basic['tag_cn'])
	{
		$resume_tag=explode(',',$resume_basic['tag_cn']);
		$tag_str='<p>';
		foreach ($resume_tag as $value)
		{
			$tag_str.='<span style="color: #656565;display:inline-block;background-color: #f2f4f7; border: 1px solid #d6d6d7;text-align: center;height:30px;line-height: 30px;margin-right:10px;padding:0 10px">'.$value.'</span>';
		}
		$tag_str.='</p>';
	}
	$resume_work=get_resume_work($uid,$resume_id);
	$show_contact = false;
	if($_CFG['showapplycontact']=='1' || $_CFG['showresumecontact']=='0')
	{
		$show_contact = '<p>手机号码：'.$resume_basic["telephone"].' 电子邮箱：'.$resume_basic["email"].'</p>';
	}
	else
	{
		$show_contact = '<p>联系方式：<a href='.url_rewrite('QS_resumeshow',array('id'=>$resume_id)).'>点击查看</a></p>';
	}	
	$htm='<div style="width: 900px;margin: 0 auto;font-size: 14px;">
		<div style="margin-bottom:10px">
			<div style="float: left;"><a href="'.$_CFG['site_domain'].$_CFG['site_dir'].'"><img src="'.$_CFG['site_domain'].$_CFG['upfiles_dir'].$_CFG['web_logo'].'" alt="'.$_CFG['site_name'].'" border="0" align="absmiddle" width=180 height=50 /></a></div>
			<div style="float: right;padding-top:10px;">'.$templates.'更新时间：'.date("Y-m-d",$resume_basic["refreshtime"]).'</div>
			<div style="clear:both"></div>
		</div>
		<div style="padding-bottom: 10px;">
			<span style="font-size: 18px;font-weight: 700;">'.$resume_basic["fullname"].'</span><span>（'.$resume_basic["sex_cn"].'，'.$resume_basic["age"].'）</span>
			<p>学历：'.$resume_basic["education_cn"].' | 专业：'.$resume_basic["major_cn"].' | 工作经验：'.$resume_basic["experience_cn"].'年 | 现居住地：'.$resume_basic["residence"].'</p>

			'.$show_contact.$tag_str.'

		</div>
		<div style="padding-bottom: 10px;">
			<p style="font-size: 16px;font-weight: 700;">求职意向</p>
			<p>期望职位：'.$resume_basic["intention_jobs"].'</p>
			<p>期望薪资：'.$resume_basic["wage_cn"].'</p>
			<p>期望地区：'.$resume_basic["district_cn"].'</p>
		</div>
		<div style="padding-bottom: 10px;">
			<p style="font-size: 16px;font-weight: 700;">工作经验</p>';
				if(!empty($resume_work))
				{
					foreach ($resume_work as $value)
					{
						$htm.='<div>
								<p style="font-size: 14px;font-weight: 700;">'.$value["companyname"].'</p>
								<p>'.$value["startyear"].'年'.$value["startmonth"].'月-'.$value["endyear"].'年'.$value["endmonth"].'月 '.$value["jobs"].'</p>
								<div style="float: left;width: 100px;">工作内容：</div>
								<div style="float: right;width: 800px;">'.$value["achievements"].'</div>
								<div style="clear:both"></div>
							</div>'	;
					}
				}
				else
				{
					$htm.='<div>
								没有填写工作经历
							</div>'	;
				}
				
		$htm.='</div>';
		if($resume_basic["specialty"])
		{
			$htm.='<div style="padding-bottom: 10px;">
				<p style="font-size: 16px;font-weight: 700;">自我描述</p>
				<p>'.$resume_basic["specialty"].'</p>
			</div>';
		}
		$htm.='<div style="text-align: center;margin-top:20px">
				该简历来自<a href="'.$_CFG["site_domain"].$_CFG["site_dir"].'">'.$_CFG["site_name"].'</a>
			</div>
		</div>';
	smtp_mail($_GET['email'],$templates_title,$htm);
}
//邀请面试发送邮件
elseif($act == 'set_invite')
{
			$templates=label_replace($mail_templates['set_invite']);
			$templates_title=label_replace($mail_templates['set_invite_title']);
			smtp_mail($_GET['email'],$templates_title,$templates);
}
//申请充值，发送邮件
elseif($act == 'set_order'){
			$templates=label_replace($mail_templates['set_order']);
			$templates_title=label_replace($mail_templates['set_order_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//充值成功，发送邮件
elseif($act == 'set_payment'){
			$templates=label_replace($mail_templates['set_payment']);
			$templates_title=label_replace($mail_templates['set_payment_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//修改密码，发送邮件
elseif($act == 'set_editpwd'){
			$templates=label_replace($mail_templates['set_editpwd']);
			$templates_title=label_replace($mail_templates['set_editpwd_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//职位审核通过，发送邮件
elseif($act == 'set_jobsallow'){
			$templates=label_replace($mail_templates['set_jobsallow']);
			$templates_title=label_replace($mail_templates['set_jobsallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//职位未审核通过，发送邮件
elseif($act == 'set_jobsnotallow'){
			$templates=label_replace($mail_templates['set_jobsnotallow']);
			$templates_title=label_replace($mail_templates['set_jobsnotallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//企业认证通过，发送邮件
elseif($act == 'set_licenseallow'){
			$templates=label_replace($mail_templates['set_licenseallow']);
			$templates_title=label_replace($mail_templates['set_licenseallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//企业认证未通过，发送邮件
elseif($act == 'set_licensenotallow'){
			$templates=label_replace($mail_templates['set_licensenotallow']);
			$templates_title=label_replace($mail_templates['set_licensenotallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//企业加入特别推荐，发送邮件
elseif($act == 'set_addmap'){
			$templates=label_replace($mail_templates['set_addmap']);
			$templates_title=label_replace($mail_templates['set_addmap_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//简历通过审核，发送邮件
elseif($act == 'set_resumeallow'){
			$templates=label_replace($mail_templates['set_resumeallow']);
			$templates_title=label_replace($mail_templates['set_resumeallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//简历未通过审核，发送邮件
elseif($act == 'set_resumenotallow'){
			$templates=label_replace($mail_templates['set_resumenotallow']);
			$templates_title=label_replace($mail_templates['set_resumenotallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//讲师通过审核，发送邮件
elseif($act == 'set_teaallow'){
			$templates=label_replace($mail_templates['set_teaallow']);
			$templates_title=label_replace($mail_templates['set_teaallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//讲师未通过审核，发送邮件
elseif($act == 'set_teanotallow'){
			$templates=label_replace($mail_templates['set_teanotallow']);
			$templates_title=label_replace($mail_templates['set_teanotallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//课程通过审核，发送邮件
elseif($act == 'set_couallow'){
			$templates=label_replace($mail_templates['set_couallow']);
			$templates_title=label_replace($mail_templates['set_couallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//课程未通过审核，发送邮件
elseif($act == 'set_counotallow'){
			$templates=label_replace($mail_templates['set_counotallow']);
			$templates_title=label_replace($mail_templates['set_counotallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//个人在线申请课程，发送邮件
elseif($act == 'set_applycou'){
			$templates=label_replace($mail_templates['set_applycou']);
			$templates_title=label_replace($mail_templates['set_applycou_title']);
			smtp_mail($_GET['email'],$templates_title,$templates);
}
//机构下载申请，发送邮件
elseif($act == 'set_downapp'){
			$templates=label_replace($mail_templates['set_downapp']);
			$templates_title=label_replace($mail_templates['set_downapp_title']);
			smtp_mail($_GET['email'],$templates_title,$templates);
}
//猎头通过审核，发送邮件
elseif($act == 'set_hunallow'){
			$templates=label_replace($mail_templates['set_hunallow']);
			$templates_title=label_replace($mail_templates['set_hunallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//猎头未通过审核，发送邮件
elseif($act == 'set_hunnotallow'){
			$templates=label_replace($mail_templates['set_hunnotallow']);
			$templates_title=label_replace($mail_templates['set_hunnotallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//高级职位通过审核，发送邮件
elseif($act == 'set_hunjobsallow'){
			$templates=label_replace($mail_templates['set_hunjobsallow']);
			$templates_title=label_replace($mail_templates['set_hunjobsallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}
//高级职位未通过审核，发送邮件
elseif($act == 'set_hunjobsnotallow'){
			$templates=label_replace($mail_templates['set_hunjobsnotallow']);
			$templates_title=label_replace($mail_templates['set_hunjobsnotallow_title']);
			$useremail=get_user_inid($uid);
			smtp_mail($useremail['email'],$templates_title,$templates);
}

?>