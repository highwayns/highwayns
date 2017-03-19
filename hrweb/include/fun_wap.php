<?php
 /*
 * 74cms WAP
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
 if(!defined('IN_QISHI'))
 {
 	die('Access Denied!');
 }
function company_one($id)
{
	global $db;
	$id=intval($id);
	$wheresql=" WHERE id=".$id;
	$sql = "select * from ".table('company_profile').$wheresql." LIMIT 1";
	$val=$db->getone($sql);
	$val['contents'] = htmlspecialchars_decode($val['contents'],ENT_QUOTES);
	$jobslist = $db->getall("select * from ".table('jobs')." where `company_id`=".$id." limit 5");
	foreach ($jobslist as $key => $value) {
		$jobslist[$key]['url'] = wap_url_rewrite("wap-jobs-show",array("id"=>$value['id']));
	}
	$val['jobslist'] = $jobslist;
	return $val;
}
function hunter_one($uid)
{
	global $db;
	$wheresql=" WHERE uid=".intval($uid);
	$sql = "select * from ".table('hunter_profile').$wheresql." LIMIT 1";
	$val=$db->getone($sql);
	$jobslist = $db->getall("select * from ".table('hunter_jobs')." where `uid`=".$val['uid']." limit 5");
	foreach ($jobslist as $key => $value) {
		$jobslist[$key]['url'] = wap_url_rewrite("wap-hunter-jobs-show",array("id"=>$value['id']));
	}
	$val['jobslist'] = $jobslist;
	return $val;
}
function news_one($id)
{
	global $db;
	$wheresql=" WHERE id=".intval($id);
	$sql = "select * from ".table('article').$wheresql." LIMIT 1";
	$val=$db->getone($sql);
	$val['content'] = htmlspecialchars_decode($val['content'],ENT_QUOTES);
	return $val;
}
function jobs_one($id)
{
	global $db;
	$id=intval($id);
	$db->query("update ".table('jobs')." set click=click+1 WHERE id='{$id}'  LIMIT 1");
	$db->query("update ".table('jobs_search_hot')." set click=click+1 WHERE id='{$id}'  LIMIT 1");
	$wheresql=" WHERE id='".$id."'";
	$sql = "select * from ".table('jobs').$wheresql." LIMIT 1";
	$val=$db->getone($sql);
	if(empty($val)){
		$sql = "select * from ".table('jobs_tmp').$wheresql." LIMIT 1";
		$val=$db->getone($sql);
	}
	if(intval($_SESSION['uid'])>0 && intval($_SESSION['utype'])==2){
		//检查该职位是否对此会员发起面试邀请,并且此会员没看
		$check_int = wap_check_interview(intval($_SESSION['uid']),$val['id']);
		if($check_int){
			wap_update_interview(intval($_SESSION['uid']),$val['id']);
		}
	}
	$val['amount']=$val['amount']=="0"?'若干':$val['amount'];
	$profile=company_one($val['company_id']);
	$val['company']=$profile;
	$val['contents'] = htmlspecialchars_decode($val['contents'],ENT_QUOTES);
	$val['company_url']=wap_url_rewrite("wap-company-show",array("id"=>$val['company_id']));
	$sql = "select * from ".table('jobs_contact')." where pid='{$id}' LIMIT 1";
	$contact=$db->getone($sql);
	$val['contact']=$contact;
	if ($val['tag_cn'])
	{
		$tag_cn=explode(',',$val['tag_cn']);
		$val['tag_cn']=$tag_cn;
	}
	else
	{
	$val['tag_cn']=array();
	}
	return $val;
}
function hunter_jobs_one($id)
{
	global $db;
	$id=intval($id);
	$db->query("update ".table('hunter_jobs')." set click=click+1 WHERE id='{$id}'  LIMIT 1");
	$wheresql=" WHERE id='{$id}'";
	$sql = "select * from ".table('hunter_jobs').$wheresql." LIMIT 1";
	$val=$db->getone($sql);
	$val['amount']=$val['amount']=="0"?'若干':$val['amount'];
	$profile=hunter_one($val['uid']);
	$val['company']=$profile;
	return $val;
}
function resume_one($id)
{
	global $db;
	$id=intval($id);
	$db->query("update ".table('resume')." set click=click+1 WHERE id='{$id}'  LIMIT 1");
	$wheresql=" WHERE id='{$id}'";
	$sql = "select * from ".table('resume').$wheresql." LIMIT 1";
	$val=$db->getone($sql);
	if ($val['display_name']=="2")
	{
		$val['fullname']="N".str_pad($val['id'],7,"0",STR_PAD_LEFT);
		$val['fullname_']=$val['fullname'];		
	}
	elseif($val['display_name']=="3")
	{
		if($val['sex']==1)
		{
			$val['fullname']=cut_str($val['fullname'],1,0,"先生");
		}
		elseif($val['sex']==2)
		{
			$val['fullname']=cut_str($val['fullname'],1,0,"女士");
		}
		$val['fullname_']=$val['fullname'];	
	}
	else
	{
		$val['fullname_']=$val['fullname'];
		$val['fullname']=$val['fullname'];
	}
	if($val['talent']==1){
		$val['talent_'] = "1";
		$val['talent'] = "普通";
	}elseif($val['talent']==2){
		$val['talent_'] = "2";
		$val['talent'] = "高级";
	}
	$val['fullname_3']=cut_str($val['fullname'],1,0,"先生/女士");
	$val['age']=date("Y")-$val['birthdate'];
	$val['education_list']=get_this_education_all($val['uid'],$val['id']);
	$val['work_list']=get_this_work_all($val['uid'],$val['id']);
	$val['training_list']=get_this_training_all($val['uid'],$val['id']);
	if ($val['tag_cn'])
	{
		$tag_cn=explode(',',$val['tag_cn']);
		$val['tag_cn']=$tag_cn;
	}
	else
	{
	$val['tag_cn']=array();
	}		
	return $val;
}
function WapShowMsg($msg_detail, $msg_type = 0, $links = array())
{
	global $smarty;
    if (count($links) == 0)
    {
        $links[0]['text'] = '返回上一页';
        $links[0]['href'] = 'javascript:history.go(-1)';
    }
   $smarty->assign('ur_here',     '系统提示');
   $smarty->assign('msg_type',    $msg_type);
   $smarty->assign('msg_detail',  $msg_detail);
   $smarty->assign('links',       $links);
   $smarty->assign('default_url', $links[0]['href']);
   $smarty->display('wap/wap-showmsg.html');
	exit();
}
function wapmulti($num, $perpage, $curpage, $mpurl)
{
	$lang['home_page']="首页";
	$lang['last_page']="上一页";
	$lang['next_page']="下一页";
	$lang['end_page']="尾页";
	$lang['page']="页";
	$lang['turn_page']="翻页";
	$multipage = '';
	$mpurl .= strpos($mpurl, '?') ? '&amp;' : '?';
	if($num > $perpage) {
		$page = 5;
		$offset = 2;

		$realpages = @ceil($num / $perpage);
		$pages = $realpages;

		if($page > $pages) {
			$from = 1;
			$to = $pages;
		} else {
			$from = $curpage - $offset;
			$to = $from + $page - 1;
			if($from < 1) {
				$to = $curpage + 1 - $from;
				$from = 1;
				if($to - $from < $page) {
					$to = $page;
				}
			} elseif($to > $pages) {
				$from = $pages - $page + 1;
				$to = $pages;
			}
		}

		$multipage = ($curpage - $offset > 1 && $pages > $page ? '<a href="'.$mpurl.'page=1">'.$lang['home_page'].'</a>' : '').
			($curpage > 1 ? ' <a href="'.$mpurl.'page='.($curpage - 1).'">'.$lang['last_page'].'</a>' : '');

		for($i = $from; $i <= $to; $i++) {
			$multipage .= $i == $curpage ? ' '.$i : ' <a href="'.$mpurl.'page='.$i.'">'.$i.'</a>';
		}

		$multipage .= ($curpage < $pages ? ' <a href="'.$mpurl.'page='.($curpage + 1).'">'.$lang['next_page'].'</a>' : '').
			($to < $pages ? ' <a href="'.$mpurl.'page='.$pages.'">'.$lang['end_page'].'</a>' : '');

		$multipage .= $realpages > $page ?
			'<br />'.$curpage.'/'.$realpages.$lang['page']: '';

	}
	return $multipage;
}
function get_this_education_all($uid,$id)
{
	global $db;
	$sql = "SELECT * FROM ".table('resume_education')." WHERE uid='".intval($uid)."' AND pid='".intval($id)."' ";
	return $db->getall($sql);
}
function get_this_work_all($uid,$id)
{
	global $db;
	$sql = "select * from ".table('resume_work')." where uid=".intval($uid)." AND pid='".$id."' " ;
	return $db->getall($sql);
}
function get_this_training_all($uid,$id)
{
	global $db;
	$sql = "select * from ".table('resume_training')." where uid='".intval($uid)."' AND pid='".intval($id)."'";
	return $db->getall($sql);
}
function get_this_education($uid,$id)
{
	global $db;
	$sql = "SELECT * FROM ".table('resume_education')." WHERE uid='".intval($uid)."' AND id='".intval($id)."' ";
	return $db->getone($sql);
}
function get_this_work($uid,$id)
{
	global $db;
	$sql = "select * from ".table('resume_work')." where uid=".intval($uid)." AND id='".$id."' " ;
	return $db->getone($sql);
}
function get_this_training($uid,$id)
{
	global $db;
	$sql = "select * from ".table('resume_training')." where uid='".intval($uid)."' AND id='".intval($id)."'";
	return $db->getone($sql);
}
function wap_get_user_info($uid)
{
	global $db;
	$uid=intval($uid);
	$sql = "select * from ".table('members')." where uid = '{$uid}' LIMIT 1";
	return $db->getone($sql);
}
function wap_url_rewrite($alias=NULL,$get=NULL,$rewrite=true)
{
	$url ='';
	if (!empty($get))
	{
		foreach($get as $k=>$v)
		{
		$url .="{$k}={$v}&";
		}
	}
	$url=!empty($url)?"?".rtrim($url,'&'):'';
	return $alias.".php".$url;
	
}
function get_member_wap_url($type,$dirname=false)
{
	global $_CFG;
	$type=intval($type);
	if ($type===0) 
	{
	return "";
	}
	elseif ($type===1)
	{
	$return="company/wap_user.php";
	}
	elseif ($type===2) 
	{
	$return="personal/wap_user.php";
	}
	if ($dirname)
	{
	return dirname($return).'/';
	}
	else
	{
	return $return;
	}
}
function wap_get_user_inusername($username){
	global $db;
	$sql = "select * from ".table('members')." where username = '$username' LIMIT 1";
	return $db->getone($sql);
}
function wap_get_user_type($uid)
{
	global $db;
	$sql = "select * from ".table('members_type')." where uid =".intval($uid)." LIMIT 1";
	$user_info=$db->getone($sql);
	return $user_info['utype'];
}
//注册会员
function wap_user_register($username,$password,$member_type=0,$email,$uc_reg=true)
{
	global $db,$timestamp,$_CFG,$online_ip,$QS_pwdhash;
	$member_type=intval($member_type);
	$ck_username=get_user_inusername($username);
	$ck_email=get_user_inemail($email);
	if ($member_type==0) 
	{
	return -1;
	}
	elseif (!empty($ck_username))
	{
	return -2;
	}
	elseif (!empty($ck_email))
	{
	return -3;
	}
	$pwd_hash=randstr();
	$password_hash=md5(md5($password).$pwd_hash.$QS_pwdhash);
	$setsqlarr['username']=$username;
	$setsqlarr['password']=$password_hash;
	$setsqlarr['pwd_hash']=$pwd_hash;
	$setsqlarr['email']=$email;
	$setsqlarr['utype']=intval($member_type);
	$setsqlarr['reg_time']=$timestamp;
	$setsqlarr['reg_ip']=$online_ip;
	$setsqlarr['reg_type']=2;	//来源于WAP
	$insert_id=$db->inserttable(table('members'),$setsqlarr,true);
			if($member_type=="1")
			{
				$setarr["uid"]=$insert_id;
				$db->inserttable(table("members_points"),$setarr);
				$db->inserttable(table("members_setmeal"),$setarr);
			}
return $insert_id;
}
function wap_user_login($account,$password,$account_type=1,$uc_login=true,$expire=NULL)
{
	global $timestamp,$online_ip,$QS_pwdhash;
	$usinfo = $login = array();
	$success = false;
	if (preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$account))
	{
		$account_type=2;
	}elseif (preg_match("/^(13|14|15|18)\d{9}$/",$account))
	{
		$account_type=3;
	}
	if ($account_type=="1")
	{
		$usinfo=get_user_inusername($account);
	}
	elseif ($account_type=="2")
	{
		$usinfo=get_user_inemail($account);
	}
	elseif ($account_type=="3")
	{
		$usinfo=get_user_inmobile($account);
	}
	if (!empty($usinfo))
	{
		$pwd_hash=$usinfo['pwd_hash'];
		$usname=$usinfo['username'];
		$pwd=md5(md5($password).$pwd_hash.$QS_pwdhash);
		if ($usinfo['password']==$pwd)
		{
		wap_update_user_info($usinfo['uid'],true,true,$expire);
		$login['qs_login']=get_member_wap_url($usinfo['utype']);
		$success=true;
		}
		else
		{
		$usinfo='';
		$success=false;
		}
	}
	return $login;	
}
function wap_update_user_info($uid,$record=true,$setcookie=true,$cookie_expire=NULL)
 {
 	global $timestamp, $online_ip,$db,$QS_cookiepath,$QS_cookiedomain,$_CFG;//3.4升级修改 引入变量$_CFG
	$user = wap_get_user_inid($uid);
	if (empty($user))
	{
	return false;
	}
	else
	{
 	$_SESSION['uid'] = intval($user['uid']);
 	$_SESSION['username'] = $user['username'];
	$_SESSION['utype']=intval($user['utype']);
	}
	if ($setcookie)
	{
		$expire=intval($cookie_expire)>0?time()+3600*24*$cookie_expire:0;
		setcookie('QS[uid]',$user['uid'],$expire,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[username]',$user['username'],$expire,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[password]',$user['password'],$expire,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[utype]',$user['utype'], $expire,$QS_cookiepath,$QS_cookiedomain);
	}
	if ($record)
	{
    	$last_login_time = $timestamp;
		$last_login_ip = $online_ip;
		$sql = "UPDATE ".table('members')." SET last_login_time = '$last_login_time', last_login_ip = '$last_login_ip' WHERE uid='{$_SESSION['uid']}'  LIMIT 1";
		$db->query($sql);
	}
	return true;
 }
 function wap_get_user_inid($uid)
{
	global $db;
	$uid=intval($uid);
	$sql = "select * from ".table('members')." where uid = '{$uid}' LIMIT 1";
	return $db->getone($sql);
}
// 获取个人简历列表
function wap_get_user_resume($uid)
{
	global $db;
	$uid=intval($uid);
	$sql="select * from ".table("resume")." where uid=$uid";
	return $db->getall($sql);
}
//增加意向职位
function wap_add_resume_jobs($pid,$uid,$str)
{
	global $db;
	$db->query("Delete from ".table('resume_jobs')." WHERE pid='".intval($pid)."'");
	$str=trim($str);
	$arr=explode(".",$str);
	if (is_array($arr) && !empty($arr))
	{
		$setsqlarr['uid']=intval($uid);
		$setsqlarr['pid']=intval($pid);
		$setsqlarr['topclass']=$arr[0];
		$setsqlarr['category']=$arr[1];
		$setsqlarr['subclass']=$arr[2];
		if (!$db->inserttable(table('resume_jobs'),$setsqlarr))return false;
	}
	return true;
}
//增加意向地区
function wap_add_resume_district($pid,$uid,$district,$sdistrict)
{
	global $db;
	$db->query("Delete from ".table('resume_district')." WHERE pid='".intval($pid)."'");
	$setsqlarr['uid']=intval($uid);
	$setsqlarr['pid']=intval($pid);
	$setsqlarr['district']=intval($district);
	$setsqlarr['sdistrict']=intval($sdistrict);
	if (!$db->inserttable(table('resume_district'),$setsqlarr))return false;
	return true;
}
// 添加意向行业
function wap_add_resume_trade($pid,$uid,$str)
{
	global $db;
	$db->query("Delete from ".table('resume_trade')." WHERE pid='".intval($pid)."'");
	$str=trim($str);
	$setsqlarr['uid']=intval($uid);
	$setsqlarr['pid']=intval($pid);
	$setsqlarr['trade']=$str;
	if (!$db->inserttable(table('resume_trade'),$setsqlarr))return false;
	return true;
}
// 绑定帐号自动登录
function wap_weixin_logon($fromUsername,$expire=null)
{
	global $db;
	if($fromUsername){
	$sql ="select uid from ".table('members')." where weixin_openid = '{$fromUsername}' LIMIT 1";
	$usinfo = $db->getone($sql);
	wap_update_user_info($usinfo['uid'],true,true,$expire);
	unset($fromUsername);
	}
}
function wap_check_interview($uid,$jobsid){
	global $db;
	$result = $db->getone("select did from ".table("company_interview")." where `personal_look`=1 and  `resume_uid`=".$uid." and `jobs_id`=".$jobsid);
	return $result;
}
function wap_update_interview($uid,$jobsid){
	global $db;
	$setsqlarr['personal_look'] = 2;
	$db->updatetable(table("company_interview"),$setsqlarr," `resume_uid`=".$uid." and `jobs_id`=".$jobsid );
}
?>