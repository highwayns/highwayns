<?php
if(!defined('IN_HIGHWAY'))
{
die('Access Denied!');
}
	global $_CFG;
	$result = $db->query("SELECT p.*,m.username FROM ".table('promotion')." AS p JOIN  ".table('members')." AS m ON p.cp_uid=m.uid  WHERE  p.cp_endtime<".time()." AND p.cp_available=1");
	while($row = $db->fetch_array($result))
	{
		if ($row['cp_promotionid']=="1")
		{
			$db->query("UPDATE ".table('jobs')." SET recommend='0' WHERE id='{$row['cp_jobid']}' LIMIT 1 ");	
			$db->query("UPDATE ".table('jobs_tmp')." SET recommend='0' WHERE id='{$row['cp_jobid']}' LIMIT 1 ");	
			$db->query("UPDATE ".table('jobs_search_hot')." SET recommend='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_key')." SET recommend='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_rtime')." SET recommend='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_scale')." SET recommend='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_stickrtime')." SET recommend='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_wage')." SET recommend='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
		}
		elseif ($row['cp_promotionid']=="2")
		{
			$db->query("UPDATE ".table('jobs')." SET emergency='0' WHERE id='{$row['cp_jobid']}' LIMIT 1 ");	
			$db->query("UPDATE ".table('jobs_tmp')." SET emergency='0' WHERE id='{$row['cp_jobid']}' LIMIT 1 ");
			$db->query("UPDATE ".table('jobs_search_hot')." SET emergency='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_key')." SET emergency='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_rtime')." SET emergency='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_scale')." SET emergency='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_stickrtime')." SET emergency='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_wage')." SET emergency='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
		}
		elseif ($row['cp_promotionid']=="3")
		{
			$db->query("UPDATE ".table('jobs')." SET stick=0 WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_tmp')." SET stick=0 WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_search_stickrtime')." SET stick='0' WHERE id='{$row['cp_jobid']}' LIMIT 1");
		}
		elseif ($row['cp_promotionid']=="4")
		{
			$db->query("UPDATE ".table('jobs')." SET highlight='' WHERE id='{$row['cp_jobid']}' LIMIT 1");
			$db->query("UPDATE ".table('jobs_tmp')." SET highlight='' WHERE id='{$row['cp_jobid']}' LIMIT 1");
		}
		write_memberslog($row['cp_uid'],1,3006,$row['username'],"広告期限切れた，自動削除，職位ID:{$row['cp_jobid']}，ケースID：{$row['cp_id']}");
		$proid[] = $row['cp_id'];		
	}
	if (is_array($proid) && !empty($proid))
	{
		$sqlin=implode(",",$proid);
		$db->query("Delete from ".table('promotion')." WHERE cp_id IN ({$sqlin})");
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
