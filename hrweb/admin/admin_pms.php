<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_pms_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
check_permissions($_SESSION['admin_purview'],"help");
$smarty->assign('pageheader',"メッセージ");	
$smarty->assign('act',$act);
if($act == 'list')
{
	get_token();
	require_once(HIGHWAY_ROOT_PATH.'include/page.class.php');
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	$oederbysql=" order BY `spmid` DESC";
	if ($key && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE message like '%{$key}%'";
	}
	if ($_GET['spms_usertype']<>'')
	{
	$wheresqlarr['spms_usertype']=intval($_GET['spms_usertype']);
	}
	!empty($_GET['spms_type'])? $wheresqlarr['spms_type']=intval($_GET['spms_type']):'';
	if (!empty($wheresqlarr)) $wheresql=wheresql($wheresqlarr);
	$total_sql="SELECT COUNT(*) AS num FROM ".table('pms_sys').$wheresql;
	$page = new page(array('total'=>$db->get_total($total_sql), 'perpage'=>$perpage));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$pmslist = get_pmssys($offset, $perpage,$wheresql.$oederbysql);
	$smarty->assign('pmslist',$pmslist);
	$smarty->assign('page',$page->show(3));	
	$smarty->display('pms/admin_pms_sys_list.htm');
}
elseif($act == 'add_pms_sys')
{
	get_token();
	$smarty->display('pms/admin_pms_sys_add.htm');
}
elseif($act == 'add_pms_sys_save')
{
	check_token();
	if (empty($_POST['us']))
	{
		$setsqlarr['spms_usertype']=intval($_POST['spms_usertype']);
		$setsqlarr['spms_type']=intval($_POST['spms_type']);
		$setsqlarr['dateline']=time();
		$setsqlarr['message']=trim($_POST['message']);
		$link[0]['text'] = "続く追加";
		$link[0]['href'] = '?act=add_pms_sys';
		$link[1]['text'] = "一覧に戻る";
		$link[1]['href'] = '?act=list';
		!$db->inserttable(table('pms_sys'),$setsqlarr)?adminmsg("追加失敗！",0):adminmsg("追加成功！",2,$link);
	}
	else
	{
		$tosuername=explode("\n",$_POST['us']);
		if (count($tosuername)==0 || empty($tosuername))
		{
		adminmsg("ユーザ名入力エラー！",1);
		}
		else
		{
			$s=0;
			$msg=trim($_POST['msg']);
			$time=time();
			foreach ($tosuername as $u)
			{ 
				$u=trim($u);
				if(!empty($u))
				{
					$userinfo= $db->getone("select uid,username from ".table('members')." where username = '{$u}' LIMIT 1");
					if (intval($userinfo['uid'])>0)
					{
						$setsqlarr['msgtype']=1;
						$setsqlarr['msgtouid']=$userinfo['uid'];
						$setsqlarr['msgtoname']=$userinfo['username'];
						$setsqlarr['message']=$msg;
						$setsqlarr['dateline']=$time;
						$setsqlarr['replytime']=$time;
						$setsqlarr['new']=1;
						$db->inserttable(table('pms'),$setsqlarr);
						unset($userinfo,$setsqlarr);
						$s++;
					}
					
				}
 			}
			if ($s>0)
			{
			$link[0]['text'] = "続く追加";
			$link[0]['href'] = '?act=add_pms_sys';
			$link[1]['text'] = "一覧に戻る";
			$link[1]['href'] = '?act=list';
			write_log("送信消息,共发给了 {$s} 个会員", $_SESSION['admin_name'],3);
			adminmsg("送信成功！ {$s} 個会員に送信しました",2,$link);
			}
			else
			{
			adminmsg("送信失敗！会員名称をチェックしてください",0);
			}			
		}
	}
}
elseif($act =='pms_sys_del')
{
	check_token();
	$id=$_REQUEST['id'];
	if (empty($id)) adminmsg("項目を選択してください！",1);
	$n=del_pms_sys($id);
	if ($n)
	{
	write_log("メッセージ削除,削除行数 {$n} 行！", $_SESSION['admin_name'],3);
	adminmsg("削除成功 削除行数 {$n} 行！",2);
	}
	else
	{
	adminmsg("削除失敗！",0);
	}
}
elseif($act == 'pms_edit')
{
	get_token();
	$id=intval($_GET['id']);
	$sql = "select * from ".table('pms_sys')." where spmid='{$id}' LIMIT 1";
	$pms=$db->getone($sql);	
	$pms['starttime']=convert_datefm($pms['starttime'],1);
	$smarty->assign('pms',$pms); 	
	$smarty->display('pms/admin_pms_sys_edit.htm');
}
elseif($act == 'pms_editsave')
{
		check_token();
		$id=intval($_POST['id']);	
		$setsqlarr['spms_usertype']=intval($_POST['spms_usertype']);
		$setsqlarr['spms_type']=intval($_POST['spms_type']);
		$setsqlarr['message']=trim($_POST['message']);
		$link[0]['text'] = "続く追加";
		$link[0]['href'] = '?act=add_pms_sys';
		$link[1]['text'] = "変更結果閲覧";
		$link[1]['href'] = "?act=pms_edit&id=".$id;
		write_log("idを次に変更".$id."のメッセージ", $_SESSION['admin_name'],3);
		!$db->updatetable(table('pms_sys'),$setsqlarr," spmid=".$id."")?adminmsg("変更失敗！",0):adminmsg("変更成功！",2,$link);
}
?>
