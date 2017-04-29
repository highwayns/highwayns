<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'select_cache';
$smarty->assign('pageheader',"更新缓存");
if ($act=="select_cache")
{
	
	$smarty->display('sys/admin_clear_cache.htm');
}
elseif ($act=="clear_cache")
{
		if ($_POST['type'])
		{
			$smarty->cache = true;
			if (in_array("tplcache",$_POST['type']))
			{			
				$smarty->clear_compiled_tpl();
			}
			if (in_array("datacache",$_POST['type']))
			{
			$smarty->clear_all_cache();
			refresh_cache('config');
			refresh_cache('text');
			refresh_cache('mailconfig');
			refresh_cache('mail_templates');
			refresh_cache('locoyspider');
			refresh_cache('sms_config');
			refresh_cache('sms_templates');
			refresh_cache('captcha');
			refresh_cache('baiduxml');
			refresh_plug_cache();
			refresh_category_cache();
			refresh_page_cache();
			refresh_nav_cache();
			refresh_points_rule_cache();
			makejs_classify();
			}
				$dirs = getsubdirs('../templates');
				foreach ($dirs as $k=> $val)
				{
					$dir="../temp/templates_c/".$val;
					if (!file_exists($dir)) mkdir($dir);
					$dir="../temp/caches/".$val;
					if (!file_exists($dir)) mkdir($dir);
				}
		}
		else
		{
		adminmsg('項目を選択してください！',1);
		}
	adminmsg('更新成功！',2);
}
?>
