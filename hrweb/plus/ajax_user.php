<?php
 /*
 * 74cms ajax返回
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(dirname(__FILE__)).'/include/plus.common.inc.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : '';
if($act =='do_login')
{
	$username=isset($_POST['username'])?trim($_POST['username']):"";
	$password=isset($_POST['password'])?trim($_POST['password']):"";
	$expire=isset($_POST['expire'])?intval($_POST['expire']):"";
	$index_login=isset($_POST['index_login'])?intval($_POST['index_login']):0;//标记登录来源，首页不要验证码
	$account_type=1;
	if (preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$username))
	{
	$account_type=2;
	}
	elseif (preg_match("/^(13|14|15|18|17)\d{9}$/",$username))
	{
	$account_type=3;
	}
	$url=isset($_POST['url'])?$_POST['url']:"";
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
	$username=utf8_to_gbk($username);
	$password=utf8_to_gbk($password);
	}
	$captcha=get_cache('captcha');
	if ($captcha['verify_userlogin']=="1" && $index_login!="1")
	{
		$postcaptcha=$_POST['postcaptcha'];
		if ($captcha['captcha_lang']=="cn" && strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
		{
		$postcaptcha=utf8_to_gbk($postcaptcha);
		}
		if (empty($postcaptcha) || empty($_SESSION['imageCaptcha_content']) || strcasecmp($_SESSION['imageCaptcha_content'],$postcaptcha)!=0)
		{
		unset($_SESSION['imageCaptcha_content']);
		exit("errcaptcha");
		}
	}
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	if ($username && $password)
	{
		$login=user_login($username,$password,$account_type,true,$expire);
		$url=$url?$url:$login['qs_login'];
		if ($login['qs_login'])
		{
			if($login['qs_login'] == 'false'){
				exit('status_err');
			}else{
				if(intval($_POST['binding']) == 1 && trim($_POST['openid'])<>'' && $_SESSION['uid']<>'' && trim($_POST['nickname'])<>''){
					$time = time();
					$nickname = utf8_to_gbk($_POST['nickname']);
					switch($_POST['bindtype']){
						case 'qq':
							$db->query("UPDATE ".table('members')." SET qq_openid = '{$_SESSION[openid]}', qq_nick = '{$nickname}', qq_binding_time = '{$time}' WHERE uid='".$_SESSION['uid']."' AND qq_openid='' LIMIT 1");
							break;
						case 'taobao':
							$db->query("UPDATE ".table('members')." SET taobao_access_token = '{$_SESSION[taobao_access_token]}', taobao_nick = '{$nickname}', taobao_binding_time = '{$time}' WHERE uid='".$_SESSION['uid']."' AND taobao_access_token='' LIMIT 1");
							break;
						case 'sina':
							$db->query("UPDATE ".table('members')." SET sina_access_token = '{$_SESSION[sina_access_token]}', sina_nick = '{$nickname}', sina_binding_time = '{$time}' WHERE uid='".$_SESSION['uid']."' AND sina_access_token='' LIMIT 1");
							break;
					}
					
				}
				exit($login['uc_login']."<script language=\"javascript\" type=\"text/javascript\">window.location.href=\"".$url."\";</script>");
			} 
		}
		else
		{
		exit("err");
		}
	}
	exit("err");
}
elseif ($act=='do_reg')
{
	$captcha=get_cache('captcha');
	if ($captcha['verify_userreg']=="1")
	{
		$postcaptcha=$_POST['postcaptcha'];
		if ($captcha['captcha_lang']=="cn" && strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
		{
		$postcaptcha=utf8_to_gbk($postcaptcha);
		}
		if (empty($postcaptcha) || empty($_SESSION['imageCaptcha_content']) || strcasecmp($_SESSION['imageCaptcha_content'],$postcaptcha)!=0)
		{
		exit("err");
		}
	}
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$username = isset($_POST['username'])?trim($_POST['username']):exit("err");
	$password = isset($_POST['password'])?trim($_POST['password']):exit("err");
	$member_type = isset($_POST['member_type'])?intval($_POST['member_type']):exit("err");
	$email = isset($_POST['email'])?trim($_POST['email']):exit("err");
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
	$username=utf8_to_gbk($username);
	$password=utf8_to_gbk($password);
	}
	$register=user_register($username,$password,$member_type,$email);
	if ($register>0)
	{	
		$login_js=user_login($username,$password);
		$mailconfig=get_cache('mailconfig');
		if ($mailconfig['set_reg']=="1")
		{
		dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid=".$_SESSION['uid']."&key=".asyn_userkey($_SESSION['uid'])."&sendemail=".$email."&sendusername=".$username."&sendpassword=".$password."&act=reg");
		}
		$qsurl=$login_js['qs_login'];
		$qsjs="<script language=\"javascript\" type=\"text/javascript\">window.location.href=\"".$qsurl."\";</script>";
		 if ($ucjs || $qsurl)
			{
			exit($ucjs.$qsjs);
			}
			else
			{
			exit("err");
			}
	}
	else
	{
	exit("err");
	}
}
elseif($act =='check_usname')
{
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$usname=trim($_POST['usname']);
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
	$usname=utf8_to_gbk($usname);
	}
	$user=get_user_inusername($usname);
	empty($user)?exit("true"):exit("false");
}
elseif($act == 'check_email')
{
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$email=trim($_POST['email']);
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
	$email=utf8_to_gbk($email);
	}
	$user=get_user_inemail($email);
	empty($user)?exit("true"):exit("false");
}
elseif($act == "check_mobile")
{
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$mobile=trim($_POST['mobile']);
	if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
	$mobile=utf8_to_gbk($mobile);
	}
	$user=get_user_inmobile($mobile);
	empty($user)?exit("true"):exit("false");
}
elseif ($act=="top_loginform")
{
	$block = isset($_GET['block'])?iconv('utf-8',QISHI_CHARSET,$_GET['block']):'';
	$contents='';
	if ($_COOKIE['QS']['username'] && $_COOKIE['QS']['password'])
	{
		$contents='欢迎&nbsp;&nbsp;<a href="{#$user_url#}" style="color:#339900">{#$username#}</a> 登录！&nbsp;&nbsp;{#$pmscount_a#}&nbsp;&nbsp;&nbsp;&nbsp;<a href="{#$user_url#}" style="color:#0066cc">[会员中心]</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="{#$logout_url#}" style="color:#0066cc">[退出]</a>';
	}
	elseif ($_SESSION['activate_username'] && defined('UC_API'))
	{
		$contents=' &nbsp;&nbsp;您的帐号 {#$activate_username#} 需激活后才可以使用！ <a href="{#$activate_url#}" style="color:#339900">立即激活</a>';
	}
	else
	{	
		if (isset($_GET['block'])) {
			$contents='您好，欢迎访问{#$site_name#}'.$block.'！';
		} else {
			$contents='欢迎来到{#$site_name#}！&nbsp;&nbsp;&nbsp;&nbsp;<a href="{#$login_url#}" style="color:#0066cc" >[登录]</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="{#$reg_url#}" style="color:#0066cc">[免费注册]</a>';
		}
	}
		$contents=str_replace('{#$activate_username#}',$_SESSION['activate_username'],$contents);
		$contents=str_replace('{#$site_name#}',$_CFG['site_name'],$contents);
		$contents=str_replace('{#$username#}',$_COOKIE['QS']['username'],$contents);
		$contents=str_replace('{#$pmscount#}',$_COOKIE['QS']['pmscount'],$contents);
		$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
		if ($_COOKIE['QS']['utype']=='1')
		{
		$user_url=$_CFG['site_dir']."user/company/company_index.php";
			if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/company/company_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="短消息">消息 '.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		if ($_COOKIE['QS']['utype']=='2')
		{
			$user_url=$_CFG['site_dir']."user/personal/personal_index.php";
			if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/personal/personal_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="短消息">消息 '.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		if ($_COOKIE['QS']['utype']=='4')
		{
			$user_url=$_CFG['site_dir']."user/train/train_index.php";
			if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/train/train_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="短消息">消息 '.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		if ($_COOKIE['QS']['utype']=='3')
		{
			$user_url=$_CFG['site_dir']."user/hunter/hunter_index.php";
			if($_COOKIE['QS']['pmscount']>0)
			 {
			 $pmscount_a='<a href="'.$_CFG['site_dir'].'user/hunter/hunter_user.php?act=pm&new=1" style="padding:1px 4px; background-color:#FF6600; color:#FFFFFF;text-decoration:none" title="短消息">消息 '.$_COOKIE['QS']['pmscount'].'</a>';
			 }
		}
		$contents=str_replace('{#$pmscount_a#}',$pmscount_a,$contents);
		$contents=str_replace('{#$user_url#}',$user_url,$contents);
		$contents=str_replace('{#$login_url#}',url_rewrite('QS_login'),$contents);
		$contents=str_replace('{#$logout_url#}',url_rewrite('QS_login')."?act=logout",$contents);
		$contents=str_replace('{#$reg_url#}',$_CFG['site_dir']."user/user_reg.php",$contents);
		$contents=str_replace('{#$activate_url#}',$_CFG['site_dir']."user/user_reg.php?act=activate",$contents);
		exit($contents);
}
elseif ($act=="loginform")
{		
		$contents='';
		if ($_COOKIE['QS']['username'] && $_COOKIE['QS']['password'] && $_SESSION['uid'])
		{
			$tpl='../templates/'.$_CFG['template_dir']."plus/login_success.htm";
		}
		elseif ($_SESSION['activate_username'] && defined('UC_API'))
		{
			$tpl='../templates/'.$_CFG['template_dir']."plus/login_activate.htm";
		}
		else
		{
			$tpl='../templates/'.$_CFG['template_dir']."plus/login_form.htm";
		}
		$contents=file_get_contents($tpl);
		$contents=str_replace('{#$site_name#}',$_CFG['site_name'],$contents);
		if($_CFG['qq_apiopen']==1)
		{
			if($_CFG['qq_logintype']==1)
			{

				$contents=str_replace('{#$qq_apiopen#}','<a href="'.$_CFG['site_dir'].'user/connect_qq_client.php" class="third-icon qq f-left"></a>',$contents);
			}
			else
			{
				$contents=str_replace('{#$qq_apiopen#}','<a href="'.$_CFG['site_dir'].'user/connect_qq_server.php" class="third-icon qq f-left"></a>',$contents);
			}
		}
		else
		{
			$contents=str_replace('{#$qq_apiopen#}','',$contents);
		}
		if($_CFG['sina_apiopen']==1)
		{
			$contents=str_replace('{#$sina_apiopen#}','<a href="'.$_CFG['site_dir'].'user/connect_sina.php" class="third-icon sina f-left"></a>',$contents);
		}
		else
		{
			$contents=str_replace('{#$sina_apiopen#}','',$contents);
		}
		if($_CFG['taobao_apiopen']==1)
		{
			$contents=str_replace('{#$taobao_apiopen#}','<a href="'.$_CFG['site_dir'].'user/connect_taobao.php" class="third-icon taobao f-left"></a>',$contents);
		}
		else
		{
			$contents=str_replace('{#$taobao_apiopen#}','',$contents);
		}
		if ($_CFG['qq_apiopen']==1 || $_CFG['sina_apiopen']==1 || $_CFG['taobao_apiopen']==1)
		{
			
			$contents=str_replace('{#$third_tit#}',"其他账户登录：",$contents);
		}
		else
		{
			$contents=str_replace('{#$third_tit#}',"",$contents);
		}
		$contents=str_replace('{#$site_name#}',$_CFG['site_name'],$contents);
		$contents=str_replace('{#$username#}',$_COOKIE['QS']['username'],$contents);
		$contents=str_replace('{#$pmscount#}',$_COOKIE['QS']['pmscount'],$contents);
		$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
		$contents=str_replace('{#$site_dir#}',$_CFG['site_dir'],$contents);
		$contents=str_replace('{#$site_tel#}',$_CFG['top_tel'],$contents);
		if($_CFG['weixin_apiopen']=='1' && $_CFG['weixin_scan_login']=='1'){
			$access_token = get_access_token();
		    $scene_id = rand(1,10000000);
		    $_SESSION['scene_id'] = $scene_id;
			$dir = QISHI_ROOT_PATH.'data/weixin/'.($scene_id%10);
			make_dir($dir);
		    $fp = @fopen($dir.'/'.$scene_id.'.txt', 'wb+');
		    $post_data = '{"expire_seconds": 1800, "action_name": "QR_SCENE", "action_info": {"scene": {"scene_id": '.$scene_id.'}}}';
		    $url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=".$access_token;
		    $result = https_request($url, $post_data);
		    $result_arr = json_decode($result,true);
		    $ticket = urlencode($result_arr["ticket"]);
		    $img_html = '<img width="120" height="120" src="https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket='.$ticket.'">';
			$weixin_html = '<div id="codeLogin"><div class="code-login" id="login_container">'.$img_html.'</div><p>打开微信扫描二维码</p></div>';
			$weixin_html_header='<div class="wechat-login"><a href="javascript:;" class="loginicon wx"></a></div>';
		}else{
			$weixin_html_header='';
			$weixin_html = '';
		}
		$contents=str_replace('{#$weixin_html_header#}',$weixin_html_header,$contents);
		$contents=str_replace('{#$weixin_html#}',$weixin_html,$contents);
		// 上一次登录时间
		if($_SESSION['username'])
		{
			if($_SESSION['no_self'])
			{
				$log=$db->getone("SELECT * FROM ".table('members_log')." WHERE log_uid=$_SESSION[uid] AND log_type='1001' ORDER BY log_id DESC LIMIT 0,1");
				if(!empty($log))
				{
					$last_login_time='上次登录：'.date("Y-m-d H:i",$log['log_addtime']);
				}
				else
				{
					$log=$db->getone("SELECT reg_time from ".table("members")." where uid=$_SESSION[uid]");
					$last_login_time='注册时间：'.date("Y-m-d H:i",$log['reg_time']);
				}
				
			}
			else
			{
				$log=$db->getone("SELECT * FROM ".table('members_log')." WHERE log_uid=$_SESSION[uid] AND log_type='1001' ORDER BY log_id DESC LIMIT 1,1");
				if(!empty($log))
				{
					$last_login_time='上次登录：'.date("Y-m-d H:i",$log['log_addtime']);
				}
				else
				{
					$log=$db->getone("SELECT reg_time from ".table("members")." where uid=$_SESSION[uid]");
					$last_login_time='注册时间：'.date("Y-m-d H:i",$log['reg_time']);
				}
			}
		}

		$contents=str_replace('{#$last_login_time#}',$last_login_time,$contents);
		if ($_COOKIE['QS']['utype']=='1' && $_SESSION['utype']=='1')
		{
			$user_url=$_CFG['site_dir']."user/company/company_index.php";
			$interview=$db->get_total("SELECT COUNT(*) AS num FROM ".table('company_interview')." as i  INNER JOIN ".table('resume')." as r ON i.resume_id=r.id   WHERE i.company_uid='{$_SESSION['uid']}'");
			$count_receivedresume=$db->get_total("SELECT COUNT(*) AS num FROM ".table('personal_jobs_apply')." AS a INNER JOIN  ".table('resume')." AS r  ON  a.resume_id=r.id  WHERE a.company_uid='{$_SESSION['uid']}'");
			$count_favorites=$db->get_total("SELECT COUNT(*) AS num FROM ".table('company_favorites')." AS f INNER JOIN  ".table('resume')." AS r ON  f.resume_id=r.id   WHERE f.company_uid='{$_SESSION['uid']}'");

			$count_htm='<div code="'.$_CFG['site_dir'].'user/company/company_recruitment.php?act=interview_list" class="core-function f-left">
					<div><a href="'.$_CFG['site_dir'].'user/company/company_recruitment.php?act=interview_list">'.intval($interview).'</a></div>
					<p><a href="'.$_CFG['site_dir'].'user/company/company_recruitment.php?act=interview_list">面试邀请</a></p>
				</div>
				<div code="'.$_CFG['site_dir'].'user/company/company_recruitment.php?act=apply_jobs" class="core-function f-left">
					<div><a href="'.$_CFG['site_dir'].'user/company/company_recruitment.php?act=apply_jobs">'.intval($count_receivedresume).'</a></div>
					<p><a href="'.$_CFG['site_dir'].'user/company/company_recruitment.php?act=apply_jobs">收到的简历</a></p>
				</div>
				<div code="'.$_CFG['site_dir'].'user/company/company_recruitment.php?act=favorites_list" class="core-function last f-left">
					<div><a href="'.$_CFG['site_dir'].'user/company/company_recruitment.php?act=favorites_list">'.intval($count_favorites).'</a></div>
					<p><a href="'.$_CFG['site_dir'].'user/company/company_recruitment.php?act=favorites_list">收藏夹</a></p>
				</div>';
			$contents=str_replace('{#$count#}',$count_htm,$contents);

		}
		if ($_COOKIE['QS']['utype']=='2' && $_SESSION['utype']=='2')
		{
			$user_url=$_CFG['site_dir']."user/personal/personal_index.php";
			$interview=$db->get_total("SELECT COUNT(*) AS num FROM ".table('company_interview')." as i WHERE i.resume_uid='{$_SESSION['uid']}'");

			$count_apply=$db->get_total("SELECT COUNT(*) AS num FROM ".table('personal_jobs_apply')." AS a  WHERE a.personal_uid='{$_SESSION['uid']}'");
			$fav_num = $db->get_total("SELECT COUNT(*) AS num FROM ".table('personal_favorites')." WHERE personal_uid='{$_SESSION['uid']}' ");

			$count_htm='<div class="core-function f-left">
					<div><a href="'.$_CFG['site_dir'].'user/personal/personal_apply.php?act=interview">'.intval($interview).'</a></div>
					<p><a href="'.$_CFG['site_dir'].'user/personal/personal_apply.php?act=interview">面试邀请</a></p>
				</div>
				<div class="core-function f-left">
					<div><a href="'.$_CFG['site_dir'].'user/personal/personal_apply.php?act=apply_jobs">'.intval($count_apply).'</a></div>
					<p><a href="'.$_CFG['site_dir'].'user/personal/personal_apply.php?act=apply_jobs">申请职位</a></p>
				</div>
				<div class="core-function last f-left">
					<div><a href="'.$_CFG['site_dir'].'user/personal/personal_apply.php?act=favorites">'.intval($fav_num).'</a></div>
					<p><a href="'.$_CFG['site_dir'].'user/personal/personal_apply.php?act=favorites">收藏夹</a></p>
				</div>';
			$contents=str_replace('{#$count#}',$count_htm,$contents);
		}
		$contents=str_replace('{#$pmscount_a#}',$pmscount_a,$contents);
		$contents=str_replace('{#$user_url#}',$user_url,$contents);
		$contents=str_replace('{#$login_url#}',url_rewrite('QS_login'),$contents);
		$contents=str_replace('{#$logout_url#}',url_rewrite('QS_login')."?act=logout",$contents);
		$contents=str_replace('{#$reg_url#}',$_CFG['site_dir']."user/user_reg.php",$contents);
		$contents=str_replace('{#$activate_url#}',$_CFG['site_dir']."user/user_reg.php?act=activate",$contents);
		exit($contents);
}
// 注册发送短信/找回密码 短信
elseif($act == "reg_send_sms")
{
	$mobile=trim($_POST['mobile']);
	$sms_type=$_POST['sms_type']?$_POST['sms_type']:"reg";
	if (empty($mobile) || !preg_match("/^(13|15|14|17|18)\d{9}$/",$mobile))
	{
		exit("手机号错误");
	}
	$rand=mt_rand(100000, 999999);	
	switch ($sms_type) {
		case 'reg':
			$sms_str="您正在注册{$_CFG['site_name']}的会员,手机验证码为:{$rand},此验证码有效期为10分钟";
			break;
		case 'getpass':
			$sms_str="您正在找回{$_CFG['site_name']}的会员密码,手机验证码为:{$rand},此验证码有效期为10分钟";
			break;
	}
	if($_SESSION['verify_mobile']==$mobile && time()<$_SESSION['send_time']+180)
	{
		exit("180秒内仅能获取一次短信验证码,请稍后重试");
	}
	else
	{
		$r=send_sms($mobile,$sms_str);
	}
	if ($r=="success")
	{
	$_SESSION['mobile_rand']=substr(md5($rand), 8,16);
	$_SESSION['send_time']=time();
	$_SESSION['verify_mobile']=$mobile;
	exit("success");
	}
	else
	{
	exit("SMS配置出错，请联系网站管理员");
	}
}
// 验证注册短信
elseif($act == "check_reg_send_sms")
{
	$mobile=trim($_POST['mobile']);
	$time=time();
	if (empty($mobile) || !preg_match("/^(13|15|14|18|17)\d{9}$/",$mobile))
	{
	exit("false");//手机号错误
	}
	if($mobile!=$_SESSION['verify_mobile'])
	{
	exit("false");//手机号不一致
	}
	if($time>$_SESSION['send_time']+600)
	{
	exit("false");//验证码过期
	}
	$vcode_sms=intval($_POST['mobile_vcode']);
	$mobile_rand=substr(md5($vcode_sms), 8,16);
	if($mobile_rand!=$_SESSION['mobile_rand']){exit("false");}//验证码错误！
	exit("true");
}
// 找回密码验证 用户名邮箱 手机
elseif($act == "get_pass_check")
{
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$username=$_POST['username']?iconv("utf-8", "gbk", trim($_POST['username'])):exit("false");
	if (preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$username))
	{
		$usinfo=get_user_inemail($username);
	}
	elseif (preg_match("/^(13|14|15|18|17)\d{9}$/",$username))
	{
		$usinfo=get_user_inmobile($username);
	}
	else
	{
		$usinfo=get_user_inusername($username);
	}
	!empty($usinfo)?exit("true"):exit("false");
}
// 找回密码 判断是否 绑定手机 邮箱 微信
elseif($act == "get_pass_check_buding")
{
	require_once(QISHI_ROOT_PATH.'include/fun_user.php');
	$username=$_POST['username']?iconv("utf-8", "gbk", trim($_POST['username'])):exit("");
	if (preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$username))
	{
		$usinfo=get_user_inemail($username);
	}
	elseif (preg_match("/^(13|14|15|18|17)\d{9}$/",$username))
	{
		$usinfo=get_user_inmobile($username);
	}
	else
	{
		$usinfo=get_user_inusername($username);
	}
	if(($usinfo['mobile'] && $usinfo['mobile_audit']==1) || ($usinfo['email'] && $usinfo['email_audit']==1))
	{
		exit("true");
	}
	elseif($usinfo['weixin_openid'])
	{
		exit('buding_wx');
	}
	else
	{
		exit('false');
	}
}
// 找回密码发送邮件
elseif($act == "getpass_sendemail")
{
	global $QS_pwdhash;
	$email=$_POST['email']?trim($_POST['email']):exit("邮箱出错");
	$username=$_POST['username']?iconv("utf-8", "gbk", trim($_POST['username'])):exit("没有用户名");
	$uid=$_POST['uid']?intval($_POST['uid']):exit("没有用户名");
	if (!preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$email))
	{
		exit("邮箱出错");
	}
	$time=time();
	$key=substr(md5($username.$QS_pwdhash),8,16);
	$email_str.=$username."您好：<br>";
	$email_str.="请在24小时内点击以下链接重新设置您的密码：<br>";
	$email_str.="<a href='".$_CFG['site_domain'].$_CFG['site_dir']."user/user_getpass.php?act=get_pass_step3_email&uid=$uid&key=$key&time=$time' target='_blank'>".$_CFG['site_domain'].$_CFG['site_dir']."user/user_getpass.php?act=get_pass_step3_email&uid=$uid&key=$key&time=$time</a><br>";
	$email_str.="如果链接无法点击,请复制粘贴到浏览器访问！<br>";
	$email_str.="本邮件由系统发出,请勿回复！<br>";
	$email_str.="如有任何疑问请联系网站官方：".$_CFG['top_tel']."";

	if (smtp_mail($email,"{$_CFG['site_name']}-找回密码",$email_str))
	{
		exit("success");
	}
	else
	{
		exit("邮箱配置出错，请联系网站管理员");
	}
}
//注册发送邮件
elseif($act == "reg_sendemail")
{
	$email=$_POST['email']?trim($_POST['email']):exit("邮箱出错1");
	$utype=$_POST['utype']?intval($_POST['utype']):exit("邮箱出错2");
	if (!preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$email))
	{
		exit("邮箱出错3");
	}
	$time=time();
	$key=substr(md5($email.$time),8,16);
	$email_str.="您好,请在24小时内点击以下链接完成注册：<br>";
	$email_str.="<a href='".$_CFG['site_domain'].$_CFG['site_dir']."user/user_reg.php?act=reg_step2_email&email=$email&utype=$utype&key=$key&time=$time' target='_blank'>".$_CFG['site_domain'].$_CFG['site_dir']."user/user_reg.php?act=reg_step2_email&email=$email&utype=$utype&key=$key&time=$time</a><br>";
	$email_str.="如果链接无法点击,请复制粘贴到浏览器访问！<br>";
	$email_str.="本邮件由系统发出,请勿回复！<br>";
	$email_str.="如有任何疑问请联系网站官方：".$_CFG['top_tel']."";

	if (smtp_mail($email,"{$_CFG['site_name']} - 会员注册",$email_str))
	{
		exit("success");
	}
	else
	{
		exit("邮箱配置出错，请联系网站管理员");
	}
}
?>