<?php
 /*
 * 74cms 下载简历
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
$act = isset($_REQUEST['act']) ? trim($_REQUEST['act']) : 'download';
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
if((empty($_SESSION['uid']) || empty($_SESSION['username']) || empty($_SESSION['utype'])) &&  $_COOKIE['QS']['username'] && $_COOKIE['QS']['password'] && $_COOKIE['QS']['uid'])
{
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	if(check_cookie($_COOKIE['QS']['uid'],$_COOKIE['QS']['username'],$_COOKIE['QS']['password']))
	{
	update_user_info($_COOKIE['QS']['uid'],false,false);
	header("Location:".get_member_url($_SESSION['utype']));
	}
	else
	{
	unset($_SESSION['uid'],$_SESSION['username'],$_SESSION['utype'],$_SESSION['uqqid'],$_SESSION['activate_username'],$_SESSION['activate_email'],$_SESSION["openid"]);
	setcookie("QS[uid]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
	setcookie('QS[username]',"", time() - 3600,$QS_cookiepath, $QS_cookiedomain);
	setcookie('QS[password]',"", time() - 3600,$QS_cookiepath, $QS_cookiedomain);
	setcookie("QS[utype]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
	}
}
if ($_SESSION['uid']=='' || $_SESSION['username']=='')
{
	$captcha=get_cache('captcha');
	$smarty->assign('verify_userlogin',$captcha['verify_userlogin']);
	$smarty->display('plus/ajax_login.htm');
	exit();
}
if ($_SESSION['utype']!='1')
{
	exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
		    <tr>
				<td width="20" align="right"></td>
				<td class="ajax_app">
					必须是企业才可以下载简历！
				</td>
		    </tr>
		</table>');
}
$id=!empty($_GET['id'])?intval($_GET['id']):exit("出错了");
$resumeshow=get_resume_basic_one($id);
require_once(QISHI_ROOT_PATH.'include/fun_company.php');
$user=get_user_info($_SESSION['uid']);
$downresumeurl="<a href=\"".get_member_url(1,true)."company_recruitment.php?act=down_resume_list&talent=2\">[查看已下载的高级简历]</a>";
if ($user['status']=='2'){
	exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
		    <tr>
				<td width="20" align="right"></td>
				<td class="ajax_app">
					您的账号处于暂停状态，请联系管理员设为正常后进行操作！
				</td>
		    </tr>
		</table>');
}
if(check_jobs_apply($id,$_SESSION['uid']))
{
	if($_CFG['showapplycontact']==1)
	{
		exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
			<tr>
				<td width="20" align="right"></td>
				<td class="ajax_app">
					简历联系方式可见，您无需下载此简历！
				</td>
			</tr>
		</table>');
	}
}
if (check_down_resumeid($id,$_SESSION['uid'])) 
{
	$str="<a href=\"".get_member_url(1,true)."company_recruitment.php?act=down_resume_list\">[查看我的下载的简历]</a>";
	exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
			<tr>
				<td width="20" align="right"></td>
				<td class="ajax_app">
					您已经下载过此简历了！'.$str.'
				</td>
			</tr>
		</table>');
}
if ($_CFG['down_resume_limit']=="1")
{
	$user_jobs=get_auditjobs($_SESSION['uid']);//审核通过的职位
	$strurl="你没有发布职位或审核未通过导致无法下载简历。<a href=\"".get_member_url(1,true)."company_jobs.php?act=jobs\">[职位管理]</a>";
	if (empty($user_jobs))
	{
		exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
			    <tr>
					<td width="20" align="right"></td>
					<td class="ajax_app">
						'.$strurl.'
					</td>
			    </tr>
			</table>');
	}
}
if ($resumeshow['display_name']=="2")
{
$resumeshow['resume_name']="N".str_pad($resumeshow['id'],7,"0",STR_PAD_LEFT);	
}
elseif ($resumeshow['display_name']=="3")
{
	if($resumeshow['sex']==1)
	{
		$resumeshow['resume_name']=cut_str($resumeshow['fullname'],1,0,"先生");
	}
	elseif($resumeshow['sex']==2)
	{
		$resumeshow['resume_name']=cut_str($resumeshow['fullname'],1,0,"女士");
	}
}
else
{
$resumeshow['resume_name']=$resumeshow['fullname'];
}
$setmeal=get_user_setmeal($_SESSION['uid']);
if ($_CFG['operation_mode']=="3")
{
	if ($_CFG['setmeal_to_points']=="1")
	{
		if (empty($setmeal) || ($setmeal['endtime']<time() && $setmeal['endtime']<>"0"))
		{
		$_CFG['operation_mode']="1";
		}
		elseif ($resumeshow['talent']=='2' && $setmeal['download_resume_senior']<=0)
		{
		$_CFG['operation_mode']="1";
		}
		elseif (($resumeshow['talent']=='1' || $resumeshow['talent']=='3' ) && $setmeal['download_resume_ordinary']<=0)
		{
		$_CFG['operation_mode']="1";
		}
		else
		{
		$_CFG['operation_mode']="2";
		}
	}
	else
	{
	$_CFG['operation_mode']="2";
	}
}
if ($_CFG['operation_mode']=="2")
{
	if (empty($setmeal) || ($setmeal['endtime']<time() && $setmeal['endtime']<>"0"))
	{
		$str="<a href=\"".get_member_url(1,true)."company_service.php?act=setmeal_list\">[申请服务]</a>";
		exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
			    <tr>
					<td width="20" align="right"></td>
					<td class="ajax_app">
						您的服务已到期。您可以 '.$str.'
					</td>
			    </tr>
			</table>');
	}
	elseif ($resumeshow['talent']=='2' && $setmeal['download_resume_senior']<=0)
	{
		$str="<a href=\"".get_member_url(1,true)."company_service.php?act=setmeal_list\">[申请服务]</a>";
		exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
			    <tr>
					<td width="20" align="right"></td>
					<td class="ajax_app">
						你下载高级人才简历数量已经超出了限制。您可以 '.$str.'
					</td>
			    </tr>
			</table>');
	}
	elseif ($resumeshow['talent']=='1' && $setmeal['download_resume_ordinary']<=0)
	{
		$str="<a href=\"".get_member_url(1,true)."company_service.php?act=setmeal_list\">[申请服务]</a>";
		exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
			    <tr>
					<td width="20" align="right"></td>
					<td class="ajax_app">
						你下载简历数量已经超出了限制。您可以 '.$str.'
					</td>
			    </tr>
			</table>');
	}
}
if ($act=="download")
{
	if ($_CFG['operation_mode']=="2")
	{
		if ($resumeshow['talent']=='2')
		{	
			$tip="提示：您还可以下载<span> {$setmeal['download_resume_senior']}</span>份高级人才简历";
		}
		else
		{	
			$tip="提示：您还可以下载<span> {$setmeal['download_resume_ordinary']}</span>份普通人才简历";
		}
		
	}
	elseif($_CFG['operation_mode']=="1")
	{
		$points_rule=get_cache('points_rule');
		$points=$resumeshow['talent']=='2'?$points_rule['resume_download_advanced']['value']:$points_rule['resume_download']['value'];
		$mypoints=get_user_points($_SESSION['uid']);
		if  ($mypoints<$points)
		{
			$str="<a href=\"".get_member_url(1,true)."company_service.php?act=order_add\">[充值{$_CFG['points_byname']}]</a>&nbsp;&nbsp;&nbsp;&nbsp;";
			$str1="<a href=\"".get_member_url(1,true)."company_service.php?act=setmeal_list\">[申请服务]</a>";
			if (!empty($setmeal) && $_CFG['setmeal_to_points']=="1")
			{
				exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
				    <tr>
						<td width="20" align="right"></td>
						<td class="ajax_app">
							你的服务已到期或超出服务条数。您可以 '.$str.$str1.'
						</td>
				    </tr>
				</table>');
			}
			else
			{
				exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
				    <tr>
						<td width="20" align="right"></td>
						<td class="ajax_app">
							你的'.$_CFG['points_byname'].' 不足，请充值后下载。'.$str.'
						</td>
				    </tr>
				</table>');
			}			
		}
		$tip="下载此份简历将扣除<span> {$points}</span>{$_CFG['points_quantifier']}{$_CFG['points_byname']}，您目前共有<span> {$mypoints}</span>{$_CFG['points_quantifier']}{$_CFG['points_byname']}";
	}
?>
<script type="text/javascript">
$(".but100").hover(function(){$(this).addClass("but100_hover")},function(){$(this).removeClass("but100_hover")});
$("#ajax_download_r").click(function() {
		var id="<?php echo $id?>";
		var tsTimeStamp= new Date().getTime();
			$("#ajax_download_r").val("处理中...");
			$("#ajax_download_r").attr("disabled","disabled");
 			 var pms_notice=$("#pms_notice").attr("checked");
			 if(pms_notice) pms_notice=1;else pms_notice=0;
		$.get("<?php echo $_CFG['site_dir'] ?>user/user_download_resume.php", { "id":id,"pms_notice":pms_notice,"time":tsTimeStamp,"act":"download_save"},
 	 	function (data,textStatus)
	 	 {
			if (data=="ok")
			{
				$(".ajax_download_tip").hide();
				$("#ajax_download_table").hide();
				$("#notice").hide();
				$("#download_ok").show();
				//刷新联系地址
				$.get("<?php echo $_CFG['site_dir'] ?>plus/ajax_contact.php", { "id": id,"time":tsTimeStamp,"act":"resume_contact"},
					function (data,textStatus)
					{			
					$("#resume_contact").html(data);
					}
				);
			}
			else
			{
				alert(data);
			}
				$("#ajax_download_r").val("下载简历");
				$("#ajax_download_r").attr("disabled","");
	 	 })
});
</script>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall" id="ajax_download_table">
    <tr>
		<td width="120" align="right">站内信通知对方：</td>
		<td class="ajax_app">
			<label><input type="checkbox" name="pms_notice" id="pms_notice" value="1"  checked="checked"/>
		  站内信通知
		   </label>
		</td>
    </tr>
    <tr>
		<td></td>
		<td>
			<input type="button" name="Submit"  id="ajax_download_r" class="but130lan" value="下载简历" />
		</td>
    </tr>
</table>
 
<table id="notice" width="100%" border="0" style="border-top:1px #CCCCCC dotted;background-color: #EEEEEE; line-height: 230%;padding: 15px; margin-top: 10px; ">
    <tbody>
    	<tr>
    		<td class="dialog_bottom">
		    	<div class="dialog_tip"></div><div class="dialog_text ajax_download_tip"><?php echo $tip?></div>
		    	<div class="clear"></div>
		    </td>
    	</tr>
	</tbody>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall" id="download_ok" style="display:none">
    <tr>
		<td width="140" align="right"><img height="100" src="<?php echo  $_CFG['site_template']?>images/big-yes.png" /></td>
		<td>
			<strong style="font-size:14px ; color:#0066CC;margin-left:20px">下载成功!</strong>
			<?php
				if($_SESSION['utype']==1){
			?>
			<div style="border-top:1px #CCCCCC solid; line-height:180%; margin-top:10px; padding-top:10px; height:50px;margin-left:20px"  class="dialog_closed">
			<a href="<?php echo get_member_url(1,true)?>company_recruitment.php?act=down_resume_list" style="color:#0180cf;text-decoration:none;" class="underline">查看已下载简历</a><br />
			<?php
				}else{
			?>
			<div style="border-top:1px #CCCCCC solid; line-height:180%; margin-top:10px; padding-top:10px; height:50px;margin-left:20px"  class="dialog_closed">
			<?php echo $downresumeurl;?><br />
			<?php
				}
			?>
			<a href="javascript:void(0)"  class="DialogClose underline" style="color:#0180cf;text-decoration:none;">下载完成</a>
			</div>
		</td>
    </tr>
</table>
<?php
}
elseif ($act=="download_save")
{
	$ruser=get_user_info($resumeshow['uid']);
	$pms_notice=intval($_GET['pms_notice']);
	if ($_CFG['operation_mode']=="2")
	{	
			if ($resumeshow['talent']=='2')
			{
					if ($setmeal['download_resume_senior']>0 && add_down_resume($id,$_SESSION['uid'],$resumeshow['uid'],$resumeshow['resume_name']))
					{
					action_user_setmeal($_SESSION['uid'],"download_resume_senior");
					$setmeal=get_user_setmeal($_SESSION['uid']);
					write_memberslog($_SESSION['uid'],1,9002,$_SESSION['username'],"下载了 {$ruser['username']} 发布的高级简历,还可以下载 {$setmeal['download_resume_senior']} 份高级简历",2,1005,"下载高级简历","1","{$setmeal['download_resume_senior']}");
					write_memberslog($_SESSION['uid'],1,4001,$_SESSION['username'],"下载了 {$ruser['username']} 发布的简历");
					//站内信
					if($pms_notice=='1'){
						$company=$db->getone("select id,companyname  from ".table('company_profile')." where uid ={$_SESSION['uid']} limit 1");
						// $user=$db->getone("select username from ".table('members')." where uid ={$resumeshow['uid']} limit 1");
						$resume_url=url_rewrite('QS_resumeshow',array('id'=>$id));
						$company_url=url_rewrite('QS_companyshow',array('id'=>$company['id']));
						$message=$_SESSION['username']."下载了您发布的简历：<a href=\"{$resume_url}\" target=\"_blank\">{$resumeshow['resume_name']}</a>，<a href=\"$company_url\" target=\"_blank\">点击查看公司详情</a>";
						write_pmsnotice($resumeshow['uid'],$ruser['username'],$message);
					}
					exit("ok");
					}
			}
			else
			{
					if ($setmeal['download_resume_ordinary']>0 && add_down_resume($id,$_SESSION['uid'],$resumeshow['uid'],$resumeshow['resume_name']))
					{		
					action_user_setmeal($_SESSION['uid'],"download_resume_ordinary");
					$setmeal=get_user_setmeal($_SESSION['uid']);
					write_memberslog($_SESSION['uid'],1,9002,$_SESSION['username'],"下载了 {$ruser['username']} 发布的普通简历,还可以下载 {$setmeal['download_resume_ordinary']} 份普通简历",2,1004,"下载普通简历","1","{$setmeal['download_resume_ordinary']}");
					write_memberslog($_SESSION['uid'],1,4001,$_SESSION['username'],"下载了 {$ruser['username']} 发布的简历");
					//站内信
					if($pms_notice=='1'){
						$company=$db->getone("select id,companyname  from ".table('company_profile')." where uid ={$_SESSION['uid']} limit 1");
						// $user=$db->getone("select username from ".table('members')." where uid ={$resumeshow['uid']} limit 1");
						$resume_url=url_rewrite('QS_resumeshow',array('id'=>$id));
						$company_url=url_rewrite('QS_companyshow',array('id'=>$company['id']));
						$message=$_SESSION['username']."下载了您发布的简历：<a href=\"{$resume_url}\" target=\"_blank\">{$resumeshow['resume_name']}</a>，<a href=\"$company_url\" target=\"_blank\">点击查看公司详情</a>";
						write_pmsnotice($resumeshow['uid'],$ruser['username'],$message);
					}
					exit("ok");
					}
			}

	}
	elseif($_CFG['operation_mode']=="1")
	{
				$points_rule=get_cache('points_rule');
				$points=$resumeshow['talent']=='2'?$points_rule['resume_download_advanced']['value']:$points_rule['resume_download']['value'];
				$ptype=$resumeshow['talent']=='2'?$points_rule['resume_download_advanced']['type']:$points_rule['resume_download']['type'];
				$mypoints=get_user_points($_SESSION['uid']);
				if  ($mypoints<$points)
				{
					exit("err");
				}
				if (add_down_resume($id,$_SESSION['uid'],$resumeshow['uid'],$resumeshow['resume_name']))
				{
					if ($points>0)
					{
					report_deal($_SESSION['uid'],$ptype,$points);
					$user_points=get_user_points($_SESSION['uid']);
					$operator=$ptype=="1"?"+":"-";
					if($resumeshow['talent']=='2'){
						write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"下载了 {$ruser['username']} 发布的简历({$operator}{$points}),(剩余:{$user_points})",1,1005,"下载高级简历","{$operator}{$points}","{$user_points}");
					}elseif($resumeshow['talent']=='1'){
						write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"下载了 {$ruser['username']} 发布的简历({$operator}{$points}),(剩余:{$user_points})",1,1004,"下载普通简历","{$operator}{$points}","{$user_points}");
					}
					write_memberslog($_SESSION['uid'],1,4001,$_SESSION['username'],"下载了 {$ruser['username']} 发布的简历");
					//站内信
					if($pms_notice=='1'){
						$company=$db->getone("select id,companyname  from ".table('company_profile')." where uid ={$_SESSION['uid']} limit 1");
						// $user=$db->getone("select username from ".table('members')." where uid ={$resumeshow['uid']} limit 1");
						$resume_url=url_rewrite('QS_resumeshow',array('id'=>$id));
						$company_url=url_rewrite('QS_companyshow',array('id'=>$company['id']));
						$message=$_SESSION['username']."下载了您发布的简历：<a href=\"{$resume_url}\" target=\"_blank\">{$resumeshow['resume_name']}</a>，<a href=\"$company_url\" target=\"_blank\">点击查看公司详情</a>";
						write_pmsnotice($resumeshow['uid'],$ruser['username'],$message);
					}

					}
					exit("ok");
				}
	}	
}
function get_resume_basic_one($id)
{
	global $db;
	$id=intval($id);
	$info=$db->getone("select * from ".table('resume')." where id='{$id}' LIMIT 1 ");
	
	if (empty($info))
	{
	return false;
	}
	else
	{
	$info['age']=date("Y")-$info['birthdate'];
	$info['number']="N".str_pad($info['id'],7,"0",STR_PAD_LEFT);
	$info['lastname']=cut_str($info['fullname'],1,0,"**");
	return $info;
	}
}
function check_jobs_apply($resume_id,$c_uid)
{
	global $db;
	$sql = "select did from ".table('personal_jobs_apply')." WHERE company_uid = '".intval($c_uid)."'  AND resume_id='".intval($resume_id)."' LIMIT 1";
	return $db->getone($sql);
}
?>