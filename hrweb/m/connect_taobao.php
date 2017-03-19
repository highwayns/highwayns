<?php
 /*
 * 74cms 淘宝号帐号登录
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
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'login';
$top_parameters=trim($_REQUEST['top_parameters']);
$top_sign=trim($_REQUEST['top_sign']);
if($act == 'login' && empty($top_parameters))
{
	$url="https://oauth.taobao.com/authorize?response_type=user&client_id={$_CFG['taobao_appkey']}&redirect_uri=";
	$url.=urlencode("{$_CFG['wap_domain']}/connect_taobao.php");
	header("Location:{$url}");	
}
elseif($act == 'login' && !empty($top_parameters))
{
	require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
	$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
	unset($dbhost,$dbuser,$dbpass,$dbname);
	require_once(QISHI_ROOT_PATH.'include/tpl.inc.php');
	if (empty($top_sign))
	{
	exit('参数错误！');
	}
	$base64str=base64_encode(md5($top_parameters.$_CFG['taobao_appsecret'],TRUE ));
	if ($base64str<>$top_sign)
	{
	exit('参数非法！');
	}
	else
	{
	$code=base64_decode($top_parameters);
	parse_str($code,$code);
	$token=md5($code['nick'].$code['user_id']);
	}
	if (empty($token))
	{
	exit('登录失败！token获取失败');
	}
	else
	{
				require_once(QISHI_ROOT_PATH.'include/fun_user.php');
				$uinfo=get_user_intaobao_access_token($token);
				if (!empty($uinfo))
				{
					update_user_info($uinfo['uid']);
					if($uinfo['utype']==1){
						$userurl="company/wap_user.php";
					}elseif($uinfo['utype']==2){
						$userurl="personal/wap_user.php";
					}
					header("Location: {$userurl}");
				}
				else
				{
					if (!empty($_SESSION['uid']) && !empty($_SESSION['utype']))
					{
					$db->query("UPDATE ".table('members')." SET taobao_access_token = '{$token}'  WHERE uid='{$_SESSION[uid]}' AND taobao_access_token='' LIMIT 1");
					exit('绑定帐号成功！');
					}
					else
					{
					$_SESSION['taobao_access_token']=$token;
					header("Location:?act=reg");
					}
				}
	}
	
}
elseif ($act=='reg')
{
	if (empty($_SESSION["taobao_access_token"]))
	{
		exit("access_token is empty");
	}
	else
	{
		require_once(QISHI_ROOT_PATH.'include/tpl.inc.php');
		$smarty->assign('title','完善信息 - '.$_CFG['site_name']);
		$smarty->assign('t_url',"?act=");
		$smarty->display('wap/wap-bind-taobao.html');
	}
}
elseif ($act=='reg_save')
{
	if (empty($_SESSION["taobao_access_token"]))
	{
		exit("access_token is empty");
	}
	$val['username']=!empty($_POST['username'])?trim(utf8_to_gbk($_POST['username'])):exit("输入用户名");
	$val['email']=!empty($_POST['email'])?trim($_POST['email']):exit("输入邮箱");
	$val['member_type']=intval($_POST['member_type']);
	$val['password']=!empty($_POST['password'])?trim($_POST['password']):exit("输入密码");
	if($val['password']!=trim($_POST['rpassword'])){
		exit("密码不一致");
	}	
	require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
	$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
	unset($dbhost,$dbuser,$dbpass,$dbname);
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$sql="select * from ".table("members")." where username='$val[username]' or email='$val[email]'";
	$row = $db->getall($sql);
	if(!empty($row)){
		exit("用户名或邮箱已经存在！");
	}
	$userid=user_register($val['username'],$val['password'],$val['member_type'],$val['email']);
	if ($userid)
	{
		$db->query("UPDATE ".table('members')." SET taobao_access_token = '{$_SESSION['taobao_access_token']}'  WHERE uid='{$userid}' AND taobao_access_token='' LIMIT 1");
		unset($_SESSION["taobao_access_token"]);
		update_user_info($userid);
		exit("ok");
	}
	else
	{
		unset($_SESSION["taobao_access_token"]);
		require_once(QISHI_ROOT_PATH.'include/tpl.inc.php');
		exit('注册失败！');
	}
	
}