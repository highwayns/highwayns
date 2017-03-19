<?php
 /*
 * 74cms 导航栏目设置
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
require_once(ADMIN_ROOT_PATH.'include/admin_nav_fun.php');
require_once(ADMIN_ROOT_PATH.'include/admin_page_fun.php');
check_permissions($_SESSION['admin_purview'],"site_navigation");
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
$smarty->assign('pageheader',"导航栏设置");
if($act == 'list')
{
	get_token();
	//筛选顶部导航
	$alias = !empty($_GET['alias']) ? trim($_GET['alias']) : 'QS_top';
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
	write_log("修改导航成功", $_SESSION['admin_name'],3);
	adminmsg("修改成功！",2);
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
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('您没有填写栏目名称！',1);
	$setsqlarr['urltype']=intval($_POST['urltype']);
		if ($setsqlarr['urltype']=="1")
		{
		$setsqlarr['url']=trim($_POST['url'])?trim($_POST['url']):adminmsg('您没有填写链接地址！',1);
		}
		else
		{
		$setsqlarr['pagealias']=trim($_POST['pagealias'])?trim($_POST['pagealias']):adminmsg('页面调用名丢失！',1);
		}
	$setsqlarr['list_id']=trim($_POST['list_id']);
	$setsqlarr['target']=trim($_POST['target'])?trim($_POST['target']):adminmsg('您没有填写打开方式！',1);
	$setsqlarr['navigationorder']=intval($_POST['navigationorder']);
	$setsqlarr['display']=$_POST['display'];
	$setsqlarr['color']=$_POST['tit_color'];
	$setsqlarr['alias']=trim($_POST['alias']);
	$setsqlarr['tag']=trim($_POST['tag']);
	if($db->inserttable(table('navigation'),$setsqlarr))
	{
	$link[0]['text'] = "返回列表";
	$link[0]['href'] ="?act=list";
	refresh_nav_cache();
	$smarty->clear_all_cache();
	write_log("添加导航", $_SESSION['admin_name'],3);
	adminmsg("添加成功！",2,$link);
	}
	else
	{
	adminmsg("添加失败！",0);
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
	$link[0]['text'] = "返回列表";
	$link[0]['href'] ="?act=";
	write_log("删除导航", $_SESSION['admin_name'],3);
	adminmsg("删除成功！",2,$link);
	}
	else
	{
	adminmsg("删除失败！",0);
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
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('您没有填写栏目名称！',1);
	$setsqlarr['urltype']=intval($_POST['urltype']);
		if ($setsqlarr['urltype']=="1")
		{
		$setsqlarr['url']=trim($_POST['url'])?trim($_POST['url']):adminmsg('您没有填写链接地址！',1);
		}
		else
		{
		$setsqlarr['pagealias']=trim($_POST['pagealias'])?trim($_POST['pagealias']):adminmsg('页面调用名丢失！',1);
		}
		//exit($setsqlarr['pagealias']);
	$setsqlarr['list_id']=trim($_POST['list_id']);
	$setsqlarr['target']=trim($_POST['target'])?trim($_POST['target']):adminmsg('您没有填写打开方式！',1);
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
	$link[0]['text'] = "返回列表";
	$link[0]['href'] ="?act=list";
	write_log("修改导航栏目", $_SESSION['admin_name'],3);
	adminmsg("修改成功！",2,$link);
	}
	else
	{
	adminmsg("修改失败！",0);
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
	$setsqlarr['categoryname']=trim($_POST['categoryname'])?trim($_POST['categoryname']):adminmsg('您没有填写名称！',1);
	$setsqlarr['alias']=trim($_POST['alias'])?trim($_POST['alias']):adminmsg('您没有填写调用名！',1);
		if (stripos($setsqlarr['alias'],"qs_")===0)
		{
			adminmsg("调用名不能用“qs_”开通",0);
		}
		else
		{
			$info=get_nav_cat_one($setsqlarr['alias']);
			if (empty($info))
			{
			$link[0]['text'] = "返回列表";
			$link[0]['href'] ="?act=site_navigation_category";
			write_log("添加导航分类", $_SESSION['admin_name'],3);
			$db->inserttable(table('navigation_category'),$setsqlarr)?adminmsg("添加成功！",2,$link):adminmsg("添加失败！",0);	
			}
			else
			{
			adminmsg("调用名".$setsqlarr['alias']."已经存在！",0);
			}					
		}
		
}
elseif($act == 'site_navigation_category_del')
{
	check_token();
	if (del_nav_cat(intval($_GET['id'])))
	{
	write_log("删除导航分类", $_SESSION['admin_name'],3);
	adminmsg("删除成功！",2);
	}
	else
	{
	adminmsg("删除失败！",0);
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
	$setsqlarr['categoryname']=trim($_POST['categoryname'])?trim($_POST['categoryname']):adminmsg('您没有填写名称！',1);
	$setsqlarr['alias']=trim($_POST['alias'])?trim($_POST['alias']):adminmsg('您没有填写调用名！',1);
	if (stripos($setsqlarr['alias'],"qs_")===0)
		{
			adminmsg("调用名不能用“qs_”开通",0);
		}
		else
		{
			$info=get_nav_cat_one($setsqlarr['alias']);
			if (empty($info) || $info['alias']==$setsqlarr['alias'])
			{
			$link[0]['text'] = "返回列表";
			$link[0]['href'] ="?act=site_navigation_category";
			$wheresql=" id='".intval($_POST['id'])."'";
			write_log("修改导航分类", $_SESSION['admin_name'],3);
			!$db->updatetable(table('navigation_category'),$setsqlarr,$wheresql)?adminmsg("修改失败！",0):adminmsg("修改成功！",2,$link);
			}
			else
			{
			adminmsg("调用名".$setsqlarr['alias']."已经存在！",0);
			}					
		}
}
?>