<?php
function _asUpFiles($dir, $file_var, $max_size='', $type='', $name=false) 
{
	if (!file_exists($dir))
	{
	showmsg("上传图片失败：上传目录 ".$dir." 不存在!",0);
	}
	elseif (!is_writable($dir)) 
	{
	showmsg("上传图片失败：上传目录 ".$dir." 无法写入!",0);
	exit(); 
	}
	$upfile=& $_FILES["$file_var"]; 
	$upfilename =  $upfile['name']; 
	if (!empty($upfilename)) 
	{
		if (!is_uploaded_file($upfile['tmp_name'])) 
		{ 
		showmsg('画像アップロード失敗：選択されたファイルアップロードができません',0);
		exit(); 
		} 
		elseif ($max_size>0 && $upfile['size']/1024>$max_size) 
		{ 
		showmsg("上传图片失败：文件大小不能超过  ".$max_size."KB",0);
		exit(); 
		}
		$ext_name = strtolower(str_replace(".","",strrchr($upfilename, ".")));
		if (!empty($type))
		{
			$arr_type=explode('/',$type);
			$arr_type=array_map("strtolower",$arr_type);
			if (!in_array($ext_name,$arr_type))
			{
			showmsg("上传图片失败：只允许上传 ".$type." 的文件！",0);
			exit(); 
			}
		/* 	$imgtype=array("jpg","gif","jpeg","bmp","png");		
			if (in_array($ext_name,$imgtype))
			{
				$imageinfo = getimagesize($upfile['tmp_name']);
				if (empty($imageinfo[0]) || empty($imageinfo[1]))
				{
				showmsg("上传图片失败：只允许上传 ".$type." 的文件！",0);
				exit();
				}
			} */
			$harmtype=array("asp","php","jsp","js","css","php3","php4","ashx","aspx","exe");	
			if (in_array($ext_name,$harmtype))
			{
			exit("ERR!");
			}
		}
			if (!is_bool($name))
			{
				$uploadname=$name.".".$ext_name;
			}
			elseif ($name===true)
			{
				$uploadname=time().mt_rand(100,999).".".$ext_name;
			}
			if (!move_uploaded_file($upfile['tmp_name'], $dir.$uploadname)) 
			{ 
				showmsg('画像アップロード失敗：ファイルアップロードエラー！',0);
				exit(); 
			} 
			return $uploadname; 
	}
	return ''; 
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
