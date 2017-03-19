<?php
 /*
 * 74cms ajax
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(dirname(__FILE__).'/../include/mysql.class.php');
require_once(dirname(__FILE__).'/../include/fun_company.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = !empty($_GET['act']) ? trim($_GET['act']) : '';
$promotionid=intval($_GET['promotionid']);
$promotion=get_promotion_category_one($promotionid);
$jobsid=intval($_GET['jobsid']);
if ($_CFG['operation_mode']=='2')
{
	$setmeal=get_user_setmeal($_SESSION['uid']);//获取会员套餐
	if($setmeal['endtime']<time() && $setmeal['endtime']<>'0'){
		$end=1;
	}
	$data=get_setmeal_promotion($_SESSION['uid'],$promotionid);//获取会员某种推广的剩余条数和天数，名称，总条数
	
	$operation_mode = 2;
}
elseif($_CFG['operation_mode']=='1')
{
	$user_points = get_user_points($_SESSION['uid']);
	$operation_mode = 1;
}
elseif($_CFG['operation_mode']=='3')
{
	$setmeal=get_user_setmeal($_SESSION['uid']);//获取会员套餐
	if($setmeal['endtime']<time() && $setmeal['endtime']<>'0'){
		$end=1;
	}
	$data=get_setmeal_promotion($_SESSION['uid'],$promotionid);//获取会员某种推广的剩余条数和天数，名称，总条数
	
	if($data['num']<1){
		$operation_mode = 1;
		$user_points = get_user_points($_SESSION['uid']);
	}else{
		$operation_mode = 2;
	}
}

if ($act=="get_promotion_one")
{
	
	if($operation_mode==1){
		$html = '<tr>
      <td width="20" height="10">&nbsp;</td>
      <td height="10" ><font color="#FF3300" id="re">推广天数最少：'.$promotion["cat_minday"].'天</font></td>
    </tr>
    <tr>
      <td width="20" height="10">&nbsp;</td>
      <td height="10"><font color="#FF3300">推广期每天消耗积分： '.$promotion["cat_points"].'点积分</font></td>
    </tr>
    <tr>
      <td width="20" height="10">&nbsp;</td>
      <td height="10"><font color="#FF3300">推广天数最多： '.$promotion["cat_maxday"].'天</font></td>
    </tr><tr>
      <td width="20" height="10">&nbsp;</td>
      <td height="10"><font color="#FF3300">推广方案：'.$promotion["cat_name"].'<font></td>
    </tr>';
    if($promotion["cat_id"]==4){
		$color = get_color();
	    $html .='<tr>
					<td align="right"></td>
					<td>
					选择颜色：
					</td>
				  </tr><tr>
					<td align="right"></td>
					<td>
						<input id="val" name="val" type="text" value="" class="input_text_100"/><div style="width:220px;" class="color_picker">';
		foreach ($color as $key => $value) {
			$html .= '<div class="icolor" value="'.$value["value"].'" style="cursor:pointer;float:left; background-color: '.$value["value"].';height:20px; width:30px;font-size:0px;margin:1px;"></div>';
		}
		$html .='</div>
					</td>
				  </tr>';
    }
    $html .='<tr>
      <td width="20" height="30">&nbsp;</td>
      <td height="30">推广期限：<input name="pdays" type="text" class="input_text_100" id="pdays" value="" maxlength="4"   />
			天<span><font class="notice" color="red"></font></span></td>
    </tr><tr>
      <td width="20" height="30">&nbsp;</td>
      <td height="30">推广职位：www</td>
    </tr>
    <input name="jobsid" id="jobsid" type="hidden" value="'.$jobsid.'" />
    <input name="promotionid" id="promotionid" type="hidden" value="'.$promotionid.'" />
	<input name="pro_name" id="pro_name" type="hidden" value="" />
    <tr>
      <td height="25">&nbsp;</td>
      <td>
	  <input type="button" name="set_promotion" value="确定" class="user_submit set_promotion"/>
 </td>
    </tr>';
	}elseif($operation_mode==2){
		if($end==1){
			$html = '<tr>
      <td width="10" height="20">&nbsp;</td>
      <td height="10" >温馨提醒：您的的服务已经到期，建议您尽快<a href="company_service.php?act=setmeal_list"> [购买新套餐] </a></td>
    </tr>';
		}else{
			$html = '<tr>
      <td width="10" height="20">&nbsp;</td>
      <td height="10" ><font color="#FF3300" id="re">您的套餐：'.$setmeal["setmeal_name"].'</font></td>
    </tr>
    <tr>
      <td width="10" height="20">&nbsp;</td>
      <td height="10"><font color="#FF3300">套餐内'.$promotion["cat_name"].'总条数：
			  '.$data["total_num"].'条</font></td>
    </tr>
    <tr>
      <td width="10" height="20">&nbsp;</td>
      <td height="10"><font color="#FF3300">套餐内'.$promotion["cat_name"].'剩余：
			   '.$data["num"].'条</font></td>
    </tr><tr>
      <td width="20" height="10">&nbsp;</td>
      <td height="10"><font color="#FF3300">推广方案：'.$promotion["cat_name"].'<font></td>
    </tr>';
	if($promotion["cat_id"]==4){
		$color = get_color();
	    $html .='<tr>
					<td align="right"></td>
					<td>
					选择颜色：
					</td>
				  </tr><tr>
					<td align="right"></td>
					<td>
						<input id="val" name="val" type="text" value="" class="input_text_100"/><div style="width:220px;" class="color_picker">';
		foreach ($color as $key => $value) {
			$html .= '<div class="icolor" value="'.trim($value["value"],"#").'" style="cursor:pointer;float:left; background-color: '.$value["value"].';height:20px; width:30px;font-size:0px;margin:1px;"></div>';
		}
		$html .='</div>
					</td>
				  </tr>';
    }
    $html .='<tr>
      <td width="20" height="20">&nbsp;</td>
      <td height="20">推广期限：'.$data["days"].'天</td>
    </tr>
    <tr>
      <td width="20" height="20">&nbsp;</td>
      <td height="20">推广职位：www</td>
    </tr>
    <input name="jobsid" id="jobsid" type="hidden" value="'.$jobsid.'" />
    <input name="promotionid" id="promotionid" type="hidden" value="'.$promotion["cat_id"].'" />
	<input name="pdays" id="pdays" type="hidden" value="'.$data["days"].'" />
	<input name="pro_name" id="pro_name" type="hidden" value="'.$data["name"].'" />
     <tr>
      <td height="25">&nbsp;</td>
      <td>
	  <input type="button" name="set_promotion" value="确定" class="user_submit set_promotion"/>
 </td>
    </tr>';
		}
	}
	exit($html);
	
}
elseif($act == "promotion_save"){
	$jobsid=intval($_GET['jobsid'])==0?exit("0"):intval($_GET['jobsid']);
	$jobs=get_jobs_one($jobsid,$_SESSION['uid']);
	$jobs = array_map("addslashes",$jobs);
	if($jobs['deadline']<time()){
		exit("-1");
		// showmsg("该职位已到期，请先延期！",1);
	}
	$days=intval($_GET['pdays']);
	$_GET['val']="#".trim($_GET['val']);
	if($operation_mode==1){
		if($promotion["cat_minday"]>0 && $days<$promotion["cat_minday"]){
			exit("-5");//小于最少天数
		}elseif ($promotion["cat_maxday"]>0 && $days>$promotion["cat_maxday"]) {
			exit("-6");//大于最大天数
		}
	}
	if ($jobsid>0 && $days>0)
	{
		$pro_cat=get_promotion_category_one(intval($_GET['promotionid']));
		if($_CFG['operation_mode']=='3'){
			$setmeal=get_setmeal_promotion($_SESSION['uid'],intval($_GET['promotionid']));//获取会员套餐
			$num=$setmeal['num'];
			if(($setmeal['endtime']<time() && $setmeal['endtime']<>'0') || $num<=0){
				if($_CFG['setmeal_to_points']==1){
					if ($pro_cat['cat_points']>0)
					{
						$points=$pro_cat['cat_points']*$days;
						$user_points=get_user_points($_SESSION['uid']);
						if ($points>$user_points)
						{
							exit("-2");
						// showmsg("你的".$_CFG['points_byname']."不够进行此次操作，请先充值！",1,$link);
						}else{
							$_CFG['operation_mode']=1;
						}
					}else{
						$_CFG['operation_mode']=2;
					}
				}else{
					exit("-3");
					// showmsg("你的套餐已到期或套餐内剩余{$pro_cat['cat_name']}不够，请尽快开通新套餐",1,$link);
				}
			}else{
				$_CFG['operation_mode']=2;
			}
		}elseif($_CFG['operation_mode']=='1'){
			if ($pro_cat['cat_points']>0)
			{
				$points=$pro_cat['cat_points']*$days;
				$user_points=get_user_points($_SESSION['uid']);
				if ($points>$user_points)
				{
				exit("-2");
				}
			}
		}elseif($_CFG['operation_mode']=='2'){
			$setmeal=get_setmeal_promotion($_SESSION['uid'],intval($_GET['promotionid']));//获取会员套餐
			$num=$setmeal['num'];
			if(($setmeal['endtime']<time() && $setmeal['endtime']<>'0') || $num<=0){
				exit("-3");
			}
		}
		$info=get_promotion_one($jobsid,$_SESSION['uid'],$_GET['promotionid']);
		if (!empty($info))
		{
			exit("-4");
		// showmsg("此职位正在推广中，请选择其他职位或其他方案",1);
		}
		$setsqlarr['cp_available']=1;
		$setsqlarr['cp_promotionid']=intval($_GET['promotionid']);
		$setsqlarr['cp_uid']=$_SESSION['uid'];
		$setsqlarr['cp_jobid']=$jobsid;
		$setsqlarr['cp_days']=$days;
		$setsqlarr['cp_starttime']=time();
		$setsqlarr['cp_endtime']=strtotime("{$days} day");
		$setsqlarr['cp_val']=$_GET['val'];
		$setsqlarr['cp_hour_cn']=trim($_GET['hour']);
		$setsqlarr['cp_hour']=intval($_GET['hour']);
		if ($setsqlarr['cp_promotionid']=="4" && empty($setsqlarr['cp_val']))
		{
		showmsg("请选择颜色！",1);
		}
			if ($db->inserttable(table('promotion'),$setsqlarr))
			{
				set_job_promotion($jobsid,$setsqlarr['cp_promotionid'],$_GET['val']);
				if ($_CFG['operation_mode']=='1' && $pro_cat['cat_points']>0)
				{
					report_deal($_SESSION['uid'],2,$points);
					$user_points=get_user_points($_SESSION['uid']);
					write_memberslog($_SESSION['uid'],1,9001,$_SESSION['username'],"{$pro_cat['cat_name']}：<strong>{$jobs['jobs_name']}</strong>，推广 {$days} 天，(-{$points})，(剩余:{$user_points})");
				}elseif($_CFG['operation_mode']=='2'){
					$user_pname=trim($_GET['pro_name']);
					action_user_setmeal($_SESSION['uid'],$user_pname); //更新套餐中相应推广方式的条数
					$setmeal=get_user_setmeal($_SESSION['uid']);//获取会员套餐
					write_memberslog($_SESSION['uid'],1,9002,$_SESSION['username'],"{$pro_cat['cat_name']}：<strong>{$jobs['jobs_name']}</strong>，推广 {$days} 天，套餐内剩余{$pro_cat['cat_name']}条数：{$setmeal[$user_pname]}条。");//9002是套餐操作
				}
				write_memberslog($_SESSION['uid'],1,3004,$_SESSION['username'],"{$pro_cat['cat_name']}：<strong>{$jobs['jobs_name']}</strong>，推广 {$days} 天。");
				if ($_GET['golist'])
				{
					exit("1");
				// showmsg("推广成功！",2,$link);
				}
				else
				{
					exit("1");
				}
			}
	}
	else
	{
	exit("0");
	//showmsg("参数错误",0);
	}
}

?>