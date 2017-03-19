<?php
/*
 * 74cms 企业会员中心
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/company_common.php');
$smarty->assign('leftmenu',"user");
if ($act=='binding')
{
	$smarty->assign('user',$user);
	$smarty->assign('title','账号绑定 - 会员中心 - '.$_CFG['site_name']);
	$smarty->display('member_company/company_binding.htm');
}
elseif ($act=='pm')
{
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$perpage=10;
	$uid=intval($_SESSION['uid']);
	$wheresql=" WHERE (p.msgfromuid='{$uid}' OR p.msgtouid='{$uid}') ";
	$joinsql=" LEFT JOIN  ".table('members')." AS i  ON  p.msgfromuid=i.uid ";
	$orderby=" order by p.pmid desc";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('pms').' AS p '.$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$sql="SELECT p.* FROM ".table('pms').' AS p'.$joinsql.$wheresql.$orderby;
	//获取所查看消息的pmid , 并且将其修改为已读
	$pmid = update_pms_read($offset, $perpage,$sql);
	if(!empty($pmid))
	{
		$db->query("UPDATE ".table('pms')." SET `new`='2' WHERE new=1 AND msgtouid='{$uid}' and pmid in (".$pmid.")");	
	}
	else
	{
		$db->query("UPDATE ".table('pms')." SET `new`='2' WHERE new=1 AND msgtouid='{$uid}'");
	}
	get_pms_no_num();
	$smarty->assign('pms',get_pms($offset,$perpage,$sql));
	$smarty->assign('total',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='1'"));
	$smarty->assign('title','短消息 - 会员中心 - '.$_CFG['site_name']);	
	$smarty->assign('page',$page->show(3));
	$smarty->assign('uid',$uid); 

	$smarty->display('member_company/company_user_pm.htm');
}
elseif ($act=='pm_del')
{
	$pmid=intval($_GET['pmid']);
	$uid=intval($_SESSION['uid']);
	$pms= $db->getone("select * from ".table('pms')." where pmid = '{$pmid}' AND (msgfromuid='{$uid}' OR msgtouid='{$uid}') LIMIT 1");
	if (!empty($pms))
	{
	$db->query("Delete from ".table('pms')." WHERE pmid='{$pms['pmid']}'");
	}
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = "?act=pm&msgtype={$_GET['msgtype']}&new={$_GET['new']}";
	//统计消息
	$pmscount=$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$_SESSION['uid']}' OR msgtouid='{$_SESSION['uid']}') AND `new`='1' AND `replyuid`<>'{$_SESSION['uid']}'");
	setcookie('QS[pmscount]',$pmscount, $expire,$QS_cookiepath,$QS_cookiedomain);
	showmsg("操作成功！",2,$link);
}
elseif ($act=='authenticate')
{
	$uid = intval($_SESSION['uid']);
	$smarty->assign('user',$user);
	$smarty->assign('re_audit',$_GET['re_audit']);
	$smarty->assign('title','认证管理 - 企业会员中心 - '.$_CFG['site_name']);
	$_SESSION['send_key']=mt_rand(100000, 999999);
	$smarty->assign('send_key',$_SESSION['send_key']);
	/**
	 * 微信扫描绑定start
	 */
    if(intval($_CFG['weixin_apiopen'])==1 && intval($_CFG['weixin_scan_bind'])==1 && !$user['weixin_openid']){
	    $scene_id = mt_rand(20000001,30000000);
	    $_SESSION['scene_id'] = $scene_id;
		$dir = QISHI_ROOT_PATH.'data/weixin/'.($scene_id%10);
		make_dir($dir);
	    $fp = @fopen($dir.'/'.$scene_id.'.txt', 'wb+');
		$access_token = get_access_token();
	    $post_data = '{"expire_seconds": 1800, "action_name": "QR_SCENE", "action_info": {"scene": {"scene_id": '.$scene_id.'}}}';
	    $url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=".$access_token;
	    $result = https_request($url, $post_data);
	    $result_arr = json_decode($result,true);
	    $ticket = urlencode($result_arr["ticket"]);
	    $html = '<img width="240" height="240" src="https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket='.$ticket.'">';
		$smarty->assign('qrcode_img',$html);
	}else{
		$smarty->assign('qrcode_img','');
	}
    /**
     * 微信扫描绑定end
     */
	$smarty->display('member_company/company_authenticate.htm');
}
//修改密码
elseif ($act=='password_edit')
{
	$smarty->assign('title','修改密码 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->display('member_company/company_password.htm');
}
//保存修改密码
elseif ($act=='save_password')
{
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$arr['username']=$_SESSION['username'];
	$arr['oldpassword']=trim($_POST['oldpassword'])?trim($_POST['oldpassword']):showmsg('请输入旧密码！',1);
	$arr['password']=trim($_POST['password'])?trim($_POST['password']):showmsg('请输入新密码！',1);
	if ($arr['password']!=trim($_POST['password1'])) showmsg('两次输入密码不相同，请重新输入！',1);
	$info=edit_password($arr);
	if ($info==-1) showmsg('旧密码输入错误，请重新输入！',1);
	if ($info==$_SESSION['username']){
			//sendemail
			$mailconfig=get_cache('mailconfig');
			if ($mailconfig['set_editpwd']=="1" && $user['email_audit']=="1")
			{
			dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid={$_SESSION['uid']}&key=".asyn_userkey($_SESSION['uid'])."&act=set_editpwd&newpassword={$arr['password']}");
			}
			//sendemail
			//sms
			$sms=get_cache('sms_config');
			if ($sms['open']=="1" && $sms['set_editpwd']=="1"  && $user['mobile_audit']=="1")
			{
				dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_sms.php?uid={$_SESSION['uid']}&key=".asyn_userkey($_SESSION['uid'])."&act=set_editpwd&newpassword={$arr['password']}");
			}
			showmsg('密码修改成功！',2);
	}
}
//保存修改用户名
elseif ($act=='save_username')
{
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$arr['uid']=$_SESSION['uid'];
	$_POST['newusername'] = utf8_to_gbk($_POST['newusername']);
	$arr['newusername']=trim($_POST['newusername'])?trim($_POST['newusername']):showmsg('新用户名！',1);
	$row_newname = $db->getone("SELECT * FROM ".table('members')." WHERE username='{$arr['newusername']}' LIMIT 1");
	if($row_newname)
	{
		exit("-1");
	}
	$info=edit_username($arr);
	if ($info==-1) exit("-2");
	if (!$info) exit("-3");
	exit("1");
}
elseif ($act=='del_qq_binding')
{
	$db->query("UPDATE ".table('members')." SET qq_openid = ''  WHERE uid='{$_SESSION[uid]}' LIMIT 1");
	exit('解除腾讯QQ绑定成功！');
}
elseif ($act=='del_sina_binding')
{
	$db->query("UPDATE ".table('members')." SET sina_access_token = ''  WHERE uid='{$_SESSION[uid]}' LIMIT 1");
	exit('解除新浪微博绑定成功！');
}
elseif ($act=='del_taobao_binding')
{
	$db->query("UPDATE ".table('members')." SET taobao_access_token = ''  WHERE uid='{$_SESSION[uid]}' LIMIT 1");
	exit('解除淘宝账号绑定成功！');
}

//会员登录日志
elseif ($act=='login_log')
{
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$wheresql=" WHERE log_uid='{$_SESSION['uid']}' AND log_type='1001' ";
	$settr=intval($_GET['settr']);
	if($settr>0)
	{
	$settr_val=strtotime("-".$settr." day");
	$wheresql.=" AND log_addtime >".$settr_val;
	}
	$perpage=15;
	$total_sql="SELECT COUNT(*) AS num FROM ".table('members_log').$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$smarty->assign('loginlog',get_user_loginlog($offset, $perpage,$wheresql));
	$smarty->assign('page',$page->show(3));
	$smarty->assign('title','会员登录日志 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->display('member_company/company_user_loginlog.htm');
}

unset($smarty);
?>