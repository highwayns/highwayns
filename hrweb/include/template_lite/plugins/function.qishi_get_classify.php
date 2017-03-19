<?php
function tpl_function_qishi_get_classify($params, &$smarty)
{
global $db;
$_CAT=get_cache('category');
$arr=explode(',',$params['set']);
foreach($arr as $str)
{
$a=explode(':',$str);
	switch ($a[0])
	{
	case "列表名":
		$aset['listname'] = $a[1];
		break;
	case "类型":
		$aset['act'] = $a[1];
		break;
	case "显示数目":
		$aset['row'] = $a[1];
		break;
	case "名称长度":
		$aset['titlelen'] = $a[1];
		break;
	case "填补字符":
		$aset['dot'] = $a[1];
		break;
	case "id":
		$aset['id'] = $a[1];
		break;
	}
}
if (is_array($aset)) $aset=array_map("get_smarty_request",$aset);
$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
$aset['titlelen']=isset($aset['titlelen'])?intval($aset['titlelen']):18;
$act=$aset['act'];
$aset['dot']=isset($aset['dot'])?$aset['dot']:null;
if (intval($aset['row'])>0) $limit=" LIMIT ".intval($aset['row']);
$list =array();
if ($act=="QS_jobs")
{
	$id=intval($aset['id']);
	$result = $db->query("SELECT * FROM ".table('category_jobs')." where parentid=".$id." ORDER BY category_order desc,id asc".$limit);
	while($row = $db->fetch_array($result))
	{
	$row['categoryname']=cut_str($row['categoryname'],$aset['titlelen'],0,$aset['dot']);
	$row['title']=cut_str($row['categoryname'],$aset['titlelen'],0,$aset['dot']);
	$row['jobcategory']=$row['parentid']=='0'?$row['id'].".0":$row['parentid'].".".$row['id'];
	$list[] = $row;
	}
}
// 楼层
elseif ($act=="QS_jobs_floor")
{
	$id=trim($aset['id']);
	$id=str_replace("_", ",", $id);
	$result = $db->query("SELECT * FROM ".table('category_jobs')." where parentid in (".$id.") ORDER BY category_order desc,id asc".$limit);
	while($row = $db->fetch_array($result))
	{
	$row['categoryname']=cut_str($row['categoryname'],$aset['titlelen'],0,$aset['dot']);
	$row['title']=cut_str($row['categoryname'],$aset['titlelen'],0,$aset['dot']);
	$row['jobcategory']=$row['parentid']=='0'?$row['id'].".0":$row['parentid'].".".$row['id'];
	$list[] = $row;
	}
}
elseif ($act=="QS_jobs_parent")
{
	if (strpos($aset['id'],"-"))
	{
		$arr=explode("-",$aset['id']);
		$idstr=implode(",",$arr);
		if (!preg_match("/^(\d{1,10},)*(\d{1,10})$/",$idstr)) exit("err");
	}
	else
	{
		$idstr=intval($aset['id']);
	}
	if ($idstr=="0")
	{
		$list="";
	}
	else
	{
		$result = $db->query("SELECT parentid FROM ".table('category_jobs')." where id IN (".$idstr.")".$limit);
		while($row = $db->fetch_array($result))
		{
		$list[] =$row['parentid'];
		}
		$list=array_unique($list);
		$list=implode(",",$list);
	}
}
elseif ($act=="QS_district")
{
	
	if (isset($aset['id']))
	{
	$wheresql=" WHERE parentid=".intval($aset['id'])." ";
	}
	$sql = "select * from ".table('category_district')." ".$wheresql."  ORDER BY category_order desc,id asc".$limit;
	$result = $db->query($sql);
	while($row = $db->fetch_array($result))
	{
	$row['categoryname']=cut_str($row['categoryname'],$aset['titlelen'],0,$aset['dot']);
	$row['title']=cut_str($row['categoryname'],$aset['titlelen'],0,$aset['dot']);
	$row['citycategory']=$row['parentid']=='0'?$row['id'].".0":$row['parentid'].".".$row['id'];
	$list[] = $row;
	}
}
elseif ($act=="QS_district_parent")
{
	if (strpos($aset['id'],"-"))
	{
		$arr=explode("-",$aset['id']);
		$idstr=implode(",",$arr);
		if (!preg_match("/^(\d{1,10},)*(\d{1,10})$/",$idstr)) exit("err");
	}
	else
	{
		$idstr=intval($aset['id']);
	}
	if ($idstr=="0")
	{
		$list="";
	}
	else
	{
		$result = $db->query("SELECT parentid FROM ".table('category_district')." where id IN (".$idstr.")".$limit);
		while($row = $db->fetch_array($result))
		{
		$list[] =$row['parentid'];
		}
		$list=array_unique($list);
		$list=implode(",",$list);
	}
}
elseif ($act=="QS_street")
{
	$wheresql=" WHERE c_alias='QS_street' ";
	$sql = "select * from ".table('category')." {$wheresql} ORDER BY c_order desc,c_id asc {$limit}";
	$result = $db->query($sql);
	while($row = $db->fetch_array($result))
	{
	$row['categoryname']=cut_str($row['c_name'],$aset['titlelen'],0,$aset['dot']);
	$list[] = $row;
	}
}
elseif ($act =="QS_major")
{
	$id=intval($aset['id']);
	$result = $db->query("SELECT * FROM ".table('category_major')." where parentid=".$id." ORDER BY category_order desc,id asc".$limit);
	while($row = $db->fetch_array($result))
	{
	$row['categoryname']=cut_str($row['categoryname'],$aset['titlelen'],0,$aset['dot']);
	$row['title']=cut_str($row['categoryname'],$aset['titlelen'],0,$aset['dot']);
	$row['jobcategory']=$row['parentid']=='0'?$row['id'].".0":$row['parentid'].".".$row['id'];
	$list[] = $row;
	}
}
else
{
	if (!empty($_CAT[$act]))
	{
		foreach ($_CAT[$act] as $cat)
		{
		$cat['categoryname']=cut_str($cat['categoryname'],$aset['titlelen'],0,$aset['dot']);
		$list[] = $cat;
		}
		if (intval($aset['row'])>0)
		{
			$list=array_slice($list,0,intval($aset['row']));
		}		
	}
}
$smarty->assign($aset['listname'],$list);
}
?>