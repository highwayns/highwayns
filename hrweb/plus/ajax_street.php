<?php
 /*
 * 74cms ajax 道路索引
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(dirname(__FILE__)).'/include/plus.common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : '';
$listtype=trim($_GET['listtype']);
if($act == 'alphabet')
{
	$alphabet=trim($_GET['x']);	
	if (!empty($alphabet))
	{
	$result = $db->query("select * from ".table('category')." where c_alias='QS_street' AND c_index='{$alphabet}' ");
	while($row = $db->fetch_array($result))
	{
		if ($listtype=="li")
		{
		$htm.="<div class=\"fl-content-li\" type=\"streetid\" code=\"{$row['c_id']}\">{$row['c_name']}</div>";
		}
		else
		{
		$_GET['streetid']=$row['c_id'];
		$url=url_rewrite('QS_street',$_GET);
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
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0) $key=utf8_to_gbk($key);
	$result = $db->query("select * from ".table('category')." where c_alias='QS_street' AND c_name LIKE '%{$key}%' ");
	while($row = $db->fetch_array($result))
	{
		if ($listtype=="li")
		{
		$htm.="<div class=\"fl-content-li\" type=\"streetid\" code=\"{$row['c_id']}\">{$row['c_name']}</div>";
		}
		else
		{
		$_GET['streetid']=$row['c_id'];
		$url=url_rewrite('QS_street',$_GET);
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