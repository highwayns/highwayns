<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_database.fun.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'backup';
$backup_dir="backup";
$smarty->assign('act',$act);
//备份
if($act == 'backup')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"database");
	$pre = str_replace('_', '\_', $pre);
	$smarty->assign('list',$db->getall("SHOW TABLES LIKE '$pre%'", MYSQL_NUM));
	$smarty->assign('pageheader',"DB");
	$smarty->assign('navlabel',"backup");
	$smarty->display('database/admin_database_backup.htm');
}
//执行备份
elseif($act =='do_backup')
{
	check_permissions($_SESSION['admin_purview'],"database");
	if (!file_exists("../data/".$backup_dir."/"))adminmsg("バックアップファイル保存フォルダーdata/".$backup_dir."が存在しません！",0);
	if (!is_writable("../data/".$backup_dir."/"))adminmsg("バックアップファイル保存フォルダーdata/".$backup_dir."書けない！",0);
	$limit_size = !empty($_REQUEST['limit_size']) ? intval($_REQUEST['limit_size']) : '2048'; 
	$mysql_type = !empty($_REQUEST['mysql_type']) ? trim($_REQUEST['mysql_type']) : '';
	$table_id = !empty($_REQUEST['table_id']) ? intval($_REQUEST['table_id']) : 0;
	$file = !empty($_GET['file']) ? trim($_GET['file']) : date("Ymd_", time()) . get_rand_char(5).uniqid();
	$num = !empty($_GET['num']) ? intval($_GET['num']) : 1;
	$pos = !empty($_GET['pos']) ? intval($_GET['pos']) : 0;
		if (!empty($_POST['tables']))
		{
		$tables = $_POST['tables'];
		 @file_put_contents("../data/{$backup_dir}/temp.txt", serialize($_POST['tables']));
		}
		elseif ($_GET['table_id'])
		{
		$content = file_get_contents("../data/{$backup_dir}/temp.txt");
		$tables = unserialize($content);
		}
		else
		{
		adminmsg("バックアップテーブルを選択してください！",1);
		}
	$db_version = $db->dbversion();
	$sql = '';
	$version = HIGHWAY_VERSION;
	$add_time = date("Y-m-d H:i:s");
	$sql .= "-- 74CMS VERSION:{$version}\r\n".
	"-- Mysql VERSION:{$db_version}\r\n".
	"-- Create time:{$add_time}\r\n";
	$count = count($tables);
	for($i = $table_id; $i < $count; $i++)
	{
		$table = $tables[$i];
		if ($pos == 0)
		{
		$table_sql = write_head($table);
		$table_sql = preg_replace('/AUTO_INCREMENT=([0-9]+)(\s+)/', '', $table_sql);
		$table_sql1 = substr($table_sql, 0, strrpos($table_sql, ')', 25)+1);
			if ($mysql_type == 'mysql40' && $db_version > 4.0)
			{
			$s = "TYPE=MyISAM;\r\n";
			$table_sql = $table_sql1 . $s;
			}
			elseif($mysql_type == 'mysql41' && $db_version < 4.1)
			{
			$s = "ENGINE=MyISAM DEFAULT CHARSET=".HIGHWAY_CHARSET.";\r\n";
			$table_sql = $table_sql1 . $s;
			}
			else
			{
			$table_sql .= ";\r\n";
			}
	}
	$result = $db->query("SELECT * FROM " . $table);
	$field_num = $db->num_fields($result);
	$row_count = $db->getfirst("SELECT COUNT(*) FROM " . $table);
	$j = 0;
	while ($row = $db->fetch_array($result, MYSQL_NUM))
	{
		if ($j < $pos)
		{
		 $j++;
		continue;
		}
			$table_sql .= "INSERT INTO `".$table."` VALUES (";
			for($m=0;$m<$field_num;$m++)
			{
			$table_sql .= "'" .escape_str($row[$m]) . "',";
			}
			$table_sql = substr($table_sql,0,-1).");\r\n";				
		if (strlen($sql.$table_sql) >= $limit_size * 1000)
		{
			if (!write_file("../data/{$backup_dir}/{$file}_{$num}.sql", $sql))
			{
			adminmsg('DBバックアップ-'.$num.'失敗',0);
			}
			if ($j == $row_count-1)
			{
			$i++;
			}
			$link[0]['text'] = "システム自動続く...";
			$link[0]['href'] = "admin_database.php?act=do_backup&limit_size={$limit_size}&mysql_type={$mysql_type}&file={$file}&num=".($num+1)."&table_id={$i}&pos=".$j;
			adminmsg('ファイル'.$file.'_'.$num.'.sql バックアップ成功。システム継続する...',1,$link,true,1);
			exit();
		}else{
			$sql .= $table_sql;
			$table_sql = '';
		}
		$j++;
	}
	$pos = 0;
	if (strlen($sql.$table_sql) >= $limit_size * 1000)
	{
		if (!write_file("../data/{$backup_dir}/{$file}_{$num}.sql", $sql))
		{
		adminmsg("DBバックアップ-{$num}失敗",0);
		}
		$link[0]['text'] = "プログラム自動実行...";
		$link[0]['href'] = "admin_database.php?act=do_backup&limit_size=".$limit_size."&mysql_type=".$mysql_type."&file=".$file."&num=".($num+1)."&table_id=".($i+1);
		adminmsg('ファイル' . $file . '_' . $num.'.sql バックアップ成功。継続...', 1,$link,true,2);
		exit();
	}
	elseif ($i == $count-1)
	{
		if (!write_file("../data/{$backup_dir}/{$file}_{$num}.sql", $sql))
		{
		adminmsg('ＤＢバックアップ-{$num}失敗');
		}
		@unlink("../data/{$backup_dir}/temp.txt");
		$link[0]['text'] = "バックアップファイル閲覧";
		$link[0]['href'] = "?act=restore";
		write_log("DBバックアップ成功", $_SESSION['admin_name'],3);
		adminmsg('DBバックアップ成功',2,$link);
		}
		elseif ($j == 0)
		{
		$sql .= $table_sql;
		}
	}
}
elseif($act =='restore')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"database");
	if (!file_exists("../data/{$backup_dir}/"))adminmsg("バックアップファイル保存フォルダーdata/backupが存在しません！",0);
	$data_backup_list = $file_info = array();
	$dir = opendir('../data/'.$backup_dir);
		while($file = readdir($dir))
		{
			if(strpos($file,'.sql')!==false && !in_array(preg_replace('#_\d+\.sql#i','',$file).'.sql', $data_backup_list))
			{
			$data_backup_list[] = preg_replace('#_\d+\.sql#i','',$file).'.sql';
			}
		}
	foreach($data_backup_list as $key => $file)
	{
			if (file_exists("../data/".$backup_dir."/".$file))
			{
				$sqlfile_info_arr = get_sqlfile_info("../data/".$backup_dir."/".$file);
				$file_info[$key]['file_size'] = round(filesize("../data/".$backup_dir."/".$file)/1024/1024,2);
			}
			else
			{
				$sqlfile_info_arr = get_sqlfile_info("../data/".$backup_dir."/".preg_replace('#\.sql#i','',$file).'_1.sql');
				$i = 1;
					while (file_exists("../data/".$backup_dir."/".preg_replace('#\.sql#i','',$file).'_'.$i.'.sql'))
					{
					$file_info[$key]['file_size'] += round(filesize("../data/{$backup_dir}/".preg_replace('#\.sql#i','',$file)."_{$i}.sql")/1024/1024,2);
					$i++;
					}
			}
		$file_info[$key]['file_name'] = substr($file,0);
		$file_info[$key]['74cms_ver'] = $sqlfile_info_arr['74cms_ver'];
		$file_info[$key]['mysql_ver'] = $sqlfile_info_arr['mysql_ver'];
		$file_info[$key]['add_time'] = $sqlfile_info_arr['add_time'];
	}
	$smarty->assign('pageheader',"DB");
	$smarty->assign('navlabel',"restore");
	$smarty->assign('list',array_reverse($file_info));
	$smarty->display('database/admin_database_restore.htm');
}
elseif($act =='del')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"database");
	$file_name = !empty($_REQUEST['file_name']) ? $_REQUEST['file_name'] : adminmsg('項目を選択してください。',0);
	if (!is_array($file_name)) $file_name=array($file_name);
	foreach ($file_name as $fname)
	{
		$fname = substr($fname, 0, -4);
		if ($handle = opendir("../data/".$backup_dir))
		{
			while (false !== $file = readdir($handle))
			{
				if (strpos($file, $fname) !== false && $file != 'index.htm' && $file != '.' && $file != '..')
				{
				$sql_file[] = $file;
				}
			}
		}
		foreach ($sql_file as $val)
		{
		@unlink("../data/".$backup_dir."/" . $val);
		}	
		unset($sql_file,$file);
	}
	write_log("バックアップファイル削除成功", $_SESSION['admin_name'],3);
	adminmsg('バックアップファイル削除成功',2);
}
elseif($act =='import')
{
		check_permissions($_SESSION['admin_purview'],"database");
		$file_name = !empty($_GET['file_name']) ? trim($_GET['file_name']) : '';
		$file_name = substr($file_name, 0, -4);
		if ($handle = opendir("../data/".$backup_dir))
		{
			while (false !== $file = readdir($handle))
			{
				if (strpos($file, $file_name) !== false && $file != 'index.htm' && $file != '.' && $file != '..')
				{
				$backup_file[] = $file;
				}
			}
			if (is_array($backup_file) && !empty($backup_file))
			{
				natsort($backup_file);
				foreach($backup_file as $v)
				{
				$backup_arr[]=$v;
				}
				$backup_file=$backup_arr;
			}
		}
		else
		{
			adminmsg('バックアップファイルが存在しません!',0);
		}
		closedir("../data/".$backup_dir);
		$file ="../data/{$backup_dir}/{$backup_file[0]}";
		$file_info = get_sqlfile_info($file);
		if($file_info['74cms_ver'] != HIGHWAY_VERSION)
		{
		adminmsg('海威人材プログラムとバックアップ一致しません');
		}
		$_SESSION['backup_file']=$backup_file;
		$filekey=intval($_GET['filekey']);
		$backup_file=$_SESSION['backup_file'][$filekey];
		if (empty($backup_file))
		{
			$link[0]['text'] = "バックアップファイル閲覧";
			$link[0]['href'] = "?act=restore";
			unset($_SESSION['backup_file']);
			write_log("DB回復成功", $_SESSION['admin_name'],3);
			adminmsg('DB回復成功',2,$link);
		}
		else
		{
			$file = "../data/{$backup_dir}/{$backup_file}";
			$file = array_filter(file($file), 'remove_comment');
			$file = str_replace("\r", "\n", implode('', $file));
			$arr = explode(";\n", trim($file));
			$arr_count = count($arr);
			for($i = 0; $i < $arr_count; $i++)
			{
				$arr[$i] = trim($arr[$i]);
				if (!empty($arr[$i]))
				{
						if ((strpos($arr[$i], 'CREATE TABLE') !== false) && (strpos($arr[$i], 'DEFAULT CHARSET='.str_replace('-', '', HIGHWAY_CHARSET) )!== false))
						{
						$arr[$i] = str_replace('DEFAULT CHARSET='. str_replace('-', '', HIGHWAY_CHARSET), '', $arr[$i]);
						}
					!$db->query($arr[$i])?adminmsg('回復失敗',0):"";
				}
			}			
			$link[0]['text'] = "システム自動続く...";
			$link[0]['href'] = "?act=import&file_name={$_GET['file_name']}&filekey=".($filekey+1);
			adminmsg(" ファイル({$backup_file}) 回復成功，次ファイルシステム自動回復...",1,$link,true,2);
		}
}
elseif($act == 'optimize')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"database");
	$smarty->assign('list',get_optimize_list());
	$smarty->assign('pageheader',"DB");
	$smarty->assign('navlabel',"optimize");
	$smarty->display('database/admin_database_optimize.htm');
}
elseif($act == 'optimize_table')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"database");
	$tablename=$_POST['tablename'];
	if (empty($tablename))
	{
	adminmsg('項目を選択してください',0);
	}
	if (is_array($tablename))
	{
		$sqlstr=implode(",",$tablename);
		if ($db->query("OPTIMIZE TABLE $sqlstr"))
		{	
			write_log("DB最適切化成功", $_SESSION['admin_name'],3);
			adminmsg('最適切化成功!',2);
		}
	}
}
?>
