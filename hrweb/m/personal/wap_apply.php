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
define('IN_QISHI', true);

require_once(dirname(__FILE__).'/../../include/common.inc.php');
require_once(QISHI_ROOT_PATH.'include/fun_wap.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
require_once(QISHI_ROOT_PATH.'include/fun_personal.php');
$smarty->cache = false;
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'apply';
if ($_SESSION['uid']=='' || $_SESSION['username']==''||intval($_SESSION['utype'])==1)
{
	header("Location: ../wap_login.php");
}
elseif ($act == 'apply')
{

	$wheresql=" WHERE a.personal_uid='{$_SESSION['uid']}' ";
	$perpage = 5;
	$count  = 0;
	$page = empty($_GET['page'])?1:intval($_GET['page']);
	if($page<1) $page = 1;
	$start = ($page-1)*$perpage;
	$total_sql="SELECT COUNT(*) AS num FROM  ".table('personal_jobs_apply')." as a  {$wheresql}";
	$count=$db->get_total($total_sql);
	$limit=" LIMIT {$start},{$perpage}";
	$sql="select a.*,j.wage_cn from ".table("personal_jobs_apply")." as a left join ".table("jobs")." as j on a.jobs_id=j.id  $wheresql order by apply_addtime desc ".$limit;
	$apply=$db->getall($sql);

	$smarty->assign('apply',$apply);
	// $smarty->assign('pagehtml',wapmulti($count, $perpage, $page, $theurl));
	$smarty->display("wap/personal/wap-apply.html");	
}
elseif ($act == 'apply_add')
{
	$jobsid=intval($_POST["jobs_id"])?intval($_POST["jobs_id"]):exit("出错了");
	$resumeid=intval($_POST["resume_id"])?intval($_POST["resume_id"]):exit("出错了");
	
	$_POST=array_map("utf8_to_gbk", $_POST);
	$sql="select * from ".table("personal_jobs_apply")." where personal_uid=".intval($_SESSION['uid'])." and resume_id=".intval($_POST["resume_id"])." and jobs_id=".intval($_POST["jobs_id"])."";
	$row=$db->getone($sql);
		
		$resume_basic=get_resume_basic($_SESSION['uid'],$resumeid);
		$resume_basic = array_map("addslashes", $resume_basic);
		if (empty($resume_basic))
		{
		exit("简历丢失");
		}
	
	if($_SESSION['utype']!=2){
		exit("个人会员请登录后申请职位");
	}
	elseif($row){
		exit("您已经申请过此职位！");
	}
	else{
	
		if (check_jobs_apply($jobs['id'],$resumeid,$_SESSION['uid']))
			{
			 continue ;
			}
			if ($resume_basic['display_name']=="2")
			{
				$personal_fullname="N".str_pad($resume_basic['id'],7,"0",STR_PAD_LEFT);
			}
			elseif($resume_basic['display_name']=="3")
			{
				if($resume_basic['sex']==1)
				{
					$personal_fullname=cut_str($resume_basic['fullname'],1,0,"先生");
				}
				elseif($resume_basic['sex']==2)
				{
					$personal_fullname=cut_str($resume_basic['fullname'],1,0,"女士");
				}
			}
			else
			{
				$personal_fullname=$resume_basic['fullname'];
			}
			
		$setsqlarr["jobs_id"]=intval($_POST["jobs_id"]);
		$setsqlarr["jobs_name"]=$_POST["jobs_name"];
		$setsqlarr["company_id"]=intval($_POST["company_id"]);
		$setsqlarr["company_name"]=$_POST["company_name"];
		$setsqlarr["company_uid"]=intval($_POST["company_uid"]);
		$setsqlarr["resume_id"]=intval($_POST["resume_id"]);
		$setsqlarr["resume_name"]=$_POST["resume_title"];
		$setsqlarr["personal_uid"]=intval($_SESSION["uid"]);
		$setsqlarr["apply_addtime"]=time();
		if($db->inserttable(table('personal_jobs_apply'),$setsqlarr)){
		
			$sql="select * from ".table("jobs")." where  id = ".$setsqlarr["jobs_id"];
			$jobs=$db->getone($sql);
			
			
			$mailconfig=get_cache('mailconfig');					
					$jobs['contact']=$db->getone("select * from ".table('jobs_contact')." where pid='{$jobs['id']}' LIMIT 1 ");
					$sms=get_cache('sms_config');	
					$comuser=get_user_info($jobs['uid']);	
					if ($mailconfig['set_applyjobs']=="1"  && $comuser['email_audit']=="1" && $jobs['contact']['notify']=="1")
					{	
						dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_mail.php?uid={$_SESSION['uid']}&key=".asyn_userkey($_SESSION['uid'])."&act=jobs_apply&jobs_id={$jobs['id']}&jobs_name={$jobs['jobs_name']}&personal_fullname={$personal_fullname}&email={$comuser['email']}&resume_id={$resumeid}");
					}
					//sms	
					if ($sms['open']=="1"  && $sms['set_applyjobs']=="1"  && $comuser['mobile_audit']=="1")
					{
					//修正bug,求职者申请职位不发送短信
						dfopen($_CFG['site_domain'].$_CFG['site_dir']."plus/asyn_sms.php?uid={$_SESSION['uid']}&key=".asyn_userkey($_SESSION['uid'])."&act=jobs_apply&jobs_id=".$jobs['id']."&jobs_name=".$jobs['jobs_name'].'&jobs_uid='.$jobs['uid']."&personal_fullname=".$personal_fullname."&mobile=".$comuser['mobile']);
					}
					//站内信
					if($pms_notice=='1'){
						$user=$db->getone("select username from ".table('members')." where uid ={$jobs['uid']} limit 1");
						$user = array_map("addslashes", $user);
						$jobs_url=url_rewrite('QS_jobsshow',array('id'=>$jobs['id']));
						$resume_url=url_rewrite('QS_resumeshow',array('id'=>$resumeid));
						$message=$personal_fullname."申请了您发布的职位：<a href=\"{$jobs_url}\" target=\"_blank\">{$jobs['jobs_name']}</a>,<a href=\"{$resume_url}\" target=\"_blank\">点击查看</a>";
						write_pmsnotice($jobs['uid'],$user['username'],$message);
					}
					write_memberslog($_SESSION['uid'],2,1301,$_SESSION['username'],"投递了简历，职位:{$jobs['jobs_name']}");
					
					
					//微信
					if(intval($_CFG['weixin_apiopen'])==1){
						$user=$db->getone("select weixin_openid from ".table('members')." where uid = {$jobs['uid']} limit 1");
						if($user['weixin_openid']!=""){
							$resume_url=$_CFG['wap_domain']."/wap-resume-show.php?id=".$resumeid;
							$template = array(
								'touser' => $user['weixin_openid'],
								'template_id' => "u_yoFifHb-ryYXMtNSlATj_Wfm1CWTKEjf8EkiM6dvY",
								'url' => $resume_url,
								'topcolor' => "#7B68EE",
								'data' => array(
									'first' => array('value' => urlencode(gbk_to_utf8("你收到了一份新简历，请及时登录".$_CFG['site_name']."查阅")),
													'color' => "#743A3A",
										),
									'job' => array('value' => urlencode(gbk_to_utf8($jobs['jobs_name'])),
													'color' => "#743A3A",
										),
									'resuname' => array('value' => urlencode(gbk_to_utf8("--")),
													'color' => "#743A3A",
										),
									'realname' => array('value' => urlencode(gbk_to_utf8($personal_fullname)),
													'color' => "#743A3A",
										),
									'exp' => array('value' => urlencode(gbk_to_utf8($resume_basic['experience_cn'])),
													'color' => "#743A3A",
										),
									'lastjob' => array('value' => urlencode(gbk_to_utf8("--")),
													'color' => "#743A3A",
										),
									'remark' => array('value' => urlencode("\\n".$notes),
													'color' => "#743A3A",
										)
									)
								);
							send_template_message(urldecode(json_encode($template)));
						}
					}
		
		
			exit("ok");
		}else{
			exit("err");
		}
	}
	
}
?>