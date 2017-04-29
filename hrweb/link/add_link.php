<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'add';
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
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
	showmsg('リンク申請が停止しました，管理者に連絡してください！',1);
	}
	else
	{
	$setsqlarr['link_name']=trim($_POST['link_name'])?trim($_POST['link_name']):showmsg('リンクの名称を入力してください！',1);
	$setsqlarr['link_url']=trim($_POST['link_url'])?trim($_POST['link_url']):showmsg('リンクアドレスを入力してください！',1);
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
