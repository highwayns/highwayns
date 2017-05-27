<?php
function tpl_function_highway_news_category($params, &$smarty)
{
global $db,$_CFG;
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
	case "開始位置":
		$aset['start'] = $a[1];
		break;
	case "ニュース大分類":
		$aset['classify'] = $a[1];
		break;
	case "ニュース小类":
		$aset['typeid'] = $a[1];
		break;
	case "ページ":
		$aset['showname'] = $a[1];
		break;
	case "表示数目":
		$aset['limit'] = $a[1];
		break;
	}
}
if (is_array($aset)) $aset=array_map("get_smarty_request",$aset);
$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
$aset['titlelen']=isset($aset['titlelen'])?intval($aset['titlelen']):8;
$aset['start']=isset($aset['start'])?intval($aset['start']):0;
$aset['dot']=isset($aset['dot'])?$aset['dot']:'';
$aset['showname']=isset($aset['showname'])?$aset['showname']:'HW_newslist';
isset($aset['typeid'])? $wheresqlarr['id']=intval($aset['typeid']):'';
isset($aset['classify'])? $wheresqlarr['parentid']=intval($aset['classify']):'';
	if (is_array($wheresqlarr))
	{
		$where_set=' WHERE';
		$comma=$wheresql='';
		foreach ($wheresqlarr as $key => $value)
		{
		$wheresql .=$where_set. $comma.'`'.$key.'`'.'=\''.$value.'\'';
		$comma = ' AND ';
		$where_set='';
		}
	}
if(isset($aset['limit']) && intval($aset['limit'])>0){
	$limit = " limit ".intval($aset['start']).",".intval($aset['limit'])." ";
}else{
	$limit = " ";
}
$result = $db->query("SELECT id,parentid,categoryname,title,description,keywords FROM ".table('article_category')." ".$wheresql." ORDER BY  category_order DESC".$limit);
$list=array();
while($row = $db->fetch_array($result))
{
$row['url']=url_rewrite($aset['showname'],array('id'=>$row['id']));
$row['title_']=$row['categoryname'];
$row['title']=cut_str($row['categoryname'],$aset['titlelen'],0,$aset['dot']);
$list[] = $row;
}
if (isset($aset['typeid']))
{
$list=$list[0];
}
$smarty->assign($aset['listname'],$list);
}
?>
