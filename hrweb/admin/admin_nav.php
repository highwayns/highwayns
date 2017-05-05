<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_nav_fun.php');
require_once(ADMIN_ROOT_PATH.'include/admin_page_fun.php');
check_permissions($_SESSION['admin_purview'],"site_navigation");
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
$smarty->assign('pageheader',"ナビ欄設定");
if($act == 'list')
{
	get_token();
	//筛选顶部导航
	$alias = !empty($_GET['alias']) ? trim($_GET['alias']) : 'HW_top';
	$smarty->assign('navlabel',"list");
	$smarty->assign('list',get_nav($alias));
	$smarty->display('nav/admin_nav.htm');
}
elseif($act == 'site_navigation_all_save')
{
	check_token();
	$id=$_POST['id'];
	$title=$_POST['title'];
	$navigationorder=$_POST['navigationorder'];
	$id_num=count($id);
		for($i=0;$i<$id_num;$i++)
		{
		$sql="update ".table('navigation')." set title='".$title[$i]."',navigationorder='".intval($navigationorder[$i])."'  where id='".intval($id[$i])."' LIMIT 1";
		$db->query($sql);
		}
	refresh_nav_cache();
	$smarty->clear_all_cache();
	write_log("ナビ変更成功", $_SESSION['admin_name'],3);
	adminmsg("変更成功！",2);
}
elseif($act == 'site_navigation_add')
{
	get_token();
	$smarty->assign('navlabel',"add");
	$smarty->assign('category',get_nav_cat());
	$smarty->assign('syspage',get_page(0,300," WHERE pagetpye=1 or pagetpye=2"));
	$smarty->display('nav/admin_nav_add.htm');
}
elseif($act == 'site_navigation_add_save')
{
	check_token();
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('コラム名称を入力してください！',1);
	$setsqlarr['urltype']=intval($_POST['urltype']);
		if ($setsqlarr['urltype']=="1")
		{
		$setsqlarr['url']=trim($_POST['url'])?trim($_POST['url']):adminmsg('リンクアドレスを入力してください！',1);
		}
		else
		{
		$setsqlarr['pagealias']=trim($_POST['pagealias'])?trim($_POST['pagealias']):adminmsg('ページCall名称をロストしました！',1);
		}
	$setsqlarr['list_id']=trim($_POST['list_id']);
	$setsqlarr['target']=trim($_POST['target'])?trim($_POST['target']):adminmsg('開く方式を入力してください！',1);
	$setsqlarr['navigationorder']=intval($_POST['navigationorder']);
	$setsqlarr['display']=$_POST['display'];
	$setsqlarr['color']=$_POST['tit_color'];
	$setsqlarr['alias']=trim($_POST['alias']);
	$setsqlarr['tag']=trim($_POST['tag']);
	if($db->inserttable(table('navigation'),$setsqlarr))
	{
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] ="?act=list";
	refresh_nav_cache();
	$smarty->clear_all_cache();
	write_log("ナビ追加", $_SESSION['admin_name'],3);
	adminmsg("追加成功！",2,$link);
	}
	else
	{
	adminmsg("追加失敗！",0);
	}
}
elseif($act == 'del_navigation')
{
	check_token();
	$id=$_GET['id'];
	if (del_navigation($id))
	{
	refresh_nav_cache();
	$smarty->clear_all_cache();
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] ="?act=";
	write_log("ナビ削除", $_SESSION['admin_name'],3);
	adminmsg("削除成功！",2,$link);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
elseif($act == 'site_navigation_edit')
{
	get_token();
	$id=intval($_GET['id']);
	$smarty->assign('show',get_nav_one($id));
	$smarty->assign('category',get_nav_cat());
	$smarty->assign('syspage',get_page(0,300," WHERE pagetpye=1 or pagetpye=2"));
	$smarty->display('nav/admin_nav_edit.htm');
}
elseif($act == 'site_navigation_edit_save')
{
	check_token();
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('コラム名称を入力してください！',1);
	$setsqlarr['urltype']=intval($_POST['urltype']);
		if ($setsqlarr['urltype']=="1")
		{
		$setsqlarr['url']=trim($_POST['url'])?trim($_POST['url']):adminmsg('リンクアドレスを入力してください！',1);
		}
		else
		{
		$setsqlarr['pagealias']=trim($_POST['pagealias'])?trim($_POST['pagealias']):adminmsg('ページCall名称をロストしました！',1);
		}
		//exit($setsqlarr['pagealias']);
	$setsqlarr['list_id']=trim($_POST['list_id']);
	$setsqlarr['target']=trim($_POST['target'])?trim($_POST['target']):adminmsg('開く方式を入力してください！',1);
	$setsqlarr['navigationorder']=intval($_POST['navigationorder']);
	$setsqlarr['display']=$_POST['display'];
	$setsqlarr['color']=$_POST['tit_color'];
	$setsqlarr['alias']=trim($_POST['alias']);
	$setsqlarr['tag']=trim($_POST['tag']);
	$wheresql=" id='".intval($_POST['id'])."'";
	if($db->updatetable(table('navigation'),$setsqlarr,$wheresql))
	{
	refresh_nav_cache();
	$smarty->clear_all_cache();
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] ="?act=list";
	write_log("ナビ欄変更", $_SESSION['admin_name'],3);
	adminmsg("変更成功！",2,$link);
	}
	else
	{
	adminmsg("変更失敗！",0);
	}
}
elseif($act == 'site_navigation_category')
{
	get_token();
	$smarty->assign('navlabel',"category");
	$smarty->assign('list',get_nav_cat());
	$smarty->display('nav/admin_nav_category.htm');
}
elseif($act == 'site_navigation_category_add')
{
	get_token();
	$smarty->assign('navlabel',"category");
	$smarty->display('nav/admin_nav_category_add.htm');
}
elseif($act == 'site_navigation_category_add_save')
{
	check_token();
	$setsqlarr['categoryname']=trim($_POST['categoryname'])?trim($_POST['categoryname']):adminmsg('名称を入力してください！',1);
	$setsqlarr['alias']=trim($_POST['alias'])?trim($_POST['alias']):adminmsg('Ｃａｌｌ名を入力してください！',1);
		if (stripos($setsqlarr['alias'],"hw_")===0)
		{
			adminmsg("CALL名“hw_”使えない",0);
		}
		else
		{
			$info=get_nav_cat_one($setsqlarr['alias']);
			if (empty($info))
			{
			$link[0]['text'] = "一覧に戻る";
			$link[0]['href'] ="?act=site_navigation_category";
			write_log("ナビ分類追加", $_SESSION['admin_name'],3);
			$db->inserttable(table('navigation_category'),$setsqlarr)?adminmsg("追加成功！",2,$link):adminmsg("追加失敗！",0);	
			}
			else
			{
			adminmsg("Call名".$setsqlarr['alias']."既に存在します！",0);
			}					
		}
		
}
elseif($act == 'site_navigation_category_del')
{
	check_token();
	if (del_nav_cat(intval($_GET['id'])))
	{
	write_log("ナビ分類削除", $_SESSION['admin_name'],3);
	adminmsg("削除成功！",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
elseif($act == 'site_navigation_category_edit')
{
		get_token();
	$smarty->assign('navlabel',"category");
	$alias=trim($_GET['alias']);
	$smarty->assign('list',get_nav_cat_one($alias));
	$smarty->display('nav/admin_nav_category_edit.htm');
}
elseif($act == 'site_navigation_category_edit_save')
{
	check_token();
	$setsqlarr['categoryname']=trim($_POST['categoryname'])?trim($_POST['categoryname']):adminmsg('名称を入力してください！',1);
	$setsqlarr['alias']=trim($_POST['alias'])?trim($_POST['alias']):adminmsg('Ｃａｌｌ名を入力してください！',1);
	if (stripos($setsqlarr['alias'],"hw_")===0)
		{
			adminmsg("CALL名“hw_”使えない",0);
		}
		else
		{
			$info=get_nav_cat_one($setsqlarr['alias']);
			if (empty($info) || $info['alias']==$setsqlarr['alias'])
			{
			$link[0]['text'] = "一覧に戻る";
			$link[0]['href'] ="?act=site_navigation_category";
			$wheresql=" id='".intval($_POST['id'])."'";
			write_log("ナビ分類変更", $_SESSION['admin_name'],3);
			!$db->updatetable(table('navigation_category'),$setsqlarr,$wheresql)?adminmsg("変更失敗！",0):adminmsg("変更成功！",2,$link);
			}
			else
			{
			adminmsg("Call名".$setsqlarr['alias']."既に存在します！",0);
			}					
		}
}
?>
