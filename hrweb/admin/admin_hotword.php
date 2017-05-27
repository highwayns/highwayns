<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_hotword_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
$smarty->assign('act',$act);
check_permissions($_SESSION['admin_purview'],"hotword");
$smarty->assign('pageheader',"ホットキーワード");
if($act == 'list')
{	
	get_token();
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$oederbysql=" order BY w_hot DESC ";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	if ($key)
	{
		$wheresql=" WHERE w_word like '%{$key}%'";
	}
	$total_sql="SELECT COUNT(*) AS num FROM ".table('hotword')." ".$wheresql;
	$page = new page(array('total'=>$db->get_total($total_sql),'perpage'=>$perpage));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$hotword = get_hotword($offset, $perpage,$wheresql.$oederbysql);	
	$smarty->assign('hotword',$hotword);
	$smarty->assign('navlabel',"list");	
	$smarty->assign('page',$page->show(3));	
	$smarty->display('hotword/admin_hotword_list.htm');
}
elseif($act == 'add')
{
	get_token();
	$smarty->assign('navlabel',"add");	
	$smarty->display('hotword/admin_hotword_add.htm');
}
elseif($act == 'addsave')
{
	check_token();
	$setsqlarr['w_word']=trim($_POST['w_word'])?trim($_POST['w_word']):adminmsg('キーワードが必須！',1);
	$setsqlarr['w_hot']=intval($_POST['w_hot']);
	if (get_hotword_obtainword($setsqlarr['w_word']))
	{
	adminmsg("キーワードが既に存在します！",0);
	}
	$link[0]['text'] = "続く追加";
	$link[0]['href'] = '?act=add&w_type='.$setsqlarr['w_type'];
	$link[1]['text'] = "一覧に戻る";
	$link[1]['href'] = '?';
	write_log("追加されたホットキーワード", $_SESSION['admin_name'],3);
	!$db->inserttable(table('hotword'),$setsqlarr)?adminmsg("追加失敗！",0):adminmsg("追加成功！",2,$link);
}
elseif($act == 'edit')
{
	get_token();
	$smarty->assign('hotword',get_hotword_one($_GET['id']));
	$smarty->display('hotword/admin_hotword_edit.htm');
}
elseif($act == 'editsave')
{
	check_token();
	$id = !empty($_POST['id']) ? intval($_POST['id']) : adminmsg('パラメータエラー',1);
	$setsqlarr['w_word']=trim($_POST['w_word'])?trim($_POST['w_word']):adminmsg('キーワードが必須！',1);
	$setsqlarr['w_hot']=intval($_POST['w_hot']);
	$word=get_hotword_obtainword($setsqlarr['w_word']);
	if ($word['w_id'] && $word['w_id']<>$id)
	{
	adminmsg("キーワードが既に存在します！",0);
	}
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = '?';
	write_log("ホットキーワード変更", $_SESSION['admin_name'],3);
 	!$db->updatetable(table('hotword'),$setsqlarr," w_id=".$id."")?adminmsg("変更失敗！",0):adminmsg("変更成功！",2,$link);
}
elseif($act == 'hottype_del')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_hottype($id))
	{
	write_log("ホットキーワード削除,削除行数 {$num} 行", $_SESSION['admin_name'],3);
	adminmsg("削除成功！削除行数 {$num} 行",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
?>
