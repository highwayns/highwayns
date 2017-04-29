<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__) . '/company_common.php');
$smarty->assign('leftmenu',"index");
if ($act=='report')
{
	$smarty->assign('title','情報報告 - 企業会員センター - '.$_CFG['site_name']);
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
	$setsqlarr['content']=trim($_POST['content'])?trim($_POST['content']):showmsg('説明を入力してください！',1);
	$setsqlarr['resume_id']=$_POST['resume_id']?intval($_POST['resume_id']):showmsg('履歴書IDなし',1);
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):showmsg('履歴書がありません',1);
	$setsqlarr['resume_addtime']=intval($_POST['resume_addtime']);
	$setsqlarr['uid']=$_SESSION['uid'];
	$setsqlarr['addtime']=time();
	write_memberslog($_SESSION['uid'],2,7003,$_SESSION['username'],"举报简历({$_POST['resume_id']})");
	!$db->inserttable(table('report_resume'),$setsqlarr)?showmsg("举报失败！",0,$link):showmsg("举报成功，管理员会认真处理！",2,$link);
}
unset($smarty);
?>
