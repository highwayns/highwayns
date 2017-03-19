<?php
 /*
 * 74cms 举报
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
$act = isset($_REQUEST['act']) ? trim($_REQUEST['act']) : 'app';
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
if ($_SESSION['utype']!='2')
{
	exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
		    <tr>
				<td width="20" align="right"></td>
				<td>
					必须是个人会员才可以举报职位信息！
				</td>
		    </tr>
		</table>');
}
require_once(QISHI_ROOT_PATH.'include/fun_personal.php');
$user=get_user_info($_SESSION['uid']);
if ($user['status']=="2") 
{
	exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
		    <tr>
				<td width="20" align="right"></td>
				<td>
					您的账号处于暂停状态，请联系管理员设为正常后进行操作！
				</td>
		    </tr>
		</table>');
}
if ($act=="report")
{		
		$id=isset($_GET['jobs_id'])?$_GET['jobs_id']:exit("id 丢失");
		$jobs=app_get_jobs($id);
		if (empty($jobs))
		{
			exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
			    <tr>
					<td width="20" align="right"></td>
					<td>
						举报信息失败！
					</td>
			    </tr>
			</table>');
		}
		if (check_jobs_report($_SESSION['uid'],intval($_GET['jobs_id'])))
		{
			exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
			    <tr>
					<td width="20" align="right"></td>
					<td>
						您已经举报过此职位！
					</td>
			    </tr>
			</table>');
		}
?>
<script type="text/javascript">
$(".but80").hover(function(){$(this).addClass("but80_hover")},function(){$(this).removeClass("but80_hover")});
//计算今天申请数量

//验证
$("#ajax_report").click(function() {
	var content=$("#content").val();
	if (content=="")
	{
	alert("请输入描述");
	}
	else
	{
		$("#report").hide();
		$("#waiting").show();
		
		$.post("<?php echo $_CFG['site_dir'] ?>user/user_report.php", { "jobs_id": $("#jobs_id").val(),"jobs_name": $("#jobs_name").val(),"content": $("#content").val(),"report_type":$('input[name="report_type"]:checked').val(),"jobs_addtime":$("#jobs_addtime").val(),"act":"app_save"},

	 	function (data,textStatus)
	 	 {
			if (data=="ok")
			{
				$("#report").hide();
				$("#waiting").hide();
				$("#app_ok").show();
			}
			else
			{
				$("#report").hide();
				$("#waiting").hide();
				$("#app_ok").hide();
				$("#error_msg").html("举报失败！"+data);
				$("#error").show();
			}
	 	 });
	}
});
</script>
<div class="report-dialog" id="report">
	<input type="hidden" id="jobs_id" value="<?php echo intval($_GET['jobs_id']);?>">
	<input type="hidden" id="jobs_name" value="<?php echo trim($_GET['jobs_name']);?>">
	<input type="hidden" id="jobs_addtime" value="<?php echo trim($_GET['jobs_addtime']);?>">
	<div class="report-item clearfix">
		<div class="report-type f-left">举报原因：</div>
		<div class="report-content f-left">
			<label><input type="radio" name="report_type"  class="radio" value="1" checked="checked"/>信息虚假<span>（乱写、乱填等无意义内容）</span></label>
			<label><input type="radio" name="report_type"  class="radio" value="2" />电话不通<span>（电话多次未通）</span></label>
			<label><input type="radio" name="report_type"  class="radio" value="3" />其它原因<span>（如中介等）</span></label>
		</div>
	</div>
	<div class="report-item clearfix">
		<div class="report-type f-left">相关描述：</div>
		<div class="report-content f-left">
			<textarea name="content" id="content" cols="30" rows="10"></textarea>
		</div>
	</div>
	<span class="r-all-row">一经核实，我们会立即... </span>
	<div class="report-item clearfix">
		<div class="report-type f-left">&nbsp;</div>
		<div class="report-content f-left">
			<p class="del-info">删除信息，为民除害 </p>
			<p class="del-info">站内信通知您 </p>
		</div>
	</div>
	<div class="center-btn-box">
		<input type="button" value="举报" class="btn-65-30blue btn-big-font " id="ajax_report"/><input type="button" value="取消" class="btn-65-30grey btn-big-font DialogClose" />
	</div>
	<p class="jubao-tip" style="padding-left: 10px;">温馨提示：找份工作不容易，请您如实举报哦！</p>
</div>


<table width="100%" border="0" cellspacing="5" cellpadding="0" id="waiting"  style="display:none">
  <tr>
    <td align="center" height="60"><img src="<?php echo  $_CFG['site_template']?>images/30.gif"  border="0"/></td>
  </tr>
  <tr>
    <td align="center" >请稍后...</td>
  </tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall" id="app_ok" style="display:none">
    <tr>
		<td width="140" align="right"><img height="100" src="<?php echo  $_CFG['site_template']?>images/big-yes.png" /></td>
		<td>
			<strong style="font-size:14px ; color:#0066CC;margin-left:20px">举报成功，管理员会认真处理!</strong>
		</td>
    </tr>
</table>

<table width="100%" border="0" cellspacing="5" cellpadding="0" id="error"  style="display:none">
  <tr>
    <td align="center" id="error_msg"></td>
  </tr>
</table>

<?php
}
elseif ($act=="app_save")
{
	$setsqlarr['content']=trim($_POST['content'])?trim($_POST['content']):exit("出错了");
	$setsqlarr['jobs_id']=$_POST['jobs_id']?intval($_POST['jobs_id']):exit("出错了");
	$setsqlarr['uid']=intval($_SESSION['uid']);
	$setsqlarr['addtime']=time();
	$setsqlarr['report_type']=intval($_POST['report_type']); // 投诉类型
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
	$setsqlarr['content']=utf8_to_gbk($setsqlarr['content']);
	}
	$jobsarr=app_get_jobs($setsqlarr['jobs_id']);
	if (empty($jobsarr))
	{
	exit("职位丢失");
	}
	else
	{
		$setsqlarr['jobs_name']=$jobsarr[0]['jobs_name'];
		$setsqlarr['jobs_addtime']=$jobsarr[0]['addtime'];
		$insert_id = $db->inserttable(table('report'),$setsqlarr,1);
	}
	if($insert_id)
	 {
	 exit("ok");
	 }
}

?>
