<?php
 if(!defined('IN_HIGHWAY'))
 {
die('Access Denied!');
 }
/*获取所有的套餐*/
function get_setmeal($apply=true)
{
	global $db;
	if ($apply==true)
	{
	$where="";
	}
	else
	{
	$where=" WHERE display=1 ";
	} 
	$sql = "select * from ".table('setmeal').$where."  order BY display desc,show_order desc,id asc";
	return $db->getall($sql);
}
 //获取短信队列
function get_smsqueue($offset, $perpage, $wheresql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT * FROM ".table('smsqueue').$wheresql.$limit);
	while($row = $db->fetch_array($result))
	{
	$row['s_body']=$row['s_body'];
	$row['s_body_']=cut_str(strip_tags($row['s_body']),18,0,"...");
	$row_arr[] = $row;
	}
	return $row_arr;
}
//获取单个短信队列
function get_smsqueue_one($id)
{
	global $db;
	$sql = "select * from ".table('smsqueue')." where s_id=".intval($id)." LIMIT 1";
	return $db->getone($sql);
}
?>
