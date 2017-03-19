<?php
/*
 * 74cms 友情连接申请
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'add';
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
if ($act=="add")
{
	$sql = "select * from ".table('link_category')."";
	$cat=$db->getall($sql);
	$smarty->assign('cat',$cat);
	$text=get_cache('text');
	$smarty->assign('link_application_txt',$text['link_application_txt']);
	$smarty->assign('random',mt_rand());
	$captcha=get_cache('captcha');
	$smarty->assign('verify_link',$captcha['verify_link']);
	$smarty->display('link/add.htm');
}
elseif ($act=="save")
{	
	$captcha=get_cache('captcha');
	$postcaptcha = trim($_POST['postcaptcha']);
	if($captcha['verify_link']=='1' && empty($postcaptcha))
	{
		showmsg("请填写验证码",1);
 	}
	if ($captcha['verify_link']=='1' &&  strcasecmp($_SESSION['imageCaptcha_content'],$postcaptcha)!=0)
	{
		showmsg("验证码错误",1);
	}
	if ($_CFG['app_link']<>"1")
	{
	showmsg('已停止自助申请链接，请联系网站管理员！',1);
	}
	else
	{
	$setsqlarr['link_name']=trim($_POST['link_name'])?trim($_POST['link_name']):showmsg('您没有填写链接名称！',1);
	$setsqlarr['link_url']=trim($_POST['link_url'])?trim($_POST['link_url']):showmsg('您没有填写链接地址！',1);
	$setsqlarr['link_logo']=trim($_POST['link_logo']);
	$setsqlarr['app_notes']=trim($_POST['app_notes']);
	$setsqlarr['alias']=trim($_POST['alias']);
	$setsqlarr['display']=2;
	$setsqlarr['type_id']=2;
	$link[0]['text'] = "返回网站首页";
	$link[0]['href'] =$_CFG['site_dir'];
	!$db->inserttable(table('link'),$setsqlarr)?showmsg("添加失败！",0):showmsg("添加成功，请等待管理员审核！",2,$link);
	}
}
unset($smarty);
?>