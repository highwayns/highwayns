<?php
function tpl_function_qishi_explain_show($params, &$smarty)
{
	global $db;
	$arr=explode(',',$params['set']);
	foreach($arr as $str)
	{
	$a=explode(':',$str);
		switch ($a[0])
		{
		case "说明页ID":
			$aset['id'] = $a[1];
			break;
		case "列表名":
			$aset['listname'] = $a[1];
			break;
		}
	}
	$aset=array_map("get_smarty_request",$aset);
	$aset['listname']=$aset['listname']?$aset['listname']:"list";
	$sql = "select id,title,seo_keywords,seo_description,content from ".table('explain')." WHERE  id=".intval($aset['id'])." LIMIT 0 , 1";
	$val=$db->getone($sql);
	if (empty($val))
	{
			header("HTTP/1.1 404 Not Found"); 
			$smarty->display("404.htm");
			exit();
	}
		if ($val['seo_keywords']=="")
		{
		$val['keywords']=$val['title'];
		}
		else
		{
		$val['keywords']=$val['seo_keywords'];
		}
		if ($val['seo_description']=="")
		{
		$val['description']=cut_str(strip_tags($val['content']),60,0,"");
		}
		else
		{
		$val['description']=$val['seo_description'];
		}
		$val['content']=htmlspecialchars_decode($val['content'],ENT_QUOTES);

	$smarty->assign($aset['listname'],$val);
}
?>