<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_smsqueue_fun.php');
$act = !empty($_GET['act']) ? trim($_GET['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"smsqueue");
$smarty->assign('pageheader',"ショートメッセージ");
if($act == 'list')
{
	get_token();
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
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
	$label[]=array('{sitename}','ウェブ名');
	$label[]=array('{sitedomain}','ドメイン');
	$label[]=array('{sitelogo}','ウェブLOGO');
	$label[]=array('{address}','連絡先');
	$label[]=array('{tel}','連絡電話');
	$smarty->assign('label',$label);
	$smarty->assign('navlabel','add');
	$smarty->display('smsqueue/admin_smsqueue_add.htm');
}
elseif($act == 'smsqueue_add_save')
{
	check_token();
	$setsqlarr['s_sms']=trim($_POST['s_sms'])?trim($_POST['s_sms']):adminmsg('携帯番号を入力してください！',1);
	$s_body=trim($_POST['s_body'])?trim($_POST['s_body']):adminmsg('ショートメッセージ内容を入力してください',1);
	mb_strlen(trim($_POST['s_body']),'utf-8')>70?adminmsg('ショートメッセージ70文字まで，再入力してください！',1):'';
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
	$link[0]['text'] = "続く追加";
	$link[0]['href'] = '?act=smsqueue_add';
	$link[1]['text'] = "一覧に戻る";
	$link[1]['href'] = '?';
	adminmsg("追加成功{$num}！",2,$links);
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
	$setsqlarr['s_sms']=trim($_POST['s_sms'])?trim($_POST['s_sms']):adminmsg('携帯番号を入力してください！',1);
	$s_body=trim($_POST['s_body'])?trim($_POST['s_body']):adminmsg('ショートメッセージ内容を入力してください',1);
	mb_strlen(trim($_POST['s_body']),'utf-8')>70?adminmsg('ショートメッセージ70文字まで，再入力してください！',1):'';
	$wheresql=" s_id='".intval($_POST['id'])."' ";
	$link[0]['text'] = "一覧に戻る";
	$link[0]['href'] = '?';
	if (preg_match("/^(13|15|14|17|18)\d{9}$/",$setsqlarr['s_sms']))
	{
		$smssqlarr['s_body']=$s_body;
		$smssqlarr['s_addtime']=time();
		$smssqlarr['s_mobile']=$setsqlarr['s_sms'];
		!$db->updatetable(table('smsqueue'),$smssqlarr,$wheresql)?adminmsg("変更失敗！",0):adminmsg("変更成功！",2,$link);
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
	$s_body=trim($_POST['s_body'])?trim($_POST['s_body']):adminmsg('ショートメッセージ内容を入力してください',1);
	mb_strlen(trim($_POST['s_body']),'utf-8')>70?adminmsg('ショートメッセージ70文字まで，再入力してください！',1):'';
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
				!$db->inserttable(table('smsqueue'),$smssqlarr)?adminmsg("追加失敗！",0):'';
				$num++;
			}
	}
	adminmsg("追加成功{$num}！",2);
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
		adminmsg("項目を選択してください！",1);
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
				adminmsg("発送可能ショートメッセージがない",1);
			}
			@file_put_contents(HIGHWAY_ROOT_PATH."temp/sendsms.txt", serialize($idarr));
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
				adminmsg("発送可能ショートメッセージがない",1);
			}
			@file_put_contents(HIGHWAY_ROOT_PATH."temp/sendsms.txt", serialize($idarr));
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
				adminmsg("発送可能ショートメッセージがない",1);
			}
			@file_put_contents(HIGHWAY_ROOT_PATH."temp/sendsms.txt", serialize($idarr));
			header("Location:?act=send&senderr={$$senderr}&intervaltime={$intervaltime}");
	}
}
elseif($act == 'send')
{
	$senderr=intval($_GET['senderr']);
	$intervaltime=intval($_GET['intervaltime']);
	$tempdir=HIGHWAY_ROOT_PATH."temp/sendsms.txt";
	$content = file_get_contents($tempdir);
	$idarr = unserialize($content);
	$totalid=count($idarr);
	if (empty($idarr))
	{
		$link[0]['text'] = "ショートメッセージキューに返す";
		$link[0]['href'] = '?act=list';
		adminmsg("タスク実行完毕!",2,$link);
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
				$link[0]['text'] = "ショートメッセージキューに返す";
				$link[0]['href'] = '?act=list';
				adminmsg('ショートメッセージ発送エラー！'.$senderr,0,$link);
				}
				else
				{
				$link[0]['text'] = "次の送信";
				$link[0]['href'] = "?act=send&senderr={$$senderr}&intervaltime={$intervaltime}";
				adminmsg("エラー発生しました，次の送信準備しました，残るタスク数：".($totalid-1),0,$link,true,$intervaltime);
				}			
			}
			else
			{
			$db->query("update  ".table('smsqueue')." SET s_type='1',s_sendtime='".time()."'  WHERE s_id = '".intval($s_id)."'  LIMIT 1");
			$link[0]['text'] = "次の送信";
			$link[0]['href'] = "?act=send&senderr={$$senderr}&intervaltime={$intervaltime}";
			adminmsg("送信成功，次の送信を準備します，残るタスク数：".($totalid-1),2,$link,true,$intervaltime);
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
		adminmsg("項目を選択してください！",1);
		}
		if(!is_array($id)) $id=array($id);
		$sqlin=implode(",",$id);
		if (preg_match("/^(\d{1,10},)*(\d{1,10})$/",$sqlin))
		{
		$db->query("Delete from ".table('smsqueue')." WHERE s_id IN ({$sqlin}) ");
		adminmsg("削除成功",2);
		}
	}
	elseif ($deltype===2)
	{
		$db->query("Delete from ".table('smsqueue')." WHERE s_type=0 ");
		adminmsg("削除成功 $delnum",2);
	}
	elseif ($deltype===3)
	{
		$db->query("Delete from ".table('smsqueue')." WHERE s_type=1 ");
		adminmsg("削除成功",2);
	}
	elseif ($deltype===4)
	{
		$db->query("Delete from ".table('smsqueue')." WHERE s_type=2 ");
		adminmsg("削除成功",2);
	}
	elseif ($deltype===5)
	{
		$db->query("Delete from ".table('smsqueue')."");
		adminmsg("削除成功",2);
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
			$v['mobile']=$v['mobile']?$v['mobile']:'未入力';
			$v['email']=$v['email']?$v['email']:'未入力';
			$contents.= '★ ユーザ名：'.$v['username'].'                 携帯：'.$v['mobile'].'                     メール：'.$v['email']."\r\n\r\n"; 
	}
  	$time=date("Y-m-d H:i:s",time());
	$header="===================================会員情報ファイル，条件満たすの合計{$total_val}人，導出時間：{$time}========================================"."\r\n\r\n";
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
