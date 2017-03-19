<?php
 /*
 * 74cms 添加到收藏夹
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
$act = isset($_REQUEST['act']) ? trim($_REQUEST['act']) : 'add';
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
			<td class="ajax_app">
				必须是个人会员才可以收藏职位！
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
			<td class="ajax_app">
				您的账号处于暂停状态，请联系管理员设为正常后进行操作！
			</td>
	    </tr>
	</table>');
}
if ($act=="add")
{
	$id=isset($_GET['id'])?trim($_GET['id']):exit("出错了"); 
	if(add_favorites($id,$_SESSION['uid'])==0)
	{
	exit('<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall">
	    <tr>
			<td width="20" align="right"></td>
			<td class="ajax_app">
				添加失败，收藏夹中已经存在此职位
			</td>
	    </tr>
	</table>');
	}
	else
	{
?>
<script type="text/javascript">
$("#add_ok .closed").click(function()
{
DialogClose();
});

function DialogClose()
{
	$("#FloatBg").hide();
	$("#FloatBox").hide();
}
</script>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tableall" id="add_ok">
    <tr>
		<td width="140" align="right"><img height="100" src="<?php echo  $_CFG['site_template']?>images/big-yes.png" /></td>
		<td>
			<strong style="font-size:14px ; color:#0066CC;margin-left:20px">添加成功!</strong>
			<div style="border-top:1px #CCCCCC solid; line-height:180%; margin-top:10px; padding-top:10px; height:50px;margin-left:20px"  class="dialog_closed">
			<a href="<?php echo get_member_url(2,true)?>personal_apply.php?act=favorites" style="color:#0180cf;text-decoration:none;" class="underline">查看职位收藏夹</a><br />
			<a href="javascript:void(0)"  class="DialogClose underline" style="color:#0180cf;text-decoration:none;">添加完成</a>
			</div>
		</td>
    </tr>
</table>
<?php
}
}
?>