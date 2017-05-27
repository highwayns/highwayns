<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_syslog_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"syslog");
$smarty->assign('pageheader',"システムログ");
if($act == 'list')
{
	get_token();
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
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
	write_log("システムログ削除,削除件数".$dnum."行", $_SESSION['admin_name'],3);
	adminmsg("削除成功！削除件数".$dnum."行",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
elseif($act == 'pidel_syslog')
{
	get_token();
	$smarty->assign('pageheader',"システムエラーログ削除");
	$smarty->display('syslog/admin_syslog_del.htm');
}
elseif($act == 'pidel_syslog_del')
{
	check_token();
	$l_type=$_POST['l_type'];
	if(empty($l_type))	adminmsg('エラータイプを選択してください！',1);
	$starttime=intval(convert_datefm($_POST['starttime'],2));
	if (empty($starttime))
	{
	adminmsg('開始時間を入力してください！',1);
	}	
	$endtime=intval(convert_datefm($_POST['endtime'],2));
	if (empty($endtime))
	{
	adminmsg('終了時間を入力してください！',1);
	}	
	if($starttime >$endtime) adminmsg('開始時間が終了時間より大きい！',1);
	$link[0]['text'] = "ログ一覧に戻る";
	$link[0]['href'] = '?act=list';
	$link[1]['text'] = "続く削除";
	$link[1]['href'] = '?act=pidel_syslog';
	$dnum=pidel_syslog($l_type,$starttime,$endtime);
	if ($dnum>0)
	{
	write_log("システムログ削除,削除件数".$dnum."行", $_SESSION['admin_name'],3);
	adminmsg("削除成功！削除件数".$dnum."行",2,$link);
	}
	else
	{
	adminmsg("該当期限内ログ或削除失敗がありません,チェックしてください！",0,$link);
	}
}
?>
