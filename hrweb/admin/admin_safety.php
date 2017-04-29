<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'captcha';
check_permissions($_SESSION['admin_purview'],"set_safety");
$smarty->assign('pageheader',"安全设置");
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
		adminmsg("修改失败，使用中文验证码需要把中文汉字的TTF文件上传到 data/font/cn 目录下",0);
		}
	}
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('captcha')." SET value='$v' WHERE name='$k'")?adminmsg('設定更新失敗', 1):"";
	}
	refresh_cache('captcha');
	write_log("配置安全设置", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2);
}
?>
