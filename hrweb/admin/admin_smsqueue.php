<?php
 /*
 * 74cms 邮件群发
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_smsqueue_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"smsqueue");
$smarty->assign('pageheader',"短信营销");
if($act == 'list')
{
	get_token();
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if (!empty($key) && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE s_body like '%{$key}%'";
		if     ($key_type===2)$wheresql=" WHERE s_mobile = '{$key}'";
		$oederbysql="";
	}
	$_GET['s_type']<>''? $wheresqlarr['s_type']=intval($_GET['s_type']):'';
	if (!empty($wheresqlarr)) $wheresql=wheresql($wheresqlarr);
	$total_sql="SELECT COUNT(*) AS num FROM ".table('smsqueue').$wheresql;
	$page = new page(array('total'=>$db->get_total($total_sql), 'perpage'=>$perpage));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_smsqueue($offset,$perpage,$wheresql.$oderbysql);
	$smarty->assign('navlabel',"list");
	$smarty->assign('list',$list);
	$smarty->assign('page',$page->show(3));
	$smarty->display('smsqueue/admin_smsqueue_list.htm');
}
elseif($act == 'smsqueue_add')
{
	get_token();
	$label[]=array('{sitename}','网站名称');
	$label[]=array('{sitedomain}','网站域名');
	$label[]=array('{sitelogo}','网站LOGO');
	$label[]=array('{address}','联系地址');
	$label[]=array('{tel}','联系电话');
	$smarty->assign('label',$label);
	$smarty->assign('navlabel','add');
	$smarty->display('smsqueue/admin_smsqueue_add.htm');
}
elseif($act == 'smsqueue_add_save')
{
	check_token();
	$setsqlarr['s_sms']=trim($_POST['s_sms'])?trim($_POST['s_sms']):adminmsg('手机号码必须填写！',1);
	$s_body=trim($_POST['s_body'])?trim($_POST['s_body']):adminmsg('请填写短信内容',1);
	mb_strlen(trim($_POST['s_body']),'gb2312')>70?adminmsg('短信内容超过70个字，请重新输入！',1):'';
	$mobile_arr=explode('|',$setsqlarr['s_sms']);
	$mobile_arr=array_unique($mobile_arr);
	foreach($mobile_arr as $list){
		if (preg_match("/^(13|15|14|17|18)\d{9}$/",$list))
		{
			$uid=$db->getone('select uid from '.table('members')." where mobile= '{$list}' limit 1 ");
			$smssqlarr['s_uid']=$uid['uid'];
			$smssqlarr['s_body']=$s_body;
			$smssqlarr['s_addtime']=time();
			$smssqlarr['s_mobile']=$list;
			$db->inserttable(table('smsqueue'),$smssqlarr);
			$num++;
		}
	}
	$link[0]['text'] = "继续添加";
	$link[0]['href'] = '?act=smsqueue_add';
	$link[1]['text'] = "返回列表";
	$link[1]['href'] = '?';
	adminmsg("添加成功{$num}！",2,$links);
}
elseif($act == 'smsqueue_edit')
{
	get_token();
	$smarty->assign('show',get_smsqueue_one($_GET['id']));
	$smarty->display('smsqueue/admin_smsqueue_edit.htm');
}
elseif($act == 'smsqueue_edit_save')
{
	check_token();
	$setsqlarr['s_sms']=trim($_POST['s_sms'])?trim($_POST['s_sms']):adminmsg('手机号码必须填写！',1);
	$s_body=trim($_POST['s_body'])?trim($_POST['s_body']):adminmsg('请填写短信内容',1);
	mb_strlen(trim($_POST['s_body']),'gb2312')>70?adminmsg('短信内容超过70个字，请重新输入！',1):'';
	$wheresql=" s_id='".intval($_POST['id'])."' ";
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?';
	if (preg_match("/^(13|15|14|17|18)\d{9}$/",$setsqlarr['s_sms']))
	{
		$smssqlarr['s_body']=$s_body;
		$smssqlarr['s_addtime']=time();
		$smssqlarr['s_mobile']=$setsqlarr['s_sms'];
		!$db->updatetable(table('smsqueue'),$smssqlarr,$wheresql)?adminmsg("修改失败！",0):adminmsg("修改成功！",2,$link);
	}
}
elseif($act == 'smsqueue_batchadd')
{
	get_token();
	$smarty->assign('setmeal',get_setmeal());	
	$smarty->assign('navlabel','batchadd');
	$smarty->display('smsqueue/admin_smsqueue_batchadd.htm');
}
elseif($act == 'smsqueue_batchadd_save')
{
	check_token();
	$s_body=trim($_POST['s_body'])?trim($_POST['s_body']):adminmsg('请填写短信内容',1);
	mb_strlen(trim($_POST['s_body']),'gb2312')>70?adminmsg('短信内容超过70个字，请重新输入！',1):'';
	$selutype=intval($_POST['selutype']);
	$selsettr=intval($_POST['selsettr']);
	if ($selutype>0)
	{
	$wheresql=" WHERE utype='{$selutype}' ";
	}	
	if ($selsettr>0)
	{
		$wheresql.=empty($wheresql)?" WHERE ":" AND ";
		$data=strtotime("-{$selsettr} day");
		$wheresql.=" last_login_time<".$data;
	}
	if (!empty($_POST['verification']))
	{
		if ($_POST['verification']=="1")
		{
		$wheresql.=" AND  email_audit = 1";
		}
		elseif ($_POST['verification']=="2")
		{
		$wheresql.=" AND  email_audit = 0";
		}
		elseif ($_POST['verification']=="3")
		{
		$wheresql.=" AND  mobile_audit = 1";
		}
		elseif ($_POST['verification']=="4")
		{
		$wheresql.=" AND  mobile_audit = 0";
		}
	}
 	$result = $db->query("SELECT * FROM ".table('members').$wheresql);

 	while($user = $db->fetch_array($result))
	{
 			if(preg_match("/^(13|15|14|17|18)\d{9}$/",$user['mobile'])){
				$smssqlarr['s_uid']=$user['uid'];
				$smssqlarr['s_body']=$s_body;
				$smssqlarr['s_addtime']=time();
				$smssqlarr['s_mobile']=$user['mobile'];
				!$db->inserttable(table('smsqueue'),$smssqlarr)?adminmsg("添加失败！",0):'';
				$num++;
			}
	}
	adminmsg("添加成功{$num}！",2);
}
elseif($act == 'totalsend')
{
	$sendtype=intval($_POST['sendtype']);
	$intervaltime=intval($_POST['intervaltime'])==0?3:intval($_POST['intervaltime']);
	$sendmax=intval($_POST['sendmax']);
	$senderr=intval($_POST['senderr']);
	if ($sendmax>0)
	{
	$limit=" LIMIT {$sendmax} ";
	}
	if ($sendtype===1)
	{
		$id=$_POST['id'];
		if (empty($id))
		{
		adminmsg("请选择项目！",1);
		}
		if(!is_array($id)) $id=array($id);
		$sqlin=implode(",",$id);
		if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
		{
			$result = $db->query("SELECT s_id FROM ".table('smsqueue')." WHERE s_id IN ({$sqlin}) {$limit}");
			while($row = $db->fetch_array($result))
			{
			$idarr[] = $row['s_id'];
			}
			if (empty($idarr))
			{
				adminmsg("没有可发送的短信",1);
			}
			@file_put_contents(QISHI_ROOT_PATH."temp/sendsms.txt", serialize($idarr));
			header("Location:?act=send&senderr={$$senderr}&intervaltime={$intervaltime}");
		}
		
	}
	elseif ($sendtype===2)
	{
			$result = $db->query("SELECT s_id FROM ".table('smsqueue')." WHERE s_type=0 {$limit}");
			while($row = $db->fetch_array($result))
			{
			$idarr[] = $row['s_id'];
			}
			if (empty($idarr))
			{
				adminmsg("没有可发送的短信",1);
			}
			@file_put_contents(QISHI_ROOT_PATH."temp/sendsms.txt", serialize($idarr));
			header("Location:?act=send&senderr={$$senderr}&intervaltime={$intervaltime}");
	}
	elseif ($sendtype===3)
	{
			$result = $db->query("SELECT s_id FROM ".table('smsqueue')." WHERE s_type=2 {$limit}");
			while($row = $db->fetch_array($result))
			{
			$idarr[] = $row['s_id'];
			}
			if (empty($idarr))
			{
				adminmsg("没有可发送的短信",1);
			}
			@file_put_contents(QISHI_ROOT_PATH."temp/sendsms.txt", serialize($idarr));
			header("Location:?act=send&senderr={$$senderr}&intervaltime={$intervaltime}");
	}
}
elseif($act == 'send')
{
	$senderr=intval($_GET['senderr']);
	$intervaltime=intval($_GET['intervaltime']);
	$tempdir=QISHI_ROOT_PATH."temp/sendsms.txt";
	$content = file_get_contents($tempdir);
	$idarr = unserialize($content);
	$totalid=count($idarr);
	if (empty($idarr))
	{
		$link[0]['text'] = "返回短信列队";
		$link[0]['href'] = '?act=list';
		adminmsg("任务执行完毕!",2,$link);
	}
	else
	{
		 $s_id=array_shift($idarr);
		 @file_put_contents($tempdir,serialize($idarr));
		 $sms =$db->getone("select * from ".table('smsqueue')." where s_id = '".intval($s_id)."' LIMIT 1");
		 
		 
		// $mailconfig=get_cache('mailconfig');
		 	if (free_send_sms($sms['s_mobile'],$sms['s_body'])!='success')
			{
				$db->query("update  ".table('smsqueue')." SET s_type='2'  WHERE s_id = '".intval($s_id)."'  LIMIT 1");
				if ($senderr=="2")
				{
				$link[0]['text'] = "返回短信列队";
				$link[0]['href'] = '?act=list';
				adminmsg('短信发送发生错误！'.$senderr,0,$link);
				}
				else
				{
				$link[0]['text'] = "发送下一条";
				$link[0]['href'] = "?act=send&senderr={$$senderr}&intervaltime={$intervaltime}";
				adminmsg("发生错误，准备发送下一条，剩余任务总数：".($totalid-1),0,$link,true,$intervaltime);
				}			
			}
			else
			{
			$db->query("update  ".table('smsqueue')." SET s_type='1',s_sendtime='".time()."'  WHERE s_id = '".intval($s_id)."'  LIMIT 1");
			$link[0]['text'] = "发送下一条";
			$link[0]['href'] = "?act=send&senderr={$$senderr}&intervaltime={$intervaltime}";
			adminmsg("发送成功，准备发送下一条，剩余任务总数：".($totalid-1),2,$link,true,$intervaltime);
			}
	}	
}
elseif($act == 'del')
{
	$n=0;
	$deltype=intval($_POST['deltype']);
	if ($deltype===1)
	{
		$id=$_POST['id'];
		if (empty($id))
		{
		adminmsg("请选择项目！",1);
		}
		if(!is_array($id)) $id=array($id);
		$sqlin=implode(",",$id);
		if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
		{
		$db->query("Delete from ".table('smsqueue')." WHERE s_id IN ({$sqlin}) ");
		adminmsg("删除成功",2);
		}
	}
	elseif ($deltype===2)
	{
		$db->query("Delete from ".table('smsqueue')." WHERE s_type=0 ");
		adminmsg("删除成功 $delnum",2);
	}
	elseif ($deltype===3)
	{
		$db->query("Delete from ".table('smsqueue')." WHERE s_type=1 ");
		adminmsg("删除成功",2);
	}
	elseif ($deltype===4)
	{
		$db->query("Delete from ".table('smsqueue')." WHERE s_type=2 ");
		adminmsg("删除成功",2);
	}
	elseif ($deltype===5)
	{
		$db->query("Delete from ".table('smsqueue')."");
		adminmsg("删除成功",2);
	}
}
/*导出用户信息*/
elseif($act == 'export_info')
{
  	$selutype=intval($_POST['selutype']);
	$selsettr=intval($_POST['selsettr']);
	if ($selutype>0)
	{
	$wheresql=" WHERE utype='{$selutype}' ";
	}	
	if ($selsettr>0)
	{
		$wheresql.=empty($wheresql)?" WHERE ":" AND ";
		$data=strtotime("-{$selsettr} day");
		$wheresql.=" last_login_time<".$data;
	}
	if (!empty($_POST['verification']))
	{
		if ($_POST['verification']=="1")
		{
		$wheresql.=" AND  email_audit = 1";
		}
		elseif ($_POST['verification']=="2")
		{
		$wheresql.=" AND  email_audit = 0";
		}
		elseif ($_POST['verification']=="3")
		{
		$wheresql.=" AND  mobile_audit = 1";
		}
		elseif ($_POST['verification']=="4")
		{
		$wheresql.=" AND  mobile_audit = 0";
		}
	}
 	$total_sql="SELECT COUNT(*) AS num FROM ".table('members').$wheresql;
	$total_val=$db->get_total($total_sql);
 	$result = $db->query("SELECT * FROM ".table('members').$wheresql);
 	while($v = $db->fetch_array($result))
	{
			$v['mobile']=$v['mobile']?$v['mobile']:'未填写';
			$v['email']=$v['email']?$v['email']:'未填写';
			$contents.= '★ 用户名：'.$v['username'].'                 手机号：'.$v['mobile'].'                     邮箱：'.$v['email']."\r\n\r\n"; 
	}
  	$time=date("Y-m-d H:i:s",time());
	$header="===================================会员信息文件，符合条件的总计{$total_val}个，导出时间：{$time}========================================"."\r\n\r\n";
	$txt=$header.$contents;
	header("Content-type:application/octet-stream"); 
	header("Content-Disposition: attachment; filename=userinfo.txt"); 
	echo $txt;	

}
elseif($act == "import_num"){
	$file = $_FILES['number_file']['tmp_name'];
	$content = file_get_contents($file);
	$array = explode("\r\n", $content);
	$str = implode("|",$array);
	$str = trim($str,"|");
	$smarty->assign('import_numbers',$str);
	$smarty->display('smsqueue/admin_smsqueue_add.htm');
}

?>