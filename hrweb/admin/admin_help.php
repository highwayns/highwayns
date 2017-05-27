<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_help_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"help");
$smarty->assign('pageheader',"ヘルプ");	
$smarty->assign('act',$act);
if($act == 'list')
{
	get_token();
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	$oederbysql=" order BY a.`order` DESC";
	if ($key && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE a.title like '%{$key}%'";
		elseif ($key_type===2)$wheresql=" WHERE a.id =".intval($key);
	}	
	!empty($_GET['parentid'])? $wheresqlarr['a.parentid']=intval($_GET['parentid']):'';
	!empty($_GET['type_id'])? $wheresqlarr['a.type_id']=intval($_GET['type_id']):'';
	if (!empty($wheresqlarr)) $wheresql=wheresql($wheresqlarr);
	$joinsql=" LEFT JOIN ".table('help_category')." AS c ON a.type_id=c.id  ";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('help')." AS a ".$joinsql.$wheresql;
	$page = new page(array('total'=>$db->get_total($total_sql), 'perpage'=>$perpage));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_help($offset, $perpage,$joinsql.$wheresql.$oederbysql);
	$smarty->assign('helplist',$list);
	$smarty->assign('page',$page->show(3));	
	$smarty->display('help/admin_help.htm');
}
elseif($act =='help_del')
{
	check_token();
	$id=$_REQUEST['id'];
	if (empty($id)) adminmsg("項目を選択してください！",1);
	$n=del_help($id);
	if ($n)
	{
	write_log("ヘルプ削除 削除行数 {$n} 行！", $_SESSION['admin_name'],3);
	adminmsg("削除成功 削除行数 {$n} 行！",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
elseif($act == 'add')
{
	get_token();
	$smarty->assign('category',get_help_category());	
	$smarty->display('help/admin_help_add.htm');
}
elseif($act == 'addsave')
{
	check_token();
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('タイトル入力してください！',1);
	$setsqlarr['type_id']=!empty($_POST['type_id'])?intval($_POST['type_id']):adminmsg('分類を選択してください！',1);
	$setsqlarr['content']=!empty($_POST['content'])?$_POST['content']:adminmsg('内容を入力してください！',1);
	$setsqlarr['order']=intval($_POST['order']);
	$setsqlarr['addtime']=$timestamp;
	$setsqlarr['parentid']=get_help_parentid($setsqlarr['type_id']);
	$link[0]['text'] = "続く追加";
	$link[0]['href'] = '?act=add&type_id_cn='.trim($_POST['type_id_cn'])."&type_id=".$_POST['type_id'];
	$link[1]['text'] = "一覧に戻る";
	$link[1]['href'] = '?act=list';
	write_log("ヘルプ追加：".$setsqlarr['title'], $_SESSION['admin_name'],3);
	!$db->inserttable(table('help'),$setsqlarr)?adminmsg("追加失敗！",0):adminmsg("追加成功！",2,$link);
}
elseif($act == 'edit')
{
	get_token();
	$id=intval($_GET['id']);
	$sql = "select * from ".table('help')." where id=".intval($id)." LIMIT 1";
	$help=$db->getone($sql);	
	$category=get_help_category_one($help['type_id']);
	$_GET['type_id_cn']=$category['categoryname'];
	$_GET['type_id']=$help['type_id'];
	$smarty->assign('help',$help); 	
	$smarty->assign('category',get_help_category());
	$smarty->display('help/admin_help_edit.htm');
}
elseif($act == 'editsave')
{
	check_token();
	$id=intval($_POST['id']);
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('タイトル入力してください！',1);
	$setsqlarr['type_id']=trim($_POST['type_id'])?intval($_POST['type_id']):0;
	$setsqlarr['content']=!empty($_POST['content'])?$_POST['content']:adminmsg('内容を入力してください！',1);
	$setsqlarr['order']=intval($_POST['order']);
	$setsqlarr['parentid']=get_help_parentid($setsqlarr['type_id']);
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = '?act=list';
	$link[1]['text'] = "変更結果閲覧";
	$link[1]['href'] = "?act=edit&id=".$id;
	write_log("idを次に変更".$id."のヘルプ", $_SESSION['admin_name'],3);
	!$db->updatetable(table('help'),$setsqlarr," id=".$id."")?adminmsg("変更失敗！",0):adminmsg("変更成功！",2,$link);
}
elseif($act == 'category')
{
	get_token();
	$smarty->display('help/admin_help_category.htm');
}
elseif($act == 'category_add')
{
	get_token();
	$parentid = !empty($_GET['parentid']) ? intval($_GET['parentid']) : '0';	
	$smarty->display('help/admin_help_category_add.htm');
}
elseif($act == 'add_category_save')
{
	check_token();
	$num=0;
	if (is_array($_POST['categoryname']) && count($_POST['categoryname'])>0)
	{
		for ($i =0; $i <count($_POST['categoryname']);$i++){
			if (!empty($_POST['categoryname'][$i]))
			{		
				$setsqlarr['categoryname']=trim($_POST['categoryname'][$i]);
				$setsqlarr['parentid']=intval($_POST['parentid'][$i]);
				$setsqlarr['category_order']=intval($_POST['category_order'][$i]);	
				!$db->inserttable(table('help_category'),$setsqlarr)?adminmsg("追加失敗！",0):"";
				$num=$num+$db->affected_rows();
			}

		}

	}
	if ($num==0)
	{
	adminmsg("追加失敗,データ不完全",1);
	}
	else
	{
	$link[0]['text'] = "分類管理に戻る";
	$link[0]['href'] = '?act=category';
	$link[1]['text'] = "続く分類追加";
	$link[1]['href'] = "?act=category_add";
	write_log("ヘルプ分類追加，追加件数".$num."件分類", $_SESSION['admin_name'],3);
	adminmsg("追加成功！追加件数".$num."件分類",2,$link);
	}
}
elseif($act == 'del_category')
{
	check_permissions($_SESSION['admin_purview'],"article_category");
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_category($id))
	{
	write_log("ヘルプ分類削除,削除件数 {$num} 件分類", $_SESSION['admin_name'],3);
	adminmsg("削除成功！削除件数 {$num} 件分類",2);
	}
	else
	{
	adminmsg("削除失敗！",1);
	}
}
elseif($act == 'edit_category')
{	
	$id=intval($_GET['id']);
	$smarty->assign('category',get_help_category_one($id));
	get_token();
	$smarty->display('help/admin_help_category_edit.htm');
}
elseif($act == 'edit_category_save')
{
	check_token();
	$id=intval($_POST['id']);
	$setsqlarr['parentid']=trim($_POST['parentid'])?intval($_POST['parentid']):0;
	$setsqlarr['categoryname']=trim($_POST['categoryname'])?trim($_POST['categoryname']):adminmsg('分類名称を入力してください！',1);
	$setsqlarr['category_order']=!empty($_POST['category_order'])?intval($_POST['category_order']):0;	
	$link[0]['text'] = "変更結果閲覧";
	$link[0]['href'] = '?act=edit_category&id='.$id;
	$link[1]['text'] = "分類管理に戻る";
	$link[1]['href'] = '?act=category';
	write_log("idを次に変更".$id."のヘルプ分類", $_SESSION['admin_name'],3);
	!$db->updatetable(table('help_category'),$setsqlarr," id='{$id}'")?adminmsg("変更失敗！",0):adminmsg("変更成功！",2,$link);
}
?>
