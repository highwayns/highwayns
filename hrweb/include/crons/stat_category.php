<?php
 /*
 * 74cms 计划任务 每日数据统计
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
if(!defined('IN_QISHI'))
{
die('Access Denied!');
}
ignore_user_abort(true);
set_time_limit(180);
	global $_CFG;
	//岗位分类
	$result = $db->query("SELECT * FROM ".table('category_jobs')." WHERE parentid=0");	
	while($row = $db->fetch_array($result))
	{
		$in_jobwheresql=" WHERE category='{$row['id']}' ";
		$in_resumewheresql=" WHERE category='{$row['id']}'";
		$jobtotal=$db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs').$in_jobwheresql);		
		$jobtotal=$jobtotal>0?"({$jobtotal})":'';
		$resumetotal=$db->get_total("SELECT COUNT(DISTINCT pid ) AS num FROM ".table('resume_jobs')." {$in_resumewheresql} ");
		$resumetotal=$resumetotal>0?"({$resumetotal})":'';
		$sql = "UPDATE ".table('category_jobs')." SET stat_jobs='{$jobtotal}',stat_resume='{$resumetotal}'  WHERE id='{$row['id']}' LIMIT 1";
		$db->query($sql);			
	}
	//地区分类
	$result = $db->query("SELECT * FROM ".table('category_district')." WHERE parentid=0");	
	while($row = $db->fetch_array($result))
	{
		$in_jobwheresql=" WHERE district='{$row['id']}' ";
		$in_resumewheresql=" WHERE r.district='{$row['id']}'";
		$jobtotal=$db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs').$in_jobwheresql);		
		$resumetotal=$db->get_total("SELECT  COUNT(*) AS num  FROM  ".table('resume_district')." AS r ".$in_resumewheresql);
		$jobtotal=$jobtotal>0?"({$jobtotal})":'';
		$resumetotal=$resumetotal>0?"({$resumetotal})":'';
		$sql = "UPDATE ".table('category_district')." SET stat_jobs='{$jobtotal}',stat_resume='{$resumetotal}'  WHERE id='{$row['id']}' LIMIT 1";
		$db->query($sql);			
	}
	//其他分类
	$result = $db->query("SELECT * FROM ".table('category')." ");	
	while($row = $db->fetch_array($result))
	{
		if ($row['c_alias']=="QS_trade")
		{
			$in_jobwheresql=" WHERE trade='{$row['c_id']}' ";
		}
		elseif ($row['c_alias']=="QS_wage")
		{
			$in_jobwheresql=" WHERE wage='{$row['c_id']}' ";
		}		
		elseif ($row['c_alias']=="QS_jobs_nature")
		{
			$in_jobwheresql=" WHERE nature='{$row['c_id']}' ";
		}
		else
		{
		continue;
		}
		$jobtotal=$db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs').$in_jobwheresql);
		$jobtotal=$jobtotal>0?"({$jobtotal})":'';
		$sql = "UPDATE ".table('category')." SET stat_jobs='{$jobtotal}',stat_resume='{$resumetotal}'  WHERE c_id='{$row['c_id']}' LIMIT 1";
		$db->query($sql);			
	}
	//更新分类缓存
	$cache_file_path =QISHI_ROOT_PATH. "data/cache_category.php";
	$sql = "SELECT * FROM ".table('category')."  ORDER BY c_order DESC,c_id ASC";
	$result = $db->query($sql);
		while($row = $db->fetch_array($result))
		{
			$catarr[$row['c_alias']][$row['c_id']] =array("id"=>$row['c_id'],"parentid"=>$row['c_parentid'],"categoryname"=>$row['c_name'],"stat_jobs"=>$row['stat_jobs'],"stat_resume "=>$row['stat_resume ']);
		}
		stat_write_static_cache($cache_file_path,$catarr);
		function stat_write_static_cache($cache_file_path, $config_arr)
		{
			$content = "<?php\r\n";
			$content .= "\$data = " . var_export($config_arr, true) . ";\r\n";
			$content .= "?>";
			file_put_contents($cache_file_path, $content, LOCK_EX);
		}
	
	
	//更新任务时间表
	if ($crons['weekday']>=0)
	{
	$weekday=array('Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday');
	$nextrun=strtotime("Next ".$weekday[$crons['weekday']]);
	}
	elseif ($crons['day']>0)
	{
	$nextrun=strtotime('+1 months'); 
	$nextrun=mktime(0,0,0,date("m",$nextrun),$crons['day'],date("Y",$nextrun));
	}
	else
	{
	$nextrun=time();
	}
	if ($crons['hour']>=0)
	{
	$nextrun=strtotime('+1 days',$nextrun); 
	$nextrun=mktime($crons['hour'],0,0,date("m",$nextrun),date("d",$nextrun),date("Y",$nextrun));
	}
	if (intval($crons['minute'])>0)
	{
	$nextrun=strtotime('+1 hours',$nextrun); 
	$nextrun=mktime(date("H",$nextrun),$crons['minute'],0,date("m",$nextrun),date("d",$nextrun),date("Y",$nextrun));
	}
	$setsqlarr['nextrun']=$nextrun;
	$setsqlarr['lastrun']=time();
	$db->updatetable(table('crons'), $setsqlarr," cronid ='".intval($crons['cronid'])."'");
?>