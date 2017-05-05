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
	$link[0]['text'] = "前頁へ！";
	$link[0]['href'] = $_POST['url'];
	if (check_resume_report($_SESSION['uid'],$_POST['resume_id']))
	{
	showmsg("この履歴書を報告済み！",1,$link);
	}
	$setsqlarr['content']=trim($_POST['content'])?trim($_POST['content']):showmsg('説明を入力してください！',1);
	$setsqlarr['resume_id']=$_POST['resume_id']?intval($_POST['resume_id']):showmsg('履歴書IDなし',1);
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):showmsg('履歴書がありません',1);
	$setsqlarr['resume_addtime']=intval($_POST['resume_addtime']);
	$setsqlarr['uid']=$_SESSION['uid'];
	$setsqlarr['addtime']=time();
	write_memberslog($_SESSION['uid'],2,7003,$_SESSION['username'],"履歴書報告({$_POST['resume_id']})");
	!$db->inserttable(table('report_resume'),$setsqlarr)?showmsg("報告失敗！",0,$link):showmsg("報告成功，管理者処理を待ってください！",2,$link);
}
unset($smarty);
?>
