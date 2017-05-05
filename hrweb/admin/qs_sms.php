<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/crm_common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'send';
check_permissions($_SESSION['crm_admin_purview'],"send_sms");
$smarty->assign('pageheader',"ショートメッセージ");
if($act == 'send')
{
	get_token();
	//$smarty->assign('navlabel','testing');
	$url=trim($_REQUEST['url']);
	if (empty($url))
	{
	$url="?act=send";
	}
	$smarty->assign('url',$url);
	$smarty->display('sms/crm_sms_send.htm');
}
elseif($act == 'sms_send')
{
	$txt=trim($_POST['txt']);
	$mobile=trim($_POST['mobile']);
	$url=trim($_REQUEST['url']);
	if (empty($txt))
	{
	crmmsg('ショートメッセージの内容をにゅうりょくしてください！',0);
	}
	if (empty($mobile))
	{
	crmmsg('携帯番号が必須！',0);
	}
	if (!preg_match("/^(13|15|18|14)\d{9}$/",$mobile))
	{
		$link[0]['text'] = "前頁に戻る";
		$link[0]['href'] = "{$url}";
		crmmsg("送信失敗！<strong>{$mobile}</strong> 携帯号フォーマット不正",1,$link);
		
	}
	else
	{
			$r=send_sms($mobile,$txt);
			if ($r=="success")
			{
				$link[0]['text'] = "前頁に戻る";
				$link[0]['href'] = "{$url}";
				crmmsg("送信成功！",2,$link);
			}
			else
			{
				$link[0]['text'] = "前頁に戻る";
				$link[0]['href'] = "{$url}";
				crmmsg("送信失敗，エラー未知！",2,$link);
			}
	}
}
?>
