<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_crons_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"crons");
$smarty->assign('pageheader',"計画タスク");
if($act == 'list')
{
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$total_sql="SELECT COUNT(*) AS num FROM ".table('crons');
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_crons($offset,$perpage,$wheresql.$oederbysql);
	$smarty->assign('list',$list);
	$smarty->assign('page',$page->show(3));
	$smarty->assign('navlabel',"list");
	get_token();
	$smarty->display('crons/admin_crons.htm');
}
elseif($act == 'add')
{
	get_token();
	$smarty->assign('navlabel',"add");
	$smarty->display('crons/admin_crons_add.htm');
}
elseif($act == 'add_save')
{
	check_token();
$setsqlarr['name']=!empty($_POST['name'])?trim($_POST['name']):adminmsg('名称が必須',1);
$setsqlarr['filename']=!empty($_POST['filename'])?trim($_POST['filename']):adminmsg('タスクスクリプトを入力してください',1);
$setsqlarr['weekday']=intval($_POST['weekday']);
$setsqlarr['day']=intval($_POST['day']);
$setsqlarr['hour']=trim($_POST['hour']);
$setsqlarr['minute']=trim($_POST['minute']);
if (!empty($setsqlarr['minute']) && !preg_match("/^(\d{1,10},)*(\d{1,10})$/",$setsqlarr['minute']))
{
$setsqlarr['minute']=intval($setsqlarr['minute'])>60?60:intval($setsqlarr['minute']);
}
$setsqlarr['admin_set']=0;
$setsqlarr['available']=intval($_POST['available']);
	if ($db->inserttable(table('crons'),$setsqlarr))
	{
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] ="?act=";
	write_log("計画タスク追加：".$setsqlarr['name'], $_SESSION['admin_name'],3);
	adminmsg("追加成功！",2,$link);
	}
	else
	{
	adminmsg("追加失敗！",0);
	}
}
elseif($act == 'edit')
{
	get_token();
	$smarty->assign('show',get_crons_one(intval($_GET['id'])));
	$smarty->display('crons/admin_crons_edit.htm');
}
elseif($act == 'edit_save')
{
	check_token();
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] ="?act=";
	$setsqlarr['name']=!empty($_POST['name'])?trim($_POST['name']):adminmsg('名称が必須',1);
	$setsqlarr['filename']=!empty($_POST['filename'])?trim($_POST['filename']):adminmsg('タスクスクリプトを入力してください',1);
	$setsqlarr['weekday']=intval($_POST['weekday']);
	$setsqlarr['day']=intval($_POST['day']);
	$setsqlarr['hour']=intval($_POST['hour']);
	$setsqlarr['minute']=trim($_POST['minute']);
	if (!empty($setsqlarr['minute']) && !preg_match("/^(\d{1,10},)*(\d{1,10})$/",$setsqlarr['minute']))
	{
	$setsqlarr['minute']=intval($setsqlarr['minute'])>60?60:intval($setsqlarr['minute']);
	}
	$setsqlarr['available']=intval($_POST['available']);
	$wheresql=" cronid=".intval($_POST['cronid']);
	!$db->updatetable(table('crons'),$setsqlarr,$wheresql)?adminmsg("変更失敗！",0):adminmsg("変更成功！",2,$link);
}
elseif($act == 'del')
{
	get_token();
	$id=$_REQUEST['id'];
	if (empty($id)) adminmsg("項目を選択してください！",0);
	if ($num=del_crons($id))
	{
	write_log("計画タスク削除,削除件数".$num."行", $_SESSION['admin_name'],3);
	adminmsg("削除成功！削除件数".$num."行",2);
	}
	else
	{
	adminmsg("削除失敗！".$num,1);
	}
}
elseif($act == 'execution')
{
	check_token();
	$id=intval($_GET['id']);
	$crons=$db->getone("select * from ".table('crons')." WHERE  cronid='{$id}' LIMIT 1 ");
	if (!empty($crons))
	{
		if (!file_exists(HIGHWAY_ROOT_PATH."include/crons/".$crons['filename']))
		{
		adminmsg("タスクファイル {$crons['filename']} が存在しません！",0);
		}
	require_once(HIGHWAY_ROOT_PATH."include/crons/".$crons['filename']);
	adminmsg("実行成功！",2);
	}	
}
?>
