<?php
 /*
 * 74cms 热门关键字
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
require_once(ADMIN_ROOT_PATH.'include/admin_hotword_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
$smarty->assign('act',$act);
check_permissions($_SESSION['admin_purview'],"hotword");
$smarty->assign('pageheader',"热门关键词");
if($act == 'list')
{	
	get_token();
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
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
	$setsqlarr['w_word']=trim($_POST['w_word'])?trim($_POST['w_word']):adminmsg('关键词必须填写！',1);
	$setsqlarr['w_hot']=intval($_POST['w_hot']);
	if (get_hotword_obtainword($setsqlarr['w_word']))
	{
	adminmsg("关键词已经存在！",0);
	}
	$link[0]['text'] = "继续添加";
	$link[0]['href'] = '?act=add&w_type='.$setsqlarr['w_type'];
	$link[1]['text'] = "返回列表";
	$link[1]['href'] = '?';
	write_log("添加热门关键字", $_SESSION['admin_name'],3);
	!$db->inserttable(table('hotword'),$setsqlarr)?adminmsg("添加失败！",0):adminmsg("添加成功！",2,$link);
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
	$id = !empty($_POST['id']) ? intval($_POST['id']) : adminmsg('参数错误',1);
	$setsqlarr['w_word']=trim($_POST['w_word'])?trim($_POST['w_word']):adminmsg('关键词必须填写！',1);
	$setsqlarr['w_hot']=intval($_POST['w_hot']);
	$word=get_hotword_obtainword($setsqlarr['w_word']);
	if ($word['w_id'] && $word['w_id']<>$id)
	{
	adminmsg("关键词已经存在！",0);
	}
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?';
	write_log("修改热门关键字", $_SESSION['admin_name'],3);
 	!$db->updatetable(table('hotword'),$setsqlarr," w_id=".$id."")?adminmsg("修改失败！",0):adminmsg("修改成功！",2,$link);
}
elseif($act == 'hottype_del')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_hottype($id))
	{
	write_log("删除热门关键字,共删除 {$num} 行", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除 {$num} 行",2);
	}
	else
	{
	adminmsg("删除失败！",0);
	}
}
?>