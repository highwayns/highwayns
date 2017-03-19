<?php
 /*
 * 74cms 清除缓存
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
		adminmsg('请选择项目！',1);
		}
	adminmsg('更新成功！',2);
}
?>