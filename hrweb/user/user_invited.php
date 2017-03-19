<?php
 /*
 * 74cms 邀请面试
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
$act = isset($_REQUEST['act']) ? trim($_REQUEST['act']) : 'invited';
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
					必须是企业会员才可以邀请面试！
				</td>
		    </tr>
		</table>');
}
require_once(QISHI_ROOT_PATH.'include/fun_company.php');
$user=get_user_info($_SESSION['uid']);
if ($user['status']=="2") 
{
	exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
		    <tr>
				<td width="20" align="right"></td>
				<td class="ajax_app">
					您的账号处于暂停状态，请联系管理员设为正常后进行操作！
				</td>
		    </tr>
		</table>');
}
$id=isset($_GET['id'])?intval($_GET['id']):exit("err");
$user_jobs=get_auditjobs($_SESSION['uid']);
if (count($user_jobs)==0)
{
	exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
		    <tr>
				<td width="20" align="right"></td>
				<td class="ajax_app">
					邀请失败，你没有发布招聘信息或者信息没有审核通过！
				</td>
		    </tr>
		</table>');
}
$setmeal=get_user_setmeal($_SESSION['uid']);
$resume=$db->getone("select * from ".table('resume')." WHERE id ='{$id}'  LIMIT 1");

if ($_CFG['operation_mode']=="3")
{
 	if ($_CFG['setmeal_to_points']=="1")
	{
		if (empty($setmeal) || ($setmeal['endtime']<time() && $setmeal['endtime']<>"0"))
		{
		$_CFG['operation_mode']="1";
		}
		else
		{
		$_CFG['operation_mode']="2";
		}
	}else{
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
}
if ($act=="invited")
{			
	if ($_CFG['operation_mode']=="2")
	{
	}
	elseif($_CFG['operation_mode']=="1")
	{
				$mypoints=get_user_points($_SESSION['uid']);
				$points_rule=get_cache('points_rule');
				$points=$resume['talent']=='2'?$points_rule['interview_invite_advanced']['value']:$points_rule['interview_invite']['value'];
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
						exit("<table width='100%' border='0' cellspacing='0' cellpadding='0' class='tableall'>
						    <tr>
								<td width='20' align='right'></td>
								<td class='ajax_app'>
									你的{$_CFG['points_byname']}不足，请充值后下载。".$str."
								</td>
						    </tr>
						</table>");
					}
				}
				$tip="邀请面试将扣除<span> {$points} </span>{$_CFG['points_quantifier']}{$_CFG['points_byname']}，您目前共有<span> {$mypoints}</span>{$_CFG['points_quantifier']}{$_CFG['points_byname']}";
	}
	/* 检测是否下载过改简历 或者是申请过 职位 */
	$row = $db->getone("select * from ".table('company_down_resume')." where company_uid={$_SESSION['uid']} and resume_id = ".intval($_GET['id'])." limit 1");
	$row_apply=$db->getone("select * from ".table('personal_jobs_apply')." where company_uid={$_SESSION['uid']} and resume_id = ".intval($_GET['id'])." limit 1");
	if(!$row && !$row_apply)
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
	<tr style='width:100%; margin:0px auto;'>
			<div style='margin:0px auto;'><h3>必须下载完成简历后 才可以邀请面试</h3></div>
    </tr>

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
    <tbody><tr>
	    <td class="ajax_download_tip" style="padding-left: 10px;"><?php echo $tip?>
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
	die;
	}
