<?php
 /*
 * 74cms 计划任务 清除缓存
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
	global $_CFG,$db,$baiduxml;
	$xmlset=get_cache('baiduxml');
	$xmlorder=$xmlset['order'];
	require_once(QISHI_ROOT_PATH.'include/baiduxml.class.php');
	$baiduxml = new BaiduXML();
	makebaiduxml($xmlorder,0,$xmlset['xmlpagesize'],1,0,$xmlset);
	function makebaiduxml($xmlorder,$start,$size,$li=1,$t=0,$xmlset)
	{
		global $db,$baiduxml,$_CFG;
		$xmldir = QISHI_ROOT_PATH.$xmlset['xmldir'];
	 	if ($xmlorder=='1')
		{
		$sqlorder=" ORDER BY `addtime` DESC ";
		}
		else
		{
		$sqlorder=" ORDER BY `refreshtime` DESC ";
		}
		$sqllimit=" LIMIT {$start},{$size}";
    	$result = $db->query("select * from ".table('jobs').$sqlorder.$sqllimit);
		while($row = $db->fetch_array($result))
		{
			$t++;
			$contact=$db->getone("SELECT * from ".table('jobs_contact')." where pid = '{$row['id']}' LIMIT 1");
			$com=$db->getone("SELECT * from ".table('company_profile')." where id = '{$row['company_id']}' LIMIT 1");
			$category=$db->getone("SELECT * FROM ".table('category_jobs')." where id=".$row['category']." LIMIT 1");
			$subclass=$db->getone("SELECT * FROM ".table('category_jobs')." where id=".$row['subclass']." LIMIT 1");
			$row['jobs_url']=url_rewrite('QS_jobsshow',array('id'=>$row['id']));
			$x=array($row['jobs_url'],date("Y-m-d",$row['refreshtime']),$row['jobs_name'],date("Y-m-d",$row['deadline']),$row['contents'],$row['nature_cn'],   str_replace('/','',$row['district_cn']),$row['companyname'],$contact['email'],$category['categoryname'],$subclass['categoryname'],$row['education_cn'],$row['experience_cn'],date("Y-m-d",$row['addtime']),date("Y-m-d",$row['deadline']),str_replace('~','-',$row['wage_cn']),$row['trade_cn'],$com['nature_cn'],$_CFG['site_name'],$_CFG['site_domain'].$_CFG['site_dir']);
			foreach ($x as $key => $value) {
				$x[$key] = strip_tags(str_replace("&","&amp;",$value));
			}
			if (in_array('',$x))
			{
			continue;
			}
			else
			{
			$baiduxml->XML_url($x);
			$rowid=$row['id'];
			if ($xmlset['xmlmax']>0 && $t>=$xmlset['xmlmax'])
			{
				for($b=1;$b<$li;$b++)
				{
					$xml_dir = '../../'.$xmlset['xmldir'];
					$xmlfile=$xml_dir.$xmlset['xmlpre'].$b.'.xml';
					$xmlfile=ltrim($xmlfile,'../');
					$xmlfile=ltrim($xmlfile,'..\\');
					$atime=filemtime($xmldir.$xmlset['xmlpre'].$b.'.xml');
					$atime=date("Y-m-d",$atime);
					$index[]=array($_CFG['site_domain'].$_CFG['site_dir'].$xmlfile,$atime);
				}
			$baiduxml->XML_index_put($xmldir.$xmlset['indexname'],$index);
			return true;
			}
			}
		}
		if (empty($rowid))
		{
			for($b=1;$b<$li;$b++)
			{
				$xml_dir = '../../'.$xmlset['xmldir'];
				$xmlfile=$xml_dir.$xmlset['xmlpre'].$b.'.xml';
				$xmlfile=ltrim($xmlfile,'../');
				$xmlfile=ltrim($xmlfile,'..\\');
				$atime=filemtime($xmldir.$xmlset['xmlpre'].$b.'.xml');
				$atime=date("Y-m-d",$atime);
				$index[]=array($_CFG['site_domain'].$_CFG['site_dir'].$xmlfile,$atime);
			}
			$baiduxml->XML_index_put($xmldir.$xmlset['indexname'],$index);
			return true;
		}
		else
		{
			$xmlname=$xmldir.$xmlset['xmlpre'].$li.'.xml';
			if ($baiduxml->XML_put($xmlname))
			{
			$li++;
			return makebaiduxml($xmlorder,$t,$xmlset['xmlpagesize'],$li,$t,$xmlset);
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