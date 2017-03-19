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
				<td>
					必须是企业会员才可以对简历进行标记状态！
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
				<td>
					您的账号处于暂停状态，请联系管理员设为正常后进行操作！
				</td>
		    </tr>
		</table>');
}
$resume_id=$_REQUEST['resume_id']?intval($_REQUEST['resume_id']):exit("简历ID丢失！");
$setarr['resume_state']=$_REQUEST['resume_state']?intval($_REQUEST['resume_state']):exit("标记状态错误！");
$setarr['resume_state_cn']=$_REQUEST['resume_state_cn']?iconv('utf-8', 'gbk',trim($_REQUEST['resume_state_cn'])):exit("标记状态错误！");
$p_uid = $db->getone("SELECT uid FROM ".table('resume')." WHERE id={$resume_id} LIMIT 1 ");
$uid=intval($_SESSION['uid']);
$row=$db->getone("select resume_id from ".table("company_label_resume")." where uid=$uid and resume_id=$resume_id limit 1");
if(empty($row))
{
	$setarr['resume_id']=$resume_id;
	$setarr['uid']=$uid;
	$setarr['personal_uid']=$p_uid['uid'];
	$db->inserttable(table('company_label_resume'),$setarr);
	//将查看状态更新成已经查看
	$db->updatetable(table('personal_jobs_apply'),array('personal_look'=>'2','is_reply'=>$setarr['resume_state']),array("company_uid"=>$uid,"resume_id"=>$resume_id));
}
else
{
	$db->updatetable(table('company_label_resume'),$setarr,array("uid"=>$uid,"resume_id"=>$resume_id));
	//将查看状态更新成已经查看
	$db->updatetable(table('personal_jobs_apply'),array('personal_look'=>'2','is_reply'=>$setarr['resume_state']),array("company_uid"=>$uid,"resume_id"=>$resume_id));
}
exit("ok");
?>
