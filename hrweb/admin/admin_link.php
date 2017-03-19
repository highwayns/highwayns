<?php
 /*
 * 74cms 友情链接
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
require_once(ADMIN_ROOT_PATH.'include/admin_link_fun.php');
require_once(ADMIN_ROOT_PATH.'include/upload.php');
$upfiles_dir="../data/link/";
$files_dir=$_CFG['site_dir']."data/link/";
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
$smarty->assign('pageheader',"友情链接");
if($act == 'list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"link_show");
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$oederbysql=" order BY l.show_order DESC";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if ($key && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE l.link_name like '%{$key}%'";
		elseif ($key_type===2)$wheresql=" WHERE l.link_url like '%{$key}%'";
	}
	else
	{
	!empty($_GET['alias'])? $wheresqlarr['l.alias']=trim($_GET['alias']):'';
	!empty($_GET['type_id'])? $wheresqlarr['l.type_id']=intval($_GET['type_id']):'';
	if (is_array($wheresqlarr)) $wheresql=wheresql($wheresqlarr);
	}
	
	$joinsql=" LEFT JOIN ".table('link_category')." AS c ON l.alias=c.c_alias  ";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('link')." AS l ".$joinsql.$wheresql;
	$page = new page(array('total'=>$db->get_total($total_sql), 'perpage'=>$perpage));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$link = get_links($offset, $perpage,$joinsql.$wheresql.$oederbysql);
	$smarty->assign('link',$link);
	$smarty->assign('page',$page->show(3));
	$smarty->assign('upfiles_dir',$upfiles_dir);
	$smarty->assign('get_link_category',get_link_category());
	$smarty->assign('navlabel',"list");
	$smarty->display('link/admin_link.htm');
}
elseif($act == 'del_link')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"link_del");
	$id=$_REQUEST['id'];
	if ($num=del_link($id))
	{
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",0);
	}
}
elseif($act =='add')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"link_add");
	$id = !empty($_GET['id']) ? trim($_GET['id']) : '';
	$smarty->assign('cat',get_link_category());
	$smarty->assign('navlabel',"add");	
	$smarty->display('link/admin_link_add.htm');
}
elseif($act =='addsave')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"link_add");
	$setsqlarr['link_name']=$_POST['link_name']?trim($_POST['link_name']):adminmsg('链接名称不能为空！',1);
	$setsqlarr['link_url']=$_POST['link_url'];
	$setsqlarr['alias']=$_POST['alias'];
	$setsqlarr['show_order'] =intval($_POST['show_order']);
	$setsqlarr['display'] =intval($_POST['display']);
	$setsqlarr['type_id'] =1;
	$setsqlarr['Notes'] =trim($_POST['Notes']);	
	if ( $_FILES['logo']['name'])
	{
		$setsqlarr['link_logo']=_asUpFiles($upfiles_dir, "logo", 1024*2, 'jpg/gif/png',true);
		if (empty($setsqlarr['link_logo']))
		{
		adminmsg('上传图片出错！',1);
		}
		else
		{
		$setsqlarr['link_logo']=$files_dir.$setsqlarr['link_logo'];
		}
	}
	else
	{
		$setsqlarr['link_logo']=trim($_POST['link_logo']);
	}
	$link[0]['text'] = "继续添加链接";
	$link[0]['href'] = '?act=add';
	$link[1]['text'] = "返回友情链接列表";
	$link[1]['href'] = '?';
	//填写管理员日志
	write_log("后台添加友情链接", $_SESSION['admin_name'],3);
	!$db->inserttable(table('link'),$setsqlarr)?adminmsg("添加失败！",0):adminmsg("添加成功！",2,$link);
}
elseif($act =='edit')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"link_edit");
	$id =$_GET['id'];
	$smarty->assign('upfiles_dir',$upfiles_dir);
	$smarty->assign('link',get_links_one($id));
	$smarty->assign('cat',get_link_category());
	$smarty->assign('url',$_SERVER['HTTP_REFERER']);
	$smarty->display('link/admin_link_edit.htm');
}
elseif($act =='editsave')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"link_edit");
	$setsqlarr['link_name']=$_POST['link_name']?trim($_POST['link_name']):adminmsg('链接名称不能为空！',1);
	$setsqlarr['link_url']=$_POST['link_url'];
	$setsqlarr['alias']=$_POST['alias'];
	$setsqlarr['show_order'] =intval($_POST['show_order']);
	$setsqlarr['display'] =intval($_POST['display']);
	$setsqlarr['Notes'] =trim($_POST['Notes']);
	if ( $_FILES['logo']['name'])
	{
		$setsqlarr['link_logo']=_asUpFiles($upfiles_dir, "logo", 1024*2, 'jpg/gif/png',true);
		if (empty($setsqlarr['link_logo']))
		{
		adminmsg('上传图片出错！',1);
		}
		else
		{
		$setsqlarr['link_logo']=$files_dir.$setsqlarr['link_logo'];
		}
	}
	else
	{
		$setsqlarr['link_logo']=trim($_POST['link_logo']);
	}
	$link[0]['text'] = "返回上一页";
	$link[0]['href'] = $_POST['url'];
	//填写管理员日志
	write_log("后台修改友情链接", $_SESSION['admin_name'],3);
	!$db->updatetable(table('link'),$setsqlarr," link_id =".intval($_POST['id']))?adminmsg("修改失败！",0):adminmsg("修改成功！",2,$link);
}
elseif($act == 'category')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"link_category");
	$smarty->assign('link',get_link_category());
	$smarty->assign('navlabel',"category");
	$smarty->display('link/admin_link_category.htm');
}
elseif($act == 'category_add')
{	
	get_token();
	check_permissions($_SESSION['admin_purview'],"link_category");
	$smarty->assign('navlabel',"category");
	$smarty->display('link/admin_link_category_add.htm');
}
elseif($act == 'add_category_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"link_category");	
	$setsqlarr['categoryname']=$_POST['categoryname']?trim($_POST['categoryname']):adminmsg('您没有填写分类名称！',1);
	$setsqlarr['c_alias']=$_POST['c_alias']?trim($_POST['c_alias']):adminmsg('您没有填调用名称！',1);
	substr($setsqlarr['c_alias'],0,3)=='QS_'?adminmsg('调用名称不允许 QS_ 开头！',1):'';
	$category=get_link_category_name($setsqlarr['c_alias']);
	if ($category)
	{
	adminmsg("调用名已经存在！",0);
	}
	else
	{
	$link[0]['text'] = "返回分类管理";
	$link[0]['href'] = '?act=category';
	$link[1]['text'] = "继续添加分类";
	$link[1]['href'] = "?act=category_add";
	!$db->inserttable(table('link_category'),$setsqlarr)?adminmsg("添加失败！",0):adminmsg("添加成功！",2,$link);
	}	
}
elseif($act == 'category_edit')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"link_category");
	$smarty->assign('navlabel',"category");
	$smarty->assign('category',get_link_category_name($_GET['alias']));
	$smarty->display('link/admin_link_category_edit.htm');
}
elseif($act == 'edit_category_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"link_category");	
	$setsqlarr['categoryname']=$_POST['categoryname']?trim($_POST['categoryname']):adminmsg('您没有填写分类名称！',1);
	$setsqlarr['c_alias']=$_POST['c_alias']?trim($_POST['c_alias']):adminmsg('您没有填调用名称！',1);
	substr($setsqlarr['c_alias'],0,3)=='QS_'?adminmsg('调用名称不允许 QS_ 开头！',1):'';
	$category=get_link_category_name($setsqlarr['c_alias']);
	if ($category && $category['id']<>$_POST['id'])
	{
	adminmsg("调用名已经存在！",0);
	}
	else
	{
	$link[0]['text'] = "返回分类管理";
	$link[0]['href'] = '?act=category';
	!$db->updatetable(table('link_category'),$setsqlarr," id=".intval($_POST['id']))?adminmsg("修改失败！",0):adminmsg("修改成功！",2,$link);
	}	
}
elseif($act == 'del_category')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"link_category");
	$id=$_REQUEST['id'];
	if ($num=del_category($id))
	{
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",0);
	}
}
elseif($act == 'link_set')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"link_set");
	$smarty->assign('config',$_CFG);
	$smarty->assign('text',get_cache('text'));
	$smarty->assign('navlabel',"link_set");
	$smarty->display('link/admin_link_set.htm');
}
elseif($act == 'link_set_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"mb_set");
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('config')." SET value='$v' WHERE name='$k'")?adminmsg('更新设置失败', 1):"";
	}
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('text')." SET value='$v' WHERE name='$k'")?adminmsg('更新设置失败', 1):"";
	}
	refresh_cache('config');
	refresh_cache('text');
	adminmsg("保存成功！",2);
}
?>