<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__) . '/personal_common.php');
$smarty->assign('leftmenu',"index");
if ($act=='report')
{
	$smarty->assign('title','情報報告 - 個人会員センター - '.$_CFG['site_name']);
	$smarty->assign('url',$_SERVER['HTTP_REFERER']);
	$smarty->display('member_personal/personal_report.htm');
}
//保存举报信息
elseif ($act=='report_save')
{
	$link[0]['text'] = "前頁へ！";
	$link[0]['href'] = $_POST['url'];
	if (check_jobs_report($_SESSION['uid'],$_POST['jobs_id']))
	{
	showmsg("この職位すでに報告しました！",1,$link);
	}
	$setsqlarr['content']=trim($_POST['content'])?trim($_POST['content']):showmsg('説明を入力してください！',1);
	$setsqlarr['jobs_id']=$_POST['jobs_id']?intval($_POST['jobs_id']):showmsg('職位IDなし',1);
	$setsqlarr['jobs_name']=trim($_POST['jobs_name'])?trim($_POST['jobs_name']):showmsg('職位名称を入力してください',1);
	$setsqlarr['jobs_addtime']=intval($_POST['jobs_addtime']);
	$setsqlarr['uid']=$_SESSION['uid'];
	$setsqlarr['addtime']=time();
	write_memberslog($_SESSION['uid'],2,7003,$_SESSION['username'],"職位({$_POST['jobs_id']})報告");
	!$db->inserttable(table('report'),$setsqlarr)?showmsg("報告失敗！",0,$link):showmsg("報告成功，管理者処理を待ってください！",2,$link);
}
unset($smarty);
?>
