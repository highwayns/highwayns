<?php
function tpl_function_qishi_help_show($params, &$smarty)
{
	global $db;
	$arr=explode(',',$params['set']);
	foreach($arr as $str)
	{
	$a=explode(':',$str);
		switch ($a[0])
		{
		case "ID":
			$aset['id'] = $a[1];
			break;
		case "列表名":
			$aset['listname'] = $a[1];
			break;
		}
	}
	$aset=array_map("get_smarty_request",$aset);
	$aset['listname']=$aset['listname']?$aset['listname']:"list";
	$sql = "select id,type_id,parentid,title,content,click,addtime from ".table('help')." WHERE  id=".intval($aset['id'])." LIMIT   1";
	$val=$db->getone($sql);
	if (empty($val))
	{
			header("HTTP/1.1 404 Not Found"); 
			$smarty->display("404.htm");
			exit();
	}
	$val['keywords']=$val['title'];
	$val['description']=cut_str(strip_tags($val['content']),60,0,"");
	$val['content']=htmlspecialchars_decode($val['content'],ENT_QUOTES);
	$smarty->assign($aset['listname'],$val);
}
?>