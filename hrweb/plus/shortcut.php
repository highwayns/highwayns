<?php
require_once(dirname(__FILE__).'/../data/cache_config.php');
$Shortcut = "[InternetShortcut]
URL={$data['site_domain']}{$data['site_dir']}?lnk
IDList= 
IconFile={$data['site_domain']}{$data['site_dir']}favicon.ico
IconIndex=100
[{000214A0-0000-0000-C000-000000000046}]
Prop3=19,2";
header("Content-type: application/octet-stream"); 
header("Content-Disposition: attachment; filename={$data['site_name']}.url;"); 
exit($Shortcut);
?>
