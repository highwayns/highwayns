<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'captcha';
check_permissions($_SESSION['admin_purview'],"set_safety");
$smarty->assign('pageheader',"安全設定");
if($act == 'filte')
{
	get_token();
	$smarty->assign('config',$_CFG);
	$smarty->assign('navlabel','filte');
	$smarty->display('safety/admin_safety_filter.htm');
}
if($act == 'ip')
{
	get_token();
	$smarty->assign('config',$_CFG);
	$smarty->assign('navlabel','ip');
	$smarty->display('safety/admin_safety_ip.htm');
}
if($act == 'csrf')
{
	get_token();
	$smarty->assign('config',$_CFG);
	$smarty->assign('navlabel','csrf');
	$smarty->display('safety/admin_safety_csrf.htm');
}
elseif($act == 'setsave')
{
	check_token();
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('config')." SET value='$v' WHERE name='$k'")?adminmsg('設定更新失敗', 1):"";
	}
	refresh_cache('config');
	adminmsg("保存成功！",2);
}
if($act == 'captcha')
{
	get_token();
	$smarty->assign('captcha',get_cache('captcha'));
	$smarty->assign('navlabel','captcha');
	$smarty->display('safety/admin_safety_captcha.htm');
}
elseif($act == 'captcha_save')
{
	check_token();
	if ($_POST['captcha_lang']=='cn')
	{
		$dir =HIGHWAY_ROOT_PATH.'data/font/cn/';
		if($handle = @opendir($dir))
		{
			$i = 0;
			while(false !== ($file = @readdir($handle)))
			{
				if(strcasecmp(substr($file,-4),'.ttf')===0)
				{
					$list[]= $file;
					$i++;
				}
			}
		}
		if (empty($list))
		{
		adminmsg("変更失敗，漢字検証コードは漢字ののTTFファイルを data/font/cn フォルダーにアップロードが必要です",0);
		}
	}
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('captcha')." SET value='$v' WHERE name='$k'")?adminmsg('設定更新失敗', 1):"";
	}
	refresh_cache('captcha');
	write_log("セキュリティ設定", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
?>
