<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_explain_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
$smarty->assign('act',$act);
$smarty->assign('pageheader',"説明ページ");
if($act == 'list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"explain_show");
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$oederbysql=" order BY e.show_order DESC,e.id DESC ";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if ($key && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE e.title like '%{$key}%'";
		$oederbysql="";
	}
	!empty($_GET['type_id'])? $wheresqlarr['e.type_id']=intval($_GET['type_id']):'';
	if (is_array($wheresqlarr)) $wheresql=wheresql($wheresqlarr);
	if (!empty($_GET['settr']))
	{
		$settr=strtotime("-".intval($_GET['settr'])." day");
		$wheresql=empty($wheresql)?" WHERE e.addtime> ".$settr:$wheresql." AND e.addtime> ".$settr;
	}
	
	$joinsql=" LEFT JOIN ".table('explain_category')." AS c ON e.type_id=c.id  ";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('explain')." AS e ".$joinsql.$wheresql;
	$page = new page(array('total'=>$db->get_total($total_sql), 'perpage'=>$perpage));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$explain_list = get_explain($offset, $perpage,$joinsql.$wheresql.$oederbysql);
	$smarty->assign('get_explain_category',get_explain_category());
	$smarty->assign('explain_list',$explain_list);
	$smarty->assign('page',$page->show(3));
	$smarty->assign('navlabel',"list");
	$smarty->display('explain/admin_explain.htm');
}
elseif($act == 'edit')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"explain_edit");
	$id = !empty($_GET['id']) ? intval($_GET['id']) : '';
	$sql = "select * from ".table('explain')." where id=".$id." LIMIT 1";
	$edit_article=$db->getone($sql);
	$smarty->assign('edit_article',$edit_article);//读取指定ID的说明页
	$smarty->assign('get_explain_category',get_explain_category());
	$smarty->display('explain/admin_explain_edit.htm');
}
elseif($act == 'editsave')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"explain_edit");
	$id = !empty($_POST['id']) ? intval($_POST['id']) : adminmsg('パラメータエラー',1);
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('説明ページの名称を入力してください！',1);
	$setsqlarr['type_id']=trim($_POST['type_id'])?intval($_POST['type_id']):0;
	$setsqlarr['content']=trim($_POST['content']);
	$setsqlarr['tit_color']=trim($_POST['tit_color']);
	$setsqlarr['tit_b']=intval($_POST['tit_b']);
	$setsqlarr['is_display']=intval($_POST['is_display']);
	$setsqlarr['is_url']=trim($_POST['is_url']);
	$setsqlarr['seo_keywords']=trim($_POST['seo_keywords']);
	$setsqlarr['seo_description']=trim($_POST['seo_description']);
	$setsqlarr['show_order']=intval($_POST['show_order']);
	$link[0]['text'] = "説明ページ一覧に戻る";
	$link[0]['href'] = '?';
	$link[1]['text'] = "変更済み説明ページを閲覧";
	$link[1]['href'] = "?act=edit&id=".$id;
	write_log("idを次に変更".$id."の説明ページ", $_SESSION['admin_name'],3);
 	!$db->updatetable(table('explain'),$setsqlarr," id=".$id."")?adminmsg("変更失敗！",0):adminmsg("変更成功！",2,$link);
}
elseif($act == 'add')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"explain_add");
	$smarty->assign('ty_id',$_GET['ty_id']);
	$smarty->assign('get_explain_category',get_explain_category());
	$smarty->assign('navlabel',"add");
	$smarty->display('explain/admin_explain_add.htm');
}
elseif($act == 'addsave')
{	
	check_token();
	check_permissions($_SESSION['admin_purview'],"explain_add");
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('説明ページの名称を入力してください！',1);
	$setsqlarr['type_id']=trim($_POST['type_id'])?intval($_POST['type_id']):0;
	$setsqlarr['content']=trim($_POST['content']);
	$setsqlarr['tit_color']=trim($_POST['tit_color']);
	$setsqlarr['tit_b']=intval($_POST['tit_b']);
	$setsqlarr['is_display']=intval($_POST['is_display']);
	$setsqlarr['is_url']=trim($_POST['is_url']);
	$setsqlarr['seo_keywords']=trim($_POST['seo_keywords']);
	$setsqlarr['seo_description']=trim($_POST['seo_description']);
	$setsqlarr['show_order']=intval($_POST['show_order']);
	$setsqlarr['addtime']=$timestamp;
	$link[0]['text'] = "続く説明ページを追加";
	$link[0]['href'] = '?act=add&type_id='.$setsqlarr['type_id'];
	$link[1]['text'] = "説明ページ一覧に戻る";
	$link[1]['href'] = '?';
	write_log("追加説明ページ：".$setsqlarr['title'], $_SESSION['admin_name'],3);
	$insertid = $db->inserttable(table('explain'),$setsqlarr,1);
	if(!$insertid){
		adminmsg("追加失敗！",0);
	}else{
		baidu_submiturl(url_rewrite('HW_explainshow',array('id'=>$insertid)),'addexplain');
		adminmsg("追加成功！",2,$link);
	}
}
elseif($act == 'explain_del')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"explain_del");
	$id=$_REQUEST['id'];
	if ($num=del_explain($id))
	{
	adminmsg("削除成功！削除件数".$num."行",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
elseif($act == 'category')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"explain_category");
	$smarty->assign('get_explain_category',get_explain_category());
	$smarty->assign('navlabel',"category");
	$smarty->display('explain/admin_explain_category.htm');
}
elseif($act == 'category_add')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"explain_category");
	$smarty->assign('navlabel',"category");
	$smarty->display('explain/admin_explain_category_add.htm');
}
elseif($act == 'add_category_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"explain_category");
	$num=0;
	if (is_array($_POST['categoryname']) && count($_POST['categoryname'])>0)
	{
		for ($i =0; $i <count($_POST['categoryname']);$i++){
			if (!empty($_POST['categoryname'][$i]))
			{		
				$setsqlarr['categoryname']=trim($_POST['categoryname'][$i]);
				$setsqlarr['category_order']=intval($_POST['category_order'][$i]);			
				!$db->inserttable(table('explain_category'),$setsqlarr)?adminmsg("追加失敗！",0):"";
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
	$link[1]['text'] = "続く追加";
	$link[1]['href'] = "?act=category_add";
	write_log("追加成功！追加件数".$num."件分類", $_SESSION['admin_name'],3);
	adminmsg("追加成功！追加件数".$num."件分類",2,$link);
	}
}
elseif($act == 'edit_category')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"explain_category");
	$id=intval($_GET['id']);
	$smarty->assign('category',get_explain_category_one($id));
	$smarty->assign('navlabel',"category");
	$smarty->display('explain/admin_explain_category_edit.htm');
}
elseif($act == 'edit_category_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"explain_category");
	$id=intval($_POST['id']);	
	$setsqlarr['categoryname']=trim($_POST['categoryname'])?trim($_POST['categoryname']):adminmsg('分類名称を入力してください！',1);
	$setsqlarr['category_order']=intval($_POST['category_order']);
	$link[0]['text'] = "変更結果閲覧";
	$link[0]['href'] = '?act=edit_category&id='.$id;
	$link[1]['text'] = "分類管理に戻る";
	$link[1]['href'] = '?act=category';
	write_log("idを次に変更".$id."の分類", $_SESSION['admin_name'],3);
	!$db->updatetable(table('explain_category'),$setsqlarr," id=".$id."")?adminmsg("変更失敗！",0):adminmsg("変更成功！",2,$link);
}
elseif($act == 'del_category')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"explain_category");
	$id=$_REQUEST['id'];
	if ($num=del_category($id))
	{
	write_log("分類削除！削除件数".$num."行", $_SESSION['admin_name'],3);
	adminmsg("削除成功！削除件数".$num."行",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}

?>
