<?php
define('IN_HIGHWAY', true);
require_once(dirname(dirname(__FILE__)).'/include/plus.common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : '';
$listtype=trim($_GET['listtype']);
if($act == 'alphabet')
{
	$alphabet=trim($_GET['x']);	
	if (!empty($alphabet))
	{
	$result = $db->query("select * from ".table('category')." where c_alias='HW_street' AND c_index='{$alphabet}' ");
	while($row = $db->fetch_array($result))
	{
		if ($listtype=="li")
		{
		$htm.="<div class=\"fl-content-li\" type=\"streetid\" code=\"{$row['c_id']}\">{$row['c_name']}</div>";
		}
		else
		{
		$_GET['streetid']=$row['c_id'];
		$url=url_rewrite('HW_street',$_GET);
		$htm.="<div class=\"fl-content-li\" type=\"streetid\" code=\"{$row['c_id']}\">{$row['c_name']}</div>";
		}
	}
	if (empty($htm))
	{
	$htm="<div class=\"fl-content-li-nostreet\">没有找到首字母为：<span class=\"le\">{$alphabet}</span> 的道路！</div>";
	}
	exit($htm);
	}
}
elseif($act == 'key')
{
	$key=trim($_GET['key']);
	if (!empty($key))
	{
	if (strcasecmp(HIGHWAY_DBCHARSET,"utf8")!=0) $key=utf8_to_gbk($key);
	$result = $db->query("select * from ".table('category')." where c_alias='HW_street' AND c_name LIKE '%{$key}%' ");
	while($row = $db->fetch_array($result))
	{
		if ($listtype=="li")
		{
		$htm.="<div class=\"fl-content-li\" type=\"streetid\" code=\"{$row['c_id']}\">{$row['c_name']}</div>";
		}
		else
		{
		$_GET['streetid']=$row['c_id'];
		$url=url_rewrite('HW_street',$_GET);
		$htm.="<div class=\"fl-content-li\" type=\"streetid\" code=\"{$row['c_id']}\">{$row['c_name']}</div>";
		};
	}
	if (empty($htm))
	{
	$htm="<div class=\"fl-content-li-nostreet\">没有找到关键字为：<span class=\"le\">{$key}</span> 相关道路！</div>";
	}
	exit($htm);
	}
}
?>
