<?php
 /*
 * 74cms 投诉与建议相关函数
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
function get_feedback_list($offset,$perpage,$get_sql= '')
{
	global $db;
	$limit=" LIMIT ".$offset.','.$perpage;
	$sql = "select * from ".table('feedback')." ".$get_sql.$limit;
	$val=$db->getall($sql);
	return $val;
}
function get_feedback_one($id)
{
	global $db;
	$sql = "select * from ".table('feedback')." where id=".intval($id);
	$val=$db->getone($sql);
	return $val;
}
function del_feedback($id)
{
	global $db;
	$return=0;
	if (!is_array($id))$id=array($id);
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
	if (!$db->query("Delete from ".table('feedback')." WHERE id IN (".$sqlin.")")) return false;
	$return=$return+$db->affected_rows();
	}
	return $return;
}
function get_report_list($offset,$perpage,$get_sql= '',$type)
{
	global $db;
	$limit=" LIMIT ".$offset.','.$perpage;
	if($type==1){
		$result = $db->query("SELECT r.*,m.username FROM ".table('report')." AS r ".$get_sql.$limit);
		while($row = $db->fetch_array($result))
		{
		$row['jobs_url']=url_rewrite('QS_jobsshow',array('id'=>$row['jobs_id']));
		$row_arr[] = $row;
		}
	}else{
		$result = $db->query("SELECT r.*,m.username FROM ".table('report_resume')." AS r ".$get_sql.$limit);
		while($row = $db->fetch_array($result))
		{
		$row['resume_url']=url_rewrite('QS_resumeshow',array('id'=>$row['resume_id']));
		$row_arr[] = $row;
		}
	}
	
	return $row_arr;
}
//反馈审核
function report_audit($id,$audit,$type,$rid)
{
	global $db;
	if (!is_array($id))$id=array($id);
	$return=0;
	$sqlin=implode(",",$id);	
	$sqlrin=implode(",",$rid);
	$rule=get_cache('points_rule');
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{	
		if($type==1) {
			if (!$db->query("update  ".table('report')." SET audit='".intval($audit)."'  WHERE id IN (".$sqlin.")")) return false;
		} else {
			if (!$db->query("update  ".table('report_resume')." SET audit='".intval($audit)."'  WHERE id IN (".$sqlin.")")) return false;
		}
		$return=$return+$db->affected_rows();
		//发送站内信
		if($type==1) {
			$result = $db->query("SELECT * FROM ".table('report')." WHERE id IN ({$sqlin})");
		} else {
			$result = $db->query("SELECT * FROM ".table('report_resume')." WHERE id IN ({$sqlin})");
		}
		while($list = $db->fetch_array($result))
		{
			$user_info=get_user($list['uid']);
			if($type==1) {
				$jobsurl=url_rewrite('QS_jobsshow',array('id'=>$list['jobs_id']));
				$setsqlarr['message']="您举报的职位：<a href=\"{$jobsurl}\" target=\"_blank\">{$list['jobs_name']}</a>,经平台核实情况".($audit==2?"属实":"不属实");
			} else {
				// 企业举报简历 获得积分
				if ($audit==2 && $rule['company_report_resume_points']['value']>0)
				{

					report_deal($_SESSION['uid'],$rule['company_report_resume_points']['type'],$rule['company_report_resume_points']['value']);
					$user_points=get_user_points($_SESSION['uid']);
					$operator=$rule['company_report_resume_points']['type']=="1"?"+":"-";
					write_memberslog($user_info['uid'],1,9001,$user_info['username']," 企业举报简历，{$_CFG['points_byname']}({$operator}{$rule['company_report_resume_points']['value']})，(剩余:{$user_points})",1,1016,"企业举报简历","{$operator}{$rule['company_report_resume_points']['value']}","{$user_points}");
				}
				$resumeurl=url_rewrite('QS_resumeshow',array('id'=>$list['resume_id']));
				$setsqlarr['message']="您举报的简历：<a href=\"{$resumeurl}\" target=\"_blank\">{$list['title']}</a>,真实姓名：{$list['fullname']},经平台核实情况".($audit==2?"属实":"不属实");
			}
			$setsqlarr['msgtype']=1;
			$setsqlarr['msgtouid']=$user_info['uid'];
			$setsqlarr['msgtoname']=$user_info['username'];
			$setsqlarr['dateline']=time();
			$setsqlarr['replytime']=time();
			$setsqlarr['new']=1;
			$db->inserttable(table('pms'),$setsqlarr);
		 }
	}
	return $return;
}
function get_user($uid)
{
	global $db;
	$uid=intval($uid);
	$sql = "select * from ".table('members')." where uid = '{$uid}' LIMIT 1";
	return $db->getone($sql);
}
function del_report($id)
{
	global $db;
	$return=0;
	if (!is_array($id))$del_id=array($id);
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
	if (!$db->query("Delete from ".table('report')." WHERE id IN (".$sqlin.")")) return false;
	$return=$return+$db->affected_rows();
	}
	return $return;
}
function del_report_resume($id)
{
	global $db;
	$return=0;
	if (!is_array($id))$del_id=array($id);
	$sqlin=implode(",",$id);
	if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
	{
	if (!$db->query("Delete from ".table('report_resume')." WHERE id IN (".$sqlin.")")) return false;
	$return=$return+$db->affected_rows();
	}
	return $return;
}
function report_deal($uid,$i_type=1,$points=0)
{
		global $db,$timestamp;
		$points_val=get_user_points($uid);
		if ($i_type==1)
		{
		$points_val=$points_val+$points;
		}
		if ($i_type==2)
		{
		$points_val=$points_val-$points;
		$points_val=$points_val<0?0:$points_val;
		}
		$sql = "UPDATE ".table('members_points')." SET points= '{$points_val}' WHERE uid='{$uid}'  LIMIT 1 ";
		return $db->query($sql);
}
function get_user_points($uid)
{
	global $db;
	$sql = "select * from ".table('members_points')." where uid = ".intval($uid)."  LIMIT 1 ";
	$points=$db->getone($sql);
	return $points['points'];
}
?>