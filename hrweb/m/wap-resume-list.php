<?php
 /*
 * 74cms WAP
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/


define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/fun_wap.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
$smarty->cache = false;
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$page = empty($_GET['page'])?1:intval($_GET['page']);
$district = intval($_GET['district'])==0?"":intval($_GET['district']);
$sdistrict = intval($_GET['sdistrict'])==0?"":intval($_GET['sdistrict']);
$experience = intval($_GET['experience'])==0?"":intval($_GET['experience']);
$education = intval($_GET['education'])==0?"":intval($_GET['education']);
$topclass = intval($_GET['topclass'])==0?"":intval($_GET['topclass']);
$category = intval($_GET['category'])==0?"":intval($_GET['category']);
$subclass = intval($_GET['subclass'])==0?"":intval($_GET['subclass']);

$talent = intval($_GET['talent'])==0?"":intval($_GET['talent']);

$key = empty($_GET['key'])?"":$_GET['key'];
$jobstable=table('resume_search_rtime');

if($talent<>'')
{
	$wheresql.=" AND `talent`=".$talent." ";
}

if ($district<>'' || $sdistrict<>'')
{
	if($district<>'')
	{
		$d_joinwheresql.=" AND  district=".$district;
	}
	if ($sdistrict<>'')
	{
		$d_joinwheresql.=" AND `sdistrict` = ".$sdistrict;
	}
	if (!empty($d_joinwheresql))
	{
	$d_joinwheresql=" WHERE ".ltrim(ltrim($d_joinwheresql),'AND');
	}
	$joinsql="  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('resume_district')." {$d_joinwheresql} )AS d ON  r.id=d.pid ";
}
if ($experience<>'')
{
	$wheresql.=" AND `experience`=".$experience." ";
}
if ($education<>'')
{
	$wheresql.=" AND `education`=".$education." ";
}
if ($topclass<>'' || $category<>'' || $subclass<>'')
{
	if ($topclass<>'')
	{
		$joinwheresql.=" AND  topclass=".$topclass;
	}
	if ($category<>'')
	{
		$joinwheresql.=" AND  category=".$category;
	}
	if ($subclass<>'')
	{
		$joinwheresql.=" AND  subclass=".$subclass;
	}
	if (!empty($joinwheresql))
	{
	$joinwheresql=" WHERE ".ltrim(ltrim($joinwheresql),'AND');
	}
	$joinsql=$joinsql==""?"  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('resume_jobs')." {$joinwheresql} )AS j ON  r.id=j.pid ":$joinsql."  INNER  JOIN  ( SELECT DISTINCT pid FROM ".table('resume_jobs')." {$joinwheresql} )AS j ON  r.id=j.pid ";
}

if (!empty($key))
{
	$key=trim($key);
	$akey=explode(' ',$key);
	if (count($akey)>1)
	{
	$akey=array_filter($akey);
	$akey=array_slice($akey,0,2);
	$akey=array_map("fulltextpad",$akey);
	$ykey='+'.implode(' +',$akey);
	$mode=' IN BOOLEAN MODE';
	}
	else
	{
	$ykey=fulltextpad($key);
	$mode=' ';
	}
	$wheresql.=" AND  MATCH (`key`) AGAINST ('{$ykey}'{$mode}) ";
	$jobstable=table('resume_search_key');
}
$orderbysql=" ORDER BY `refreshtime` desc,`id` desc ";
if (!empty($wheresql))
{
$wheresql=" WHERE ".ltrim(ltrim($wheresql),'AND');
}

	$perpage = 5;
	$count  = 0;
	$page = empty($_GET['page'])?1:intval($_GET['page']);
	if($page<1) $page = 1;
	
	$theurl = "wap-resume-list.php?sdistrict=".$sdistrict."&amp;subclass=".$subclass."&amp;key=".$key;
	$start = ($page-1)*$perpage;
	$total_sql="SELECT COUNT(*) AS num FROM {$jobstable} as r {$joinsql} {$wheresql}";
	$count=$db->get_total($total_sql);
	$limit=" LIMIT {$start},{$perpage}";
	$idresult = $db->query("SELECT id FROM {$jobstable} as r ".$joinsql.$wheresql.$orderbysql.$limit);
	while($row = $db->fetch_array($idresult))
	{
	$id[]=$row['id'];
	}
	if (!empty($id))
	{
		$wheresql=" WHERE id IN (".implode(',',$id).") AND display=1 AND audit=1 ";
		$resume = $db->getall("SELECT * FROM ".table('resume').$wheresql.$orderbysql);	
		foreach ($resume as $key => $value) {
			if ($value['display_name']=="2")
			{
				$value['fullname']="N".str_pad($value['id'],7,"0",STR_PAD_LEFT);
				$value['fullname_']=$value['fullname'];		
			}
			elseif($value['display_name']=="3")
			{
				if($value['sex']==1)
				{
					$value['fullname']=cut_str($value['fullname'],1,0,"先生");
				}
				elseif($value['sex']==2)
				{
					$value['fullname']=cut_str($value['fullname'],1,0,"女士");
				}
				$value['fullname_']=$value['fullname'];	
			}
			else
			{
				$value['fullname_']=$value['fullname'];
				$value['fullname']=$value['fullname'];
			}
			$resume[$key]['url'] = wap_url_rewrite("wap-resume-show",array("id"=>$value["id"]));
			$resume[$key]['fullname_']=$value['fullname_'];
			$resume[$key]['fullname']=$value['fullname'];
		}
		
	}
	else
	{
		$resume=array();
	}
	$smarty->assign('resume',$resume);
	$smarty->assign('pagehtml',wapmulti($count, $perpage, $page, $theurl));
	$smarty->display("wap/wap-resume-list.html");
?>