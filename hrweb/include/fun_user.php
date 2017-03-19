<?php
 /*
 * 74cms 会员中心函数
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
 if(!defined('IN_QISHI'))
 {
 	die('Access Denied!');
 }
//注册会员
function user_register($reg_type,$password,$member_type=0,$email="",$mobile="",$uc_reg=true,$username="",$weixin_openid="",$weixin_nickname="")
{
	global $db,$timestamp,$_CFG,$online_ip,$QS_pwdhash;
	$member_type=intval($member_type);
	$reg_type=intval($reg_type);
	$email=trim($email);
	$email_audit=intval($email_audit);
	$mobile=trim($mobile);

	$ck_email=get_user_inemail($email);
	$ck_mobile=get_user_inmobile($mobile);
	if($member_type==0 || $reg_type==0)
	{
	return -1;	
	}
	elseif ($reg_type==2 && !empty($ck_email))//邮箱注册验证 邮箱
	{
	return -2;
	}
	elseif($reg_type==1 &&  !empty($ck_mobile))//手机注册 验证手机号
	{
	return -3;
	}
	$pwd_hash=randstr();
	$name_rand=randusername();
	$password_hash=md5(md5($password).$pwd_hash.$QS_pwdhash);
	if(!$username)
	{
		if($reg_type==1)
		{
			$setsqlarr['username']= strtolower("sj_".$name_rand);
		}
		elseif($reg_type==2)
		{
			$setsqlarr['username']=strtolower("em_".$name_rand);
		}
		else
		{
			$setsqlarr['username']=strtolower("userthird_".$name_rand);
		}
	}
	else
	{
		
		$ck_uname=get_user_inusername($username);
		if(!empty($ck_uname))
		{
			return -4;
		}
		else
		{
			$setsqlarr['username']=$username;
		}
	}

	$setsqlarr['password']=$password_hash;
	$setsqlarr['pwd_hash']=$pwd_hash;

	if($email)
	{
		$setsqlarr['email']=$email;
		if($_CFG['check_reg_email']=="1" && $reg_type!=3 && $reg_type!=4)
		{
			$setsqlarr['email_audit']=1;
		}
		else
		{
			$setsqlarr['email_audit']=0;
		}
		
	}
	if($mobile)
	{
		$setsqlarr['mobile']=$mobile;
		if($reg_type!=3 && $reg_type!=4)
		{
			$setsqlarr['mobile_audit']=1;
		}
	}
	$setsqlarr['utype']=$member_type;
	$setsqlarr['reg_time']=$timestamp;
	$setsqlarr['reg_ip']=$online_ip;
	$setsqlarr['reg_type']=1;
	if($weixin_openid!=''){
		$setsqlarr['weixin_nick']=$weixin_nickname;
		$setsqlarr['weixin_openid']=$weixin_openid;
		$setsqlarr['bindingtime']=$setsqlarr['reg_time'];
		$w_uid = $db->getone("select uid from ".table("members")." where weixin_openid='".$weixin_openid."'");
		if($w_uid){
			return $w_uid['uid'];
		}
	}
	$insert_id=$db->inserttable(table('members'),$setsqlarr,true);
			if($member_type=="1")
			{
				$setarr['uid']=$insert_id;
				if(!$db->inserttable(table("members_points"),$setarr))  return false;
				if(!$db->inserttable(table("members_setmeal"),$setarr))  return false;
					$points=get_cache('points_rule');
					include_once(QISHI_ROOT_PATH.'include/fun_company.php');
					set_consultant($insert_id);
					if ($points['reg_points']['value']>0)
					{
						report_deal($insert_id,$points['reg_points']['type'],$points['reg_points']['value']);
						$operator=$points['reg_points']['type']=="1"?"+":"-";
						write_memberslog($insert_id,1,9001,$username,"新注册会员,({$operator}{$points['reg_points']['value']}),(剩余:{$points['reg_points']['value']})",1,1010,"注册会员系统自动赠送积分","{$operator}{$points['reg_points']['value']}","{$points['reg_points']['value']}");
						//积分变更记录
						write_setmeallog($insert_id,$username,"注册会员系统自动赠送：({$operator}{$points['reg_points']['value']}),(剩余:{$points['reg_points']['value']})",1,'0.00','1',1,1);
					
					}
					if ($_CFG['reg_service']>0){
						set_members_setmeal($insert_id,$_CFG['reg_service']);
						$setmeal=get_setmeal_one($_CFG['reg_service']);
						write_memberslog($insert_id,1,9002,$username,"注册会员系统自动赠送：{$setmeal['setmeal_name']}",2,1011,"开通服务(系统赠送)","-","-");
						//套餐变更记录
						write_setmeallog($insert_id,$username,"注册会员系统自动赠送：{$setmeal['setmeal_name']}",1,'0.00','1',2,1);
					}
			}
			write_memberslog($insert_id,$member_type,1000,$username,"注册成为会员");
return $insert_id;
}
//会员登录
function user_login($account,$password,$account_type=1,$uc_login=true,$expire=NULL)
{
	global $timestamp,$online_ip,$QS_pwdhash;
	$usinfo = $login = array();
	$success = false;
	if ($account_type=="1")
	{
		$usinfo=get_user_inusername($account);
	}
	elseif ($account_type=="2")
	{
		$usinfo=get_user_inemail($account);
	}
	elseif ($account_type=="3")
	{
		$usinfo=get_user_inmobile($account);
	}
	if (!empty($usinfo))
	{
		$pwd_hash=$usinfo['pwd_hash'];
		$usname=addslashes($usinfo['username']);
		$pwd=md5(md5($password).$pwd_hash.$QS_pwdhash);
		if ($usinfo['password']==$pwd)
		{
			if($usinfo['status'] == 2){
				$usinfo='';
				$success=false;
				$login['qs_login']='false';
			}else{
				update_user_info($usinfo['uid'],true,true,$expire);
				$login['qs_login']=get_member_url($usinfo['utype']);
				$success=true;
				write_memberslog($usinfo['uid'],$usinfo['utype'],1001,$usname,"成功登录");
			} 
		}
		else
		{
		$usinfo='';
		$success=false;
		}
	}
	return $login;	
}
//检测COOKIE
function check_cookie($uid,$name,$pwd){
 	global $db;
 	$row = $db->getone("SELECT COUNT(*) AS num FROM ".table('members')." WHERE uid='{$uid}' and username='{$name}' and password = '{$pwd}'");
 	if($row['num'] > 0)
	{
 	return true;
 	}else{
 	return false;
 	}
 }
 /**
  *
  * 更新用户信息
  *
  *
  */
 function update_user_info($uid,$record=true,$setcookie=true,$cookie_expire=NULL)
 {
 	global $timestamp, $online_ip,$db,$QS_cookiepath,$QS_cookiedomain,$_CFG;//3.4升级修改 引入变量$_CFG
	$user = get_user_inid($uid);
	if (empty($user))
	{
	return false;
	}
	else
	{
	unset($_SESSION['no_self']);
 	$_SESSION['uid'] = intval($user['uid']);
 	$_SESSION['username'] = addslashes($user['username']);
	$_SESSION['utype']=intval($user['utype']);
	}
	if ($setcookie)
	{
		$expire=intval($cookie_expire)>0?time()+3600*24*$cookie_expire:0;
		setcookie('QS[uid]',$user['uid'],$expire,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[username]',addslashes($user['username']),$expire,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[password]',$user['password'],$expire,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[utype]',$user['utype'], $expire,$QS_cookiepath,$QS_cookiedomain);
	}
	if ($record)
	{
 		if (($_CFG['operation_mode']=='1' || $_CFG['operation_mode']=='3') && $_SESSION['utype']=="1" )
		{
			$rule=get_cache('points_rule');
			if ($rule['userlogin']['value']>0 )
			{
				$time=time();
				$today=mktime(0, 0, 0,date('m'), date('d'), date('Y'));
				$info=$db->getone("SELECT uid FROM ".table('members_handsel')." WHERE uid ='{$_SESSION['uid']}' AND htype='userlogin' AND addtime>{$today}  LIMIT 1");
				if(empty($info))
				{	
					$members_handsel_arr['uid']=$_SESSION['uid'];
					$members_handsel_arr['htype']="userlogin";
					$members_handsel_arr['addtime']=$time;
					$db->inserttable(table("members_handsel"),$members_handsel_arr);
					require_once(QISHI_ROOT_PATH.'include/fun_company.php');
					report_deal($_SESSION['uid'],$rule['userlogin']['type'],$rule['userlogin']['value']);
					$user_points=get_user_points($_SESSION['uid']);
					$operator=$rule['userlogin']['type']=="1"?"+":"-";
					$_SESSION['handsel_userlogin']=$operator.$rule['userlogin']['value'];
					write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],date("Y-m-d")." 第一次登录，({$operator}{$rule['userlogin']['value']})，(剩余:{$user_points})",1,1014,"会员每天第一次登录","{$operator}{$rule['userlogin']['value']}","{$user_points}");
				}
			}
		}
		elseif($_SESSION['utype']=='2' )
		{
			$time=time();
			$today=mktime(0, 0, 0,date('m'), date('d'), date('Y'));
			$info=$db->getone("SELECT uid FROM ".table('members_handsel')." WHERE uid ='{$_SESSION['uid']}' AND htype='userlogin' AND addtime>{$today}  LIMIT 1");
			if(empty($info))
			{				
				$members_handsel_arr['uid']=$_SESSION['uid'];
				$members_handsel_arr['htype']="userlogin";
				$members_handsel_arr['addtime']=$time;
				$db->inserttable(table("members_handsel"),$members_handsel_arr);
				$_SESSION['personal_login_first']=1;
			}
		}
	}
	//消息
	$user_pmid=$db->getone("SELECT pmid FROM ".table('pms_sys_log')." WHERE loguid ='{$_SESSION['uid']}' ORDER BY `pmid` DESC  LIMIT 1");
	$user_pmid=intval($user_pmid['pmid']);
	$result = $db->query("SELECT * FROM ".table('pms_sys')." WHERE spmid>{$user_pmid} AND (spms_usertype='0' OR spms_usertype='{$_SESSION['utype']}') AND spms_type='1' ");
	while($row = $db->fetch_array($result))
	{
		$setsqlarr['msgtype']=1;
		$setsqlarr['msgtouid']=$_SESSION['uid'];
		$setsqlarr['msgtoname']=$_SESSION['username'];
		$setsqlarr['message']=$row['message'];
		$setsqlarr['dateline']=$timestamp;
		$setsqlarr['replytime']=$timestamp;
		$setsqlarr['new']=1;
		$db->inserttable(table('pms'),$setsqlarr);
		$log['loguid']=$_SESSION['uid'];
		$log['pmid']=$row['spmid'];
		$db->inserttable(table('pms_sys_log'),$log);
		unset($setsqlarr,$log);
	}
	//统计消息
	$pmscount=$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$_SESSION['uid']}' OR msgtouid='{$_SESSION['uid']}') AND `new`='1' AND `replyuid`<>'{$_SESSION['uid']}'");
	setcookie('QS[pmscount]',$pmscount, $expire,$QS_cookiepath,$QS_cookiedomain);
	return true;
 }
