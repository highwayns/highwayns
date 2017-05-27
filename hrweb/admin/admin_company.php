<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_company_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'jobs';
if($act == 'jobs')
{
	check_permissions($_SESSION['admin_purview'],"jobs_show");
	$audit=intval($_GET['audit']);
	$invalid=intval($_GET['invalid']);
	$deadline=intval($_GET['deadline']);
	$jobtype=intval($_GET['jobtype']);
	if (empty($jobtype))
	{
		$jobtype=1;
		$_GET['jobtype']=1;
	}
	if ($jobtype==1)
	{
	$tablename="jobs";
	$audit="";
	$deadline=$deadline>2?$deadline:'';
	}
	else
	{
	$tablename="jobs_tmp";
	}
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$oederbysql=" order BY id DESC";
	$wheresqlarr=array();
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if (!empty($key) && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE jobs_name like '%{$key}%'";
		elseif ($key_type===2)$wheresql=" WHERE companyname like '%{$key}%'";
		elseif ($key_type===3 && intval($key)>0)$wheresql=" WHERE id =".intval($key);
		elseif ($key_type===4 && intval($key)>0)$wheresql=" WHERE company_id =".intval($key);
		elseif ($key_type===5 && intval($key)>0)$wheresql=" WHERE uid =".intval($key);
		$oederbysql="";
		$tablename="all";
	}
	else
	{
			if ($audit>0)
			{
			$wheresqlarr['audit']=$audit;
			}
			if(isset($_GET['emergency']) && $_GET['emergency']<>'')
			{
			$wheresqlarr['emergency']=intval($_GET['emergency']);
			$oederbysql=" order BY refreshtime DESC";
			}
			if(isset($_GET['recommend']) && $_GET['recommend']<>'')
			{
			$wheresqlarr['recommend']=intval($_GET['recommend']);
			$oederbysql=" order BY refreshtime DESC";
			}
			if (!empty($wheresqlarr)) $wheresql=wheresql($wheresqlarr);
			if (!empty($_GET['settr']))
			{
				$settr=strtotime("-".intval($_GET['settr'])." day");
				$wheresql=empty($wheresql)?" WHERE refreshtime> ".$settr:$wheresql." AND refreshtime> ".$settr;
				$oederbysql=" order BY refreshtime DESC ";
			}
			if (!empty($_GET['addsettr']))
			{
				$settr=strtotime("-".intval($_GET['addsettr'])." day");
				$wheresql=empty($wheresql)?" WHERE addtime> ".$settr:$wheresql." AND addtime> ".$settr;
				$oederbysql=" order BY addtime DESC ";
			}
			//无效原因(1->职位到期  2->套餐到期  3->职位暂停  4->审核未通过)
			if ($invalid==1)
			{
			$wheresql=empty($wheresql)?" WHERE deadline< ".time():$wheresql." AND deadline< ".time();
			$oederbysql=" order BY deadline DESC ";
			}
			elseif($invalid==2)
			{
			$wheresql=empty($wheresql)?" WHERE setmeal_deadline!=0 AND setmeal_deadline< ".time():$wheresql." AND  setmeal_deadline!=0 AND  setmeal_deadline< ".time();
			$oederbysql=" order BY setmeal_deadline DESC ";
			}
			elseif($invalid==3)
			{
			$wheresql=empty($wheresql)?" WHERE display=2 ":$wheresql." AND  display=2 ";
			$oederbysql=" order BY refreshtime DESC ";
			}
			elseif($invalid==4)
			{
			$wheresql=empty($wheresql)?" WHERE audit!=1 ":$wheresql." AND  audit!=1 ";
			$oederbysql=" order BY deadline DESC ";
			}

			if($deadline==1)
			{
			$wheresql=empty($wheresql)?" WHERE deadline< ".time():$wheresql." AND deadline< ".time();
			$oederbysql=" order BY deadline DESC ";
			}
			elseif($deadline==2)
			{			
			$wheresql=empty($wheresql)?" WHERE deadline>  ".time():$wheresql." AND deadline> ".time();
			$oederbysql=" order BY deadline DESC ";
			}
			elseif($deadline>2)
			{
			$settr=strtotime("+{$deadline} day");
			$wheresql=empty($wheresql)?" WHERE deadline< {$settr}":$wheresql." AND deadline<{$settr} ";
			$oederbysql=" order BY deadline DESC ";
			}
			
			if (!empty($_GET['promote']))
			{
				$promote=intval($_GET['promote']);
				if ($promote==-1)
				{
				$psql="recommend=0 AND emergency=0 AND stick=0 AND highlight=''";
				$wheresql=empty($wheresql)?" WHERE {$psql} ":"{$wheresql} AND {$psql} ";
				}
				elseif ($promote==1)
				{
				$psql="recommend=1";
				$wheresql=empty($wheresql)?" WHERE {$psql} ":"{$wheresql} AND {$psql} ";
				}
				elseif ($promote==2)
				{
				$psql="emergency=1";
				$wheresql=empty($wheresql)?" WHERE {$psql} ":"{$wheresql} AND {$psql} ";
				}
				elseif ($promote==3)
				{
				$psql="stick=1";
				$wheresql=empty($wheresql)?" WHERE {$psql} ":"{$wheresql} AND {$psql} ";
				}
				elseif ($promote==4)
				{
				$psql="highlight<>'' ";
				$wheresql=empty($wheresql)?" WHERE {$psql} ":"{$wheresql} AND {$psql} ";
				}
				$oederbysql="";
			}
		 
	}
	if ($tablename=="all")
	{
	$total_sql="SELECT COUNT(*) AS num FROM ".table('jobs').$wheresql." UNION ALL SELECT COUNT(*) AS num FROM ".table('jobs_tmp').$wheresql;
	}
	else
	{
	$total_sql="SELECT COUNT(*) AS num FROM ".table($tablename).$wheresql;
	}
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	if ($tablename=="all")
	{
	$getsql="SELECT * FROM ".table('jobs').$wheresql." UNION ALL SELECT * FROM ".table('jobs_tmp').$wheresql;
	}
	else
	{
	$getsql="SELECT * FROM ".table($tablename)." ".$wheresql.$oederbysql;
	}
	$total[0]=$db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs')."");
	$total[1]=$db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs_tmp')."");
	if ($jobtype==2)
	{
	$total[2]=$db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs_tmp')." WHERE audit=1 ");
	$total[3]=$db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs_tmp')." WHERE audit=2 ");
	$total[4]=$db->get_total("SELECT COUNT(*) AS num FROM ".table('jobs_tmp')." WHERE audit=3 ");
	}
	$jobs = get_jobs($offset,$perpage,$getsql);
	$smarty->assign('pageheader',"職位管理");
	$smarty->assign('jobs',$jobs);
	$smarty->assign('now',time());
	$smarty->assign('total',$total);
	$smarty->assign('page',$page->show(3));
	$smarty->assign('totaljob',$total_val);
	$smarty->assign('cat',get_promotion_cat(1));
	get_token();
	$smarty->display('company/admin_company_jobs.htm');
}
elseif($act == 'jobs_perform')
{
		check_token();
		$yid =!empty($_REQUEST['y_id'])?$_REQUEST['y_id']:adminmsg("選択された職位がなし！",1);
		if (!empty($_REQUEST['delete']))
		{
			check_permissions($_SESSION['admin_purview'],"jobs_del");
			$num=del_jobs($yid);
			if ($num>0)
			{
			adminmsg("削除成功！削除件数".$num."行",2);
			}
			else
			{
			adminmsg("削除失敗！",0);
			}
		}
		elseif (!empty($_POST['set_audit']))
		{
			check_permissions($_SESSION['admin_purview'],"jobs_audit");
			$audit=intval($_POST['audit']);
			$pms_notice=intval($_POST['pms_notice']);
			$reason=trim($_POST['reason']);
			if ($n=edit_jobs_audit($yid,$audit,$reason,$pms_notice))
			{
			refresh_jobs($yid); 
			adminmsg("審査成功！影響行数 {$n}",2);			
			}
			else
			{
			adminmsg("審査失敗！影響行数 0",1);
			}
		}
		elseif (!empty($_GET['refresh']))
		{
			if($n=refresh_jobs($yid))
			{
			adminmsg("更新成功！変更行数 {$n}",2);
			}
			else
			{
			adminmsg("更新失敗！",0);
			}
		}
		elseif (!empty($_POST['set_delay']))
		{
			$days=intval($_POST['days']);
			if (empty($days))
			{
			adminmsg("延長の日数を入力してください！",0);
			}
			$arr=delay_jobs($yid,$days);
			if(!empty($arr))
			{
				$job_arr = explode(',', $arr);
				if(intval($job_arr[1])==0)
				{
					$img_type = 0;
				}
				else
				{
					$img_type = 2;
				}
				distribution_jobs($yid);
				adminmsg("職位延長 {$job_arr[0]} 件！成功 {$job_arr[1]} 件，失敗 {$job_arr[2]} 件",$img_type);
			}
			else
			{
			adminmsg("操作失敗！",0);
			}
		}
}
elseif($act == 'edit_jobs')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"jobs_edit");
	$id =!empty($_REQUEST['id'])?intval($_REQUEST['id']):adminmsg("選択された職位がなし！",1);
	$smarty->assign('pageheader',"職位管理");
	$jobs=get_jobs_one($id);
	$smarty->assign('url',$_SERVER["HTTP_REFERER"]);
	$smarty->assign('jobs',$jobs);
	$smarty->assign('jobsaudit',get_jobsaudit_one($id));
	$smarty->display('company/admin_company_jobs_edit.htm');
}
elseif ($act=='editjobs_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"jobs_edit");
	$id=intval($_POST['id']);
	$company_id=intval($_POST['company_id']);
    $company_profile=get_company_one_id($company_id);
	$setsqlarr['jobs_name']=trim($_POST['jobs_name'])?trim($_POST['jobs_name']):adminmsg('職位を選択してください！',1);
	$setsqlarr['nature']=intval($_POST['nature']);
	$setsqlarr['nature_cn']=trim($_POST['nature_cn']);	
	$setsqlarr['topclass']=intval($_POST['topclass']);
	$setsqlarr['category']=intval($_POST['category']);
	$setsqlarr['subclass']=intval($_POST['subclass']);
	$setsqlarr['category_cn']=trim($_POST['category_cn']);
	$setsqlarr['amount']=intval($_POST['amount']);
	$setsqlarr['district']=intval($_POST['district']);
	$setsqlarr['sdistrict']=intval($_POST['sdistrict']);
	$setsqlarr['district_cn']=trim($_POST['district_cn']);
	$setsqlarr['wage']=intval($_POST['wage']);
	$setsqlarr['wage_cn']=trim($_POST['wage_cn']);
	$setsqlarr['display']=intval($_POST['display']);
	$setsqlarr['audit']=intval($_POST['audit']);
	$setsqlarr['sex']=intval($_POST['sex']);
	$setsqlarr['sex_cn']=trim($_POST['sex_cn']);
	$setsqlarr['education']=intval($_POST['education']);
	$setsqlarr['education_cn']=trim($_POST['education_cn']);
	$setsqlarr['experience']=intval($_POST['experience']);
	$setsqlarr['experience_cn']=trim($_POST['experience_cn']);
	$setsqlarr['graduate']=intval($_POST['graduate']);
	$setsqlarr['contents']=trim($_POST['contents'])?trim($_POST['contents']):adminmsg('職位説明を入力してください！',1);	
	$setsqlarr['key']=$setsqlarr['jobs_name'].$company_profile['companyname'].$setsqlarr['category_cn'].$setsqlarr['district_cn'].$setsqlarr['contents'];
	require_once(HIGHWAY_ROOT_PATH.'include/splitword.class.php');
	$sp = new SPWord();
	$setsqlarr['key']="{$setsqlarr['jobs_name']} {$company_profile['companyname']} ".$sp->extracttag($setsqlarr['key']);
	$setsqlarr['key']=$sp->pad($setsqlarr['key']);
	$days=intval($_POST['days']);
	if ($days>0 && (intval($_POST['olddeadline'])-time())>0) $setsqlarr['deadline']=intval($_POST['olddeadline'])+($days*(60*60*24));
	if ($days>0 && (intval($_POST['olddeadline'])-time())<0) $setsqlarr['deadline']=strtotime("".$days." day");
	$setsqlarr_contact['contact']=trim($_POST['contact']);
	$setsqlarr_contact['qq']=trim($_POST['qq']);
	$setsqlarr_contact['telephone']=trim($_POST['telephone']);
	if(!preg_match("/1[3458]{1}\d{9}$/",$setsqlarr_contact['telephone'])){
		$setsqlarr_contact['notify_mobile'] = 0;
	}
	$setsqlarr_contact['address']=trim($_POST['address']);
	$setsqlarr_contact['email']=trim($_POST['email']);
	$setsqlarr_contact['notify']=trim($_POST['notify']);
		$setsqlarr_contact['contact_show']=intval($_POST['contact_show']);
	$setsqlarr_contact['email_show']=intval($_POST['email_show']);
	$setsqlarr_contact['telephone_show']=intval($_POST['telephone_show']);
	$setsqlarr_contact['address_show']=intval($_POST['address_show']);
	$setsqlarr_contact['qq_show']=intval($_POST['qq_show']);
	
	$wheresql=" id='".$id."' ";
	$tb1=$db->getone("select * from ".table('jobs')." where id='{$id}' LIMIT 1");
	if (!empty($tb1))
	{
		if (!$db->updatetable(table('jobs'),$setsqlarr,$wheresql)) adminmsg("保存失敗！",0);
	}
	else
	{
		if (!$db->updatetable(table('jobs_tmp'),$setsqlarr,$wheresql)) adminmsg("保存失敗！",0);
	}
	$wheresql=" pid=".$id;
	if (!$db->updatetable(table('jobs_contact'),$setsqlarr_contact,$wheresql)) adminmsg("保存失敗！",0);
	$searchtab['nature']=$setsqlarr['nature'];
	$searchtab['sex']=$setsqlarr['sex'];
	$searchtab['topclass']=$setsqlarr['topclass'];
	$searchtab['category']=$setsqlarr['category'];
	$searchtab['subclass']=$setsqlarr['subclass'];
	$searchtab['district']=$setsqlarr['district'];
	$searchtab['sdistrict']=$setsqlarr['sdistrict'];
	$searchtab['education']=$setsqlarr['education'];
	$searchtab['experience']=$setsqlarr['experience'];
	$searchtab['wage']=$setsqlarr['wage'];
	$searchtab['graduate']=$setsqlarr['graduate'];
	//
	$db->updatetable(table('jobs_search_wage'),$searchtab," id='{$id}'");
	$db->updatetable(table('jobs_search_rtime'),$searchtab," id='{$id}'");
	$db->updatetable(table('jobs_search_stickrtime'),$searchtab," id='{$id}'");
	$db->updatetable(table('jobs_search_hot'),$searchtab," id='{$id}'");
	$db->updatetable(table('jobs_search_scale'),$searchtab," id='{$id}'");
	$searchtab['key']=$setsqlarr['key'];
	$searchtab['likekey']=$setsqlarr['jobs_name'].','.$company_profile['companyname'];
	$db->updatetable(table('jobs_search_key'),$searchtab," id='{$id}' ");
	write_log("変更職位idは".$id."の職位,", $_SESSION['admin_name'],3);
	unset($setsqlarr_contact,$setsqlarr);
	distribution_jobs($id);
	$link[0]['text'] = "職位一覧に戻る";
	$link[0]['href'] = $_POST['url'];
	adminmsg("変更成功！",2,$link);
}
elseif($act == 'company_list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"com_show");
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$oederbysql=" order BY c.id DESC ";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if ($key && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE c.companyname like '%{$key}%'";
		elseif ($key_type===2)$wheresql=" WHERE c.id ='".intval($key)."'";
		elseif ($key_type===3)$wheresql=" WHERE m.username like '{$key}%'";
		elseif ($key_type===4)$wheresql=" WHERE c.uid ='".intval($key)."'";
		elseif ($key_type===5)$wheresql=" WHERE c.address  like '%{$key}%'";
		elseif ($key_type===6)$wheresql=" WHERE c.telephone  like '{$key}%'";		
		$oederbysql="";
	}
	$_GET['audit']<>""? $wheresqlarr['c.audit']=intval($_GET['audit']):'';
	if (is_array($wheresqlarr)) $wheresql=wheresql($wheresqlarr);
	if (!empty($_GET['settr']))
	{
		$settr=strtotime("-".intval($_GET['settr'])." day");
		$wheresql=empty($wheresql)?" WHERE addtime> ".$settr:$wheresql." AND addtime> ".$settr;
	}
	$operation_mode=$_CFG['operation_mode'];
	if($operation_mode=='1'){
		$joinsql=" LEFT JOIN ".table('members')." AS m ON c.uid=m.uid  LEFT JOIN ".table('members_points')." AS p ON c.uid=p.uid";
	}else{
		$joinsql=" LEFT JOIN ".table('members')." AS m ON c.uid=m.uid  LEFT JOIN ".table('members_setmeal')." AS p ON c.uid=p.uid";
	}
	$total_sql="SELECT COUNT(*) AS num FROM ".table('company_profile')." AS c".$joinsql.$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$clist = get_company($offset,$perpage,$joinsql.$wheresql.$oederbysql,$operation_mode);
	$smarty->assign('pageheader',"企業管理");
	$smarty->assign('clist',$clist);
	$smarty->assign('certificate_dir',$certificate_dir);
	$smarty->assign('page',$page->show(3));
	$smarty->display('company/admin_company_list.htm');
}

