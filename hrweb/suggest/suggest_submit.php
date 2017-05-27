<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../include/common.inc.php');
require_once(HIGHWAY_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
if($_SESSION['input_token']!="" && $_SESSION['input_token']==$_POST['input_token']){
	$setsqlarr['infotype']=intval($_POST['infotype'])>0?intval($_POST['infotype']):showmsg("タイプを選択してください！",1);
	$setsqlarr['feedback']=trim($_POST['feedback'])?trim($_POST['feedback']):showmsg("内容を入力してください！",1);
	$setsqlarr['tel']=trim($_POST['tel'])?trim($_POST['tel']):showmsg("連絡先を入力してください！",1);
	$setsqlarr['addtime']=time();
	!$db->inserttable(table('feedback'),$setsqlarr)?showmsg("追加失敗！",0):showmsg("追加成功，有難うございます！",2);
}else{
	showmsg("違法操作！",1);
}
?>
