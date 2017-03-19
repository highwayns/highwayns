<?php
define('IN_QISHI', true);
$alias="QS_login";
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
require_once(QISHI_ROOT_PATH.'include/fun_user.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
unset($dbhost,$dbuser,$dbpass,$dbname);
$smarty->caching = false;
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'binding';
if($act == 'binding'){
	$smarty->assign("from",$_GET['from']);
	$smarty->display('wap/wap-binding.html');
}
elseif($act == 'binding_save')
{
	$fromUsername = trim($_POST['from']);
	if(empty($fromUsername)){
		$smarty->assign("from",$_POST['from']);
		$smarty->assign('err',"绑定失败！请返回微信重新绑定");
		$smarty->display('wap/wap-binding.html');
		die;
	}
	$username = trim($_POST['username']);
	if(empty($username)){
		$smarty->assign("from",$_POST['from']);
		$smarty->assign('err',"请输入用户名");
		$smarty->display('wap/wap-binding.html');
		die;
	}
	$password = trim($_POST['password']);
	if(empty($password)){
		$smarty->assign("from",$_POST['from']);
		$smarty->assign('err',"请输入密码");
		$smarty->display('wap/wap-binding.html');
		die;
	}
	
	$usinfo = $db->getone("select * from ".table('members')." where username = '{$username}' LIMIT 1");
	
	if(!empty($usinfo)){
		if($usinfo['weixin_openid']){
			$smarty->assign("from",$_POST['from']);
			$smarty->assign('err',"您已绑定了微信帐号");
			$smarty->display('wap/wap-binding.html');
			die;
		}
		$success = false;
		$pwd_hash=$usinfo['pwd_hash'];
		$usname=$usinfo['username'];
		$pwd=md5(md5($password).$pwd_hash.$QS_pwdhash);
		if ($usinfo['password']==$pwd)
		{
			$access_token = get_access_token();
			$w_url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=".$access_token."&openid=".$fromUsername."&lang=zh_CN";
			$w_result = https_request($w_url);
			$w_userinfo = json_decode($w_result,true);
			$success == true;
			$db->query("update ".table('members')." set `weixin_openid`='".$fromUsername."',`weixin_nick`='".$w_userinfo['nickname']."',bindingtime=".time()." where uid=".$usinfo['uid']);
			require_once(QISHI_ROOT_PATH.'include/fun_wap.php');
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
			if (wap_user_login($username,$password))
			{	
					if(!empty($_SESSION['url'])){
						header("location:".$_SESSION['url']);
						unset($_SESSION['url']);
						die;
					}
				$smarty->display('wap/wap-binding-success.html');
				die;
			}
		}
		else
		{
			$success = false;
		}
		if($success == false){
				$smarty->assign("from",$_POST['from']);
				$smarty->assign('err',"用户名或密码错误!");
				$smarty->display('wap/wap-binding.html');
		}
	}else{
			$smarty->assign("from",$_POST['from']);
			$smarty->assign('err',"用户名或密码错误!");
			$smarty->display('wap/wap-binding.html');
	}
		
}
elseif($act == 'change_binding'){
	$smarty->assign("from",$_GET['from']);
	$smarty->display('wap/wap-change-binding.html');
}
elseif($act == 'change_binding_save')
{
	$fromUsername = trim($_POST['from']);
	if(empty($fromUsername)){
		$smarty->assign("from",$_POST['from']);
		$smarty->assign('err',"绑定失败！请返回微信重新绑定");
		$smarty->display('wap/wap-change-binding.html');
		die;
	}
	$username = trim($_POST['username']);
	if(empty($username)){
		$smarty->assign("from",$_POST['from']);
		$smarty->assign('err',"请输入用户名");
		$smarty->display('wap/wap-change-binding.html');
		die;
	}
	$password = trim($_POST['password']);
	if(empty($password)){
		$smarty->assign("from",$_POST['from']);
		$smarty->assign('err',"请输入密码");
		$smarty->display('wap/wap-change-binding.html');
		die;
	}
	
	$usinfo = $db->getone("select * from ".table('members')." where username = '{$username}' LIMIT 1");
	
	if(!empty($usinfo)){
		$db->query("update ".table('members')." set `weixin_openid`=null,`weixin_nick`='',bindingtime=0 where weixin_openid='".$fromUsername."'");
		$success = false;
		$pwd_hash=$usinfo['pwd_hash'];
		$usname=$usinfo['username'];
		$pwd=md5(md5($password).$pwd_hash.$QS_pwdhash);
		if ($usinfo['password']==$pwd)
		{
			$access_token = get_access_token();
			$w_url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=".$access_token."&openid=".$fromUsername."&lang=zh_CN";
			$w_result = https_request($w_url);
			$w_userinfo = json_decode($w_result,true);
			$success == true;
			$db->query("update ".table('members')." set `weixin_openid`='".$fromUsername."',`weixin_nick`='".$w_userinfo['nickname']."',bindingtime=".time()." where uid=".$usinfo['uid']);
			require_once(QISHI_ROOT_PATH.'include/fun_wap.php');
			
			if (wap_user_login($username,$password))
						{	
								if(!empty($_SESSION['url'])){
									header("location:".$_SESSION['url']);
									unset($_SESSION['url']);
									die;
								}
							$smarty->display('wap/wap-binding-success.html');
							die;
						}
		}
		else
		{
			$success = false;
		}
		if($success == false){
				$smarty->assign("from",$_POST['from']);
				$smarty->assign('err',"用户名或密码错误!");
				$smarty->display('wap/wap-change-binding.html');
		}
	}else{
			$smarty->assign("from",$_POST['from']);
			$smarty->assign('err',"用户名或密码错误!");
			$smarty->display('wap/wap-change-binding.html');
	}
		
}
?>