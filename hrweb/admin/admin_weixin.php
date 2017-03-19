<?php
 /*
 * 74cms 微信公众平台
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
require_once(ADMIN_ROOT_PATH.'include/admin_weixin_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'set_weixin';
$smarty->assign('act',$act);
$smarty->assign('navlabel',$act);
$smarty->assign('pageheader',"微信公众平台");	
if($act == 'set_weixin')
{
	check_permissions($_SESSION['admin_purview'],"set_weixinconnect");	
	get_token();
	$smarty->assign('rand',rand(1,100));
	$smarty->assign('upfiles_dir',$upfiles_dir);	
	$smarty->assign('config',$_CFG);
	$smarty->display('weixin/admin_weixin.htm');
}
elseif($act == 'set_weixin_save')
{
	check_permissions($_SESSION['admin_purview'],"set_weixinconnect");	
	check_token();
	require_once(ADMIN_ROOT_PATH.'include/upload.php');
	if($_FILES['weixin_img']['name'])
	{
	$weixin_img=_asUpFiles($upfiles_dir, "weixin_img", 1024*2, 'jpg/gif/png',"weixin_img");
	!$db->query("UPDATE ".table('config')." SET value='$weixin_img' WHERE name='weixin_img'")?adminmsg('更新站点设置失败', 1):"";
	}
	if($_FILES['weixin_first_pic']['name'])
	{
	$weixin_first_pic=_asUpFiles($upfiles_dir, "weixin_first_pic", 1024*2, 'jpg/gif/png',"weixin_first_pic");
	!$db->query("UPDATE ".table('config')." SET value='$weixin_first_pic' WHERE name='weixin_first_pic'")?adminmsg('更新站点设置失败', 1):"";
	}
	if($_FILES['weixin_default_pic']['name'])
	{
	$weixin_default_pic=_asUpFiles($upfiles_dir, "weixin_default_pic", 1024*2, 'jpg/gif/png',"weixin_default_pic");
	!$db->query("UPDATE ".table('config')." SET value='$weixin_default_pic' WHERE name='weixin_default_pic'")?adminmsg('更新站点设置失败', 1):"";
	}
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('config')." SET value='{$v}' WHERE name='{$k}'")?adminmsg('更新站点设置失败', 1):"";
	}
	refresh_cache('config');
	write_log("设置微信", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
elseif($act == 'set_menu')
{
	get_token();
	$smarty->assign('navlabel',"set_menu");
	$smarty->assign('menu',get_weixin_menu());
	$smarty->display('weixin/admin_weixin_menu.htm');
}
elseif($act == 'menu_all_save')
{
	check_token();
	if (is_array($_POST['save_id']) && count($_POST['save_id'])>0)
	{
		foreach($_POST['save_id'] as $k=>$v)
		{
		 
				$setsqlarr['menu_order']=intval($_POST['menu_order'][$k]);
				!$db->updatetable(table('weixin_menu'),$setsqlarr," id=".intval($_POST['save_id'][$k]))?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
 
		}
	}
	//新增的入库
	if (is_array($_POST['add_pid']) && count($_POST['add_pid'])>0)
	{
		for ($i =0; $i <count($_POST['add_pid']);$i++){
			if (!empty($_POST['add_title'][$i]))
			{	
				$setsqlarr['menu_order']=intval($_POST['add_menu_order'][$i]);
				$setsqlarr['parentid']=intval($_POST['add_pid'][$i]);	
				!$db->inserttable(table('weixin_menu'),$setsqlarr)?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
			}

		}
	}
	write_log("设置微信菜单", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
elseif($act == 'del_menu')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_menu($id))
	{
	write_log("删除微信菜单,共删除".$num."行", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",1);
	}
}
elseif($act == 'edit_menu')
{
	get_token();
	$smarty->assign('navlabel',"set_menu");
	$smarty->assign('parent_manu',get_parent_menu());
	$smarty->assign('menu',get_weixin_menu_one($_GET['id']));
	$smarty->display('weixin/admin_weixin_menu_edit.htm');
}
elseif($act == 'edit_menu_save')
{
	check_token();
	$setsqlarr['parentid']=intval($_POST['parentid']);
	$setsqlarr['title']=trim($_POST['title']);
	$setsqlarr['key']=trim($_POST['key']);
	$setsqlarr['type']=trim($_POST['type']);
	$setsqlarr['url']=trim($_POST['url']);
	$setsqlarr['status']=intval($_POST['status']);
	$setsqlarr['menu_order']=intval($_POST['menu_order']);	
	!$db->updatetable(table('weixin_menu'),$setsqlarr," id=".intval($_POST['id']))?adminmsg("修改失败！",0):"";
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?act=set_menu';
	write_log("修改微信菜单", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2,$link);
}
elseif($act == 'add_menu')
{
	get_token();
	$smarty->assign('navlabel',"set_menu");
	$smarty->assign('parent_manu',get_parent_menu());
	$smarty->display('weixin/admin_weixin_menu_add.htm');
}
elseif($act == 'add_menu_save')
{
	check_token();
	$setsqlarr['parentid']=intval($_POST['parentid']);
	$setsqlarr['title']=trim($_POST['title']);
	$setsqlarr['key']=trim($_POST['key']);
	$setsqlarr['type']=trim($_POST['type']);
	$setsqlarr['url']=trim($_POST['url']);
	$setsqlarr['status']=intval($_POST['status']);
	$setsqlarr['menu_order']=intval($_POST['menu_order']);
	!$db->inserttable(table('weixin_menu'),$setsqlarr)?adminmsg("保存失败！",0):"";
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?act=set_menu';
	write_log("添加微信菜单", $_SESSION['admin_name'],3);
	adminmsg("添加成功！",2,$link);	
}
elseif($act == 'binding_list')
{
	get_token();
	$smarty->assign('navlabel',"binding_list");
	$smarty->assign('userlist',get_binding_list());
	$smarty->display('weixin/admin_binding_list.htm');
}
elseif($act == 'del_binding')
{
	check_token();
	$uid=$_REQUEST['uid'];
	if ($num=del_binding($uid))
	{
	write_log("解绑".$num."个会员", $_SESSION['admin_name'],3);
	adminmsg("解绑成功！共解绑".$num."个会员",2);
	}
	else
	{
	adminmsg("解绑失败！",1);
	}
}
?>