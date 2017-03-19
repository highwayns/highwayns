<?php
 /*
 * 74cms ajax返回
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../include/plus.common.inc.php');
require_once(dirname(__FILE__).'/../include/fun_wap.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : '';
if($act == 'ajaxjobslist'){
	$jobslisthtml="";
	if(isset($_GET['recommend'])&&intval($_GET['recommend'])==1){
		$wheresql.=" AND `recommend`=1 ";
	}
	if(isset($_GET['emergency'])&&intval($_GET['emergency'])==1){
		$wheresql.=" AND `emergency`=1 ";
	}
	if (!empty($wheresql))
	{
	$wheresql=" WHERE ".ltrim(ltrim($wheresql),'AND');
	}
	$rows=intval($_GET['rows']);
	$offset=intval($_GET['offset']); 
	$jobslistarray=$db->getall("select * from ".table('jobs').$wheresql." ORDER BY `refreshtime` DESC LIMIT {$offset},{$rows}");
	if (!empty($jobslistarray) && $offset<=100)
	{
		foreach($jobslistarray as $li)
		{
			$url = wap_url_rewrite("wap-jobs-show",array("id"=>$li["id"]));
			$jobslisthtml.='<div class="list" id="li-'.$offset.'" url="'.$url.'">
	  <div class="t1"><span><a href="'.$url.'">'.$li["jobs_name"].'</a></span><br />
'.$li["companyname"].'</div>
	  <div class="t2">'.$li["district_cn"].'<br />'.$li["wag_cn"].'</div>
	  <div class="t3"><img src="images/14.jpg"  border="0"/></div>
	  <div class="clear"></div>
	</div>';
	}
		exit($jobslisthtml);
	}
	else
	{
		exit('-1');
	}
}
elseif($act == 'ajaxnewslist'){
	$newslisthtml="";
	$rows=intval($_GET['rows']);
	$offset=intval($_GET['offset']); 
	$newslistarray=$db->getall("select * from ".table('article')." ORDER BY `id` DESC LIMIT {$offset},{$rows}");
	if (!empty($newslistarray) && $offset<=100)
	{
		foreach($newslistarray as $li)
		{
			$url = wap_url_rewrite("wap-news-show",array("id"=>$li["id"]));
			$newslisthtml.='<div class="list" id="li-'.$offset.'" url="'.$url.'">
	  <div class="t1"><span><a href="'.$url.'">'.$li["title"].'</a></span></div>
	  <div class="t2"><br /></div>
	  <div class="t3"><img src="images/14.jpg"  border="0"/></div>
	  <div class="clear"></div>
	</div>';
	}
		exit($newslisthtml);
	}
	else
	{
		exit('-1');
	}
}
elseif($act == 'ajaxcomjobslist'){
	$jobslisthtml="";
	$rows=intval($_GET['rows']);
	$offset=intval($_GET['offset']); 
	$companyid=intval($_GET['companyid']); 
	$jobslistarray=$db->getall("select * from ".table('jobs')." WHERE `company_id`={$companyid} ORDER BY `refreshtime` DESC LIMIT {$offset},{$rows}");
	if (!empty($jobslistarray) && $offset<=100)
	{
		foreach($jobslistarray as $li)
		{
			$url = wap_url_rewrite("wap-jobs-show",array("id"=>$li["id"]));
			$jobslisthtml.='<div class="list" url="'.$url.'" id="li-'.$offset.'">
	<h3><a href="'.$url.'">'.$li["jobs_name"].'</a></h3>
	<h5>'.$li["wage_cn"].' '.$li["district_cn"].' </h5>   
	</div>';
	}
		exit($jobslisthtml);
	}
	else
	{
		exit('-1');
	}
}
elseif($act == 'ajaxresumelist'){
	$resumelisthtml="";
	$rows=intval($_GET['rows']);
	$offset=intval($_GET['offset']); 
	$resumelistarray=$db->getall("select * from ".table('resume')." ORDER BY `refreshtime` DESC LIMIT {$offset},{$rows}");
	if (!empty($resumelistarray) && $offset<=100)
	{
		foreach($resumelistarray as $li)
		{
			$url = wap_url_rewrite("wap-resume-show",array("id"=>$li["id"]));
			$resumelisthtml.='<div class="list" id="li-'.$offset.'" url="'.$url.'">
	  <span><a href="'.$url.'">'.$li["fullname"].'('.$li["sex_cn"].')</a></span><br />
学历：'.$li["education_cn"].'  工作经验：'.$li["experience_cn"].'<br />
'.$li["intention_jobs"].'
	</div>';
	}
		exit($resumelisthtml);
	}
	else
	{
		exit('-1');
	}
}
elseif($act == 'jobs_contact')
{
	$id=intval($_GET['id']);
	if ($id>0)
	{
		$show=false;
		if($_CFG['showjobcontact']=='0')
		{
		$show=true;
		}
		elseif($_CFG['showjobcontact']=='1')
		{
			if ($_SESSION['uid'] && $_SESSION['username'] && $_SESSION['utype']=='2')
			{
			$show=true;
			}
			else
			{
			$show=false;
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
  			$html.='个人会员请<a href="wap_login.php">[登录]</a>后查看联系方式<br />没有帐号？<a href="wap_user_reg.php">[免费注册]</a>';
  			$html.='</div><div class="telimg"></div>';
			}
		}
		elseif($_CFG['showjobcontact']=='2')
		{
			if ($_SESSION['uid'] && $_SESSION['username'] && $_SESSION['utype']=='2')
			{
				$val=$db->getone("select uid from ".table('resume')." where uid='{$_SESSION['uid']}' LIMIT 1");
			 	if (!empty($val))
				{
				$show=true;
				}
				else
				{
				$show=false;
				$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
				$html.="您没有发布简历或者简历无效，发布简历后才可以查看联系方式。<a href=\"".get_member_url($_SESSION['utype'],true)."personal_resume.php?act=resume_list\">[查看我的简历]</a>";
				$html.='</div><div class="telimg"></div>';
				}
			}
			else
			{
			$show=false;
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
			$html.='个人会员请<a href="wap_login.php">[登录]</a>后查看联系方式<br />没有帐号？<a href="wap_user_reg.php">[免费注册]</a>';
			$html.='</div><div class="telimg"></div>';
			}
		}
		if ($show)
		{
		$sql = "select * from ".table('jobs_contact')." where pid='{$id}' LIMIT 1";
		$val=$db->getone($sql);
			if ($_CFG['contact_img_job']=='2')
			{
			$token=md5($val['contact'].$id.$val['telephone']);
			$contact=$val['contact_show']=='1'?"联系人：<img src=\"{$_CFG['site_dir']}plus/contact_img.php?act=jobs_contact&type=1&id={$id}&token={$token}\"  border=\"0\" align=\"absmiddle\"/><br />":"联系人：企业设置不对外公开<br />";
			$telephone=$val['telephone_show']=='1'?"联系电话：<img src=\"{$_CFG['site_dir']}plus/contact_img.php?act=jobs_contact&type=2&id={$id}&token={$token}\"  border=\"0\" align=\"absmiddle\"/><br />":"联系电话：企业设置不对外公开<br />";
			$address=$val['address_show']=='1'?"联系地址：<img src=\"{$_CFG['site_dir']}plus/contact_img.php?act=jobs_contact&type=4&id={$id}&token={$token}\"  border=\"0\" align=\"absmiddle\"/><br />":"联系地址：企业设置不对外公开<br />";
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt telbox">';
			$html.=$contact.$telephone.$address;
			$html.='</div><div class="telimg"><a href="wtai://wp/mc;'.$val["telephone"].'"><img src="images/23.jpg"  border="0"/></a></div>';
			}
			else
			{
			$contact=$val['contact_show']=='1'?"联系人：{$val['contact']}<br />":"联系人：企业设置不对外公开<br />";
			$telephone=$val['telephone_show']=='1'?"联系电话：{$val['telephone']}<br />":"联系电话：企业设置不对外公开<br />";
			$address=$val['address_show']=='1'?"联系地址：{$val['address']}<br />":"联系地址：企业设置不对外公开<br />";
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
			$html.=$contact.$telephone.$address;
			$html.='</div><div class="telimg"><a href="wtai://wp/mc;'.$val["telephone"].'"><img src="images/23.jpg"  border="0"/></a></div>';
			}
		exit($html);
		}
		else
		{		
		exit($html);
		}
	}
}
elseif($act == 'company_contact')
{
	
	$id=intval($_GET['id']);
	if ($id>0)
	{
		$show=false;
		if($_CFG['showjobcontact']=='0')
		{
		$show=true;
		}
		elseif($_CFG['showjobcontact']=='1')
		{
			if ($_SESSION['uid'] && $_SESSION['username'] && $_SESSION['utype']=='2')
			{
			$show=true;
			}
			else
			{
			$show=false;
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
			$html.='个人会员请<a href="wap_login.php">[登录]</a>后查看联系方式<br />没有帐号？<a href="wap_user_reg.php">[免费注册]</a>';
			$html.='</div><div class="telimg"></div>';
			}
		}
		elseif($_CFG['showjobcontact']=='2')
		{
			if ($_SESSION['uid'] && $_SESSION['username'] && $_SESSION['utype']=='2')
			{
				$val=$db->getone("select uid from ".table('resume')." where uid='{$_SESSION['uid']}' LIMIT 1");
			 	if (!empty($val))
				{
				$show=true;
				}
				else
				{
				$show=false;
				$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
				$html.="您没有发布简历或者简历无效，发布简历后才可以查看联系方式。<a href=\"".get_member_url($_SESSION['utype'],true)."personal_resume.php?act=resume_list\">[查看我的简历]</a>";
				$html.='</div><div class="telimg"></div>';
				}
			}
			else
			{
			$show=false;
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
			$html.='个人会员请<a href="wap_login.php">[登录]</a>后查看联系方式<br />没有帐号？<a href="wap_user_reg.php">[免费注册]</a>';
			$html.='</div><div class="telimg"></div>';
			}
		}
		if ($show)
		{
		$sql = "select contact,contact_show,telephone,telephone_show,email,email_show,address,address_show,website FROM ".table('company_profile')." where id='{$id}' LIMIT 1";
		$val=$db->getone($sql);
			if ($_CFG['contact_img_com']=='2')
			{
			$token=md5($val['contact'].$id.$val['telephone']);
			$contact=$val['contact_show']=='1'?"联系人：<img src=\"{$_CFG['site_dir']}plus/contact_img.php?act=company_contact&type=1&id={$id}&token={$token}\"  border=\"0\" align=\"absmiddle\"/><br />":"联系人：企业设置不对外公开<br />";
			$telephone=$val['telephone_show']=='1'?"联系电话：<img src=\"{$_CFG['site_dir']}plus/contact_img.php?act=company_contact&type=2&id={$id}&token={$token}\"  border=\"0\" align=\"absmiddle\"/><br />":"联系电话：企业设置不对外公开<br />";
			$address=$val['address_show']=='1'?"联系地址：<img src=\"{$_CFG['site_dir']}plus/contact_img.php?act=company_contact&type=4&id={$id}&token={$token}\"  border=\"0\" align=\"absmiddle\"/><br />":"联系地址：企业设置不对外公开<br />";
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
			$html.=$contact.$telephone.$address;
			$html.='</div><div class="telimg"><a href="wtai://wp/mc;'.$val["telephone"].'"><img src="images/23.jpg"  border="0"/></a></div>';
			}
			else
			{
			$contact=$val['contact_show']=='1'?"联系人：{$val['contact']}<br />":"联系人：企业设置不对外公开<br />";
			$telephone=$val['telephone_show']=='1'?"联系电话：{$val['telephone']}<br />":"联系电话：企业设置不对外公开<br />";
			$address=$val['address_show']=='1'?"联系地址：{$val['address']}<br />":"联系地址：企业设置不对外公开<br />";
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
			$html.=$contact.$telephone.$address;
			$html.='</div><div class="telimg"><a href="wtai://wp/mc;'.$val["telephone"].'"><img src="images/23.jpg"  border="0"/></a></div>';
			}
			exit($html);
		}
		else
		{		
		exit($html);
		}
	}	
}
//简历联系方式
elseif($act == 'resume_contact')
{   
	$id=intval($_GET['id']);
		$show=false;
		if($_CFG['showresumecontact']=='0')
		{
		$show=true;
		}
		elseif($_CFG['showresumecontact']=='1')
		{
			if ($_SESSION['uid'] && $_SESSION['username'] && $_SESSION['utype']=='1')
			{
			$show=true;
			}
			else
			{
			$show=false;
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
			$html.='企业会员请<a href="wap_login.php">[登录]</a>后查看联系方式<br />没有帐号？<a href="wap_user_reg.php">[免费注册]</a>';
			$html.='</div><div class="telimg"></div>';
			}
		}
		elseif($_CFG['showresumecontact']=='2')
		{
			if ($_SESSION['uid'] && $_SESSION['username'] && $_SESSION['utype']=='1')
			{
				$sql = "select did from ".table('company_down_resume')." WHERE company_uid = {$_SESSION['uid']} AND resume_id='{$id}' LIMIT 1";
				$info=$db->getone($sql);
			 	if (!empty($info))
				{
				$show=true;
				}
				else
				{
				$show=false;
				$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
				$html="<div align=\"center\"><img src=\"{$_CFG['site_template']}images/44.gif\"  border=\"0\" id=\"download\"/></div>";
				$html.="<div align=\"center\"><span class=\"add_resume_pool\">[添加到人才库]</span><br/><br/></div>";
				$html.='</div><div class="telimg"></div>';
				}
			}
			else
			{
			$show=false;
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
			$html.='企业会员请<a href="wap_login.php">[登录]</a>后查看联系方式<br />没有帐号？<a href="wap_user_reg.php">[免费注册]</a>';
			$html.='</div><div class="telimg"></div>';
			}
		}
		if ($show)
		{
			$tb1=$db->getone("select fullname,telephone,email,qq,address,website from ".table('resume')." WHERE  id='{$id}'  LIMIT 1");
			$tb2=$db->getone("select fullname,telephone,email,qq,address,website from ".table('resume_tmp')." WHERE  id='{$id}'  LIMIT 1");		
			$val=!empty($tb1)?$tb1:$tb2;
			if ($_CFG['contact_img_resume']=='2')
			{
			$token=md5($val['fullname'].$id.$val['telephone']);
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
			$html.="联 系 人：<img src=\"{$_CFG['site_dir']}plus/contact_img.php?act=resume_contact&type=1&id={$id}&token={$token}\"  border=\"0\" align=\"absmiddle\"/><br />";
			$html.="联系电话：<img src=\"{$_CFG['site_dir']}plus/contact_img.php?act=resume_contact&type=2&id={$id}&token={$token}\"  border=\"0\" align=\"absmiddle\"/><br />";
			$html.="联系地址：<img src=\"{$_CFG['site_dir']}plus/contact_img.php?act=resume_contact&type=5&id={$id}&token={$token}\"  border=\"0\" align=\"absmiddle\"/><br />";
			$html.="<div align=\"center\"><br/><img src=\"{$_CFG['site_template']}images/64.gif\"  border=\"0\" id=\"invited\"/></div>";
			$html.="<div align=\"center\"><span class=\"add_resume_pool\">[添加到人才库]</span><br/><br/></div>";
			$html.='</div><div class="telimg"><a href="wtai://wp/mc;'.$val["telephone"].'"><img src="images/23.jpg"  border="0"/></a></div>';
			}
			else
			{
			$html='<div class="title"><h2>联系方式</h2></div><div class="txt">';
			$html.="联 系 人：".$val['fullname']."<br />";
			$html.="联系电话：".$val['telephone']."<br />";
			$html.="联系地址：".$val['address']."<br />";
			$html.="<div align=\"center\"><br/><img src=\"{$_CFG['site_template']}images/64.gif\"  border=\"0\" id=\"invited\"/></div>";
			$html.="<div align=\"center\"><span class=\"add_resume_pool\">[添加到人才库]</span><br/><br/></div>";
			$html.='</div><div class="telimg"><a href="wtai://wp/mc;'.$val["telephone"].'"><img src="images/23.jpg"  border="0"/></a></div>';
			}
			exit($html);
		exit($html);
		}
		else
		{		
		exit($html);
		}
}
//获取职位或者简历的所属UID
function get_uid($aid,$type='jobs')
{
    global $db;
	if($type=='resume')
	{
	    $table=table('resume');
	}
	else
	{
	    $table=table('jobs');
	}
	$row=$db->getone("Select uid From {$table} Where id={$aid} LIMIT 1");
	return $row['uid'];
}
?>