elseif($act == 'company_perform')
{
	check_token();
	$u_id =!empty($_POST['y_id'])?$_POST['y_id']:adminmsg("企業を選択してください！",1);
	if ($_POST['delete'])
	{
		check_permissions($_SESSION['admin_purview'],"com_del");
		if ($_POST['delete_company']=='yes')
		{
		!del_company($u_id)?adminmsg("企業資料削除失敗！",0):"";
		}
		if ($_POST['delete_jobs']=='yes')
		{
		!del_company_alljobs($u_id)?adminmsg("削除職位失敗！",0):"";
		}
		if ($_POST['delete_jobs']<>'yes' && $_POST['delete_company']<>'yes')
		{
		adminmsg("削除タイプ未選択！",1);
		}
		adminmsg("削除成功！",2);
	}
	if (trim($_POST['set_audit']))
	{
		check_permissions($_SESSION['admin_purview'],"com_audit");
		$audit=$_POST['audit'];
		$pms_notice=intval($_POST['pms_notice']);
		$reason=trim($_POST['reason']);
		!edit_company_audit($u_id,intval($audit),$reason,$pms_notice)?adminmsg("設定失敗！",0):adminmsg("設定成功！",2);
	}
	elseif (!empty($_POST['set_refresh']))
	{
		if (empty($_POST['refresh_jobs']))
		{
		$refresjobs=false;
		}
		else
		{
		$refresjobs=true;
		}
		if($n=refresh_company($u_id,$refresjobs))
		{
		adminmsg("更新成功！更新行数 {$n} 行",2);
		}
		else
		{
		adminmsg("更新失敗！",0);
		}
	}
}
elseif($act == 'edit_company_profile')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"com_edit");
	$yid =!empty($_REQUEST['id'])?intval($_REQUEST['id']):adminmsg("企業を選択してください！",1);
	$smarty->assign('pageheader',"企業管理");
	$company_profile=get_company_one_id($yid);
	$smarty->assign('url',$_SERVER["HTTP_REFERER"]);
	$smarty->assign('comaudit',get_comaudit_one($yid));

	$smarty->assign('company_profile',$company_profile);
	$smarty->assign('certificate_dir',$certificate_dir);//营业执照路径
	$smarty->display('company/admin_company_profile_edit.htm');
}
elseif ($act=='company_profile_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"com_edit");
	$setsqlarr=array();
	$contents=array();
	$id=intval($_POST['id']);
	$setsqlarr['audit']=intval($_POST['audit']);
	$setsqlarr['companyname']=trim($_POST['companyname'])?trim($_POST['companyname']):adminmsg('企業名称を入力してください！',1);
	$setsqlarr['nature']=trim($_POST['nature'])?trim($_POST['nature']):adminmsg('企業の性質を選択してください！',1);
	$setsqlarr['nature_cn']=trim($_POST['nature_cn'])?trim($_POST['nature_cn']):adminmsg('企業の性質を選択してください！',1);
	$setsqlarr['trade']=trim($_POST['trade'])?trim($_POST['trade']):adminmsg('業界を選択してください！',1);
	$setsqlarr['trade_cn']=trim($_POST['trade_cn'])?trim($_POST['trade_cn']):adminmsg('業界を選択してください！',1);
	$setsqlarr['district_cn']=trim($_POST['district_cn'])?trim($_POST['district_cn']):adminmsg('所属地区を選択してください！',1);
	$setsqlarr['district']=intval($_POST['district']);
	$setsqlarr['sdistrict']=intval($_POST['sdistrict']);
	$setsqlarr['street']=intval($_POST['street']);
	$setsqlarr['street_cn']=trim($_POST['street_cn']);
	$setsqlarr['scale']=trim($_POST['scale'])?trim($_POST['scale']):adminmsg('会社規模を選択してください！',1);
	$setsqlarr['scale_cn']=trim($_POST['scale_cn'])?trim($_POST['scale_cn']):adminmsg('会社規模を選択してください！',1);	
	$setsqlarr['registered']=trim($_POST['registered']);
	$setsqlarr['currency']=trim($_POST['currency']);
	$setsqlarr['address']=trim($_POST['address']);
	$setsqlarr['contact']=trim($_POST['contact']);
	$setsqlarr['telephone']=trim($_POST['telephone']);
	$setsqlarr['email']=trim($_POST['email']);
	$setsqlarr['website']=trim($_POST['website']);
	$setsqlarr['contents']=trim($_POST['contents'])?trim($_POST['contents']):adminmsg('会社の紹介を入力してください！',1);
		$setsqlarr['contact_show']=intval($_POST['contact_show']);
	$setsqlarr['email_show']=intval($_POST['email_show']);
	$setsqlarr['telephone_show']=intval($_POST['telephone_show']);
	$setsqlarr['address_show']=intval($_POST['address_show']);
		
	$wheresql=" id='{$id}' ";
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = $_POST['url'];
		if ($db->updatetable(table('company_profile'),$setsqlarr,$wheresql))
		{
				$jobarr['companyname']=$setsqlarr['companyname'];
				$jobarr['trade']=$setsqlarr['trade'];
				$jobarr['trade_cn']=$setsqlarr['trade_cn'];
				$jobarr['scale']=$setsqlarr['scale'];
				$jobarr['scale_cn']=$setsqlarr['scale_cn'];
				$jobarr['street']=$setsqlarr['street'];
				$jobarr['street_cn']=$setsqlarr['street_cn'];
				if (!$db->updatetable(table('jobs'),$jobarr," uid=".intval($_POST['cuid'])."")) adminmsg('職位修正エラー！',0);
				if (!$db->updatetable(table('jobs_tmp'),$jobarr," uid=".intval($_POST['cuid'])."")) adminmsg('職位修正エラー！',0);
				$soarray['trade']=$jobarr['trade'];
				$soarray['scale']=$jobarr['scale'];
				$soarray['street']=$setsqlarr['street'];
				$db->updatetable(table('jobs_search_scale'),$soarray," uid=".intval($_POST['cuid'])."");
				$db->updatetable(table('jobs_search_wage'),$soarray," uid=".intval($_POST['cuid'])."");
				$db->updatetable(table('jobs_search_rtime'),$soarray," uid=".intval($_POST['cuid'])."");
				$db->updatetable(table('jobs_search_stickrtime'),$soarray," uid=".intval($_POST['cuid'])."");
				$db->updatetable(table('jobs_search_hot'),$soarray," uid=".intval($_POST['cuid'])."");
				$db->updatetable(table('jobs_search_key'),$soarray," uid=".intval($_POST['cuid'])."");
				
				unset($setsqlarr);
				adminmsg("保存成功！",2,$link);
		}
		else
		{
		unset($setsqlarr);
		adminmsg("保存失敗！",0);
		}
}
elseif($act == 'order_list')
{	
	get_token();
	check_permissions($_SESSION['admin_purview'],"ord_show");
		require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
		require_once(ADMIN_ROOT_PATH.'include/admin_pay_fun.php');
	$wheresql=" WHERE o.utype=1 ";
	$oederbysql=" order BY o.addtime DESC ";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if ($key && $key_type>0)
	{
		if     ($key_type===1)$wheresql=" WHERE o.utype=1 AND c.companyname like '%{$key}%'";
		elseif ($key_type===2)$wheresql=" WHERE o.utype=1 AND m.username = '{$key}'";
		elseif ($key_type===3)$wheresql=" WHERE o.utype=1 AND o.oid ='".trim($key)."'";
		$oederbysql="";
	}
	else
	{	
		$wheresqlarr['o.utype']='1';
		!empty($_GET['is_paid'])? $wheresqlarr['o.is_paid']=intval($_GET['is_paid']):'';
		!empty($_GET['typename'])?$wheresqlarr['o.payment_name']=trim($_GET['typename']):'';
		if (is_array($wheresqlarr)) $wheresql=wheresql($wheresqlarr);
		
		if (!empty($_GET['settr']))
		{
			$settr=strtotime("-".intval($_GET['settr'])." day");
			$wheresql.=empty($wheresql)?" WHERE ": " AND ";
			$wheresql.="o.addtime> ".$settr;
		}
	}
	$joinsql=" left JOIN ".table('members')." as m ON o.uid=m.uid LEFT JOIN  ".table('company_profile')." as c ON o.uid=c.uid ";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('order')." as o ".$joinsql.$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$orderlist = get_order_list($offset,$perpage,$joinsql.$wheresql.$oederbysql);
	$smarty->assign('pageheader',"订单管理");
	$smarty->assign('payment_list',get_payment(2));
	$smarty->assign('orderlist',$orderlist);
	$smarty->assign('page',$page->show(3));
	$smarty->display('company/admin_order_list.htm');
}
elseif($act == 'show_order')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"ord_show");
	$smarty->assign('pageheader',"订单管理");
	$smarty->assign('url',$_SERVER["HTTP_REFERER"]);
	$smarty->assign('payment',get_order_one($_GET['id']));
	$smarty->display('company/admin_order_show.htm');
}
elseif($act == 'order_notes_save')
{
	check_token();
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = $_POST['url'];
	!$db->query("UPDATE ".table('order')." SET  notes='".$_POST['notes']."' WHERE id='".intval($_GET['id'])."'")?adminmsg('操作失敗',1):adminmsg("操作成功！",2,$link);
}
//设置充值记录（收款开通）
elseif($act == 'order_set')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"ord_set");
	$smarty->assign('pageheader',"订单管理");
	$smarty->assign('url',$_SERVER["HTTP_REFERER"]);
	$smarty->assign('payment',get_order_one($_GET['id']));
	$smarty->display('company/admin_order_set.htm');
}
elseif($act == 'order_set_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"ord_set");
		if (order_paid(trim($_POST['oid'])))
		{
		$link[0]['text'] = "一覧に戻る";
		$link[0]['href'] = $_POST['url'];
		!$db->query("UPDATE ".table('order')." SET notes='".$_POST['notes']."' WHERE id=".intval($_GET['id'])."  LIMIT 1 ")?adminmsg('操作失敗',1):adminmsg("操作成功！",2,$link);
		}
		else
		{
		adminmsg('操作失敗',1);
		}
}
//取消会员充值申请
elseif($act == 'order_del')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"ord_del");
	$id =!empty($_REQUEST['id'])?$_REQUEST['id']:adminmsg("项目を選択してください！",1);
	if (del_order($id))
	{
	adminmsg("取消成功！",2,$link);
	}
	else
	{
	adminmsg("取消失敗！",1);
	}
}
elseif($act == 'meal_members')
{
	get_token();
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$wheresql=" WHERE a.effective=1 ";
	$oederbysql=" order BY a.uid DESC ";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if ($key && $key_type>0)
	{
		if     ($key_type===1)$wheresql.=" AND b.username = '{$key}'";
		elseif ($key_type===2)$wheresql.=" AND b.uid = '".intval($key)."' ";
		elseif ($key_type===3)$wheresql.=" AND b.email = '{$key}'";
		elseif ($key_type===4)$wheresql.=" AND b.mobile like '{$key}%'";
		elseif ($key_type===5)$wheresql.=" AND c.companyname like '{$key}%'";
		$oederbysql="";
	}
	else
	{	
		if (!empty($_GET['setmeal_id']))
		{
			$setmeal_id=intval($_GET['setmeal_id']);
			$wheresql.=" AND a.setmeal_id=".$setmeal_id;
		}
		if (!empty($_GET['settr']))
		{
			$settr=intval($_GET['settr']);
			if ($settr==-1)
			{
			$wheresql.=" AND a.endtime<".time()." AND a.endtime>0 ";
			}
			else
			{
			$settr=strtotime("{$settr} day");
			$wheresql.="  AND a.endtime>".time()." AND a.endtime< {$settr}";
			}			
		}
	}
	$joinsql=" LEFT JOIN ".table('members')." as b ON a.uid=b.uid  LEFT JOIN ".table('company_profile')." as c ON a.uid=c.uid ";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('members_setmeal')." as a ".$joinsql.$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$member = get_meal_members_list($offset,$perpage,$joinsql.$wheresql.$oederbysql);
	$smarty->assign('pageheader',"企業管理");
	$smarty->assign('navlabel','meal_members');
	$smarty->assign('member',$member);
	$smarty->assign('setmeal',get_setmeal());	
	$smarty->assign('page',$page->show(3));
	$smarty->display('company/admin_company_meal_members.htm');
}
elseif($act == 'meal_log')
{
	get_token();
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$oederbysql=" order BY a.log_id DESC ";
	$key_uid=isset($_GET['key_uid'])?trim($_GET['key_uid']):"";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	$operation_mode=trim($_CFG['operation_mode']);
	//积分、套餐和混合三种模式变更记录，混合模式下积分和套餐变更的记录都显示
	if($operation_mode=='1')
	{
		$wheresql=" WHERE a.log_mode=1 AND a.log_utype=1";
	}
	elseif($operation_mode=='2')
	{
		$wheresql=" WHERE a.log_mode=2 AND a.log_utype=1";
	}
	else
	{
		$wheresql=" WHERE (a.log_mode=1 OR a.log_mode=2) AND a.log_utype=1";
	}
	//单个会员(uid)查看变更记录
	if ($key_uid)
	{
		$wheresql.="  AND a.log_uid = '".intval($key_uid)."' ";
		//做个标识，如果查询单个会员的话 那么右下角的搜索栏就没用了
		$smarty->assign('sign','1');
	}
	//下面的搜索栏 : 搜索某个会员的变更记录
	elseif ($key && $key_type>0)
	{
		if     ($key_type===1)$wheresql.="  AND a.log_username = '{$key}'";
		elseif ($key_type===2)$wheresql.="  AND a.log_uid = '".intval($key)."' ";
		elseif ($key_type===3)$wheresql.=" AND c.companyname like '{$key}%'";
		$oederbysql=" order BY a.log_id DESC ";
	}
	//操作类型筛选（1->系统赠送、2->会员购买、3->管理员修改、4->管理员开通）等筛选
	if (!empty($_GET['log_type']))
	{
		$log_type=intval($_GET['log_type']);
		$wheresql.=" AND  a.log_type=".$log_type;
	}
	if (!empty($_GET['settr']))
	{
		$settr=intval($_GET['settr']);
		$settr=strtotime("-{$settr} day");
		$wheresql.=" AND a.log_addtime> ".$settr;
	}
	if (!empty($_GET['is_money']))
	{
		$is_money=intval($_GET['is_money']);
		$wheresql.= " AND a.log_ismoney={$is_money}";
	}
	//三种模式 的外连接sql
	if($operation_mode=='1')
	{
		$joinsql=" LEFT JOIN ".table('members_points')." as b ON a.log_uid=b.uid  LEFT JOIN ".table('company_profile')." as c ON a.log_uid=c.uid ";
	}
	elseif($operation_mode=='2')
	{
		$joinsql=" LEFT JOIN ".table('members_setmeal')." as b ON a.log_uid=b.uid  LEFT JOIN ".table('company_profile')." as c ON a.log_uid=c.uid ";
	}
	else
	{
		$joinsql=" LEFT JOIN ".table('members_points')." as pb ON a.log_uid=pb.uid ";
		$joinsql.=" LEFT JOIN ".table('members_setmeal')." as sb ON a.log_uid=sb.uid  LEFT JOIN ".table('company_profile')." as c ON a.log_uid=c.uid ";
	}
	$total_sql="SELECT COUNT(*) AS num FROM ".table('members_charge_log')." as a ".$joinsql.$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$meallog = get_meal_members_log($offset,$perpage,$joinsql.$wheresql.$oederbysql,$operation_mode);
	$smarty->assign('pageheader','企業管理');
	$smarty->assign('navlabel','meal_log');
	$smarty->assign('meallog',$meallog);
	$smarty->assign('page',$page->show(3));
	$smarty->display('company/admin_company_meal_log.htm');
}
elseif($act == 'meal_log_pie')
{
	require_once(ADMIN_ROOT_PATH.'include/admin_flash_statement_fun.php');
	$pie_type=!empty($_GET['pie_type'])?intval($_GET['pie_type']):1;
	meal_log_pie($pie_type,1);	
	$smarty->assign('pageheader',"企業管理");
	$smarty->assign('navlabel','meal_log_pie');
	$smarty->display('company/admin_company_meal_log_pie.htm');
}
elseif($act == 'meallog_del')
{
	check_permissions($_SESSION['admin_purview'],"meallog_del");
	check_token();
	$id =!empty($_REQUEST['id'])?$_REQUEST['id']:adminmsg("選択履歴がありません！",1);
	$num=del_meal_log($id);
	if ($num>0){adminmsg("削除成功！削除件数".$num."行",2);}else{adminmsg("削除失敗！",0);}
}


