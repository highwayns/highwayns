<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_templates_fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
$smarty->assign('pageheader',"テンプレート設定");
$smarty->assign('act',$act);
if($act == 'list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"tpl_show");
	$dirs = getsubdirs('../templates');
	unset($dirs[array_search("tpl_company",$dirs)]);
	unset($dirs[array_search("tpl_resume",$dirs)]);
	unset($dirs[array_search("tpl_hunter",$dirs)]);
	unset($dirs[array_search("tpl_train",$dirs)]);
	$list=array();
		foreach ($dirs as $k=> $val)
		{
		$list[$k]['dir']=$val;
		$list[$k]['info']=get_templates_info("../templates/".$val."/info.txt");
		}
	$smarty->assign('list',$list);
	$templates['dir']=substr($_CFG['template_dir'],0,-1);
	$templates['info']=get_templates_info("../templates/".$templates['dir']."/info.txt");
	$smarty->assign('templates',$templates);
	$smarty->assign('navlabel',"list");	
	$smarty->display('tpl/admin_templates_list.htm');
}
elseif ($act == 'backup')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"tpl_backup");
	require_once(ADMIN_ROOT_PATH.'include/admin_phpzip.php');
	$tpl = trim($_REQUEST['tpl_name']);
	if (dirname($tpl)<>'.')
	{
	adminmsg("操作失敗！",0);
	}
	$filename = '../temp/backup_templates/' . $tpl . '_' . date('Ymd') . '.zip';
	$zip = new PHPZip;
	$done = $zip->zip('../templates/' . $tpl . '/', $filename);
		if ($done)
		{
		header("Location:".$filename."");
		}
		else
		{
		adminmsg("操作失敗！",0);
		}
	}
