<?php
 /*
 * 74cms 生成word
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
unset($dbhost,$dbuser,$dbpass,$dbname);
$uid=intval($_GET['uid']);//简历所属会员的uid
$id=intval($_GET['resume_id']);
if ($_SESSION['uid']=='' || $_SESSION['username']=='')
{
	$resume_url=url_rewrite('QS_resumeshow',array('id'=>$id));
    header("Location:".url_rewrite('QS_login')."?url={$resume_url}");
	exit();
}
if(($_SESSION['utype']=='2' && $_SESSION['uid']==$uid) || $_SESSION['utype']=='1'){
	$flag=true;
}else{
	$flag=false;
}
if(!$flag) {showmsg('您没有权限！只有个人用户和企业用户可以转换简历',1);exit();}
$wheresql=" WHERE  id='{$id}'  AND uid='{$uid}' ";
$sql = "select * from ".table('resume').$wheresql." LIMIT  1";
$val=$db->getone($sql);
if ($val)
{
	$val['education_list']=get_this_education($val['uid'],$val['id']);
	$val['work_list']=get_this_work($val['uid'],$val['id']);
	$val['training_list']=get_this_training($val['uid'],$val['id']);
	$val['age']=date("Y")-$val['birthdate'];
	$val['tagcn']=preg_replace("/\d+/", '',$val['tag']);
	$val['tagcn']=preg_replace('/\,/','',$val['tagcn']);
	$val['tagcn']=preg_replace('/\|/','&nbsp;&nbsp;&nbsp;',$val['tagcn']);
 	if ($val['display_name']=="2")
	{
		$val['fullname']="N".str_pad($val['id'],7,"0",STR_PAD_LEFT);
		$val['fullname_']=$val['fullname'];		
	}
	elseif($val['display_name']=="3")
	{
		if($val['sex']==1)
		{
			$val['fullname']=cut_str($val['fullname'],1,0,"先生");
		}
		elseif($val['sex']==2)
		{
			$val['fullname']=cut_str($val['fullname'],1,0,"女士");
		}
		$val['fullname_']=$val['fullname'];	
	}
	else
	{
		$val['fullname_']=$val['fullname'];
		$val['fullname']=$val['fullname'];
	}
	if(intval($_GET['apply'])==1){
		$set_apply = 1;
		$apply = $db->getone("select * from ".table('personal_jobs_apply')." where `resume_id`=".$val['id']);
		$val['jobs_name'] = $apply['jobs_name'];
	}else{
		$set_apply = 0;
	}
	// 最近登录时间
	$userinfo=$db->getone("select last_login_time from ".table('members')." where uid=$val[uid]");
	$last_login_time=date('Y-m-d',$userinfo['last_login_time']);
	$htm='<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv=Content-Type  content="text/html; charset=gb2312" >
	<title></title>
	<style>

	.table_tit{border-bottom: 2px solid #000;font-size: 14px;font-weight: bold;padding:10px 0;}
	.fl{float: left;}
	.fr{float: right;}
	</style>
</head>
<body>
<table width="700" border="0" align="center" cellpadding="10" cellspacing="0" style="background-color: #FAF7C1;padding:10px 0">
		<tr>
			<td align="center">简历标题</td>
			<td align="center">'.$val['title'].'</td>
			<td align="center">最近登录</td>
			<td align="center">'.$last_login_time.'</td>
			<td align="center"><img width="130" height="40" src="'.$_CFG["upfiles_dir"].$_CFG["web_logo"].'" alt="{#$QISHI.site_name#}" border="0" align="absmiddle"></td>
		</tr>
	</table>';
// if($set_apply==1){
// 	if($val['jobs_name']!=""){
// 		$htm .= '<tr>
// 	    <td colspan="1">申请职位：'.$val["jobs_name"].'</td>
// 	  </tr>';
// 	}
// }

// 个人信息
if($_CFG['showresumecontact']=='1' || $_CFG['showresumecontact']=='0')
{
	$val['fullname']=$val['fullname'];
	$val['telephone']=$val['telephone'];
	$val['email']=$val['email'];
}
elseif($_CFG['showresumecontact']=='2')//联系方式：会员下载后可见
{
	if ($_SESSION['uid'] && $_SESSION['username'] && $_SESSION['utype']=='1')
	{
		$sql = "select did from ".table('company_down_resume')." WHERE company_uid = {$_SESSION['uid']} AND resume_id='{$id}' LIMIT 1";
		$info=$db->getone($sql);
		if (!empty($info))
		{
			$val['fullname']=$val['fullname'];
			$val['telephone']=$val['telephone'];
			$val['email']=$val['email'];
		}
		else
		{
			$val['fullname']=$val['fullname'];
			$val['telephone']="下载后可见";
			$val['email']="下载后可见";
		}
	}elseif($_SESSION['utype']=='2' && $_SESSION['uid']==$uid){
		$val['fullname']=$val['fullname'];
		$val['telephone']=$val['telephone'];
		$val['email']=$val['email'];
	}else{
			$val['fullname']=$val['fullname'];
			$val['telephone']="下载后可见";
			$val['email']="下载后可见";
	}
}
if ($val['photo']=="1")
{
$val['photosrc']=$_CFG['resume_photo_dir_thumb'].$val['photo_img'];
}
else
{
$val['photosrc']=$_CFG['resume_photo_dir_thumb']."no_photo.gif";
}
$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0">
		<tr>
			<td class="table_tit">个人信息</td>
		</tr>
	</table>
	<table width="700" border="0" align="center" cellpadding="6" cellspacing="0" style="font-size: 12px;">
		<tr>
			<td align="right" width="100">姓名：</td>
			<td align="left" width="200">'.$val['fullname'].'</td>
			<td align="right" width="100">性别：</td>
			<td align="left" width="200">'.$val['sex_cn'].'</td>
			<td rowspan=5 align="center">
				<img width="100" height="100" src="'.$val['photosrc'].'">
			</td>
		</tr>
		<tr>
			<td align="right" width="100" >手机号码：</td>
			<td align="left" width="200">'.$val['telephone'].'</td>
			<td align="right" width="100">年龄：</td>
			<td align="left" width="200">'.$val['age'].'岁</td>
		</tr>
		<tr>
			<td align="right" width="100">电子邮件：</td>
			<td align="left" width="200">'.$val['email'].'</td>
			<td align="right" width="100">教育程度：</td>
			<td align="left" width="200">'.$val['education_cn'].'</td>
		</tr>
		<tr>
			<td align="right" width="100">工作年限：</td>
			<td align="left" width="200">'.$val['experience_cn'].'</td>
			<td align="right" width="100">婚姻状况：</td>
			<td align="left" width="200">'.$val['marriage_cn'].'</td>
		</tr>
		<tr>
			<td align="right" width="100">籍贯：</td>
			<td align="left" width="200">'.$val['householdaddress'].'</td>
			<td align="right" width="100">目前所在地：</td>
			<td align="left" width="200">'.$val['residence'].'</td>
		</tr>
	</table>';
// 求职意向

$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0">
		<tr>
			<td class="table_tit">求职意向</td>
		</tr>
	</table>
	<table width="700" border="0" align="center" cellpadding="6" cellspacing="0" style="font-size: 12px;">
			<tr>
				<td align="right" width="100">期望行业：</td>
				<td  align="left">'.$val['trade_cn'].'</td>
			</tr>
			<tr>
				<td align="right" width="100">期望职位：</td>
				<td align="left">'.$val['intention_jobs'].'</td>
			</tr>
			<tr>
				<td align="right" width="100">期望地点：</td>
				<td align="left">'.$val['district_cn'].'</td>
			</tr>
			<tr>
				<td align="right" width="100">期望薪资：</td>
				<td align="left">'.$val['wage_cn'].'</td>
			</tr>
		</table>';
// 工作经历
$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0">
		<tr>
			<td class="table_tit">工作经历</td>
		</tr>
	</table>';
if($val['work_list'])
{
	foreach ($val['work_list'] as $wli)
	{  
		$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0" style="font-size: 12px;padding-top: 20px;">
		<tr>
			<td align="right" width="100">'.$wli['startyear'].'.'.$wli['startmonth'].'-'.$wli['endyear'].'.'.$wli['endmonth'].'</td>
			<td align="left">'.$wli['companyname'].'</td>
		</tr>
		<tr>
			<td width="100">&nbsp;</td>
			<td >
				<span class="fl w100">职位名称：</span>
				<span class="fl">'.$wli['jobs'].'</span>
			</td>
		</tr>
		<tr>
			<td width="100">&nbsp;</td>
			<td >
				<span class="fl w100">工作职责：</span>
				<span class="fl">
				'.$wli['achievements'].'
				</span>
			</td>
		</tr>
	</table>';
	}
}
else
{
 $htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0" style="font-size: 12px;padding-top: 20px;"><tr>没有填写工作经历</tr></table>';
}
// 培训经历
$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0">
		<tr>
			<td class="table_tit">培训经历</td>
		</tr>
	</table>
';
if($val['training_list'])
{
	foreach ($val['training_list'] as $tli)
	{
		$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0" style="font-size: 12px;padding-top: 20px;">
		<tr>
			<td class="w100 td_left td_bgc">'.$tli['startyear'].'.'.$tli['startmonth'].'-'.$tli['endyear'].'.'.$tli['endmonth'].'</td>
			<td class="td_left td_bgc">'.$tli['agency'].'</td>
		</tr>
		<tr>
			<td width="100">&nbsp;</td>
			<td >
				培训课程：'.$tli['course'].'
			</td>
		</tr>
		<tr>
			<td width="100">&nbsp;</td>
			<td >
				<span class="fl w100">培训介绍：</span>
				<span class="fl">
					'.$tli['description'].'
				</span>
			</td>
		</tr>
	</table>';
	}
}
else
{
	$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0" style="font-size: 12px;padding-top: 20px;">
		<tr>没有填写培训经历</tr></table>';
}
// 教育经历
$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0">
		<tr>
			<td class="table_tit">教育经历</td>
		</tr>
	</table>';
if($val['education_list']){
	foreach ($val['education_list'] as $eli)
	{
		$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0" style="font-size: 12px;padding-top: 20px;">
		<tr>
			<td style="font-weight: bold;">'.$eli['school'].'</td>
			<td>'.$eli['startyear'].'.'.$eli['startmonth'].'-'.$eli['endyear'].'.'.$eli['endmonth'].'</td>
			<td>'.$eli['speciality'].'</td>
			<td>'.$eli['education_cn'].'</td>
		</tr>
	</table>';
	}
}
else
{
	$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0" style="font-size: 12px;padding-top: 20px;">
		<tr>没有填写教育经历</tr></table>';
}
// 自我描述
$htm.='<table width="700" border="0" align="center" cellpadding="10" cellspacing="0">
		<tr>
			<td class="table_tit">自我描述</td>
		</tr>
	</table>
	<table width="700" border="0" align="center" cellpadding="10" cellspacing="0" style="font-size: 12px;padding-top: 20px;">
		<tr>
			<td>'.nl2br($val['specialty']).'</td>
		</tr>
	</table>';
$htm.="<div align=\"center\"><br />
	<a title=\"{$_CFG['site_name']}\" href=\"{$_CFG['site_domain']}{$_CFG['site_dir']}\">{$_CFG['site_name']}</a>
</div>
</body>
</html>";
header("Cache-Control: no-cache, must-revalidate"); 
header("Pragma: no-cache");   
header("Content-Type: application/doc"); 
header("Content-Disposition:attachment; filename={$val['fullname']}的个人简历.doc"); 
echo $htm;
}
else
{
 showmsg('简历不存在！',1);
 exit();
}
function get_this_education($uid,$pid)
{
	global $db;
	$sql = "SELECT * FROM ".table('resume_education')." WHERE uid='".intval($uid)."' AND pid='".intval($pid)."' ";
	return $db->getall($sql);
}
function get_this_work($uid,$pid)
{
	global $db;
	$sql = "select * from ".table('resume_work')." where uid=".intval($uid)." AND pid='".$pid."' " ;
	return $db->getall($sql);
}
function get_this_training($uid,$pid)
{
	global $db;
	$sql = "select * from ".table('resume_training')." where uid='".intval($uid)."' AND pid='".intval($pid)."'";
	return $db->getall($sql);
}
function get_user_setmealt($uid)
{
	global $db;
	$sql = "select * from ".table('members_setmeal')."  WHERE uid=".intval($uid)." AND  effective=1 LIMIT 1";
	return $db->getone($sql);
}
?>