<?php
//header('Access-Control-Allow-Origin: http://www.baidu.com'); //璁剧疆http://www.baidu.com鍏佽璺ㄥ煙璁块棶
//header('Access-Control-Allow-Headers: X-Requested-With,X_Requested_With'); //璁剧疆鍏佽鐨勮法鍩焗eader
date_default_timezone_set("Asia/chongqing");
error_reporting(E_ERROR);
header("Content-Type: text/html; charset=gbk");

$CONFIG = json_decode(preg_replace("/\/\*[\s\S]+?\*\//", "", file_get_contents("config.json")), true);
$action = $_GET['action'];

switch ($action) {
    case 'config':
        $result =  json_encode($CONFIG);
        break;

    /* 涓婁紶鍥剧墖 */
    case 'uploadimage':
    /* 涓婁紶娑傞甫 */
    case 'uploadscrawl':
    /* 涓婁紶瑙嗛 */
    case 'uploadvideo':
    /* 涓婁紶鏂囦欢 */
    case 'uploadfile':
        $result = include("action_upload.php");
        break;

    /* 鍒楀嚭鍥剧墖 */
    case 'listimage':
        $result = include("action_list.php");
        break;
    /* 鍒楀嚭鏂囦欢 */
    case 'listfile':
        $result = include("action_list.php");
        break;

    /* 鎶撳彇杩滅▼鏂囦欢 */
    case 'catchimage':
        $result = include("action_crawler.php");
        break;

    default:
        $result = json_encode(array(
            'state'=> '璇锋眰鍦板潃鍑洪敊'
        ));
        break;
}

/* 杈撳嚭缁撴灉 */
if (isset($_GET["callback"])) {
    if (preg_match("/^[\w_]+$/", $_GET["callback"])) {
        echo htmlspecialchars($_GET["callback"]) . '(' . $result . ')';
    } else {
        echo json_encode(array(
            'state'=> 'callback鍙傛暟涓嶅悎娉?
        ));
    }
} else {
    echo $result;
}