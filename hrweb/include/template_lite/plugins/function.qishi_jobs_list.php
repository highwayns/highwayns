<?php
/*********************************************
*骑士职位列表
* *******************************************/
function tpl_function_qishi_jobs_list($params, &$smarty)
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
	case "应届生职位":
		$aset['graduate'] = $a[1];
		break;
	case "职位分类":
		$aset['jobcategory'] = $a[1];
		break;
	case "职位大类":
		$aset['category'] = $a[1];
		break;
	case "职位小类":
		$aset['subclass'] = $a[1];
		break;
	case "地区分类":
		$aset['citycategory'] = $a[1];
		break;
	case "地区大类":
		$aset['district'] = $a[1];
		break;
	case "地区小类":
		$aset['sdistrict'] = $a[1];
		break;
	case "道路":
		$aset['street'] = $a[1];
		break;
	case "写字楼":
		$aset['officebuilding'] = $a[1];
		break;
	case "标签":
		$aset['tag'] = $a[1];
		break;
	case "行业":
		$aset['trade'] = $a[1];
		break;
	case "学历":
		$aset['education'] = $a[1];
		break;
	case "工作经验":
		$aset['experience'] = $a[1];
		break;
	case "工资":
		$aset['wage'] = $a[1];
		break;
	case "职位性质":
		$aset['nature'] = $a[1];
		break;
	case "公司规模":
		$aset['scale'] = $a[1];
		break;
	case "紧急招聘":
		$aset['emergency'] = $a[1];
		break;
	case "推荐":
		$aset['recommend'] = $a[1];
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
	case "会员UID":
		$aset['uid'] = $a[1];
		break;
	case "公司页面":
		$aset['companyshow'] = $a[1];
		break;
	case "职位页面":
		$aset['jobsshow'] = $a[1];
		break;
	case "列表页":
		$aset['listpage'] = $a[1];
		break;
	case "合并":
		$aset['mode'] = $a[1];
		break;
	case "公司列表名":
		$aset['comlistname'] = $a[1];
		break;
	case "公司职位页面":
		$aset['companyjobs'] = $a[1];
		break;
	case "单个公司显示职位数":
		$aset['companyjobs_row'] = $a[1];
		break;
	}

}
$aset=array_map("get_smarty_request",$aset);
$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
$aset['listpage']=isset($aset['listpage'])?$aset['listpage']:"QS_jobslist";
$aset['row']=intval($aset['row'])>0?intval($aset['row']):20;
if ($aset['row']>20)$aset['row']=20;
$aset['companyjobs_row']=intval($aset['companyjobs_row'])>0?intval($aset['companyjobs_row']):3;
$aset['start']=isset($aset['start'])?intval($aset['start']):0;
$aset['jobslen']=isset($aset['jobslen'])?intval($aset['jobslen']):8;
$aset['companynamelen']=isset($aset['companynamelen'])?intval($aset['companynamelen']):15;
$aset['brieflylen']=isset($aset['brieflylen'])?intval($aset['brieflylen']):0;
$aset['companyshow']=isset($aset['companyshow'])?$aset['companyshow']:'QS_companyshow';
$aset['jobsshow']=isset($aset['jobsshow'])?$aset['jobsshow']:'QS_jobsshow';
$aset['companyjobs']=isset($aset['companyjobs'])?$aset['companyjobs']:'QS_companyjobs';
$aset['mode']=isset($aset['mode'])?intval($aset['mode']):0;
$openorderby=false;
if (isset($aset['displayorder']))
{
		$arr=explode('>',$aset['displayorder']);
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
			$arr[1]="desc";
		}
		if ($arr[0]=="rtime")
		{
		$orderbysql=" ORDER BY  refreshtime {$arr[1]} , setmeal_id {$arr[1]}";
		$jobstable=table('jobs_search_rtime');
		}
		elseif ($arr[0]=="stickrtime")
		{
		$orderbysql=" ORDER BY stick {$arr[1]} , refreshtime {$arr[1]}  , setmeal_id {$arr[1]} ";
		$jobstable=table('jobs_search_stickrtime');		
		}
		elseif ($arr[0]=="hot")
		{
		$orderbysql=" ORDER BY  click {$arr[1]} , setmeal_id {$arr[1]}";
		$jobstable=table('jobs_search_hot');		
		}
		elseif ($arr[0]=="scale")
		{
		$orderbysql=" ORDER BY scale {$arr[1]} , refreshtime {$arr[1]} , setmeal_id {$arr[1]}  ";
		$jobstable=table('jobs_search_scale');		
		}
		elseif ($arr[0]=="wage")
		{
		$orderbysql=" ORDER BY  wage {$arr[1]} ,refreshtime {$arr[1]}  , setmeal_id {$arr[1]}";
		$jobstable=table('jobs_search_wage');		
		}
		elseif ($arr[0]=="key")
		{
		$jobstable=table('jobs_search_key');
		}
		elseif ($arr[0]=="null")
		{
		$orderbysql="";
		$jobstable=table('jobs_search_rtime');
		}
		else
		{
		$orderbysql=" ORDER BY stick {$arr[1]} , setmeal_id {$arr[1]} , refreshtime {$arr[1]}";
		$jobstable=table('jobs_search_stickrtime');	
		}
}
else
{
	$orderbysql=" ORDER BY stick DESC , refreshtime DESC , setmeal_id desc ";
	$jobstable=table('jobs_search_stickrtime');
}
//应届生职位	
if (isset($aset['graduate']) && !empty($aset['graduate']))
{
	$wheresql.=" AND graduate=1 ";
}
if (isset($aset['settr']) && $aset['settr']<>'')
{
	$settr=intval($aset['settr']);
	if ($settr>0)
	{
	$settr_val=intval(strtotime("-".$aset['settr']." day"));
	$wheresql.=" AND refreshtime>".$settr_val;
	}
}
if (isset($aset['uid'])  && $aset['uid']<>'')
{
	$wheresql.=" AND uid=".intval($aset['uid']);
}
if (isset($aset['emergency'])  && $aset['emergency']<>'')
{
	$wheresql.=" AND emergency=".intval($aset['emergency']);
}
if (isset($aset['recommend']) && $aset['recommend']<>'')
{
	$wheresql.=" AND recommend=".intval($aset['recommend']);
}
if (isset($aset['nature']) && $aset['nature']<>'')
{
	if (strpos($aset['nature'],"-"))
	{
		$or=$orsql="";
		$arr=explode("-",$aset['nature']);
		if (count($arr)>10) exit();
		$sqlin=implode(",",$arr);
		if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
		{
		$wheresql.=" AND nature IN  (".$sqlin.") ";
		}
	}
	else
	{
	$wheresql.=" AND nature=".intval($aset['nature'])." ";
	}
}
if (isset($aset['scale']) && $aset['scale']<>'')
{
	$wheresql.=" AND scale=".intval($aset['scale']);
}
if (isset($aset['education']) && $aset['education']<>'')
{
	$wheresql.=" AND education=".intval($aset['education']);
}
if (isset($aset['wage'])  && $aset['wage']<>'')
{
	$wheresql.=" AND wage=".intval($aset['wage']);
}
if (isset($aset['experience'])  && $aset['experience']<>'')
{
	$wheresql.=" AND experience=".intval($aset['experience']);
}
if (isset($aset['trade']) && $aset['trade']<>'')
{
	if (strpos($aset['trade'],"_"))
	{
		$or=$orsql="";
		$arr=explode("_",$aset['trade']);
		$arr=array_unique($arr);
		if (count($arr)>10) exit();
		$sqlin=implode(",",$arr);
		if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
		{
		$wheresql.=" AND trade IN  ({$sqlin}) ";
		}
	}
	else
	{
	$wheresql.=" AND trade=".intval($aset['trade'])." ";
	}
}
if (!empty($aset['citycategory']))
{
		$dsql=$xsql="";
		$arr=explode("_",$aset['citycategory']);
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
if (isset($aset['street']) && $aset['street']<>'')
{
	$wheresql.=" AND street=".intval($aset['street']);
}
if (isset($aset['officebuilding']) && $aset['officebuilding']<>'')
{
	$wheresql.=" AND officebuilding=".intval($aset['officebuilding']);
}
if (!empty($aset['jobcategory']))
{
	$dsql=$xsql="";
	$arr=explode("_",$aset['jobcategory']);
	$arr=array_unique($arr);
	if (count($arr)>10) exit();
	foreach($arr as $sid)
	{
		$cat=explode(".",$sid);
		if (intval($cat[2])===0)
		{
		$dsql.= " OR category =".intval($cat[1]);
		}
		else
		{
		$xsql.= " OR subclass =".intval($cat[2]);
		}
	}
	$wheresql.=" AND  (".ltrim(ltrim($dsql.$xsql),'OR').") ";
}
else
{
			if (isset($aset['category'])  && $aset['category']<>'')
			{
				if (strpos($aset['category'],"-"))
				{
					$or=$orsql="";
					$arr=explode("-",$aset['category']);
					$arr=array_unique($arr);
					if (count($arr)>10) exit();
					$sqlin=implode(",",$arr);
					if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
					{
					$wheresql.=" AND topclass IN  ({$sqlin}) ";
					}
				}
				else
				{
					$wheresql.=" AND topclass = ".intval($aset['category']);
				}
			}
			if (isset($aset['subclass'])  && $aset['subclass']<>'')
			{
				if (strpos($aset['subclass'],"-"))
				{
					$or=$orsql="";
					$arr=explode("-",$aset['subclass']);
					$arr=array_unique($arr);
					if (count($arr)>10) exit();
					$sqlin=implode(",",$arr);
					if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
					{
						$wheresql.=" AND category IN  ({$sqlin}) ";
					}
				}
				else
				{
					$wheresql.=" AND category = ".intval($aset['subclass']);
				}
			}
}
if (isset($aset['key']) && !empty($aset['key']))
{
	if ($_CFG['jobsearch_purview']=='2')
	{
		if ($_SESSION['username']=='')
		{
		header("Location: ".url_rewrite('QS_login')."?url=".urlencode($_SERVER["REQUEST_URI"]));
		}
	}
	$key=help::addslashes_deep(trim($aset['key']));
	if ($_CFG['jobsearch_type']=='1')
	{
			$akey=explode(' ',$key);
			if (count($akey)>1)
			{
			$akey=array_filter($akey);
			$akey=array_slice($akey,0,2);
			$akey=array_map("fulltextpad",$akey);
			$key='+'.implode(' +',$akey);
			$mode=' IN BOOLEAN MODE';
			}
			else
			{
			$key=fulltextpad($key);
			$mode=' ';
			}
			$wheresql.=" AND  MATCH (`key`) AGAINST ('{$key}'{$mode}) ";
	}
	else
	{
			$wheresql.=" AND likekey LIKE '%{$key}%' ";
	}
	$orderbysql=" ORDER BY refreshtime DESC,id desc ";
	$jobstable=table('jobs_search_key');
}
/* 搜索 时间范围 */
$moth=intval($_CFG['search_time']);
if($moth>0)
{
	$moth_time=$moth*3600*24*30;
	$time=time()-$moth_time;
	$wheresql.=" AND refreshtime>$time ";
}
if (!empty($aset['tag']))
{
	if (strpos($aset['tag'],","))
	{
		$or=$orsql="";
		$arr=explode(",",$aset['tag']);
		$sqlin=implode(",",$arr);
		if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
		{
			$joinwheresql_tag.=" AND tag IN  ({$sqlin}) ";
		}
	}
	else
	{
	$joinwheresql_tag.=" AND tag=".intval($aset['tag']);
	}
	
	if (!empty($joinwheresql_tag))
	{
	$joinwheresql_tag=" WHERE ".ltrim(ltrim($joinwheresql_tag),'AND');
	}
	$joinsql=$joinsql==""?"  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('jobs_tag')." {$joinwheresql_tag} ) AS g ON  r.id=g.pid ":$joinsql."  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('jobs_tag')." {$joinwheresql_tag} )AS g ON  r.id=g.pid ";
}
if (!empty($wheresql))
{
$wheresql=" WHERE ".ltrim(ltrim($wheresql),'AND');
}
if (isset($aset['page']))
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$total_sql="SELECT COUNT(*) AS num FROM {$jobstable} {$wheresql}";
	//echo $total_sql;
	$total_count=$db->get_total($total_sql);	
	if ($_CFG['jobs_list_max']>0)
	{
		$total_count>intval($_CFG['jobs_list_max']) && $total_count=intval($_CFG['jobs_list_max']);
	}
	$page = new page(array('total'=>$total_count, 'perpage'=>$aset['row'],'alias'=>$aset['listpage'],'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$aset['start']=abs($currenpage-1)*$aset['row'];
	if ($total_count>$aset['row'])
	{
	$smarty->assign('page',$page->show(8));
	$smarty->assign('pagemin',$page->show(7));
	$smarty->assign('pagenow',$page->show(6));
	}
	$smarty->assign('total',$total_count);
}
	$limit=" LIMIT {$aset['start']} , {$aset['row']}";
	$list = $id = $com_list = array();
	$idresult = $db->query("SELECT id FROM {$jobstable} ".$wheresql.$orderbysql.$limit);
	//echo "SELECT id FROM {$jobstable} ".$wheresql.$orderbysql.$limit;
	
	while($row = $db->fetch_array($idresult))
	{
	$id[]=$row['id'];
	}
	if (!empty($id))
	{
		$wheresql=" WHERE id IN (".implode(',',$id).") ";
		$result = $db->query("SELECT id,jobs_name,recommend,emergency,stick,highlight,companyname,company_id,company_audit,nature_cn,sex_cn,age,amount,category_cn,graduate,trade_cn,scale,scale_cn,district_cn,street_cn,tag_cn,education_cn,experience_cn,wage,wage_cn,contents,setmeal_id,setmeal_name,refreshtime,click FROM ".table('jobs')." AS r ".$joinsql.$wheresql.$orderbysql);
		while($row = $db->fetch_array($result))
		{
			$row['jobs_name_']=$row['jobs_name'];
			$row['refreshtime_cn']=daterange(time(),$row['refreshtime'],'Y-m-d',"#FF3300");
			$row['jobs_name']=cut_str($row['jobs_name'],$aset['jobslen'],0,$aset['dot']);
			if (!empty($row['highlight']))
			{
				$row['jobs_name']="<span style=\"color:{$row['highlight']}\">{$row['jobs_name']}</span>";
			}
			if ($aset['brieflylen']>0)
			{
				$row['briefly']=cut_str(strip_tags($row['contents']),$aset['brieflylen'],0,$aset['dot']);
			}
			else
			{
				$row['briefly']=strip_tags($row['contents']);
			}
			$row['amount']=$row['amount']=="0"?'若干':$row['amount'];
			$row['briefly_']=strip_tags($row['contents']);
			$row['companyname_']=$row['companyname'];
			$row['companyname']=cut_str($row['companyname'],$aset['companynamelen'],0,$aset['dot']);
			$row['jobs_url']=url_rewrite($aset['jobsshow'],array('id'=>$row['id']));
			$row['company_url']=url_rewrite($aset['companyshow'],array('id'=>$row['company_id']));
			if ($row['tag_cn'])
			{
				$tag_cn=explode(',',$row['tag_cn']);
				$row['tag_cn']=$tag_cn;
			}
			else
			{
				$row['tag_cn']=array();
			}
			//合并公司 显示模式
			if($aset['mode']==1)
			{
				//统计单个公司符合条件职位数
				$count_com = $db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs')."  WHERE  company_id=".$row['company_id']);
				$row['count']= $count_com;
				$row['count_url']= $row['company_url'];
				$list[$row['company_id']][] = $row;
			}
			//职位列表 显示模式
			else
			{
				$list[] = $row;
			}
		}
	}
	else
	{
		$list=array();
	}
	$smarty->assign($aset['listname'],$list);
}
?>