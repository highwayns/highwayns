<?php
 /*
 * 74cms AJAX BAIDUMAP
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
header("Content-Type:text/html;charset=utf-8");
$id =intval($_GET['id']);
$jobshow =trim($_GET['jobshow']);
$perpage=10;
function iconv_js($string)
{
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
	return gbk_to_utf8($string);
	}
}
if (!empty($jobshow))
{
	$page =intval($_GET['page']);
	$jobstable=table('jobs_search_rtime');
	$orderbysql='';
	$limit=" LIMIT {$page},{$perpage}";
	$sqltype=explode(":::",$jobshow);
	if ($sqltype[0]=="point")
	{
	$p=explode(',',$sqltype[1]);
	$p = array_map("floatval", $p);
	$wheresql=" WHERE map_x>{$p[0]} AND map_x<{$p[1]} AND map_y>{$p[2]} AND map_y<{$p[3]}";
	}
	elseif($sqltype[0]=="jobcategory")
	{
		$dsql=$xsql="";
		$arr=explode("_",$sqltype[1]);
		$arr=array_unique($arr);
		if (count($arr)>10) exit();
		foreach($arr as $sid)
		{
			$cat=explode(".",$sid);
			if (intval($cat[1])===0)
			{
			$dsql.= " OR topclass =".intval($cat[0]);
			}
			else
			{
			$xsql.= " OR category =".intval($cat[1]);
			}
		}
		$wheresql=" WHERE (".ltrim(ltrim($dsql.$xsql),'OR').") AND map_x>0 ";
	}
	elseif($sqltype[0]=="citycategory")
	{
		$dsql=$xsql="";
		$arr=explode("_",$sqltype[1]);
		$arr=array_unique($arr);
		if (count($arr)>10) exit();
		foreach($arr as $sid)
		{
			$cat=explode(".",$sid);
			if (intval($cat[1])===0)
			{
				$dsql.= " OR district =".intval($cat[0]);
			}
			else
			{
				$xsql.= " OR sdistrict =".intval($cat[1]);
			}
		}
		$wheresql=" WHERE (".ltrim(ltrim($dsql.$xsql),'OR').") AND map_x>0";
	}
	elseif($sqltype[0]=="trade")
	{
		$arr=explode("_",$sqltype[1]);
		$arr=array_unique($arr);
		if (count($arr)>20) exit();
		$sqlin=implode(",",$arr);
		if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
		{
		$wheresql=" WHERE trade IN  ({$sqlin}) AND map_x>0";
		}
	}
	elseif($sqltype[0]=="key")
	{
		$wheresql=" WHERE  MATCH (`key`) AGAINST ('".fulltextpad($sqltype[1])."') AND map_x>0 ";
		$jobstable=table('jobs_search_key');
	}
	elseif($sqltype[0]=="new")
	{
		$wheresql=" WHERE map_x>0 ";
		$orderbysql=' ORDER BY refreshtime DESC ';
	}
	else
	{
	exit("alert('error:parameter');");
	}
	$total_sql="SELECT COUNT(*) AS num FROM {$jobstable} {$wheresql}";
	$jobscount=$db->get_total($total_sql);
	$comcount=$db->get_total("SELECT COUNT(distinct uid) AS num FROM {$jobstable} {$wheresql}");
	$uidresult = $db->query("SELECT uid FROM {$jobstable} {$wheresql} GROUP BY uid ".$orderbysql.$limit);
	while($uidrow = $db->fetch_array($uidresult))
	{
	$mapuid[]=$uidrow['uid'];
	}
	$li='';
	$js.="$(\".maploading\").hide();";
	$js.="map.clearOverlays();";
	$js.="function Gethtml(id,txt)";
	$js.="{";
	$js.="return '<div class=\"maplabel\"><a href=\"#\">'+txt+'<u></u></a></div>';";
	$js.="};";
	if (!empty($mapuid))
	{
		$wheresql=" where uid IN (".implode(',',$mapuid).") ";
		$result = $db->query("SELECT uid,companyname,company_id,company_addtime,id,refreshtime,map_x,map_y  FROM ".table('jobs').$wheresql." GROUP BY uid ");
		while($row = $db->fetch_array($result))
		{
		$row['companyname_']=$row['companyname'];
		$row['companyname']=cut_str($row['companyname'],13,0,"...");
		$row['company_url']=url_rewrite('QS_companyshow',array('id'=>$row['company_id']));
		$li.="<li><a href=\\\"{$row['company_url']}\\\" target=\\\"_blank\\\" id=\\\"o{$row['id']}\\\">{$row['companyname']}</a></li>";
		//
		$position[]="new BMap.Point({$row['map_x']},{$row['map_y']})";
		$js.="var myLabel{$row['id']} = new BMap.Label(Gethtml({$row['id']},'{$row['companyname_']}'),";
		$js.="{offset:new BMap.Size(-12,-33),";
		$js.="position:new BMap.Point({$row['map_x']},{$row['map_y']})});";
		$js.="myLabel{$row['id']}.setStyle({ border:0,background:''});";
		$js.="map.addOverlay(myLabel{$row['id']});";
		$js.="var infoWindow{$row['id']} = new BMap.InfoWindow(\"载入中...\",{width:300});";
		$js.="myLabel{$row['id']}.addEventListener(\"click\", function(){ map.openInfoWindow(infoWindow{$row['id']},new BMap.Point({$row['map_x']},{$row['map_y']}));}); "; 
		$js.="infoWindow{$row['id']}.addEventListener(\"open\", function(){";
		$js.="if (infoWindow{$row['id']}.getContent()=='载入中...')";
		$js.="{";
		$js.="var htm='<div class=\"mapinfowindow link_lan\"><div class=\"tit link_bk\"><a href=\"{$row['company_url']}\" target=\"_blank\">{$row['companyname_']}</a></div><ul>';";
		$js.="var htmend='</ul></div>';";
		$js.="$.get('{$_CFG['site_dir']}plus/ajax_common.php?uid={$row['uid']}', {'act':'joblisttip'},function (data,textStatus){infoWindow{$row['id']}.setContent(htm+data+htmend)});";
		$js.="}";	
		$js.="});";
		$js_right.="$('#infolist li a[id=\"o{$row['id']}\"]').unbind().mouseover(function()";
		$js_right.="{";
		$js_right.="map.openInfoWindow(infoWindow{$row['id']},new BMap.Point({$row['map_x']},{$row['map_y']}));";
		$js_right.="});";
		}
	}
	if (empty($li))
	{
	$li="<li class=\\\"noinfo\\\">没有找到您要的信息...</li>";
	$js.="$(\"#infolist\").html(\"{$li}\");";
	$js.="$(\"#infotiploading\").hide();";
	$js.="$(\"#infotipshow\").show().html(\"没有找到您要的信息...\");";
	$js.="$('.listloading').hide();";
	}
	else
	{
		if ($sqltype[0]!="point")
		{
		$js.="var points = [".implode(',',$position)."];";
		$js.="map.setViewport(points);";
		}
	$js.="$(\"#infolist\").html(\"{$li}\");";
	$js.="$(\"#infotiploading\").hide();";
	$js.="$(\"#infotipshow\").show().html(\"共找到{$comcount}个公司，{$jobscount}个职位\");";
	$js.="$('.listloading').hide();";
	$js.=$js_right;
	}	
	if ($_GET['pagenav']!='no')
	{
	$js.="function pageselectCallback(page_id, jq){";
	$js.="$('.listloading').show().css('opacity',0.8);";
	$js.="$.getScript('{$_CFG['site_dir']}plus/ajax_map.php?jobshow={$jobshow}&pagenav=no&page='+page_id*{$perpage});";
    $js.="}";
	$js.="$('#pagination').pagination({$comcount}, {";
	$js.="items_per_page:10,";
	$js.="num_edge_entries:0,";
	$js.="num_display_entries: 6,";
	$js.="prev_text:'<',";
	$js.="next_text:'>',";
	$js.="ellipse_text:'',";
    $js.="callback: pageselectCallback";
    $js.="});";
	}
	exit(iconv_js($js));
}
if($id==0)
{
	$id=1;
}
	$js.="$(\".maploading\").show().css('opacity',0.7);";
	$js.="var map = new BMap.Map(\"mapcontainer\");";
	$js.="var point = new BMap.Point({$_CFG['map_center_x']}, {$_CFG['map_center_y']});";
	$js.="map.centerAndZoom(point,{$_CFG['map_zoom']});";
	$js.="var myCity = new BMap.LocalCity();";
	$js.="myCity.get(function (r) {";
	$js.="map.centerAndZoom(r.center, r.level);";
	$js.="});";
	$js.="map.addControl(new BMap.NavigationControl());";
	$js.="map.addControl(new BMap.OverviewMapControl());  ";
	$js.="map.addControl(new BMap.MapTypeControl());";
	$js.="map.enableScrollWheelZoom();";
	//$js.="map.setMinZoom(6);";
if ($id==1)
{	
	$js.="map.addEventListener(\"zoomend\", function(){ ;";
	$js.="var b = map.getBounds();";
	$js.="var ne = b.getNorthEast();";
	$js.="var sw = b.getSouthWest();";
	$js.="$('.maploading').hide();";
	$js.="$.getScript(\"{$_CFG['site_dir']}plus/ajax_map.php?jobshow=point:::\"+sw.lng+\",\"+ne.lng+\",\"+sw.lat+\",\"+ne.lat);";
	$js.="map.addEventListener(\"zoomend\", function(){ ;";
	$js.="if (map.getZoom()<8)";
	$js.="{";
	$js.="$(\"#infotiploading\").hide();";
	$js.="$(\"#infotipshow\").show().html('视野过大，无法获取信息');";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">视野过大，无法获取信息</li>');";
	$js.="map.clearOverlays();"; 
	$js.="$(\"#pagination\").empty();";
	$js.="}";
	$js.="else";
	$js.="{";
	$js.="var b = map.getBounds();";
	$js.="var ne = b.getNorthEast();";
	$js.="var sw = b.getSouthWest();";
	$js.="$(\"#infotiploading\").show();";
	$js.="$(\"#infotipshow\").hide();";
	$js.="$('.listloading').show().css('opacity',0.8);";
	$js.="$('.maploading').show().css('opacity',0.8);";
	$js.="$.getScript(\"{$_CFG['site_dir']}plus/ajax_map.php?jobshow=point:::\"+sw.lng+\",\"+ne.lng+\",\"+sw.lat+\",\"+ne.lat);";
	$js.="}";
	$js.="}); ";
	$js.="map.addEventListener(\"dragend\", function(){ ;";
	$js.="if (map.getZoom()<8)";
	$js.="{";
	$js.="$(\"#infotiploading\").hide();";
	$js.="$(\"#infotipshow\").show().html('视野过大，无法获取信息');";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">视野过大，无法获取信息</li>');";
	$js.="map.clearOverlays();";
	$js.="$(\"#pagination\").empty();";
	$js.="}";
	$js.="else";
	$js.="{";
	$js.="var b = map.getBounds();";
	$js.="var ne = b.getNorthEast();";
	$js.="var sw = b.getSouthWest();";
	$js.="$(\"#infotiploading\").show();";
	$js.="$(\"#infotipshow\").hide();";
	$js.="$('.listloading').show().css('opacity',0.8);";
	$js.="$('.maploading').show().css('opacity',0.8);";
	$js.="$.getScript(\"{$_CFG['site_dir']}plus/ajax_map.php?jobshow=point:::\"+sw.lng+\",\"+ne.lng+\",\"+sw.lat+\",\"+ne.lat);";
	$js.="}";
	$js.="}); ";
	$js.="}); ";
	exit(iconv_js($js));
}
elseif ($id==2)
{
	$js.="$('.maploading').html(\"请在先在右侧选择职位分类\");";
	$js.="$('.maploading').unbind().click(function(){";
	$js.="alert('请先在右侧选择职位分类,然后点击搜索');";
	$js.="});";
	$js.="$(\"#infotiploading\").hide();";
	$js.="$(\"#infotipshow\").show().html(\"请先在下方选择职位分类\");";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">请先在选择职位分类</li>');";
	$js.="$('#search').unbind().click(function(){";
	$js.="var jobcategory=$('#jobcategory').val();";
	$js.="if (jobcategory=='')";
	$js.="{";
	$js.="alert('请先选择职位分类,然后点击搜索');";
	$js.="}";
	$js.="else";
	$js.="{";
	$js.="$('.maploading').unbind();";
	$js.="$(\"#infotiploading\").show();";
	$js.="$(\"#infotipshow\").hide();";
	$js.="$('.maploading').html('<img src=\"{$_CFG['site_template']}images/90.gif\" />地图加载中...');";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">载入中...</li>');";
	$js.="$('.listloading').show().css('opacity',0.8);";
	$js.="$.getScript(\"{$_CFG['site_dir']}plus/ajax_map.php?jobshow=jobcategory:::\"+jobcategory);";
	$js.="}";
	$js.="});";
	exit(iconv_js($js));
}
elseif ($id==3)
{
	$js.="$('.maploading').html(\"请在先在右侧选择地区\");";
	$js.="$('.maploading').unbind().click(function(){";
	$js.="alert('请先在右侧选择地区分类,然后点击搜索');";
	$js.="});";
	$js.="$(\"#infotiploading\").hide();";
	$js.="$(\"#infotipshow\").show().html(\"请先在下方选择地区分类\");";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">请先在选择地区分类</li>');";
	$js.="$('#search').unbind().click(function(){";
	$js.="var citycategory=$('#citycategory').val();";
	$js.="if (citycategory=='')";
	$js.="{";
	$js.="alert('请先选择地区分类,然后点击搜索');";
	$js.="}";
	$js.="else";
	$js.="{";
	$js.="$('.maploading').unbind();";
	$js.="$(\"#infotiploading\").show();";
	$js.="$(\"#infotipshow\").hide();";
	$js.="$('.maploading').html('<img src=\"{$_CFG['site_template']}images/90.gif\" />地图加载中...');";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">载入中...</li>');";
	$js.="$('.listloading').show().css('opacity',0.8);";
	$js.="$.getScript(\"{$_CFG['site_dir']}plus/ajax_map.php?jobshow=citycategory:::\"+citycategory);";
	$js.="}";
	$js.="});";
	exit(iconv_js($js));
}
elseif ($id==4)
{
	$js.="$('.maploading').html(\"请在先在右侧选择行业分类\");";
	$js.="$('.maploading').unbind().click(function(){";
	$js.="alert('请先在右侧选择行业分类,然后点击搜索');";
	$js.="});";
	$js.="$(\"#infotiploading\").hide();";
	$js.="$(\"#infotipshow\").show().html(\"请先在下方选择行业分类\");";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">请先在选择行业分类</li>');";
	$js.="$('#search').unbind().click(function(){";
	$js.="var trade=$('#trade').val();";
	$js.="if (trade=='')";
	$js.="{";
	$js.="alert('请先选择行业分类,然后点击搜索');";
	$js.="}";
	$js.="else";
	$js.="{";
	$js.="$('.maploading').unbind();";
	$js.="$(\"#infotiploading\").show();";
	$js.="$(\"#infotipshow\").hide();";
	$js.="$('.maploading').html('<img src=\"{$_CFG['site_template']}images/90.gif\" />地图加载中...');";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">载入中...</li>');";
	$js.="$('.listloading').show().css('opacity',0.8);";
	$js.="$.getScript(\"{$_CFG['site_dir']}plus/ajax_map.php?jobshow=trade:::\"+trade);";
	$js.="}";
	$js.="});";
	exit(iconv_js($js));
}
elseif ($id==5)
{
	$js.="$('.maploading').html(\"请在先在右侧输入关键字\");";
	$js.="$('.maploading').unbind().click(function(){";
	$js.="alert('请先在右侧输入关键字,然后点击搜索');";
	$js.="});";
	$js.="$(\"#infotiploading\").hide();";
	$js.="$(\"#infotipshow\").show().html(\"请先在下方输入关键字\");";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">请先输入关键字</li>');";
	$js.="$('#search').unbind().click(function(){";
	$js.="var key=$('#key').val();";
	$js.="if (key=='' || key=='请输入关键字...')";
	$js.="{";
	$js.="alert('请输入关键字,然后点击搜索');";
	$js.="}";
	$js.="else";
	$js.="{";
	$js.="$('.maploading').unbind();";
	$js.="$(\"#infotiploading\").show();";
	$js.="$(\"#infotipshow\").hide();";
	$js.="$('.maploading').html('<img src=\"{$_CFG['site_template']}images/90.gif\" />地图加载中...');";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">载入中...</li>');";
	$js.="$('.listloading').show().css('opacity',0.8);";
	$js.="$.getScript(\"{$_CFG['site_dir']}plus/ajax_map.php?jobshow=key:::\"+key);";
	$js.="}";
	$js.="});";
	exit(iconv_js($js));
}
elseif ($id==6)
{
	$js.="$('.maploading').unbind();";
	$js.="$(\"#infotiploading\").show();";
	$js.="$(\"#infotipshow\").hide();";
	$js.="$('.maploading').html('<img src=\"{$_CFG['site_template']}images/90.gif\" />地图加载中...');";
	$js.="$(\"#infolist\").html('<li class=\"noinfo\">载入中...</li>');";
	$js.="$('.listloading').show().css('opacity',0.8);";
	$js.="$.getScript(\"{$_CFG['site_dir']}plus/ajax_map.php?jobshow=new:::1\");";
	exit(iconv_js($js));
}
?>