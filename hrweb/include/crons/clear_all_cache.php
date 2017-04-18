<?php
if(!defined('IN_HIGHWAY'))
{
die('Access Denied!');
}
	global $_CFG;
	include_once(HIGHWAY_ROOT_PATH.'include/template_lite/class.template.php');
	$cronstpl = new Template_Lite; 
	$cronstpl -> cache_dir = HIGHWAY_ROOT_PATH.'temp/caches/'.$_CFG['template_dir'];
	$cronstpl -> compile_dir =  HIGHWAY_ROOT_PATH.'temp/templates_c/'.$_CFG['template_dir'];
	$cronstpl -> template_dir = HIGHWAY_ROOT_PATH.'templates/'.$_CFG['template_dir'];
	$cronstpl->cache = true;
	$cronstpl -> clear_all_cache();

	//删除微信扫描缓存文件
	deldir(HIGHWAY_ROOT_PATH."data/weixin/");
	function deldir($dir) {
	  //删除目录下的文件：
	  $dh=opendir($dir);
	  while ($file=readdir($dh)) {
	    if($file!="." && $file!="..") {
	      $fullpath=$dir."/".$file;
	      if(!is_dir($fullpath)) {
	          unlink($fullpath);
	      } else {
	          deldir($fullpath);
	      }
	    }
	  }
	  closedir($dh);
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
