﻿<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_users_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
$smarty->assign('pageheader',"ウェブ管理者");
if($act == 'list')
{
	get_token();
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	if ($_SESSION['admin_purview']<>"all")
	{
		$wheresql=" WHERE admin_name='".$_SESSION['admin_name']."'";
	}
	$total_sql="SELECT COUNT(*) AS num FROM ".table('admin').$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_admin_list($offset,$perpage,$wheresql);	
	$smarty->assign('list',$list);
	$smarty->assign('admin_purview',$_SESSION['admin_purview']);
	$smarty->assign('page',$page->show(3));
	$smarty->assign('navlabel','list');	
	$smarty->display('users/admin_users_list.htm');
}
elseif($act == 'add_users')
{
	get_token();
	if ($_SESSION['admin_purview']<>"all")adminmsg("権限不足！",1);
	$smarty->assign('navlabel','add');	
	$smarty->display('users/admin_users_add.htm');
}
elseif($act == 'add_users_save')
{
	check_token();
	if ($_SESSION['admin_purview']<>"all")adminmsg("権限不足！",1);
	$setsqlarr['admin_name']=trim($_POST['admin_name'])?trim($_POST['admin_name']):adminmsg('ユーザ名を入力してください！',1);
	if (get_admin_one($setsqlarr['admin_name']))adminmsg('該当ユーザすでに存在する！',1);
	$setsqlarr['email']=trim($_POST['email'])?trim($_POST['email']):adminmsg('Emailを入力してください！',1);
	if (!preg_match("/^[\w\-\.]+@[\w\-\.]+(\.\w+)+$/",$setsqlarr['email']))adminmsg('emailフォーマットエラー！',1);
	$password=trim($_POST['password'])?trim($_POST['password']):adminmsg('パスワードを入力してください',1);
	if (strlen($password)<6)adminmsg('パスワード不能少于6位！',1);
	if ($password<>trim($_POST['password1']))adminmsg('パスワード一致しません！',1);
	$setsqlarr['rank']=trim($_POST['rank'])?trim($_POST['rank']):adminmsg('タイトルを入力してください',1);
	$setsqlarr['add_time']=time();
	$setsqlarr['last_login_time']=0;
	$setsqlarr['last_login_ip']="Never";
	$setsqlarr['pwd_hash']=randstr();
	$setsqlarr['pwd']=md5($password.$setsqlarr['pwd_hash'].$HW_pwdhash);	
	
	if ($db->inserttable(table('admin'),$setsqlarr))
	{
		//填写管理员日志
		write_log("追加ユーザ名は".$setsqlarr['admin_name']."の管理者", $_SESSION['admin_name'],3);
		$link[0]['text'] = "一覧に戻る";
		$link[0]['href'] ="?act=";
		adminmsg('追加成功！',2,$link);
	}
	else
	{
	adminmsg('追加失敗',1);
	}	
}
elseif($act == 'del_users')
{
	check_token();
	$id=$_REQUEST['id'];
	if ($num=del_users($id,$_SESSION['admin_purview']))
	{
	adminmsg("削除成功！削除件数".$num."行",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
elseif($act == 'edit_users')
{
	get_token();
	$id=intval($_GET['id']);
	$account=get_admin_account($id);
	if ($account['admin_name']==$_SESSION['admin_name'] || $_SESSION['admin_purview']=="all")
	{
	$smarty->assign('account',$account);
	$smarty->assign('admin_purview',$_SESSION['admin_purview']);
	$smarty->display('users/admin_users_edit.htm');
	}
	else
	{
	adminmsg("パラメータエラー！",1);
	}
}
elseif($act == 'edit_users_pwd')
{
	get_token();
	$id=intval($_GET['id']);
	$account=get_admin_account($id);
	if ($account['admin_name']==$_SESSION['admin_name'] || $_SESSION['admin_purview']=="all")
	{
	$smarty->assign('account',$account);
	$smarty->assign('admin_purview',$_SESSION['admin_purview']);
	$smarty->display('users/admin_users_edit_pwd.htm');
	}
	else
	{
	adminmsg("パラメータエラー！",1);
	}
}
elseif($act == 'edit_users_info_save' && $_SESSION['admin_purview']=="all")//超级管理员才可以修改资料
{
	check_token();
		$id=intval($_POST['id']);
		$account=get_admin_account($id);
		if ($account['purview']=="all")adminmsg("パラメータエラー！",1);//超级管理员的资料不能修改
		$setsqlarr['admin_name']=trim($_POST['admin_name'])?trim($_POST['admin_name']):adminmsg('ユーザ名が必須！',1);
		$setsqlarr['email']=trim($_POST['email'])?trim($_POST['email']):adminmsg('emailを入力してください！',1);
		$setsqlarr['rank']=trim($_POST['rank'])?trim($_POST['rank']):adminmsg('タイトルを入力してください！',1);
			$sql = "select * from ".table('admin')." where admin_name = '".$$setsqlarr['admin_name']."' AND admin_id<>".$id;
			$ck_info=$db->getone($sql);
			if (!empty($ck_info))adminmsg("ユーザ名重複！",1);
		if ($db->updatetable(table('admin'),$setsqlarr,' admin_id='.$id))
		{
			//填写管理员日志
			write_log("スーパー管理者資料変更成功", $_SESSION['admin_name'],3);
			adminmsg("変更成功！",2);
		 }
		 else
		{
			//填写管理员日志
			write_log("ウーパー管理者資料変更失敗", $_SESSION['admin_name'],3);
			adminmsg("変更失敗！",0);
		 }
}
elseif($act == 'edit_users_pwd_save')
{
	check_token();
	$id=intval($_POST['id']);
	$account=get_admin_account($id);
	if ($account['purview']=="all" && $_SESSION['admin_purview']=="all")
	{
				if (strlen($_POST['password'])<6)adminmsg("パスワード長さは6位以上！",1);
				if ($_POST['password']<>$_POST['password1'])adminmsg("入力のパスワードは一致しません！",1);		
				$md5_pwd=md5($_POST['old_password'].$account['pwd_hash'].$HW_pwdhash);
				if ($md5_pwd<>$account['pwd'])adminmsg("旧パスワード入力エラー！",1);
				$setsqlarr['pwd']=md5($_POST['password'].$account['pwd_hash'].$HW_pwdhash);
				if ($db->updatetable(table('admin'),$setsqlarr,' admin_id='.$id))
				{
					//填写管理员日志
					write_log("スーパー管理者パスワード変更成功", $_SESSION['admin_name'],3);
					adminmsg("変更成功！",2);
				 }
				 else
				 {
				 	//填写管理员日志
					write_log("スーバー管理者パスワード変更失敗", $_SESSION['admin_name'],3);
					adminmsg("変更失敗！",0);
				 }
	}
	else
	{
				if ($_SESSION['admin_purview']=="all")
				{
					if (strlen($_POST['password'])<6)adminmsg("パスワード長さは6位以上！",1);
					$setsqlarr['pwd']=md5($_POST['password'].$account['pwd_hash'].$HW_pwdhash);
					//填写管理员日志
					write_log("管理者パスワード変更", $_SESSION['admin_name'],3);
					if (!$db->updatetable(table('admin'),$setsqlarr,' admin_id='.$id)) adminmsg("変更失敗！",0);
				}
				else
				{
					if (strlen($_POST['password'])<6)adminmsg("パスワード長さは6位以上！",1);
					if ($_POST['password']<>$_POST['password1'])adminmsg("入力のパスワードは一致しません！",1);		
					$md5_pwd=md5($_POST['old_password'].$account['pwd_hash'].$HW_pwdhash);
					if ($md5_pwd<>$account['pwd'])adminmsg("旧パスワード入力エラー！",1);
					$setsqlarr['pwd']=md5($_POST['password'].$account['pwd_hash'].$HW_pwdhash);
					//填写管理员日志
					write_log("管理者パスワード変更", $_SESSION['admin_name'],3);
					if (!$db->updatetable(table('admin'),$setsqlarr,' admin_id='.$id)) adminmsg("変更失敗！",0);
				}
				 adminmsg("変更成功！",2);
	}
}
elseif($act == 'loglist')
{
	get_token();
	$adminname=trim($_GET['adminname']);
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	if ($_SESSION['admin_purview']=="all")//超级管理员可以查看任何管理员的日志
	{
		$wheresql="";
	}
	else
	{
		$wheresql=" WHERE admin_name='".$_SESSION['admin_name']."'";
	}
	if (!empty($_GET['log_type']))
	{
		$wheresql=empty($wheresql)?" WHERE log_type= ".intval($_GET['log_type']):$wheresql." AND log_type=".intval($_GET['log_type']);
	}
	$total_sql="SELECT COUNT(*) AS num FROM ".table('admin_log').$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_admin_log($offset,$perpage,$wheresql);
	$smarty->assign('pageheader',"登録ログ");
	$smarty->assign('list',$list);//列表
	$smarty->assign('perpage',$perpage);//每页显示数量POST
		if ($total_val>$perpage)
		{
		$smarty->assign('page',$page->show(3));//分页符
		}
	$smarty->display('users/admin_users_log.htm');
}
elseif($act == 'users_set')
{
	get_token();
	$id=intval($_GET['id']);
	$account=get_admin_account($id);
	$smarty->assign('account',$account);
	$smarty->assign('admin_purview',$_SESSION['admin_purview']);
	$smarty->assign('admin_set',explode(',',$account['purview']));
	$smarty->display('users/admin_users_set.htm');
}
elseif($act == 'users_set_save')
{
	check_token();
	$id=intval($_POST['id']);
	if ($_SESSION['admin_purview']<>"all")adminmsg("権限不足！",1);
	$setsqlarr['purview']=$_POST['purview'];
	$setsqlarr['purview']=implode(',',$setsqlarr['purview']);
		if ($db->updatetable(table('admin'),$setsqlarr,' admin_id='.$id))
		{
			//填写管理员日志
			write_log("管理者権限設定成功", $_SESSION['admin_name'],3);
			adminmsg("設定成功！",2);
		 }
		 else
		{
			//填写管理员日志
			write_log("管理者权限設定失敗", $_SESSION['admin_name'],3);
			adminmsg("設定失敗！",0);
		 }
}
?>
