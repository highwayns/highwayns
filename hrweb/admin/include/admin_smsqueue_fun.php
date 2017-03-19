<?php
 /*
 * 74cms 管理中心 营销中心相关函数
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