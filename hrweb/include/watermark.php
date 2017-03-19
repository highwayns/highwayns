<?php
/*
 * 74cms PHP鍥剧墖鍔犳枃瀛楁按鍗扮被搴? * ============================================================================
 * 鐗堟潈鎵€鏈? 楠戝＋缃戠粶锛屽苟淇濈暀鎵€鏈夋潈鍒┿€? * 缃戠珯鍦板潃: http://www.74cms.com锛? * ----------------------------------------------------------------------------
 * 杩欎笉鏄竴涓嚜鐢辫蒋浠讹紒鎮ㄥ彧鑳藉湪涓嶇敤浜庡晢涓氱洰鐨勭殑鍓嶆彁涓嬪绋嬪簭浠ｇ爜杩涜淇敼鍜? * 浣跨敤锛涗笉鍏佽瀵圭▼搴忎唬鐮佷互浠讳綍褰㈠紡浠讳綍鐩殑鐨勫啀鍙戝竷銆? * ============================================================================
	
  	璇ョ被搴撴殏鏃跺彧鏀寔鏂囧瓧姘村嵃锛屼綅缃负鍙充笅瑙掞紝棰滆壊闅忔満
  	璋冪敤鏂规硶锛?   		1銆佸湪闇€瑕佸姞姘村嵃鐨勬枃浠堕《閮ㄥ紩鍏ョ被搴擄細
     	 include_once 'watermark.php';
   		2銆佸０鏄庢柊绫伙細
	 	 $tpl=new watermark;
   		3銆佺粰鍥剧墖姘村嵃鎻愪緵鍙傛暟锛?     	 $tpl->img(鍥剧墖璺緞,姘村嵃鏂囧瓧,瀛椾綋璺緞,瀛椾綋澶у皬,瀛椾綋瑙掑害);
	 	 姣斿锛?tpl->img('abc.jpg','杩欐槸姘村嵃鏂囧瓧','ziti.ttf',30,0)
*/
 class watermark{ 
	 private $image;
	 private $img_info;
	 private $img_width;
	 private $img_height;
	 private $img_im;
	 private $img_text;
	 private $img_ttf='';
	 private $img_new;
	 private $img_text_size;
	 private $img_jd;
	 function img($img='',$txt='',$ttf='',$size=12,$jiaodu=0){
		if(isset($img)&&file_exists($img)){//妫€娴嬪浘鐗囨槸鍚﹀瓨鍦?		   $this->image   =$img; 
		   $this->img_text=$txt;
		   $this->img_text_size=$size;
		   $this->img_jd=$jiaodu;
		  if(file_exists($ttf)){
		   $this->img_ttf=$ttf;
		  }else{
		   exit('瀛椾綋鏂囦欢锛?.$ttf.'涓嶅瓨鍦紒'); 
		  }
		   $this->imgyesno();
		}else{
	      exit('鍥剧墖鏂囦欢:'.$img.'涓嶅瓨鍦?);
	   } 
	 }
	 private function imgyesno(){	
		$this->img_info  =getimagesize($this->image);
		$this->img_width =$this->img_info[0];//鍥剧墖瀹?		$this->img_height=$this->img_info[1];//鍥剧墖楂?		//妫€娴嬪浘鐗囩被鍨?		switch($this->img_info[2]){
             case 1:$this->img_im = imagecreatefromgif($this->image);break; 
             case 2:$this->img_im = imagecreatefromjpeg($this->image);break; 
             case 3:$this->img_im = imagecreatefrompng($this->image);break; 
             default:exit('鍥剧墖鏍煎紡涓嶆敮鎸佹按鍗?); 
		}
		   $this->img_text();
	 }
	 private function img_text(){	 
		 imagealphablending($this->img_im,true); 
		 //璁惧畾棰滆壊
		  $color=imagecolorallocate($this->img_im,rand(0,255),rand(0,255),rand(0,255));
		  $txt_height=$this->img_text_size;
		  $txt_jiaodu=$this->img_jd;
		  $ttf_im=imagettfbbox($txt_height,$txt_jiaodu,$this->img_ttf,$this->img_text);
		  $w = $ttf_im[2] - $ttf_im[6]; 
          $h = $ttf_im[3] - $ttf_im[7]; 
		  //$w = $ttf_im[7]; 
          //$h = $ttf_im[8]; 
          unset($ttf_im);  
		  $txt_y     =$this->img_height-$h;
		  $txt_x     =$this->img_width-$w;
		  //$txt_y     =0;
		  //$txt_x     =0; 
		  $this->img_new=@imagettftext($this->img_im,$txt_height,$txt_jiaodu,$txt_x,$txt_y,$color,$this->img_ttf,$this->img_text);	 
		 @unlink($this->image);//鍒犻櫎鍥剧墖
		switch($this->img_info[2]) {//鍙栧緱鑳屾櫙鍥剧墖鐨勬牸寮?
          case 1:imagegif($this->img_im,$this->image);break; 
          case 2:imagejpeg($this->img_im,$this->image);break; 
          case 3:imagepng($this->img_im,$this->image);break; 
          default: exit('姘村嵃鍥剧墖澶辫触'); 
        } 
	 }
	 //鏄剧ず鍥剧墖
	 function img_show(){echo '<img src="'.$this->image.'" border="0" alt="'.$this->img_text.'" />';}
	 //閲婃斁鍐呭瓨
	 private function img_nothing(){
		 unset($this->img_info); 
         imagedestroy($this->img_im); 
	 }
 }
?>