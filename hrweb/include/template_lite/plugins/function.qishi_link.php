<?php
function tpl_function_qishi_link($params, &$smarty)
{
global $db,$_CFG;
$arr=explode(',',$params['set']);
foreach($arr as $str)
{
$a=explode(':',$str);
	switch ($a[0])
	{
	case "列表名":
		$aset['listname'] = $a[1];
		break;
	case "显示数目":
		$aset['row'] = $a[1];
		break;
	case "开始位置":
		$aset['start'] = $a[1];
		break;
	case "文字长度":
		$aset['len'] = $a[1];
		break;
	case "填补字符":
		$aset['dot'] = $a[1];
		break;
	case "类型":
		$aset['linktype'] = $a[1];
		break;
	case "调用名称":
		$aset['alias'] = $a[1];
		break;
	}
}
	$aset=array_map("get_smarty_request",$aset);
	$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
	$aset['row']=isset($aset['row'])?intval($aset['row']):60;
	$aset['start']=isset($aset['start'])?intval($aset['start']):0;
	$aset['len']=isset($aset['len'])?intval($aset['len']):8;
	$aset['linktype']=isset($aset['linktype'])?intval($aset['linktype']):1;
	$aset['dot']=isset($aset['dot'])?$aset['dot']:'';
	if ($aset['linktype']=="1"){
	$wheresql=" WHERE link_logo='' ";
	}
	else
	{
	$wheresql=" WHERE link_logo<>'' ";
	}
	$wheresql.=" AND display=1 ";
	if ($aset['alias']) $wheresql.=" AND alias='".$aset['alias']."' ";
	$limit=" LIMIT ".intval($aset['start']).','.intval($aset['row']);
	$result = $db->query("SELECT link_url,link_name,link_logo FROM ".table('link')." ".$wheresql." ORDER BY show_order DESC ".$limit);
	$list = array();
	while($row = $db->fetch_array($result))
	{
		$row['title_']=$row['link_name'];
		$row['title']=cut_str($row['link_name'],$aset['len'],0,$aset['dot']);
		$list[] = $row;
	}
unset($arr,$str,$a,$params);
$smarty->assign($aset['listname'],$list);
}
?>