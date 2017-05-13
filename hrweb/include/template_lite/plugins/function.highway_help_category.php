<?php
function tpl_function_highway_help_category($params, &$smarty)
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
	case "大分類":
		$aset['classify'] = $a[1];
		break;
	case "小类":
		$aset['typeid'] = $a[1];
		break;
	case "ページ":
		$aset['showname'] = $a[1];
		break;
	case "表示数目":
		$aset['num'] = $a[1];
		break;
	}
}
if (is_array($aset)) $aset=array_map("get_smarty_request",$aset);
$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
$aset['titlelen']=isset($aset['titlelen'])?intval($aset['titlelen']):8;
$aset['dot']=isset($aset['dot'])?$aset['dot']:'';
$aset['showname']=isset($aset['showname'])?$aset['showname']:'HW_helplist';
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
if(isset($aset['num']) && $aset['num']>0){
	$limitsql = " limit ".$aset['num']." ";
}else{
	$limitsql = "";
}
$result = $db->query("SELECT id,parentid,categoryname,category_order FROM ".table('help_category')." ".$wheresql." ORDER BY  `category_order`  DESC".$limitsql);
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
