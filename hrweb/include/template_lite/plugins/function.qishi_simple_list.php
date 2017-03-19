<?php
/*********************************************
*微招聘
********************************************/
function tpl_function_qishi_simple_list($params, &$smarty)
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
	case "开始位置":
		$aset['start'] = $a[1];
		break;
	case "职位名长度":
		$aset['jobslen'] = $a[1];
		break;
	case "企业名长度":
		$aset['companynamelen'] = $a[1];
		break;
	case "描述长度":
		$aset['brieflylen'] = $a[1];
		break;
	case "填补字符":
		$aset['dot'] = $a[1];
		break;	 
	case "关键字":
		$aset['key'] = $a[1];
		break;
	case "关键字类型":
		$aset['keytype'] = $a[1];
		break;
	case "日期范围":
		$aset['settr'] = $a[1];
		break;
	case "排序":
		$aset['displayorder'] = $a[1];
		break;
	case "分页显示":
		$aset['page'] = $a[1];
		break;
	case "页面":
		$aset['simpleshow'] = $a[1];
		break;	
	case "地区分类":
		$aset['citycategory'] = $a[1];
		break;	
	}
}
$aset=array_map("get_smarty_request",$aset);
$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
$aset['row']=isset($aset['row'])?intval($aset['row']):10;
$aset['start']=isset($aset['start'])?intval($aset['start']):0;
$aset['jobslen']=isset($aset['jobslen'])?intval($aset['jobslen']):8;
$aset['companynamelen']=isset($aset['companynamelen'])?intval($aset['companynamelen']):15;
$aset['brieflylen']=isset($aset['brieflylen'])?intval($aset['brieflylen']):0;
$aset['simpleshow']=isset($aset['simpleshow'])?$aset['simpleshow']:'QS_simpleshow';

if (isset($aset['displayorder']))
{
	if (strpos($aset['displayorder'],'>'))
	{
		$arr=explode('>',$aset['displayorder']);
		// 排序字段
		if($arr[0]=='refreshtime'){
			$arr[0]="refreshtime";
		}
		elseif($arr[0]=="id")
		{
			$arr[0]="id";
		}
		elseif($arr[0]=="click")
		{
			$arr[0]=="click";
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
		$orderbysql=" ORDER BY {$arr[0]} {$arr[1]} ";
		}
	}
}else{
	$orderbysql=" ORDER BY id DESC";
}
$wheresql=" AND audit=1 ";

if (isset($aset['settr']) && $aset['settr']<>'')
{
	$settr=intval($aset['settr']);
	if ($settr>0)
	{
	$settr_val=intval(strtotime("-".$aset['settr']." day"));
	$wheresql.=" AND refreshtime>".$settr_val;
	}
}
if (isset($aset['key']) && !empty($aset['key']))
{
	$aset['key']=help::addslashes_deep(trim($aset['key']));
	if ($aset['keytype']=="1" || $aset['keytype']=="")
	{
		$wheresql.=" AND  jobname like '%{$aset['key']}%'";
		$orderbysql="";
	}
	elseif ($aset['keytype']=="2")
	{
		$wheresql.=" AND  MATCH (`key`) AGAINST ('".fulltextpad($aset['key'])."') ";
		$orderbysql="";
	}
}
if (!empty($aset['citycategory']))
{
		$dsql=$xsql="";
		$arr=explode(",",$aset['citycategory']);
		$arr=array_unique($arr);
		if (count($arr)>10) exit();
		foreach($arr as $sid)
		{
				$cat=explode(".",$sid);
				if (intval($cat[1])===0)
				{
				$dsql.= " OR district =".intval($cat[0]);
				}
				else
				{
				$xsql.= " OR sdistrict =".intval($cat[1]);
				}
				
				
		}
		$wheresql.=" AND  (".ltrim(ltrim($dsql.$xsql),'OR').") ";
}
else
{
	if (isset($aset['district'])  && $aset['district']<>'')
	{
		if (strpos($aset['district'],"-"))
		{
			$or=$orsql="";
			$arr=explode("-",$aset['district']);
			$arr=array_unique($arr);
			if (count($arr)>20) exit();
			$sqlin=implode(",",$arr);
			if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
			{
				$wheresql.=" AND district IN  ({$sqlin}) ";
			}
		}
		else
		{
			$wheresql.=" AND district =".intval($aset['district']);
		}
	}
	if (isset($aset['sdistrict'])  && $aset['sdistrict']<>'')
	{
		if (strpos($aset['sdistrict'],"-"))
		{
			$or=$orsql="";
			$arr=explode("-",$aset['sdistrict']);
			$arr=array_unique($arr);
			if (count($arr)>10) exit();
			$sqlin=implode(",",$arr);
			if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
			{
				$wheresql.=" AND sdistrict IN  ({$sqlin}) ";
			}
		}
		else
		{
			$wheresql.=" AND sdistrict =".intval($aset['sdistrict']);
		}
	}	
}
if (!empty($wheresql))
{
$wheresql=" WHERE ".ltrim(ltrim($wheresql),'AND');
}
if (isset($aset['page']))
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$total_sql="SELECT COUNT(*) AS num FROM ".table('simple').$wheresql;
	//echo $total_sql;
	$total_count=$db->get_total($total_sql);	
	$page = new page(array('total'=>$total_count, 'perpage'=>$aset['row'],'alias'=>'QS_simplelist','getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$aset['start']=($currenpage-1)*$aset['row'];
	$smarty->assign('page',$page->show(3));
	$smarty->assign('total',$total_count);
}
	$limit=" LIMIT ".abs($aset['start']).','.$aset['row'];
	$result = $db->query("SELECT id,jobname,detailed,refreshtime,comname,amount,deadline,district_cn,sdistrict_cn FROM ".table('simple')." ".$wheresql.$orderbysql.$limit);
	$list   = array();
	//echo "SELECT * FROM ".table('jobs')." ".$wheresql.$orderbysql.$limit;
		while($row = $db->fetch_array($result))
		{
		$row['jobname_']=$row['jobname'];
		$row['jobname']=cut_str($row['jobname'],$aset['jobslen'],0,$aset['dot']);
		$row['detailed_']=strip_tags($row['detailed']);
		if ($aset['brieflylen']>0)
			{
				$row['detailed']=cut_str($row['detailed_'],$aset['brieflylen'],0,$aset['dot']);
			}
			else
			{
				$row['detailed']=$row['detailed_'];
			}
		if(strlen($row['detailed_']) > $aset['brieflylen']*2) {
			$row['show_detail'] = true;
		}else{
			$row['show_detail'] = false;
		}
		$row['comname_']=$row['comname'];
		$row['refreshtime_cn']=daterange(time(),$row['refreshtime'],'Y-m-d',"#FF3300");
		$row['comname']=cut_str($row['comname'],$aset['companynamelen'],0,$aset['dot']);
		$row['simple_url']=url_rewrite($aset['simpleshow'],array('id'=>$row['id']));
		$list[] = $row;
		}
		$smarty->assign($aset['listname'],$list);
}
?>