<?php
define('IN_HIGHWAY', true);
require_once(dirname(dirname(__FILE__)).'/include/plus.common.inc.php');
$act = !empty($_POST['act']) ? trim($_POST['act']) :trim($_GET['act']);
if ($act=='get_companyjobslist')
{
	$comjobshtml="";
	$rows=2;
	$companyid=intval($_GET['company_id']); 
	$comjobsarray=$db->getall("select * from ".table('jobs')." where company_id = '{$companyid}' ORDER BY stick DESC , refreshtime DESC LIMIT {$rows}");
	if (!empty($comjobsarray))
	{
		foreach($comjobsarray as $li)
		{
			$jobs_url=url_rewrite("HW_jobsshow",array('id'=>$li['id']));
			$jobs_name=cut_str($li['jobs_name'],"10",0,"..");
			$comjobshtml.="<li><div class=\"j_name\"><a href=\"{$jobs_url}\" target=\"_blank\">{$jobs_name}<span class=\"ji\"></span></a><span class=\"j_time\">".date('Y-m-d',$li['addtime'])." 更新</span></div><p><span>学歴要求：{$li['education_cn']}</span>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;<span>仕事経験：{$li['experience_cn']}</span>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;<span>仕事アドレス：{$li['district_cn']}</span>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;<span>給料：<em>{$li['wage_cn']}</em></span></p></li>";
		}
		$jobslist_url = url_rewrite("HW_companyjobs",array("id"=>$companyid));
		$comjobshtml.="<p class=\"more\"><a target=\"_blank\" href=\"".$jobslist_url."\">閲覧More>></a></p>";
		exit($comjobshtml);
	}
	else
	{
		exit('empty!');
	}
}
?>
