<?php
function tpl_function_qishi_news_show($params, &$smarty)
{
global $db;
$arr=explode(',',$params['set']);
foreach($arr as $str)
{
$a=explode(':',$str);
	switch ($a[0])
	{
	case "资讯ID":
		$aset['id'] = $a[1];
		break;
	case "列表名":
		$aset['listname'] = $a[1];
		break;
	}
}
$aset=array_map("get_smarty_request",$aset);
$aset['id']=$aset['id']?intval($aset['id']):0;
$aset['listname']=$aset['listname']?$aset['listname']:"list";
unset($arr,$str,$a,$params);
$sql = "select id,content,title,seo_keywords,seo_description,type_id,addtime from ".table('article')." WHERE  id=".intval($aset['id'])." AND  is_display=1 LIMIT 1";
$val=$db->getone($sql);
if (empty($val))
{
	header("HTTP/1.1 404 Not Found"); 
	$smarty->display("404.htm");
	exit();
}
$val['content']=htmlspecialchars_decode($val['content'],ENT_QUOTES);
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
$prev = $db->getone("select id,title from ".table('article')." where id<".$val['id']." and type_id=".$val['type_id']." order by id desc limit 1");
if(!$prev){
	$val['prev'] = 0;
}else{
	$val['prev'] = 1;
	$val['prev_title'] = $prev['title'];
	$val['prev_url'] = url_rewrite("QS_newsshow",array('id'=>$prev['id']));
}
$next = $db->getone("select id,title from ".table('article')." where id>".$val['id']." and type_id=".$val['type_id']." limit 1");
if(!$next){
	$val['next'] = "没有了";
}else{
	$val['next'] = 1;
	$val['next_title'] = $next['title'];
	$val['next_url'] = url_rewrite("QS_newsshow",array('id'=>$next['id']));
}
$smarty->assign($aset['listname'],$val);
}
?>