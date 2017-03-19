<?php
/**
 * 涓婁紶闄勪欢鍜屼笂浼犺棰? * User: Jinqn
 * Date: 14-04-09
 * Time: 涓婂崍10:17
 */
include "Uploader.class.php";
include '../../../data/cache_config.php';
/* 涓婁紶閰嶇疆 */
$base64 = "upload";
switch (htmlspecialchars($_GET['action'])) {
    case 'uploadimage':
        $config = array(
            // "pathFormat" => $CONFIG['imagePathFormat'],
            "pathFormat" => $data['site_dir'].$CONFIG['imagePathFormat'],
            "maxSize" => $CONFIG['imageMaxSize'],
            "allowFiles" => $CONFIG['imageAllowFiles']
        );
        $fieldName = $CONFIG['imageFieldName'];
        break;
    case 'uploadscrawl':
        $config = array(
            "pathFormat" => $CONFIG['scrawlPathFormat'],
            "maxSize" => $CONFIG['scrawlMaxSize'],
            "allowFiles" => $CONFIG['scrawlAllowFiles'],
            "oriName" => "scrawl.png"
        );
        $fieldName = $CONFIG['scrawlFieldName'];
        $base64 = "base64";
        break;
    case 'uploadvideo':
        $config = array(
            "pathFormat" => $CONFIG['videoPathFormat'],
            "maxSize" => $CONFIG['videoMaxSize'],
            "allowFiles" => $CONFIG['videoAllowFiles']
        );
        $fieldName = $CONFIG['videoFieldName'];
        break;
    case 'uploadfile':
    default:
        $config = array(
            "pathFormat" => $CONFIG['filePathFormat'],
            "maxSize" => $CONFIG['fileMaxSize'],
            "allowFiles" => $CONFIG['fileAllowFiles']
        );
        $fieldName = $CONFIG['fileFieldName'];
        break;
}

/* 鐢熸垚涓婁紶瀹炰緥瀵硅薄骞跺畬鎴愪笂浼?*/
$up = new Uploader($fieldName, $config, $base64);

/**
 * 寰楀埌涓婁紶鏂囦欢鎵€瀵瑰簲鐨勫悇涓弬鏁?鏁扮粍缁撴瀯
 * array(
 *     "state" => "",          //涓婁紶鐘舵€侊紝涓婁紶鎴愬姛鏃跺繀椤昏繑鍥?SUCCESS"
 *     "url" => "",            //杩斿洖鐨勫湴鍧€
 *     "title" => "",          //鏂版枃浠跺悕
 *     "original" => "",       //鍘熷鏂囦欢鍚? *     "type" => ""            //鏂囦欢绫诲瀷
 *     "size" => "",           //鏂囦欢澶у皬
 * )
 */

/* 杩斿洖鏁版嵁 */
return json_encode($up->getFileInfo());
