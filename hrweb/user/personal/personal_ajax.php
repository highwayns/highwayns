<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__) . '/personal_common.php');
if($act=="privacy"){
	$pid = intval($_GET['pid']);
	$uid = intval($_SESSION['uid']);
	$resume_basic=get_resume_basic($uid,$pid);
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_privacy_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$title#}',$resume_basic['title'],$contents);
	$contents=str_replace('{#$lastname#}',$resume_basic['lastname'],$contents);
	$contents=str_replace('{#$privacy_display#}',$resume_basic['display'],$contents);
	$contents=str_replace('{#$privacy_display_name#}',$resume_basic['display_name'],$contents);
	$contents=str_replace('{#$privacy_photo_display#}',$resume_basic['photo_display'],$contents);
	$contents=str_replace('{#$site_dir#}',$_CFG['site_dir'],$contents);
	exit($contents);
}
elseif($act=="user_email"){
	$tpl='../../templates/'.$_CFG['template_dir']."plus/ajax_authenticate_email_box.htm";
	$contents=file_get_contents($tpl);
	$_SESSION['send_email_key']=mt_rand(100000, 999999);
	$contents=str_replace('{#$email#}',$user["email"],$contents);
	$contents=str_replace('{#$site_name#}',$_CFG['site_name'],$contents);
	$contents=str_replace('{#$send_email_key#}',$_SESSION['send_email_key'],$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	$contents=str_replace('{#$notice#}','面接誘いをアクセプト',$contents);
	exit($contents);
}	
elseif($act=="user_mobile"){
	$tpl='../../templates/'.$_CFG['template_dir']."plus/ajax_authenticate_mobile_box.htm";
	$contents=file_get_contents($tpl);
	$_SESSION['send_mobile_key']=mt_rand(100000, 999999);
	$contents=str_replace('{#$mobile#}',$user["mobile"],$contents);
	$contents=str_replace('{#$site_name#}',$_CFG['site_name'],$contents);
	$contents=str_replace('{#$send_mobile_key#}',$_SESSION['send_mobile_key'],$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	$contents=str_replace('{#$notice#}','HR電話許可',$contents);
	exit($contents);
}
elseif($act=="old_mobile"){
	$tpl='../../templates/'.$_CFG['template_dir']."plus/ajax_authenticate_old_mobile_box.htm";
	$contents=file_get_contents($tpl);
	$_SESSION['send_mobile_key']=mt_rand(100000, 999999);
	$user["hid_mobile"] = substr($user["mobile"],0,3)."*****".substr($user["mobile"],7,4);
	$contents=str_replace('{#$mobile#}',$user["mobile"],$contents);
	$contents=str_replace('{#$hid_mobile#}',$user["hid_mobile"],$contents);
	$contents=str_replace('{#$send_mobile_key#}',$_SESSION['send_mobile_key'],$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
elseif($act=="edit_mobile"){
	$tpl='../../templates/'.$_CFG['template_dir']."plus/ajax_authenticate_edit_mobile_box.htm";
	$contents=file_get_contents($tpl);
	$_SESSION['send_mobile_key']=mt_rand(100000, 999999);
	$contents=str_replace('{#$send_mobile_key#}',$_SESSION['send_mobile_key'],$contents);
	$contents=str_replace('{#$site_name#}',$_CFG['site_name'],$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	$contents=str_replace('{#$notice#}','HR電話許可',$contents);
	exit($contents);
}
elseif($act=="tpl"){
	$pid = intval($_GET['pid']);
	$uid = intval($_SESSION['uid']);
	$resume_basic=get_resume_basic($uid,$pid);
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_tpl.htm";
	$resumetpl = get_resumetpl();
	$resume_url = url_rewrite("HW_resumeshow",array("id"=>$pid),false);
	if ($resume_basic['tpl']=="")
	{
	$resume_basic['tpl']=$_CFG['tpl_personal'];
	}
	$html = "";
	if(!empty($resumetpl)){
		foreach ($resumetpl as $key => $value) {
			$html_l.='<label><input type="radio" id="tpl_pid" name="resume_tpl" value="'.$value["tpl_dir"].'" class="radio set_tpl" '.($resume_basic['tpl']==$value['tpl_dir']?'checked':'').'>'.$value["tpl_name"].($resume_basic['tpl']==$value['tpl_dir']?'<span>[現在]</span>':'').'</label>';
			$html.='<div class="resume_box tpl_img'.$value["tpl_dir"].'" '.($resume_basic['tpl']==$value['tpl_dir']?'style="display:block"':'style="display:none"').'>
					<div class="img"><img src="'.$_CFG["site_dir"].'templates/tpl_resume/'.$value["tpl_dir"].'/thumbnail.jpg" alt="" /></div>
					<p style="margin-top:10px;"><a target="_blank" href="'.$resume_url.'&style='.$value["tpl_dir"].'">[预览]</a></p>
				</div>';
			
		}
	}
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$resume_tpl#}',$resume_basic['tpl'],$contents);
	$contents=str_replace('{#$title#}',$resume_basic['title'],$contents);
	$contents=str_replace('{#$tpl_left#}',$html_l,$contents);
	$contents=str_replace('{#$tpl_img#}',$html,$contents);
	$contents=str_replace('{#$pid#}',$resume_basic['id'],$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	$contents=str_replace('{#$site_dir#}',$_CFG['site_dir'],$contents);
	exit($contents);
}
elseif($act == "save_tpl"){
	$wheresqlarr['id'] = intval($_POST['tpl_pid'])?intval($_POST['tpl_pid']):exit("-1");
	$wheresqlarr['uid'] = intval($_SESSION['uid'])?intval($_SESSION['uid']):exit("-1");
	$setsqlarr['tpl'] = trim($_POST['tpl_dir'])?trim($_POST['tpl_dir']):exit("-1");
	$db->updatetable(table('resume'),$setsqlarr,$wheresqlarr);
	exit("1");
}
elseif($act == "del_resume"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("履歴書されていません！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	exit($contents);
}
elseif($act == "refresh_resume"){
	$resumeid = intval($_GET['id'])?intval($_GET['id']):exit("履歴書されていません！");
	$refrestime=get_last_refresh_date($_SESSION['uid'],"2001");
	$duringtime=time()-$refrestime['max(addtime)'];
	$space = $_CFG['per_refresh_resume_space']*60;
	$refresh_time = get_today_refresh_times($_SESSION['uid'],"2001");
	if($_CFG['per_refresh_resume_time']!=0&&($refresh_time['count(*)']>=$_CFG['per_refresh_resume_time']))
	{
	exit("毎日最大更新件数".$_CFG['per_refresh_resume_time']."回,今日最大更新回数を超えた！");	
	}
	elseif($duringtime<=$space){
	exit($_CFG['per_refresh_resume_space']."分内履歴書重複更新できません！");
	}
	else 
	{
	refresh_resume($resumeid,$_SESSION['uid'])?exit("1"):exit('操作失敗！');
	}
}
elseif($act == "del_resume_edu"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("履歴書されていません！");
	$id = intval($_GET['id'])?intval($_GET['id']):exit("教育職歴選択してください！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_edu_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$id#}',$id,$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
elseif($act == "del_resume_work"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("履歴書されていません！");
	$id = intval($_GET['id'])?intval($_GET['id']):exit("仕事職歴選択してください！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_work_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$id#}',$id,$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
elseif($act == "del_resume_training"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("履歴書されていません！");
	$id = intval($_GET['id'])?intval($_GET['id']):exit("訓練職歴を選択してください！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_training_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$id#}',$id,$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
elseif($act == "del_resume_language"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("履歴書されていません！");
	$id = intval($_GET['id'])?intval($_GET['id']):exit("言語能力を選択してください！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_language_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$id#}',$id,$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
elseif($act == "del_resume_credent"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("履歴書されていません！");
	$id = intval($_GET['id'])?intval($_GET['id']):exit("証明書を選択しません！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_credent_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$id#}',$id,$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
unset($smarty);
?>
