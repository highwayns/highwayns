<?php
 /*
 * 74cms 安装向导
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
define('QISHI_PRE','qs_');
define('QISHI_CHARSET', 'gb2312');
define('QISHI_DBCHARSET', 'GBK');
require_once(dirname(__FILE__) . '/include/common.inc.php');
require_once(QISHI_ROOT_PATH . 'include/74cms_version.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : '1';
if(file_exists(QISHI_ROOT_PATH.'data/install.lock')&&$act!='5')
{
exit('您已经安装过本系统，如果想重新安装，请删除data目录下install.lock文件');
}
if($act =="1")
{
	$install_smarty->assign("act", $act);
	$install_smarty->display('step1.htm');
}
if($act =="2")
{
	$system_info = array();
	$system_info['version'] = QISHI_VERSION;
	$system_info['os'] = PHP_OS;
	$system_info['ip'] = $_SERVER['SERVER_ADDR'];
	$system_info['web_server'] = $_SERVER['SERVER_SOFTWARE'];
	$system_info['php_ver'] = PHP_VERSION;
	$system_info['max_filesize'] = ini_get('upload_max_filesize');
	if (PHP_VERSION<5.0) exit("安装失败，请使用PHP5.0及以上版本");
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
 	$pre  = isset($_POST['pre']) ? trim($_POST['pre']) : 'qs_';
 	$admin_name = isset($_POST['admin_name']) ? trim($_POST['admin_name']) : '';
    $admin_pwd = isset($_POST['admin_pwd']) ? trim($_POST['admin_pwd']) : '';
    $admin_pwd1 = isset($_POST['admin_pwd1']) ? trim($_POST['admin_pwd1']) : '';
    $admin_email = isset($_POST['admin_email']) ? trim($_POST['admin_email']) : '';
	if($dbhost == '' || $dbname == ''|| $dbuser == ''|| $admin_name == ''|| $admin_pwd == '' || $admin_pwd1 == '' || $admin_email == '')
	{
		install_showmsg('您填写的信息不完整，请核对');
	}
	if($admin_pwd != $admin_pwd1)
	{
		install_showmsg('您两次输入的密码不一致');
	}
	if (!preg_match("/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/",$admin_email))
	{
		install_showmsg('电子邮箱格式错误！');
	}
	if(!$db = @mysql_connect($dbhost, $dbuser, $dbpass))
	{
		install_showmsg('连接数据库错误，请核对信息是否正确');
	}
	if (mysql_get_server_info()<5.0) exit("安装失败，请使用mysql5以上版本");
	if (mysql_get_server_info() > '4.1')
	{
		mysql_query("CREATE DATABASE IF NOT EXISTS `{$dbname}` DEFAULT CHARACTER SET ".QISHI_DBCHARSET, $db);
	}
	else
	{
		mysql_query("CREATE DATABASE IF NOT EXISTS `{$dbname}`", $db);
	}
	mysql_query("CREATE DATABASE IF NOT EXISTS `{$dbname}`;",$db);
	if(!mysql_select_db($dbname))
	{
		install_showmsg('选择数据库错误，请检查是否拥有权限或存在此数据库');
	}
	mysql_query("SET NAMES '".QISHI_DBCHARSET."',character_set_client=binary,sql_mode='';",$db);
	ob_end_clean();
	$html ="";
	$html.= "<script type=\"text/javascript\">\n";
	$html.= "$('#installing').append('<p>数据库创建成功！...</p>');\n";
	$html.= "var div = document.getElementById('installing');";
	$html.= "div.scrollTop = div.scrollHeight;";
	$html.= "</script>";
	echo $html;
	ob_flush();
	flush();
	$mysql_version = mysql_get_server_info($db);
	$site_dir=substr(dirname($php_self), 0, -7)?substr(dirname($php_self), 0, -7):'/';
	$QS_pwdhash=randstr(16);
	$content = '<?'."php\n";
    $content .= "\$dbhost   = \"{$dbhost}\";\n\n";
    $content .= "\$dbname   = \"{$dbname}\";\n\n";
    $content .= "\$dbuser   = \"{$dbuser}\";\n\n";
    $content .= "\$dbpass   = \"{$dbpass}\";\n\n";
    $content .= "\$pre    = \"{$pre}\";\n\n";
	$content .= "\$QS_cookiedomain = '';\n\n";
	$content .= "\$QS_cookiepath =  \"{$site_dir}\";\n\n";
	$content .= "\$QS_pwdhash = \"{$QS_pwdhash}\";\n\n";
	$content .= "define('QISHI_CHARSET','".QISHI_CHARSET."');\n\n";
	$content .= "define('QISHI_DBCHARSET','".QISHI_DBCHARSET."');\n\n";
    $content .= '?>';
	$fp = @fopen(QISHI_ROOT_PATH . 'data/config.php', 'wb+');
	if (!$fp)
	{
		install_showmsg('打开配置文件失败');
	}
	if (!@fwrite($fp, trim($content)))
	{
		install_showmsg('写入配置文件失败');
	}
	@fclose($fp);
	$site_domain = "http://".$_SERVER['HTTP_HOST'];
	$site_dir=substr(dirname($php_self), 0, -7)?substr(dirname($php_self), 0, -7):'/';
	if(is_writable(QISHI_ROOT_PATH.'data/'))
	{
		$fp = @fopen(QISHI_ROOT_PATH.'data/install.lock', 'wb+');
		fwrite($fp, 'OK');
		fclose($fp);
	}
	$install_smarty->assign("act", $act);
	$install_smarty->assign("domain", $site_domain);
	$install_smarty->assign("domaindir", $site_domain.$site_dir);
	$install_smarty->assign("v", QISHI_VERSION);
	$install_smarty->assign("t", 2);
	$install_smarty->assign("email", $admin_email);
	$install_smarty->display('step4.htm');
  	if(!$fp = @fopen(dirname(__FILE__).'/sql-structure.sql','rb'))
	{
		install_showmsg('打开文件sql-structure.sql出错，请检查文件是否存在');
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
				$query = str_replace(QISHI_PRE,$pre,$query);
				if ( $mysql_version >= 4.1 )
				{
					mysql_query(str_replace("TYPE=MyISAM", "ENGINE=MyISAM  DEFAULT CHARSET=".QISHI_DBCHARSET,  $query), $db);
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
		install_showmsg('打开文件sql-data.sql出错，请检查文件是否存在');
	}
	while(!feof($fp))
	{
		 $line = rtrim(fgets($fp,1024));
		 if(ereg(";$",$line))
		 {
		 	$query .= $line;
			$query = str_replace(QISHI_PRE,$pre,$query);
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
	$html.= "$('#installing').append('<p>基础数据添加成功！...</p>');\n";
	$html.= "var div = document.getElementById('installing');";
	$html.= "div.scrollTop = div.scrollHeight;";
	$html.= "</script>";
	echo $html;
	ob_flush();
	flush();
	$query = '';
	if(!$fp = @fopen(dirname(__FILE__).'/sql-hrtools.sql','rb'))
	{
		install_showmsg('打开文件sql-hrtools.sql出错，请检查文件是否存在');
	}
	while(!feof($fp))
	{
		 $line = rtrim(fgets($fp,1024));
		 if(ereg(";$",$line))
		 {
		 	$query .= $line;
			$query = str_replace(QISHI_PRE,$pre,$query);
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
	$html.= "$('#installing').append('<p>hr工具箱数据添加成功！...</p>');\n";
	$html.= "var div = document.getElementById('installing');";
	$html.= "div.scrollTop = div.scrollHeight;";
	$html.= "</script>";
	echo $html;
	ob_flush();
	flush();	
	$query = '';
	if(!$fp = @fopen(dirname(__FILE__).'/sql-hotword.sql','rb'))
	{
		install_showmsg('打开文件sql-hotword.sql出错，请检查文件是否存在');
	}
	while(!feof($fp))
	{
		 $line = rtrim(fgets($fp,1024));
		 if(ereg(";$",$line))
		 {
		 	$query .= $line;
			$query = str_replace(QISHI_PRE,$pre,$query);
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
	$html.= "$('#installing').append('<p>热门关键词数据添加成功！...</p>');\n";
	$html.= "var div = document.getElementById('installing');";
	$html.= "div.scrollTop = div.scrollHeight;";
	$html.= "</script>";
	echo $html;
	ob_flush();
	flush();	
	mysql_query("UPDATE `{$pre}config` SET value = '{$site_dir}' WHERE name = 'site_dir'", $db);
	mysql_query("UPDATE `{$pre}config` SET value = '{$site_domain}' WHERE name = 'site_domain'", $db);
	$pwd_hash=randstr();
	$admin_md5pwd=md5($admin_pwd.$pwd_hash.$QS_pwdhash);
	mysql_query("INSERT INTO `{$pre}admin` (admin_id,admin_name, email, pwd,pwd_hash, purview, rank,add_time, last_login_time, last_login_ip) VALUES (1, '$admin_name', '$admin_email', '$admin_md5pwd', '$pwd_hash', 'all','超级管理员', '$timestamp', '$timestamp', '')",$db);
	//生成静态缓存
	require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
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
	$html.= "$('#installing').append('<p>缓存数据添加成功！...</p><p>安装完成！</p>');\n";
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