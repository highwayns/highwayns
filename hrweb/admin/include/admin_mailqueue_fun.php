<?php
 if(!defined('IN_HIGHWAY'))
 {
 	die('Access Denied!');
 }
function get_mailqueue($offset, $perpage, $wheresql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT * FROM ".table('mailqueue').$wheresql.$limit);
	while($row = $db->fetch_array($result))
	{
	$row['m_subject']=$row['m_subject'];
	$row['m_subject_']=cut_str(strip_tags($row['m_subject']),18,0,"...");
	$row['m_body']=strip_tags($row['m_body']);
	$row['m_body_']=cut_str(strip_tags($row['m_body']),18,0,"...");
	$row_arr[] = $row;
	}
	return $row_arr;
}
function get_mailqueue_one($id)
{
	global $db;
	$sql = "select * from ".table('mailqueue')." where m_id=".intval($id)." LIMIT 1";
	return $db->getone($sql);
}
function replace_label($str,$user='')
{
	global $_CFG;
	$str=str_replace('{sitename}',$_CFG['site_name'],$str);
	$str=str_replace('{sitedomain}',$_CFG['site_domain'].$_CFG['site_dir'],$str);
	$str=str_replace('{sitelogo}',"<a href=\"{$_CFG['upfiles_dir']}\" target=\"_blank\"><img src=\"{$_CFG['site_domain']}{$_CFG['upfiles_dir']}{$_CFG['web_logo']}\"  border=\"0\"/></a>",$str);
	$str=str_replace('{address}',$_CFG['address'],$str);
	$str=str_replace('{tel}',$_CFG['top_tel'],$str);
	$str=str_replace('{username}',$user['username'],$str);
	if ($user['last_login_time'])
	{
	$str=str_replace('{lastlogintime}',date("Y-m-d",$user['last_login_time']),$str);
	}	
	return $str;
}
?>
