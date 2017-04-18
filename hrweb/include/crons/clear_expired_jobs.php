<?php
if(!defined('IN_HIGHWAY'))
{
die('Access Denied!');
}
	global $_CFG;
	$time=time();
	if ($_CFG['outdated_jobs']=="1")
	{
		$result = $db->query("SELECT * FROM ".table('jobs')." WHERE  deadline<{$time} OR (setmeal_deadline<{$time} and setmeal_deadline>0)");
		while($row = $db->fetch_array($result))
		{	
			$row=array_map('addslashes',$row);
		
			$db->query("Delete from ".table('jobs_tmp')." WHERE id='{$row['id']}' LIMIT 1");
			$row['display']=2;
			$db->inserttable(table('jobs_tmp'),$row);
			$did[]=$row['id'];
			if ((time()-$time)>3)
			{
				if (!empty($did) && is_array($did) )
				{
					foreach($did as $id)
					{
						$db->query("Delete from ".table('jobs')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_hot')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_key')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_rtime')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_scale')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_stickrtime')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_wage')." WHERE id='{$id}' LIMIT 1");
					}
				}
				break;
			}
		}
			if (!empty($did) && is_array($did) )
				{
					foreach($did as $id)
					{
						$db->query("Delete from ".table('jobs')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_hot')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_key')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_rtime')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_scale')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_stickrtime')." WHERE id='{$id}' LIMIT 1");
						$db->query("Delete from ".table('jobs_search_wage')." WHERE id='{$id}' LIMIT 1");
					}
				}	
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
