<?php
 /*
 * 74cms 管理中心 个人用户相关函数
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
  function get_user_info($uid)
{
	global $db;
	$sql = "select * from ".table('members')." where uid = ".intval($uid)." LIMIT 1";
	return $db->getone($sql);
}
 //检查该简历是否投递过该职位
 function check_jobs_apply($jobs_id,$resume_id,$p_uid)
{
	global $db;
	$sql = "select did from ".table('personal_jobs_apply')." WHERE personal_uid = '".intval($p_uid)."' AND jobs_id='".intval($jobs_id)."'  AND resume_id='".intval($resume_id)."'";
	return $db->getall($sql);
}
 //获取职位信息列表
function get_jobs($offset,$perpage,$get_sql= '')
{
	global $db,$timestamp;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query($get_sql.$limit);
	while($row = $db->fetch_array($result))
	{
	$row['jobs_name']=cut_str($row['jobs_name'],12,0,"...");
	if (!empty($row['highlight']))
	{
	$row['jobs_name']="<span style=\"color:{$row['highlight']}\">{$row['jobs_name']}</span>";
	}
	$row['companyname']=cut_str($row['companyname'],18,0,"...");
	$row['company_url']=url_rewrite('QS_companyshow',array('id'=>$row['company_id']));
	$row['jobs_url']=url_rewrite('QS_jobsshow',array('id'=>$row['id']));
	$get_resume_nolook = $db->getone("select count(*) from ".table('personal_jobs_apply')." where personal_look=1 and jobs_id=".$row['id']);
	$get_resume_all = $db->getone("select count(*) from ".table('personal_jobs_apply')." where jobs_id=".$row['id']);
	$row['get_resume'] = "( ".$get_resume_nolook['count(*)']." / ".$get_resume_all['count(*)']." )";
	$row_arr[] = $row;
	}
	return $row_arr;
}
 //******************************简历部分**********************************
function get_resume_list($offset,$perpage,$get_sql= '')
{
	global $db;
	$limit=" LIMIT ".$offset.','.$perpage;
	$result = $db->query($get_sql.$limit);
	while($row = $db->fetch_array($result))
	{
	$row['resume_url']=url_rewrite('QS_resumeshow',array('id'=>$row['id']));
	$row_arr[] = $row;
	}
	return $row_arr;
}
function del_resume($id)
{
	global $db;
	if (!is_array($id)) $id=array($id);
	$sqlin=implode(",",$id);
	$return=0;
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
	if (!$db->query("Delete from ".table('resume')." WHERE id IN ({$sqlin})")) return false;
	$return=$return+$db->affected_rows();
	if (!$db->query("Delete from ".table('resume_jobs')." WHERE pid IN ({$sqlin}) ")) return false;
	if (!$db->query("Delete from ".table('resume_district')." WHERE pid IN ({$sqlin}) ")) return false;
	if (!$db->query("Delete from ".table('resume_trade')." WHERE pid IN ({$sqlin}) ")) return false;
	if (!$db->query("Delete from ".table('resume_tag')." WHERE pid IN ({$sqlin}) ")) return false;
	if (!$db->query("Delete from ".table('resume_education')." WHERE pid IN ({$sqlin}) ")) return false;
	if (!$db->query("Delete from ".table('resume_training')." WHERE pid IN ({$sqlin}) ")) return false;
	if (!$db->query("Delete from ".table('resume_work')." WHERE pid IN ({$sqlin}) ")) return false;
	if (!$db->query("Delete from ".table('resume_search_rtime')." WHERE id IN ({$sqlin})")) return false;
	if (!$db->query("Delete from ".table('resume_search_key')." WHERE id IN ({$sqlin})")) return false;
	//填写管理员日志
	write_log("删除简历id为".$id."的简历 , 共删除".$return."行", $_SESSION['admin_name'],3);
	return $return;
	}
	return $return;
}
function del_resume_for_uid($uid)
{
	global $db;
	if (!is_array($uid)) $uid=array($uid);
	$sqlin=implode(",",$uid);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		$result = $db->query("SELECT id FROM ".table('resume')." WHERE uid IN (".$sqlin.")");
		while($row = $db->fetch_array($result))
		{
		$rid[]=$row['id'];
		}
		if (empty($rid))
		{
		return true;
		}
		else
		{
		return del_resume($rid);
		}		
	}
}
function edit_resume_audit($id,$audit,$reason,$pms_notice)
{
	global $db,$_CFG;
	$audit=intval($audit);
	if (!is_array($id))  $id=array($id);
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("update  ".table('resume')." SET audit='{$audit}'  WHERE id IN ({$sqlin}) ")) return false;
		if (!$db->query("update  ".table('resume_search_key')." SET audit='{$audit}'  WHERE id IN ({$sqlin}) ")) return false;
		if (!$db->query("update  ".table('resume_search_rtime')." SET audit='{$audit}'  WHERE id IN ({$sqlin}) ")) return false;
		// distribution_resume($id);
		//填写管理员日志
		write_log("修改简历id为".$sqlin."的审核状态为".$audit, $_SESSION['admin_name'],3);
		//发送站内信
		if ($pms_notice=='1')
		{
				$result = $db->query("SELECT  fullname,title,uid  FROM ".table('resume')." WHERE id IN ({$sqlin})");
				$reason=$reason==''?'原因：未知':'原因：'.$reason;
				while($list = $db->fetch_array($result))
				{
					$user_info=get_user($list['uid']);
					$setsqlarr['message']=$audit=='1'?"您创建的简历：{$list['title']},真实姓名：{$list['fullname']},成功通过网站管理员审核！":"您创建的简历：{$list['title']},真实姓名：{$list['fullname']},未通过网站管理员审核,{$reason}";
					$setsqlarr['msgtype']=1;
					$setsqlarr['msgtouid']=$user_info['uid'];
					$setsqlarr['msgtoname']=$user_info['username'];
					$setsqlarr['dateline']=time();
					$setsqlarr['replytime']=time();
					$setsqlarr['new']=1;
					$db->inserttable(table('pms'),$setsqlarr);
				 }
		}
		//审核未通过增加原因
		if($audit=='3'){
			foreach($id as $list){
				$auditsqlarr['resume_id']=$list;
				$auditsqlarr['reason']=$reason;
				$auditsqlarr['addtime']=time();
				$db->inserttable(table('audit_reason'),$auditsqlarr);
			}
		}
			
			//发送邮件
				$mailconfig=get_cache('mailconfig');//获取邮件规则
				$sms=get_cache('sms_config');
				if ($audit=="1" && $mailconfig['set_resumeallow']=="1")//审核通过
				{
						$result = $db->query("SELECT * FROM ".table('resume')." WHERE id IN ({$sqlin}) ");
						while($list = $db->fetch_array($result))
						{
						dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid=".$list['uid']."&key=".asyn_userkey($list['uid'])."&act=set_resumeallow");
						}
				}
				if ($audit=="3" && $mailconfig['set_resumenotallow']=="1")//审核未通过
				{
					$result = $db->query("SELECT * FROM ".table('resume')." WHERE id IN ({$sqlin}) ");
						while($list = $db->fetch_array($result))
						{
						dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid=".$list['uid']."&key=".asyn_userkey($list['uid'])."&act=set_resumenotallow");
						}
				}
				//sms		
				if ($audit=="1" && $sms['open']=="1" && $sms['set_resumeallow']=="1" )
				{
					$result = $db->query("SELECT * FROM ".table('resume')." WHERE id IN ({$sqlin}) ");
						while($list = $db->fetch_array($result))
						{
							$user_info=get_user($list['uid']);
							if ($user_info['mobile_audit']=="1")
							{
							dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_sms.php?uid=".$list['uid']."&key=".asyn_userkey($list['uid'])."&act=set_resumeallow");
							}
						}
				}
				//sms
				if ($audit=="3" && $sms['open']=="1" && $sms['set_resumenotallow']=="1" )//认证未通过
				{
					$result = $db->query("SELECT * FROM ".table('resume')." WHERE id IN ({$sqlin}) ");
						while($list = $db->fetch_array($result))
						{
							$user_info=get_user($list['uid']);
							if ($user_info['mobile_audit']=="1")
							{
							dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_sms.php?uid=".$list['uid']."&key=".asyn_userkey($list['uid'])."&act=set_resumenotallow");
							}
						}
				}
				//sms
			//发送邮件
	return true;
	}
	return false;
}
//修改照片审核状态
function edit_resume_photoaudit($id,$audit,$is_del_img)
{
	global $db,$_CFG;
	$audit=intval($audit);
	$is_del_img=intval($is_del_img);
	if (!is_array($id)) $id=array($id);
	if (!empty($id))
	{
		foreach($id as $i)
		{
			$i=intval($i);
			$tb1=$db->getone("select photo_img,photo_audit,photo_display from ".table('resume')." WHERE id='{$i}' LIMIT  1");
			if (!empty($tb1))
			{
				if($is_del_img==1 && $audit==3)
				{
					$photo=0;
					@unlink(QISHI_ROOT_PATH.'data/photo/'.$tb1['photo_img']);
					@unlink(QISHI_ROOT_PATH.'data/photo/thumb/'.$tb1['photo_img']);
					$db->query("update  ".table('resume')." SET photo_img='',photo_audit='{$audit}',photo='{$photo}' WHERE id='{$i}' LIMIT 1 ");
					$db->query("update  ".table('resume_search_rtime')." SET photo='{$photo}' WHERE id='{$i}' LIMIT 1 ");
					$db->query("update  ".table('resume_search_key')." SET photo='{$photo}' WHERE id='{$i}' LIMIT 1 ");
				}
				else
				{
					if ($tb1['photo_img'] && $audit=="1" && $tb1['photo_display']=="1")
					{
					$photo=1;
					}
					else
					{
					$photo=0;
					}	
					$db->query("update  ".table('resume')." SET photo_audit='{$audit}',photo='{$photo}' WHERE id='{$i}' LIMIT 1 ");
					$db->query("update  ".table('resume_search_rtime')." SET photo='{$photo}' WHERE id='{$i}' LIMIT 1 ");
					$db->query("update  ".table('resume_search_key')." SET photo='{$photo}' WHERE id='{$i}' LIMIT 1 ");
				}
			}
		}
		//填写管理员日志
		write_log("修改简历id为".$id."的照片审核状态为".$audit, $_SESSION['admin_name'],3);
	}
	return true;
}
//修改人才等级
function edit_resume_talent($id,$talent)
{
	global $db;
	$talent=intval($talent);
	if (!is_array($id)) $id=array($id);
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("update  ".table('resume')." SET talent={$talent}  WHERE id IN ({$sqlin})")) return false;
		if (!$db->query("update  ".table('resume_search_rtime')." SET talent={$talent}  WHERE id IN ({$sqlin})")) return false;
		if (!$db->query("update  ".table('resume_search_key')." SET talent={$talent}  WHERE id IN ({$sqlin})")) return false;
		//填写管理员日志
		write_log("修改简历id为".$sqlin."的人才等级为".$talent, $_SESSION['admin_name'],3);
		return true;
	}
	return false;
}
//从UID获取所有简历
function get_resume_uid($uid)
{
	global $db;
	$uid=intval($uid);
	$result = $db->query("select * FROM ".table('resume')." where uid='{$uid}'");
	while($row = $db->fetch_array($result))
	{ 
	$row['resume_url']=url_rewrite('QS_resumeshow',array('id'=>$row['id']));
	$row_arr[] = $row;
	}
	return $row_arr;	
}
function refresh_resume($id)
{
	global $db;
	$return=0;
	if (!is_array($id))$id=array($id);
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
		if (!$db->query("update  ".table('resume')." SET refreshtime='".time()."'  WHERE id IN (".$sqlin.")")) return false;
		$return=$return+$db->affected_rows();
		if (!$db->query("update  ".table('resume_search_rtime')." SET refreshtime='".time()."'  WHERE id IN (".$sqlin.")")) return false;
		if (!$db->query("update  ".table('resume_search_key')." SET refreshtime='".time()."'  WHERE id IN (".$sqlin.")")) return false;
	}
	//填写管理员日志
	write_log("刷新简历id为".$sqlin."的简历 , 共刷新".$return."行", $_SESSION['admin_name'],3);
	return $return;
}
//**************************个人会员列表
function get_member_list($offset,$perpage,$get_sql= '')
{
	global $db;
	$row_arr = array();
	$limit=" LIMIT ".$offset.','.$perpage;	
	$result = $db->query("SELECT * FROM ".table('members')." as m ".$get_sql.$limit);
		while($row = $db->fetch_array($result))
		{
			$address = $db->getone("select log_address,log_id,log_uid from ".table("members_log")." where log_type = '1000' and log_uid = ".$row['uid']." order by log_id asc limit 1");
			$row['ipAddress'] = $address['log_address'];
			$row_arr[] = $row;
		}
	return $row_arr;
}
function delete_member($uid)
{
	global $db;
	if (!is_array($uid)) $uid=array($uid);
	$sqlin=implode(",",$uid);
		if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
		{
		if (!$db->query("Delete from ".table('members')." WHERE uid IN (".$sqlin.")")) return false;
		if (!$db->query("Delete from ".table('members_info')." WHERE uid IN (".$sqlin.")")) return false;
		//填写管理员日志
		write_log("删除uid为".$sqlin."的会员", $_SESSION['admin_name'],3);
		return true;
		}
	return false;
}
function get_member_one($memberuid)
{
	global $db;
	$sql = "select * from ".table('members')." where uid=".intval($memberuid)." LIMIT 1";
	$val=$db->getone($sql);
	return $val;
}
function get_user($uid)
{
	global $db;
	$uid=intval($uid);
	$sql = "select * from ".table('members')." where uid = '{$uid}' LIMIT 1";
	return $db->getone($sql);
}
//获取简历的审核日志
function get_resumeaudit_one($resume_id){
	global $db;
	$sql = "select * from ".table('audit_reason')."  WHERE resume_id='".intval($resume_id)."' ORDER BY id DESC";
	return $db->getall($sql);
}
//获取简历基本信息
function get_resume_basic($uid,$id)
{
	global $db;
	$id=intval($id);
	$uid=intval($uid);
	$info=$db->getone("select * from ".table('resume')." where id='{$id}'  AND uid='{$uid}' LIMIT 1 ");
	
	if (empty($info))
	{
	return false;
	}
	else
	{
	$info['age']=date("Y")-$info['birthdate'];
	$info['number']="N".str_pad($info['id'],7,"0",STR_PAD_LEFT);
	$info['lastname']=$info['fullname'];
	return $info;
	}
}
//获取教育经历列表
function get_resume_education($uid,$pid)
{
	global $db;
	if (intval($uid)!=$uid) return false;
	$sql = "SELECT * FROM ".table('resume_education')." WHERE  pid='".intval($pid)."' AND uid='".intval($uid)."' ";
	return $db->getall($sql);
}
//获取：工作经历
function get_resume_work($uid,$pid)
{
	global $db;
	$sql = "select * from ".table('resume_work')." where pid='".$pid."' AND uid=".intval($uid)."" ;
	return $db->getall($sql);
}
//获取：培训经历列表
function get_resume_training($uid,$pid)
{
	global $db;
	$sql = "select * from ".table('resume_training')." where pid='".intval($pid)."' AND  uid='".intval($uid)."' ";
	return $db->getall($sql);
}
//获取意向职位
function get_resume_jobs($pid)
{
	global $db;
	$pid=intval($pid);
	$sql = "select * from ".table('resume_jobs')." where pid='{$pid}'  LIMIT 20" ;
	return $db->getall($sql);
}
function reasonaudit_del($id)
{
	global $db;
	if (!is_array($id)) $id=array($id);
	$sqlin=implode(",",$id);
	if (!preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin)) return false;
	if (!$db->query("Delete from ".table('audit_reason')." WHERE id IN ({$sqlin})")) return false;
	//填写管理员日志
	write_log("后台删除日志id为".$sqlin."的日志", $_SESSION['admin_name'],3);
	return $db->affected_rows();
}
//修改用户状态
function set_user_status($status,$uid)
{
	global $db;
	$status=intval($status);
	$uid=intval($uid);
	if (!$db->query("UPDATE ".table('members')." SET status= {$status} WHERE uid={$uid} LIMIT 1")) return false;
	//填写管理员日志
	write_log("后台将uid为".$uid."会员的用户状态修改为".$status, $_SESSION['admin_name'],3);
	return true;
}
?>