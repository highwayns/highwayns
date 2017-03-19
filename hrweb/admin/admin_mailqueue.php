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
require_once(ADMIN_ROOT_PATH.'include/admin_mailqueue_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"mailqueue");
$smarty->assign('pageheader',"邮件群发");
if($act == 'list')
{
	get_token();
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if (!empty($key) && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE m_subject like '%{$key}%'";
		if     ($key_type===2)$wheresql=" WHERE m_mail = '{$key}'";
		$oederbysql="";
	}
	$_GET['m_type']<>''? $wheresqlarr['m_type']=intval($_GET['m_type']):'';
	if (!empty($wheresqlarr)) $wheresql=wheresql($wheresqlarr);
	$total_sql="SELECT COUNT(*) AS num FROM ".table('mailqueue').$wheresql;
	$page = new page(array('total'=>$db->get_total($total_sql), 'perpage'=>$perpage));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$list = get_mailqueue($offset,$perpage,$wheresql.$oderbysql);
	$smarty->assign('navlabel',"list");
	$smarty->assign('list',$list);
	$smarty->assign('page',$page->show(3));
	$smarty->display('mailqueue/admin_mailqueue_list.htm');
}
elseif($act == 'mailqueue_add')
{
	get_token();
	$label[]=array('{sitename}','网站名称');
	$label[]=array('{sitedomain}','网站域名');
	$label[]=array('{sitelogo}','网站LOGO');
	$label[]=array('{address}','联系地址');
	$label[]=array('{tel}','联系电话');
	$smarty->assign('label',$label);
	$smarty->assign('navlabel','add');
	$smarty->display('mailqueue/admin_mailqueue_add.htm');
}
elseif($act == 'mailqueue_add_save')
{
	check_token();
	$setsqlarr['m_mail']=trim($_POST['m_mail'])?trim($_POST['m_mail']):adminmsg('邮件地址必须填写！',1);
	if (!preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$setsqlarr['m_mail'])) 
    {
	adminmsg('邮箱格式错误！',1);
    }
	$uid=$db->getone('select uid from '.table('members')." where email= '{$setsqlarr['m_mail']}' limit 1 ");
	$setsqlarr['m_subject']=trim($_POST['m_subject'])?replace_label($_POST['m_subject']):adminmsg('邮件标题必须填写！',1);	
	$setsqlarr['m_body']=trim($_POST['m_body'])?replace_label($_POST['m_body']):adminmsg('邮件内容必须填写！',1);
	$setsqlarr['m_addtime']=time();
	$setsqlarr['m_uid']=$uid['uid'];
	$link[0]['text'] = "继续添加";
	$link[0]['href'] = '?act=mailqueue_add';
	$link[1]['text'] = "返回列表";
	$link[1]['href'] = '?';
	write_log("添加邮件队列 ", $_SESSION['admin_name'],3);
	!$db->inserttable(table('mailqueue'),$setsqlarr)?adminmsg("添加失败！",0):adminmsg("添加成功！",2,$link);
}
elseif($act == 'mailqueue_edit')
{
	get_token();
	$label[]=array('{sitename}','网站名称');
	$label[]=array('{sitedomain}','网站域名');
	$label[]=array('{sitelogo}','网站LOGO');
	$label[]=array('{address}','联系地址');
	$label[]=array('{tel}','联系电话');
	$smarty->assign('label',$label);
	$smarty->assign('show',get_mailqueue_one($_GET['id']));
	$smarty->display('mailqueue/admin_mailqueue_edit.htm');
}
elseif($act == 'mailqueue_edit_save')
{
	check_token();
	$setsqlarr['m_mail']=trim($_POST['m_mail'])?trim($_POST['m_mail']):adminmsg('邮件地址必须填写！',1);
	if (!preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/", $setsqlarr['m_mail'])) 
    {
	adminmsg('邮箱格式错误！',1);
    }
	$setsqlarr['m_subject']=trim($_POST['m_subject'])?replace_label($_POST['m_subject']):adminmsg('邮件标题必须填写！',1);
	$setsqlarr['m_body']=trim($_POST['m_body'])?replace_label($_POST['m_body']):adminmsg('邮件内容必须填写！',1);
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?';
	$wheresql=" m_id='".intval($_POST['id'])."' ";
	!$db->updatetable(table('mailqueue'),$setsqlarr,$wheresql)?adminmsg("修改失败！",0):adminmsg("修改成功！",2,$link);
}
elseif($act == 'mailqueue_batchadd')
{
	get_token();
	$label[]=array('{sitename}','网站名称');
	$label[]=array('{sitedomain}','网站域名');
	$label[]=array('{username}','会员用户名');
	$label[]=array('{lastlogintime}','最后登录时间');
	$label2[]=array('{sitelogo}','网站LOGO');
	$label2[]=array('{address}','联系地址');
	$label2[]=array('{tel}','联系电话');
	$smarty->assign('label',$label);
	$smarty->assign('label2',array_merge($label,$label2));
	$smarty->assign('navlabel','batchadd');
	$smarty->display('mailqueue/admin_mailqueue_batchadd.htm');
}
elseif($act == 'mailqueue_batchadd_save')
{
	check_token();
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
	$m_subject=!empty($_POST['m_subject'])?trim($_POST['m_subject']):adminmsg('邮件标题必须填写！',1);	
	$m_body=!empty($_POST['m_body'])?trim($_POST['m_body']):adminmsg('邮件内容必须填写！',1);
	$result = $db->query("SELECT * FROM ".table('members').$wheresql);
	$n=0;
	while($user = $db->fetch_array($result))
	{
 		$setsqlarr['m_uid']=$user['uid'];
 		$setsqlarr['m_mail']=$user['email'];
		$setsqlarr['m_subject']=replace_label($m_subject,$user);	
		$setsqlarr['m_body']=replace_label($m_body,$user);
		$setsqlarr['m_addtime']=time();
		!$db->inserttable(table('mailqueue'),$setsqlarr)?adminmsg("添加失败！",0):'';
		$n++;
	}
	$link[0]['text'] = "返回列表";
	$link[0]['href'] = '?';
	write_log("批量添加邮件队列，共添加 {$n} 行 ", $_SESSION['admin_name'],3);
	adminmsg("添加成功，共添加 {$n} 行 ",2,$link);
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
			$result = $db->query("SELECT m_id FROM ".table('mailqueue')." WHERE  m_id IN ({$sqlin}) {$limit}");
			while($row = $db->fetch_array($result))
			{
			$idarr[] = $row['m_id'];
			}
			if (empty($idarr))
			{
				adminmsg("没有可发送的邮件",1);
			}
			@file_put_contents(QISHI_ROOT_PATH."temp/send.txt", serialize($idarr));
			header("Location:?act=send&senderr={$senderr}&intervaltime={$intervaltime}");
		}
		
	}
	elseif ($sendtype===2)
	{
			$result = $db->query("SELECT m_id FROM ".table('mailqueue')." WHERE m_type=0 {$limit}");
			while($row = $db->fetch_array($result))
			{
			$idarr[] = $row['m_id'];
			}
			if (empty($idarr))
			{
				adminmsg("没有可发送的邮件",1);
			}
			@file_put_contents(QISHI_ROOT_PATH."temp/send.txt", serialize($idarr));
			header("Location:?act=send&senderr={$senderr}&intervaltime={$intervaltime}");
	}
	elseif ($sendtype===3)
	{
			$result = $db->query("SELECT m_id FROM ".table('mailqueue')." WHERE m_type=2 {$limit}");
			while($row = $db->fetch_array($result))
			{
			$idarr[] = $row['m_id'];
			}
			if (empty($idarr))
			{
				adminmsg("没有可发送的邮件",1);
			}
			@file_put_contents(QISHI_ROOT_PATH."temp/send.txt", serialize($idarr));
			header("Location:?act=send&senderr={$senderr}&intervaltime={$intervaltime}");
	}
}
elseif($act == 'send')
{
	$senderr=intval($_GET['senderr']);
	$intervaltime=intval($_GET['intervaltime']);
	$tempdir=QISHI_ROOT_PATH."temp/send.txt";
	$content = file_get_contents($tempdir);
	$idarr = unserialize($content);
	$totalid=count($idarr);
	if (empty($idarr))
	{
		$link[0]['text'] = "返回邮件列队";
		$link[0]['href'] = '?act=list';
		adminmsg("任务执行完毕!",2,$link);
	}
	else
	{
		 $m_id=array_shift($idarr);
		 @file_put_contents($tempdir,serialize($idarr));
		 $mail =$db->getone("select * from ".table('mailqueue')." where m_id = '".intval($m_id)."' LIMIT 1");
		 $mailconfig=get_cache('mailconfig');
		 	if (!smtp_mail($mail['m_mail'],$mail['m_subject'],$mail['m_body']))
			{
				$db->query("update  ".table('mailqueue')." SET m_type='2'  WHERE m_id = '".intval($m_id)."'  LIMIT 1");
				if ($senderr=="2")
				{
				$link[0]['text'] = "返回邮件列队";
				$link[0]['href'] = '?act=list';
				adminmsg('邮件发送发生错误！'.$senderr,0,$link);
				}
				else
				{
				$link[0]['text'] = "发送下一封";
				$link[0]['href'] = "?act=send&senderr={$senderr}&intervaltime={$intervaltime}";
				adminmsg("发生错误，准备发送下一封，剩余任务总数：".($totalid-1),0,$link,true,$intervaltime);
				}			
			}
			else
			{
			$db->query("update  ".table('mailqueue')." SET m_type='1',m_sendtime='".time()."'  WHERE m_id = '".intval($m_id)."'  LIMIT 1");
			$link[0]['text'] = "发送下一封";
			$link[0]['href'] = "?act=send&senderr={$senderr}&intervaltime={$intervaltime}";
			adminmsg("发送成功，准备发送下一封，剩余任务总数：".($totalid-1),2,$link,true,$intervaltime);
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
		$db->query("Delete from ".table('mailqueue')." WHERE m_id IN ({$sqlin}) ");
		adminmsg("删除成功",2);
		}
	}
	elseif ($deltype===2)
	{
		$db->query("Delete from ".table('mailqueue')." WHERE m_type=0 ");
		adminmsg("删除成功 $delnum",2);
	}
	elseif ($deltype===3)
	{
		$db->query("Delete from ".table('mailqueue')." WHERE m_type=1 ");
		adminmsg("删除成功",2);
	}
	elseif ($deltype===4)
	{
		$db->query("Delete from ".table('mailqueue')." WHERE m_type=2 ");
		adminmsg("删除成功",2);
	}
	elseif ($deltype===5)
	{
		$db->query("Delete from ".table('mailqueue')."");
		adminmsg("删除成功",2);
	}
}
?>