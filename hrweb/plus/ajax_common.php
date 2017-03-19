<?php
 /*
 * 74cms ajax
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
$act = !empty($_GET['act']) ? trim($_GET['act']) : '';
if ($act=="countinfo")
{
	$total="SELECT COUNT(*) AS num FROM ".table('company_profile');
	$total_company=$db->get_total($total);
	$total="SELECT COUNT(*) AS num FROM ".table('jobs').$wheresql;
	$total_jobs=$db->get_total($total);
	$total="SELECT COUNT(*) AS num FROM ".table('resume').$wheresql;
	$total_resume=$db->get_total($total);
	$total="SELECT COUNT(*) AS num FROM ".table('members');
	$total_members=$db->get_total($total);
		$contents = "企业总数：<span>{$total_company}</span>&nbsp;&nbsp;&nbsp;&nbsp;有效职位：<span>{$total_jobs}</span>&nbsp;&nbsp;&nbsp;&nbsp;有效简历：<span>{$total_resume}</span>&nbsp;&nbsp;&nbsp;&nbsp;会员总数：<span>{$total_members}</span>";
		$contents = ereg_replace("\t","",$contents); 
		$contents = ereg_replace("\r\n","",$contents); 
		$contents = ereg_replace("\r","",$contents); 
		$contents = ereg_replace("\n","",$contents);
		exit($contents);
}
elseif ($act=="company_down_resume")
{
	$wheresql = "";	
	$result = $db->query("SELECT a.down_addtime,b.companyname,b.id AS bid,b.addtime AS baddtime,c.id AS cid,c.sex,c.display_name,c.addtime AS caddtime,c.fullname FROM ".table('company_down_resume')." AS a LEFT JOIN  ".table('company_profile')." AS b ON a.company_uid=b.uid  LEFT JOIN ".table('resume')." AS c ON a.resume_id=c.id".$wheresql." ORDER BY a.down_addtime DESC  LIMIT 30");
	$html=array();
	while($row = $db->fetch_array($result))
	{
		if ($row['companyname'] && $row['fullname'] && empty($html[$row['bid']]))
		{
		$row['time']=daterange(time(),$row['down_addtime'],'Y-m-d',"#FF3300");
		if ($row['display_name']=="2")
		{
		$row['fullname']="N".str_pad($row['cid'],7,"0",STR_PAD_LEFT);
		}
		elseif ($row['display_name']=="3")
		{
			if($row['sex']==1)
			{
				$row['fullname']=cut_str($row['fullname'],1,0,"先生");
			}
			elseif($row['sex']==2)
			{
				$row['fullname']=cut_str($row['fullname'],1,0,"女士");
			}
		}
		if($_CFG['closetime']==1){
			$html[$row['bid']]="<li><a href=".url_rewrite('QS_companyshow',array('id'=>$row['bid']))." target=\"_blank\">".$row['companyname']." </a> 下载了 <a href=".url_rewrite('QS_resumeshow',array('id'=>$row['cid']))." target=\"_blank\">".$row['fullname']." </a>的个人简历</li>";
		}else{
			$html[$row['bid']]="<li><a href=".url_rewrite('QS_companyshow',array('id'=>$row['bid']))." target=\"_blank\">".$row['companyname']." </a> 下载了 <a href=".url_rewrite('QS_resumeshow',array('id'=>$row['cid']))." target=\"_blank\">".$row['fullname']." </a>的个人简历<span>{$row['time']}</span></li>";
		}
		}
	}
	exit(implode("",$html));
}
elseif($act=="hotword")
{
	if (empty($_GET['query']))
	{
	exit();
	}
	$gbk_query=trim($_GET['query']);
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
	$gbk_query=utf8_to_gbk($gbk_query);
	}
	$sql="SELECT * FROM ".table('hotword')." WHERE w_word like '%{$gbk_query}%' ORDER BY `w_hot` DESC LIMIT 0 , 10";
	$result = $db->query($sql);
	while($row = $db->fetch_array($result))
	{
		$list[]="'".$row['w_word']."'";
	}
	if ($list)
	{
	$liststr=implode(',',$list);
	$str="{";
	$str.="query:'{$gbk_query}',";
	$str.="suggestions:[{$liststr}]";
	$str.="}";
	exit($str);
	}
}
// 邮箱自动提示
elseif($act=="reg_email")
{
	if (empty($_GET['query']))
	{
	exit();
	}
	$gbk_query=trim($_GET['query']);
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
	$gbk_query=utf8_to_gbk($gbk_query);
	}
	$gbk_query = explode("@", $gbk_query);
	$gbk_query = $gbk_query[0];
	$list = array(
			0 => "'{$gbk_query}@qq.com'",
			1 => "'{$gbk_query}@163.com'",
			2 => "'{$gbk_query}@126.com'",
			3 => "'{$gbk_query}@hotmail.com'",
			4 => "'{$gbk_query}@yahoo.com'",
			5 => "'{$gbk_query}@sina.com'",
			6 => "'{$gbk_query}@gmail.com'",
			7 => "'{$gbk_query}@sogou.com'",
			8 => "'{$gbk_query}@139.com'"
		);
	if ($list)
	{
	$liststr=implode(',',$list);
	$str="{";
	$str.="query:'{$gbk_query}',";
	$str.="suggestions:[{$liststr}]";
	$str.="}";
	exit($str);
	}
}
elseif($act=="joblisttip")
{
	$uid=intval($_GET['uid']);
	$wheresql='';
	$result = $db->query("SELECT * FROM ".table('jobs')." WHERE uid='{$uid}' {$wheresql} ORDER BY refreshtime desc limit 6");
	$i=1;
	while($row = $db->fetch_array($result))
	{
				
				$row['jobs_name']=cut_str($row['jobs_name'],8,0,"");
				
				$row['refreshtime_cn']=daterange(time(),$row['refreshtime'],'Y-m-d',"#FF3300"); 
				if (!empty($row['highlight']))
				{
				$row['jobs_name']="<span style=\"color:{$row['highlight']}\">{$row['jobs_name']}</span>";
				} 
				$row['companyname_']=$row['companyname'];
				$row['companyname']=cut_str($row['companyname'],15,0,"...");
				$row['jobs_url']=url_rewrite('QS_jobsshow',array('id'=>$row['id']));
				$row['company_url']=url_rewrite('QS_companyshow',array('id'=>$row['company_id']));
				if ($i>5)
				{
				$html.="<li class=\"more\"><a href=\"{$row['company_url']}\" target=\"_blank\">更多...</span></li>";
				break;
				}
				if($_CFG['closetime']==1){
					$html.="<li><a href=\"{$row['jobs_url']}\" target=\"_blank\">{$row['jobs_name']}</a></li>";
				}else{
					$html.="<li><a href=\"{$row['jobs_url']}\" target=\"_blank\">{$row['jobs_name']}</a>&nbsp;&nbsp;<span>({$row['refreshtime_cn']})</span></li>";
				}
				
				
				$i++;				
	}
	exit($html);
}
elseif($act=="ajaxcomlist")
		{
			$categoryid=intval($_GET['categoryid']);
			$tradeid=intval($_GET['tradeid']);
			$showtype=trim($_GET['showtype']);
			$limit=intval($_GET['comrow'])>0?intval($_GET['comrow']):10;
			$jobrow=intval($_GET['jobrow'])>0?intval($_GET['jobrow']):3;
			$companynamelen=intval($_GET['companynamelen'])>0?intval($_GET['companynamelen']):12;
			$jobslen=intval($_GET['jobslen'])>0?intval($_GET['jobslen']):6;	
			$jobstable=table('jobs_search_rtime');
			$wheresql='';
			$ordersql='  ORDER BY refreshtime desc ';
			$limitsql=" LIMIT ".$limit*15;
			if ($showtype=="category")
			{
				if ($categoryid>0)
				{
				$wheresql.=" AND topclass='{$categoryid}' ";
				}
			}
			elseif($showtype=="trade" && $tradeid>0)
			{
				$wheresql.=" AND trade='{$tradeid}' ";
			}
			if (!empty($wheresql))
			{
			$wheresql=" WHERE ".ltrim(ltrim($wheresql),'AND');
			}
			$result = $db->query("SELECT id,uid FROM ".$jobstable.$wheresql.$ordersql.$limitsql);
			$uidarr= array();
			while($row = $db->fetch_array($result))
			{
				if (count($uidarr)>=$limit) break;
				$uidarr[$row['uid']]=$row['uid'];
			}
			if (!empty($uidarr))
			{
				$uidarr= implode(",",$uidarr);
				$wheresql=$wheresql?$wheresql." AND uid IN ({$uidarr}) ":" WHERE uid IN ({$uidarr}) ";
				$sql="SELECT company_audit,company_id,companyname,company_addtime,refreshtime,id,jobs_name,addtime,uid,click,highlight,highlight,setmeal_id,setmeal_name FROM ".table('jobs').$wheresql.$ordersql;
				$countuid=array();
				$result = $db->query($sql);
				while($row = $db->fetch_array($result))
				{		
					$countuid[$row['uid']][]=$row['uid'];
					if (count($countuid[$row['uid']])>$jobrow)continue;
					$companyarray[$row['uid']]['companyname_']=$row['companyname'];
					$companyarray[$row['uid']]['companyname']=cut_str($row['companyname'],$companynamelen,0,'');
					$companyarray[$row['uid']]['company_url']=url_rewrite('QS_companyshow',array('id'=>$row['company_id']));
					$companyarray[$row['uid']]['company_addtime']=$row['company_addtime'];
					$companyarray[$row['uid']]['refreshtime']=$companyarray[$row['uid']]['refreshtime']>$row['refreshtime']?$companyarray[$row['uid']]['refreshtime']:$row['refreshtime'];
					$companyarray[$row['uid']]['refreshtime_cn']=daterange(time(),$companyarray[$row['uid']]['refreshtime'],'m-d',"#A9A9A9");
					$companyarray[$row['uid']]['setmeal_id']=$row['setmeal_id'];
					$companyarray[$row['uid']]['company_audit']=$row['company_audit'];
					$companyarray[$row['uid']]['setmeal_name']=$row['setmeal_name'];
					$companyarray[$row['uid']]['uid']=$row['uid'];
					$companyarray[$row['uid']]['jobs'][$row['id']]['jobs_addtime']=$row['addtime'];
					$companyarray[$row['uid']]['jobs'][$row['id']]['jobs_refreshtime']=$row['refreshtime'];
					$companyarray[$row['uid']]['jobs'][$row['id']]['jobs_click']=$row['click'];
					$companyarray[$row['uid']]['jobs'][$row['id']]['jobs_name']=cut_str($row['jobs_name'],$jobslen,0,'');
						if (!empty($row['highlight']))
						{
						$companyarray[$row['uid']]['jobs'][$row['id']]['jobs_name']="<span style=\"color:{$row['highlight']}\">{$companyarray[$row['uid']]['jobs'][$row['id']]['jobs_name']}</span>";
						}
					$companyarray[$row['uid']]['jobs'][$row['id']]['jobs_url']=url_rewrite('QS_jobsshow',array('id'=>$row['id']));
				}
			}
			if (!empty($companyarray))
			{
				$html='';
				foreach($companyarray as $li)
				{
				if($_CFG['closetime']==1){
					$html.="<li><p><a class=\"com_name\" href=\"{$li['company_url']}\" target=\"_blank\"><span class=\"comtip\" id=\"{$li['uid']}\">{$li['companyname']}</span>&nbsp;";
					if ($li['company_audit'] == 1) {
						$html.="<img title=\"企业已认证\" class=\"vtip\" src=\"$_CFG[site_template]images/iconren.jpg\" border=\"0\">&nbsp;";
					}
					if ($_CFG['operation_mode'] >= 2 && $li['setmeal_id'] > 1) {
						$html.="<img src=\"$_CFG[site_dir]data/setmealimg/{$li['setmeal_id']}.gif\" border=\"0\" title=\"{$li['setmeal_name']}\" class=\"vtip\"/>";
					}
					$html.="</a><span>&nbsp;&nbsp;&nbsp;&nbsp;</span></p>";
				}else{
					$html.="<li><p><a class=\"com_name\" href=\"{$li['company_url']}\" target=\"_blank\"><span class=\"comtip\" id=\"{$li['uid']}\">{$li['companyname']}</span>&nbsp;";
					if ($li['company_audit'] == 1) {
						$html.="<img title=\"企业已认证\" class=\"vtip\" src=\"$_CFG[site_template]images/iconren.jpg\" border=\"0\">&nbsp;";
					}
					if ($_CFG['operation_mode'] >= 2 && $li['setmeal_id'] > 1) {
						$html.="<img src=\"$_CFG[site_dir]data/setmealimg/{$li['setmeal_id']}.gif\" border=\"0\" title=\"{$li['setmeal_name']}\" class=\"vtip\"/>";
					}
					$html.="</a><span>{$li['refreshtime_cn']}更新</span></p>";
				}
		 		
		 		if (!empty($li['jobs'])){
		 			$html.="<p>聘：";
		 			foreach($li['jobs'] as $jobsli){
			 			$html.="<a href=\"{$jobsli['jobs_url']}\" target=\"_blank\">{$jobsli['jobs_name']}</a>";
			 		}
			 		$html.="</p>";
		 		}
				}
			}
			exit($html);
		}
elseif($act=="ajaxjoblist")
{
	$trade=intval($_GET['trade']);
	$wheresql=$trade>0?' WHERE trade='.$trade." ":"";	
	$result = $db->query("SELECT * FROM ".table('jobs')." {$wheresql} ORDER BY refreshtime desc limit 12");
	$i=1;
	$html = '<ul class="clearfix">';
	while($row = $db->fetch_array($result))
	{
				$row['jobs_name']=cut_str($row['jobs_name'],12,0,"");
				$row['refreshtime_cn']=daterange(time(),$row['refreshtime'],'Y-m-d',"#FF3300"); 
				if (!empty($row['highlight']))
				{
				$row['jobs_name']="<span style=\"color:{$row['highlight']}\">{$row['jobs_name']}</span>";
				} 
				$row['companyname_']=$row['companyname'];
				$row['companyname']=cut_str($row['companyname'],13,0,"");
				$row['jobs_url']=url_rewrite('QS_jobsshow',array('id'=>$row['id']));
				$row['company_url']=url_rewrite('QS_companyshow',array('id'=>$row['company_id']));
				if($_CFG['closetime']==1){
					$html.="<li class=\"clearfix\"><span>.</span><a target=\"_blank\" href=\"{$row['jobs_url']}\">{$row['jobs_name']}</a><b><a target=\"_blank\" href=\"{$row['company_url']}\">{$row['companyname']}</a></b></li>";
				}else{
					$html.="<li class=\"clearfix\"><span>.</span><a target=\"_blank\" href=\"{$row['jobs_url']}\">{$row['jobs_name']}</a><b><a target=\"_blank\" href=\"{$row['company_url']}\">{$row['companyname']}</a></b><em>{$row['refreshtime_cn']}</em></li>";
				}
				
				
				$i++;				
	}
	$html .= '</div>';
	exit($html);
}
// 解除微信绑定
elseif($act == "bind_wx_jc")
{
	$uid=intval($_GET['uid']);
	$user=$db->getone("SELECT * FROM ".table("members")." where uid=$uid");
	$bind_wx_true=intval($_GET['bind_wx_true']);
	$html="";
	if($bind_wx_true)
	{
		$db->query("update ".table('members')." set weixin_openid=null,bindingtime='' where uid=$uid ");

		$html.="<div style='padding:10px 20px 20px 20px;'>";
		$html.="<div style='font-size: 14px;line-height: 1.8;'>解除微信绑定成功</div>";
		$html.="</div>";
	}
	else
	{
		if($user['email_audit']!=1 && $user['mobile_audit']!=1)
		{
			$html.="<div class='unbindBox unbindBoxs'>";
			$html.="<div class='con'><div class='f-left'><img src='$_CFG[site_template]images/wx_showmsg.jpg'></div><div class='f-right tex'>检测到您的账号没有绑定手机和邮箱，关闭微信安全登录后如果密码丢失后<span class='class'>将无法找回密码</span>，确定要关闭微信安全登录吗？</div><div class='clear'></div></div>";
			$html.="<div class='sclosePd'><a class='sclose f-left' href='javascript:;' id='bind_wx_true' uid='".$user['uid']."'>确认</a><a class='sclose f-left sclosem' href='javascript:;'>取消</a></div>";
			$html.="</div>";
		}
		else
		{
			$html.="<div class='unbindBox unbindBoxs'>";
			$html.="<div class='con'><div class='f-left'><img src='$_CFG[site_template]images/wx_showmsg.jpg'></div><div class='f-right tex'>微信登录更简单安全，且可防止木马、键盘录制窃取密码，确定要关闭微信安全登录吗？</div><div class='clear'></div></div>";
			$html.="<div class='sclosePd'><a class='sclose f-left' href='javascript:;' id='bind_wx_true' uid='".$user['uid']."'>确认</a><a class='sclose f-left sclosem' href='javascript:;'>取消</a></div>";
			$html.="</div>";
		}
	}
	exit($html);
}
elseif($act == 'waiting_weixin_scan'){
	$event_key = $_SESSION['scene_id'];
	$openid = "";
	if(file_exists(QISHI_ROOT_PATH."data/weixin/".($event_key%10).'/'.$event_key.".txt")){
		$openid = file_get_contents(QISHI_ROOT_PATH."data/weixin/".($event_key%10).'/'.$event_key.".txt");
	}
	if($openid){
		$access_token = get_access_token();
		$w_url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=".$access_token."&openid=".$openid."&lang=zh_CN";
		$w_result = https_request($w_url);
		$w_userinfo = json_decode($w_result,true);
		$w_userinfo = array_map('utf8_to_gbk', $w_userinfo);
		global $db;
		$result = $db->query("update ".table('members')." set weixin_openid='".$openid."',weixin_nick='".$w_userinfo['nickname']."' where uid=".$_SESSION['uid']." and weixin_openid IS NULL");
		if($result){
			// 绑定微信 获得积分
			$rule=get_cache('points_rule');
			if ($rule['company_wx_points']['value']>0)
			{
				$info=$db->getone("SELECT uid FROM ".table('members_handsel')." WHERE uid ='{$_SESSION['uid']}' AND htype='company_wx_points' LIMIT 1");
				if(empty($info))
				{
				$time=time();			
				$db->query("INSERT INTO ".table('members_handsel')." (uid,htype,addtime) VALUES ('{$_SESSION['uid']}', 'company_wx_points','{$time}')");
				require_once(QISHI_ROOT_PATH.'include/fun_comapny.php');
				report_deal($_SESSION['uid'],$rule['company_wx_points']['type'],$rule['company_wx_points']['value']);
				$user_points=get_user_points($_SESSION['uid']);
				$operator=$rule['company_wx_points']['type']=="1"?"+":"-";
				$_SESSION['handsel_company_wx_points']=$_CFG['points_byname'].$operator.$rule['company_wx_points']['value'];
				write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username']," 绑定微信，{$_CFG['points_byname']}({$operator}{$rule['company_wx_points']['value']})，(剩余:{$user_points})",1,1016,"绑定微信","{$operator}{$rule['company_wx_points']['value']}","{$user_points}");
				}
			}
			unlink(QISHI_ROOT_PATH."data/weixin/".($event_key%10).'/'.$event_key.".txt");
			exit("1");
		}else{
			exit("-1");
		}
	}
}
?>