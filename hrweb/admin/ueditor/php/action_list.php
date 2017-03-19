<?php
/**
 * 鑾峰彇宸蹭笂浼犵殑鏂囦欢鍒楄〃
 * User: Jinqn
 * Date: 14-04-09
 * Time: 涓婂崍10:17
 */
include "Uploader.class.php";

/* 鍒ゆ柇绫诲瀷 */
switch ($_GET['action']) {
    /* 鍒楀嚭鏂囦欢 */
    case 'listfile':
        $allowFiles = $CONFIG['fileManagerAllowFiles'];
        $listSize = $CONFIG['fileManagerListSize'];
        $path = $CONFIG['fileManagerListPath'];
        break;
    /* 鍒楀嚭鍥剧墖 */
    case 'listimage':
    default:
        $allowFiles = $CONFIG['imageManagerAllowFiles'];
        $listSize = $CONFIG['imageManagerListSize'];
        $path = $CONFIG['imageManagerListPath'];
}
$allowFiles = substr(str_replace(".", "|", join("", $allowFiles)), 1);

/* 鑾峰彇鍙傛暟 */
$size = isset($_GET['size']) ? htmlspecialchars($_GET['size']) : $listSize;
$start = isset($_GET['start']) ? htmlspecialchars($_GET['start']) : 0;
$end = $start + $size;

/* 鑾峰彇鏂囦欢鍒楄〃 */
$path = $_SERVER['DOCUMENT_ROOT'] . (substr($path, 0, 1) == "/" ? "":"/") . $path;
$files = getfiles($path, $allowFiles);
if (!count($files)) {
    return json_encode(array(
        "state" => "no match file",
        "list" => array(),
        "start" => $start,
        "total" => count($files)
    ));
}

/* 鑾峰彇鎸囧畾鑼冨洿鐨勫垪琛?*/
$len = count($files);
for ($i = min($end, $len) - 1, $list = array(); $i < $len && $i >= 0 && $i >= $start; $i--){
    $list[] = $files[$i];
}
//鍊掑簭
//for ($i = $end, $list = array(); $i < $len && $i < $end; $i++){
//    $list[] = $files[$i];
//}

/* 杩斿洖鏁版嵁 */
$result = json_encode(array(
    "state" => "SUCCESS",
    "list" => $list,
    "start" => $start,
    "total" => count($files)
));

return $result;


/**
 * 閬嶅巻鑾峰彇鐩綍涓嬬殑鎸囧畾绫诲瀷鐨勬枃浠? * @param $path
 * @param array $files
 * @return array
 */
function getfiles($path, $allowFiles, &$files = array())
{
    if (!is_dir($path)) return null;
    if(substr($path, strlen($path) - 1) != '/') $path .= '/';
    $handle = opendir($path);
    while (false !== ($file = readdir($handle))) {
        if ($file != '.' && $file != '..') {
            $path2 = $path . $file;
            if (is_dir($path2)) {
                getfiles($path2, $allowFiles, $files);
            } else {
                if (preg_match("/\.(".$allowFiles.")$/i", $file)) {
                    $files[] = array(
                        'url'=> substr($path2, strlen($_SERVER['DOCUMENT_ROOT'])),
                        'mtime'=> filemtime($path2)
                    );
                }
            }
        }
    }
    return $files;
}