<?php
 /*
 * 74cms 投诉与建议
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_feedback_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'suggest_list';
if($act == 'suggest_list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"suggest_show");
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
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
	$smarty->assign('pageheader',"意见和建议");
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
	$id =!empty($_REQUEST['id'])?$_REQUEST['id']:adminmsg("你没有选择项目！",1);
	if ($num=del_feedback($id))
	{
	write_log("删除意见建议,共删除".$num."行", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",0);
	}
}
elseif($act == 'report_list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"report_show");
	$type=intval($_GET['type'])==0?1:intval($_GET['type']);
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
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
	$smarty->assign('pageheader',"举报信息");
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
		adminmsg("您没有选择项目！",1);
		}
		if ($num=report_audit($id,$audit,$type,$rid))
		{
		write_log("设置举报信息审核状态，共影响{$num}行 ", $_SESSION['admin_name'],3);
		adminmsg("设置成功！共影响 {$num}行 ",2);
		}
		else
		{
		adminmsg("设置失败！",0);
		}
	}
}
elseif($act == 'del_report')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"report_del");
	$id =!empty($_REQUEST['id'])?$_REQUEST['id']:adminmsg("你没有选择项目！",1);
	$id=$_REQUEST['id'];
	if ($num=del_report($id))
	{
	write_log("删除举报信息，共删除{$num}行 ", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",0);
	}
}
elseif($act == 'del_report_resume')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"report_del");
	$id =!empty($_REQUEST['id'])?$_REQUEST['id']:adminmsg("你没有选择项目！",1);
	$id=$_REQUEST['id'];
	if ($num=del_report_resume($id))
	{
	write_log("删除举报简历信息，共删除{$num}行 ", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",0);
	}
}
?>