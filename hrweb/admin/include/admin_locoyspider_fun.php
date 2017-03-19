<?php
/*
 * 74cms 管理中心 
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
//检测文章标题是否重复
function ck_article_title($val)
{
	global $db;
	$sql = "select * from ".table('article')." where title='{$val}' LIMIT 1";
	$alist=$db->getone($sql);
	return $alist;
}
///--------------------------------
//获取公司信息
function get_companyinfo($val)
{
	global $db;
	$sql = "select * from ".table('company_profile')." where companyname='{$val}' AND  robot=1 LIMIT 1";
	return $db->getone($sql);
}
//检测职位名称是否有重复
function ck_jobs_name($val,$uid)
{
	global $db;
	$uid=intval($uid);
	$sql = "select id from ".table('jobs')." where jobs_name='{$val}' AND uid='{$uid}' LIMIT 1";
	$alist=$db->getone($sql);
	return $alist;
}
//匹配企业性质
function locoyspider_company_nature($str=NULL)
{
	global $db,$locoyspider;
	$sql = "select c_id,c_name from ".table('category')." where c_alias='QS_company_type' AND  c_id=".intval($locoyspider['company_nature'])." LIMIT 1";
	$nature=$db->getone($sql);
	$default=array("id"=>$nature['c_id'],"cn"=>$nature['c_name']);
	if (empty($str))
	{
		return $default;
	}
	else
	{
		if($str=='外资（欧美）' || $str=='外资（非欧美）'){
			$default=array("id"=>49,"cn"=>'外商独资');
		}elseif($str=='合资'){
			$default=array("id"=>48,"cn"=>'合资');
		}elseif($str=='国企'){
			$default=array("id"=>46,"cn"=>'国企');
		}elseif($str=='民营公司'){
			$default=array("id"=>47,"cn"=>'民营');
		}elseif($str=='国内上市公司'){
			$default=array("id"=>51,"cn"=>'上市公司');
		}elseif($str=='事业单位'){
			$default=array("id"=>53,"cn"=>'事业单位');
		}else{
			$default=array("id"=>54,"cn"=>'其它');
		}
		return $default;

	}
}
//匹配企业行业
function locoyspider_company_trade($str=NULL)
{	
	global $db,$locoyspider;
	$sql = "select c_id,c_name from ".table('category')." where  c_alias='QS_trade' AND  c_id=".intval($locoyspider['company_trade'])." LIMIT 1";
	$info=$db->getone($sql);
	$default=array("id"=>$info['c_id'],"cn"=>$info['c_name']);
	if (empty($str))
	{
		return $default;
	}
	else
	{
		$sql = "select c_id,c_name from ".table('category')." where  c_alias='QS_trade'";
		$info=$db->getall($sql);
		$return=locoyspider_search_str($info,$str,"c_name");
		if ($return)
		{
		return array("id"=>$return['c_id'],"cn"=>$return['c_name']);
		}
		else
		{
		return $default;
		}
	}
}
//匹配职位分类
function locoyspider_jobs_category($str=NULL)
{
	global $db,$locoyspider;
	$sql = "select id,parentid,categoryname from ".table('category_jobs')." where id=".intval($locoyspider['jobs_subclass'])." LIMIT 1";
	$info=$db->getone($sql);
	$sql2 = "select id,parentid,categoryname from ".table('category_jobs')." where id=".intval($info['parentid'])." LIMIT 1";
	$info2=$db->getone($sql2);
	$default=array("topclass"=>$info2['id'],"category"=>$info['id'],"subclass"=>0,"category_cn"=>$info['categoryname']);
	//var_dump($default);
	if (empty($str))
	{
		return $default;
	}
	else
	{
		$sql = "select id,parentid,categoryname from ".table('category_jobs');
		$info=$db->getall($sql);
		$return=locoyspider_search_str($info,$str,"categoryname");
		//匹配到了
		if(!empty($return)){
			//它是一级的情况
			if($return['parentid']==0){
				return array("topclass"=>$return['id'],"category"=>0,"subclass"=>0,"category_cn"=>$return['categoryname']);
			}else{
				$sql2 = "select id,parentid,categoryname from ".table('category_jobs')." where id=".intval($return['parentid']);
				$info2=$db->getone($sql2);
				//二级的情况
				if($info2['parentid']==0){
					return array("topclass"=>$info2['id'],"category"=>$return['id'],"subclass"=>0,"category_cn"=>$return['categoryname']);
				}
				//三级的情况
				else{
					$sql3 = "select id,parentid,categoryname from ".table('category_jobs')." where id=".intval($info2['parentid']);
					$info3=$db->getone($sql3);
					return array("topclass"=>$info3['id'],"category"=>$info2['id'],"subclass"=>$return['id'],"category_cn"=>$return['categoryname']);
				}
				
			}

		}
		//没有匹配到
		else{
			return $default;
		}
		
	}
}
//匹配企业地区
function locoyspider_company_district($str=NULL)
{
	global $db,$locoyspider;
	$sql = "select id,parentid,categoryname from ".table('category_district')." where id=".intval($locoyspider['company_district'])." LIMIT 1";
	$info=$db->getone($sql);
	$default=array("district"=>$info['parentid'],"sdistrict"=>$info['id'],"district_cn"=>$info['categoryname']);
	if (empty($str))
	{
		return $default;
	}
	else
	{
		$sql = "select id,parentid,categoryname from ".table('category_district')." ";
		$info=$db->getall($sql);
		$return=locoyspider_search_str($info,$str,"categoryname");
		if ($return)
		{
		return array("district"=>$return['parentid'],"sdistrict"=>$return['id'],"district_cn"=>$return['categoryname']);	
		}
		else
		{
		return $default;
		}
	}
}
//匹配工作地区
function locoyspider_jobs_district($str=NULL)
{
	global $db,$locoyspider;
	$sql = "select id,parentid,categoryname from ".table('category_district')." where id=".intval($locoyspider['jobs_district'])." LIMIT 1";
	$info=$db->getone($sql);
	$default=array("district"=>$info['parentid'],"sdistrict"=>$info['id'],"district_cn"=>$info['categoryname']);
	if (empty($str))
	{
		return $default;
	}
	else
	{
		$sql = "select id,parentid,categoryname from ".table('category_district')." ";
		$info=$db->getall($sql);
		$return=locoyspider_search_str($info,$str,"categoryname");
		if ($return)
		{
		return array("district"=>$return['parentid'],"sdistrict"=>$return['id'],"district_cn"=>$return['categoryname']);
		}
		else
		{
		return $default;
		}
	}
}
//匹配企业规模
function locoyspider_company_scale($str=NULL)
{
	global $db,$locoyspider;
	$sql = "select c_id,c_name from ".table('category')." where  c_alias='QS_scale' and c_id=".intval($locoyspider['company_scale'])." LIMIT 1";
	$info=$db->getone($sql);
	$default=array("id"=>$info['c_id'],"cn"=>$info['c_name']);
	if (empty($str))
	{
		return $default;
	}
	elseif(trim($str)=='少于50人'){
		return array("id"=>'80',"cn"=>'20人以下');
	}
	elseif(trim($str)=='50-150人'){
		return array("id"=>'81',"cn"=>'20-99人');
	}
	elseif(trim($str)=='150-500人'){
		return array("id"=>'82',"cn"=>'100-499人');
	}
	elseif(trim($str)=='500-1000人'){
		return array("id"=>'83',"cn"=>'500-999人');
	}
	elseif(trim($str)=='1000-5000人' || trim($str)=='5000-10000人'){
		return array("id"=>'84',"cn"=>'1000-9999人');
	}
	elseif(trim($str)=='10000人以上'){
		return array("id"=>'85',"cn"=>'10000人以上');
	}
	else
	{
		return $default;		
	}
}
//匹配企业注册资金
function locoyspider_company_registered($str=NULL)
{
	global $locoyspider;
	if (empty($str))
	{
		return array("registered"=>$locoyspider['company_registered'],"currency"=>$locoyspider['company_currency']);
	}
	else
	{
		return array("registered"=>$str,"currency"=>"");
	}
}
//匹配职位性质
function locoyspider_jobs_nature($str=NULL)
{
	global $db,$locoyspider;
	
	$sql = "select c_id,c_name from ".table('category')." where  c_alias='QS_jobs_nature' AND c_id=".intval($locoyspider['jobs_nature'])." LIMIT 1";
	$info=$db->getone($sql);
	$default=array("id"=>$info['c_id'],"cn"=>$info['c_name']);
	if (empty($str))
	{
		return $default;
	}
	else
	{
		$sql = "select c_id,c_name from ".table('category')." where  c_alias='QS_jobs_nature' ";
		$info=$db->getall($sql);
		$return=locoyspider_search_str($info,$str,"c_name");
		if ($return)
		{
		return array("id"=>$return['c_id'],"cn"=>$return['c_name']);
		}
		else
		{
		return $default;
		}
	}
}
//匹配职位 性别
function locoyspider_jobs_sex($str=NULL)
{	
	return get_locoyspider_jobs_sex($str);
}
function get_locoyspider_jobs_sex($sex_cn=NULL,$sex=NULL)
{
		global $locoyspider;
		if ($sex_cn=="男" || $sex=="1")
		{
		return array("id"=>1,"cn"=>"男");
		}
		elseif ($sex_cn=="女" ||  $sex=="2")
		{
		return array("id"=>2,"cn"=>"女");
		}
		elseif ($sex_cn=="不限"  ||  $sex=="3")
		{
		return array("id"=>3,"cn"=>"不限");
		}
		else
		{
			if ($locoyspider['jobs_sex']=="0")
			{
			return get_locoyspider_jobs_sex("",3);
			}
			else
			{
			return get_locoyspider_jobs_sex("",intval($locoyspider['jobs_sex']));
			}
		}
}
//匹配职位 招聘人数
function locoyspider_jobs_amount($str=NULL)
{
	global $locoyspider;
	$str=intval($str);
	if ($str>0)
	{
		return $str;
	}
	else
	{
		return mt_rand(intval($locoyspider['jobs_amount_min']),intval($locoyspider['jobs_amount_max']));
	}
}
//匹配要求学历
function locoyspider_jobs_education($str=NULL)
{
	global $db,$locoyspider;
	$sql = "select c_id,c_name from ".table('category')." where c_alias='QS_education'  and c_id=".intval($locoyspider['jobs_education'])." LIMIT 1";
	$info=$db->getone($sql);
	$default=array("id"=>$info['c_id'],"cn"=>$info['c_name']);
	if (empty($str))
	{
		return $default;
	}
	else
	{
		$sql = "select c_id,c_name from ".table('category')."  where c_alias='QS_education'";
		$info=$db->getall($sql);
		$return=locoyspider_search_str($info,$str,"c_name");
		if ($return)
		{
		return array("id"=>$return['c_id'],"cn"=>$return['c_name']);
		}
		else
		{
		return $default;
		}
	}
}
//匹配要求工作经验1(模糊匹配不适合51job采集)
function locoyspider_jobs_experience($str=NULL)
{
	global $db,$locoyspider;
	$sql = "select c_id,c_name from ".table('category')." where  c_alias='QS_experience' AND c_id=".intval($locoyspider['jobs_experience'])." LIMIT 1";
	$info=$db->getone($sql);
	$default=array("id"=>$info['c_id'],"cn"=>$info['c_name']);
	if (empty($str))
	{
		return $default;
	}
	else
	{
		$sql = "select c_id,c_name from ".table('category')." where  c_alias='QS_experience'";
		$info=$db->getall($sql);
		$return=locoyspider_search_str($info,$str,"c_name");
		if ($return)
		{
		return array("id"=>$return['c_id'],"cn"=>$return['c_name']);
		}
		else
		{
		return $default;
		}
	}
}
//匹配要求工作经验3(适合51job采集)
function get_experience($str=NULL){
	$str=intval($str);
	$arr=array();
	if($str=="1年" || $str=="2年"){
		$arr=array('id'=>76,'cn'=>'1-3年');
	}elseif($str=="3-4年"){
		$arr=array('id'=>77,'cn'=>'3-5年');
	}elseif($str=="5-7年" || $str=="8-9年"){
		$arr=array('id'=>78,'cn'=>'5-10年');
	}elseif($str=="10年以上"){
		$arr=array('id'=>79,'cn'=>'10年以上');
	}else{
		$arr=array('id'=>74,'cn'=>'无经验');
	}
	return $arr;
}
//匹配薪资待遇
/*function locoyspider_jobs_wage($str=NULL)
{
	global $db,$locoyspider;
	$sql = "select  c_id,c_name from ".table('category')." where  c_alias='QS_wage' and c_id=".intval($locoyspider['jobs_wage'])." LIMIT 1";
	$info=$db->getone($sql);
	$default=array("id"=>$info['c_id'],"cn"=>$info['c_name']);
	if (empty($str))
	{
		return $default;
	}
	else
	{
		$sql = "select c_id,c_name from ".table('category')." where  c_alias='QS_wage'";
		$info=$db->getall($sql);
		$return=locoyspider_search_str($info,$str,"c_name");
		if ($return)
		{
		return array("id"=>$return['c_id'],"cn"=>$return['c_name']);
		}
		else
		{
		return $default;
		}
	}
}*/
//修改后的匹配薪资待遇
function locoyspider_jobs_wage($str=NULL)
{
	global $db,$locoyspider;
	$sql = "select  c_id,c_name from ".table('category')." where  c_alias='QS_wage' and c_id=".intval($locoyspider['jobs_wage'])." LIMIT 1";
	$info=$db->getone($sql);
	$default=array("id"=>$info['c_id'],"cn"=>$info['c_name']);
	if (empty($str))
	{
		return $default;
	}
	else
	{
		if($str=='面议'){
			return array('id'=>55,'cn'=>'面议');
		}elseif($str=="1500以下"){
			return array('id'=>56,'cn'=>'1000~1500元/月');
		}elseif($str=="1500-1999"){
			return array('id'=>57,'cn'=>'1500~2000元/月');
		}elseif($str=="2000-2999"){
			return array('id'=>58,'cn'=>'2000~3000元/月');
		}elseif($str=="3000-4499"){
			return array('id'=>59,'cn'=>'3000~5000元/月');
		}elseif($str=="4500-5999" || $str=="6000-7999" || $str=="8000-9999"){
			return array('id'=>60,'cn'=>'5000~10000元/月');
		}else{
			return array('id'=>61,'cn'=>'1万以上/月');
		}
	}
}
//生成到期时间
function locoyspider_jobs_deadline()
{
	global $locoyspider;
	$jobs_days_min=intval($locoyspider['jobs_days_min']);
	$jobs_days_max=intval($locoyspider['jobs_days_max']);
	if ($jobs_days_min==0 && $jobs_days_max==0)
	{
	return strtotime("30 day");
	}
	else
	{
	return strtotime("".mt_rand($jobs_days_min,$jobs_days_max)." day");
	}
}
//采集注册会员
function locoyspider_user_register($email=NULL,$utype='1')
{
	global $db,$locoyspider,$QS_pwdhash;
	$setsqlarr['username']=$locoyspider['reg_usname'].uniqid().time();
	$setsqlarr['pwd_hash']=res_randstr();
		//reg_password
		if ($locoyspider['reg_password_tpye']=="1")//密码=用户名
		{
			$pwd=$setsqlarr['username'];
		}
		elseif ($locoyspider['reg_password_tpye']=="3")//密码=固定设置值
		{
			$pwd=$locoyspider['reg_password'];
		}
		else
		{
			$pwd=res_randstr(7);//长度为7的随机字符串
		}
		//email
		if (empty($email) || !preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$email))//正则校验email
		{
			$email=time().uniqid().$locoyspider['reg_email'];
		}
	$setsqlarr['password']=md5(md5($pwd).$setsqlarr['pwd_hash'].$QS_pwdhash);
	$setsqlarr['email']=$email;
	$setsqlarr['utype']=$utype;
	$setsqlarr['reg_time']=time();
	$setsqlarr['robot']=1;//标记为采集
	$reg_id=$db->inserttable(table('members'),$setsqlarr,true);
	if (!$reg_id) return false;
	if($utype=='1'){
		if(!$db->query("INSERT INTO ".table('members_points')." (uid) VALUES ('{$reg_id}')"))  return false;
		if(!$db->query("INSERT INTO ".table('members_setmeal')." (uid) VALUES ('{$reg_id}')"))  return false;
	}
	return $reg_id;
}
//添加职位
function locoyspider_addjobs($companyinfo)
{
		global $locoyspider,$db;
		$jobssetsqlarr['uid']=$companyinfo['uid'];		
		$jobssetsqlarr['companyname']=$companyinfo['companyname'];
		$jobssetsqlarr['company_id']=$companyinfo['id'];		
		$jobssetsqlarr['company_addtime']=$companyinfo['addtime'];
		$jobssetsqlarr['jobs_name']=trim($_POST['jobs_name']);
		if (empty($jobssetsqlarr['jobs_name']))  exit("职位名称丢失");
		if (ck_jobs_name($jobssetsqlarr['jobs_name'],$jobssetsqlarr['uid'])) exit("职位名称有重复");
		$jobssetsqlarr['contents']=html2text($_POST['jobs_contents']);
			$nature=locoyspider_jobs_nature(trim($_POST['jobs_nature']));
		$jobssetsqlarr['nature']=$nature['id'];
		$jobssetsqlarr['nature_cn']=$nature['cn'];
			$sex=locoyspider_jobs_sex(trim($_POST['jobs_sex']));
		$jobssetsqlarr['sex']=$sex['id'];
		$jobssetsqlarr['sex_cn']=$sex['cn'];
		//年龄要求
		$jobssetsqlarr['age']=trim($_POST['jobs_age']);
		$jobssetsqlarr['amount']=locoyspider_jobs_amount(trim($_POST['jobs_amount']));
		$jobs_category=trim($_POST['jobs_category'])?trim($_POST['jobs_category']):$jobssetsqlarr['jobs_name'];
			$category=locoyspider_jobs_category($jobs_category);//$_POST['jobs_category']
		//一级
		$jobssetsqlarr['topclass']=$category['topclass'];
		$jobssetsqlarr['category']=$category['category'];
		$jobssetsqlarr['subclass']=$category['subclass'];
		$jobssetsqlarr['category_cn']=$category['category_cn'];
		$jobssetsqlarr['trade']=$companyinfo['trade'];
		$jobssetsqlarr['trade_cn']=$companyinfo['trade_cn'];
			$district=locoyspider_jobs_district(trim($_POST['jobs_district']));
		$jobssetsqlarr['scale']=$companyinfo['scale'];
		$jobssetsqlarr['scale_cn']=$companyinfo['scale_cn'];
		$jobssetsqlarr['district']=$district['district'];
		$jobssetsqlarr['sdistrict']=$district['sdistrict'];
		$jobssetsqlarr['district_cn']=$district['district_cn'];
		//街道id 和 街道
		$jobssetsqlarr['street']=$companyinfo['street'];		
		$jobssetsqlarr['street_cn']=$companyinfo['street_cn'];
			$education=locoyspider_jobs_education(trim($_POST['jobs_education']));
		$jobssetsqlarr['education']=$education['id'];
		$jobssetsqlarr['education_cn']=$education['cn'];
			$experience=get_experience(trim($_POST['jobs_experience']));
		$jobssetsqlarr['experience']=$experience['id'];	
		$jobssetsqlarr['experience_cn']=$experience['cn'];
			$wage=locoyspider_jobs_wage(trim($_POST['jobs_wage']));
		$jobssetsqlarr['wage']=$wage['id'];
		$jobssetsqlarr['wage_cn']=$wage['cn'];
		$jobssetsqlarr['addtime']=time();
		$jobssetsqlarr['deadline']=locoyspider_jobs_deadline();
		$jobssetsqlarr['refreshtime']=time();
		$jobssetsqlarr['key']=$jobssetsqlarr['jobs_name'].$companyinfo['companyname'].$jobssetsqlarr['category_cn'].$jobssetsqlarr['district_cn'].$jobssetsqlarr['contents'];
		require_once(QISHI_ROOT_PATH.'include/splitword.class.php');
		$sp = new SPWord();
		$jobssetsqlarr['key']="{$jobssetsqlarr['jobs_name']} {$companyinfo['companyname']} ".$sp->extracttag($jobssetsqlarr['key']);
		$jobssetsqlarr['key']=$sp->pad($jobssetsqlarr['key']);
		$jobssetsqlarr['audit']=$locoyspider['jobs_audit'];
		$jobssetsqlarr['display']=$locoyspider['jobs_display'];
		$jobssetsqlarr['robot']=1;
		$pid=$db->inserttable(table('jobs'),$jobssetsqlarr,true);
		if (!$pid) exit("添加招聘信息失败");
		//职位联系方式
		$setsqlarr_contact['contact']=trim($_POST['contact']);
		//QQ
		$setsqlarr_contact['qq']=trim($_POST['qq']);
		$setsqlarr_contact['telephone']=trim($_POST['telephone']);
		$setsqlarr_contact['address']=trim($_POST['address']);
		$setsqlarr_contact['email']=check_email(trim($_POST['email']));
			//3.4新增字段,3.5也有
		$setsqlarr_contact['contact_show']=1;
		$setsqlarr_contact['telephone_show']=1;
		$setsqlarr_contact['email_show']=1;
		$setsqlarr_contact['address_show']=1;
		$setsqlarr_contact['qq_show']=1;

		$setsqlarr_contact['notify']=$locoyspider['jobs_notify'];
		$setsqlarr_contact['pid']=$pid;
		if (!$db->inserttable(table('jobs_contact'),$setsqlarr_contact)) exit("添加招聘联系方式失败");
		//------
		$searchtab['id']=$pid;
		$searchtab['uid']=$jobssetsqlarr['uid'];
		$searchtab['recommend']=$jobssetsqlarr['recommend'];
		$searchtab['emergency']=$jobssetsqlarr['emergency'];
		$searchtab['nature']=$jobssetsqlarr['nature'];
		$searchtab['sex']=$jobssetsqlarr['sex'];
		$searchtab['topclass']=$jobssetsqlarr['topclass'];
		$searchtab['category']=$jobssetsqlarr['category'];
		$searchtab['subclass']=$jobssetsqlarr['subclass'];
		$searchtab['trade']=$jobssetsqlarr['trade'];
		$searchtab['district']=$jobssetsqlarr['district'];
		$searchtab['sdistrict']=$jobssetsqlarr['sdistrict'];	
		$searchtab['street']=$companyinfo['street'];	
		$searchtab['education']=$jobssetsqlarr['education'];
		$searchtab['experience']=$jobssetsqlarr['experience'];
		$searchtab['wage']=$jobssetsqlarr['wage'];
		$searchtab['refreshtime']=$jobssetsqlarr['refreshtime'];
		$searchtab['scale']=$jobssetsqlarr['scale'];	
		//
		$db->inserttable(table('jobs_search_wage'),$searchtab);
		$db->inserttable(table('jobs_search_scale'),$searchtab);
		$db->inserttable(table('jobs_search_rtime'),$searchtab);
		//
		$searchtab['stick']=$jobssetsqlarr['stick'];
		$db->inserttable(table('jobs_search_stickrtime'),$searchtab);
		unset($searchtab['stick']);
		//
		$searchtab['click']=$jobssetsqlarr['click'];
		$db->inserttable(table('jobs_search_hot'),$searchtab);
		unset($searchtab['click']);
		//
		$searchtab['likekey']=$jobssetsqlarr['jobs_name'].','.$jobssetsqlarr['companyname'];
		$searchtab['key']=$jobssetsqlarr['key'];
		$db->inserttable(table('jobs_search_key'),$searchtab);
		require_once(ADMIN_ROOT_PATH.'include/admin_company_fun.php');
		distribution_jobs($pid);
		exit("添加成功");	
}
//添加企业
function locoyspider_addcompany($companyname)
{
	global $locoyspider,$db;
		$setsqlarr['uid']=locoyspider_user_register(check_email(trim($_POST['email'])));
		if ($setsqlarr['uid']=="") exit("添加会员出错");
		$setsqlarr['companyname']=$companyname;
			$nature=locoyspider_company_nature(trim($_POST['nature']));
		$setsqlarr['nature']=$nature['id'];
		$setsqlarr['nature_cn']=$nature['cn'];
			$trade=locoyspider_company_trade(trim($_POST['trade']));
		$setsqlarr['trade']=$trade['id'];
		$setsqlarr['trade_cn']=$trade['cn'];
			$district=locoyspider_company_district(trim($_POST['district']));
		$setsqlarr['district']=$district['district'];
		$setsqlarr['sdistrict']=$district['sdistrict'];
		$setsqlarr['district_cn']=$district['district_cn'];
			$scale=locoyspider_company_scale(trim($_POST['scale']));
		$setsqlarr['scale']=$scale['id'];
		$setsqlarr['scale_cn']=$scale['cn'];
	 		$registered=locoyspider_company_registered(trim($_POST['registered']));
		$setsqlarr['registered']=$registered['registered'];//注册资金
		$setsqlarr['currency']=$registered['currency'];//注册资金单位（人民币 or 美元）
		$setsqlarr['address']=trim($_POST['address']);
		$setsqlarr['contact']=trim($_POST['contact']);
		$setsqlarr['telephone']=trim($_POST['telephone']);
		$setsqlarr['email']=trim($_POST['email']);
		$setsqlarr['website']=trim($_POST['website']);
		$setsqlarr['contents']=html2text($_POST['contents']);
		$setsqlarr['audit']=intval($locoyspider['company_audit']);
		$setsqlarr['addtime']=time();
		$setsqlarr['refreshtime']=time();
		$setsqlarr['robot']=1;
			//3.4新增字段,3.5也有
		$setsqlarr['contact_show']=1;
		$setsqlarr['telephone_show']=1;
		$setsqlarr['email_show']=1;
		$setsqlarr['address_show']=1;
		if (!$db->inserttable(table('company_profile'),$setsqlarr)) exit("添加企业出错");
		return true;
}
//获取随机字符串
function res_randstr($length=6)
{
	$hash='';
	$chars= 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz@#!~?:-=';   
	$max=strlen($chars)-1;   
	mt_srand((double)microtime()*1000000);   
	for($i=0;$i<$length;$i++)   {   
	$hash.=$chars[mt_rand(0,$max)];   
	}   
	return $hash;   
}
//模糊搜索
function locoyspider_search_str($arr,$str,$arrinname)
{
		global $locoyspider;
 
		foreach ($arr as $key =>$list)
		{
			similar_text($list[$arrinname],$str,$percent);
			$od[$percent]=$key;
		}
			krsort($od);
			foreach ($od as $key =>$li)
			{
				if ($key>=$locoyspider['search_threshold'])
				{
				return $arr[$li];
				}
				else
				{
				return false;
				}
			}	
}
/*
 * 功能：修正矿工采集的图片生成的email
 * params：待修正的email
 * return：修正后的email
 */
function check_email($val)
{
	$str_email = strtolower($val);
	$str_email=str_replace(' ','',$str_email);//删除掉半角空格
	$str_email=str_replace('　','',$str_email);//删除掉全角空格
	$str_email=str_replace('c0m','com',$str_email);
	$str_email=str_replace('-com','.com',$str_email);
	$str_email=stripslashes($str_email);
	$str_email=str_replace("co|\\",'com',$str_email);
	return $str_email;
}
?>