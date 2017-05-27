<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'set_sms';
check_permissions($_SESSION['admin_purview'],"set_sms");
$smarty->assign('pageheader',"ショートメッセージ設定");
if($act == 'set_sms')
{
	get_token();
	$smarty->assign('sms',get_cache('sms_config'));
	$smarty->assign('navlabel','set');
	$smarty->display('sms/admin_sms_set.htm');
}
elseif($act == 'set_save')
{
	check_token();
	header("Cache-control: private");
	foreach($_POST as $k => $v){
	!$db->query("UPDATE ".table('sms_config')." SET value='$v' WHERE name='$k'")?adminmsg('設定更新失敗', 1):"";
	}
	//填写管理员日志
	write_log("サイト設定更新", $_SESSION['admin_name'],3);
	refresh_cache('sms_config');
	adminmsg("保存成功！",2);
}
if($act == 'testing')
{
	get_token();
	$smarty->assign('navlabel','testing');
	$smarty->display('sms/admin_sms_testing.htm');
}
elseif($act == 'sms_testing')
{
	check_token();
	$txt="こんにちは！ショートメッセージモジュールのショートメッセージです。このショートメッセージ受信したら，ショートメッセージモジュール設定正い！";
	$mobile=$_POST['mobile'];
	if (!preg_match("/^(13|15|14|17|18)\d{9}$/",$mobile))
	{
	adminmsg("携帯番号入力エラー，再入力してください!",0);
	}
	if($_POST['type']==1){
		$r=captcha_send_sms($mobile,$txt);
		if ($r=="success")
		{
			//填写管理员日志
		write_log("ショートメッセージ送信成功！", $_SESSION['admin_name'],3);
		adminmsg('ショートメッセージ送信成功！',2);
		}
		else
		{
		adminmsg("ショートメッセージ送信失敗！$r",1);
		}
	}elseif($_POST['type']==2){
		$r=send_sms($mobile,$txt);
		if ($r=="success")
		{
			//填写管理员日志
		write_log("ショートメッセージ送信成功！", $_SESSION['admin_name'],3);
		adminmsg('ショートメッセージ送信成功！',2);
		}
		else
		{
		adminmsg("ショートメッセージ送信失敗！$r",1);
		}
	}elseif($_POST['type']==3){
		$r=free_send_sms($mobile,$txt);
		if ($r=="success")
		{
			//填写管理员日志
		write_log("ショートメッセージ送信成功！", $_SESSION['admin_name'],3);
		adminmsg('ショートメッセージ送信成功！',2);
		}
		else
		{
		adminmsg("ショートメッセージ送信失敗！$r",1);
		}
	}
	
}
elseif($act == 'set_tpl')
{
	get_token();
	$smarty->assign('navlabel','templates');
	$smarty->assign('mailconfig',get_cache('mailconfig'));
	$smarty->display('sms/admin_sms_templates.htm');
}
elseif($act == 'rule')
{
	get_token();
	$smarty->assign('navlabel','rule');
	$smarty->assign('sms_config',get_cache('sms_config'));
	$smarty->display('sms/admin_sms_rule.htm');
}
elseif($act == 'sms_rule_save')
{
	check_token();
	foreach($_POST as $k => $v)
	{
	!$db->query("UPDATE ".table('sms_config')." SET value='$v' WHERE name='$k'")?adminmsg('設定更新失敗', 1):"";
	}
	//填写管理员日志
	write_log("ショートメッセージ配置設定！", $_SESSION['admin_name'],3);
	refresh_cache('sms_config');
	adminmsg("保存成功！",2);
}
elseif($act == 'edit_tpl')
{
	get_token();
	$templates_name=trim($_GET['templates_name']);
	$label=array();
	$label[]=array('{sitename}','ウェブ名');
	$label[]=array('{sitedomain}','ドメイン');
	//生成标签
	if ($templates_name=='set_reg')
	{
	$label[]=array('{username}','ユーザ名');
	$label[]=array('{password}','パスワード');
	}
	elseif ($templates_name=='set_applyjobs')
	{
	$label[]=array('{personalfullname}','申込者');
	$label[]=array('{jobsname}','申し込み職位名称');
	}
	elseif ($templates_name=='set_invite')
	{
	$label[]=array('{companyname}','誘い元(会社名称)');
	}
	elseif ($templates_name=='set_order')
	{
	$label[]=array('{paymenttpye}','支払い方式');
	$label[]=array('{oid}','オーダー番号');
	$label[]=array('{amount}','金額');
	}
	elseif ($templates_name=='set_editpwd')
	{
	$label[]=array('{newpassword}','新パスワード');
	}
	elseif ($templates_name=='set_jobsallow' || $templates_name=='set_jobsnotallow')
	{
	$label[]=array('{jobsname}','職位名称');
	}
	//-end
	if ($templates_name)
	{
		$sql = "select * from ".table('sms_templates')." where name='".$templates_name."'";
		$info=$db->getone($sql);
	}
	$info['thisname']=trim($_GET['thisname']);
	$smarty->assign('info',$info);
	$smarty->assign('label',$label);
	$smarty->assign('navlabel','templates');
	$smarty->display('sms/admin_sms_templates_edit.htm');
}
elseif($act == 'templates_save')
{
	check_token();
	$templates_value=trim($_POST['templates_value']);
	$templates_name=trim($_POST['templates_name']);
	!$db->query("UPDATE ".table('sms_templates')." SET value='{$templates_value}' WHERE name='{$templates_name}'")?adminmsg('设置失敗', 1):"";
	$link[0]['text'] = "前頁に戻る";
	$link[0]['href'] ="?act=set_tpl";
	refresh_cache('sms_templates');
	//填写管理员日志
	write_log("成功テンプレート保存成功！", $_SESSION['admin_name'],3);
	adminmsg("保存成功！",2,$link);
}
elseif($act == 'send')
{
	get_token();
	$smarty->assign('pageheader',"ショートメッセージ");
	
	require_once(dirname(__FILE__).'/include/admin_smsqueue_fun.php');
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$uid=intval($_GET['uid']);
	$mobile=trim($_GET['mobile']);
	
	$wheresql=' WHERE s_uid='.$uid.' ORDER BY s_id DESC ';
	$total_sql="SELECT COUNT(*) AS num FROM ".table('smsqueue').$wheresql;
	$perpage=10;
	$page = new page(array('total'=>$db->get_total($total_sql), 'perpage'=>$perpage));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$sms_log = get_smsqueue($offset,$perpage,$wheresql);
	
	$url=trim($_REQUEST['url']);
	if (empty($url))
	{
	$url="?act=send&mobile={$mobile}&uid={$uid}";
	}
	$smarty->assign('url',$url);
	$smarty->assign('smslog',$sms_log);
	$smarty->assign('page',$page->show(3));
	$smarty->display('sms/admin_sms_send.htm');
}
elseif($act == 'sms_send')
{
	check_token();
	$txt=trim($_POST['txt']);
	$mobile=trim($_POST['mobile']);
	$uid=intval($_POST['uid']);
	$url=trim($_REQUEST['url']);
	if (!$uid)
	{
	adminmsg('ユーザUIDエラー！',0);
	}
	if (empty($txt))
	{
	adminmsg('ショートメッセージの内容をにゅうりょくしてください！',0);
	}
	if (empty($mobile))
	{
	adminmsg('携帯番号が必須！',0);
	}
	if (!preg_match("/^(13|15|14|17|18)\d{9}$/",$mobile))
	{
		$link[0]['text'] = "前頁に戻る";
		$link[0]['href'] = "{$url}";
		adminmsg("送信失敗！<strong>{$mobile}</strong> 携帯号フォーマット不正",1,$link);
		
	}
	else
	{
			$setsqlarr['s_uid']=$uid;
			$setsqlarr['s_mobile']=$mobile;
			$setsqlarr['s_body']=$txt;
			$setsqlarr['s_addtime']=time();
			$r=free_send_sms($mobile,$txt);
			if ($r=="success")
			{
				$setsqlarr['s_sendtime']=time();
				$setsqlarr['s_type']=1;//发送成功
				$db->inserttable(table('smsqueue'),$setsqlarr);
				unset($setsqlarr);
				//填写管理员日志
				write_log("ショートメッセージ送信成功！", $_SESSION['admin_name'],3);
				$link[0]['text'] = "前頁に戻る";
				$link[0]['href'] = "{$url}";
				adminmsg("送信成功！",2,$link);
			}
			else
			{
				$setsqlarr['s_sendtime']=time();
				$setsqlarr['s_type']=2;//发送失败
				$db->inserttable(table('smsqueue'),$setsqlarr);
				unset($setsqlarr);
				$link[0]['text'] = "前頁に戻る";
				$link[0]['href'] = "{$url}";
				adminmsg("送信失敗，エラー未知！",0,$link);
			}
	}
}
elseif ($act=='again_send')
{
	$id=intval($_GET['id']);
	if (empty($id))
	{
	adminmsg("送信の项目を選択してください！",1);
	}
	$result = $db->getone("SELECT * FROM ".table('smsqueue')." WHERE  s_id = {$id} limit 1");
	$wheresql=" s_id={$id} ";
	$r=free_send_sms($result['s_mobile'],$result['s_body']);
	if ($r=='success')
	{
		$setsqlarr['s_sendtime']=time();
		$setsqlarr['s_type']=1;//发送成功
		!$db->updatetable(table('smsqueue'),$setsqlarr,$wheresql);
		//填写管理员日志
		write_log("項目送信成功！", $_SESSION['admin_name'],3);
		adminmsg('送信成功',2);
	}else{
		$setsqlarr['s_sendtime']=time();
		$setsqlarr['s_type']=2;
		!$db->updatetable(table('smsqueue'),$setsqlarr,$wheresql);
		adminmsg('送信失敗',0);
	}
		
}
elseif ($act=='del')
{
	$id=$_POST['id'];
	if (empty($id))
	{
	adminmsg("項目を選択してください！",1);
	}
	if(!is_array($id)) $id=array($id);
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
	$db->query("Delete from ".table('smsqueue')." WHERE s_id IN ({$sqlin}) ");
	//填写管理员日志
	write_log("项目削除成功！", $_SESSION['admin_name'],3);
	adminmsg("削除成功",2);
	}
}
?>
