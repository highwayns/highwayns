<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : ''; 
$email=trim($_POST['email']);
$send_key=trim($_POST['send_key']);
if (empty($send_key) || $send_key<>$_SESSION['send_email_key'])
{
exit("検証コードエラー");
}
if ($act=="send_code")
{
		if (empty($email) || !preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]w+)*$/",$email))
		{
		exit("メールボックスフォーマットエラー");
		}
		$sql = "select * from ".table('members')." where email = '{$email}' LIMIT 1";
		$userinfo=$db->getone($sql);
		if ($userinfo && $userinfo['uid']<>$_SESSION['uid'])
		{
		exit("メールボックス既に存在します！その他メールボックスを入力してください");
		}
		elseif(!empty($userinfo['email']) && $userinfo['email_audit']=="1" && $userinfo['email']==$email)
		{
		exit("メールボックス {$email} 検証済み！");
		}
		else
		{
			if ($_SESSION['sendemail_time'] && (time()-$_SESSION['sendemail_time'])<10)
			{
			exit("60秒後再検証してください！");
			}
			$rand=mt_rand(100000, 999999);
			if (smtp_mail($email,"{$_CFG['site_name']}メール認定","{$HIGHWAY['site_name']}お知らせ：<br>メールボックス検証しています，検証コードは:<strong>{$rand}</strong>"))
			{
			$_SESSION['verify_email']=$email;
			$_SESSION['email_rand']=$rand;
			$_SESSION['sendemail_time']=time();
			exit("success");
			}
			else
			{
			exit("メールボックス配置エラー，系ウェブ管理者に連絡してください");
			}
		} 
}
elseif ($act=="verify_code")
{
	$verifycode=trim($_POST['verifycode']);
	if (empty($verifycode) || empty($_SESSION['email_rand']) || $verifycode<>$_SESSION['email_rand'])
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
					$setsqlarr['email']=$_SESSION['verify_email'];
					$setsqlarr['email_audit']=1;
					$db->updatetable(table('members'),$setsqlarr," uid='{$uid}'");
					if ($_SESSION['utype']=="2")
					{
						$infoarr['email']=$setsqlarr['email'];
						$db->updatetable(table('members_info'),$infoarr," uid='{$uid}'");
						$u['email']=$setsqlarr['email'];
						$db->updatetable(table('resume'),$u," uid='{$uid}'");
					}
					unset($setsqlarr,$_SESSION['verify_email'],$_SESSION['email_rand'],$u,$infoarr);
					if (($_CFG['operation_mode']=='1' || $_CFG['operation_mode']=='3') && $_SESSION['utype']=='1')
					{
						$rule=get_cache('points_rule');
						if ($rule['verifyemail']['value']>0)
						{
							$info=$db->getone("SELECT uid FROM ".table('members_handsel')." WHERE uid ='{$_SESSION['uid']}' AND htype='verifyemail'   LIMIT 1");
							if(empty($info))
							{
							$time=time();			
							$db->query("INSERT INTO ".table('members_handsel')." (uid,htype,addtime) VALUES ('{$_SESSION['uid']}', 'verifyemail','{$time}')");
							require_once(HIGHWAY_ROOT_PATH.'include/fun_company.php');
							report_deal($_SESSION['uid'],$rule['verifyemail']['type'],$rule['verifyemail']['value']);
							$user_points=get_user_points($_SESSION['uid']);
							$operator=$rule['verifyemail']['type']=="1"?"+":"-";
							$_SESSION['handsel_verifyemail']=$_CFG['points_byname'].$operator.$rule['verifyemail']['value'];
							write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username']," メールボックス検証済み，{$_CFG['points_byname']}({$operator}{$rule['verifyemail']['value']})，(残る:{$user_points})",1,1015,"メールボックス認定済み","{$operator}{$rule['verifyemail']['value']}","{$user_points}");
							}
						}
					}elseif ($_CFG['operation_train_mode']=='1' && $_SESSION['utype']=='4')
					{
						$rule=get_cache('points_rule');
						if ($rule['train_verifyemail']['value']>0)
						{
							$info=$db->getone("SELECT uid FROM ".table('members_handsel')." WHERE uid ='{$_SESSION['uid']}' AND htype='verifyemail'   LIMIT 1");
							if(empty($info))
							{
							$time=time();			
							$db->query("INSERT INTO ".table('members_handsel')." (uid,htype,addtime) VALUES ('{$_SESSION['uid']}', 'verifyemail','{$time}')");
							require_once(HIGHWAY_ROOT_PATH.'include/fun_train.php');
							report_deal($_SESSION['uid'],$rule['train_verifyemail']['type'],$rule['train_verifyemail']['value']);
							$user_points=get_user_points($_SESSION['uid']);
							$operator=$rule['train_verifyemail']['type']=="1"?"+":"-";
							$_SESSION['handsel_verifyemail']=$_CFG['train_points_byname'].$operator.$rule['train_verifyemail']['value'];
							write_memberslog($_SESSION['uid'],4,9101,$_SESSION['username']," メールボックス検証済み，{$_CFG['train_points_byname']}({$operator}{$rule['train_verifyemail']['value']})，(残る:{$user_points})");
							}
						}
					}elseif ($_CFG['operation_hunter_mode']=='1' && $_SESSION['utype']=='3')
					{
						$rule=get_cache('points_rule');
						if ($rule['hunter_verifyemail']['value']>0)
						{
							$info=$db->getone("SELECT uid FROM ".table('members_handsel')." WHERE uid ='{$_SESSION['uid']}' AND htype='verifyemail'   LIMIT 1");
							if(empty($info))
							{
							$time=time();			
							$db->query("INSERT INTO ".table('members_handsel')." (uid,htype,addtime) VALUES ('{$_SESSION['uid']}', 'verifyemail','{$time}')");
							require_once(HIGHWAY_ROOT_PATH.'include/fun_hunter.php');
							report_deal($_SESSION['uid'],$rule['hunter_verifyemail']['type'],$rule['hunter_verifyemail']['value']);
							$user_points=get_user_points($_SESSION['uid']);
							$operator=$rule['hunter_verifyemail']['type']=="1"?"+":"-";
							$_SESSION['handsel_verifyemail']=$_CFG['hunter_points_byname'].$operator.$rule['hunter_verifyemail']['value'];
							write_memberslog($_SESSION['uid'],3,9201,$_SESSION['username']," メールボックス検証済み，{$_CFG['hunter_points_byname']}({$operator}{$rule['hunter_verifyemail']['value']})，(残る:{$user_points})");
							}
						}
					}

					exit("success");
			}
	}
}
?>
