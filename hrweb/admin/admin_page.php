<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_page_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'page_list';
check_permissions($_SESSION['admin_purview'],"site_page");
$norewrite=array('HW_login');
$nocaching=array('HW_login','HW_jobslist','HW_street','HW_jobtag','HW_resumelist','HW_resumetag','HW_simplelist','HW_simpleresumelist','HW_helpsearch','HW_newssearch');
$smarty->assign('pageheader',"ページ管理");
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
	adminmsg("CallID ".$_POST['alias']." 既に存在します！再入力してください",1);
	exit();
	}
	if (ck_page_file($_POST['file']))
	{
	adminmsg("ファイルパス ".$_POST['file']." 既に存在します！再入力してください",1);
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
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] ="?act=";	
		if ($_POST['mkdir']=="y" && $setsqlarr['html'])
		{
		ck_page_dir($setsqlarr['html']);
		}
	!copy_page($setsqlarr['file'],$setsqlarr['alias'])?adminmsg("作成：".$setsqlarr['file']."ファイル作成失敗，フォルダー権限をチェックしてくださいまたは手動ファイル作成",0):"";
	refresh_page_cache();
	refresh_nav_cache();
	write_log("ページ追加", $_SESSION['admin_name'],3);
	adminmsg("追加成功！",2,$link);
	}
	else
	{
	adminmsg("追加失敗！",0);
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
	adminmsg("CallID ".$_POST['alias']." 既に存在します！再入力してください",1);
	exit();
	}
	if (ck_page_file($_POST['file'],$_POST['id']))
	{
	adminmsg("ファイルパス ".$_POST['file']." 既に存在します！再入力してください",1);
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
	write_log("ページ変更", $_SESSION['admin_name'],3);
	adminmsg("変更成功！",2);
	}
	else
	{
	adminmsg("変更失敗！",0);
	}
}
elseif($act == 'del_page')
{
	check_token();
	$id=$_REQUEST['id'];
	if (empty($id)) adminmsg("項目を選択してください！",0);
	if ($num=del_page($id))
	{
	refresh_page_cache();
	refresh_nav_cache();
	write_log("ページ削除，削除件数".$num."行", $_SESSION['admin_name'],3);
	adminmsg("削除成功！削除件数".$num."行",2);
	}
	else
	{
	adminmsg("削除失敗！".$num,1);
	}
}
elseif($act == 'set_page')
{
	check_token();
	$id =!empty($_POST['id'])?$_POST['id']:adminmsg("ページを選択してください！",1);
	if ($_POST['set_url'])//设置页面链接
	{
		if (set_page_url($id,$_POST['url'],$norewrite))
		{
		refresh_page_cache();
		refresh_nav_cache();
		write_log("ページリンク設定", $_SESSION['admin_name'],3);
		adminmsg("設定成功！",2);		
		}
		else
		{
		adminmsg("設定失敗！",0);
		}
	}
	if ($_POST['set_caching'])//设置页面缓存时间
	{		
		if (set_page_caching($id,$_POST['caching'],$nocaching))
		{
		refresh_page_cache();
		write_log("このページCacheしている", $_SESSION['admin_name'],3);
		adminmsg("設定成功！",2);
		}
		else
		{
		adminmsg("設定失敗！",0);;
		}
	}
}
?>
