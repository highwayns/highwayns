<?php
 /*
 * 74cms 分类
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
require_once(ADMIN_ROOT_PATH.'include/admin_category_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'district';
check_permissions($_SESSION['admin_purview'],"site_category");
$smarty->assign('pageheader',"分类管理");
if($act == 'grouplist')
{
	get_token();
	$smarty->assign('navlabel',"group");
	$smarty->assign('group',get_category_group());
	$smarty->display('category/admin_category_group.htm');
}
elseif($act == 'add_group')
{
	get_token();
	$smarty->assign('navlabel',"group");
	$smarty->display('category/admin_category_group_add.htm');
}
elseif($act == 'add_group_save')
{
	check_token();
	$setsqlarr['g_name']=!empty($_POST['g_name']) ?trim($_POST['g_name']) : adminmsg("请填写分组名",1);
	$setsqlarr['g_alias']=!empty($_POST['g_alias']) ?trim($_POST['g_alias']) : adminmsg("请填写调用名",1);
	$info=get_category_group_one($setsqlarr['g_alias']);
	if (empty($info))
	{
		if (stripos($setsqlarr['g_alias'],"qs_")===0)
		{
			adminmsg("调用名不能用“qs_”开通",0);
		}
		else
		{
			$link[0]['text'] = "分类组列表";
			$link[0]['href'] = '?act=grouplist';
			$link[1]['text'] = "继续添加分类组";
			$link[1]['href'] = "?act=add_group";
			//填写管理员日志
			write_log("后台添加分类！", $_SESSION['admin_name'],3);
			$db->inserttable(table('category_group'),$setsqlarr)?adminmsg("添加成功！",2,$link):adminmsg("添加失败！",0);			
		}
	}
	else
	{
	 adminmsg("添加失败,调用名有重复",0);
	}
}
elseif($act == 'edit_group')
{
	get_token();
	$smarty->assign('navlabel',"group");
	$smarty->assign('group',get_category_group_one($_GET['alias']));
	$smarty->display('category/admin_category_group_edit.htm');
}
elseif($act == 'edit_group_save')
{
	check_token();
	$setsqlarr['g_name']=!empty($_POST['g_name']) ?trim($_POST['g_name']) : adminmsg("请填写分组名",1);
	$setsqlarr['g_alias']=!empty($_POST['g_alias']) ?trim($_POST['g_alias']) : adminmsg("请填写调用名",1);
	$info=get_category_group_one($setsqlarr['g_alias']);
	if (empty($info) || $info['g_id']==intval($_POST['g_id']))
	{
		if (stripos($setsqlarr['g_alias'],"qs_")===0)
		{
			adminmsg("调用名不能用“qs_”开通",0);
		}
		else
		{
			$link[0]['text'] = "分类组列表";
			$link[0]['href'] = '?act=grouplist';
			$link[1]['text'] = "查看修改结果";
			$link[1]['href'] = "?act=edit_group&alias=".$setsqlarr['g_alias'];
			$db->updatetable(table('category_group'),$setsqlarr," g_id=".intval($_POST['g_id']))?'':adminmsg("修改失败！",0);
			//同时修改分类组下的分类别名
			$catarr['c_alias']=$setsqlarr['g_alias'];
			$db->updatetable(table('category'),$catarr," c_alias='".$_POST['old_g_alias']."'")?'':adminmsg("修改失败！",0);
			//填写管理员日志
			write_log("后台成功修改分类！", $_SESSION['admin_name'],3);
			adminmsg("修改成功！",2,$link);						
		}
	}
	else
	{
	 adminmsg("添加失败,调用名有重复",0);
	}
}
elseif($act == 'del_group')
{
	check_token();
	$alias=$_REQUEST['alias'];
	if ($num=del_group($alias))
	{
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",1);
	}
}
elseif($act == 'show_category')
{
	get_token();
	$smarty->assign('navlabel',"group");
	$smarty->assign('group',get_category_group_one($_GET['alias']));
	$smarty->assign('category',get_category($_GET['alias']));	
	$smarty->display('category/admin_category_list.htm');
}
elseif($act == 'category_save')
{
	check_token();
	if (is_array($_POST['c_id']) && count($_POST['c_id'])>0)
	{
		for ($i =0; $i <count($_POST['c_id']);$i++){
			if (!empty($_POST['c_name'][$i]))
			{	
				$setsqlarr['c_name']=trim($_POST['c_name'][$i]);
				$setsqlarr['c_order']=intval($_POST['c_order'][$i]);
				$setsqlarr['c_index']=getfirstchar($setsqlarr['c_name']);
				!$db->updatetable(table('category'),$setsqlarr," c_id=".intval($_POST['c_id'][$i]))?adminmsg("添加失败！",0):"";
				$num=$num+$db->affected_rows();
			}

		}

	}
	//填写管理员日志
	write_log("后台成功修改分类！", $_SESSION['admin_name'],3);
	refresh_category_cache();
	makejs_classify();
	adminmsg("修改完成！",2);
}
elseif($act == 'add_category')
{
	get_token();
	$smarty->assign('navlabel',"group");
	$smarty->assign('group',get_category_group_one($_GET['alias']));
	$smarty->display('category/admin_category_add.htm');
}
elseif($act == 'add_category_save')
{
	check_token();
	$num=0;
	if (is_array($_POST['c_name']) && count($_POST['c_name'])>0)
	{
		for ($i =0; $i <count($_POST['c_name']);$i++){
			if (!empty($_POST['c_name'][$i]))
			{		
				$setsqlarr['c_name']=trim($_POST['c_name'][$i]);
				$setsqlarr['c_alias']=trim($_POST['c_alias'][$i]);
				$setsqlarr['c_order']=intval($_POST['c_order'][$i]);
				$setsqlarr['c_index']=getfirstchar($setsqlarr['c_name']);
				$setsqlarr['c_note']=trim($_POST['c_note'][$i]);				
				!$db->inserttable(table('category'),$setsqlarr)?adminmsg("添加失败！",0):"";
				$num=$num+$db->affected_rows();
			}

		}

	}
	if ($num==0)
	{
	adminmsg("添加失败,数据不完整",1);
	}
	else
	{
	$link[0]['text'] = "返回分类列表";
	$link[0]['href'] = "?act=show_category&alias=".$setsqlarr['c_alias'];
	$link[1]['text'] = "继续添加分类";
	$link[1]['href'] = "?act=add_category&alias=".$setsqlarr['c_alias'];
	refresh_category_cache();
	makejs_classify();
	//填写管理员日志
	write_log("后台成功添加分类 , 共添加".$num."个！", $_SESSION['admin_name'],3);
	adminmsg("添加成功！共添加".$num."个分类",2,$link);
	}
}
elseif($act == 'edit_category')
{	
	get_token();
	$smarty->assign('navlabel',"group");
	$smarty->assign('category',get_category_one($_GET['id']));
	$smarty->display('category/admin_category_edit.htm');
}
elseif($act == 'edit_category_save')
{
	check_token();
	$setsqlarr['c_name']=!empty($_POST['c_name']) ?trim($_POST['c_name']) : adminmsg("请填写名称",1);
	$setsqlarr['c_order']=intval($_POST['c_order']);
	$setsqlarr['c_parentid']=intval($_POST['c_parentid']);
	$setsqlarr['c_index']=getfirstchar($setsqlarr['c_name']);
	$setsqlarr['c_note']=trim($_POST['c_note']);				
	!$db->updatetable(table('category'),$setsqlarr," c_id=".intval($_POST['c_id']))?adminmsg("保存失败！",0):"";
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?act=show_category&alias='.$_POST['c_alias'];
	$link[1]['text'] = "查看修改结果";
	$link[1]['href'] = "?act=edit_category&id=".intval($_POST['c_id']);
	refresh_category_cache();
	makejs_classify();
	//填写管理员日志
	write_log("后台成功修改分类", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2,$link);
}
elseif($act == 'del_category')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_category($id))
	{
	refresh_category_cache();
	makejs_classify();
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",1);
	}
}
//地区--------------
elseif($act == 'district')
{
	get_token();
	$smarty->assign('navlabel',"district");
	$smarty->assign('district',get_category_district());
	$smarty->display('category/admin_category_district.htm');
}
elseif($act == 'district_all_save')
{
	check_token();
	if (is_array($_POST['save_id']) && count($_POST['save_id'])>0)
	{
		foreach($_POST['save_id'] as $k=>$v)
		{
		 
				$setsqlarr['categoryname']=trim($_POST['categoryname'][$k]);
				$setsqlarr['category_order']=intval($_POST['category_order'][$k]);
				!$db->updatetable(table('category_district'),$setsqlarr," id=".intval($_POST['save_id'][$k]))?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
 
		}
		//填写管理员日志
		write_log("后台成功更新地区分类 , 共更新".$num."个", $_SESSION['admin_name'],3);
	}
	//新增的入库
	if (is_array($_POST['add_pid']) && count($_POST['add_pid'])>0)
	{
		for ($i =0; $i <count($_POST['add_pid']);$i++){
			if (!empty($_POST['add_categoryname'][$i]))
			{	
				$setsqlarr['categoryname']=trim($_POST['add_categoryname'][$i]);
				$setsqlarr['category_order']=intval($_POST['add_category_order'][$i]);
				$setsqlarr['parentid']=intval($_POST['add_pid'][$i]);	
				!$db->inserttable(table('category_district'),$setsqlarr)?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
			}

		}
		//填写管理员日志
		write_log("后台成功添加地区分类 , 共添加".$num."个", $_SESSION['admin_name'],3);
	}
	makejs_classify();
	adminmsg("保存成功！",2);
}
elseif($act == 'del_district')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_district($id))
	{
	makejs_classify();
	//填写管理员日志
	write_log("后台成功删除地区分类！共删除".$num."行", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",1);
	}
}
elseif($act == 'edit_district')
{
	get_token();
	$smarty->assign('navlabel',"district");
	$smarty->assign('district',get_category_district_one($_GET['id']));
	$smarty->display('category/admin_category_district_edit.htm');
}
elseif($act == 'edit_district_save')
{
	check_token();
	$setsqlarr['categoryname']=!empty($_POST['categoryname']) ?trim($_POST['categoryname']) : adminmsg("请填写名称",1);
	$setsqlarr['category_order']=intval($_POST['category_order']);
	$setsqlarr['parentid']=intval($_POST['parentid']);				
	!$db->updatetable(table('category_district'),$setsqlarr," id=".intval($_POST['id']))?adminmsg("修改失败！",0):"";
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?act=district';
	makejs_classify();
	//填写管理员日志
	write_log("后台成功修改地区分类！", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2,$link);
}
elseif($act == 'add_district')
{
	get_token();
	$smarty->assign('navlabel',"district");
	$smarty->display('category/admin_category_district_add.htm');
}
elseif($act == 'add_district_save')
{
	check_token();
	//新增的入库
	if (is_array($_POST['categoryname']) && count($_POST['categoryname'])>0)
	{
		for ($i =0; $i <count($_POST['categoryname']);$i++){
			if (!empty($_POST['categoryname'][$i]))
			{	
				$setsqlarr['categoryname']=trim($_POST['categoryname'][$i]);
				$setsqlarr['category_order']=intval($_POST['category_order'][$i]);
				$setsqlarr['parentid']=intval($_POST['parentid'][$i]);	
				!$db->inserttable(table('category_district'),$setsqlarr)?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
			}

		}
	}
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?act=district';
	makejs_classify();
	//填写管理员日志
	write_log("后台成功添加地区分类！本次添加了{$num}个分类", $_SESSION['admin_name'],3);
	adminmsg("添加成功！本次添加了{$num}个分类",2,$link);	
}
///////---------------职位分类
elseif($act == 'jobs')
{
	get_token();
	$smarty->assign('navlabel',"jobs");
	$smarty->assign('district',get_category_jobs());
	$smarty->display('category/admin_category_jobs.htm');
}
elseif($act == 'jobs_all_save')
{
	check_token();
	if (is_array($_POST['save_id']) && count($_POST['save_id'])>0)
	{
		for ($i =0; $i <count($_POST['save_id']);$i++){
			if (!empty($_POST['categoryname'][$i]))
			{	
				$setsqlarr['categoryname']=trim($_POST['categoryname'][$i]);
				$setsqlarr['category_order']=intval($_POST['category_order'][$i]);
				$setsqlarr['content']=trim($_POST['content'][$i]);				
				!$db->updatetable(table('category_jobs'),$setsqlarr," id=".intval($_POST['save_id'][$i]))?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
			}
		}
		//填写管理员日志
		write_log("后台成功更新职位分类！本次更新了{$num}个分类", $_SESSION['admin_name'],3);
	}
	//新增的入库
	if (is_array($_POST['add_pid']) && count($_POST['add_pid'])>0)
	{
		for ($i =0; $i <count($_POST['add_pid']);$i++){
			if (!empty($_POST['add_categoryname'][$i]))
			{	
				$setsqlarr['categoryname']=trim($_POST['add_categoryname'][$i]);
				$setsqlarr['category_order']=intval($_POST['add_category_order'][$i]);
				$setsqlarr['content']=trim($_POST['content'][$i]);	
				$setsqlarr['parentid']=intval($_POST['add_pid'][$i]);	
				!$db->inserttable(table('category_jobs'),$setsqlarr)?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
			}

		}
		//填写管理员日志
		write_log("后台成功添加职位分类！本次添加了{$num}个分类", $_SESSION['admin_name'],3);
	}
	makejs_classify();
	adminmsg("保存成功！",2);
}
elseif($act == 'del_jobs_category')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_jobs_category($id))
	{
		//填写管理员日志
		write_log("后台成功删除职位分类！共删除".$num."行", $_SESSION['admin_name'],3);
		adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",1);
	}
}
elseif($act == 'edit_jobs_category')
{
	get_token();
	$smarty->assign('navlabel',"jobs");
	$smarty->assign('category',get_category_jobs_one($_GET['id']));
	$smarty->display('category/admin_category_jobs_edit.htm');
}
elseif($act == 'edit_jobs_category_save')
{
	check_token();
	$setsqlarr['categoryname']=!empty($_POST['categoryname']) ?trim($_POST['categoryname']) : adminmsg("请填写名称",1);
	$setsqlarr['category_order']=intval($_POST['category_order']);
	$setsqlarr['content']=trim($_POST['content']);
	$setsqlarr['parentid']=intval($_POST['parentid']);				
	!$db->updatetable(table('category_jobs'),$setsqlarr," id=".intval($_POST['id']))?adminmsg("修改失败！",0):"";
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?act=jobs';
	makejs_classify();
	//填写管理员日志
	write_log("后台成功修改职位分类！", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2,$link);
}
elseif($act == 'add_category_jobs')
{
	get_token();
	$smarty->assign('navlabel',"jobs");
	$smarty->display('category/admin_category_jobs_add.htm');
}
elseif($act == 'add_category_jobs_save')
{
	check_token();
	//新增的入库
	if (is_array($_POST['categoryname']) && count($_POST['categoryname'])>0)
	{
		for ($i =0; $i <count($_POST['categoryname']);$i++){
			if (!empty($_POST['categoryname'][$i]))
			{	
				$setsqlarr['categoryname']=trim($_POST['categoryname'][$i]);
				$setsqlarr['category_order']=intval($_POST['category_order'][$i]);
				$setsqlarr['content']=trim($_POST['content'][$i]);	
				$setsqlarr['parentid']=intval($_POST['parentid'][$i]);	
				!$db->inserttable(table('category_jobs'),$setsqlarr)?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
			}

		}
	}
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?act=jobs';
	makejs_classify();
	//填写管理员日志
	write_log("后台成功添加职位分类！本次添加了".$num."个分类", $_SESSION['admin_name'],3);
	adminmsg("添加成功！本次添加了".$num."个分类",2,$link);	
}
elseif($act == 'colorlist')
{
	get_token();
	$smarty->assign('navlabel',"color");
	$smarty->assign('color',get_color());
	$smarty->display('category/admin_color.htm');
}
elseif($act == 'add_color')
{
	get_token();
	$smarty->assign('navlabel',"color");
	$smarty->display('category/admin_color_add.htm');
}
elseif($act == 'add_color_save')
{
	check_token();
	$setsqlarr['value']=!empty($_POST['val']) ?trim($_POST['val']) : adminmsg("请选择颜色",1);
	$link[0]['text'] = "颜色列表";
	$link[0]['href'] = '?act=colorlist';
	$link[1]['text'] = "继续添加颜色";
	$link[1]['href'] = "?act=add_color";
	//填写管理员日志
	write_log("后台添加颜色分类！", $_SESSION['admin_name'],3);
	$db->inserttable(table('color'),$setsqlarr)?adminmsg("添加成功！",2,$link):adminmsg("添加失败！",0);			

}
elseif($act == 'edit_color')
{
	get_token();
	$smarty->assign('navlabel',"color");
	$smarty->assign('color',get_color_one($_GET['id']));
	$smarty->display('category/admin_color_edit.htm');
}
elseif($act == 'edit_color_save')
{
	check_token();
	$setsqlarr['value']=!empty($_POST['val']) ?trim($_POST['val']) : adminmsg("请选择颜色",1);
	$info=get_color_one($_POST['id']);
	
	$link[0]['text'] = "颜色列表";
	$link[0]['href'] = '?act=colorlist';
	$link[1]['text'] = "查看修改结果";
	$link[1]['href'] = "?act=edit_color&id=".$_POST['id'];
	$db->updatetable(table('color'),$setsqlarr," id=".intval($_POST['id']))?'':adminmsg("修改失败！",0);
	//填写管理员日志
	write_log("后台成功修改颜色分类！", $_SESSION['admin_name'],3);
	adminmsg("修改成功！",2,$link);						
	
}
elseif($act == 'del_color')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_color($id))
	{
		//填写管理员日志
		write_log("后台成功删除颜色分类！共删除".$num."行", $_SESSION['admin_name'],3);
		adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",1);
	}
}
///////---------------专业分类
elseif($act == 'major')
{
	get_token();
	$smarty->assign('navlabel',"major");
	$smarty->assign('district',get_category_major());
	$smarty->display('category/admin_category_major.htm');
}
elseif($act == 'major_all_save')
{
	check_token();
	if (is_array($_POST['save_id']) && count($_POST['save_id'])>0)
	{
		for ($i =0; $i <count($_POST['save_id']);$i++){
			if (!empty($_POST['categoryname'][$i]))
			{	
				$setsqlarr['categoryname']=trim($_POST['categoryname'][$i]);
				$setsqlarr['category_order']=intval($_POST['category_order'][$i]);			
				!$db->updatetable(table('category_major'),$setsqlarr," id=".intval($_POST['save_id'][$i]))?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
			}
		}
		//填写管理员日志
		write_log("后台成功更新专业分类！本次更新了{$num}个分类", $_SESSION['admin_name'],3);
	}
	//新增的入库
	if (is_array($_POST['add_pid']) && count($_POST['add_pid'])>0)
	{
		for ($i =0; $i <count($_POST['add_pid']);$i++){
			if (!empty($_POST['add_categoryname'][$i]))
			{	
				$setsqlarr['categoryname']=trim($_POST['add_categoryname'][$i]);
				$setsqlarr['category_order']=intval($_POST['add_category_order'][$i]);
				$setsqlarr['parentid']=intval($_POST['add_pid'][$i]);	
				!$db->inserttable(table('category_major'),$setsqlarr)?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
			}

		}
		//填写管理员日志
		write_log("后台成功添加专业分类！本次添加了{$num}个分类", $_SESSION['admin_name'],3);
	}
	makejs_classify();
	adminmsg("保存成功！",2);
}
elseif($act == 'del_major_category')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_major_category($id))
	{
		//填写管理员日志
		write_log("后台成功删除专业分类！共删除".$num."行", $_SESSION['admin_name'],3);
		adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",1);
	}
}
elseif($act == 'edit_major_category')
{
	get_token();
	$smarty->assign('navlabel',"major");
	$smarty->assign('category',get_category_major_one($_GET['id']));
	$smarty->display('category/admin_category_major_edit.htm');
}
elseif($act == 'edit_major_category_save')
{
	check_token();
	$setsqlarr['categoryname']=!empty($_POST['categoryname']) ?trim($_POST['categoryname']) : adminmsg("请填写名称",1);
	$setsqlarr['category_order']=intval($_POST['category_order']);
	$setsqlarr['parentid']=intval($_POST['parentid']);				
	!$db->updatetable(table('category_major'),$setsqlarr," id=".intval($_POST['id']))?adminmsg("修改失败！",0):"";
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?act=major';
	makejs_classify();
	//填写管理员日志
	write_log("后台成功修改专业分类！", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2,$link);
}
elseif($act == 'add_category_major')
{
	get_token();
	$smarty->assign('navlabel',"major");
	$smarty->display('category/admin_category_major_add.htm');
}
elseif($act == 'add_category_major_save')
{
	check_token();
	//新增的入库
	if (is_array($_POST['categoryname']) && count($_POST['categoryname'])>0)
	{
		for ($i =0; $i <count($_POST['categoryname']);$i++){
			if (!empty($_POST['categoryname'][$i]))
			{	
				$setsqlarr['categoryname']=trim($_POST['categoryname'][$i]);
				$setsqlarr['category_order']=intval($_POST['category_order'][$i]);
				$setsqlarr['parentid']=intval($_POST['parentid'][$i]);	
				!$db->inserttable(table('category_major'),$setsqlarr)?adminmsg("保存失败！",0):"";
				$num=$num+$db->affected_rows();
			}

		}
	}
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?act=major';
	makejs_classify();
	//填写管理员日志
	write_log("后台成功添加专业分类！本次添加了".$num."个分类", $_SESSION['admin_name'],3);
	adminmsg("添加成功！本次添加了".$num."个分类",2,$link);	
}
?>