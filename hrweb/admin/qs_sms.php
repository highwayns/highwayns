<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/crm_common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'send';
check_permissions($_SESSION['crm_admin_purview'],"send_sms");
$smarty->assign('pageheader',"短信营销");
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
		$link[0]['text'] = "返回上一页";
		$link[0]['href'] = "{$url}";
		crmmsg("发送失败！<strong>{$mobile}</strong> 不是标准的手机号格式",1,$link);
		
	}
	else
	{
			$r=send_sms($mobile,$txt);
			if ($r=="success")
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
