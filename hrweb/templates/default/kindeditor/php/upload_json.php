<?php
/**
 * KindEditor PHP
 *
 * 鏈琍HP绋嬪簭鏄紨绀虹▼搴忥紝寤鸿涓嶈鐩存帴鍦ㄥ疄闄呴」鐩腑浣跨敤銆? * 濡傛灉鎮ㄧ‘瀹氱洿鎺ヤ娇鐢ㄦ湰绋嬪簭锛屼娇鐢ㄤ箣鍓嶈浠旂粏纭鐩稿叧瀹夊叏璁剧疆銆? *
 */
define('IN_QISHI', true);

require_once('../../../data/config.php');
require_once('../../include/admin_common.inc.php');
require_once 'JSON.php';
//鏂囦欢淇濆瓨鐩綍璺緞
$save_path = '../../'.$upfiles_dir.date("Y/m/d/");
make_dir($save_path);
//鏂囦欢淇濆瓨鐩綍URL
$save_url = $_CFG['upfiles_dir'].date("Y/m/d/");
//瀹氫箟鍏佽涓婁紶鐨勬枃浠舵墿灞曞悕
$ext_arr = array(
	'image' => array('gif', 'jpg', 'jpeg', 'png', 'bmp'),
	'flash' => array('swf', 'flv'),
	'media' => array('swf', 'flv', 'mp3', 'wav', 'wma', 'wmv', 'mid', 'avi', 'mpg', 'asf', 'rm', 'rmvb'),
	'file' => array('doc', 'docx', 'xls', 'xlsx', 'ppt', 'htm', 'html', 'txt', 'zip', 'rar', 'gz', 'bz2'),
);
//鏈€澶ф枃浠跺ぇ灏?$max_size = 100000000;

$save_path = realpath($save_path) . '/';

//PHP涓婁紶澶辫触
if (!empty($_FILES['imgFile']['error'])) {
	switch($_FILES['imgFile']['error']){
		case '1':
			$error = '瓒呰繃php.ini鍏佽鐨勫ぇ灏忋€?;
			break;
		case '2':
			$error = '瓒呰繃琛ㄥ崟鍏佽鐨勫ぇ灏忋€?;
			break;
		case '3':
			$error = '鍥剧墖鍙湁閮ㄥ垎琚笂浼犮€?;
			break;
		case '4':
			$error = '璇烽€夋嫨鍥剧墖銆?;
			break;
		case '6':
			$error = '鎵句笉鍒颁复鏃剁洰褰曘€?;
			break;
		case '7':
			$error = '鍐欐枃浠跺埌纭洏鍑洪敊銆?;
			break;
		case '8':
			$error = 'File upload stopped by extension銆?;
			break;
		case '999':
		default:
			$error = '鏈煡閿欒銆?;
	}
	alert($error);
}

//鏈変笂浼犳枃浠舵椂
if (empty($_FILES) === false) {
	//鍘熸枃浠跺悕
	$file_name = $_FILES['imgFile']['name'];
	//鏈嶅姟鍣ㄤ笂涓存椂鏂囦欢鍚?	$tmp_name = $_FILES['imgFile']['tmp_name'];
	//鏂囦欢澶у皬
	$file_size = $_FILES['imgFile']['size'];
	//妫€鏌ユ枃浠跺悕
	if (!$file_name) {
		alert("璇烽€夋嫨鏂囦欢銆?);
	}
	//妫€鏌ョ洰褰?	if (@is_dir($save_path) === false) {
		alert("涓婁紶鐩綍涓嶅瓨鍦ㄣ€?);
	}
	//妫€鏌ョ洰褰曞啓鏉冮檺
	if (@is_writable($save_path) === false) {
		alert("涓婁紶鐩綍娌℃湁鍐欐潈闄愩€?);
	}
	//妫€鏌ユ槸鍚﹀凡涓婁紶
	if (@is_uploaded_file($tmp_name) === false) {
		alert("涓婁紶澶辫触銆?);
	}
	//妫€鏌ユ枃浠跺ぇ灏?	if ($file_size > $max_size) {
		alert("涓婁紶鏂囦欢澶у皬瓒呰繃闄愬埗銆?);
	}
	//妫€鏌ョ洰褰曞悕
	$dir_name = empty($_GET['dir']) ? 'image' : trim($_GET['dir']);
	if (empty($ext_arr[$dir_name])) {
		alert("鐩綍鍚嶄笉姝ｇ‘銆?);
	}
	//鑾峰緱鏂囦欢鎵╁睍鍚?	$temp_arr = explode(".", $file_name);
	$file_ext = array_pop($temp_arr);
	$file_ext = trim($file_ext);
	$file_ext = strtolower($file_ext);
	//妫€鏌ユ墿灞曞悕
	if (in_array($file_ext, $ext_arr[$dir_name]) === false) {
		alert("涓婁紶鏂囦欢鎵╁睍鍚嶆槸涓嶅厑璁哥殑鎵╁睍鍚嶃€俓n鍙厑璁? . implode(",", $ext_arr[$dir_name]) . "鏍煎紡銆?);
	}
	//鍒涘缓鏂囦欢澶?	if ($dir_name !== '') {
		$save_path .= $dir_name . "/";
		$save_url .= $dir_name . "/";
		if (!file_exists($save_path)) {
			mkdir($save_path);
		}
	}
	$ymd = date("Ymd");
	$save_path .= $ymd . "/";
	$save_url .= $ymd . "/";
	if (!file_exists($save_path)) {
		mkdir($save_path);
	}
	//鏂版枃浠跺悕
	$new_file_name = date("YmdHis") . '_' . rand(10000, 99999) . '.' . $file_ext;
	//绉诲姩鏂囦欢
	$file_path = $save_path . $new_file_name;
	if (move_uploaded_file($tmp_name, $file_path) === false) {
		alert("涓婁紶鏂囦欢澶辫触銆?);
	}
	@chmod($file_path, 0644);
	$file_url = $save_url . $new_file_name;

	header('Content-type: text/html; charset=UTF-8');
	$json = new Services_JSON();
	echo $json->encode(array('error' => 0, 'url' => $file_url));
	exit;
}

function alert($msg) {
	header('Content-type: text/html; charset=UTF-8');
	$json = new Services_JSON();
	echo $json->encode(array('error' => 1, 'message' => $msg));
	exit;
}
