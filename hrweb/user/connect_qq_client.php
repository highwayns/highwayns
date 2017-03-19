<?php
 /*
 * 74cms QQ互联 client-side模式
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
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'QQlogin';
if($act == 'QQlogin')
{
$url="https://graph.qq.com/oauth2.0/authorize?response_type=token&client_id={$_CFG['qq_appid']}&redirect_uri={$_CFG['site_domain']}{$_CFG['site_dir']}user/connect_qq_client.php".urlencode('?act=login_check');
header("Location:{$url}");	
}
elseif ($act=='login_check')
{
	$html ="<script type=\"text/javascript\" src=\"http://qzonestyle.gtimg.cn/qzone/openapi/qc_loader.js\" charset=\"utf-8\" data-callback=\"true\"></script> ";
	$html.="<script type=\"text/javascript\">";
	$html.="if(QC.Login.check())";
	$html.="{";
	$html.="QC.Login.getMe(function(openId, accessToken)";
	$html.="{";
	$html.="window.location.href = '?act=login_go&openid='+openId+'&accessToken='+accessToken;"; 
	$html.="});";
	$html.="}";
	$html.="</script>";
	exit($html);
}
elseif ($act=='login_go')
{
	$_SESSION["openid"] = trim($_GET['openid']);
	$_SESSION["accessToken"] = trim($_GET['accessToken']);
	if (empty($_SESSION["openid"]))
	{
		showmsg('登录失败！openid获取不到',0);
	}
	else
	{
			require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
			$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
			unset($dbhost,$dbuser,$dbpass,$dbname);
			require_once(QISHI_ROOT_PATH.'include/fun_user.php');
			$user=get_user_inqqopenid($_SESSION["openid"]);
			if (!empty($user))
			{
				update_user_info($user['uid']);
				$userurl=get_member_url($_SESSION['utype']);
				header("Location:{$userurl}");
			}
			else
			{
				if (!empty($_SESSION['uid']) && !empty($_SESSION['utype']) && !empty($_SESSION['openid']))
				{
					$url = "https://graph.qq.com/user/get_user_info?access_token=".$_SESSION['accessToken']."&oauth_consumer_key={$_CFG['qq_appid']}&openid=".$_SESSION["openid"];
					$ch = curl_init();
					curl_setopt($ch, CURLOPT_URL, $url);
					curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);
					curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, FALSE);
					curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
					$output = curl_exec($ch);
					curl_close($ch);
					$jsoninfo = json_decode($output, true);
					$nickname = iconv("utf-8","gbk",$jsoninfo["nickname"]);
					$time=time();
					require_once(QISHI_ROOT_PATH.'include/tpl.inc.php');
					$db->query("UPDATE ".table('members')." SET qq_openid = '{$_SESSION['openid']}',qq_nick ='{$nickname}' ,qq_binding_time = '{$time}' WHERE uid='{$_SESSION[uid]}' AND qq_openid='' LIMIT 1");
					$link[0]['text'] = "进入会员中心";
					$link[0]['href'] = get_member_url($_SESSION['utype']);
					$_SESSION['uqqid']=$_SESSION['openid'];
					showmsg('绑定QQ帐号成功！',2,$link);
				}
				else
				{
					header("Location:?act=reg");
				}
			}
	}
}
elseif ($act=='reg')
{	
	if (empty($_SESSION["openid"]))
	{
		exit("openid is empty");
	}
	else
	{
		$url = "https://graph.qq.com/user/get_user_info?access_token=".$_SESSION['accessToken']."&oauth_consumer_key={$_CFG['qq_appid']}&openid=".$_SESSION["openid"];
		$ch = curl_init();
		curl_setopt($ch, CURLOPT_URL, $url);
		curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);
		curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, FALSE);
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
		$output = curl_exec($ch);
		curl_close($ch);
		$jsoninfo = json_decode($output, true);
		$nickname = iconv("utf-8","gbk",$jsoninfo["nickname"]);
		require_once(QISHI_ROOT_PATH.'include/tpl.inc.php');
		$smarty->assign('title','补充信息 - '.$_CFG['site_name']);
		$smarty->assign('third_name',"QQ");
		$smarty->assign('qqurl',"?act=");
		$smarty->assign('nickname',$nickname);
		$smarty->assign('openid',$_SESSION["openid"]);
		$smarty->assign('nickname',$nickname);
		$smarty->assign('bindtype','qq');
		$smarty->display('user/connect_activate.htm');
	}
}
elseif ($act=='reg_save')
{
	if (empty($_SESSION["openid"]))
	{
		exit("openid is empty");
	}
	$val['qq_nick']= trim(utf8_to_gbk($_POST['nickname']));
	$val['email']=!empty($_POST['email'])?trim($_POST['email']):exit("err");
	$val['mobile']=!empty($_POST['mobile'])?trim($_POST['mobile']):exit("err");
	$val['member_type']=intval($_POST['utype']);
	$val['password']=!empty($_POST['password'])?trim($_POST['password']):exit("err");
	require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
	$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
	unset($dbhost,$dbuser,$dbpass,$dbname);
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$userid=user_register(3,$val['password'],$val['member_type'],$val['email'],$val['mobile'],$uc_reg=true);
	if ($userid)
	{
		$time=time();
		$db->query("UPDATE ".table('members')." SET qq_openid = '{$_SESSION[openid]}', qq_nick = '{$val[qq_nick]}', qq_binding_time = '{$time}' WHERE uid='{$userid}' AND qq_openid='' LIMIT 1");
		update_user_info($userid);
		$userurl=get_member_url($val['member_type']);
		header("Location:{$userurl}");
	}
	else
	{
		require_once(QISHI_ROOT_PATH.'include/tpl.inc.php');
		$link[0]['text'] = "返回首页";
		$link[0]['href'] = "{$_CFG['site_dir']}";
		showmsg('注册失败！',0,$link);
	}
}

elseif($act == 'binding')
{
	$url="https://graph.qq.com/oauth2.0/authorize?response_type=token&client_id={$_CFG['qq_appid']}&redirect_uri={$_CFG['site_domain']}{$_CFG['site_dir']}user/connect_qq_client.php".urlencode('?act=binding_check');
header("Location:{$url}");
}
elseif ($act=='binding_check')
{
	$html ="<script type=\"text/javascript\" src=\"http://qzonestyle.gtimg.cn/qzone/openapi/qc_loader.js\" charset=\"utf-8\" data-callback=\"true\"></script> ";
	$html.="<script type=\"text/javascript\">";
	$html.="if(QC.Login.check())";
	$html.="{";			
	$html.="QC.Login.getMe(function(openId, accessToken)";
	$html.="{";
	$html.="window.location.href = '?act=binding_callback&openid='+openId+'&accessToken='+accessToken;"; 
	$html.="});";
	$html.="}";
	$html.="</script>";
	exit($html);
}
elseif ($act=='binding_callback')
{
		if (empty($_SESSION['uid']) || empty($_SESSION['utype']))
		{
			exit("error");
		}
		$_SESSION["openid"] = trim($_GET['openid']);
		$_SESSION["accessToken"] = trim($_GET['accessToken']);
		if (empty($_SESSION['openid']))
		{
			exit("error");
		}
			require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
			$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
			unset($dbhost,$dbuser,$dbpass,$dbname);
			require_once(QISHI_ROOT_PATH.'include/fun_user.php');
			$user=get_user_inqqopenid($_SESSION["openid"]);
			require_once(QISHI_ROOT_PATH.'include/tpl.inc.php');
			if (!empty($user))
			{
					$link[0]['text'] = "用别的QQ帐号绑定";
					$link[0]['href'] = "?act=binding";
					$link[1]['text'] = "进入会员中心";
					$link[1]['href'] =get_member_url($_SESSION['utype']);
					showmsg('此QQ帐号已经绑定了其他会员,请换一个QQ帐号！',2,$link);
			}
			else
			{	
					$url = "https://graph.qq.com/user/get_user_info?access_token=".$_SESSION['accessToken']."&oauth_consumer_key={$_CFG['qq_appid']}&openid=".$_SESSION["openid"];
					$ch = curl_init();
					curl_setopt($ch, CURLOPT_URL, $url);
					curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);
					curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, FALSE);
					curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
					$output = curl_exec($ch);
					curl_close($ch);
					$jsoninfo = json_decode($output, true);
					$nickname = iconv("utf-8","gbk",$jsoninfo["nickname"]);
					$time=time();
					$db->query("UPDATE ".table('members')." SET qq_openid = '{$_SESSION[openid]}', qq_nick = '{$nickname}', qq_binding_time = '{$time}' WHERE uid='".$_SESSION['uid']."' AND qq_openid='' LIMIT 1");
					$link[0]['text'] = "进入会员中心";
					$link[0]['href'] = get_member_url($_SESSION['utype']);
					$_SESSION['uqqid']=$_SESSION['openid'];
					showmsg('绑定QQ帐号成功！',2,$link);
			}
}
?>