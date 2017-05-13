<?php
function tpl_function_highway_pageinfo($params, &$smarty)
{
global $db,$_CFG;
$arr=explode(',',$params['set']);
foreach($arr as $str)
{
$a=explode(':',$str);
	switch ($a[0])
	{
	case "CALL":
		$aset['alias'] = $a[1];
		break;
	case "一覧名":
		$aset['listname'] = $a[1];
		break;
	case "分類ID":
		$aset['id'] = $a[1];
		break;		
	}
}
if (is_array($aset))$aset=array_map("get_smarty_request",$aset);
$aset['alias']=$aset['alias']?$aset['alias']:"HW_index";
$aset['listname']=$aset['listname']?$aset['listname']:"list";
if ($aset['alias']=="HW_newslist" && $aset['id'])
{
	$sql = "select title,description,keywords from ".table('article_category')." where id = ".intval($aset['id'])." LIMIT  1";
	$info=$db->getone($sql);
}
else
{
$sql = "select title,description,keywords from ".table('page')." where alias = '".$aset['alias']."'  LIMIT  1";
$info=$db->getone($sql);
}
	$info['title']=str_replace('{domain}',$_CFG['site_domain'],$info['title']);
	$info['title']=str_replace('{sitename}',$_CFG['site_name'],$info['title']);
	$info['title']=str_replace('{district}','',$info['title']);
	$info['description']=str_replace('{domain}',$_CFG['site_domain'],$info['description']);
	$info['description']=str_replace('{sitename}',$_CFG['site_name'],$info['description']);
	$info['description']=str_replace('{district}','',$info['description']);
	$info['keywords']=str_replace('{domain}',$_CFG['site_domain'],$info['keywords']);
	$info['keywords']=str_replace('{sitename}',$_CFG['site_name'],$info['keywords']);
	$info['keywords']=str_replace('{district}','',$info['keywords']);
$smarty->assign($aset['listname'],$info);
}
?>
