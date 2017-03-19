<?php
function tpl_function_qishi_hotword($params, &$smarty)
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
	case "显示数目":
		$aset['row'] = $a[1];
		break;
	case "标题长度":
		$aset['titlelen'] = $a[1];
		break;	
	case "开始位置":
		$aset['start'] = $a[1];
		break;
	case "填补字符":
		$aset['dot'] = $a[1];
		break;
	}
}
if (is_array($aset)) $aset=array_map("get_smarty_request",$aset);
$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
$aset['row']=isset($aset['row'])?intval($aset['row']):10;
$aset['start']=isset($aset['start'])?intval($aset['start']):0;
$aset['titlelen']=isset($aset['titlelen'])?intval($aset['titlelen']):5;
	$orderbysql=" ORDER BY w_hot DESC";
	$limit=" LIMIT ".abs($aset['start']).','.$aset['row'];
	$result = $db->query("SELECT w_id,w_word,w_hot FROM ".table('hotword')." ".$wheresql.$orderbysql.$limit);
	$list=array();
	while($row = $db->fetch_array($result))
	{
		$row['w_word']=cut_str($row['w_word'],$aset['titlelen'],0,$aset['dot']);
		$row['w_word_code']=rawurlencode($row['w_word']);
		$list[] = $row;
	}
	$smarty->assign($aset['listname'],$list);
}
?>