?>
<!-- 邀请面试弹出 -->
<div class="interview-dialog dialog-block">
	<div class="dialog-item clearfix">
		<div class="d-type f-left">邀请简历：</div>
		<div class="d-content f-left"><?php echo $resume['fullname']; ?></div>
	</div>
	<div class="dialog-item clearfix">
		<div class="d-type f-left">面试职位：</div>
		<div class="data-filter d-content f-left">
			<div class="dropdown">
				<span class="dropdown-ctrl"><span class="jobtit">选择职位</span><i class="drop-icon"></i></span>
				<ul class="dropdown-list">
					<?php 
					foreach ($user_jobs as $list)
					{
					?>
					<li><a href="javascript:;"id="<?php echo $list['id']?>" title="<?php echo $list['jobs_name']; ?>" class='dropdown-list-a'><?php echo $list['jobs_name']?></a></li>
					<?php
					}
					?>
				</ul>
			</div>
            <input type="hidden" name="jobsid" value="" id="jobsid">
		</div>
	</div>
	<div class="dialog-item clearfix">
		<div class="d-type f-left">面试日期：</div>
		<div class="d-content f-left">
			<span class="datepicker" style="padding: 0;"><input type="text" name="interview_time" id="interview_time" class="date-picker"/></span>
		</div>
	</div>
	<div class="dialog-item clearfix">
		<div class="d-type f-left">通知内容：</div>
		<div class="d-content f-left">
			<textarea name="notes" id="notes" cols="30" rows="10" class="dialog-textarea"></textarea>
		</div>
	</div>
	<div class="dialog-item clearfix">
		<div class="d-type f-left">站内通知：</div>
		<div class="d-content f-left">
			<div class="f-left"><label><input type="checkbox" name="pms_notice" id="pms_notice" value="1"  checked="checked"/>
		  站内信通知</label></div>
		</div>
	</div>
	<div class="dialog-item clearfix">
		<div class="d-type f-left">短信通知：</div>
		<div class="d-content f-left">
			<div class="f-left"><label><input  type="checkbox" name="sms_notice" id="sms_notice" class="checkbox" />是</label></div>
		</div>
	</div>

	<div class="dialog-item clearfix">
		<div class="d-type f-left">&nbsp;</div>
		<div class="d-content f-left">
			<input type="button" value="发送" class="btn-65-30blue btn-big-font DialogSubmit"/><input type="button" value="取消" class="btn-65-30grey btn-big-font DialogClose" />
		</div>
	</div>
