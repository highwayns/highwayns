<?php
/*********************************************
*骑士广告
* *******************************************/
function tpl_function_qishi_ad($params, &$smarty)
{
global $db,$_CFG;
$arrset=explode(',',$params['set']);
foreach($arrset as $str)
{
$a=explode(':',$str);
	switch ($a[0])
	{
	case "显示数目":
		$aset['row'] = $a[1];
		break;
	case "开始位置":
		$aset['start'] = $a[1];
		break;
	case "文字长度":
		$aset['titlelen'] = $a[1];
		break;
	case "填补字符":
		$aset['dot'] = $a[1];
		break;
	case "调用名称":
		$aset['alias'] = $a[1];
		break;
	case "列表名":
		$aset['listname'] = $a[1];
		break;
	case "排序":
		$aset['displayorder'] = $a[1];
		break;
	}
}
$aset['row']=isset($aset['row'])?intval($aset['row']):10;
$aset['start']=isset($aset['start'])?intval($aset['start']):0;
$aset['titlelen']=isset($aset['titlelen'])?intval($aset['titlelen']):15;
$aset['listname']=isset($aset['listname'])?$aset['listname']:"list";
$aset['dot']=isset($aset['dot'])?$aset['dot']:null;
$aset['displayorder']=isset($aset['displayorder'])?$aset['displayorder']:null;
unset($arr,$str,$a,$params);
$wheresql=" WHERE alias='".$aset['alias']."' AND( starttime<=".time()."  OR starttime=0 ) AND (deadline>=".time()." OR deadline='0' ) AND is_display=1 ";
$limit=" LIMIT ".$aset['start'].','.$aset['row'];
if ($aset['displayorder'])
{
	if (strpos($aset['displayorder'],'>'))
	{
		$arr=explode('>',$aset['displayorder']);
		// 排序字段
		if($arr[0]=='show_order'){
			$arr[0]="show_order";
		}
		elseif($arr[0]=="id")
		{
			$arr[0]="id";
		}
		else
		{
			$arr[0]="";
		}
		// 排序方式
		if($arr[1]=='desc'){
			$arr[1]="desc";
		}
		elseif($arr[1]=="asc")
		{
			$arr[1]="asc";
		}
		else
		{
			$arr[1]="";
		}

		if ($arr[0] && $arr[1])
		{
		$orderbysql=" ORDER BY ".$arr[0]." ".$arr[1];
		}
	}
}
else
{
	$orderbysql=" ORDER BY show_order DESC ";
}
$result = $db->query("SELECT * FROM ".table('ad')." ".$wheresql.$orderbysql.$limit);
$arr=array();
while($row = $db->fetch_array($result))
{
	if ($row['type_id']=="1")//文字
	{
	$list['text_content_']=$row['text_content'];
	$list['text_content']=cut_str($row['text_content'],$aset['titlelen'],0,$aset['dot']);
	$list['text_url']=$row['text_url'];
	$row['text_color']?$list['text_content']="<span style=\"color:{$row['text_color']}\">{$list['text_content']}</span>":'';
	}
	elseif ($row['type_id']=="2")//图片
	{
		$list['img_path']=$row['img_path'];
		$list['img_url']=$row['img_url'];
		$list['img_explain_']=$row['img_explain'];
		$list['img_explain']=cut_str($row['img_explain'],$aset['titlelen'],0,$aset['dot']);
		$list['img_uid']=$row['img_uid'];
		unset($list['jobs']);
		if($row['img_uid'] > 0) {
			$companyinfo = $db->getone("select id,companyname,contents from ".table('company_profile')." where uid={$row['img_uid']}");
			$list["briefly"]=htmlspecialchars_decode(strip_tags($companyinfo['contents']),ENT_QUOTES);
			unset($list["companyname"]);
			$list["companyname"]=strip_tags($companyinfo['companyname']);
			$list["company_url"]=url_rewrite("QS_companyshow",array('id'=>$companyinfo['id']));
			if($list['img_url']=="")
			{
				$list['img_url']=$list["company_url"];
			}
			$jobsarray = $db->getall("select * from ".table('jobs')." where uid={$row['img_uid']}");
			unset($list['jobs']);
			foreach ($jobsarray as $key=>$val) {
				$val["jobs_url"]=url_rewrite("QS_jobsshow",array('id'=>$val['id']));
				$list['jobs'][$key]=$val;
			}
		}
	}
	elseif ($row['type_id']=="3")//代码
	{
	$list['code']=$row['code_content'];
	}
	elseif ($row['type_id']=="4")//flash
	{
	$list['flash_path']=$row['flash_path'];
	$list['flash_width']=$row['flash_width'];
	$list['flash_height']=$row['flash_height'];
	}
	elseif ($row['type_id']=="5")//浮动
	{
	$list['type_id']=$row['type_id'];
	$list['id']=$row['id'];
	$list['floating_type']=$row['floating_type'];
	$list['floating_width']=$row['floating_width'];
	$list['floating_height']=$row['floating_height'];
	$list['floating_url']=$row['floating_url'];
	$list['floating_path']=$row['floating_path'];
	$list['floating_left']=$row['floating_left'];
	$list['floating_right']=$row['floating_right'];
	$list['floating_top']=$row['floating_top'];
	$list['float_code']='';	
	}
	elseif ($row['type_id']=="6")//视频
	{
	$list['type_id']=$row['type_id'];
	$list['video_path']=$row['video_path'];
	$list['video_width']=$row['video_width'];
	$list['video_height']=$row['video_height'];
	$list['video_code']='';	
	}
	$list['type_id']=$row['type_id'];
	$arr[]=$list;
	// echo "<pre>";
	// print_r($arr);
	// echo "</pre>";
}
if(!empty($arr) && $arr[0]['type_id']=="5")
{
	$arr=qs_ad_floating($arr);
}
elseif (!empty($arr) && $arr[0]['type_id']=="6")
{
	$arr=qs_ad_video($arr);
}
$smarty->assign($aset['listname'],$arr);
unset($alist,$row,$aset,$list);
}
function qs_ad_floating($arr)
{
	global $_CFG;
	if (empty($arr)) return array('float_code'=>'');
	$html="";
	$html.="<SCRIPT LANGUAGE=\"JavaScript\">\n"; 
	$html.="<!--\n";
	foreach($arr as $str)
	{
	$floatingstr=$str['floating_left']!==""?" LEFT: ".$str['floating_left']."px;":" RIGHT: ".$str['floating_right']."px;";
		if ($str['floating_type']=="1")
		{
		$html.="suspendcode=\"<DIV id=\'floating".$str['id']."\' style='Z-INDEX: 10".$str['id'].";".$floatingstr." POSITION: absolute; TOP: ".$str['floating_top']."px; width: ".$str['floating_width']."; height: ".($str['floating_height']+14)."px;'><img src='".$_CFG['site_template']."images/45close.gif' onClick=javascript:close_divs(\'floating".$str['id']."\') width='100' height='14' border='0' vspace='3' alt='关闭广告'><br/><a href='".$str['floating_url']."' target='_blank'><img src='".$str['floating_path']."' width='".$str['floating_width']."' height='".$str['floating_height']."' border='0'></a></DIV>\";\n"; 
		$html.="document.write(suspendcode);\n"; 
		}
		if ($str['floating_type']=="2")
		{
		$html.="suspendcode=\"<DIV id=\'floating".$str['id']."\' style='Z-INDEX: 10".$str['id'].";".$floatingstr." POSITION: absolute; TOP: ".$str['floating_top']."px; width: ".$str['floating_width']."; height: ".($str['floating_height']+14)."px;'><img src='".$_CFG['site_template']."images/45close.gif' onClick=javascript:close_divs(\'floating".$str['id']."\') width='100' height='14' border='0' vspace='3' alt='关闭广告'><br/><object classid=\'clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\' codebase=\'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0\' width=\'".$str['floating_width']."\' height=\'".$str['floating_height']."\'><param name=\'movie\' value=\'".$str['floating_path']."\' /><param name=\'quality\' value=\'high\' /><embed src=\'".$str['floating_path']."\' quality=\'high\' pluginspage=\'http://www.macromedia.com/go/getflashplayer\' type=\'application/x-shockwave-flash\' width=\'".$str['floating_width']."\' height=\'".$str['floating_height']."\'></embed></object></DIV>\";\n"; 
		$html.="document.write(suspendcode);\n"; 
		}
	}
	$html.="lastScrollY = 0;\n"; 
	$html.="function heartBeat(){\n"; 
	$html.="var diffY;\n"; 
	$html.="if (document.documentElement && document.documentElement.scrollTop)\n"; 
	$html.="diffY = document.documentElement.scrollTop;\n"; 
	$html.="else if (document.body)\n"; 
	$html.="diffY = document.body.scrollTop;\n"; 
	$html.="else\n"; 
	$html.="{/*Netscape stuff*/}\n"; 
	$html.="percent=.1*(diffY-lastScrollY);\n"; 
	$html.="if(percent>0)percent=Math.ceil(percent);\n"; 
	$html.="else percent=Math.floor(percent);\n"; 
		foreach($arr as $str)
		{
		$html.="document.getElementById(\"floating".$str['id']."\").style.top = parseInt(document.getElementById(\"floating".$str['id']."\").style.top)+percent+\"px\";\n"; 
		}
	$html.="lastScrollY=lastScrollY+percent;\n"; 
	$html.="}\n"; 
	$html.="window.setInterval(\"heartBeat()\",1);\n"; 
	$html.="function close_divs(id)\n"; 
	$html.="{\n"; 
	$html.="document.getElementById(id).style.visibility='hidden';\n"; 
	$html.="}\n";
	$html.="//-->\n"; 
	$html.="</SCRIPT>\n";
	return array('float_code'=>$html);
}
function qs_ad_video($arr)
{
	global $_CFG;
	if (empty($arr)) return array('video_code'=>'');
	$html="";
	foreach($arr as $str)
	{
	$html.="<object type=\"application/x-shockwave-flash\" data=\"".$_CFG['site_dir']."data/comads/vcastr.swf\" width=\"".$str['video_width']."\" height=\"".$str['video_height']."\" id=\"vcastr\">";	
	$html.="<param name=\"movie\" value=\"".$_CFG['site_dir']."data/comads/vcastr.swf\"/> ";
	$html.="<param name=\"allowFullScreen\" value=\"true\" />";
	$html.="<param name=\"FlashVars\" value=\"xml=";
	$html.="{vcastr}";
	$html.="{channel}";
	$html.="{item}";
	$html.="{source}".$str['video_path']."{/source}";
	$html.="{duration}{/duration}";
	$html.="{title}{/title}";
	$html.="{/item}";
	$html.="{/channel}";
	$html.="{config}";
	$html.="{isAutoPlay}false{/isAutoPlay}";
	$html.="{/config}";
	$html.="{plugIns}";
	$html.="{/plugIns}";
	$html.="{/vcastr}\"/>";
	$html.="</object>\n";
	}
	return array('video_code'=>$html);
}
?>