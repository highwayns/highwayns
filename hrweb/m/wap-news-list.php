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
$jobstable=table('article');
$orderbysql=" ORDER BY `id` desc";


	$perpage = 5;
	$count  = 0;
	$page = empty($_GET['page'])?1:intval($_GET['page']);
	if($page<1) $page = 1;
	$theurl = "wap-news-list.php";
	$start = ($page-1)*$perpage;
	$total_sql="SELECT COUNT(*) AS num FROM {$jobstable} ";
	$count=$db->get_total($total_sql);
	$limit=" LIMIT {$start},{$perpage}";
	$idresult = $db->query("SELECT id FROM {$jobstable} ".$orderbysql.$limit);
	while($row = $db->fetch_array($idresult))
	{
	$id[]=$row['id'];
	}
	if (!empty($id))
	{
		$wheresql=" WHERE id IN (".implode(',',$id).") ";
		$article = $db->getall("SELECT * FROM ".table('article').$wheresql.$orderbysql);
		foreach ($article as $key => $value) {
			$article[$key]['url'] = wap_url_rewrite("wap-news-show",array("id"=>$value['id']));
		}	
	}
	else
	{
		$article=array();
	}
	$smarty->assign('article',$article);
	$smarty->display("wap/wap-news-list.html");
?>