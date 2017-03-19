<?php
 /*
 * 74cms 帮助
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
require_once(ADMIN_ROOT_PATH.'include/admin_help_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"help");
$smarty->assign('pageheader',"帮助");	
$smarty->assign('act',$act);
if($act == 'list')
{
	get_token();
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
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
	if (empty($id)) adminmsg("请选择项目！",1);
	$n=del_help($id);
	if ($n)
	{
	write_log("删除帮助 共删除 {$n} 行！", $_SESSION['admin_name'],3);
	adminmsg("删除成功 共删除 {$n} 行！",2);
	}
	else
	{
	adminmsg("删除失败！",0);
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
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('您没有填写标题！',1);
	$setsqlarr['type_id']=!empty($_POST['type_id'])?intval($_POST['type_id']):adminmsg('您没有选择分类！',1);
	$setsqlarr['content']=!empty($_POST['content'])?$_POST['content']:adminmsg('您没有内容！',1);
	$setsqlarr['order']=intval($_POST['order']);
	$setsqlarr['addtime']=$timestamp;
	$setsqlarr['parentid']=get_help_parentid($setsqlarr['type_id']);
	$link[0]['text'] = "继续添加";
	$link[0]['href'] = '?act=add&type_id_cn='.trim($_POST['type_id_cn'])."&type_id=".$_POST['type_id'];
	$link[1]['text'] = "返回列表";
	$link[1]['href'] = '?act=list';
	write_log("添加帮助：".$setsqlarr['title'], $_SESSION['admin_name'],3);
	!$db->inserttable(table('help'),$setsqlarr)?adminmsg("添加失败！",0):adminmsg("添加成功！",2,$link);
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
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('您没有填写标题！',1);
	$setsqlarr['type_id']=trim($_POST['type_id'])?intval($_POST['type_id']):0;
	$setsqlarr['content']=!empty($_POST['content'])?$_POST['content']:adminmsg('您没有内容！',1);
	$setsqlarr['order']=intval($_POST['order']);
	$setsqlarr['parentid']=get_help_parentid($setsqlarr['type_id']);
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?act=list';
	$link[1]['text'] = "查看修改结果";
	$link[1]['href'] = "?act=edit&id=".$id;
	write_log("修改id为".$id."的帮助", $_SESSION['admin_name'],3);
	!$db->updatetable(table('help'),$setsqlarr," id=".$id."")?adminmsg("修改失败！",0):adminmsg("修改成功！",2,$link);
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
				!$db->inserttable(table('help_category'),$setsqlarr)?adminmsg("添加失败！",0):"";
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
	$link[1]['text'] = "继续添加分类";
	$link[1]['href'] = "?act=category_add";
	write_log("添加帮助分类，共添加".$num."个分类", $_SESSION['admin_name'],3);
	adminmsg("添加成功！共添加".$num."个分类",2,$link);
	}
}
elseif($act == 'del_category')
{
	check_permissions($_SESSION['admin_purview'],"article_category");
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_category($id))
	{
	write_log("删除帮助分类,共删除 {$num} 个分类", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除 {$num} 个分类",2);
	}
	else
	{
	adminmsg("删除失败！",1);
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
	$setsqlarr['categoryname']=trim($_POST['categoryname'])?trim($_POST['categoryname']):adminmsg('请填写分类名称！',1);
	$setsqlarr['category_order']=!empty($_POST['category_order'])?intval($_POST['category_order']):0;	
	$link[0]['text'] = "查看修改结果";
	$link[0]['href'] = '?act=edit_category&id='.$id;
	$link[1]['text'] = "返回分类管理";
	$link[1]['href'] = '?act=category';
	write_log("修改id为".$id."的帮助分类", $_SESSION['admin_name'],3);
	!$db->updatetable(table('help_category'),$setsqlarr," id='{$id}'")?adminmsg("修改失败！",0):adminmsg("修改成功！",2,$link);
}
?>