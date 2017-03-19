<?php
/*
 * 74cms 个人会员中心
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__) . '/personal_common.php');
$smarty->assign('leftmenu',"user");
if ($act=='binding')
{
	$smarty->assign('user',$user);
	$smarty->assign('title','账号绑定 - 会员中心 - '.$_CFG['site_name']);
	$smarty->display('member_personal/personal_binding.htm');
}
elseif ($act=='userprofile')
{
	$_SESSION['send_mobile_key']=mt_rand(100000, 999999);
	$_SESSION['send_email_key']=mt_rand(100000, 999999);
	$uid = intval($_SESSION['uid']);
	$smarty->assign('total',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='1'"));
	$smarty->assign('send_mobile_key',$_SESSION['send_mobile_key']);
	$smarty->assign('send_email_key',$_SESSION['send_email_key']);
	$smarty->assign('user',$user);
	$smarty->assign('title','个人资料 - 会员中心 - '.$_CFG['site_name']);
	$smarty->assign('userprofile',get_userprofile($_SESSION['uid']));
	// 新注册会员 邮箱调取注册邮箱
	$smarty->assign('user',$user);
	$smarty->display('member_personal/personal_userprofile.htm');
}
elseif ($act=='userprofile_save')
{
	$setsqlarr['uid']=intval($_SESSION['uid']);
	$setsqlarr['email']=trim($_POST['email'])?trim($_POST['email']):showmsg('请填写邮箱！',1);
	if($user['email_audit']!="1")
	{
		$members['email']=$setsqlarr['email'];
		$resume['email']=$setsqlarr['email'];
		$db->updatetable(table("members"),$members,array("uid"=>intval($_SESSION['uid'])));
		$db->updatetable(table("resume"),$resume,array("uid"=>intval($_SESSION['uid'])));
		unset($members['email'],$resume['email']);
	}
	$setsqlarr['phone']=trim($_POST['mobile'])?trim($_POST['mobile']):showmsg('请填写手机号！',1);
	if($user['mobile_audit']!="1")
	{
		$members['mobile']=$setsqlarr['phone'];
		$resume['telephone']=$setsqlarr['phone'];
		$db->updatetable(table("members"),$members,array("uid"=>intval($_SESSION['uid'])));
		$db->updatetable(table("resume"),$resume,array("uid"=>intval($_SESSION['uid'])));
		unset($members['mobile'],$resume['telephone']);
	}
	$setsqlarr['realname']=trim($_POST['realname'])?trim($_POST['realname']):showmsg('请填写真实姓名！',1);
	$setsqlarr['sex']=intval($_POST['sex'])?intval($_POST['sex']):showmsg('请选择性别！',1);
	$setsqlarr['sex_cn']=trim($_POST['sex_cn']);
	$setsqlarr['birthday']=intval($_POST['birthday'])?intval($_POST['birthday']):showmsg('请选择出生年份',1);
	$setsqlarr['residence']=trim($_POST['residence'])?trim($_POST['residence']):showmsg('请填写现居住地！',1);
	$setsqlarr['education']=intval($_POST['education'])?intval($_POST['education']):showmsg('请选择学历',1);
	$setsqlarr['education_cn']=trim($_POST['education_cn']);
	$setsqlarr['major']=intval($_POST['major'])?intval($_POST['major']):showmsg('请选择专业',1);
	$setsqlarr['major_cn']=trim($_POST['major_cn']);
	$setsqlarr['experience']=intval($_POST['experience'])?intval($_POST['experience']):showmsg('请选择工作经验',1);
	$setsqlarr['experience_cn']=trim($_POST['experience_cn']);
	$setsqlarr['height']=intval($_POST['height']);
	$setsqlarr['householdaddress']=trim($_POST['householdaddress']);
	$setsqlarr['marriage']=intval($_POST['marriage']);
	$setsqlarr['marriage_cn']=trim($_POST['marriage_cn']);
	if (get_userprofile($_SESSION['uid']))
	{
	$wheresql=" uid='".intval($_SESSION['uid'])."'";
	write_memberslog($_SESSION['uid'],2,1005,$_SESSION['username'],"修改了个人资料");
	!$db->updatetable(table('members_info'),$setsqlarr,$wheresql)?showmsg("修改失败！",0):showmsg("修改成功！",2);
	}
	else
	{
	$setsqlarr['uid']=intval($_SESSION['uid']);
	write_memberslog($_SESSION['uid'],2,1005,$_SESSION['username'],"修改了个人资料");
	!$db->inserttable(table('members_info'),$setsqlarr)?showmsg("修改失败！",0):showmsg("修改成功！",2);
	}
}
//头像
elseif ($act=='avatars')
{
	$uid = intval($_SESSION['uid']);
	$smarty->assign('total',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='1'"));
	$smarty->assign('title','个人头像 - 会员中心 - '.$_CFG['site_name']);
	$smarty->assign('user',$user);
	$smarty->assign('rand',rand(1,100));
	$smarty->display('member_personal/personal_avatars.htm');
}
elseif ($act=='avatars_ready')
{
	require_once(QISHI_ROOT_PATH.'include/cut_upload.php');
	!$_FILES['avatars']['name']?showmsg('请上传图片！',1):"";
	$up_dir_original="../../data/avatar/original/";
	$up_dir_100="../../data/avatar/100/";
	$up_dir_48="../../data/avatar/48/";
	$up_dir_thumb="../../data/avatar/thumb/";
	make_dir($up_dir_original.date("Y/m/d/"));
	make_dir($up_dir_100.date("Y/m/d/"));
	make_dir($up_dir_48.date("Y/m/d/"));
	make_dir($up_dir_thumb.date("Y/m/d/"));
	$setsqlarr['avatars']=_asUpFiles($up_dir_original.date("Y/m/d/"), "avatars",500,'gif/jpg/bmp/png',true);
	$setsqlarr['avatars']=date("Y/m/d/").$setsqlarr['avatars'];
	if ($setsqlarr['avatars'])
	{
		
	makethumb($up_dir_original.$setsqlarr['avatars'],$up_dir_thumb.date("Y/m/d/"),445,300);
	// makethumb($up_dir_original.$setsqlarr['avatars'],$up_dir_100.date("Y/m/d/"),100,100);
	// makethumb($up_dir_original.$setsqlarr['avatars'],$up_dir_48.date("Y/m/d/"),48,48);
	$wheresql=" uid='".$_SESSION['uid']."'";
	write_memberslog($_SESSION['uid'],2,1006 ,$_SESSION['username'],"修改了个人头像");
	$db->updatetable(table('members'),$setsqlarr,$wheresql)?exit($setsqlarr['avatars']):showmsg('保存失败！',1);
	}
	else
	{
	showmsg('保存失败！',1);
	}
}
elseif ($act=='avatars_save')
{	
	$savePath = "../../data/avatar/100/";  //图片存储路径
	$savePathThumb = "../../data/avatar/48/";  //图片存储路径
	$savePicName = time();//图片存储名称
	$file_src = $savePath.$savePicName."_src.jpg";
	$filename150 = $savePath.$savePicName.".jpg"; 
	$filename50 = $savePathThumb.$savePicName.".jpg"; 
	$src=base64_decode($_POST['pic']);
	$pic1=base64_decode($_POST['pic1']);   
	$pic2=base64_decode($_POST['pic2']);
	if($src) {
		file_put_contents($file_src,$src);
	}
	file_put_contents($filename150,$pic1);
	if($pic2)file_put_contents($filename50,$pic2);
	$rs['status'] = 1;
	$rs['picUrl'] = $savePicName.".jpg";
	$setsqlarr['avatars']=$savePicName.".jpg";
	$wheresql=" uid='".$_SESSION['uid']."'";
	$db->updatetable(table('members'),$setsqlarr,$wheresql)?print json_encode($rs):showmsg('保存失败！',1);
	write_memberslog($_SESSION['uid'],2,1006 ,$_SESSION['username'],"修改了个人头像");
}
//修改密码
elseif ($act=='password_edit')
{
	$uid = intval($_SESSION['uid']);
	$smarty->assign('total',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='1'"));
	$smarty->assign('title','修改密码 - 个人会员中心 - '.$_CFG['site_name']);
	$smarty->display('member_personal/personal_password.htm');
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
			//发送邮件
			$mailconfig=get_cache('mailconfig');
			if ($mailconfig['set_editpwd']=="1" && $user['email_audit']=="1")
			{
			dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid=".$_SESSION['uid']."&key=".asyn_userkey($_SESSION['uid'])."&act=set_editpwd&newpassword=".$arr['password']);
			}
			//邮件发送完毕
			//sms
			$sms=get_cache('sms_config');
			if ($sms['open']=="1" && $sms['set_editpwd']=="1"  && $user['mobile_audit']=="1")
			{
			dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_sms.php?uid=".$_SESSION['uid']."&key=".asyn_userkey($_SESSION['uid'])."&act=set_editpwd&newpassword=".$arr['password']);
			}
	 write_memberslog($_SESSION['uid'],2,1004 ,$_SESSION['username'],"修改密码");
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
elseif ($act=='authenticate')
{
	$uid = intval($_SESSION['uid']);
	$smarty->assign('total',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='1'"));
	$smarty->assign('user',$user);
	$smarty->assign('re_audit',$_GET['re_audit']);
	$smarty->assign('title','验证邮箱 - 个人会员中心 - '.$_CFG['site_name']);
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
	$smarty->display('member_personal/personal_authenticate.htm');
}
elseif ($act=='feedback')
{
	$smarty->assign('title','用户反馈 - 个人会员中心 - '.$_CFG['site_name']);
	$smarty->assign('feedback',get_feedback($_SESSION['uid']));
	$smarty->display('member_personal/personal_feedback.htm');
}
//保存用户反馈
elseif ($act=='feedback_save')
{
	$get_feedback=get_feedback($_SESSION['uid']);
	if (count($get_feedback)>=5) 
	{
	showmsg('反馈信息不能超过5条！',1);
	exit();
	}
	$setsqlarr['infotype']=intval($_POST['infotype']);
	$setsqlarr['feedback']=trim($_POST['feedback'])?trim($_POST['feedback']):showmsg('请填写内容！',1);
	$setsqlarr['uid']=$_SESSION['uid'];
	$setsqlarr['usertype']=$_SESSION['utype'];
	$setsqlarr['username']=$_SESSION['username'];
	$setsqlarr['addtime']=$timestamp;
	write_memberslog($_SESSION['uid'],2,7001,$_SESSION['username'],"添加反馈信息");
	!$db->inserttable(table('feedback'),$setsqlarr)?showmsg("添加失败！",0):showmsg("添加成功，请等待管理员回复！",2);
}
//删除用户反馈
elseif ($act=='del_feedback')
{
	$id=intval($_GET['id']);
	del_feedback($id,$_SESSION['uid'])?showmsg('删除成功！',2):showmsg('删除失败！',1);
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
	$smarty->display('member_personal/personal_user_pm.htm');
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
	$link[0]['href'] = "?act=pm";
	//统计消息
	$pmscount=$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$_SESSION['uid']}' OR msgtouid='{$_SESSION['uid']}') AND `new`='1' AND `replyuid`<>'{$_SESSION['uid']}'");
	setcookie('QS[pmscount]',$pmscount, $expire,$QS_cookiepath,$QS_cookiedomain);
	showmsg("操作成功！",2,$link);
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
	$uid = intval($_SESSION['uid']);
	$smarty->assign('total',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='1'"));
	$wheresql=" WHERE log_uid='{$_SESSION['uid']}' AND log_type='1001' ";
	$perpage=15;
	$total_sql="SELECT COUNT(*) AS num FROM ".table('members_log').$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$smarty->assign('loginlog',get_user_loginlog($offset, $perpage,$wheresql));
	$smarty->assign('page',$page->show(3));
	$smarty->assign('title','会员登录日志 - 企业会员中心 - '.$_CFG['site_name']);
	$smarty->display('member_personal/personal_user_loginlog.htm');
}elseif($act == 'demo'){
	require_once(QISHI_ROOT_PATH.'include/fun_user.php'); 
	echo '<pre>';
	print_r(get_ip_area('113.25.9.112'));
	echo '</pre>';
}
unset($smarty);
?>