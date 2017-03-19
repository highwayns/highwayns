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
$trade = intval($_GET['trade'])==0?"":intval($_GET['trade']);
$topclass = intval($_GET['topclass'])==0?"":intval($_GET['topclass']);
$category = intval($_GET['category'])==0?"":intval($_GET['category']);
$subclass = intval($_GET['subclass'])==0?"":intval($_GET['subclass']);
$recommend = intval($_GET['recommend'])==0?"":intval($_GET['recommend']);
$emergency = intval($_GET['emergency'])==0?"":intval($_GET['emergency']);
$wage = intval($_GET['wage'])==0?"":intval($_GET['wage']);
$key = empty($_GET['key'])?"":$_GET['key'];
$jobstable=table('jobs_search_stickrtime');
if ($district<>'')
{
	$wheresql.=" AND `district` = ".$district;
	if ($sdistrict<>'')
	{
		$wheresql.=" AND `sdistrict` = ".$sdistrict;
	}
}
if ($trade<>'')
{
	$wheresql.=" AND `trade` = ".$trade;
}
if ($topclass<>'')
{
	$wheresql.=" AND `topclass` = ".$topclass;
	if ($category<>'')
	{
		$wheresql.=" AND `category` = ".$category;
		if ($subclass<>'')
		{
			$wheresql.=" AND `subclass` = ".$subclass;
		}
	}
}
if ($wage<>'')
{
	$wheresql.=" AND `wage` = ".$wage;
}
if ($recommend<>'')
{
	$wheresql.=" AND `recommend` = ".$recommend;
}
if ($emergency<>'')
{
	$wheresql.=" AND `emergency` = ".$emergency;
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
	$jobstable=table('jobs_search_key');
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
	$theurl = "wap-jobs-list.php?district=".$district."&amp;sdistrict=".$sdistrict."&amp;trade=".$trade."&amp;topclass=".$topclass."&amp;category=".$category."&amp;subclass=".$subclass."&amp;wage=".$wage."&amp;key=".$key;
	$start = ($page-1)*$perpage;
	$total_sql="SELECT COUNT(*) AS num FROM {$jobstable} {$wheresql}";
	$count=$db->get_total($total_sql);
	$limit=" LIMIT {$start},{$perpage}";
	$idresult = $db->query("SELECT id FROM {$jobstable} ".$wheresql.$orderbysql.$limit);
	while($row = $db->fetch_array($idresult))
	{
	$id[]=$row['id'];
	}
	if (!empty($id))
	{
		$wheresql=" WHERE id IN (".implode(',',$id).") ";
		$jobs = $db->getall("SELECT * FROM ".table('jobs').$wheresql.$orderbysql);
		foreach ($jobs as $key => $value) {
			$jobs[$key]['url'] = wap_url_rewrite("wap-jobs-show",array("id"=>$value['id']));
			if (!empty($value['highlight']))
			{
			$jobs[$key]['jobs_name']="<span style=\"color:{$value['highlight']}\">{$value['jobs_name']}</span>";
			}
		}	
	}
	else
	{
		$jobs=array();
	}
	$smarty->assign('jobs',$jobs);
	$smarty->assign('pagehtml',wapmulti($count, $perpage, $page, $theurl));
	$smarty->display("wap/wap-jobs-list.html");
?>