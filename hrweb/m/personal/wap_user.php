<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_wap.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
require_once(HIGHWAY_ROOT_PATH.'include/fun_personal.php');
$smarty->cache = false;
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'index';
if (intval($_SESSION['uid'])=='' || $_SESSION['username']==''||intval($_SESSION['utype'])==1)
{
	header("Location: ../wap_login.php");
}
elseif ($act == 'index')
{
	$smarty->cache = false;
	$user=wap_get_user_info(intval($_SESSION['uid']));	
	$smarty->assign('user',$user);
	$resume_info=get_userprofile(intval($_SESSION['uid']));
	if(empty($resume_info))
	{
		header("Location: ?act=make_resume");
	}
	else
	{
		$resume_info['age']=date("Y")-$resume_info['birthday'];
		$smarty->assign('resume_info',$resume_info);
		$smarty->display("wap/personal/wap-user-personal-index.html");
	}
	
}
elseif ($act == 'favorites')
{
	
	$perpage = 5;
	$count  = 0;
	$page = empty($_GET['page'])?1:intval($_GET['page']);
	if($page<1) $page = 1;
	$theurl = "wap_user.php?act=favorites";
	$start = ($page-1)*$perpage;
	$wheresql=" WHERE f.personal_uid='{$_SESSION['uid']}' ";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('personal_favorites')." AS f {$wheresql} ";
	$count=$db->get_total($total_sql);
	$joinsql=" LEFT JOIN ".table('jobs')." as  j  ON f.jobs_id=j.id ";
	$smarty->assign('favorites',get_favorites($start,$perpage,$joinsql.$wheresql));
	$smarty->assign('pagehtml',wapmulti($count, $perpage, $page, $theurl));
	$smarty->display('wap/personal/wap-collect.html');
}
elseif ($act == 'add_favorites')
{
	$id=isset($_POST['id'])?intval($_POST['id']):exit("err");
	if(intval($_SESSION['utype']!=2)){
		exit("個人会員登録後職位をお気に入り");
	}
	elseif(add_favorites($id,intval($_SESSION['uid']))==0)
	{
	exit("この職位がお気に入り中既に存在します");
	}
	else
	{
	exit("ok");
	}
}
// 填写简历
elseif($act == "make_resume")
{
	$smarty->cache = false;
	$uid=intval($_SESSION['uid']);
	$smarty->assign('user',$user);
	$smarty->assign('userprofile',get_userprofile($_SESSION['uid']));
	$smarty->display('wap/personal/wap_make_resume.html');
}
elseif($act == "make_resume_save")
{	
	$_POST=array_map("utf8_to_gbk",$_POST);
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):"名称無し履歴書";
	$setsqlarr['uid']=$_SESSION['uid'];
	$setsqlarr['fullname']=trim($_POST['fullname'])?trim($_POST['fullname']):exit("名前を入力してください");
	$setsqlarr['display_name']=1;
	$setsqlarr['sex']=trim($_POST['sex'])?trim($_POST['sex']):exit("選択性别を選択してください");
	$setsqlarr['sex_cn']=trim($_POST['sex_cn'])?trim($_POST['sex_cn']):exit("選択性别を選択してください");
	$setsqlarr['birthdate']=intval($_POST['birthdate'])?intval($_POST['birthdate']):exit("出生年を選択してください");
	$setsqlarr['residence']=trim($_POST['residence']);
	$setsqlarr['education']=intval($_POST['education'])?intval($_POST['education']):exit("学歴を選択してください");
	$setsqlarr['education_cn']=trim($_POST['education_cn'])?trim($_POST['education_cn']):exit("学歴を選択してください");
	$setsqlarr['experience']=intval($_POST['experience'])?intval($_POST['experience']):exit("仕事経験を選択してください");
	$setsqlarr['experience_cn']=trim($_POST['experience_cn'])?trim($_POST['experience_cn']):exit("仕事経験を選択してください");
	$setsqlarr['email']=trim($_POST['email'])?trim($_POST['email']):exit("メールボックスを入力してください");
	$setsqlarr['email_notify']=$_POST['email_notify']=="1"?1:0;
	$setsqlarr['telephone']=trim($_POST['telephone'])?trim($_POST['telephone']):exit("携帯番号");
	$setsqlarr['intention_jobs']=trim($_POST['intention_jobs'])?trim($_POST['intention_jobs']):exit("期望職位を選択してください");
	$_POST['intention_jobs_id']=trim($_POST['intention_jobs_id'])?trim($_POST['intention_jobs_id']):exit("期望職位を選択してください");
	$setsqlarr['trade']=trim($_POST['trade'])?trim($_POST['trade']):exit("期望業界を選択してください");
	$setsqlarr['trade_cn']=trim($_POST['trade_cn'])?trim($_POST['trade_cn']):exit("期望業界を選択してください");
	// $setsqlarr['district']=trim($_POST['district']);
	// $setsqlarr['sdistrict']=intval($_POST['sdistrict']);
	$setsqlarr['district_cn']=trim($_POST['district_cn'])?trim($_POST['district_cn']):exit("期望仕事地区");
	$setsqlarr['nature']=intval($_POST['nature'])?intval($_POST['nature']):exit("仕事性质を選択してください");
	$setsqlarr['nature_cn']=trim($_POST['nature_cn'])?trim($_POST['nature_cn']):exit("仕事性质を選択してください");
	$setsqlarr['wage']=intval($_POST['wage'])?intval($_POST['wage']):exit("期望給料を選択してください");
	$setsqlarr['wage_cn']=trim($_POST['wage_cn'])?trim($_POST['wage_cn']):exit("期望給料を選択してください");
	$setsqlarr['specialty']=trim($_POST['specialty'])?trim($_POST['specialty']):exit("自我紹介を入力してください");
	$setsqlarr['refreshtime']=time();
	$setsqlarr['audit']=intval($_CFG['audit_resume']);
	//1->PC  2->APP  3->wap
	$setsqlarr['resume_from_pc']=3;
	$total=$db->get_total("SELECT COUNT(*) AS num FROM ".table('resume')." WHERE uid='{$_SESSION['uid']}'");
	if ($total>=intval($_CFG['resume_max']))
	{
	exit("最大{$_CFG['resume_max']} 件履歴書を作成できます,制限を超えました！");
	}
	else
	{
	$setsqlarr['addtime']=time();
	$pid=$db->inserttable(table('resume'),$setsqlarr,1);
	$searchtab['id'] = $pid;
	$searchtab['uid'] = intval($_SESSION['uid']);
	$district_arr['uid']=intval($_SESSION['uid']);
	$district_arr['pid']=$pid;
	$district_arr['district']=trim($_POST['district']);
	$district_arr['sdistrict']=trim($_POST['sdistrict']);
	$db->inserttable(table('resume_district'),$district_arr);
	
	$db->inserttable(table('resume_search_key'),$searchtab);
	$db->inserttable(table('resume_search_rtime'),$searchtab);
	if (empty($pid))exit("保存失敗！");

	if(!wap_add_resume_jobs($pid,intval($_SESSION['uid']),$_POST["intention_jobs_id"]))exit('err');
	check_resume($_SESSION['uid'],$pid);
	if(intval($_POST['entrust'])){
		set_resume_entrust($pid);
	}
	write_memberslog(intval($_SESSION['uid']),2,1101,$_SESSION['username'],"履歴書を作成済み");
	
	if(!get_userprofile(intval($_SESSION['uid']))){
		$infoarr['realname']=$setsqlarr['fullname'];
		$infoarr['sex']=$setsqlarr['sex'];
		$infoarr['sex_cn']=$setsqlarr['sex_cn'];
		$infoarr['birthday']=$setsqlarr['birthdate'];
		$infoarr['residence']=$setsqlarr['residence'];
		$infoarr['residence']=$setsqlarr['residence'];
		$infoarr['education']=$setsqlarr['education'];
		$infoarr['education_cn']=$setsqlarr['education_cn'];
		$infoarr['experience']=$setsqlarr['experience'];
		$infoarr['experience_cn']=$setsqlarr['experience_cn'];
		$infoarr['phone']=$setsqlarr['telephone'];
		$infoarr['email']=$setsqlarr['email'];
		$infoarr['uid']=intval($_SESSION['uid']);
		$db->inserttable(table('members_info'),$infoarr);
	}
	baidu_submiturl(url_rewrite('HW_resumeshow',array('id'=>$pid)),'addresume');
	echo $pid;
	// header("Location: ?act=resume_success&pid=".$pid);
	}
}
// 填写简历完成
elseif($act == "resume_success")
{
	$smarty->cache = false;
	$id=intval($_GET['pid']);
	$sql="select j.* from ".table("jobs")." as j left join ".table("resume_jobs")." as r on r.category=j.category where r.pid=$id limit 5";
	$resume_jobs=$db->getall($sql);
	$smarty->assign('resume_jobs',$resume_jobs);
	$smarty->display('wap/personal/wap-create-resume-success.html');
}
// 简历列表
elseif($act == "resume_list")
{	
	$smarty->cache = false;
	$wheresql=" WHERE uid='".intval($_SESSION['uid'])."' ";
	$sql="SELECT * FROM ".table('resume').$wheresql;
	$resume_list=get_resume_list($sql,12,true,true,true);
	$smarty->assign('resume_list',$resume_list);
	$smarty->display('wap/personal/wap-resume-index.html');
}
// 完善简历
elseif($act == "resume_one")
{
	$smarty->cache = false;
	$id=intval($_GET['pid']);
	$resume_one=resume_one($id);
	$smarty->assign('resume_one',$resume_one);
	$smarty->assign('resume_basic',get_resume_basic(intval($_SESSION['uid']),$id));
	$smarty->assign('resume_jobs',get_resume_jobs($id));
	$smarty->assign('resume_education',get_resume_education(intval($_SESSION['uid']),$id));
	$smarty->assign('resume_work',get_resume_work(intval($_SESSION['uid']),$id));
	$smarty->assign('resume_training',get_resume_training(intval($_SESSION['uid']),$id));

	$smarty->display('wap/personal/wap-comlpete-resume.html');
}
elseif($act == "resume_basic")
{
	$smarty->cache = false;
	$id=intval($_GET['pid']);
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),$id);
	// var_dump($resume_basic);
	$smarty->assign('userprofile',get_userprofile(intval($_SESSION['uid'])));
	$smarty->assign('resume_basic',$resume_basic);
	$smarty->display('wap/personal/wap-personal-info.html');
}
elseif($act == "resume_basic_save")
{
	$smarty->cache = false;
	$_POST=array_map("utf8_to_gbk",$_POST);
	$setsqlarr['uid']=intval($_SESSION['uid']);
	$setsqlarr['fullname']=trim($_POST['fullname'])?trim($_POST['fullname']):exit("名前を入力してください");
	$setsqlarr['display_name']=intval($_POST['display_name']);
	$setsqlarr['sex']=trim($_POST['sex'])?trim($_POST['sex']):exit("選択性别を選択してください");
	$setsqlarr['sex_cn']=trim($_POST['sex_cn'])?trim($_POST['sex_cn']):exit("選択性别を選択してください");
	$setsqlarr['birthdate']=intval($_POST['birthdate'])?intval($_POST['birthdate']):exit("出生年を選択してください");
	$setsqlarr['residence']=trim($_POST['residence'])?trim($_POST['residence']):exit("アドレスを選択してください");
	$setsqlarr['residence']=trim($_POST['residence'])?trim($_POST['residence']):exit("アドレスを選択してください");
	$setsqlarr['education']=intval($_POST['education'])?intval($_POST['education']):exit("学歴を選択してください");
	$setsqlarr['education_cn']=trim($_POST['education_cn'])?trim($_POST['education_cn']):exit("学歴を選択してください");
	$setsqlarr['experience']=intval($_POST['experience'])?intval($_POST['experience']):exit("仕事経験を選択してください");
	$setsqlarr['experience_cn']=trim($_POST['experience_cn'])?trim($_POST['experience_cn']):exit("仕事経験を選択してください");
	$setsqlarr['email']=trim($_POST['email'])?trim($_POST['email']):exit("メールボックスを入力してください");
	$setsqlarr['email_notify']=$_POST['email_notify']=="1"?1:0;
	$setsqlarr['telephone']=trim($_POST['telephone'])?trim($_POST['telephone']):exit("携帯番号");
	$db->updatetable(table('resume'),$setsqlarr," id='".intval($_POST['pid'])."'  AND uid='{$setsqlarr['uid']}'");
	check_resume($_SESSION['uid'],intval($_REQUEST['pid']));
	if($_CFG['audit_edit_resume']!="-1"){
		set_resume_entrust(intval($_REQUEST['pid']));
	}
	write_memberslog($_SESSION['uid'],2,1105,$_SESSION['username'],"履歴書変更済み({$_POST['title']})");

	$infoarr['realname']=$setsqlarr['fullname'];
	$infoarr['sex']=$setsqlarr['sex'];
	$infoarr['sex_cn']=$setsqlarr['sex_cn'];
	$infoarr['birthday']=$setsqlarr['birthdate'];
	$infoarr['residence']=$setsqlarr['residence'];
	$infoarr['residence']=$setsqlarr['residence'];
	$infoarr['education']=$setsqlarr['education'];
	$infoarr['education_cn']=$setsqlarr['education_cn'];
	$infoarr['experience']=$setsqlarr['experience'];
	$infoarr['experience_cn']=$setsqlarr['experience_cn'];
	$infoarr['phone']=$setsqlarr['telephone'];
	$infoarr['email']=$setsqlarr['email'];
	$infoarr['uid']=intval($_SESSION['uid']);
	$db->updatetable(table('members_info'),$infoarr," uid={$infoarr['uid']} ");
	exit("ok");
}
// 求职意向 职位
elseif($act == "resume_jobs")
{
	$smarty->cache = false;
	$id=$_GET['pid'];
	$resume_one=resume_one($id);
	$smarty->assign('resume_one',$resume_one);
	$resume_jobs = get_resume_jobs($id);
	$smarty->display('wap/personal/wap-want-job.html');
}
elseif($act == "resume_jobs_save")
{
	$smarty->cache = false;
	$_POST=array_map("utf8_to_gbk",$_POST);
	$setsqlarr['intention_jobs']=trim($_POST['intention_jobs'])?trim($_POST['intention_jobs']):exit("期望職位を選択してください");
	$_POST['intention_jobs_id']=trim($_POST['intention_jobs_id'])?trim($_POST['intention_jobs_id']):exit("期望職位を選択してください");
	$setsqlarr['wage']=trim($_POST['wage'])?trim($_POST['wage']):exit("期望給料を選択してください");
	$setsqlarr['wage_cn']=trim($_POST['wage_cn'])?trim($_POST['wage_cn']):exit("期望給料を選択してください");
	$setsqlarr['nature']=trim($_POST['nature'])?trim($_POST['nature']):exit("期望仕事性质を選択してください");
	$setsqlarr['nature_cn']=trim($_POST['nature_cn'])?trim($_POST['nature_cn']):exit("期望仕事性质を選択してください");
	$setsqlarr['trade']=trim($_POST['trade'])?trim($_POST['trade']):exit("期望業界を選択してください");
	$setsqlarr['trade_cn']=trim($_POST['trade_cn'])?trim($_POST['trade_cn']):exit("期望業界を選択してください");
	$setsqlarr['district_cn']=trim($_POST['district_cn'])?trim($_POST['district_cn']):exit("期望仕事地区を選択してください");
	if(!$db->updatetable(table('resume'),$setsqlarr,array("id"=>intval($_POST['pid']),"uid"=>intval($_SESSION['uid']))))exit("err");
	if(!wap_add_resume_jobs(intval($_POST['pid']),intval($_SESSION['uid']),intval($_POST['intention_jobs_id'])))exit('err');
	if(!wap_add_resume_district(intval($_POST['pid']),intval($_SESSION['uid']),intval($_POST['district']),intval($_POST['sdistrict'])))exit('err');
	if(!wap_add_resume_trade(intval($_POST['pid']),intval($_SESSION['uid']),intval($setsqlarr['trade'])))exit('err');
	exit("ok");
}
//  工作经验
elseif($act == "resume_work_list")
{
	$smarty->cache = false;
	$id=intval($_GET['pid']);
	$resume_work_list=get_resume_work(intval($_SESSION['uid']),$id);
	$smarty->assign('resume_work',get_resume_work(intval($_SESSION['uid']),$id));
	$smarty->display('wap/personal/wap-work-experience.html');
}
// 添加 修改 工作经验
elseif($act == "resume_work_add")
{	
	$smarty->cache = false;
	$id=intval($_GET['id']);
	$resume_work=get_this_work(intval($_SESSION['uid']),$id);
	if($resume_work){
		$smarty->assign('resume_work',$resume_work);
	}else{
		$smarty->assign('resume_work',false);
	}
	$smarty->display('wap/personal/wap-edit-work-experience.html');
}
elseif($act == "resume_work_save")
{
	$_POST=array_map("utf8_to_gbk",$_POST);
	// print_r($_POST);die;
	$id=intval($_POST['id']);
	$setsqlarr['uid'] = intval($_SESSION['uid']);
	$setsqlarr['pid'] = intval($_POST['pid']);

	if ($setsqlarr['uid']==0 || $setsqlarr['pid']==0 )exit('履歴書存在しない');

	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_POST['pid']));
	if (empty($resume_basic)) exit('履歴書基本情報を入力してください');
	$resume_work=get_resume_work($_SESSION['uid'],intval($_POST['pid']));
	if (count($resume_work)>=6)  exit('仕事履歴は6件まで');
	$setsqlarr['companyname'] = trim($_POST['companyname'])?trim($_POST['companyname']):exit('会社の名称を入力してください！');
	$setsqlarr['jobs'] = trim($_POST['jobs'])?trim($_POST['jobs']):exit("職位名称を入力してください！");
	if(trim($_POST['startyear'])==""||trim($_POST['startmonth'])==""||trim($_POST['endyear'])==""||trim($_POST['endmonth'])==""){
		exit("職時間選択してください!");
	}
	$setsqlarr['startyear'] = intval($_POST['startyear']);
	$setsqlarr['startmonth'] = intval($_POST['startmonth']);
	$setsqlarr['endyear'] = intval($_POST['endyear']);
	$setsqlarr['endmonth'] = intval($_POST['endmonth']);
	$setsqlarr['achievements'] = trim($_POST['achievements'])?trim($_POST['achievements']):exit("仕事責務を入力してください！");
	
	if($id){
		$db->updatetable(table("resume_work"),$setsqlarr,array("id"=>$id,"uid"=>intval($_SESSION['uid'])));
		exit("ok");
	}else{
		$insert_id = $db->inserttable(table("resume_work"),$setsqlarr,1);
		if($insert_id){
			check_resume($_SESSION['uid'],intval($_REQUEST['pid']));
			exit("ok");
		}else{
			exit("err");
		}
	}
}
elseif($act == "resume_work_del")
{
	$smarty->cache = false;
	$id=intval($_GET['work_id']);
	$uid=intval($_SESSION["uid"]);
	$sql="delete from ".table("resume_work")." where id=$id and uid=$uid ";
	if($db->query($sql)){
		exit("ok");
	}else{
		exit("err");
	}
}
// 教育经历
elseif($act == "resume_education")
{	
	$smarty->cache = false;
	$id=intval($_GET['pid']);
	$resume_education_list=get_resume_education(intval($_SESSION['uid']),$id);
	// var_dump($resume_education_list);
	$smarty->assign("resume_education_list",$resume_education_list);
	$smarty->display('wap/personal/wap-edu-experience.html');
}
// 添加 修改 教育经历
elseif($act == "resume_education_add")
{	
	$smarty->cache = false;
	$id=intval($_GET["id"]);
	$resume_edu=get_this_education(intval($_SESSION['uid']),$id);
	if($resume_edu){
		$smarty->assign('resume_edu',$resume_edu);
	}else{
		$smarty->assign('resume_edu',false);
	}
	$smarty->display('wap/personal/wap-edit-edu-experience.html');
}
elseif($act == "resume_education_save")
{
	// print_r($_POST);die;
	$_POST=array_map("utf8_to_gbk",$_POST);
	$id=intval($_POST['id']);
	$setsqlarr['uid'] = intval($_SESSION['uid']);
	$setsqlarr['pid'] = intval($_POST['pid']);
	if ($setsqlarr['uid']==0 || $setsqlarr['pid']==0 )exit('履歴書存在しない');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_POST['pid']));
	if (empty($resume_basic)) exit('履歴書基本情報を入力してください');
	$resume_education=get_resume_education($_SESSION['uid'],intval($_POST['pid']));
	if (count($resume_education)>=6)  exit('教育履歴６件まで');
	$setsqlarr['school'] = trim($_POST['school'])?trim($_POST['school']):exit('学校名を入力してください！');
	$setsqlarr['speciality'] = trim($_POST['speciality'])?trim($_POST['speciality']):WapShowMsg("専門名称を入力してください！");
	if(trim($_POST['startyear'])==""||trim($_POST['startmonth'])==""||trim($_POST['endyear'])==""||trim($_POST['endmonth'])==""){
		exit("在校期間を選択してください！");
	}
	$setsqlarr['startyear'] = intval($_POST['startyear']);
	$setsqlarr['startmonth'] = intval($_POST['startmonth']);
	$setsqlarr['endyear'] = intval($_POST['endyear']);
	$setsqlarr['endmonth'] = intval($_POST['endmonth']);
	// $setsqlarr['education'] = trim($_POST['education'])?trim($_POST['education']):WapShowMsg("学歴を選択してください",0);
	// $setsqlarr['education_cn'] = trim($_POST['education_cn'])?trim($_POST['education_cn']):WapShowMsg("学歴を選択してください",0);
	if($id){
		$db->updatetable(table("resume_education"),$setsqlarr,array("id"=>$id,"uid"=>intval($_SESSION['uid'])));
		exit("ok");
	}else{
		$insert_id = $db->inserttable(table("resume_education"),$setsqlarr,1);
		if($insert_id){
			check_resume(intval($_SESSION['uid']),intval($_REQUEST['pid']));
			exit("ok");
		}else{
			exit("err");
		}
	}
}
// 删除教育经历
elseif($act == "resume_education_del")
{
	$smarty->cache = false;
	$id=intval($_GET['education_id']);
	$uid=intval($_SESSION["uid"]);
	$sql="delete from ".table("resume_education")." where id=$id and uid=$uid ";
	if($db->query($sql)){
		exit("ok");
	}else{
		exit("err");
	}
}
// 培训经历 
elseif($act == "resume_train")
{
	$smarty->cache = false;
	$id=intval($_GET['pid']);
	$resume_train_list=get_resume_training(intval($_SESSION['uid']),$id);
	$smarty->assign("resume_train_list",$resume_train_list);
	$smarty->display('wap/personal/wap-train-experience.html');
}
elseif($act == "resume_train_add")
{
	$smarty->cache = false;
	$id=intval($_GET["id"]);
	$resume_train=get_this_training(intval($_SESSION['uid']),$id);
	if($resume_train){
		$smarty->assign('resume_train',$resume_train);
	}else{
		$smarty->assign('resume_train',false);
	}
	$smarty->display('wap/personal/wap-edit-train-experience.html');
}
elseif($act == "resume_train_save")
{
	// print_r($_POST);die;
	$_POST=array_map("utf8_to_gbk",$_POST);
	$id=intval($_POST['id']);
	$setsqlarr['uid'] = intval($_SESSION['uid']);
	$setsqlarr['pid'] = intval($_POST['pid']);
	if ($setsqlarr['uid']==0 || $setsqlarr['pid']==0 )exit('履歴書存在しない');
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),intval($_POST['pid']));
	if (empty($resume_basic)) exit('履歴書基本情報を入力してください');
	$resume_training=get_resume_training($_SESSION['uid'],intval($_POST['pid']));
	if (count($resume_training)>=6)  exit('訓練履歴は6件まで');
	$setsqlarr['agency'] = trim($_POST['agency'])?trim($_POST['agency']):exit('訓練機構の名称を入力してください！');
	$setsqlarr['course'] = trim($_POST['course'])?trim($_POST['course']):exit("訓練専門名称入力してください！");
	if(trim($_POST['startyear'])==""||trim($_POST['startmonth'])==""||trim($_POST['endyear'])==""||trim($_POST['endmonth'])==""){
		exit("訓練時間を選択してください！");
	}
	$setsqlarr['startyear'] = intval($_POST['startyear']);
	$setsqlarr['startmonth'] = intval($_POST['startmonth']);
	$setsqlarr['endyear'] = intval($_POST['endyear']);
	$setsqlarr['endmonth'] = intval($_POST['endmonth']);
	if($id){
		$db->updatetable(table("resume_training"),$setsqlarr,array("id"=>$id,"uid"=>intval($_SESSION['uid'])));
		exit("ok");
	}else{
		$insert_id = $db->inserttable(table("resume_training"),$setsqlarr,1);
		if($insert_id){
			check_resume($_SESSION['uid'],intval($_REQUEST['pid']));
			exit("ok");
		}else{
			exit("err");
		}
	}	
}
elseif($act == "resume_train_del")
{
	$smarty->cache = false;
	$id=intval($_GET['train_id']);
	$uid=intval($_SESSION["uid"]);
	$sql="delete from ".table("resume_training")." where id=$id and uid=$uid ";
	if($db->query($sql)){
		exit("ok");
	}else{
		exit("err");
	}
}
// 自我评价
elseif($act == "resume_specialty")
{
	$smarty->cache = false;
	$id=intval($_GET['pid']);
	$resume_basic=get_resume_basic(intval($_SESSION['uid']),$id);
	$smarty->assign('resume_basic',$resume_basic);
	$smarty->display('wap/personal/wap-evaluation.html');
}
elseif($act == "resume_specialty_save")
{
	$_POST=array_map("utf8_to_gbk",$_POST);
	$smarty->cache = false;
	$id=intval($_POST['pid']);
	$uid=intval($_SESSION["uid"]);
	$specialty=$_POST['specialty']?$_POST['specialty']:exit("自己評価を追加してください");
	$sql="update ".table("resume")." set specialty='$specialty' where id=$id and uid=$uid ";
	if($db->query($sql)){
		exit("ok");
	}else{
		exit("err");
	}

}
// 简历刷新
elseif($act == "resume_refresh")
{
	$smarty->cache = false;

	$resumeid = intval($_GET['pid']);
	$refrestime=get_last_refresh_date(intval($_SESSION['uid']),"2001");
	$duringtime=time()-$refrestime['max(addtime)'];
	$space = $_CFG['per_refresh_resume_space']*60;
	$refresh_time = get_today_refresh_times($_SESSION['uid'],"2001");
	if($_CFG['per_refresh_resume_time']!=0&&($refresh_time['count(*)']>=$_CFG['per_refresh_resume_time']))
	{
	exit("毎日最大更新件数".$_CFG['per_refresh_resume_time']."回,今日最大更新回数を超えた！");	
	}
	elseif($duringtime<=$space)
	{
	exit($_CFG['per_refresh_resume_space']."分内履歴書重複更新できません！");
	}
	else 
	{
	refresh_resume($resumeid,intval($_SESSION['uid']))?exit('ok'):exit("err");
	}
}
// 简历隐私设置
elseif($act == "resume_privacy")
{
	$smarty->cache = false;
	$pid=intval($_GET['pid']);
	//屏蔽的企业
	$uid=intval($_SESSION["uid"]);
	$shield_company=$db->getall("select * from ".table("personal_shield_company")." where uid=$uid and pid=$pid");
	$smarty->assign('shield_company',$shield_company);
	$smarty->assign('resume_one',resume_one($pid));
	$smarty->display('wap/personal/wap-privacy-settings.html');
}
elseif($act == "resume_privacy_save")
{
	$smarty->cache = false;
	$uid=intval($_SESSION['uid']);
	$pid=intval($_POST['pid']);
	$setsqlarr['display']=intval($_POST['display']);
	$setsqlarr['display_name']=intval($_POST['display_name']);
	// $setsqlarr['photo_display']=intval($_POST['photo_display']);
	!$db->updatetable(table('resume'),$setsqlarr," uid='{$uid}' AND  id='{$pid}'");
	$setsqlarrdisplay['display']=intval($_POST['display']);
	!$db->updatetable(table('resume_search_key'),$setsqlarrdisplay," uid='{$uid}' AND  id='{$pid}'");
	!$db->updatetable(table('resume_search_rtime'),$setsqlarrdisplay," uid='{$uid}' AND  id='{$pid}'");
	$rst=write_memberslog($uid,2,1104,$_SESSION['username'],"履歴書設定({$pid})");
	if($rst){
		exit("ok");
	}else{
		exit("err");
	}
}
// 屏蔽企业
elseif($act == "shield_company_save")
{
	$smarty->cache = false;
	if($_GET['comkeyword']==""){
		exit("err");
	}
	$setsqlarr['uid']=intval($_SESSION['uid']);
	$setsqlarr['pid']=intval($_GET['pid']);
	$setsqlarr['comkeyword']=$_GET['comkeyword'];
	$db->inserttable(table('personal_shield_company'),$setsqlarr,1)?exit("ok"):exit("err");
}
// 删除屏蔽企业
elseif($act == "shield_company_del")
{
	$smarty->cache = false;
	$id=intval($_GET["id"]);
	$uid=intval($_SESSION["uid"]);
	$sql="delete from ".table("personal_shield_company")." where id=$id and uid=$uid ";
	$db->query($sql)?exit("ok"):exit("err");
}
elseif($act == "resume_del")
{
	$smarty->cache = false;
	$id=intval($_GET["pid"]);
	$uid=intval($_SESSION['uid']);
	del_resume($uid,$id)?exit('ok'):exit('err');
}
// 升级高级简历
elseif($act == "resume_talent")
{
	$smarty->cache = false;
	$id=intval($_GET["pid"]);
	$uid=intval($_SESSION['uid']);
	$resume=get_resume_basic($uid,$id);
	if ($resume['complete_percent']<$_CFG['elite_resume_complete_percent'])
	{
		exit("履歴書完全度＜{$_CFG['elite_resume_complete_percent']}%，申し込み禁止！");
	}
	else
	{
		$setsqlarr["talent"]=3;
		$db->updatetable(table("resume"),$setsqlarr,array("id"=>$id,"uid"=>intval($_SESSION['uid'])))?exit("ok"):exit("err");
	}
}
// 修改简历名
elseif($act == 'resume_name_save')
{
	$smarty->cache = false;
	$_POST=array_map("utf8_to_gbk", $_POST);
	$resume_id=intval($_POST["resume_id"]);
	$uid=intval($_SESSION["uid"]);
	$title=trim($_POST['title'])?trim($_POST['title']):exit("履歴書名称入力");
	$sql="update ".table("resume")." set title='$title' where id=$resume_id and uid=$uid ";
	if($db->query($sql)){
		exit("ok");
	}else{
		exit("err");
	}
	
}
?>
