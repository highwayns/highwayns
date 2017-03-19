<?php
function tpl_function_qishi_notice_list($params, &$smarty)
{
global $db,$_CFG;
$arrset=explode(',',$params['set']);
foreach($arrset as $str)
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
	case "摘要长度":
		$aset['infolen'] = $a[1];
		break;		
	case "开始位置":
		$aset['start'] = $a[1];
		break;
	case "填补字符":
		$aset['dot'] = $a[1];
		break;
	case "分类":
		$aset['type_id'] = $a[1];
		break;
	case "排序":
		$aset['displayorder'] = $a[1];
		break;
	case "分页显示":
		$aset['paged'] = $a[1];
		break;
	case "页面":
		$aset['showname'] = $a[1];
		break;
	}
}
if (is_array($aset)) $aset=array_map("get_smarty_request",$aset);
$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
$aset['row']=isset($aset['row'])?intval($aset['row']):10;
$aset['start']=isset($aset['start'])?intval($aset['start']):0;
$aset['titlelen']=isset($aset['titlelen'])?intval($aset['titlelen']):15;
$aset['infolen']=isset($aset['infolen'])?intval($aset['infolen']):0;
$aset['showname']=isset($aset['showname'])?$aset['showname']:'QS_noticeshow';
if ($aset['displayorder'])
{
	if (strpos($aset['displayorder'],'>'))
	{
		$arr=explode('>',$aset['displayorder']);
		// 排序字段
		if($arr[0]=='sort'){
			$arr[0]="sort";
		}
		elseif($arr[0]=="id")
		{
			$arr[0]="id";
		}
		else
		{
			$arr[0]="";
		}
		// 排序方式
		if($arr[1]=='desc'){
			$arr[1]="desc";
		}
		elseif($arr[1]=="asc")
		{
			$arr[1]="asc";
		}
		else
		{
			$arr[1]="";
		}
		if ($arr[0] && $arr[1])
		{
		$orderbysql=" ORDER BY ".$arr[0]." ".$arr[1];
		}
	}
}
else
{
	$orderbysql="  ORDER BY `sort` desc , id desc";
}
$wheresql=" WHERE is_display=1 ";
$aset['type_id']?$wheresql.=" AND type_id=".intval($aset['type_id'])." ":'';

if (isset($aset['paged']))
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$total_sql="SELECT COUNT(*) AS num FROM ".table('notice').$wheresql;
	$total_count=$db->get_total($total_sql);
	$pagelist = new page(array('total'=>$total_count, 'perpage'=>$aset['row'],'alias'=>'QS_noticelist','getarray'=>$_GET));
	$currenpage=$pagelist->nowindex;
	$aset['start']=($currenpage-1)*$aset['row'];
		if ($total_count>$aset['row'])
		{
		$smarty->assign('page',$pagelist->show(3));
		}
		$smarty->assign('total',$total_count);
}
$limit=" LIMIT ".abs($aset['start']).','.$aset['row'];
$result = $db->query("SELECT id,title,tit_color,tit_b,is_url,content,addtime,click FROM ".table('notice')." ".$wheresql.$orderbysql.$limit);
//echo "SELECT * FROM ".table('notice')." ".$wheresql.$orderbysql.$limit;
$list=array();
while($row = $db->fetch_array($result))
{
$row['title_']=$row['title'];
$style_color=$row['tit_color']?"color:".$row['tit_color'].";":'';
$style_font=$row['tit_b']=="1"?"font-weight:bold;":'';
$row['title']=cut_str($row['title'],$aset['titlelen'],0,$aset['dot']);
if ($style_color || $style_font)$row['title']="<span style=".$style_color.$style_font.">".$row['title']."</span>";
$row['url'] =$row['is_url']<>"http://"?$row['is_url']:url_rewrite($aset['showname'],array('id'=>$row['id']));
$row['briefly_']=strip_tags($row['content']);
	if ($aset['infolen']>0)
	{
	$row['briefly']=cut_str(strip_tags($row['content']),$aset['infolen'],0,$aset['dot']);
	}
$list[] = $row;
}
$smarty->assign($aset['listname'],$list);
}
?>