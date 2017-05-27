<?php
define('IN_HIGHWAY', true);
require_once('../../data/config.php');
require_once('../include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_locoyspider_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'set';
$locoyspider=get_cache('locoyspider');
if ($locoyspider['open']<>"1")
{
exit("火車頭を有効にしてください");
}
elseif($act=="news")
{
	require_once(ADMIN_ROOT_PATH.'include/admin_article_fun.php');
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('文書タイトルを入力してください！',1);
	if (ck_article_title($setsqlarr['title']))
	{
	exit("追加失敗，ニュースタイトル重複");
	}
	$setsqlarr['type_id']=trim($_POST['type_id'])?trim($_POST['type_id']):exit('文書所属分類が必須！');
	$setsqlarr['parentid']=get_article_parentid($setsqlarr['type_id']);
	$setsqlarr['content']=trim($_POST['content'])?trim($_POST['content']):exit('文書の内容を入力してください！');
	$setsqlarr['tit_color']=intval($_POST['tit_color']);
	$setsqlarr['tit_b']=intval($_POST['tit_b']);
	$setsqlarr['author']=trim($_POST['author']);
	$setsqlarr['source']=trim($_POST['source']);
		//判断是否设置，否则启用系统默认
		if ($_POST['focos']=="")
		{
		$setsqlarr['focos']=$locoyspider['article_focos'];
		}
		else
		{
		$setsqlarr['focos']=intval($_POST['focos']);
		}
		//判断是否设置，否则启用系统默认
		if ($_POST['is_display']=="")
		{
		$setsqlarr['is_display']=$locoyspider['article_display'];
		}
		else
		{
		$setsqlarr['is_display']=intval($_POST['is_display']);
		}
	$setsqlarr['is_url']=trim($_POST['is_url'])==""? "http://":trim($_POST['is_url']);
	$setsqlarr['seo_keywords']=trim($_POST['seo_keywords']);
	$setsqlarr['seo_description']=trim($_POST['seo_description']);
	$setsqlarr['article_order']=trim($_POST['article_order']);
	$setsqlarr['click']=intval($_POST['click']);
	$setsqlarr['Small_img']=trim($_POST['Small_img']);
	$setsqlarr['addtime']=$timestamp;
	$setsqlarr['robot']=1;
		if ($db->inserttable(table('article'),$setsqlarr))
		{
		exit("追加成功");
		}
		else
		{
		exit("追加失敗");
		}
		exit();
}
elseif($act=="jobs")
{
$companyname=isset($_POST['companyname'])?trim($_POST['companyname']):exit('会社名が必須！');
$companyinfo=get_companyinfo($companyname);
	if ($companyinfo)
	{
		locoyspider_addjobs($companyinfo);
	}
	else
	{
		if (locoyspider_addcompany($companyname))
		{
		$companyinfo=get_companyinfo($companyname);
		locoyspider_addjobs($companyinfo);
		}
		else
		{
		exit("追加失敗");
		}
	} 
}

?>
