<?php
function _asUpFiles($dir, $file_var, $max_size='', $type='', $name=false) 
{
if (!file_exists($dir)) adminmsg("画像アップロード失敗：アップロードフォルダー ".$dir." が存在しません!",0);
if (!is_writable($dir)) 
{
adminmsg("画像アップロード失敗：アップロードフォルダー ".$dir." 書けない!",0);
exit(); 
}
$upfile=& $_FILES["$file_var"]; 
$upfilename =  $upfile['name']; 
if (!($upfilename==='')) 
{ 
if (!is_uploaded_file($upfile['tmp_name'])) 
{ 
adminmsg('画像アップロード失敗：選択されたファイルアップロードができません',0);
exit(); 
} 
if ($max_size>0 && $upfile['size']/1024>$max_size) 
{ 
adminmsg("アップロード写真失敗：ファイル最大サイズ  ".$max_size."KB",0);
exit(); 
} 
$ext_name = strtolower(str_replace(".", "", strrchr($upfilename, "."))); 
if (!($type==='') && strpos($type, $ext_name)===false) 
{ 
adminmsg("画像アップロード失敗：アップロード許可 ".$type." のファイル！",0);
exit(); 
}
($name==true)?$uploadname=time().mt_rand(100,999).".".$ext_name :'';
($name==false)?$uploadname=$upfilename:'';
!is_bool($name)?($uploadname=$name.".".$ext_name):'';
//$uploadname = $name ? md5(uniqid(rand())).".".$ext_name : $upfilename; 
if (!move_uploaded_file($upfile['tmp_name'], $dir.$uploadname)) 
{ 
adminmsg('画像アップロード失敗：ファイルアップロードエラー！',0);
 exit(); 
} 
return $uploadname; 
} 
else 
{ 
return ''; 
} 
} 
/*图象缩略函数
参数说明：
$srcfile 原图地址； 
$dir  新图目录 
$thumbwidth 缩小图宽最大尺寸 
$thumbheitht 缩小图高最大尺寸 
$ratio 默认等比例缩放 为1是缩小到固定尺寸。 
*/ 
function makethumb($srcfile,$dir,$thumbwidth,$thumbheight,$ratio=0)
{ 
 //判断文件是否存在 
if (!file_exists($srcfile))return false;
 //生成新的同名文件，但目录不同 
$imgname=explode('/',$srcfile); 
$arrcount=count($imgname); 
$dstfile = $dir.$imgname[$arrcount-1]; 
//缩略图大小 
$tow = $thumbwidth; 
$toh = $thumbheight; 
if($tow < 40) $tow = 40; 
if($toh < 45) $toh = 45;    
 //获取图片信息 
    $im =''; 
    if($data = getimagesize($srcfile)) { 
        if($data[2] == 1) { 
            $make_max = 0;//gif不处理 
            if(function_exists("imagecreatefromgif")) { 
                $im = imagecreatefromgif($srcfile); 
            } 
        } elseif($data[2] == 2) { 
            if(function_exists("imagecreatefromjpeg")) { 
                $im = imagecreatefromjpeg($srcfile); 
            } 
        } elseif($data[2] == 3) { 
            if(function_exists("imagecreatefrompng")) { 
                $im = imagecreatefrompng($srcfile); 
            } 
        } 
    } 
    if(!$im) return ''; 
    $srcw = imagesx($im); 
    $srch = imagesy($im); 
    $towh = $tow/$toh; 
    $srcwh = $srcw/$srch; 
    if($towh <= $srcwh){ 
        $ftow = $tow; 
        $ftoh = $ftow*($srch/$srcw); 
    } else { 
        $ftoh = $toh; 
        $ftow = $ftoh*($srcw/$srch); 
    } 
    if($ratio){ 
        $ftow = $tow; 
        $ftoh = $toh; 
    } 
    //缩小图片 
    if($srcw > $tow || $srch > $toh || $ratio) { 
        if(function_exists("imagecreatetruecolor") && function_exists("imagecopyresampled") && @$ni = imagecreatetruecolor($ftow, $ftoh)) { 
            imagecopyresampled($ni, $im, 0, 0, 0, 0, $ftow, $ftoh, $srcw, $srch); 
        } elseif(function_exists("imagecreate") && function_exists("imagecopyresized") && @$ni = imagecreate($ftow, $ftoh)) { 
            imagecopyresized($ni, $im, 0, 0, 0, 0, $ftow, $ftoh, $srcw, $srch); 
        } else { 
            return ''; 
        } 
        if(function_exists('imagejpeg')) { 
            imagejpeg($ni, $dstfile); 
        } elseif(function_exists('imagepng')) { 
            imagepng($ni, $dstfile); 
        } 
    }else { 
        //小于尺寸直接复制 
    copy($srcfile,$dstfile); 
    } 
    imagedestroy($im); 
    if(!file_exists($dstfile)) { 
        return ''; 
    } else { 
        return $dstfile; 
    } 
}
?>
