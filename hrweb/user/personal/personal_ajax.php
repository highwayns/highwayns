<?php
/*
 * 74cms 个人会员中心ajax弹出框
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
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
	$contents=str_replace('{#$notice#}','接收HR面试邀请',$contents);
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
	$contents=str_replace('{#$notice#}','接收HR来电',$contents);
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
	$contents=str_replace('{#$notice#}','接收HR来电',$contents);
	exit($contents);
}
elseif($act=="tpl"){
	$pid = intval($_GET['pid']);
	$uid = intval($_SESSION['uid']);
	$resume_basic=get_resume_basic($uid,$pid);
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_tpl.htm";
	$resumetpl = get_resumetpl();
	$resume_url = url_rewrite("QS_resumeshow",array("id"=>$pid),false);
	if ($resume_basic['tpl']=="")
	{
	$resume_basic['tpl']=$_CFG['tpl_personal'];
	}
	$html = "";
	if(!empty($resumetpl)){
		foreach ($resumetpl as $key => $value) {
			$html_l.='<label><input type="radio" id="tpl_pid" name="resume_tpl" value="'.$value["tpl_dir"].'" class="radio set_tpl" '.($resume_basic['tpl']==$value['tpl_dir']?'checked':'').'>'.$value["tpl_name"].($resume_basic['tpl']==$value['tpl_dir']?'<span>[当前]</span>':'').'</label>';
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
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("您没有选择简历！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	exit($contents);
}
elseif($act == "refresh_resume"){
	$resumeid = intval($_GET['id'])?intval($_GET['id']):exit("您没有选择简历！");
	$refrestime=get_last_refresh_date($_SESSION['uid'],"2001");
	$duringtime=time()-$refrestime['max(addtime)'];
	$space = $_CFG['per_refresh_resume_space']*60;
	$refresh_time = get_today_refresh_times($_SESSION['uid'],"2001");
	if($_CFG['per_refresh_resume_time']!=0&&($refresh_time['count(*)']>=$_CFG['per_refresh_resume_time']))
	{
	exit("每天最多只能刷新".$_CFG['per_refresh_resume_time']."次,您今天已超过最大刷新次数限制！");	
	}
	elseif($duringtime<=$space){
	exit($_CFG['per_refresh_resume_space']."分钟内不能重复刷新简历！");
	}
	else 
	{
	refresh_resume($resumeid,$_SESSION['uid'])?exit("1"):exit('操作失败！');
	}
}
elseif($act == "del_resume_edu"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("您没有选择简历！");
	$id = intval($_GET['id'])?intval($_GET['id']):exit("您没有选择教育经历！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_edu_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$id#}',$id,$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
elseif($act == "del_resume_work"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("您没有选择简历！");
	$id = intval($_GET['id'])?intval($_GET['id']):exit("您没有选择工作经历！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_work_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$id#}',$id,$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
elseif($act == "del_resume_training"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("您没有选择简历！");
	$id = intval($_GET['id'])?intval($_GET['id']):exit("您没有选择培训经历！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_training_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$id#}',$id,$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
elseif($act == "del_resume_language"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("您没有选择简历！");
	$id = intval($_GET['id'])?intval($_GET['id']):exit("您没有选择语言能力！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_language_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$id#}',$id,$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
elseif($act == "del_resume_credent"){
	$pid = intval($_GET['pid'])?intval($_GET['pid']):exit("您没有选择简历！");
	$id = intval($_GET['id'])?intval($_GET['id']):exit("您没有选择证书！");
	$tpl='../../templates/'.$_CFG['template_dir']."member_personal/ajax_delete_resume_credent_box.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$resumeid#}',$pid,$contents);
	$contents=str_replace('{#$id#}',$id,$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
unset($smarty);
?>