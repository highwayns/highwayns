<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : ''; 
$mobile=trim($_POST['mobile']);
$send_key=trim($_POST['send_key']);
if (empty($send_key) || $send_key<>$_SESSION['send_mobile_key'])
{
exit("検証コードエラー");
}
$SMSconfig=get_cache('sms_config');
if ($SMSconfig['open']!="1")
{
exit("ショートメッセージモジュール無効");
}
if ($act=="send_code")
{
		if (empty($mobile) || !preg_match("/^(13|15|14|17|18)\d{9}$/",$mobile))
		{
		exit("携帯番号エラー");
		}
		$sql = "select * from ".table('members')." where mobile = '{$mobile}' LIMIT 1";
		$userinfo=$db->getone($sql);
		if ($userinfo && $userinfo['uid']<>$_SESSION['uid'])
		{
		exit("携帯番号既に存在します！その他携帯番号を入力してください");
		}
		elseif(!empty($userinfo['mobile']) && $userinfo['mobile_audit']=="1" && $userinfo['mobile']==$mobile)
		{
		exit("携帯番号 {$mobile} 検証済み！");
		}
		else
		{
			if ($_SESSION['send_time'] && (time()-$_SESSION['send_time'])<180)
			{
			exit("180秒後再検証してください！");
			}
			$rand=mt_rand(100000, 999999);	
			$r=captcha_send_sms($mobile,"{$_CFG['site_name']}携帯認定有難うございます,検証コードは:{$rand}");
			if ($r=="success")
			{
			$_SESSION['mobile_rand']=$rand;
			$_SESSION['send_time']=time();
			$_SESSION['verify_mobile']=$mobile;
			exit("success");
			}
			else
			{
			exit("SMS配置エラー，ウェブ管理者に連絡");
			}
		} 
}
elseif ($act=="verify_code")
{
	$verifycode=trim($_POST['verifycode']);
	if (empty($verifycode) || empty($_SESSION['mobile_rand']) || $verifycode<>$_SESSION['mobile_rand'])
	{
		exit("確認コードエラー");
	}
	else
	{
			$uid=intval($_SESSION['uid']);
			if (empty($uid))
			{
				exit("システムエラー，UID失った！");
			}
			else
			{
					$setsqlarr['mobile']=$_SESSION['verify_mobile'];
					$setsqlarr['mobile_audit']=1;
					$db->updatetable(table('members'),$setsqlarr," uid='{$uid}'");
					if ($_SESSION['utype']=="2")
					{
						$infoarr['phone']=$setsqlarr['mobile'];
						$db->updatetable(table('members_info'),$infoarr," uid='{$uid}'");
						
						$u['telephone']=$setsqlarr['mobile'];
						$db->updatetable(table('resume'),$u," uid='{$uid}'");
					}
					unset($setsqlarr,$infoarr,$_SESSION['verify_mobile'],$_SESSION['mobile_rand']);
					if ($_SESSION['utype']=="1" && ($_CFG['operation_mode']=='1' || $_CFG['operation_mode']=='3'))
					{
						$rule=get_cache('points_rule');
						if ($rule['verifymobile']['value']>0)
						{
							$info=$db->getone("SELECT uid FROM ".table('members_handsel')." WHERE uid ='{$_SESSION['uid']}' AND htype='verifymobile' LIMIT 1");
							if(empty($info))
							{
							$time=time();			
							$db->query("INSERT INTO ".table('members_handsel')." (uid,htype,addtime) VALUES ('{$_SESSION['uid']}', 'verifymobile','{$time}')");
							require_once(HIGHWAY_ROOT_PATH.'include/fun_comapny.php');
							report_deal($_SESSION['uid'],$rule['verifymobile']['type'],$rule['verifymobile']['value']);
							$user_points=get_user_points($_SESSION['uid']);
							$operator=$rule['verifymobile']['type']=="1"?"+":"-";
							$_SESSION['handsel_verifymobile']=$_CFG['points_byname'].$operator.$rule['verifymobile']['value'];
							write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username']," 携帯番号検証済み，{$_CFG['points_byname']}({$operator}{$rule['verifymobile']['value']})，(残る:{$user_points})",1,1016,"携帯認定済み","{$operator}{$rule['verifymobile']['value']}","{$user_points}");
							}
						}
					}elseif ($_SESSION['utype']=='4' && $_CFG['operation_train_mode']=='1')
					{
						$rule=get_cache('points_rule');
						if ($rule['train_verifymobile']['value']>0)
						{
							$info=$db->getone("SELECT uid FROM ".table('members_handsel')." WHERE uid ='{$_SESSION['uid']}' AND htype='verifymobile' LIMIT 1");
							if(empty($info))
							{
							$time=time();			
							$db->query("INSERT INTO ".table('members_handsel')." (uid,htype,addtime) VALUES ('{$_SESSION['uid']}', 'verifymobile','{$time}')");
							require_once(HIGHWAY_ROOT_PATH.'include/fun_train.php');
							report_deal($_SESSION['uid'],$rule['train_verifymobile']['type'],$rule['train_verifymobile']['value']);
							$user_points=get_user_points($_SESSION['uid']);
							$operator=$rule['train_verifymobile']['type']=="1"?"+":"-";
							$_SESSION['handsel_verifymobile']=$_CFG['train_points_byname'].$operator.$rule['train_verifymobile']['value'];
							write_memberslog($_SESSION['uid'],4,9101,$_SESSION['username']," 携帯検証済み，{$_CFG['train_points_byname']}({$operator}{$rule['train_verifymobile']['value']})，(残る:{$user_points})");
							}
						}
					}elseif ($_SESSION['utype']=='3' && $_CFG['operation_hunter_mode']=='1')
					{
						$rule=get_cache('points_rule');
						if ($rule['hunter_verifymobile']['value']>0)
						{
							$info=$db->getone("SELECT uid FROM ".table('members_handsel')." WHERE uid ='{$_SESSION['uid']}' AND htype='verifymobile' LIMIT 1");
							if(empty($info))
							{
							$time=time();			
							$db->query("INSERT INTO ".table('members_handsel')." (uid,htype,addtime) VALUES ('{$_SESSION['uid']}', 'verifymobile','{$time}')");
							require_once(HIGHWAY_ROOT_PATH.'include/fun_hunter.php');
							report_deal($_SESSION['uid'],$rule['hunter_verifymobile']['type'],$rule['hunter_verifymobile']['value']);
							$user_points=get_user_points($_SESSION['uid']);
							$operator=$rule['hunter_verifymobile']['type']=="1"?"+":"-";
							$_SESSION['handsel_verifymobile']=$_CFG['hunter_points_byname'].$operator.$rule['hunter_verifymobile']['value'];
							write_memberslog($_SESSION['uid'],3,9201,$_SESSION['username'],"携帯番号確認済み，{$_CFG['hunter_points_byname']}({$operator}{$rule['hunter_verifymobile']['value']})，(残る:{$user_points})");
							}
						}
					}

					exit("success");
			}
	}
}
// 个人发布简历 修改简历的时候 
elseif($act == "mobile_code")
{
	$verifycode=trim($_POST['mobile_code']);
	if (empty($verifycode) || empty($_SESSION['mobile_rand']) || $verifycode<>$_SESSION['mobile_rand'])
	{
		exit("false");
	}
	else
	{
		$uid=intval($_SESSION['uid']);
		if (empty($uid))
		{
			exit("false");
		}
		else
		{
			unset($_SESSION['verify_mobile'],$_SESSION['mobile_rand']);
			exit("true");

		}
	}
}
?>
