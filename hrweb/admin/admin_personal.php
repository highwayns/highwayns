<?php
 /*
 * 74cms 个人
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_personal_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
if($act == 'list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"resume_show");
	$tabletype=intval($_GET['tabletype']);
	$audit=intval($_GET['audit']);
	if (empty($tabletype))
	{
		$tabletype=1;
		$_GET['tabletype']=1;
	}
	if ($tabletype==1)
	{
	$audit="";
	}
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$oederbysql=" order BY refreshtime DESC ";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if ($key && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE fullname like '%{$key}%'";
		elseif ($key_type===2)$wheresql=" WHERE id='".intval($key)."'";
		elseif ($key_type===3)$wheresql=" WHERE uid='".intval($key)."'";	
		elseif ($key_type===4)$wheresql=" WHERE telephone like '%{$key}%'";	
		elseif ($key_type===5)$wheresql=" WHERE qq like '%{$key}%'";
		elseif ($key_type===6)$wheresql=" WHERE address like '%{$key}%'";
		$oederbysql="";
		$tablename="all";
	}
	else
	{
		$photo_audit=intval($_GET['photo_audit']);
		!empty($audit)? $wheresqlarr['audit']=$audit:'';
		!empty($_GET['talent'])? $wheresqlarr['talent']=intval($_GET['talent']):'';
		if ($photo_audit>0)
		{
			$wheresqlarr['photo_audit']=$photo_audit;
			$oederbysql="";
		}
		if ($_GET['photo']<>'')
		{
		$wheresqlarr['photo']=intval($_GET['photo']);
		$oederbysql=" order BY addtime DESC ";
		}
		if ($_GET['photo_display']<>'')
		{
		$wheresqlarr['photo_display']=intval($_GET['photo_display']);
		$oederbysql=" order BY addtime DESC ";
		}
		if (is_array($wheresqlarr)) $wheresql=wheresql($wheresqlarr);	
		if (!empty($_GET['addtimesettr']))
		{
			$settr=strtotime("-".intval($_GET['addtimesettr'])." day");
			$wheresql=empty($wheresql)?" WHERE addtime> ".$settr:$wheresql." AND addtime> ".$settr;
			$oederbysql=" order BY addtime DESC ";
		}
		if (!empty($_GET['settr']))
		{
			$settr=strtotime("-".intval($_GET['settr'])." day");
			$wheresql=empty($wheresql)?" WHERE refreshtime> ".$settr:$wheresql." AND refreshtime> ".$settr;
		}
	}
	if ($tablename=="all")
	{
	$total_sql="SELECT COUNT(*) AS num FROM ".table('resume').$wheresql;
	}
	else
	{
		if($tabletype==1){
			$wheresql=empty($wheresql)?" WHERE display=1 AND audit=1":$wheresql." AND display=1 AND audit=1";
		}elseif($tabletype==2){
			if(!empty($audit)){
				if($audit==1){
					$wheresql=$wheresql." AND display=2 ";
				}
			}else{
				$wheresql=empty($wheresql)?" WHERE display=2 OR (display=1 AND audit<>1) ":$wheresql." AND display=2 OR (display=1 AND audit<>1) ";
			}
			
		}
		$total_sql="SELECT COUNT(*) AS num FROM ".table('resume').$wheresql;
	}
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	if ($tablename=="all")
	{
	$getsql="SELECT * FROM ".table('resume').$wheresql;
	}
	else
	{
		if($tabletype==1){
			$wheresql=empty($wheresql)?" WHERE display=1 AND audit=1":$wheresql." AND display=1 AND audit=1";
		}elseif($tabletype==2){
			if(!empty($audit)){
				if($audit==1){
					$wheresql=$wheresql." AND display=2 ";
				}
			}else{
				$wheresql=empty($wheresql)?" WHERE display=2 OR (display=1 AND audit<>1) ":$wheresql." AND display=2 OR (display=1 AND audit<>1) ";
			}
			
		}
		$getsql="SELECT * FROM ".table('resume')." ".$wheresql.$oederbysql;
	}
	$resumelist = get_resume_list($offset,$perpage,$getsql);
	$total_all_resume = $db->get_total("SELECT COUNT(*) AS num FROM ".table('resume'));
	$total[0]=$db->get_total("SELECT COUNT(*) AS num FROM ".table('resume')." where display=1 and audit=1");
	$total[1]=$total_all_resume-$total[0];
	if ($tabletype===2)
	{
	$total[2]=$db->get_total("SELECT COUNT(*) AS num FROM ".table('resume')." WHERE audit=1 AND display=2");
	$total[3]=$db->get_total("SELECT COUNT(*) AS num FROM ".table('resume')." WHERE audit=2 ");
	$total[4]=$db->get_total("SELECT COUNT(*) AS num FROM ".table('resume')." WHERE audit=3 ");
	}
	$smarty->assign('total',$total);
	$smarty->assign('pageheader',"简历列表");
	$smarty->assign('resumelist',$resumelist);
	$smarty->assign('page',$page->show(3));
	$smarty->assign('total_val',$total_val);
	$smarty->display('personal/admin_personal_resume.htm');
}
elseif($act == 'perform')
{
		check_token();
		$id =!empty($_REQUEST['id'])?$_REQUEST['id']:adminmsg("你没有选择简历！",1);
		if (!empty($_REQUEST['delete']))
		{
			check_permissions($_SESSION['admin_purview'],"resume_del");
			if ($n=del_resume($id))
			{
			adminmsg("删除成功！该删除 {$n} 行",2);
			}
			else
			{
			adminmsg("删除失败！",0);
			}
		}
		if (!empty($_POST['set_audit']))
		{
			check_permissions($_SESSION['admin_purview'],"resume_audit");
			$audit=$_POST['audit'];
			$pms_notice=intval($_POST['pms_notice']);
			$reason=trim($_POST['reason']);
			!edit_resume_audit($id,$audit,$reason,$pms_notice)?adminmsg("设置失败！",0):adminmsg("设置成功！",2,$link);
		}
		
		if (!empty($_POST['set_talent']))
		{
		check_permissions($_SESSION['admin_purview'],"resume_talent");
		$talent=$_POST['talent'];
		!edit_resume_talent($id,$talent)?adminmsg("设置失败！",0):adminmsg("设置成功！",2,$link);
		}
		if (!empty($_POST['set_photoaudit']))
		{
		check_permissions($_SESSION['admin_purview'],"resume_photo_audit");
		$photoaudit=$_POST['photoaudit'];
		$is_del_img=intval($_POST['is_del_img']);
		!edit_resume_photoaudit($id,$photoaudit,$is_del_img)?adminmsg("设置失败！",0):adminmsg("设置成功！",2,$link);
		}
		elseif (!empty($_GET['refresh']))
		{
			if($n=refresh_resume($id))
			{
			adminmsg("刷新成功！响应行数 {$n}",2);
			}
			else
			{
			adminmsg("刷新失败！",0);
			}
		}	
}
elseif($act == 'members_list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"per_user_show");
		require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$wheresql=" WHERE  m.utype=2 ";
	$oederbysql=" order BY m.uid DESC ";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if ($key && $key_type>0)
	{
		if     ($key_type===1)$wheresql.=" AND m.username like '{$key}%'";
		elseif ($key_type===2)$wheresql.=" AND m.uid='".intval($key)."'";
		elseif ($key_type===3)$wheresql.=" AND m.email like '{$key}%'";
		elseif ($key_type===4)$wheresql.=" AND m.mobile like '{$key}%'";
		$oederbysql="";
	}
	else
	{	
		if (!empty($_GET['settr']))
		{
			$settr=strtotime("-".intval($_GET['settr'])." day");
			$wheresql.=" AND m.reg_time> ".$settr;
		}
		if (!empty($_GET['verification']))
		{
			if ($_GET['verification']=="1")
			{
			$wheresql.=" AND m.email_audit = 1";
			}
			elseif ($_GET['verification']=="2")
			{
			$wheresql.=" AND m.email_audit = 0";
			}
			elseif ($_GET['verification']=="3")
			{
			$wheresql.=" AND m.mobile_audit = 1";
			}
			elseif ($_GET['verification']=="4")
			{
			$wheresql.=" AND m.mobile_audit = 0";
			}
		}
	}
	$total_sql="SELECT COUNT(*) AS num FROM ".table('members')." as m ".$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$member = get_member_list($offset,$perpage,$wheresql.$oederbysql);
	$smarty->assign('pageheader',"个人会员");
	$smarty->assign('member',$member);
	$smarty->assign('page',$page->show(3));
	$smarty->display('personal/admin_personal_user_list.htm');
}
elseif($act == 'delete_user')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"per_user_del");
	$tuid =!empty($_POST['tuid'])?$_POST['tuid']:adminmsg("你没有选择会员！",1);
	if ($_POST['delete'])
	{
		if ($_POST['delete_user']=='yes' && !delete_member($tuid))
		{
			adminmsg("删除会员失败！",0);
		}
		if ($_POST['delete_resume']=='yes' && !del_resume_for_uid($tuid))
		{
			adminmsg("删除简历失败！",0);
		}
		adminmsg("删除成功！",2);
	}
}
elseif($act == 'user_edit')
{	
	get_token();
	check_permissions($_SESSION['admin_purview'],"per_user_edit");
	$smarty->assign('pageheader',"个人会员");
	$smarty->assign('user',get_member_one($_GET['tuid']));
	$smarty->assign('resume',get_resume_uid($_GET['tuid']));
	$smarty->assign('url',$_SERVER["HTTP_REFERER"]);
	$smarty->display('personal/admin_personal_user_edit.htm');
}
elseif($act == 'set_account_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"per_user_edit");
	require_once(ADMIN_ROOT_PATH.'include/admin_user_fun.php');
	$setsqlarr['username']=trim($_POST['username']);
	$setsqlarr['email']=trim($_POST['email']);
	$setsqlarr['email_audit']=intval($_POST['email_audit']);
	$setsqlarr['mobile']=trim($_POST['mobile']);
	$setsqlarr['mobile_audit']=intval($_POST['mobile_audit']);
	if ($_POST['qq_openid']=="1")
	{
	$setsqlarr['qq_openid']='';
	}
	$thisuid=intval($_POST['thisuid']);	
	if (strlen($setsqlarr['username'])<3) adminmsg('用户名必须为3位以上！',1);
	$getusername=get_user_inusername($setsqlarr['username']);
	if (!empty($getusername)  && $getusername['uid']<>$thisuid)
	{
	adminmsg("用户名 {$setsqlarr['username']}  已经存在！",1);
	}
	//若勾选已验证，则需判断手机号是否填写
	if($setsqlarr['mobile_audit']==1)
	{
		if (empty($setsqlarr['mobile']))
		{
		adminmsg('手机号码为空！',1);
		}
	}
	if (!empty($setsqlarr['mobile']) && !preg_match("/^(13|15|14|17|18)\d{9}$/",$setsqlarr['mobile']))
	{
	adminmsg('手机号码错误！',1);
	}
	$getemail=get_user_inemail($setsqlarr['email']);
	if (!empty($getemail)  && $getemail['uid']<>$thisuid)
	{
	adminmsg("Email  {$setsqlarr['email']}  已经存在！",1);
	}
	//若勾选已验证，则需判断手机号是否填写
	if($setsqlarr['mobile_audit']==1)
	{
		if (empty($setsqlarr['mobile']))
		{
		adminmsg('手机号码为空！',1);
		}
	}
	if (!empty($setsqlarr['mobile']) && !preg_match("/^(13|15|14|17|18)\d{9}$/",$setsqlarr['mobile']))
	{
	adminmsg('手机号码错误！',1);
	}
	$getmobile=get_user_inmobile($setsqlarr['mobile']);
	if (!empty($setsqlarr['mobile']) && !empty($getmobile)  && $getmobile['uid']<>$thisuid)
	{
	adminmsg("手机号 {$setsqlarr['mobile']}  已经存在！",1);
	}
	if ($db->updatetable(table('members'),$setsqlarr," uid=".$thisuid.""))
	{
		$u['email']=$setsqlarr['email'];
		$db->updatetable(table('resume'),$u," uid={$thisuid}");
		//填写管理员日志
		write_log("修改会员uid为".$thisuid."的基本信息", $_SESSION['admin_name'],3);
		$link[0]['text'] = "返回列表";
		$link[0]['href'] = $_POST['url'];
		adminmsg('修改成功！',2,$link);
	}
	else
	{
		adminmsg('修改失败！',1);
	}
}
elseif($act == 'userpass_edit')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"per_user_edit");
	if (strlen(trim($_POST['password']))<6) adminmsg('新密码必须为6位以上！',1);
	$user_info=get_member_one($_POST['memberuid']);
	$pwd_hash=$user_info['pwd_hash'];
	$md5password=md5(md5(trim($_POST['password'])).$pwd_hash.$QS_pwdhash);	
		if ($db->query( "UPDATE ".table('members')." SET password = '{$md5password}'  WHERE uid='{$user_info['uid']}' LIMIT 1"))
		{
			$link[0]['text'] = "返回列表";
			$link[0]['href'] = $_POST['url'];
			$member=get_member_one($user_info['uid']);
			write_memberslog($member['uid'],1,1004,$member['username'],"管理员在后台修改登录密码");
			//填写管理员日志
			write_log("修改会员uid为".$member['uid']."的密码", $_SESSION['admin_name'],3);
			adminmsg('操作成功！',2,$link);
		}
		else
		{
			adminmsg('操作失败！',1);
		}
}
elseif($act == 'members_add')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"per_user_add");
	$smarty->assign('pageheader',"个人会员");
	$smarty->display('personal/admin_personal_user_add.htm');
}
elseif($act == 'members_add_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"per_user_add");	
	require_once(ADMIN_ROOT_PATH.'include/admin_user_fun.php');
	if (strlen(trim($_POST['username']))<3) adminmsg('用户名必须为3位以上！',1);
	if (strlen(trim($_POST['password']))<6) adminmsg('密码必须为6位以上！',1);
	$sql['username'] = !empty($_POST['username']) ? trim($_POST['username']):adminmsg('请填写用户名！',1);
	$sql['password'] = !empty($_POST['password']) ? trim($_POST['password']):adminmsg('请填写密码！',1);	
	if ($sql['password']<>trim($_POST['password1']))
	{
	adminmsg('两次输入的密码不相同！',1);
	}
	$sql['utype'] = !empty($_POST['member_type']) ? intval($_POST['member_type']):adminmsg('你没有选择注册类型！',1);
	if (empty($_POST['email']) || !preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$_POST['email']))
	{
	adminmsg('电子邮箱格式错误！',1);
	}
	$sql['email']= trim($_POST['email']);
	if (get_user_inusername($sql['username']))
	{
	adminmsg('该用户名已经被使用！',1);
	}
	if (get_user_inemail($sql['email']))
	{
	adminmsg('该 Email 已经被注册！',1);
	}
	$sql['pwd_hash'] = randstr();
	$sql['password'] = md5(md5($sql['password']).$sql['pwd_hash'].$QS_pwdhash);
	$sql['reg_time']=time();
	$sql['reg_ip']=$online_ip;
	$insert_id=$db->inserttable(table('members'),$sql,true);
	if ($insert_id)
	{
		//填写管理员日志
		write_log("添加id为".$insert_id."的个人会员", $_SESSION['admin_name'],3);
		write_memberslog($insert_id,1,1000,$sql['username'],"管理员在后台新增会员");
		$link[0]['text'] = "返回列表";
		$link[0]['href'] = "?act=members_list";
		adminmsg('添加成功！',2,$link);
	}	
}
elseif($act == 'resume_show')
{
	check_permissions($_SESSION['admin_purview'],"resume_show");
	$id =!empty($_REQUEST['id'])?intval($_REQUEST['id']):adminmsg("你没有选择简历！",1);
	$uid =intval($_REQUEST['uid']);
	$smarty->assign('pageheader',"查看简历");
	$resume=get_resume_basic($uid,$id);
	if (empty($resume))
	{
	$link[0]['text'] = "返回简历列表";
	$link[0]['href'] = '?act=list';
	adminmsg('简历不存在或已经被删除！',1,$link);
	}
	$smarty->assign('random',mt_rand());
	$smarty->assign('time',time());
	$smarty->assign('url',$_SERVER["HTTP_REFERER"]);
	$smarty->assign('resume',$resume);
	$smarty->assign('resume_education',get_resume_education($uid,$id));
	$smarty->assign('resume_work',get_resume_work($uid,$id));
	$smarty->assign('resume_training',get_resume_training($uid,$id));
	$smarty->assign('resumeaudit',get_resumeaudit_one($id));
	$smarty->display('personal/admin_personal_resume_show.htm');
}
elseif($act == 'del_auditreason')
{	
	check_permissions($_SESSION['admin_purview'],"resume_audit");
	$id =!empty($_REQUEST['a_id'])?$_REQUEST['a_id']:adminmsg("你没有选择日志！",1);
$n=reasonaudit_del($id);
	if ($n>0)
	{
	adminmsg("删除成功！共删除 {$n} 行",2);
	}
	else
	{
	adminmsg("删除失败！",0);
	}
}
elseif($act == 'management')
{	
	$id=intval($_GET['id']);
	$u=get_user($id);
	if (!empty($u))
	{
		unset($_SESSION['uid']);
		unset($_SESSION['username']);
		unset($_SESSION['utype']);
		unset($_SESSION['uqqid']);
		setcookie("QS[uid]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		setcookie("QS[username]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		setcookie("QS[password]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		setcookie("QS[utype]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		unset($_SESSION['activate_username']);
		unset($_SESSION['activate_email']);
		
		$_SESSION['uid']=$u['uid'];
		$_SESSION['username']=$u['username'];
		$_SESSION['utype']=$u['utype'];
		$_SESSION['uqqid']="1";
		setcookie('QS[uid]',$u['uid'],0,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[username]',$u['username'],0,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[password]',$u['password'],0,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[utype]',$u['utype'], 0,$QS_cookiepath,$QS_cookiedomain);
		header("Location:".get_member_url($u['utype']));
	}	
}
elseif($act == 'userstatus_edit')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"com_user_edit");
	if(set_user_status(intval($_POST['status']),intval($_POST['userstatus_uid'])))
	{
		$link[0]['text'] = "返回列表";
		$link[0]['href'] = $_POST['url'];
		adminmsg('操作成功！',2,$link);
	}
	else
	{
	adminmsg('操作失败！',1);
	}
}
?>