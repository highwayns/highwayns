<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__) . '/personal_common.php');
$smarty->assign('leftmenu',"index");
if ($act=='index')
{
	$uid=intval($_SESSION['uid']);
	$smarty->assign('title','個人会員センター - '.$_CFG['site_name']);
	$smarty->assign('user',$user);

	require_once(HIGHWAY_ROOT_PATH.'include/fun_user.php');
	$smarty->assign('loginlog',get_loginlog_one($uid,'1001'));
	$wheresql=" WHERE uid='".$_SESSION['uid']."' ";
	$sql="SELECT * FROM ".table('resume').$wheresql;
	$smarty->assign('rand',rand(1,100));
	$smarty->assign('my_resume',get_resume_list($sql));
	$smarty->assign('count_resume',count_resume($uid));
	$smarty->assign('count_interview',count_interview($uid));
	$smarty->assign('count_apply',count_personal_jobs_apply($uid));
	$smarty->assign('msg_total1',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='1' AND `replyuid`<>'{$uid}' AND msgtype=1"));
	$smarty->assign('msg_total2',$db->get_total("SELECT COUNT(*) AS num FROM ".table('pms')." WHERE (msgfromuid='{$uid}' OR msgtouid='{$uid}') AND `new`='2' AND `replyuid`<>'{$uid}' AND msgtype=1"));
	//首页提示消息(最近两周 下载 和 面试邀请的信息)
	$message = array();
	$time = strtotime("- 14 day");
	$down_resume = $db->getall("SELECT distinct company_uid , company_name FROM ".table('company_down_resume')." WHERE resume_uid='".$_SESSION['uid']."' AND down_addtime > ".$time);
	foreach ($down_resume as $key => $value) 
	{
		$company_id = $db->getone("SELECT id FROM ".table('company_profile')." WHERE uid=".$value['company_uid']." LIMIT 1");
		$company_url = url_rewrite('HW_companyshow',array('id'=>$company_id['id']));
		$message[] ="履歴書を<a href=\"".$company_url."\" target=\"_black\" class=\"underline\">【".$value['company_name']."】</a>ダウンロード！募集会社に連絡して仕事チャンス得る！";
	}
	$inter_resume = $db->getall("SELECT distinct company_id , company_name FROM ".table('company_interview')." WHERE resume_uid='".$_SESSION['uid']."' AND interview_addtime > ".$time." AND personal_look=1 ");
	foreach ($inter_resume as $key => $value) 
	{
		$company_url = url_rewrite('HW_companyshow',array('id'=>$value['company_id']));
		$message[] ="<a href=\"".$company_url."\" target=\"_black\" class=\"underline\">【".$value['company_name']."】</a>面接誘いました，募集会社に連絡してください！";
	}
	$smarty->assign('message',$message);
	$smarty->display('member_personal/personal_index.htm');
}
elseif($act=='ajax_get_interest_jobs'){
	global $_CFG;
	$uid=intval($_SESSION['uid']);
	$pid=intval($_GET['pid']);
	$html = "";
	$interest_id = get_interest_jobs_id_by_resume($uid,$pid);
	$jobs_list = get_interest_jobs_list($interest_id);
	if(!empty($jobs_list)){
		foreach($jobs_list as $k=>$v){
			$jobs_url = url_rewrite("HW_jobsshow",array("id"=>$v['id']));
			$company_url = url_rewrite("HW_companyshow",array("id"=>$v['company_id']));
			$html.= '<table>
						<tbody>
							<tr>
								<td class="frist" width="117"><div class="index-line1"><a target="_black" href="'.$jobs_url.'" class="underline job-link">'.$v["jobs_name"].'</a></div></td>
								<td width="228"><div class="index-line2"><a target="_black" href="'.$company_url.'" class="underline com-link">'.$v["companyname"].'</a></div></td>
								<td width="139"><div class="index-line3">'.$v["district_cn"].'</div></td>
								<td width="195"><span class="yh">'.$v["wage_cn"].'</span></td>
								<td width="75">'.date("Y-m-d",$v["refreshtime"]).'</td>
							</tr>
						</tbody>
					</table>';
		}
	}
	exit($html);
}
unset($smarty);
?>
