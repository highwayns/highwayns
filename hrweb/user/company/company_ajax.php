<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__) . '/company_common.php');
if($act=="company_profile_save_succeed"){
	$tpl='../../templates/'.$_CFG['template_dir']."member_company/ajax_companyprofile_save_succeed_box.htm";
	$contents=file_get_contents($tpl);
	if($company_profile['map_open'] == '1'){
		$save_msg = '職位配布できました！ <br />';
		$opt_button = '<div class="but130cheng " onclick="javascript:location.href=\'company_jobs.php?act=addjobs\'">職位配布</div>';
	}else{ 
		$save_msg = '会社の住所がわかりやすくため， 面接ルートを<br />設定しましょう，98%の会社は電子地図を利用しています。';
		$opt_button = '<div class="but130cheng" onclick="javascript:location.href=\'company_info.php?act=company_map_open\'">立即开通</div>
		<div class="but130hui but_right" onclick="javascript:location.href=\'company_jobs.php?act=addjobs\'">发布职位</div>';
	}
	$contents=str_replace('{#$save_msg#}',$save_msg,$contents);
	$contents=str_replace('{#$opt_button#}',$opt_button,$contents);
	exit($contents);
}
elseif($act=="user_email"){
	$tpl='../../templates/'.$_CFG['template_dir']."plus/ajax_authenticate_email_box.htm";
	$contents=file_get_contents($tpl);
	$_SESSION['send_email_key']=mt_rand(100000, 999999);
	$contents=str_replace('{#$email#}',$user["email"],$contents);
	$contents=str_replace('{#$site_name#}',$_CFG['site_name'],$contents);
	$contents=str_replace('{#$send_email_key#}',$_SESSION['send_email_key'],$contents);
	$contents=str_replace('{#$notice#}','職位申し込めメール受信',$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}	
elseif($act=="user_mobile"){
	$tpl='../../templates/'.$_CFG['template_dir']."plus/ajax_authenticate_mobile_box.htm";
	$contents=file_get_contents($tpl);
	$_SESSION['send_mobile_key']=mt_rand(100000, 999999);
	$contents=str_replace('{#$mobile#}',$user["mobile"],$contents);
	$contents=str_replace('{#$site_name#}',$_CFG['site_name'],$contents);
	$contents=str_replace('{#$send_mobile_key#}',$_SESSION['send_mobile_key'],$contents);
	$contents=str_replace('{#$notice#}','職位申し込み受信',$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
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
	$contents=str_replace('{#$notice#}','職位申し込み受信',$contents);
	$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
	exit($contents);
}
elseif($act=="set_promotion"){
	$catid = intval($_GET['catid'])?intval($_GET['catid']):exit("パラメータエラー！");
	$jobid = intval($_GET['jobid'])?intval($_GET['jobid']):exit("パラメータエラー！");
	$uid = intval($_SESSION['uid'])?intval($_SESSION['uid']):exit("パラメータエラー！");
	$jobinfo = get_jobs_one($jobid);
	$promotion = get_promotion_category_one($catid);
	if ($_CFG['operation_mode']=='2')
	{
		$setmeal=get_user_setmeal($uid);//获取会员套餐
		if($setmeal['endtime']<time() && $setmeal['endtime']<>'0'){
			$end=1;//判断套餐是否到期
			$tpl='../../templates/'.$_CFG['template_dir']."member_company/ajax_set_promotion_end.htm";
		}else{
			$data=get_setmeal_promotion($uid,$catid);//获取会员某种推广的剩余条数和天数，名称，总条数
			$operation_mode=2;
			$tpl='../../templates/'.$_CFG['template_dir']."member_company/ajax_set_setmeal_promotion.htm";
		}
	}
	elseif($_CFG['operation_mode']=='1')
	{
		$points = get_user_points($uid);
		$operation_mode=1;
		$tpl='../../templates/'.$_CFG['template_dir']."member_company/ajax_set_points_promotion.htm";
	}
	elseif($_CFG['operation_mode']=='3')
	{
		$setmeal=get_user_setmeal($_SESSION['uid']);//获取会员套餐
		if($setmeal['endtime']<time() && $setmeal['endtime']<>'0'){
			if($_CFG['setmeal_to_points']!=1){
				$end=1;//判断套餐是否到期
				$tpl='../../templates/'.$_CFG['template_dir']."member_company/ajax_set_promotion_end.htm";
			}else{
				$operation_mode=1;
				$points = get_user_points($uid);
				$tpl='../../templates/'.$_CFG['template_dir']."member_company/ajax_set_points_promotion.htm";
			}
		}else{
			$data=get_setmeal_promotion($uid,$catid);//获取会员某种推广的剩余条数和天数，名称，总条数
			if($data['num']<1){
				if($_CFG['setmeal_to_points']==1){
					$operation_mode=1;
					$points = get_user_points($uid);
					$tpl='../../templates/'.$_CFG['template_dir']."member_company/ajax_set_points_promotion.htm";
				}else{
					$operation_mode=2;
					$tpl='../../templates/'.$_CFG['template_dir']."member_company/ajax_set_setmeal_promotion.htm";
				}
			}else{
				$operation_mode=2;
				$tpl='../../templates/'.$_CFG['template_dir']."member_company/ajax_set_setmeal_promotion.htm";
			}
		}
	}
	$contents=file_get_contents($tpl);
	if($end!=1){
		if($catid=="4"){
			$color = get_color();
			$color_list = '<tr>
				<td height="50">选择颜色：</td>
				<td>
					<div style="position:relateve;">
	             	 	<div id="val_menu" class="input_text_70_bg">请选择</div>	
	             	 	<div class="menu" id="menu1">
		              		<ul style="width:88px;">';
			foreach ($color as $key => $value) {
				$color_list.='<li id="'.$value["id"].'" title="'.$value["value"].'" style="background-color:'.$value["value"].'">&nbsp;</li>';
			}
			$color_list.='</ul>
		              	</div>
		            </div>
		            <input type="hidden" name="val" value="" id="val">
				</td>
				<td></td>
			</tr>';
			$contents=str_replace('{#$color_list#}',$color_list,$contents);
		}else{
			$contents=str_replace('{#$color_list#}',"",$contents);
		}
		$contents=str_replace('{#$jobid#}',$jobid,$contents);
		$contents=str_replace('{#$catid#}',$catid,$contents);
		$contents=str_replace('{#$jobs_name#}',$jobinfo['jobs_name'],$contents);
		$contents=str_replace('{#$promotion_name#}',$promotion['cat_name'],$contents);
		$contents=str_replace('{#$site_template#}',$_CFG['site_template'],$contents);
		if($operation_mode==1){
			if($promotion['cat_minday']=="0"){
				$promotion['cat_minday'] = "制限なし";
			}
			if($promotion['cat_maxday']=="0"){
				$promotion['cat_maxday'] = "制限なし";
			}
			if($promotion['cat_points']=="0"){
				$promotion['cat_points'] = "フリー";
			}
			$contents=str_replace('{#$user_points#}',$points,$contents);
			$contents=str_replace('{#$points_perday#}',$promotion['cat_points'],$contents);
			$contents=str_replace('{#$points_quantifier#}',$_CFG['points_quantifier'],$contents);
			$contents=str_replace('{#$points_byname#}',$_CFG['points_byname'],$contents);
			$contents=str_replace('{#$cat_minday#}',$promotion['cat_minday'],$contents);
			$contents=str_replace('{#$cat_maxday#}',$promotion['cat_maxday'],$contents);
		}elseif($operation_mode==2){
			$contents=str_replace('{#$days#}',$data['days'],$contents);
			$contents=str_replace('{#$setmeal_name#}',$setmeal['setmeal_name'],$contents);
			$contents=str_replace('{#$num#}',$data['num'],$contents);
			$contents=str_replace('{#$pro_name#}',$data['name'],$contents);
			$contents=str_replace('{#$cat_minday#}',$promotion['cat_minday'],$contents);
			$contents=str_replace('{#$cat_maxday#}',$promotion['cat_maxday'],$contents);
		}
	}
	exit($contents);
}
elseif($act=="promotion_add_save"){
	$catid = intval($_POST['catid'])?intval($_POST['catid']):exit("広告タイプを選択してください！");
	$jobid = intval($_POST['jobid'])?intval($_POST['jobid']):exit("職位id失った！");
	$days = intval($_POST['days'])?intval($_POST['days']):exit("広告日数を入力してください！");
	$uid = intval($_SESSION['uid'])?intval($_SESSION['uid']):exit("UID失った！");

	if($catid==4){
		$val = intval($_POST['val'])?intval($_POST['val']):exit("色を選択してください！");
		$color = get_color_one($val);
		$val_code = $color['value'];
	}else{
		$val_code = "";
	}
	$jobs=get_jobs_one($jobid,$uid);
	$jobs = array_map("addslashes", $jobs);
	if($jobs['deadline']<time()){
		exit("该職位期限切れた，延期してください！");
	}
	if ($jobid>0 && $days>0)
	{
		$pro_cat=get_promotion_category_one($catid);
		if($_CFG['operation_mode']=='3'){
			$setmeal=get_setmeal_promotion($uid,$catid);//获取会员套餐
			$num=$setmeal['num'];
			if(($setmeal['endtime']<time() && $setmeal['endtime']<>'0') || $num<=0){
				if($_CFG['setmeal_to_points']==1){
					if ($pro_cat['cat_points']>0)
					{
						$points=$pro_cat['cat_points']*$days;
						$user_points=get_user_points($uid);
						if ($points>$user_points)
						{
							exit("貴方の".$_CFG['points_byname']."ポイント足りない，振込してください！");
						}else{
							$_CFG['operation_mode']=1;
						}
					}else{
						$_CFG['operation_mode']=2;
					}
				}else{
					exit("コース期限切れたまたはコース内残る{$pro_cat['cat_name']}足りない，新コースを申し込みしてください");
				}
			}else{
				$_CFG['operation_mode']=2;
			}
		}elseif($_CFG['operation_mode']=='1'){
			if ($pro_cat['cat_points']>0)
			{
				$points=$pro_cat['cat_points']*$days;
				$user_points=get_user_points($uid);
				if ($points>$user_points)
				{
				exit("貴方の".$_CFG['points_byname']."ポイント足りない，振込してください！");
				}
			}
		}elseif($_CFG['operation_mode']=='2'){
			$setmeal=get_setmeal_promotion($uid,$catid);//获取会员套餐
			$num=$setmeal['num'];
			if(($setmeal['endtime']<time() && $setmeal['endtime']<>'0') || $num<=0){
				exit("コース期限切れたまたはコース内残る{$pro_cat['cat_name']}足りない，新コースを申し込みしてください");
			}
		}
		$info=get_promotion_one($jobid,$uid,$catid);
		if (!empty($info))
		{
		exit("この職位は広告中，その他職位或その他ケースを選択してください");
		}
		$setsqlarr['cp_available']=1;
		$setsqlarr['cp_promotionid']=$catid;
		$setsqlarr['cp_uid']=$uid;
		$setsqlarr['cp_jobid']=$jobid;
		$setsqlarr['cp_days']=$days;
		$setsqlarr['cp_starttime']=time();
		$setsqlarr['cp_endtime']=strtotime("{$days} day");
		$setsqlarr['cp_val']=$val_code;
		if ($setsqlarr['cp_promotionid']=="4" && empty($setsqlarr['cp_val']))
		{
		exit("色を選択してください！");
		}
			if ($db->inserttable(table('promotion'),$setsqlarr))
			{
				set_job_promotion($jobid,$setsqlarr['cp_promotionid'],$val_code);
				if ($_CFG['operation_mode']=='1' && $pro_cat['cat_points']>0)
				{
					report_deal($uid,2,$points);
					$user_points=get_user_points($uid);
					write_memberslog($uid,1,9001,$_SESSION['username'],"{$pro_cat['cat_name']}：<strong>{$jobs['jobs_name']}</strong>，広告 {$days} 日，(-{$points})，(残る:{$user_points})",1,1018,"{$pro_cat['cat_name']}","-{$points}","{$user_points}");
				}elseif($_CFG['operation_mode']=='2'){
					$user_pname=trim($_POST['pro_name']);
					action_user_setmeal($uid,$user_pname); //更新套餐中相应推广方式的条数
					$setmeal=get_user_setmeal($uid);//获取会员套餐
					write_memberslog($uid,1,9002,$_SESSION['username'],"{$pro_cat['cat_name']}：<strong>{$jobs['jobs_name']}</strong>，広告 {$days} 日，コース内残る{$pro_cat['cat_name']}件数：{$setmeal[$user_pname]}件。",2,1018,"{$pro_cat['cat_name']}","-{$days}","{$setmeal[$user_pname]}");//9002是套餐操作
				}
				write_memberslog($uid,1,3004,$_SESSION['username'],"{$pro_cat['cat_name']}：<strong>{$jobs['jobs_name']}</strong>，広告 {$days} 日。");
				exit('お勧め成功！');
			}
	}
	else
	{
	exit("広告失敗！");
	}
}
//订单详情
elseif($act=='order_detail')
{
	$uid = intval($_SESSION['uid']);
	$order_id = intval($_GET['order_id'])?intval($_GET['order_id']):exit("オーダー番号失った！");
	$order =  $db->getone("SELECT * FROM ".table('order')." WHERE uid ='{$uid}' AND id='{$order_id}' LIMIT 1");
	$tpl='../../templates/'.$_CFG['template_dir']."member_company/ajax_order_detail.htm";
	$contents=file_get_contents($tpl);
	$contents=str_replace('{#$order_oid#}',$order['oid'],$contents);
	$contents=str_replace('{#$order_addtime#}',date('Y-m-d',$order['addtime']),$contents);
	if($order['is_paid']=='1')
	{
		$contents=str_replace('{#$order_is_paid#}','未完了',$contents);
		$button = '<a href="?act=payment&order_id={#$order_id#}"><input type="button" value="支払" class="btn-65-30blue btn-big-font" /></a>';
		$contents=str_replace('{#$button#}',$button,$contents);
	}
	else
	{
		$contents=str_replace('{#$order_is_paid#}','支払い済み',$contents);
		$button = '<input type="button" value="支払いました" class="btn-65-30blue btn-big-font" />';
		$contents=str_replace('{#$button#}',$button,$contents);
	}
	$contents=str_replace('{#$order_des#}',$order['description'],$contents);
	if($order['payment_name']!='points')
	{
		$contents=str_replace('{#$order_amount#}','￥'.$order['amount'],$contents);
	}
	else
	{
		$contents=str_replace('{#$order_amount#}','チャージ'.$order['amount'].'ポイント',$contents);
	}
	$contents=str_replace('{#$order_payname#}',get_payment_info($order['payment_name'],ture),$contents);
	if($order['notes'])
	{
		$contents=str_replace('{#$order_note#}',$order['notes'],$contents);
	}
	else
	{
		$contents=str_replace('{#$order_note#}',"なし",$contents);
	}
	$contents=str_replace('{#$order_id#}',$order['id'],$contents);
	exit($contents);
}
//  简历发送到邮箱
elseif($act == "sendtoemail")
{
	global $_CFG;
	$uid=intval($_GET['uid']);
	$resume_id=intval($_GET['resume_id']);
	$email=trim($_GET['email']);
	$resume_basic=get_resume_basic($resume_id);
	if($resume_basic['tag_cn'])
	{
		$resume_tag=explode(',',$resume_basic['tag_cn']);
		$tag_str='<p>';
		foreach ($resume_tag as $value)
		{
			$tag_str.='<span style="color: #656565;display:inline-block;background-color: #f2f4f7; border: 1px solid #d6d6d7;text-align: center;height:30px;line-height: 30px;margin-right:10px;padding:0 10px">'.$value.'</span>';
		}
		$tag_str.='</p>';
	}
	$resume_work=get_resume_work($uid,$resume_id);
	$show_contact = false;
	if($_CFG['showapplycontact']=='1' || $_CFG['showresumecontact']=='0')
	{
		$show_contact = '<p>携帯番号：'.$resume_basic["telephone"].' 電子メール：'.$resume_basic["email"].'</p>';
	}
	else
	{
		$show_contact = '<p>ご連絡：<a href='.url_rewrite('HW_resumeshow',array('id'=>$resume_id)).'>クリック</a></p>';
	}	
	$htm='<div style="width: 900px;margin: 0 auto;font-size: 14px;">
		<div style="margin-bottom:10px">
			<div style="float: left;"><a href="'.$_CFG['site_dir'].'"><img src="'.$_CFG['site_domain'].$_CFG['upfiles_dir'].$_CFG['web_logo'].'" alt="'.$_CFG['site_name'].'" border="0" align="absmiddle" width=180 height=50 /></div>
			<div style="float: right;padding-top:10px;">'.$templates.'更新时间：'.date("Y-m-d",$resume_basic["refreshtime"]).'</div>
			<div style="clear:both"></div>
		</div>
		<div style="padding-bottom: 10px;">
			<span style="font-size: 18px;font-weight: 700;">'.$resume_basic["fullname"].'</span><span>（'.$resume_basic["sex_cn"].'，'.$resume_basic["age"].'）</span>
			<p>学历：'.$resume_basic["education_cn"].' | 专业：'.$resume_basic["major_cn"].' | 工作经验：'.$resume_basic["experience_cn"].'年 | 现居住地：'.$resume_basic["residence"].'</p>

			'.$show_contact.$tag_str.'

		</div>
		<div style="padding-bottom: 10px;">
			<p style="font-size: 16px;font-weight: 700;">求职意向</p>
			<p>期望职位：'.$resume_basic["intention_jobs"].'</p>
			<p>期望薪资：'.$resume_basic["wage_cn"].'</p>
			<p>期望地区：'.$resume_basic["district_cn"].'</p>
		</div>
		<div style="padding-bottom: 10px;">
			<p style="font-size: 16px;font-weight: 700;">工作经验</p>';
				if(!empty($resume_work))
				{
					foreach ($resume_work as $value)
					{
						$htm.='<div>
								<p style="font-size: 14px;font-weight: 700;">'.$value["companyname"].'</p>
								<p>'.$value["startyear"].'年'.$value["startmonth"].'月-'.$value["endyear"].'年'.$value["endmonth"].'月 '.$value["jobs"].'</p>
								<div style="float: left;width: 100px;">工作内容：</div>
								<div style="float: right;width: 800px;">'.$value["achievements"].'</div>
								<div style="clear:both"></div>
							</div>'	;
					}
				}
				else
				{
					$htm.='<div>
								没有填写工作经历
							</div>'	;
				}
				
		$htm.='</div>';
		if($resume_basic["specialty"])
		{
			$htm.='<div style="padding-bottom: 10px;">
				<p style="font-size: 16px;font-weight: 700;">自我描述</p>
				<p>'.$resume_basic["specialty"].'</p>
			</div>';
		}
		$htm.='<div style="text-align: center;margin-top:20px">
				该简历来自<a href="'.$_CFG["site_domain"].$_CFG["site_dir"].'">'.$_CFG["site_name"].'</a>
			</div>
		</div>';
		$rst=smtp_mail($_GET['email'],"{$resume_basic['fullname']}の履歴書",$htm);
		exit($rst);
}
unset($smarty);
?>