elseif ($act == 'set')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"tpl_edit");
	$templates_info=get_templates_info("../templates/".trim($_REQUEST['tpl_dir'])."/info.txt");
	$tpl_dir = trim($_REQUEST['tpl_dir'])."/";
	!$db->query("UPDATE ".table('config')." SET value='{$tpl_dir}' WHERE name='template_dir'")?adminmsg('设置失敗',1):"";
	refresh_cache("config");
		$dir="../temp/templates_c/".$tpl_dir;
		if (!file_exists($dir)) mkdir($dir);
		$dir="../temp/caches/".$tpl_dir;
		if (!file_exists($dir)) mkdir($dir);
	$link[0]['text'] = "テンプレート一覧に戻る";
	$link[0]['href'] ="?act=list";
	adminmsg('保存成功', 2,$link);
}
elseif ($act == 'edit')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"tpl_edit");
	$tpl_dir = trim($_REQUEST['tpl_dir'])?trim($_REQUEST['tpl_dir']):substr($_CFG['template_dir'],0,-1);
	$tpl = '';
		$dir ='../templates/'.$tpl_dir;
		if($handle = @opendir($dir))
		{
			$i = 0;
			while(false !== ($file = @readdir($handle)))
			{
				if(substr($file,-4) == '.htm'  && $file != '.' && $file != '..')
				{
					$list[$i]['name'] = $file;
					$list[$i]['modify_time'] = date('Y-m-d H:i:s',filemtime($dir.'/'.$file));
					$list[$i]['size'] = filesize($dir.'/'.$file);
					$i++;
				}
			}
			array_multisort($list);
		}
		else
		{
		adminmsg('テンプレートフォルダーエラー，権限チェックしてください', 0);
		}
	$smarty->assign('list',$list);
	$templates['dir']=$tpl_dir;
	$templates['info']=get_templates_info("../templates/".$templates['dir']."/info.txt");
	$smarty->assign('templates',$templates);
	$smarty->assign('navlabel',"edit");	
	$smarty->display('tpl/admin_templates_file_list.htm');
}
elseif ($act == 'edit_file')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"tpl_edit");
	$file = $_GET['tpl_name'];
	$file_dir='../templates/'.$_GET['tpl_dir'].'/'.$file;
	if (substr($file_dir,-4)==".php") exit(adminmsg('テンプレートファイル開く失敗', 0));
	if(!$handle = @fopen($file_dir, 'rb')){
	adminmsg('テンプレートファイル開く失敗', 0);
	}
	$tpl['content'] = fread($handle, filesize($file_dir));
	$tpl['content'] = htmlentities($tpl['content'], ENT_QUOTES, HIGHWAY_CHARSET);
	fclose($handle);
	$tpl['name'] = $file;
	$tpl['dir'] = $_GET['tpl_dir'];
	$smarty->assign('tpl',$tpl);
	$smarty->display('tpl/admin_templates_file_edit.htm');
}
elseif ($act == 'do_edit')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"tpl_edit");
	$tpl_name = !empty($_POST['tpl_name']) ? trim($_POST['tpl_name']) : '';
	$tpl_content = !empty($_POST['tpl_content']) ? deep_stripslashes($_POST['tpl_content']) : '';
		if(empty($tpl_name)){
	adminmsg('テンプレートファイル保存エラー', 0);
		}
		$temp_arr = explode(".", $tpl_name);
		$file_ext = array_pop($temp_arr);
		$file_ext = trim($file_ext);
		$file_ext = strtolower($file_ext);
		$tpl_type=array("htm","html");
		if (!in_array($file_ext,$tpl_type))
		{
		exit("err");
		}		
		$file_dir='../templates/'.$_POST['tpl_dir'].'/'.$tpl_name;
		if(!$handle = @fopen($file_dir, 'wb')){
		adminmsg("テンプレート $tpl_name 開く失敗，フォルダー権限をチェックしてください",0);
		}
		if(fwrite($handle, $tpl_content) === false){
			adminmsg('目標 $tpl_name に書き失敗しました。権限チェックしてください',0);
		}
		fclose($handle);
		$link[0]['text'] = "続くファイル編集";
		$link[0]['href'] =$_SERVER['HTTP_REFERER'];
		$link[1]['text'] = "テンプレートファイル一覧へ戻る";
		$link[1]['href'] ="?act=edit&tpl_dir=".$_POST['tpl_dir'];
		adminmsg('テンプレート編集成功',2,$link);
}
elseif ($act == 'com_tpl')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"tpl_company");
	$smarty->assign('pageheader',"テンプレート設定");	
	$smarty->assign('navlabel',"com_tpl");	
	$smarty->assign('list',get_user_tpl(1,"tpl_company"));
	$smarty->display('tpl/admin_com_tpl_list.htm');
}
elseif ($act == 'com_tpl_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"tpl_company");
	$tpl_company=trim($_POST['tpl_company']);
	!$db->query("UPDATE ".table('config')." SET value='{$tpl_company}' WHERE name='tpl_company'")?adminmsg('設定更新失敗', 1):"";
	refresh_cache('config');
	$tpl_id=$_POST['tpl_id'];
	if (is_array($_POST['tpl_id']) && count($_POST['tpl_id'])>0)
	{
		for ($i =0; $i <count($_POST['tpl_id']);$i++){
				$setsqlarr['tpl_name']=trim($_POST['tpl_name'][$i]);
				$setsqlarr['tpl_display']=intval($_POST['tpl_display'][$i]);
				$setsqlarr['tpl_val']=intval($_POST['tpl_val'][$i]);
				!$db->updatetable(table('tpl'),$setsqlarr," tpl_id=".intval($_POST['tpl_id'][$i]))?adminmsg("保存失敗！",0):"";
		}

	}
	adminmsg("保存成功！",2);

}
elseif ($act == 'refresh_tpl')
{
	check_token();
	$type=intval($_GET['type']);
	$tpl_dir=trim($_GET['tpl_dir']);
	$tab_dir=get_user_tpl_dir($type);
	$dirs = getsubdirs('../templates/'.$tpl_dir);
	foreach ($dirs as $str)
	{
			if (!in_array($str,$tab_dir))
			{
			$info=get_templates_info("../templates/".$tpl_dir."/".$str."/info.txt");
			$db->query("INSERT INTO ".table('tpl')." (tpl_name,tpl_dir,tpl_type) VALUES ('{$info['name']}','{$str}','$type')");
			}
			$dararray[]=" tpl_dir!='{$str}' ";
	}
	if (!empty($dararray))
	{
	$db->query("Delete from ".table('tpl')." WHERE  ".implode(" and ",$dararray)." AND  tpl_type='$type'");
	}
	adminmsg('更新成功',2);
}
elseif ($act == 'resume_tpl')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"tpl_resume");
	$smarty->assign('pageheader',"テンプレート設定");	
	$smarty->assign('navlabel',"resume_tpl");
	$smarty->assign('list',get_user_tpl(2,"tpl_resume"));
	$smarty->display('tpl/admin_resume_tpl_list.htm');
}
elseif ($act == 'resume_tpl_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"tpl_resume");
	$tpl_personal=trim($_POST['tpl_personal']);
	!$db->query("UPDATE ".table('config')." SET value='{$tpl_personal}' WHERE name='tpl_personal'")?adminmsg('設定更新失敗', 1):"";
	refresh_cache('config');
	$tpl_id=$_POST['tpl_id'];
	if (is_array($_POST['tpl_id']) && count($_POST['tpl_id'])>0)
	{
		for ($i =0; $i <count($_POST['tpl_id']);$i++){
				$setsqlarr['tpl_name']=trim($_POST['tpl_name'][$i]);
				$setsqlarr['tpl_display']=intval($_POST['tpl_display'][$i]);
				$setsqlarr['tpl_val']=intval($_POST['tpl_val'][$i]);
				!$db->updatetable(table('tpl'),$setsqlarr," tpl_id=".intval($_POST['tpl_id'][$i]))?adminmsg("保存失敗！",0):"";

		}

	}
	adminmsg("保存成功！",2);

}
?>
