<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_page_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'page_list';
check_permissions($_SESSION['admin_purview'],"site_page");
$norewrite=array('HW_login');
$nocaching=array('HW_login','HW_jobslist','HW_street','HW_jobtag','HW_resumelist','HW_resumetag','HW_simplelist','HW_simpleresumelist','HW_helpsearch','HW_newssearch');
$smarty->assign('pageheader',"页面管理");
if($act == 'page_list')
{
	get_token();
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$total_sql="SELECT COUNT(*) AS num FROM ".table('page');
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_page($offset,$perpage,$wheresql.$oederbysql);
	$smarty->assign('list',$list);
	$smarty->assign('page',$page->show(3));
	$smarty->assign('navlabel',"list");
	$smarty->display('page/admin_page.htm');
}
elseif($act == 'add_page')
{
	get_token();
	$smarty->assign('navlabel',"add");
	$smarty->display('page/admin_page_add.htm');
}
elseif($act == 'add_page_save')
{
	check_token();
    substr($_POST['alias'],0,3)=='HW_'?adminmsg('Call名は HW_ 含まれている！',1):'';
	if (ck_page_alias($_POST['alias']))
	{
	adminmsg("调用ID ".$_POST['alias']." 已经存在！请重新填写",1);
	exit();
	}
	if (ck_page_file($_POST['file']))
	{
	adminmsg("文件路径 ".$_POST['file']." 已经存在！请重新填写",1);
	exit();
	}
$setsqlarr['systemclass']=0;
$setsqlarr['pagetpye']=trim($_POST['pagetpye'])?trim($_POST['pagetpye']):1;
$setsqlarr['alias']=trim($_POST['alias'])?trim($_POST['alias']):adminmsg('Call IDを入力してください！',1);
$setsqlarr['pname']=trim($_POST['pname'])?trim($_POST['pname']):adminmsg('ページ名を入力してください！',1);
$setsqlarr['tag']=trim($_POST['tag']);
$setsqlarr['url']=trim($_POST['url'])?trim($_POST['url']):0;
$setsqlarr['file']=trim($_POST['file'])?trim($_POST['file']):adminmsg('ファイルパスを入力してください！',1);
$setsqlarr['tpl']=trim($_POST['tpl'])?trim($_POST['tpl']):adminmsg('テンプレートパスを入力してください！',1);
$setsqlarr['rewrite']=trim($_POST['rewrite']);
$setsqlarr['caching']=intval($_POST['caching']);
$setsqlarr['title']=trim($_POST['title']);
$setsqlarr['keywords']=trim($_POST['keywords']);
$setsqlarr['description']=trim($_POST['description']);
	if ($db->inserttable(table('page'),$setsqlarr))
	{
	$link[0]['text'] = "返回列表";
	$link[0]['href'] ="?act=";	
		if ($_POST['mkdir']=="y" && $setsqlarr['html'])
		{
		ck_page_dir($setsqlarr['html']);
		}
	!copy_page($setsqlarr['file'],$setsqlarr['alias'])?adminmsg("新建：".$setsqlarr['file']."文件失败，请检查目录权限或者手动建立文件",0):"";
	refresh_page_cache();
	refresh_nav_cache();
	write_log("添加页面", $_SESSION['admin_name'],3);
	adminmsg("添加成功！",2,$link);
	}
	else
	{
	adminmsg("添加失败！",0);
	}
}
elseif($act == 'edit_page')
{
	get_token();
	$smarty->assign('list',get_page_one(intval($_GET['id'])));
	$smarty->display('page/admin_page_edit.htm');
}
elseif($act == 'edit_page_save')
{
	check_token();
	if ($_POST['systemclass']<>"1")//非系统内置
	{
	$setsqlarr['pagetpye']=trim($_POST['pagetpye'])?trim($_POST['pagetpye']):1;
	$setsqlarr['alias']=trim($_POST['alias'])?trim($_POST['alias']):adminmsg('Call IDを入力してください！',1);
	substr($_POST['alias'],0,3)=='HW_'?adminmsg('Call名は HW_ 含まれている！',1):'';
	}
$setsqlarr['pname']=trim($_POST['pname'])?trim($_POST['pname']):adminmsg('ページ名を入力してください！',1);
$setsqlarr['tag']=trim($_POST['tag']);
$setsqlarr['url']=trim($_POST['url'])?trim($_POST['url']):0;
$setsqlarr['file']=trim($_POST['file'])?trim($_POST['file']):adminmsg('ファイルパスを入力してください！',1);
$setsqlarr['tpl']=trim($_POST['tpl'])?trim($_POST['tpl']):adminmsg('テンプレートパスを入力してください！',1);
$setsqlarr['rewrite']=trim($_POST['rewrite']);
$setsqlarr['caching']=intval($_POST['caching']);
$setsqlarr['title']=trim($_POST['title']);
$setsqlarr['keywords']=trim($_POST['keywords']);
$setsqlarr['description']=trim($_POST['description']);
	 if (in_array(trim($_POST['alias']),$nohtml) && $setsqlarr['url']=='2')
	 {
	 $setsqlarr['url']=0;
	 }
	 if (in_array(trim($_POST['alias']),$norewrite) && $setsqlarr['url']=='1')
	 {
	 $setsqlarr['url']=0;
	 }
	 if (in_array(trim($_POST['alias']),$nocaching))
	 {
	 $setsqlarr['caching']=0;
	 }
	if (ck_page_alias($_POST['alias'],$_POST['id']))
	{
	adminmsg("调用ID ".$_POST['alias']." 已经存在！请重新填写",1);
	exit();
	}
	if (ck_page_file($_POST['file'],$_POST['id']))
	{
	adminmsg("文件路径 ".$_POST['file']." 已经存在！请重新填写",1);
	exit();
	}
$wheresql=" id='".intval($_POST['id'])."'";
	if ($_POST['mkdir']=="y"  && $setsqlarr['html'])
	{
	ck_page_dir($setsqlarr['html']);
	}
	refresh_page_cache();
	refresh_nav_cache();
	if ($db->updatetable(table('page'),$setsqlarr,$wheresql))
	{
	refresh_page_cache();
	write_log("修改页面", $_SESSION['admin_name'],3);
	adminmsg("修改成功！",2);
	}
	else
	{
	adminmsg("修改失败！",0);
	}
}
elseif($act == 'del_page')
{
	check_token();
	$id=$_REQUEST['id'];
	if (empty($id)) adminmsg("请选择项目！",0);
	if ($num=del_page($id))
	{
	refresh_page_cache();
	refresh_nav_cache();
	write_log("删除页面，共删除".$num."行", $_SESSION['admin_name'],3);
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！".$num,1);
	}
}
elseif($act == 'set_page')
{
	check_token();
	$id =!empty($_POST['id'])?$_POST['id']:adminmsg("你没有选择页面！",1);
	if ($_POST['set_url'])//设置页面链接
	{
		if (set_page_url($id,$_POST['url'],$norewrite))
		{
		refresh_page_cache();
		refresh_nav_cache();
		write_log("设置页面链接", $_SESSION['admin_name'],3);
		adminmsg("设置成功！",2);		
		}
		else
		{
		adminmsg("设置失败！",0);
		}
	}
	if ($_POST['set_caching'])//设置页面缓存时间
	{		
		if (set_page_caching($id,$_POST['caching'],$nocaching))
		{
		refresh_page_cache();
		write_log("这页页面缓存", $_SESSION['admin_name'],3);
		adminmsg("设置成功！",2);
		}
		else
		{
		adminmsg("设置失败！",0);;
		}
	}
}
?>
