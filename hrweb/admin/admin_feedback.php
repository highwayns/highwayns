<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_feedback_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'suggest_list';
if($act == 'suggest_list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"suggest_show");
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	!empty($_GET['infotype'])? $wheresqlarr['infotype']=intval($_GET['infotype']):'';
		if (is_array($wheresqlarr))
		{
		$where_set=' WHERE';
			foreach ($wheresqlarr as $key => $value)
			{
			$wheresql .=$where_set. $comma.'`'.$key.'`'.'=\''.$value.'\'';
			$comma = ' AND ';
			$where_set='';
			}
		}
	$total_sql="SELECT COUNT(*) AS num FROM ".table('feedback').$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_feedback_list($offset,$perpage,$wheresql);
	$smarty->assign('pageheader',"アドバイス");
	$smarty->assign('infotype',$_GET['infotype']);
	$smarty->assign('perpage',$perpage);
	$smarty->assign('list',$list);//列表
	if ($total_val>$perpage)
	{
	$smarty->assign('page',$page->show(3));//分页符
	}
	$smarty->display('feedback/admin_feedback_suggest_list.htm');
}
elseif($act == 'del_feedback')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"suggest_del");
	$id =!empty($_REQUEST['id'])?$_REQUEST['id']:adminmsg("项目を選択してください！",1);
	if ($num=del_feedback($id))
	{
	write_log("アッドベス削除,削除件数".$num."行", $_SESSION['admin_name'],3);
	adminmsg("削除成功！削除件数".$num."行",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
elseif($act == 'report_list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"report_show");
	$type=intval($_GET['type'])==0?1:intval($_GET['type']);
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$oederbysql=" order BY r.id DESC ";
	if (!empty($_GET['settr']))
	{
		$settr=strtotime("-".intval($_GET['settr'])." day");
		$wheresql=empty($wheresql)?" WHERE r.addtime> ".$settr:$wheresql." AND r.addtime> ".$settr;
	}
	$joinsql=" LEFT JOIN ".table('members')." AS m ON r.uid=m.uid  ";
	if($type==1){
		$total_sql="SELECT COUNT(*) AS num FROM ".table('report')." AS r ".$joinsql.$wheresql;
	}else{
		$total_sql="SELECT COUNT(*) AS num FROM ".table('report_resume')." AS r ".$joinsql.$wheresql;
	}
	if (!empty($_GET['reporttype']))
	{
		$wheresql=empty($wheresql)?" WHERE r.report_type=".$_GET['reporttype']:$wheresql." AND r.report_type=".$_GET['reporttype'];
	}
	if (!empty($_GET['audit']))
	{
		$wheresql=empty($wheresql)?" WHERE r.audit=".$_GET['audit']:$wheresql." AND r.audit=".$_GET['audit'];
	}
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_report_list($offset,$perpage,$joinsql.$wheresql.$oederbysql,$type);
	$smarty->assign('pageheader',"情報報告");
	$smarty->assign('list',$list);
	$smarty->assign('page',$page->show(3));
	if($type==1){
		$smarty->display('feedback/admin_report_list.htm');
	}else{
		$smarty->display('feedback/admin_report_resume_list.htm');
	}
}
elseif($act == 'report_perform')
{
	$type=intval($_POST['type'])==0?1:intval($_POST['type']);
	//审核
	if(!empty($_POST['set_audit'])){
		check_permissions($_SESSION['admin_purview'],"report_audit");
		check_token();
		$id=$_REQUEST['id'];
		if ($type==1) {
			$rid=$_REQUEST['jobs_id'];
		} else {
			$rid=$_REQUEST['resume_id'];
		}
		$audit=intval($_POST['audit']);
		if (empty($id))
		{
		adminmsg("项目を選択してください！",1);
		}
		if ($num=report_audit($id,$audit,$type,$rid))
		{
		write_log("情報報告審査状態設定，影響された行数{$num}行 ", $_SESSION['admin_name'],3);
		adminmsg("設定成功！影響行数 {$num}行 ",2);
		}
		else
		{
		adminmsg("設定失敗！",0);
		}
	}
}
elseif($act == 'del_report')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"report_del");
	$id =!empty($_REQUEST['id'])?$_REQUEST['id']:adminmsg("项目を選択してください！",1);
	$id=$_REQUEST['id'];
	if ($num=del_report($id))
	{
	write_log("情報報告削除，削除行数{$num}行 ", $_SESSION['admin_name'],3);
	adminmsg("削除成功！削除件数".$num."行",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
elseif($act == 'del_report_resume')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"report_del");
	$id =!empty($_REQUEST['id'])?$_REQUEST['id']:adminmsg("项目を選択してください！",1);
	$id=$_REQUEST['id'];
	if ($num=del_report_resume($id))
	{
	write_log("履歴書報告削除，削除件数{$num}件 ", $_SESSION['admin_name'],3);
	adminmsg("削除成功！削除件数".$num."行",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
?>
