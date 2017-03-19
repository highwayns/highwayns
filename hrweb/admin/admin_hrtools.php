<?php
 /*
 * 74cms HR工具箱
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
require_once(ADMIN_ROOT_PATH.'include/admin_hrtools_fun.php');
require_once(ADMIN_ROOT_PATH.'include/upload.php');
check_permissions($_SESSION['admin_purview'],"hrtools");
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
$hrtools_updir="../data/hrtools/";
$hrtools_dir="data/hrtools/";
$smarty->assign('pageheader',"HR工具箱");
if($act == 'list')
{
	get_token();
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$oederbysql=" order BY h.h_order DESC,h_id DESC";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if ($key && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE h.h_filename like '%{$key}%'";
		$oederbysql="";
	}
	!empty($_GET['h_typeid'])? $wheresqlarr['h.h_typeid']=intval($_GET['h_typeid']):'';
	if (!empty($wheresqlarr)) $wheresql=wheresql($wheresqlarr);
	$joinsql=" LEFT JOIN  ".table('hrtools_category')." AS c ON h.h_typeid=c.c_id ";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('hrtools')." AS h ".$joinsql.$wheresql;
	$page = new page(array('total'=>$db->get_total($total_sql), 'perpage'=>$perpage));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$hrtools = get_hrtools($offset, $perpage,$joinsql.$wheresql.$oederbysql);
	$smarty->assign('category',get_hrtools_category());
	$smarty->assign('hrtools',$hrtools);
	$smarty->assign('page',$page->show(3));
	$smarty->assign('navlabel',"list");
	$smarty->display('hrtools/admin_hrtools.htm');
}
elseif($act == 'edit')
{
	get_token();
	$id = intval($_GET['id']);
	$sql = "select * from ".table('hrtools')." AS h LEFT JOIN ".table('hrtools_category')." AS c ON h.h_typeid=c.c_id where h.h_id='{$id}' LIMIT 1";
	$show=$db->getone($sql);
	$smarty->assign('show',$show);
	$smarty->assign('category',get_hrtools_category());
	$smarty->display('hrtools/admin_hrtools_edit.htm');
}
elseif($act == 'editsave')
{
	check_token();
	$setsqlarr['h_filename']=!empty($_POST['h_filename'])?trim($_POST['h_filename']):adminmsg('文档名称不能为空！',1);
	$setsqlarr['h_typeid']=intval($_POST['h_typeid'])>0?intval($_POST['h_typeid']):adminmsg('请选择分类！',1);
	$setsqlarr['h_color']=trim($_POST['h_color']);
	$setsqlarr['h_strong']=intval($_POST['h_strong']);
	$setsqlarr['h_order']=intval($_POST['h_order']);
	if (empty($_FILES['upfile']['name']) && empty($_POST['url']))
	{
	adminmsg('请上传文件或者填写文件路径！',1);
	}
	if ($_FILES['upfile']['name'])
		{
			$hrtools_updir=$hrtools_updir.date("Y/m/");
			make_dir($hrtools_updir);
			$setsqlarr['h_fileurl']=_asUpFiles($hrtools_updir,"upfile",3000,'doc/ppt/xls/rtf',true);
			if (empty($setsqlarr['h_fileurl']))
			{
			adminmsg('上传文件失败！',1);
			}
			$setsqlarr['h_fileurl']=$hrtools_dir.date("Y/m/").$setsqlarr['h_fileurl'];
		}
		else
		{
			$setsqlarr['h_fileurl']=trim($_POST['url']);
		}
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?';
	write_log("修改id为".intval($_POST['id'])."的hr工具", $_SESSION['admin_name'],3);
	!$db->updatetable(table('hrtools'),$setsqlarr," h_id=".intval($_POST['id'])."")?adminmsg("修改失败！",0):adminmsg("修改成功！",2,$link);
}
elseif($act == 'add')
{
	get_token();
	$smarty->assign('category',get_hrtools_category());
	$smarty->assign('navlabel',"add");
	$smarty->display('hrtools/admin_hrtools_add.htm');
}
elseif($act == 'addsave')
{	
	check_token();
	$setsqlarr['h_filename']=!empty($_POST['h_filename'])?trim($_POST['h_filename']):adminmsg('文档名称不能为空！',1);
	$setsqlarr['h_typeid']=intval($_POST['h_typeid'])>0?intval($_POST['h_typeid']):adminmsg('请选择分类！',1);
	$setsqlarr['h_color']=trim($_POST['h_color']);
	$setsqlarr['h_strong']=intval($_POST['h_strong']);
	$setsqlarr['h_order']=intval($_POST['h_order']);
	if (empty($_FILES['upfile']['name']) && empty($_POST['url']))
	{
	adminmsg('请上传文件或者填写文件路径！',1);
	}
	if ($_FILES['upfile']['name'])
		{
			$hrtools_updir=$hrtools_updir.date("Y/m/");
			make_dir($hrtools_updir);
			$setsqlarr['h_fileurl']=_asUpFiles($hrtools_updir,"upfile",3000,'doc/ppt/xls/rtf',true);
			if (empty($setsqlarr['h_fileurl']))
			{
			adminmsg('上传文件失败！',1);
			}
			$setsqlarr['h_fileurl']=$hrtools_dir.date("Y/m/").$setsqlarr['h_fileurl'];
		}
		else
		{
			$setsqlarr['h_fileurl']=trim($_POST['url']);
		}
	$link[0]['text'] = "继续添加";
	$link[0]['href'] = "?act=add&h_typeid={$setsqlarr['h_typeid']}&h_typeid_cn={$_POST['h_typeid_cn']}";
	$link[1]['text'] = "返回列表";
	$link[1]['href'] = '?';
	write_log("添加hr工具:".$setsqlarr['h_filename'], $_SESSION['admin_name'],3);
	!$db->inserttable(table('hrtools'),$setsqlarr)?adminmsg("添加失败！",0):adminmsg("添加成功！",2,$link);
}
elseif($act == 'hrtools_del')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_hrtools($id))
	{
	write_log("删除hr工具,共删除".$num."行", $_SESSION['admin_name'],3);	
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",0);
	}
}
elseif($act == 'category')
{
	get_token();
	$smarty->assign('category',get_hrtools_category());
	$smarty->assign('navlabel',"category");
	$smarty->display('hrtools/admin_hrtools_category.htm');
}
elseif($act == 'category_add')
{
	get_token();
	$smarty->assign('navlabel',"category");
	$smarty->display('hrtools/admin_hrtools_category_add.htm');
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
				$setsqlarr['c_order']=intval($_POST['c_order'][$i]);	
				$setsqlarr['c_adminset']=0;		
				!$db->inserttable(table('hrtools_category'),$setsqlarr)?adminmsg("添加失败！",0):"";
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
	$link[0]['text'] = "返回分类管理";
	$link[0]['href'] = '?act=category';
	$link[1]['text'] = "继续添加";
	$link[1]['href'] = "?act=category_add";
	write_log("添加hr工具分类,共添加".$num."个分类", $_SESSION['admin_name'],3);
	adminmsg("添加成功！共添加".$num."个分类",2,$link);
	}
}
elseif($act == 'edit_category')
{
	get_token();
	$id=intval($_GET['id']);
	$smarty->assign('category',get_hrtools_category_one($id));
	$smarty->assign('navlabel',"category");
	$smarty->display('hrtools/admin_hrtools_category_edit.htm');
}
elseif($act == 'edit_category_save')
{
	check_token();
	$id=intval($_POST['id']);	
	$setsqlarr['c_name']=!empty($_POST['c_name'])?trim($_POST['c_name']):adminmsg('请填写分类名称！',1);
	$setsqlarr['c_order']=intval($_POST['c_order']);
	$link[0]['text'] = "查看修改结果";
	$link[0]['href'] = '?act=edit_category&id='.$id;
	$link[1]['text'] = "返回分类管理";
	$link[1]['href'] = '?act=category';
	write_log("修改id为".$id."的分类", $_SESSION['admin_name'],3);
	!$db->updatetable(table('hrtools_category'),$setsqlarr," c_id=".$id."")?adminmsg("修改失败！",0):adminmsg("修改成功！",2,$link);
}
elseif($act == 'del_category')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_hrtools_category($id))
	{
	write_log("删除hr工具分类,共删除".$num."行", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！",0);
	}
}

?>