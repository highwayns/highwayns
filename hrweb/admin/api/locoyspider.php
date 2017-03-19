<?php
 /*
 * 74cms 火车头采集
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once('../../data/config.php');
require_once('../include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_locoyspider_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'set';
$locoyspider=get_cache('locoyspider');
if ($locoyspider['open']<>"1")
{
exit("请在网站后台开启火车头采集");
}
elseif($act=="news")
{
	require_once(ADMIN_ROOT_PATH.'include/admin_article_fun.php');
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('文章标题不能为空！',1);
	if (ck_article_title($setsqlarr['title']))
	{
	exit("添加失败，新闻标题有重复");
	}
	$setsqlarr['type_id']=trim($_POST['type_id'])?trim($_POST['type_id']):exit('文章所属分类不能为空！');
	$setsqlarr['parentid']=get_article_parentid($setsqlarr['type_id']);
	$setsqlarr['content']=trim($_POST['content'])?trim($_POST['content']):exit('文章内容不能为空！');
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
		exit("添加成功");
		}
		else
		{
		exit("添加失败");
		}
		exit();
}
elseif($act=="jobs")
{
$companyname=isset($_POST['companyname'])?trim($_POST['companyname']):exit('公司名称不能为空！');
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
		exit("添加失败");
		}
	} 
}

?>