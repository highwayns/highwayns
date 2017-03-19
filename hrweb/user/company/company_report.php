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
require_once(dirname(__FILE__) . '/company_common.php');
$smarty->assign('leftmenu',"index");
if ($act=='report')
{
	$smarty->assign('title','举报信息 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->assign('url',$_SERVER['HTTP_REFERER']);
	$smarty->display('member_company/company_report.htm');
}
//保存举报信息
elseif ($act=='report_save')
{
	$link[0]['text'] = "返回上一页！";
	$link[0]['href'] = $_POST['url'];
	if (check_resume_report($_SESSION['uid'],$_POST['resume_id']))
	{
	showmsg("您已经举报过此简历！",1,$link);
	}
	$setsqlarr['content']=trim($_POST['content'])?trim($_POST['content']):showmsg('请输入相关描述！',1);
	$setsqlarr['resume_id']=$_POST['resume_id']?intval($_POST['resume_id']):showmsg('没有简历ID',1);
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):showmsg('没有简历',1);
	$setsqlarr['resume_addtime']=intval($_POST['resume_addtime']);
	$setsqlarr['uid']=$_SESSION['uid'];
	$setsqlarr['addtime']=time();
	write_memberslog($_SESSION['uid'],2,7003,$_SESSION['username'],"举报简历({$_POST['resume_id']})");
	!$db->inserttable(table('report_resume'),$setsqlarr)?showmsg("举报失败！",0,$link):showmsg("举报成功，管理员会认真处理！",2,$link);
}
unset($smarty);
?>