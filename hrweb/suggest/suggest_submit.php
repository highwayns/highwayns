<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
if($_SESSION['input_token']!="" && $_SESSION['input_token']==$_POST['input_token']){
	$setsqlarr['infotype']=intval($_POST['infotype'])>0?intval($_POST['infotype']):showmsg("请选择类型！",1);
	$setsqlarr['feedback']=trim($_POST['feedback'])?trim($_POST['feedback']):showmsg("请填写内容！",1);
	$setsqlarr['tel']=trim($_POST['tel'])?trim($_POST['tel']):showmsg("请填写联系方式！",1);
	$setsqlarr['addtime']=time();
	!$db->inserttable(table('feedback'),$setsqlarr)?showmsg("添加失败！",0):showmsg("添加成功，感谢您对本站的支持！",2);
}else{
	showmsg("非法操作！",1);
}
?>
