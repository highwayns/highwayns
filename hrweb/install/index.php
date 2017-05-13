<?php
define('IN_HIGHWAY', true);
define('HIGHWAY_PRE','hw_');
define('HIGHWAY_CHARSET', 'utf-8');
define('HIGHWAY_DBCHARSET', 'utf8');
require_once(dirname(__FILE__) . '/include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH . 'include/highwayns_version.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : '1';
if(file_exists(HIGHWAY_ROOT_PATH.'data/install.lock')&&$act!='5')
{
exit('システムすでにインストールしました，再インストールには，dataフォルダーにのinstall.lockファイルを削除してください。');
}
if($act =="1")
{
	$install_smarty->assign("act", $act);
	$install_smarty->display('step1.htm');
}
if($act =="2")
{
	$system_info = array();
	$system_info['version'] = HIGHWAY_VERSION;
	$system_info['os'] = PHP_OS;
	$system_info['ip'] = $_SERVER['SERVER_ADDR'];
	$system_info['web_server'] = $_SERVER['SERVER_SOFTWARE'];
	$system_info['php_ver'] = PHP_VERSION;
	$system_info['max_filesize'] = ini_get('upload_max_filesize');
	if (PHP_VERSION<5.0) exit("インストール失敗，PHP5.0及び以上のバージョン一を使ってください");
	$dir_check = check_dirs($need_check_dirs);
	$install_smarty->assign("dir_check", $dir_check);
	$install_smarty->assign("system_info", $system_info);
	$install_smarty->assign("act", $act);
	$install_smarty->display('step2.htm');
}
if($act =="3")
{
	$install_smarty->assign("act", $act);
	$install_smarty->display('step3.htm');
}
if($act =="4")
{
 	$dbhost = isset($_POST['dbhost']) ? trim($_POST['dbhost']) : '';
 	$dbname = isset($_POST['dbname']) ? trim($_POST['dbname']) : '';
 	$dbuser = isset($_POST['dbuser']) ? trim($_POST['dbuser']) : '';
 	$dbpass = isset($_POST['dbpass']) ? trim($_POST['dbpass']) : '';
 	$pre  = isset($_POST['pre']) ? trim($_POST['pre']) : 'hw_';
 	$admin_name = isset($_POST['admin_name']) ? trim($_POST['admin_name']) : '';
    $admin_pwd = isset($_POST['admin_pwd']) ? trim($_POST['admin_pwd']) : '';
    $admin_pwd1 = isset($_POST['admin_pwd1']) ? trim($_POST['admin_pwd1']) : '';
    $admin_email = isset($_POST['admin_email']) ? trim($_POST['admin_email']) : '';
	if($dbhost == '' || $dbname == ''|| $dbuser == ''|| $admin_name == ''|| $admin_pwd == '' || $admin_pwd1 == '' || $admin_email == '')
	{
		install_showmsg('未入力情報があります、チェックしてください');
	}
	if($admin_pwd != $admin_pwd1)
	{
		install_showmsg('パスワードが一致しません');
	}
	if (!preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$admin_email))
	{
		install_showmsg('メールアドレスエラー！');
	}
	if(!$db = @mysql_connect($dbhost, $dbuser, $dbpass))
	{
		install_showmsg('DB接続エアー，接続情報を確認してください');
	}
	if (mysql_get_server_info()<5.0) exit("インストール失敗，mysql5以上のバージョンを使ってください");
	if (mysql_get_server_info() > '4.1')
	{
		mysql_query("CREATE DATABASE IF NOT EXISTS `{$dbname}` DEFAULT CHARACTER SET ".HIGHWAY_DBCHARSET, $db);
	}
	else
	{
		mysql_query("CREATE DATABASE IF NOT EXISTS `{$dbname}`", $db);
	}
	mysql_query("CREATE DATABASE IF NOT EXISTS `{$dbname}`;",$db);
	if(!mysql_select_db($dbname))
	{
		install_showmsg('DB選択エラー、DBの存在と権限をチェックしてください');
	}
	mysql_query("SET NAMES '".HIGHWAY_DBCHARSET."',character_set_client=binary,sql_mode='';",$db);
	ob_end_clean();
	$html ="";
	$html.= "<script type=\"text/javascript\">\n";
	$html.= "$('#installing').append('<p>データベース作成成功！...</p>');\n";
	$html.= "var div = document.getElementById('installing');";
	$html.= "div.scrollTop = div.scrollHeight;";
	$html.= "</script>";
	echo $html;
	ob_flush();
	flush();
	$mysql_version = mysql_get_server_info($db);
	$site_dir=substr(dirname($php_self), 0, -7)?substr(dirname($php_self), 0, -7):'/';
	$HW_pwdhash=randstr(16);
	$content = '<?'."php\n";
    $content .= "\$dbhost   = \"{$dbhost}\";\n\n";
    $content .= "\$dbname   = \"{$dbname}\";\n\n";
    $content .= "\$dbuser   = \"{$dbuser}\";\n\n";
    $content .= "\$dbpass   = \"{$dbpass}\";\n\n";
    $content .= "\$pre    = \"{$pre}\";\n\n";
	$content .= "\$HW_cookiedomain = '';\n\n";
	$content .= "\$HW_cookiepath =  \"{$site_dir}\";\n\n";
	$content .= "\$HW_pwdhash = \"{$HW_pwdhash}\";\n\n";
	$content .= "define('HIGHWAY_CHARSET','".HIGHWAY_CHARSET."');\n\n";
	$content .= "define('HIGHWAY_DBCHARSET','".HIGHWAY_DBCHARSET."');\n\n";
    $content .= '?>';
	$fp = @fopen(HIGHWAY_ROOT_PATH . 'data/config.php', 'wb+');
	if (!$fp)
	{
		install_showmsg('設置ファイル開く失敗');
	}
	if (!@fwrite($fp, trim($content)))
	{
		install_showmsg('設定ファイル書き込む失敗');
	}
	@fclose($fp);
	$site_domain = "http://".$_SERVER['HTTP_HOST'];
	$site_dir=substr(dirname($php_self), 0, -7)?substr(dirname($php_self), 0, -7):'/';
	if(is_writable(HIGHWAY_ROOT_PATH.'data/'))
	{
		$fp = @fopen(HIGHWAY_ROOT_PATH.'data/install.lock', 'wb+');
		fwrite($fp, 'OK');
		fclose($fp);
	}
	$install_smarty->assign("act", $act);
	$install_smarty->assign("domain", $site_domain);
	$install_smarty->assign("domaindir", $site_domain.$site_dir);
	$install_smarty->assign("v", HIGHWAY_VERSION);
	$install_smarty->assign("t", 2);
	$install_smarty->assign("email", $admin_email);
	$install_smarty->display('step4.htm');
  	if(!$fp = @fopen(dirname(__FILE__).'/sql-structure.sql','rb'))
	{
		install_showmsg('ファイルsql-structure.sql開くエラー，ファイル存在チェックしてください');
	}
	$query = '';
	while(!feof($fp))
    {
		$line = rtrim(fgets($fp,1024)); 
		if(strstr($line,'||-_-||')!=false) {
			$line = ltrim(rtrim($line,"||-_-||"),"||-_-||");
			$html ="";
			$html.= "<script type=\"text/javascript\">\n";
			$html.= "$('#installing').append('<p>".$line."...</p>');\n";
			$html.= "var div = document.getElementById('installing');";
			$html.= "div.scrollTop = div.scrollHeight;";
			$html.= "</script>";
			echo $html;
			ob_flush();
			flush();
		}else{
			if(preg_match('/;$/',$line)) 
			{
				$query .= $line."\n";
				$query = str_replace(HIGHWAY_PRE,$pre,$query);
				if ( $mysql_version >= 4.1 )
				{
					mysql_query(str_replace("TYPE=InnoDB", "ENGINE=InnoDB  DEFAULT CHARSET=".HIGHWAY_DBCHARSET,  $query), $db);
				}
				else
				{
					mysql_query($query, $db);
				}
				$query='';
			 }
			 else if(!ereg('/^(//|--)/',$line))
			 {
			 	$query .= $line;
			 }
		}
	}
	@fclose($fp);	
	$query = '';
	if(!$fp = @fopen(dirname(__FILE__).'/sql-data.sql','rb'))
	{
		install_showmsg('ファイルsql-data.sql開くエラー，ファイル存在チェックしてください');
	}
	while(!feof($fp))
	{
		 $line = rtrim(fgets($fp,1024));
		 if(ereg(";$",$line))
		 {
		 	$query .= $line;
			$query = str_replace(HIGHWAY_PRE,$pre,$query);
			mysql_query($query,$db);
			$query='';
		 }
		 else if(!ereg("^(//|--)",$line))
		 {
			$query .= $line;
		 }
	}
	@fclose($fp);	
	$html ="";
	$html.= "<script type=\"text/javascript\">\n";
	$html.= "$('#installing').append('<p>基本データ追加成功！...</p>');\n";
	$html.= "var div = document.getElementById('installing');";
	$html.= "div.scrollTop = div.scrollHeight;";
	$html.= "</script>";
	echo $html;
	ob_flush();
	flush();
	$query = '';
	if(!$fp = @fopen(dirname(__FILE__).'/sql-hrtools.sql','rb'))
	{
		install_showmsg('ファイルsql-hrtools.sql開くエラー，ファイルをチェックしてください');
	}
	while(!feof($fp))
	{
		 $line = rtrim(fgets($fp,1024));
		 if(ereg(";$",$line))
		 {
		 	$query .= $line;
			$query = str_replace(HIGHWAY_PRE,$pre,$query);
			mysql_query($query,$db);
			$query='';
		 }
		 else if(!ereg("^(//|--)",$line))
		 {
			$query .= $line;
		 }
	}
	@fclose($fp);
	$html ="";
	$html.= "<script type=\"text/javascript\">\n";
	$html.= "$('#installing').append('<p>hrツールデータ追加成功！...</p>');\n";
	$html.= "var div = document.getElementById('installing');";
	$html.= "div.scrollTop = div.scrollHeight;";
	$html.= "</script>";
	echo $html;
	ob_flush();
	flush();	
	$query = '';
	if(!$fp = @fopen(dirname(__FILE__).'/sql-hotword.sql','rb'))
	{
		install_showmsg('ファイルsql-hotword.sql開くエラー，ファイルをチェックしてください');
	}
	while(!feof($fp))
	{
		 $line = rtrim(fgets($fp,1024));
		 if(ereg(";$",$line))
		 {
		 	$query .= $line;
			$query = str_replace(HIGHWAY_PRE,$pre,$query);
			mysql_query($query,$db);
			$query='';
		 }
		 else if(!ereg("^(//|--)",$line))
		 {
			$query .= $line;
		 }
	}
	@fclose($fp);
	$html ="";
	$html.= "<script type=\"text/javascript\">\n";
	$html.= "$('#installing').append('<p>ホットワード追加成功！...</p>');\n";
	$html.= "var div = document.getElementById('installing');";
	$html.= "div.scrollTop = div.scrollHeight;";
	$html.= "</script>";
	echo $html;
	ob_flush();
	flush();	
	mysql_query("UPDATE `{$pre}config` SET value = '{$site_dir}' WHERE name = 'site_dir'", $db);
	mysql_query("UPDATE `{$pre}config` SET value = '{$site_domain}' WHERE name = 'site_domain'", $db);
	$pwd_hash=randstr();
	$admin_md5pwd=md5($admin_pwd.$pwd_hash.$HW_pwdhash);
	mysql_query("INSERT INTO `{$pre}admin` (admin_id,admin_name, email, pwd,pwd_hash, purview, rank,add_time, last_login_time, last_login_ip) VALUES (1, '$admin_name', '$admin_email', '$admin_md5pwd', '$pwd_hash', 'all','管理者', '$timestamp', '$timestamp', '')",$db);
	//生成静态缓存
	require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
	$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
	unset($dbhost,$dbuser,$dbpass,$dbname);		
	refresh_cache('config');
	$_CFG=get_cache('config');	
	refresh_page_cache();
	$_PAGE=get_cache('page');	
	refresh_nav_cache();
	$_NAV=get_cache('nav');	
	refresh_category_cache();	
	refresh_cache('text');
	refresh_cache('mailconfig');
	refresh_cache('mail_templates');
	refresh_cache('locoyspider');
	refresh_cache('sms_config');
	refresh_cache('sms_templates');
	refresh_cache('captcha');
	refresh_cache('baiduxml');
	
	refresh_cache('baidu_submiturl');
	
	refresh_plug_cache();
	refresh_category_cache();
	refresh_points_rule_cache();			
	//生成分类JS
	makejs_classify();
	$html ="";
	$html.= "<script type=\"text/javascript\">\n";
	$html.= "$('#installing').append('<p>Ｃａｃｈｅデータ追加成功！...</p><p>インストール完了！</p>');\n";
	$html.= "var div = document.getElementById('installing');";
	$html.= "div.scrollTop = div.scrollHeight;";
	$html.= "</script>";
	echo $html;
	ob_flush();
	flush();
	
	$html ="";
	$html.= "<script type=\"text/javascript\">\n";
	$html.= "window.location.href='?act=5';";
	$html.= "</script>";
	echo $html;
}
if($act =="5")
{
	$install_smarty->assign("act", $act);
	$install_smarty->display('step5.htm');
}
?>