elseif($act == 'meal_delay')
{
			$tuid =!empty($_REQUEST['tuid'])?$_REQUEST['tuid']:adminmsg("会員を選択してください！",1);
			$days=intval($_POST['days']);
			if (empty($days))
			{
			adminmsg("延長の日数を入力してください！",0);
			}
			if($n=delay_meal($tuid,$days))
			{
			distribution_jobs_uid($tuid);
			adminmsg("有効期限延長成功！影響行数 {$n}",2);
			}
			else
			{
			adminmsg("操作失敗！",0);
			}
}
elseif($act == 'members_list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"com_user_show");
		require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$wheresql=" WHERE m.utype=1 ";
	$oederbysql=" order BY m.uid DESC ";
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if ($key && $key_type>0)
	{
		if     ($key_type===1)$wheresql.=" AND m.username = '{$key}'";
		elseif ($key_type===2)$wheresql.=" AND m.uid = '".intval($key)."' ";
		elseif ($key_type===3)$wheresql.=" AND m.email = '{$key}'";
		elseif ($key_type===4)$wheresql.=" AND m.mobile like '{$key}%'";
		elseif ($key_type===5)$wheresql.=" AND c.companyname like '%{$key}%'";
		$oederbysql="";
	}
	else
	{	
		//注册时间
		if (!empty($_GET['settr']))
		{
			$settr=strtotime("-".intval($_GET['settr'])." day");
			$wheresql.=" AND m.reg_time> ".$settr;
		}
		//验证类型
		if (!empty($_GET['verification']))
		{
			if ($_GET['verification']=="1")
			{
			$wheresql.=" AND m.email_audit = 1";
			}
			elseif ($_GET['verification']=="2")
			{
			$wheresql.=" AND m.email_audit = 0";
			}
			elseif ($_GET['verification']=="3")
			{
			$wheresql.=" AND m.mobile_audit = 1";
			}
			elseif ($_GET['verification']=="4")
			{
			$wheresql.=" AND m.mobile_audit = 0";
			}
		}
		//有无顾问
		if ($_GET['consultant']!="")
		{
			//未分配
			$consultant=intval($_GET['consultant']);
			if ($consultant=="0")
			{
			$wheresql.=" AND  m.consultant=0";
			}
			//已分配
			elseif ($consultant=="1")
			{
			$wheresql.=" AND m.consultant != 0";
			}
		}
	}
	$joinsql=" LEFT JOIN ".table('company_profile')." as c ON m.uid=c.uid ";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('members')." as m ".$joinsql.$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$member = get_member_list($offset,$perpage,$joinsql.$wheresql.$oederbysql);
	$smarty->assign('pageheader',"企業会員");
	$smarty->assign('member',$member);
	$smarty->assign('page',$page->show(3));
	$smarty->display('company/admin_company_user_list.htm');
}
elseif($act == 'delete_user')
{	
	check_token();
	check_permissions($_SESSION['admin_purview'],"com_user_del");
	$tuid =!empty($_REQUEST['tuid'])?$_REQUEST['tuid']:adminmsg("会員を選択してください！",1);
	if ($_POST['delete'])
	{
		if (!empty($_POST['delete_user']))
		{
		!delete_company_user($tuid)?adminmsg("会員削除失敗！",0):"";
		}
		if (!empty($_POST['delete_company']))
		{
		!del_company($tuid)?adminmsg("企業資料削除失敗！",0):"";
		}
		if (!empty($_POST['delete_jobs']))
		{
		!del_company_alljobs($tuid)?adminmsg("削除職位失敗！",0):"";
		}
	adminmsg("削除成功！",2);
	}
}
//添加会员
elseif($act == 'members_add')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"com_user_add");
	$smarty->assign('pageheader',"企業会員");
	$smarty->assign('givesetmeal',get_setmeal(false));
	$smarty->assign('points',get_cache('points_rule'));
	$smarty->display('company/admin_company_user_add.htm');
}
elseif($act == 'members_add_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"com_user_add");
	require_once(ADMIN_ROOT_PATH.'include/admin_user_fun.php');
	if (strlen(trim($_POST['username']))<3) adminmsg('ユーザ名3桁以上が必須！',1);
	if (strlen(trim($_POST['password']))<6) adminmsg('パスワードは6桁以上が必要！',1);
	$sql['username'] = !empty($_POST['username']) ? trim($_POST['username']):adminmsg('ユーザ名を入力してください！',1);
	$sql['password'] = !empty($_POST['password']) ? trim($_POST['password']):adminmsg('パスワードを入力してください！',1);	
	if ($sql['password']<>trim($_POST['password1']))
	{
	adminmsg('パスワード一致しません！',1);
	}
	$sql['utype'] = !empty($_POST['member_type']) ? intval($_POST['member_type']):adminmsg('登録タイプを選択してください！',1);
	if (empty($_POST['email']) || !preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$_POST['email']))
	{
	adminmsg('メールアドレスエラー！',1);
	}
	$sql['email']= trim($_POST['email']);
	if (get_user_inusername($sql['username']))
	{
	adminmsg('該当ユーザ名すでに存在！',1);
	}
	if (get_user_inemail($sql['email']))
	{
	adminmsg('該当 Email既に使用中 ！',1);
	}
	$sql['pwd_hash'] = randstr();
	$sql['password'] = md5(md5($sql['password']).$sql['pwd_hash'].$HW_pwdhash);
	$sql['reg_time']=time();
	$sql['reg_ip']=$online_ip;
	$insert_id=$db->inserttable(table('members'),$sql,true);
			if($sql['utype']=="1")
			{
			$db->query("INSERT INTO ".table('members_points')." (uid) VALUES ('{$insert_id}')");
			$db->query("INSERT INTO ".table('members_setmeal')." (uid) VALUES ('{$insert_id}')");
				if(intval($_POST['is_money']) && $_POST['log_amount']){
					$amount=round($_POST['log_amount'],2);
					$ismoney=2;
				}else{
					$amount='0.00';
					$ismoney=1;
				}
				$regpoints_num=intval($_POST['regpoints_num']);
				if ($_POST['regpoints']=="y")
				{
				write_memberslog($insert_id,1,9001,$sql['username'],"<span style=color:#FF6600>登録会員にシステム自動贈り物!(+{$regpoints_num})</span>",1,1010,"登録会員システム自動贈り物","+{$regpoints_num}","{$regpoints_num}");
						//会员积分变更记录。管理员后台修改会员的积分。3表示：管理员后台修改
				$notes="操作人：{$_SESSION['admin_name']},説明：企業会員追加かつ贈り物(+{$regpoints_num})ポイント，费用：{$amount}円";
				write_setmeallog($insert_id,$sql['username'],$notes,4,$amount,$ismoney,1,1);
					
				report_deal($insert_id,1,$regpoints_num);
				}
				$reg_service=intval($_POST['reg_service']);
				if ($reg_service>0)
				{
				$service=get_setmeal_one($reg_service);
				write_memberslog($insert_id,1,9002,$sql['username'],"サービスActive({$service['setmeal_name']})",2,1011,"サービスActive","","");
				set_members_setmeal($insert_id,$reg_service);
						//会员积分变更记录。管理员后台修改会员的积分。3表示：管理员后台修改
				$notes="操作者：{$_SESSION['admin_name']},説明：企業会員登録及びサービス有効({$service['setmeal_name']})，費用：{$amount}円";
				write_setmeallog($insert_id,$sql['username'],$notes,4,$amount,$ismoney,2,1);
					
				}
				if(intval($_POST['is_money']) && $_POST['log_amount'] && !$notes){
				$notes="操作人：{$_SESSION['admin_name']},説明：企業会員追加，未贈り物ポイント，未申し込みコース，費用：{$amount}円";
				write_setmeallog($insert_id,$sql['username'],$notes,4,$amount,2,2,1);
				}			
			}
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = "?act=members_list";
	$link[1]['text'] = "続く追加";
	$link[1]['href'] = "?act=members_add";
	write_log("会員追加".$sql['username'], $_SESSION['admin_name'],3);
	adminmsg('追加成功！',2,$link);
}
//设置顾问
elseif($act == 'consultant_install')
{	
	//得到要设置顾问的企业会员uid 
	$tuid =!empty($_REQUEST['tuid'])?$_REQUEST['tuid']:adminmsg("会員を選択してください！",1);
	if(is_array($tuid)){
		$tuid=implode(",",$tuid);
	}
	//得到顾问信息
	$consultants = $db->getall("select * from ".table('consultant'));
	//分页
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$total_sql="SELECT COUNT(*) AS num FROM ".table('consultant').$oederbysql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$clist = get_consultant($offset,$perpage,$oederbysql);

	$smarty->assign('tuid',$tuid);
	$smarty->assign('pageheader',"顧問設定");
	$smarty->assign('page',$page->show(3));
	$smarty->assign('consultants',$consultants);
	$smarty->display('company/admin_consultant_install.htm');
}
//保存  设置顾问
elseif($act == 'consultant_install_save')
{
	//得到 顾问的id 
	$id = !empty($_GET['id'])?intval($_GET['id']):adminmsg("顧問選択エラー表示！",0);
	//得到要设置顾问的企业会员uid 
	$tuid =!empty($_REQUEST['tuid'])?$_REQUEST['tuid']:adminmsg("会員を選択してください！",1);
	$tuid=explode(",", $tuid);
	foreach ($tuid as $uid) {
		$db->updatetable(table('members'),array('consultant' => $id )," uid='{$uid}'");
	}
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = "?act=members_list";
	write_log("企業uidは".$tuid."の企業設定顧問,顧問idは".$id, $_SESSION['admin_name'],3);
	adminmsg('設定成功！',2,$link);
}
elseif($act == 'user_edit')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"com_user_edit");
	$company_user=get_user($_GET['tuid']);
	$smarty->assign('pageheader',"企業会員");
	$company_profile=get_company_one_uid($company_user['uid']);
	$company_user['tpl']=$company_profile['tpl'];
	$smarty->assign('company_user',$company_user);
	$smarty->assign('userpoints',get_user_points($company_user['uid']));
	$smarty->assign('setmeal',get_user_setmeal($company_user['uid']));
	$smarty->assign('givesetmeal',get_setmeal(false));
	$smarty->assign('url',$_SERVER["HTTP_REFERER"]);
	$smarty->display('company/admin_company_user_edit.htm');
}
elseif($act == 'set_account_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"com_user_edit");
	require_once(ADMIN_ROOT_PATH.'include/admin_user_fun.php');
	$setsqlarr['username']=trim($_POST['username']);
	$setsqlarr['email']=trim($_POST['email']);
	$setsqlarr['email_audit']=intval($_POST['email_audit']);
	$setsqlarr['mobile']=trim($_POST['mobile']);
	$setsqlarr['mobile_audit']=intval($_POST['mobile_audit']);
	if ($_POST['qq_openid']=="1")
	{
	$setsqlarr['qq_openid']='';
	}
	$thisuid=intval($_POST['company_uid']);	
	if (strlen($setsqlarr['username'])<3) adminmsg('ユーザ名3桁以上が必須！',1);
	$getusername=get_user_inusername($setsqlarr['username']);
	if (!empty($getusername)  && $getusername['uid']<>$thisuid)
	{
	adminmsg("ユーザ名 {$setsqlarr['username']}  既に存在します！",1);
	}
	if (empty($setsqlarr['email']) || !preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$setsqlarr['email']))
	{
	adminmsg('メールアドレスエラー！',1);
	}
	$getemail=get_user_inemail($setsqlarr['email']);
	if (!empty($getemail)  && $getemail['uid']<>$thisuid)
	{
	adminmsg("Email  {$setsqlarr['email']}  既に存在します！",1);
	}
	if (!empty($setsqlarr['mobile']) && !preg_match("/^(13|15|14|17|18)\d{9}$/",$setsqlarr['mobile']))
	{
	adminmsg('携帯番号エラー！',1);
	}
	$getmobile=get_user_inmobile($setsqlarr['mobile']);
	if (!empty($setsqlarr['mobile']) && !empty($getmobile)  && $getmobile['uid']<>$thisuid)
	{
	adminmsg("携帯番号 {$setsqlarr['mobile']}  既に存在します！",1);
	}
	if ($_POST['tpl'])
	{
		$tplarr['tpl']=trim($_POST['tpl']);
		$db->updatetable(table('company_profile'),$tplarr," uid='{$thisuid}'");
		$db->updatetable(table('jobs'),$tplarr," uid='{$thisuid}'");
		$db->updatetable(table('jobs_tmp'),$tplarr," uid='{$thisuid}'");
		unset($tplarr);
	}
	if ($db->updatetable(table('members'),$setsqlarr," uid=".$thisuid.""))
	{
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = $_POST['url'];
	write_log("会員uid下記に変更".$thisuid."の基本情報", $_SESSION['admin_name'],3);
	adminmsg('修正成功！',2,$link);
	}
	else
	{
	adminmsg('修正失敗！',1);
	}
}
elseif($act == 'userpoints_edit')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"com_user_edit");
	if (intval($_POST['points'])<1) adminmsg('ポイント入力してください！',1);
	if (trim($_POST['points_notes'])=='') adminmsg('ポイント操作説明を入力してください！',1);
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = $_POST['url'];
	$user=get_user($_POST['company_uid']);
	$points_type=intval($_POST['points_type']);	
	$t=$points_type==1?"+":"-";
	report_deal($user['uid'],$points_type,intval($_POST['points']));
	$points=get_user_points($user['uid']);
	write_memberslog(intval($_POST['company_uid']),1,9001,$user['username']," 管理者操作ポイント({$t}{$_POST['points']})，(残る:{$points})，コメント：".$_POST['points_notes'],1,1012,"管理者操作ポイント","{$t}{$_POST['points']}","{$points}");
		//会员积分变更记录。管理员后台修改会员的积分。3表示：管理员后台修改
		$user=get_user($_POST['company_uid']);
		if(intval($_POST['is_money']) && $_POST['log_amount']){
			$amount=round($_POST['log_amount'],2);
			$ismoney=2;
		}else{
			$amount='0.00';
			$ismoney=1;
		}
		$notes="操作人：{$_SESSION['admin_name']},説明：会員変更 {$user['username']} ポイント ({$t}{$_POST['points']})。ポイント金額：{$amount} 円，コメント：{$_POST['points_notes']}";
		write_setmeallog($_POST['company_uid'],$user['username'],$notes,3,$amount,$ismoney,1,1);
	write_log("会員uid下記に変更".$user['uid']."ポイント", $_SESSION['admin_name'],3);		
	adminmsg('保存成功！',2);
}
elseif($act == 'set_setmeal_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"com_user_edit");
    if (intval($_POST['reg_service'])>0)
	{
		if (set_members_setmeal($_POST['company_uid'],$_POST['reg_service']))
		{
		$link[0]['text'] = "一覧に戻る";
		$link[0]['href'] = $_POST['url'];
		//会员套餐变更记录。管理员后台修改会员套餐：重新开通套餐。3表示：管理员后台修改
		$user=get_user($_POST['company_uid']);
		if(intval($_POST['is_money']) && $_POST['log_amount']){
			$amount=round($_POST['log_amount'],2);
			$ismoney=2;
		}else{
			$amount='0.00';
			$ismoney=1;
		}
		$notes="操作人：{$_SESSION['admin_name']},説明：会員 {$user['username']} サービス再Active，サービス金額：{$amount}円，サービスID：{$_POST['reg_service']}。";
		write_setmeallog($_POST['company_uid'],$user['username'],$notes,4,$amount,$ismoney,2,1);
		write_log("会員uid下記に変更".$_POST['company_uid']."コース情報", $_SESSION['admin_name'],3);
		adminmsg('操作成功！',2,$link);
		}
		else
		{
		adminmsg('操作失敗！',1);
		}
	}
	else
	{
	adminmsg('コースを選択してください！',1);
	}	
}
elseif($act == 'edit_setmeal_save')
{
	check_token();
    check_permissions($_SESSION['admin_purview'],"com_user_edit");
	$setsqlarr['jobs_ordinary']=$_POST['jobs_ordinary'];
	$setsqlarr['download_resume_ordinary']=$_POST['download_resume_ordinary'];
	$setsqlarr['download_resume_senior']=$_POST['download_resume_senior'];
	$setsqlarr['interview_ordinary']=$_POST['interview_ordinary'];
	$setsqlarr['interview_senior']=$_POST['interview_senior'];
	$setsqlarr['talent_pool']=$_POST['talent_pool'];
	$setsqlarr['recommend_num']=intval($_POST['recommend_num']);
	$setsqlarr['recommend_days']=intval($_POST['recommend_days']);
	$setsqlarr['stick_num']=intval($_POST['stick_num']);
	$setsqlarr['stick_days']=intval($_POST['stick_days']);
	$setsqlarr['emergency_num']=intval($_POST['emergency_num']);
	$setsqlarr['emergency_days']=intval($_POST['emergency_days']);
	$setsqlarr['highlight_num']=intval($_POST['highlight_num']);
	$setsqlarr['highlight_days']=intval($_POST['highlight_days']);
	$setsqlarr['change_templates']=intval($_POST['change_templates']);
	$setsqlarr['jobsfair_num']=intval($_POST['jobsfair_num']);
	$setsqlarr['map_open']=intval($_POST['map_open']);

	$setsqlarr['added']=$_POST['added'];
	if ($_POST['setendtime']<>"")
	{
		$setendtime=convert_datefm($_POST['setendtime'],2);
		if ($setendtime=='')
		{
		adminmsg('日付フォーマットエラー！',0);	
		}
		else
		{
		$setsqlarr['endtime']=$setendtime;
		}
	}
	else
	{
	$setsqlarr['endtime']=0;
	}
	if ($_POST['days']<>"")
	{
			if (intval($_POST['days'])<>0)
			{
				$oldendtime=intval($_POST['oldendtime']);
				$setsqlarr['endtime']=strtotime("".intval($_POST['days'])." days",$oldendtime==0?time():$oldendtime);
			}
			if (intval($_POST['days'])=="0")
			{
				$setsqlarr['endtime']=0;
			}
	}
	$setmealtime=$setsqlarr['endtime'];
	$company_uid=intval($_POST['company_uid']);
	if ($company_uid)
	{
			$setmeal=get_user_setmeal($company_uid);
			if (!$db->updatetable(table('members_setmeal'),$setsqlarr," uid=".$company_uid."")) adminmsg('修正エラー！',0);
		//会员套餐变更记录。管理员后台修改会员套餐：修改会员。3表示：管理员后台修改
			$setmeal['endtime']=date('Y-m-d',$setmeal['endtime']);
			$setsqlarr['endtime']=date('Y-m-d',$setsqlarr['endtime']);
			$setsqlarr['log_amount']=round($_POST['log_amount']);
			$notes=edit_setmeal_notes($setsqlarr,$setmeal);
			if($notes){
				$user=get_user($_POST['company_uid']);
				$ismoney=round($_POST['log_amount'])?2:1;
				write_setmeallog($company_uid,$user['username'],$notes,3,$setsqlarr['log_amount'],$ismoney,2,1);
			}

			if ($setsqlarr['endtime']<>"")
			{
				$setmeal_deadline['setmeal_deadline']=$setmealtime;
				if (!$db->updatetable(table('jobs'),$setmeal_deadline," uid='{$company_uid}' AND add_mode='2' "))adminmsg('修正エラー！',0);
				if (!$db->updatetable(table('jobs_tmp'),$setmeal_deadline," uid='{$company_uid}' AND add_mode='2' "))adminmsg('修正エラー！',0);
				distribution_jobs_uid($company_uid);
			}
	}
	write_log("編集された会員uidは".$company_uid."コース情報", $_SESSION['admin_name'],3);
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = $_POST['url'];
	adminmsg('操作成功！',2,$link);
}
elseif($act == 'userpass_edit')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"com_user_edit");
	if (strlen(trim($_POST['password']))<6) adminmsg('新パスワード必须为6位以上！',1);
	require_once(ADMIN_ROOT_PATH.'include/admin_user_fun.php');
	$user_info=get_user_inusername($_POST['username']);
	$pwd_hash=$user_info['pwd_hash'];
	$md5password=md5(md5(trim($_POST['password'])).$pwd_hash.$HW_pwdhash);	
	if ($db->query( "UPDATE ".table('members')." SET password = '$md5password'  WHERE uid='".$user_info['uid']."'"))
	{
	write_log("会員uid下記に変更".$user_info['uid']."パスワード", $_SESSION['admin_name'],3);
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = $_POST['url'];
	adminmsg('操作成功！',2,$link);
	}
	else
	{
	adminmsg('操作失敗！',1);
	}
}
elseif($act == 'userstatus_edit')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"com_user_edit");
	if(set_user_status(intval($_POST['status']),intval($_POST['userstatus_uid'])))
	{
		write_log("会員uid下記に変更".intval($_POST['userstatus_uid'])."の状態", $_SESSION['admin_name'],3);
		$link[0]['text'] = "一覧に戻る";
		$link[0]['href'] = $_POST['url'];
		adminmsg('操作成功！',2,$link);
	}
	else
	{
	adminmsg('操作失敗！',1);
	}
}
elseif($act == 'del_auditreason')
{	
	//check_token();
	check_permissions($_SESSION['admin_purview'],"jobs_audit");//用的是职位审核的权限
	$id =!empty($_REQUEST['a_id'])?$_REQUEST['a_id']:adminmsg("ログを選択してください！",1);
	$n=reasonaudit_del($id);
	if ($n>0)
	{
	adminmsg("削除成功！削除行数 {$n} ",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
elseif($act == 'management')
{	
	$id=intval($_GET['id']);
	$u=get_user($id);
	if (!empty($u))
	{
		unset($_SESSION['uid']);
		unset($_SESSION['username']);
		unset($_SESSION['utype']);
		unset($_SESSION['uqqid']);
		setcookie("QS[uid]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
		setcookie("QS[username]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
		setcookie("QS[password]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
		setcookie("QS[utype]","",time() - 3600,$HW_cookiepath, $HW_cookiedomain);
		unset($_SESSION['activate_username']);
		unset($_SESSION['activate_email']);
		
		$_SESSION['uid']=$u['uid'];
		$_SESSION['username']=$u['username'];
		$_SESSION['utype']=$u['utype'];
		$_SESSION['uqqid']="1";
		$_SESSION['no_self']="1";
		setcookie('QS[uid]',$u['uid'],0,$HW_cookiepath,$HW_cookiedomain);
		setcookie('QS[username]',$u['username'],0,$HW_cookiepath,$HW_cookiedomain);
		setcookie('QS[password]',$u['password'],0,$HW_cookiepath,$HW_cookiedomain);
		setcookie('QS[utype]',$u['utype'], 0,$HW_cookiepath,$HW_cookiedomain);
		header("Location:".get_member_url($u['utype']));
	}	
} 
elseif($act == 'consultant')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"consultant_show");
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$oederbysql=" order BY id DESC ";
	
	$total_sql="SELECT COUNT(*) AS num FROM ".table('consultant').$oederbysql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$clist = get_consultant($offset,$perpage,$oederbysql);
	$smarty->assign('pageheader',"顧問管理");
	$smarty->assign('clist',$clist);
	$smarty->assign('page',$page->show(3));
	$smarty->display('company/admin_consultant_list.htm');
}
//顾问  管理
elseif($act == 'consultant_manage')
{
	//得到顾问id 
	$id = intval($_GET['id']);
	$sql = "select * from ".table('consultant')." where id = {$id}";
	$consultant = $db->getone($sql);
	if(empty($consultant)){
		adminmsg('顧問失った',1);
	}
	//分页
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$wheresql = " where consultant ={$id}";
	$total_sql="select count(*) as num from ".table('members')." where consultant ={$id}";
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$members = get_member_manage($offset,$perpage,$wheresql);
	$smarty->assign('pageheader',"顧問リセット");
	$smarty->assign('consultant',$consultant);
	$smarty->assign('members',$members);
	$smarty->assign('page',$page->show(3));
	$smarty->display('company/admin_consultant_manage.htm');
}
//管理 顾问  重置 按钮 
elseif($act == 'resetting')
{
	//批量和非批量得到不同的会员uid（批量得到的uid是个数组memberstuid，非批量得到的是一个id值membersids）
	$membersid =$_GET['uid'];
	$memberstuid =$_REQUEST['tuid'];
	if(empty($membersid) && empty($memberstuid)){
		adminmsg("リセットエラー発生しました！",0);
	}
	$members_id = empty($membersid)?$memberstuid:$membersid;
	$member_del_id='';
	if(is_array($members_id)){
		foreach ($members_id as  $value) {
			if(empty($member_del_id)){
				$member_del_id = $value;
			}else{
				$member_del_id = $member_del_id.','.$value;
			}
		}
	}else{
		$member_del_id = $members_id;
	}
	//对这些会员进行重置顾问
	if($db->updatetable(table('members'),array('consultant'=>0)," uid in ({$member_del_id}) ")){
		adminmsg('再設置成功!',2);
	}else{
		adminmsg('再設定失敗!',0);
	}
	

}
elseif($act == 'consultant_add')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"consultant_add");
	$smarty->assign('pageheader',"顧問管理");
	$smarty->display('company/admin_consultant_add.htm');
}
elseif($act == 'consultant_add_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"consultant_add");
	$setsqlarr['name'] = !empty($_POST['name']) ? trim($_POST['name']):adminmsg('名前を入力してください！',1);
	$setsqlarr['qq'] = !empty($_POST['qq']) ? trim($_POST['qq']):adminmsg('QQ番号を入力してください！',1);	
	
	!$_FILES['pic']['name']?adminmsg('写真をアップロード！',1):"";
	$upload_image_dir="../data/".$_CFG['updir_images']."/".date("Y/m/d/");
	make_dir($upload_image_dir);
	require_once(dirname(__FILE__).'/include/upload.php');
	$setsqlarr['pic']=_asUpFiles($upload_image_dir, "pic","2048",'gif/jpg/bmp/png',true);
	$setsqlarr['pic']=date("Y/m/d/").$setsqlarr['pic'];

	$insert_id=$db->inserttable(table('consultant'),$setsqlarr,true);
	write_log("顧問追加".$setsqlarr['name'], $_SESSION['admin_name'],3);
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = "?act=consultant";
	$link[1]['text'] = "続く追加";
	$link[1]['href'] = "?act=consultant_add";
	adminmsg('追加成功！',2,$link);
}
elseif($act == 'consultant_edit')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"consultant_edit");
	$id=intval($_GET['id']);
	if(!$id){
		adminmsg("顧問を選択してください！",1);
	}
	$consultant = get_consultant_one($id);
	$smarty->assign('consultant',$consultant);
	$smarty->assign('pageheader',"顧問管理");
	$smarty->display('company/admin_consultant_edit.htm');
}
elseif($act == 'consultant_edit_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"consultant_edit");
	$id=intval($_POST['id']);
	if(!$id){
		adminmsg("顧問を選択してください！",1);
	}
	$consultant = get_consultant_one($id);
	$setsqlarr['name'] = !empty($_POST['name']) ? trim($_POST['name']):adminmsg('名前を入力してください！',1);
	$setsqlarr['qq'] = !empty($_POST['qq']) ? trim($_POST['qq']):adminmsg('QQ番号を入力してください！',1);	
	if($_FILES['pic']['name']){
		$upload_image_dir="../data/".$_CFG['updir_images']."/".date("Y/m/d/");
		make_dir($upload_image_dir);
		require_once(dirname(__FILE__).'/include/upload.php');
		$setsqlarr['pic']=_asUpFiles($upload_image_dir, "pic","2048",'gif/jpg/bmp/png',true);
		$setsqlarr['pic']=date("Y/m/d/").$setsqlarr['pic'];
		@unlink("../data/".$_CFG['updir_images']."/".$consultant['pic']);
	}
	
	$db->updatetable(table('consultant'),$setsqlarr," id={$id} ");
	write_log("変更顧問idは".$id."の顧問情報", $_SESSION['admin_name'],3);
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = "?act=consultant";
	$link[1]['text'] = "変更結果閲覧";
	$link[1]['href'] = "?act=consultant_edit&id={$id}";
	adminmsg('修正成功！',2,$link);
}
elseif($act == "consultant_del"){
	check_permissions($_SESSION['admin_purview'],"consultant_del");
	$id=intval($_GET['id']);
	if(!$id){
		adminmsg("顧問を選択してください！",1);
	}
	del_consultant($id);
	adminmsg("削除成功！",2);
}
?>
