<?php
define('IN_HIGHWAY', true);

require_once(dirname(__FILE__).'/../../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_personal.php');
$smarty->cache = false;
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'interview';
if ($_SESSION['uid']=='' || $_SESSION['username']==''||intval($_SESSION['utype'])==1)
{
	header("Location: ../wap_login.php");
}
elseif ($act == 'interview')
{

	$uid=intval($_SESSION["uid"]);
	$wheresql=" WHERE  i.resume_uid=$uid ";
	$perpage = 5;
	$count  = 0;
	$page = empty($_GET['page'])?1:intval($_GET['page']);
	if($page<1) $page = 1;
	$start = ($page-1)*$perpage;
	$total_sql="SELECT COUNT(*) AS num FROM  ".table('company_interview')." as i {$wheresql}";
	$count=$db->get_total($total_sql);
	$limit=" LIMIT {$start},{$perpage}";
	$smarty->assign('title','面接誘い一覧 - 個人会員センター - '.$_CFG['site_name']);

	$sql="select * from ".table("company_interview")." as i $wheresql order by i.interview_addtime ".$limit;
	$interview=$db->getall($sql);
	$smarty->assign('interview',$interview);
	$smarty->display("wap/personal/wap-interview.html");

	
}
?>
