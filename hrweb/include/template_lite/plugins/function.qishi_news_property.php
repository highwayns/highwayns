<?php
function tpl_function_highway_news_property($params, &$smarty)
{
global $db;
$arr=explode(',',$params['set']);
foreach($arr as $str)
{
$a=explode(':',$str);
	switch ($a[0])
	{
	case "一覧名":
		$aset['listname'] = $a[1];
		break;
	case "名称長さ":
		$aset['titlelen'] = $a[1];
		break;
	case "記号を入力してください":
		$aset['dot'] = $a[1];
		break;
	case "分類ID":
		$aset['ID'] = $a[1];
		break;
	}
}
if (is_array($aset)) $aset=array_map("get_smarty_request",$aset);
$aset['listname']=$aset['listname']?$aset['listname']:"list";
$aset['titlelen']=$aset['titlelen']?intval($aset['titlelen']):8;
if ($aset['ID'])
{
$wheresql=" WHERE id=".intval($aset['ID']);
}
$List = $db->getall("SELECT id,categoryname,category_order FROM ".table('article_property')." ".$wheresql);
$smarty->assign($aset['listname'],$List);
}
?>
