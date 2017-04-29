<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'send';
$smarty->assign('pageheader',"邮件营销");
//需要注意
check_permissions($_SESSION['admin_purview'],"send_email");
if($act == 'send')
{
	get_token();
	$url=trim($_REQUEST['url']);
	if (empty($url))
	{
	$url="?act=send";
	}
	$smarty->assign('url',$url);
	$smarty->display('mail/admin_mail_send.htm');
}
elseif($act == 'email_send')
{
	$email=trim($_POST['email']);
	$subject=trim($_POST['subject']);
	$body=trim($_POST['body']);
	$url=trim($_REQUEST['url']);
	if (empty($subject) || empty($body))
	{
	crmmsg('タイトルと内容を入力してください！',0);
	}
	if (empty($email))
	{
	crmmsg('メールを入力してください！',0);
	}
	if (!preg_match("/^[\w\-\.]+@[\w\-\.]+(\.\w+)+$/",$email))
	{
		$link[0]['text'] = "返回上一页";
		$link[0]['href'] = "{$url}";
		crmmsg("发送失败！<strong>{$mobile}</strong> 格式不正确",1,$link);
		
	}
	else
	{
			if (smtp_mail($email,$subject,$body))
			{
				$link[0]['text'] = "返回上一页";
				$link[0]['href'] = "{$url}";
				crmmsg("发送成功！",2,$link);
			}
			else
			{
				$link[0]['text'] = "返回上一页";
				$link[0]['href'] = "{$url}";
				crmmsg("发送失败，错误未知！",2,$link);
			}
	}
}
?>
