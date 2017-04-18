<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
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
