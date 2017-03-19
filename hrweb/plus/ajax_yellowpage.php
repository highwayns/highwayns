<?php
 /*
 * 74cms ajax 加载公司职位更多
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
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
			$jobs_url=url_rewrite("QS_jobsshow",array('id'=>$li['id']));
			$jobs_name=cut_str($li['jobs_name'],"10",0,"..");
			$comjobshtml.="<li><div class=\"j_name\"><a href=\"{$jobs_url}\" target=\"_blank\">{$jobs_name}<span class=\"ji\"></span></a><span class=\"j_time\">".date('Y-m-d',$li['addtime'])." 更新</span></div><p><span>学历要求：{$li['education_cn']}</span>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;<span>工作经验：{$li['experience_cn']}</span>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;<span>工作地点：{$li['district_cn']}</span>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;<span>薪资待遇：<em>{$li['wage_cn']}</em></span></p></li>";
		}
		$jobslist_url = url_rewrite("QS_companyjobs",array("id"=>$companyid));
		$comjobshtml.="<p class=\"more\"><a target=\"_blank\" href=\"".$jobslist_url."\">查看更多>></a></p>";
		exit($comjobshtml);
	}
	else
	{
		exit('empty!');
	}
}
?>