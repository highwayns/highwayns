<?php
function tpl_function_qishi_help_category($params, &$smarty)
{
global $db;
$arr=explode(',',$params['set']);
foreach($arr as $str)
{
$a=explode(':',$str);
	switch ($a[0])
	{
	case "列表名":
		$aset['listname'] = $a[1];
		break;
	case "名称长度":
		$aset['titlelen'] = $a[1];
		break;
	case "填补字符":
		$aset['dot'] = $a[1];
		break;
	case "大类":
		$aset['classify'] = $a[1];
		break;
	case "小类":
		$aset['typeid'] = $a[1];
		break;
	case "页面":
		$aset['showname'] = $a[1];
		break;
	case "显示数目":
		$aset['num'] = $a[1];
		break;
	}
}
if (is_array($aset)) $aset=array_map("get_smarty_request",$aset);
$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
$aset['titlelen']=isset($aset['titlelen'])?intval($aset['titlelen']):8;
$aset['dot']=isset($aset['dot'])?$aset['dot']:'';
$aset['showname']=isset($aset['showname'])?$aset['showname']:'QS_helplist';
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