<?php
function tpl_function_highway_resume_list($params, &$smarty)
{
global $db,$_CFG;
$arrset=explode(',',$params['set']);
foreach($arrset as $str)
{
$a=explode(':',$str);
	switch ($a[0])
	{
	case "一覧名":
		$aset['listname'] = $a[1];
		break;
	case "表示数目":
		$aset['row'] = $a[1];
		break;
	case "新卒履歴書":
		$aset['campu_sresume'] = $a[1];
		break;
	case "更新時間":
		$aset['refreshtime'] = $a[1];
		break;
	case "開始位置":
		$aset['start'] = $a[1];
		break;
	case "姓名長さ":
		$aset['namelen'] = $a[1];
		break;
	case "特徴説明長さ":
		$aset['specialtylen'] = $a[1];
		break;
	case "意向職位長さ":
		$aset['jobslen'] = $a[1];
		break;
	case "専門長さ":
		$aset['majorlen'] = $a[1];
		break;
	case "記号を入力してください":
		$aset['dot'] = $a[1];
		break;
	case "日期範囲":
		$aset['settr'] = $a[1];
		break;
	case "職位分類":
		$aset['jobcategory'] = trim($a[1]);
		break;
	case "職位大分類":
		$aset['category'] = trim($a[1]);
		break;
	case "職位小类":
		$aset['subclass'] = trim($a[1]);
		break;
	case "地区分類":
		$aset['citycategory'] = trim($a[1]);
		break;
	case "地区大分類":
		$aset['district'] = $a[1];
		break;
	case "地区小分類":
		$aset['sdistrict'] = $a[1];
		break;
	case "業界":
		$aset['trade'] = trim($a[1]);
		break;
	case "専門":
		$aset['major'] = trim($a[1]);
		break;
	case "タグ":
		$aset['tag'] = $a[1];
		break;
	case "学歴":
		$aset['education'] = $a[1];
		break;
	case "仕事経験":
		$aset['experience'] = $a[1];
		break;
	case "級別":
		$aset['talent'] = $a[1];
		break;
	case "性别":
		$aset['sex'] = $a[1];  // 添加搜索条件  男 女
		break;
	case "写真":
		$aset['photo'] = $a[1];
		break;
	case "キーワード":
		$aset['key'] = $a[1];
		break;
	case "ソート":
		$aset['displayorder'] = $a[1];
		break;
	case "ページごと表示":
		$aset['paged'] = $a[1];
		break;
	case "ページ":
		$aset['showname'] = $a[1];
		break;
	case "一覧ページ":
		$aset['listpage'] = $a[1];
		break;
	case "閲覧済み履歴書":
		$aset['readresume'] = $a[1];
		break;
	}
}
if (is_array($aset)) $aset=array_map("get_smarty_request",$aset);
$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
$aset['row']=intval($aset['row'])>0?intval($aset['row']):20;
if ($aset['row']>20)$aset['row']=20;
$aset['start']=isset($aset['start'])?intval($aset['start']):0;
$aset['namelen']=isset($aset['namelen'])?intval($aset['namelen']):4;
$aset['specialtylen']=isset($aset['specialtylen'])?intval($aset['specialtylen']):0;
$aset['jobslen']=isset($aset['jobslen'])?intval($aset['jobslen']):0;
$aset['majorlen']=isset($aset['majorlen'])?intval($aset['majorlen']):50;
$aset['dot']=isset($aset['dot'])?$aset['dot']:null;
$aset['showname']=isset($aset['showname'])?$aset['showname']:'HW_resumeshow';
$aset['listpage']=isset($aset['listpage'])?$aset['listpage']:'HW_resumelist';
$resumetable=table('resume_search_rtime');
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
		$orderbysql=" ORDER BY r.refreshtime {$arr[1]},id desc ";
	}
}
else
{
	$orderbysql=" ORDER BY r.refreshtime desc,id desc ";
}
//应届生简历
if(isset($aset['campu_sresume']) && !empty($aset['campu_sresume']))
{
	$wheresql.=" AND r.experience='-1' ";
}
//更新时间
if(isset($aset['refreshtime']) && !empty($aset['refreshtime']))
{
	$refreshtime_min = strtotime("-".$aset['refreshtime']."day");
	$wheresql.=" AND r.refreshtime > {$refreshtime_min} ";
}
if (!empty($aset['category']) || !empty($aset['subclass']) || !empty($aset['jobcategory']))
{
	if (!empty($aset['jobcategory']))
	{
					$dsql=$xsql="";
					$arr=explode("_",$aset['jobcategory']);
					$arr=array_unique($arr);
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
					$joinwheresql.=" WHERE ".ltrim(ltrim($dsql.$xsql),'OR');
	}
	else
	{
					if (!empty($aset['category']))
					{
						if (strpos($aset['category'],"-"))
						{
							$or=$orsql="";
							$arr=explode("-",$aset['category']);
							$sqlin=implode(",",$arr);
							if (count($arr)>10) exit();
							$sqlin=implode(",",$arr);
							if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
							{
								$joinwheresql.=" AND topclass IN  ({$sqlin}) ";
							}
						}
						else
						{
							$joinwheresql.=" AND  topclass=".intval($aset['category']);
						}
					}
					if (!empty($aset['subclass']))
					{
						if (strpos($aset['subclass'],"-"))
						{
							$or=$orsql="";
							$arr=explode("-",$aset['subclass']);
							if (count($arr)>10) exit();
							$sqlin=implode(",",$arr);
							if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
							{
								$joinwheresql.=" AND category IN  ({$sqlin}) ";
							}
						}
						else
						{
						$joinwheresql.=" AND category=".intval($aset['subclass']);
						}
					}
					if (!empty($joinwheresql))
					{
					$joinwheresql=" WHERE ".ltrim(ltrim($joinwheresql),'AND');
					}
					
	}
	$joinsql="  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('resume_jobs')." {$joinwheresql} )AS j ON  r.id=j.pid ";
}
if (!empty($aset['trade']))
{
	
	if (strpos($aset['trade'],"_"))
	{
		$or=$orsql="";
		$arr=explode("_",$aset['trade']);
		if (count($arr)>10) exit();
		$sqlin=implode(",",$arr);
		if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
		{
			$joinwheresql_trade.=" AND trade IN  ({$sqlin}) ";
		}
	}
	else
	{
	$joinwheresql_trade.=" AND trade=".intval($aset['trade']);
	}
	
	if (!empty($joinwheresql_trade))
	{
	$joinwheresql_trade=" WHERE ".ltrim(ltrim($joinwheresql_trade),'AND');
	}
	$joinsql=$joinsql==""?"  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('resume_trade')." {$joinwheresql_trade} )AS t ON  r.id=t.pid ":$joinsql."  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('resume_trade')." {$joinwheresql_trade} )AS t ON  r.id=t.pid ";
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
// major 专业搜索
if (!empty($aset['major']))
{
	
	if (strpos($aset['major'],"_"))
	{
		$or=$orsql="";
		$arr=explode("_",$aset['major']);
		if (count($arr)>10) exit();
		$sqlin=implode(",",$arr);
		if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
		{
			$wheresql.=" AND r.major IN  ({$sqlin}) ";
		}
	}
	else
	{
	$wheresql.=" AND r.major=".intval($aset['major']);
	}	
}
if (!empty($aset['district']) || !empty($aset['sdistrict']) || !empty($aset['citycategory']))
{
	if (!empty($aset['citycategory']))
	{
					$dsql=$xsql="";
					$arr=explode("_",$aset['citycategory']);
					$arr=array_unique($arr);
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
					$djoinwheresql.=" WHERE ".ltrim(ltrim($dsql.$xsql),'OR');
	}
	else
	{
					if (!empty($aset['district']))
					{
						if (strpos($aset['district'],"-"))
						{
							$or=$orsql="";
							$arr=explode("-",$aset['district']);
							$sqlin=implode(",",$arr);
							if (count($arr)>10) exit();
							$sqlin=implode(",",$arr);
							if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
							{
								$djoinwheresql.=" AND district IN  ({$sqlin}) ";
							}
						}
						else
						{
							$djoinwheresql.=" AND  district=".intval($aset['district']);
						}
					}
					if (!empty($aset['sdistrict']))
					{
						if (strpos($aset['sdistrict'],"-"))
						{
							$or=$orsql="";
							$arr=explode("-",$aset['sdistrict']);
							if (count($arr)>10) exit();
							$sqlin=implode(",",$arr);
							if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
							{
								$djoinwheresql.=" AND sdistrict IN  ({$sqlin}) ";
							}
						}
						else
						{
						$djoinwheresql.=" AND sdistrict=".intval($aset['sdistrict']);
						}
					}
					if (!empty($djoinwheresql))
					{
					$djoinwheresql=" WHERE ".ltrim(ltrim($djoinwheresql),'AND');
					}
					
	}
	$joinsql=$joinsql==""?"  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('resume_district')." {$djoinwheresql} )AS d ON  r.id=d.pid ":$joinsql."  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('resume_district')." {$djoinwheresql} )AS d ON  r.id=d.pid ";
}
if (isset($aset['experience']) && !empty($aset['experience']))
{
	$wheresql.=" AND r.experience=".intval($aset['experience'])." ";
}
if (isset($aset['education']) && !empty($aset['education']))
{
	$wheresql.=" AND r.education=".intval($aset['education'])." ";
}
if (isset($aset['talent']) && !empty($aset['talent']))
{
	$wheresql.=" AND r.talent=".intval($aset['talent'])." ";
}
if (isset($aset['sex']) && !empty($aset['sex']))
{
	$wheresql.=" AND r.sex=".intval($aset['sex'])." "; // 添加搜索条件  男 女
}
if (isset($aset['photo']) && !empty($aset['photo']))
{
	$wheresql.=" AND r.photo='".intval($aset['photo'])."' ";
}
if (isset($aset['key']) && !empty($aset['key']))
{
	if ($_CFG['resumesearch_purview']=='2')
	{
		if ($_SESSION['username']=='')
		{
		header("Location: ".url_rewrite('HW_login')."?url=".urlencode($_SERVER["REQUEST_URI"]));
		}
	}
	$key=help::addslashes_deep(trim($aset['key']));
	if ($_CFG['resumesearch_type']=='1')
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
		$wheresql.=" AND  MATCH (r.`key`) AGAINST ('{$key}'{$mode}) ";
	}
	else
	{
		$wheresql.=" AND r.likekey LIKE '%{$key}%' ";
	}
	$resumetable=table('resume_search_key');
}
if (!empty($aset['tag']))
{
	if (strpos($aset['tag'],","))
	{
		$or=$orsql="";
		$arr=explode(",",$aset['tag']);
		if (count($arr)>10) exit();
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
	$joinsql=$joinsql==""?"  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('resume_tag')." {$joinwheresql_tag} )AS g ON  r.id=g.pid ":$joinsql."  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('resume_tag')." {$joinwheresql_tag} )AS g ON  r.id=g.pid ";
}
/* 搜索 时间范围 */
$moth=intval($_CFG['search_time']);
if($moth>0)
{
	$moth_time=$moth*3600*24*30;
	$time=time()-$moth_time;
	$wheresql.=" AND r.refreshtime>$time ";
}
$wheresql.=" AND r.display='1' AND r.audit='1' ";
if (!empty($wheresql))
{
$wheresql=" WHERE ".ltrim(ltrim($wheresql),'AND');
}
		if (isset($aset['paged']))
		{
			require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
			$total_sql="SELECT  COUNT(*) AS num  FROM  {$resumetable} AS r ".$joinsql.$wheresql;
			$total_count=$db->get_total($total_sql);
			if (intval($_CFG['resume_list_max'])>0)
			{
				$total_count>intval($_CFG['resume_list_max']) && $total_count=intval($_CFG['resume_list_max']);
			}
			
			//echo $total_sql;
			//echo "SELECT  COUNT(DISTINCT r.id) AS num  FROM  ".table('resume')." AS r ".$wheresql;
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
	$list = $id = array();
	$idresult = $db->query("SELECT id FROM {$resumetable}  AS r".$joinsql.$wheresql.$orderbysql.$limit);
	//echo "SELECT id FROM {$resumetable}  AS r".$joinsql.$wheresql.$orderbysql.$limit;

	
		while($row = $db->fetch_array($idresult))
		{
			$id[]=$row['id'];
		}
	
	if (!empty($id))
	{
	$wheresql=" WHERE id IN (".implode(',',$id).") ";
	$result = $db->query("SELECT id,uid,display_name,nature_cn,fullname,sex,major_cn,specialty,intention_jobs,trade_cn,photo,photo_img,photo_display,refreshtime,birthdate,tag_cn,talent,education_cn,sex_cn,wage_cn,experience_cn,district_cn,current_cn FROM ".table('resume')."  AS r ".$joinsql.$wheresql.$orderbysql);
		while($row = $db->fetch_array($result))
		{
			if ($row['display_name']=="2")
			{
				$row['fullname']="N".str_pad($row['id'],7,"0",STR_PAD_LEFT);
				$row['fullname_']=$row['fullname'];		
			}
			elseif($row['display_name']=="3")
			{ 
				if($row['sex']==1){
				$row['fullname']=cut_str($row['fullname'],1,0,"男");
				}elseif($row['sex'] == 2){
				$row['fullname']=cut_str($row['fullname'],1,0,"女");
				}else{
				$row['fullname']=cut_str($row['fullname'],1,0,"**");
				}	
			}
			else
			{
				$row['fullname_']=$row['fullname'];
				$row['fullname']=cut_str($row['fullname'],$aset['namelen'],0,$aset['dot']);
			}
			$row['checked'] = false;
			$row['specialty_']=strip_tags($row['specialty']);
			if ($aset['specialtylen']>0)
			{
			$row['specialty']=cut_str(strip_tags($row['specialty']),$aset['specialtylen'],0,$aset['dot']);
			}
			$row['intention_jobs_'] = $row['intention_jobs'];
			if ($aset['jobslen']>0)
			{
			$row['intention_jobs']=cut_str(strip_tags($row['intention_jobs']),$aset['jobslen'],0,$aset['dot']);
			}
			if ($aset['majorlen']>0)
			{
			$row['major_cn']=cut_str(strip_tags($row['major_cn']),$aset['majorlen'],0,$aset['dot']);
			}
			$row['trade_cn_'] = $row['trade_cn'];
			$row['trade_cn'] = cut_str(strip_tags($row['trade_cn']),10,0,"..");
			$row['resume_url']=url_rewrite($aset['showname'],array('id'=>$row['id']));
			$row['refreshtime_cn']=daterange(time(),$row['refreshtime'],'Y-m-d',"#FF3300");
			$row['age']=date("Y")-$row['birthdate'];
			if ($row['tag_cn'])
			{
				$tag_cn=explode(',',$row['tag_cn']);
				$row['tag_cn']=$tag_cn;
			}
			else
			{
			$row['tag_cn']=array();
			}
			// 照片显示方式
			if ($row['photo']=="1")
			{
				if($row['photo_display']=="1")
				{
					$row['photosrc']=$_CFG['resume_photo_dir_thumb'].$row['photo_img'];
				}
				else
				{
					$row['photosrc']=$_CFG['resume_photo_dir_thumb']."no_photo_display.gif";
				}
				
			}
			else
			{
				$row['photosrc']=$_CFG['resume_photo_dir_thumb']."no_photo.gif";
			}
			//判断手机是否验证
			$is_audit_phone = $db->getone("SELECT mobile_audit FROM ".table('members')." WHERE uid={$row['uid']}  LIMIT 1 ");
			$row['is_audit_mobile'] = $is_audit_phone['mobile_audit'];
			//语言能力
			$language = $db->getall("SELECT * FROM ".table('resume_language')." WHERE pid={$row['id']} ");
			$row['language'] = $language;
			$list[] = $row;
		}
	}
	else
	{
	$list=array();
	}
	$smarty->assign($aset['listname'], $list);
}
?>