</div>
<script type="text/javascript">
$('input.date-picker').datePicker();
/* 职位选择 */
$(".dropdown-ctrl").on('click',function() {
	$('.dropdown-list').slideToggle('fast');
});
$('.dropdown-list a').on('click',function() {
	var id = $(this).attr("id");
	var title = $(this).attr("title");
	$("#jobsid").val(id);
	$(".jobtit").html(title);
	$('.dropdown-list').slideUp('fast');
});
</script>
<?php
}
elseif ($act=="invited_save")
{
	$jobs_id=isset($_GET['jobs_id'])?intval($_GET['jobs_id']):exit("err");
	$notes=isset($_GET['notes'])?trim($_GET['notes']):"";
	$pms_notice=intval($_GET['pms_notice']);
	$sms_notice=intval($_GET['sms_notice']);
	$interview_time=trim($_GET['interview_time']);
	if (check_interview($id,$jobs_id,$_SESSION['uid']))
	{
	exit("您已对该简历经行过面试邀请,不能重复邀请！");
	}
	$jobs=get_jobs_one($jobs_id);
	$addarr['resume_id']=$resume['id'];
	$addarr['resume_addtime']=$resume['addtime'];
	if ($resume['display_name']=="2")
	{
	$addarr['resume_name']="N".str_pad($resume['id'],7,"0",STR_PAD_LEFT);	
	}
	elseif ($resume['display_name']=="3")
	{
		if($resume['sex']==1)
		{
			$addarr['resume_name']=cut_str($resume['fullname'],1,0,"先生");	
		}
		elseif($resume['sex']==2)
		{
			$addarr['resume_name']=cut_str($resume['fullname'],1,0,"女士");
		}
	}
	else
	{
	$addarr['resume_name']=$resume['fullname'];
	}
	$addarr['resume_uid']=$resume['uid'];
	$addarr['company_id']=$jobs['company_id'];
	$addarr['company_addtime']=$jobs['company_addtime'];
	$addarr['company_name']=$jobs['companyname'];
	$addarr['company_uid']=$_SESSION['uid'];
	$addarr['jobs_id']=$jobs['id'];
	$addarr['jobs_name']=$jobs['jobs_name'];
	$addarr['jobs_addtime']=$jobs['addtime'];	
	$addarr['notes']= $notes;
	$addarr['interview_addtime']= time();
	$addarr['interview_time']= $interview_time;
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
		$addarr['notes']=iconv("utf-8", "gbk",$addarr['notes']);
	}
	$addarr['personal_look']= 1;
	$resume_user=get_user_info($resume['uid']);
	if ($_CFG['operation_mode']=="2")
	{
		$db->inserttable(table('company_interview'),$addarr);
		// 邀请面试 不做 套餐限制
		write_memberslog($_SESSION['uid'],1,6001,$_SESSION['username'],"邀请了 {$resume_user['username']} 面试");
	}		 
	elseif($_CFG['operation_mode']=="1")
	{
		$mypoints=get_user_points($_SESSION['uid']);
		$points_rule=get_cache('points_rule');
		$points=$resume['talent']=='2'?$points_rule['interview_invite_advanced']['value']:$points_rule['interview_invite']['value'];
		$ptype=$resumeshow['talent']=='2'?$points_rule['interview_invite_advanced']['type']:$points_rule['interview_invite']['type'];
		if  ($mypoints<$points)
		{
			exit("您的积分不足,不能经行邀请面试！");
		}
		$db->inserttable(table('company_interview'),$addarr);
		if ($points>0)
		{
			report_deal($_SESSION['uid'],$ptype,$points);
			$user_points=get_user_points($_SESSION['uid']);
			$operator=$ptype=="1"?"+":"-";
			if($resume['talent']=='2'){
				write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"邀请 {$resume_user['username']} 面试({$operator}{$points}),(剩余:{$user_points})",1,1007,"邀请高级人才面试","{$operator}{$points}","{$user_points}");
			}else{
				write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"邀请 {$resume_user['username']} 面试({$operator}{$points}),(剩余:{$user_points})",1,1006,"邀请普通人才面试","{$operator}{$points}","{$user_points}");
			}
			write_memberslog($_SESSION['uid'],1,6001,$_SESSION['username'],"邀请 {$resume_user['username']} 面试");
		}		
	}
	/*
		发送短信提示 操作 
	*/
	$sms=get_cache('sms_config');
	if($sms['open']=="1" && $sms['set_invite']=="1" && $sms_notice=="1")
	{
		$user=get_user_info($_SESSION['uid']);
		$success = dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_sms.php?uid={$_SESSION['uid']}&key=".asyn_userkey($_SESSION['uid'])."&act=set_invite&companyname={$jobs['companyname']}&mobile={$resume_user['mobile']}");	
	}
	//站内信
	if($pms_notice=='1'){
		$jobs_url=url_rewrite('QS_jobsshow',array('id'=>$jobs['id']));
		$company_url=url_rewrite('QS_companyshow',array('id'=>$jobs['company_id']),false);
		$message=$jobs['companyname']."邀请您参加公司面试，面试职位：<a href=\"{$jobs_url}\" target=\"_blank\"> {$jobs['jobs_name']} </a>，<a href=\"{$company_url}\" target=\"_blank\">点击查看公司详情</a>";
		write_pmsnotice($resume['uid'],$resume_user['username'],$message);
	}
	$html='<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall" id="invited_ok">
	    <tr>
			<td width="140" align="right"><img height="100" src="'.$_CFG['site_template'].'images/big-yes.png" /></td>
			<td>
				<strong style="font-size:14px ; color:#0066CC;margin-left:20px">邀请成功!</strong>
				<div style="border-top:1px #CCCCCC solid; line-height:180%; margin-top:10px; padding-top:10px; height:50px;margin-left:20px"  class="dialog_closed">
				<a href="'.get_member_url(1,true).'company_recruitment.php?act=interview_list" style="color:#0180cf;text-decoration:none;" class="underline">查看我发起的面试邀请</a><br />
				<a href="javascript:void(0)"  class="DialogClose underline" style="color:#0180cf;text-decoration:none;">邀请完成</a>
				</div>
			</td>
	    </tr>
	</table>';
	exit($html);
}
?>