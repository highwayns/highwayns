<?php
 /*
 * 74cms 系统日志
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
require_once(ADMIN_ROOT_PATH.'include/admin_syslog_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"syslog");
$smarty->assign('pageheader',"系统日志");
if($act == 'list')
{
	get_token();
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$wheresql="";
	$oederbysql=" order BY l_id DESC ";
	if (isset($_GET['l_type']) && !empty($_GET['l_type']))
	{
		$wheresql=" WHERE l_type='".intval($_GET['l_type'])."'";
	}
	if (isset($_GET['settr']) && !empty($_GET['settr']))
	{
		$settr=strtotime("-".intval($_GET['settr'])." day");
		$wheresql=empty($wheresql)?" WHERE l_time> ".$settr:$wheresql." AND l_time> ".$settr;
	}
	$total_sql="SELECT COUNT(*) AS num FROM ".table('syslog').$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_syslog_list($offset,$perpage,$wheresql.$oederbysql);
	$smarty->assign('list',$list);
	$smarty->assign('page',$page->show(3));
	$smarty->display('syslog/admin_syslog_list.htm');
}
elseif($act == 'del_syslog')
{
	check_token();
	$id=$_REQUEST['id'];
	$dnum=del_syslog($id);
	if ($dnum>0)
	{
	write_log("删除系统日志,共删除".$dnum."行", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除".$dnum."行",2);
	}
	else
	{
	adminmsg("删除失败！",0);
	}
}
elseif($act == 'pidel_syslog')
{
	get_token();
	$smarty->assign('pageheader',"删除系统错误日志");
	$smarty->display('syslog/admin_syslog_del.htm');
}
elseif($act == 'pidel_syslog_del')
{
	check_token();
	$l_type=$_POST['l_type'];
	if(empty($l_type))	adminmsg('请选择错误类型！',1);
	$starttime=intval(convert_datefm($_POST['starttime'],2));
	if (empty($starttime))
	{
	adminmsg('请填写开始时间！',1);
	}	
	$endtime=intval(convert_datefm($_POST['endtime'],2));
	if (empty($endtime))
	{
	adminmsg('请填写结束时间！',1);
	}	
	if($starttime >$endtime) adminmsg('开始时间不能大于结束时间！',1);
	$link[0]['text'] = "返回日志列表";
	$link[0]['href'] = '?act=list';
	$link[1]['text'] = "继续删除";
	$link[1]['href'] = '?act=pidel_syslog';
	$dnum=pidel_syslog($l_type,$starttime,$endtime);
	if ($dnum>0)
	{
	write_log("删除系统日志,共删除".$dnum."行", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除".$dnum."行",2,$link);
	}
	else
	{
	adminmsg("该日期段没有日志或删除失败,请检查！",0,$link);
	}
}
?>