<?php
/*********************************************
*导航栏目
* *******************************************/
function tpl_function_qishi_nav($params, &$smarty)
{
	global $db,$_NAV;
	$arr=explode(',',$params['set']);
	foreach($arr as $str)
	{
	$a=explode(':',$str);
		switch ($a[0])
		{
		case "调用名称":
			$aset['alias'] = $a[1];
			break;
		case "列表名":
			$aset['listname'] = $a[1];
			break;
		case "分割":
			$aset['limit'] = $a[1];
			break;
		}
	}
	$aset['listname']=$aset['listname']?$aset['listname']:"list";
	$limit=intval($aset['limit']);
	if($limit>0)
	{
		$navarr=$_NAV[$aset['alias']];
		$smarty->assign($aset['listname'],array_slice($navarr,0,$limit));
		$smarty->assign($aset['listname']."_more",array_slice($navarr,$limit));
	}
	else
	{
		$smarty->assign($aset['listname'],$_NAV[$aset['alias']]);
	}
	
}
?>