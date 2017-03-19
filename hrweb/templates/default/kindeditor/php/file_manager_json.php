<?php
/**
 * KindEditor PHP
 *
 * 鏈琍HP绋嬪簭鏄紨绀虹▼搴忥紝寤鸿涓嶈鐩存帴鍦ㄥ疄闄呴」鐩腑浣跨敤銆? * 濡傛灉鎮ㄧ‘瀹氱洿鎺ヤ娇鐢ㄦ湰绋嬪簭锛屼娇鐢ㄤ箣鍓嶈浠旂粏纭鐩稿叧瀹夊叏璁剧疆銆? *
 */

require_once 'JSON.php';

$php_path = dirname(__FILE__) . '/';
$php_url = dirname($_SERVER['PHP_SELF']) . '/';

//鏍圭洰褰曡矾寰勶紝鍙互鎸囧畾缁濆璺緞锛屾瘮濡?/var/www/attached/
$root_path = $php_path . '../attached/';
//鏍圭洰褰昒RL锛屽彲浠ユ寚瀹氱粷瀵硅矾寰勶紝姣斿 http://www.yoursite.com/attached/
$root_url = $php_url . '../attached/';
//鍥剧墖鎵╁睍鍚?$ext_arr = array('gif', 'jpg', 'jpeg', 'png', 'bmp');

//鐩綍鍚?$dir_name = empty($_GET['dir']) ? '' : trim($_GET['dir']);
if (!in_array($dir_name, array('', 'image', 'flash', 'media', 'file'))) {
	echo "Invalid Directory name.";
	exit;
}
if ($dir_name !== '') {
	$root_path .= $dir_name . "/";
	$root_url .= $dir_name . "/";
	if (!file_exists($root_path)) {
		mkdir($root_path);
	}
}

//鏍规嵁path鍙傛暟锛岃缃悇璺緞鍜孶RL
if (empty($_GET['path'])) {
	$current_path = realpath($root_path) . '/';
	$current_url = $root_url;
	$current_dir_path = '';
	$moveup_dir_path = '';
} else {
	$current_path = realpath($root_path) . '/' . $_GET['path'];
	$current_url = $root_url . $_GET['path'];
	$current_dir_path = $_GET['path'];
	$moveup_dir_path = preg_replace('/(.*?)[^\/]+\/$/', '$1', $current_dir_path);
}
//echo realpath($root_path);
//鎺掑簭褰㈠紡锛宯ame or size or type
$order = empty($_GET['order']) ? 'name' : strtolower($_GET['order']);

//涓嶅厑璁镐娇鐢?.绉诲姩鍒颁笂涓€绾х洰褰?if (preg_match('/\.\./', $current_path)) {
	echo 'Access is not allowed.';
	exit;
}
//鏈€鍚庝竴涓瓧绗︿笉鏄?
if (!preg_match('/\/$/', $current_path)) {
	echo 'Parameter is not valid.';
	exit;
}
//鐩綍涓嶅瓨鍦ㄦ垨涓嶆槸鐩綍
if (!file_exists($current_path) || !is_dir($current_path)) {
	echo 'Directory does not exist.';
	exit;
}

//閬嶅巻鐩綍鍙栧緱鏂囦欢淇℃伅
$file_list = array();
if ($handle = opendir($current_path)) {
	$i = 0;
	while (false !== ($filename = readdir($handle))) {
		if ($filename{0} == '.') continue;
		$file = $current_path . $filename;
		if (is_dir($file)) {
			$file_list[$i]['is_dir'] = true; //鏄惁鏂囦欢澶?			$file_list[$i]['has_file'] = (count(scandir($file)) > 2); //鏂囦欢澶规槸鍚﹀寘鍚枃浠?			$file_list[$i]['filesize'] = 0; //鏂囦欢澶у皬
			$file_list[$i]['is_photo'] = false; //鏄惁鍥剧墖
			$file_list[$i]['filetype'] = ''; //鏂囦欢绫诲埆锛岀敤鎵╁睍鍚嶅垽鏂?		} else {
			$file_list[$i]['is_dir'] = false;
			$file_list[$i]['has_file'] = false;
			$file_list[$i]['filesize'] = filesize($file);
			$file_list[$i]['dir_path'] = '';
			$file_ext = strtolower(pathinfo($file, PATHINFO_EXTENSION));
			$file_list[$i]['is_photo'] = in_array($file_ext, $ext_arr);
			$file_list[$i]['filetype'] = $file_ext;
		}
		$file_list[$i]['filename'] = $filename; //鏂囦欢鍚嶏紝鍖呭惈鎵╁睍鍚?		$file_list[$i]['datetime'] = date('Y-m-d H:i:s', filemtime($file)); //鏂囦欢鏈€鍚庝慨鏀规椂闂?		$i++;
	}
	closedir($handle);
}

//鎺掑簭
function cmp_func($a, $b) {
	global $order;
	if ($a['is_dir'] && !$b['is_dir']) {
		return -1;
	} else if (!$a['is_dir'] && $b['is_dir']) {
		return 1;
	} else {
		if ($order == 'size') {
			if ($a['filesize'] > $b['filesize']) {
				return 1;
			} else if ($a['filesize'] < $b['filesize']) {
				return -1;
			} else {
				return 0;
			}
		} else if ($order == 'type') {
			return strcmp($a['filetype'], $b['filetype']);
		} else {
			return strcmp($a['filename'], $b['filename']);
		}
	}
}
usort($file_list, 'cmp_func');

$result = array();
//鐩稿浜庢牴鐩綍鐨勪笂涓€绾х洰褰?$result['moveup_dir_path'] = $moveup_dir_path;
//鐩稿浜庢牴鐩綍鐨勫綋鍓嶇洰褰?$result['current_dir_path'] = $current_dir_path;
//褰撳墠鐩綍鐨刄RL
$result['current_url'] = $current_url;
//鏂囦欢鏁?$result['total_count'] = count($file_list);
//鏂囦欢鍒楄〃鏁扮粍
$result['file_list'] = $file_list;

//杈撳嚭JSON瀛楃涓?header('Content-type: application/json; charset=UTF-8');
$json = new Services_JSON();
echo $json->encode($result);
