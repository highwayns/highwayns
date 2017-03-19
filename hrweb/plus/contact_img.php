<?php
 /*
 * 74cms 鑱旂郴鏂瑰紡鍥惧舰鍖? * ============================================================================
 * 鐗堟潈鎵€鏈? 楠戝＋缃戠粶锛屽苟淇濈暀鎵€鏈夋潈鍒┿€? * 缃戠珯鍦板潃: http://www.74cms.com锛? * ----------------------------------------------------------------------------
 * 杩欎笉鏄竴涓嚜鐢辫蒋浠讹紒鎮ㄥ彧鑳藉湪涓嶇敤浜庡晢涓氱洰鐨勭殑鍓嶆彁涓嬪绋嬪簭浠ｇ爜杩涜淇敼鍜? * 浣跨敤锛涗笉鍏佽瀵圭▼搴忎唬鐮佷互浠讳綍褰㈠紡浠讳綍鐩殑鐨勫啀鍙戝竷銆? * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../include/plus.common.inc.php');
require_once(QISHI_ROOT_PATH.'include/mysql.class.php');
$db = new mysql($dbhost,$dbuser,$dbpass,$dbname);
$act = trim($_GET['act']);
$type =intval($_GET['type']);
$token=trim($_GET['token']);
$id=intval($_GET['id']);
function hidtel($phone){
    $IsWhat = preg_match('/(0[0-9]{2,3}[-]?[2-9][0-9]{6,7}[-]?[0-9]?)/i',$phone); //鍥哄畾鐢佃瘽
    if($IsWhat == 1){
        return preg_replace('/(0[0-9]{2,3}[-]?[2-9])[0-9]{3,4}([0-9]{3}[-]?[0-9]?)/i','$1****$2',$phone);
    }else{
        return  preg_replace('/(1[358]{1}[0-9])[0-9]{4}([0-9]{4})/i','$1****$2',$phone);
    }
} 
if($act == 'jobs_contact')
{
			$sql = "select * from ".table('jobs_contact')." where pid='{$id}' LIMIT 1";
			$val=$db->getone($sql);
			$tmd5=md5($val['contact'].$id.$val['telephone']);
			$user = $db->getone("select m.username uname from ".table("members")." as m left join ".table("jobs")." as j on m.uid=j.uid where j.id=$id ");
			$hashstr=substr(md5($user['uname']),8,16);
			if ($tmd5<>$token)
			{
			exit();
			}
			switch ($type)
			{
			case 1:
			  $t=$val['contact'];
			  break;  
			case 2:
				if($hashstr==$_GET['hashstr']){
					$t=$val['telephone'];
				}else{
					$t=hidtel($val['telephone']); 
				} 
			  break;
			  case 3:
			   $t=$val['email'];
			  break;
			  case 4:
			   $t=$val['address'];
			  break;
			  case 5:
			   $t=$val['qq'];
			  break;
			}
}
elseif($act == 'company_contact')
{
			$sql = "select contact,telephone,email FROM ".table('company_profile')." where id=".intval($id)." LIMIT 1";
			$val=$db->getone($sql);
			$tmd5=md5($val['contact'].$id.$val['telephone']);
			if ($tmd5<>$token)
			{
			exit();
			}
			switch ($type)
			{
			case 1:
			  $t=$val['contact'];
			  break;  
			case 2: 
			 $t=$val['telephone']; 
			  break;
			case 3:
			   $t=$val['email'];
			  break;
			}
}
//绠€鍘嗚仈绯绘柟寮?elseif($act == 'resume_contact')
{
		$val=$db->getone("select fullname,telephone,email from ".table('resume')." WHERE  id='{$id}'  LIMIT 1");
		$tmd5=md5($val['fullname'].$id.$val['telephone']);
			if ($tmd5<>$token)
			{
			exit();
			}	
		switch ($type)
			{
			case 1:
			  $t=$val['fullname'];
			  break; 
			case 2: 
			 $t=$val['telephone']; 
			  break;  
			case 3:
			   $t=$val['email'];
			  break;
			}
}
header("Content-type: image/gif");
$w=10+(strlen($t)*9);
$h=22;
$im = imagecreate($w,$h);
$white = imagecolorallocate($im, 255,255,255);
$black = imagecolorallocate($im, 255,0,0);
if (strcasecmp(QISHI_DBCHARSET,"utf8")!=0)
	{
		$t=iconv(QISHI_DBCHARSET,"utf-8",$t);
	}
$ttf=QISHI_ROOT_PATH."data/contactimgfont/cn.ttc";
imagettftext($im, 12, 0, 10, 15, $black, $ttf,$t); 
imagegif($im);
ImageDestroy($im);

  
?> 