function get_user_inemail($email)
{
	global $db;
	return $db->getone("select * from ".table('members')." where email = '{$email}' LIMIT 1");
}
function get_user_inusername($username)
{
	global $db;
	$sql = "select * from ".table('members')." where username = '{$username}' LIMIT 1";
	return $db->getone($sql);
}
function get_user_inid($uid)
{
	global $db;
	$uid=intval($uid);
	$sql = "select * from ".table('members')." where uid = '{$uid}' LIMIT 1";
	return $db->getone($sql);
}
function get_user_inmobile($mobile)
{
	global $db;
	$sql = "select * from ".table('members')." where mobile = '{$mobile}' LIMIT 1";
	return $db->getone($sql);
}
function get_user_inqqopenid($openid)
{
	global $db;
	if (empty($openid))
	{
	return false;
	}
	$sql = "select * from ".table('members')." where qq_openid = '{$openid}' LIMIT 1";
	return $db->getone($sql);
}
function get_user_insina_access_token($access)
{
	global $db;
	if (empty($access))
	{
	return false;
	}
	$sql = "select * from ".table('members')." where sina_access_token = '{$access}' LIMIT 1";
	return $db->getone($sql);
}
function get_user_intaobao_access_token($access)
{
	global $db;
	if (empty($access))
	{
	return false;
	}
	$sql = "select * from ".table('members')." where taobao_access_token = '{$access}' LIMIT 1";
	return $db->getone($sql);
}
//获取随机字符串
function randstr($length=6)
{
$hash='';
$chars= 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz@#!~?:-='; 
$max=strlen($chars)-1;   
mt_srand((double)microtime()*1000000);   
for($i=0;$i<$length;$i++)   {   
$hash.=$chars[mt_rand(0,$max)];   
}   
return $hash;   
}
//获取随机字符串用于用户名
function randusername($length=6)
{
$hash='';
$chars=str_shuffle('ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz'); 
$max=strlen($chars)-1;   
mt_srand((double)microtime()*1000000);   
for($i=0;$i<$length;$i++)   {   
$hash.=$chars[mt_rand(0,$max)];   
}   
return $hash;   
}
//修改密码
function edit_password($arr,$check=true)
{
	global $db,$QS_pwdhash;
	if (!is_array($arr))return false;
	$user_info=get_user_inusername($arr['username']);
	$pwd_hash=$user_info['pwd_hash'];
	$password=md5(md5($arr['oldpassword']).$pwd_hash.$QS_pwdhash);
	if ($check)
	{
		$row = $db->getone("SELECT * FROM ".table('members')." WHERE username='{$arr['username']}' and password = '{$password}' LIMIT 1");
		if(empty($row))
		{
			return -1;
		}
	}
	$md5password=md5(md5($arr['password']).$pwd_hash.$QS_pwdhash);	
	if ($db->query( "UPDATE ".table('members')." SET password = '$md5password'  WHERE username='".$arr['username']."'")) return $arr['username'];
	write_memberslog($_SESSION['uid'],$_SESSION['utype'],1004,$_SESSION['username'],"修改了密码");
	return false;
}
//修改用户名
function edit_username($arr,$check=true)
{
	global $db,$QS_pwdhash,$QS_cookiepath,$QS_cookiedomain;
	if (!is_array($arr))return false;
	$row = $db->getone("SELECT * FROM ".table('members')." WHERE uid='{$arr['uid']}' LIMIT 1");
	if(empty($row))
	{
		return -1;
	}
	if ($db->query( "UPDATE ".table('members')." SET username = '{$arr['newusername']}'  WHERE uid='".$arr['uid']."'"))
	{
		write_memberslog($_SESSION['uid'],$_SESSION['utype'],1004,$_SESSION['username'],"修改了用户名");
		//修改session 值中的用户名
 		$_SESSION['username'] = $arr['newusername'];
		return $arr['newusername'];
	} 
	return false;
}
//获取会员登录日志
function get_user_loginlog($offset,$perpage,$get_sql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query("SELECT * FROM ".table('members_log')." ".$get_sql." ORDER BY log_id DESC ".$limit);
	while($row = $db->fetch_array($result))
	{
	$row_arr[] = $row;
	}
	return $row_arr;
}
function get_loginlog_one($uid,$type)
{
	global $db;
	$sql = "SELECT * FROM ".table('members_log')." WHERE log_uid={$uid} AND log_type={$type} ORDER BY log_id DESC LIMIT 1,1"; 
	$result = $db->getone($sql);
	return $result;
}
function get_loginlog_two($uid,$type)
{
	global $db;
	$sql = "SELECT * FROM ".table('members_log')." WHERE log_uid={$uid} AND log_type={$type} ORDER BY log_id DESC LIMIT 0,1"; 
	$result = $db->getone($sql);
	return $result;
}
?>