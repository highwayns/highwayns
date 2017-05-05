<?php
 if(!defined('IN_HIGHWAY'))
 {
 	die('Access Denied!');
 }
function get_news($offset, $perpage, $sql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT a.*,c.id as cid,c.categoryname as c_categoryname,p.id as pid,p.categoryname as p_categoryname FROM ".table('article')." AS a ".$sql."  ".$limit);
	while($row = $db->fetch_array($result)){
	$tit_color = $row['tit_color'] ? "color:".$row['tit_color'].";" : '';
	$tit_b = $row['tit_b']>0 ? "font-weight:bold;" : '';
	$tit_style= $tit_color || $tit_b ? "style=\"".$tit_color.$tit_b."\""  : '';
	$Small_img = $row['Small_img'] ? "<span style=\"color:#009900\">(図)</span>" : '';
	if (!empty($row['is_url']) && $row['is_url']!='http://')
	{
	$row['url'] = $row['is_url'];
	}
	else
	{
	$row['url'] = url_rewrite('HW_newsshow',array('id'=>$row['id']));
	}
	$row['url_title'] ="<a href=\"".$row['url']."\" target=\"_blank\" ".$tit_style.">".$row['title']."</a> ".$Small_img."";
	$row_arr[] = $row;
	}
	return $row_arr;
}
function del_news($id)
{
	global $db,$thumb_dir,$upfiles_dir;
	if (!is_array($id)) $id=array($id);
	foreach($id as $val)
	{
		$sql_img="select Small_img from ".table('article')." where id=".intval($val)." LIMIT 1";
		$y_img=$db->getone($sql_img);
		@unlink($upfiles_dir."/".$y_img['Small_img']);
		@unlink($thumb_dir.$y_img['Small_img']);
		$db->query("Delete from  ".table('article')." where id=".intval($val)." LIMIT 1");
		write_log("削除された職位idは".intval($val)."文書", $_SESSION['admin_name'],3);
	}
	return true;
}
function get_article_category()
{
	global $db;
	$sql = "select * from ".table('article_category')." where parentid=0  ORDER BY category_order desc";
	$category_list=$db->getall($sql);
	if (is_array($category_list))
	{
	foreach($category_list as $v)
	{
	$list[]=array("id"=>$v['id'],"categoryname"=>$v['categoryname'],"parentid"=>$v['parentid']);
		$sqlf = "select * from ".table('article_category')." where parentid=".$v['id']."  ORDER BY category_order desc";
		$category_f=$db->getall($sqlf);
		if (is_array($category_f))
		{
			foreach($category_f as $vs)
			{
			$list[]=array("id"=>$vs['id'],"categoryname"=>"|-".$vs['categoryname'],"parentid"=>$vs['parentid']);
			}
		}
	}
	}
	return $list;
}
function get_article_category_one($id)
{
	global $db;
	$sql = "select * from ".table('article_category')." where id=".intval($id)." LIMIT 1";
	$var=$db->getone($sql);
	return $var;
}
function get_article_parentid($val)
{
	global $db;
	$sql = "select * from ".table('article_category')." where id=".intval($val)."  LIMIT 1";
	$category=$db->getone($sql);
	return $category['parentid'];
}
function del_category($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	foreach($id as $sid)
	{
		$sql = "select * from ".table('article_category')." where id=".intval($sid)."  LIMIT 1";
		$category=$db->getone($sql);
		if ($category['parentid']=="0")
		{
			if (!$db->query("Delete from ".table('article_category')." WHERE (parentid =".intval($sid)." OR id =".intval($sid).")  AND admin_set<>1")) return false;
			$return=$return+$db->affected_rows();
		}
		else
		{
			if (!$db->query("Delete from ".table('article_category')." WHERE id =".intval($sid)." AND admin_set<>1")) return false;
			$return=$return+$db->affected_rows();
		}
	}
	return $return;
}
function del_property($id)
{
	global $db;
	if(!is_array($id)) $id=array($id);
	$return=0;
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("Delete from ".table('article_property')." WHERE id IN (".$sqlin.")  AND admin_set<>1")) return false;
		$return=$return+$db->affected_rows();
		write_log("削除idは".$sqlin."のニュース属性,削除件数".$return."行", $_SESSION['admin_name'],3);
	}
	return $return;
}
function get_article_property_one($id)
{
	global $db;
	$sql = "select * from ".table('article_property')." where id=".intval($id)." LIMIT 1";
	$var=$db->getone($sql);
	return $var;
}
?>
