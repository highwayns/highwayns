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
$code = $_GET['code'];
if($act == 'login' && empty($code))
{
	$url="https://oauth.taobao.com/authorize?response_type=code&client_id={$_CFG['taobao_appkey']}&redirect_uri=";
	$url.=urlencode("{$_CFG['site_domain']}{$_CFG['site_dir']}user/connect_taobao.php");
	header("Location:{$url}");	
	
}
elseif($act == 'login' && !empty($code))
{
	require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
	$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
	unset($dbhost,$dbuser,$dbpass,$dbname);
	require_once(QISHI_ROOT_PATH.'include/tpl.inc.php');
	if (empty($code))
	{
	exit('参数错误！');
	}
	else
	{
	$url = 'https://oauth.taobao.com/token';
	$postfields= array(
		'grant_type'=>'authorization_code',
		'client_id'=>$_CFG['taobao_appkey'],
		'client_secret'=>$_CFG['taobao_appsecret'],
		'code'=>$code,
		'redirect_uri'=>$_CFG['site_domain'].$_CFG['site_dir'].'user/connect_taobao.php'
	);
	$post_data = '';
	foreach($postfields as $key=>$value){
		$post_data .="$key=".urlencode($value)."&";
	}
	$result = https_request($url,$post_data);
	$jsoninfo = json_decode($result, true);
	$token = $jsoninfo["access_token"];
	$taobao_user_id = $jsoninfo["taobao_user_id"];
	$taobao_nickname = iconv('utf-8','gbk',urldecode($jsoninfo["taobao_user_nick"]));
	}
	if (empty($token))
	{
	$link[0]['text'] = "返回上一页";
	$link[0]['href'] = "{$_CFG['site_dir']}user/connect_taobao.php";
	showmsg('登录失败！token获取失败',0);
	}
	else
	{
				require_once(QISHI_ROOT_PATH.'include/fun_user.php');
				$uinfo=get_user_intaobao_access_token($taobao_user_id);
				if (!empty($uinfo))
				{
					update_user_info($uinfo['uid']);
					$member_url=get_member_url($_SESSION['utype']);
					header("Location: {$member_url}");
				}
				else
				{
					if (!empty($_SESSION['uid']) && !empty($_SESSION['utype']))
					{
					$time=time();
					$db->query("UPDATE ".table('members')." SET taobao_access_token = '{$taobao_user_id}',taobao_nick='{$taobao_nickname}',bindingtime='{$time}' WHERE uid='{$_SESSION[uid]}' AND taobao_access_token='' LIMIT 1");
					$link[0]['text'] = "进入会员中心";
					$link[0]['href'] = get_member_url($_SESSION['utype']);
					showmsg('绑定帐号成功！',2,$link);
					}
					else
					{
					$_SESSION['taobao_access_token']=$taobao_user_id;
					$_SESSION['taobao_nickname']=$taobao_nickname;
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
		$smarty->assign('third_name',"淘宝");
		$smarty->assign('nickname',$_SESSION['taobao_nickname']);
		$smarty->assign('openid',$_SESSION["taobao_access_token"]);
		$smarty->assign('bindtype','taobao');
		$smarty->display('user/connect_activate.htm');
	}
}
elseif ($act=='reg_save')
{
	if (empty($_SESSION["taobao_access_token"]))
	{
		exit("access_token is empty");
	}
	
	$val['username']=!empty($_POST['nickname'])?trim($_POST['nickname']):exit("err");
	$val['mobile']=!empty($_POST['mobile'])?trim($_POST['mobile']):exit("err");
	$val['email']=!empty($_POST['email'])?trim($_POST['email']):exit("err");
	$val['member_type']=intval($_POST['utype']);
	$val['password']=!empty($_POST['password'])?trim($_POST['password']):exit("err");
	require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
	$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
	unset($dbhost,$dbuser,$dbpass,$dbname);
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$userid=user_register(3,$val['password'],$val['member_type'],$val['email'],$val['mobile'],$uc_reg=true);
	if ($userid>0)
	{	
		$time=time();
		$db->query("UPDATE ".table('members')." SET taobao_access_token = '{$_SESSION['taobao_access_token']}', taobao_nick = '{$val['username']}',taobao_binding_time='{$time}'  WHERE uid='{$userid}' AND taobao_access_token='' LIMIT 1");
		unset($_SESSION["taobao_access_token"]);
		unset($_SESSION["taobao_nickname"]);
		update_user_info($userid);
		$userurl=get_member_url($val['member_type']);
		header("Location:{$userurl}");
	}
	else
	{
		unset($_SESSION["taobao_access_token"]);
		unset($_SESSION["taobao_nickname"]);
		require_once(QISHI_ROOT_PATH.'include/tpl.inc.php');
		$link[0]['text'] = "返回首页";
		$link[0]['href'] = "{$_CFG['site_dir']}";
		showmsg('注册失败！',0,$link);
	}
	
}