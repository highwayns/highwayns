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
		if(isset($img)&&file_exists($img)){//检测图片是否存在
		   $this->image   =$img; 
		   $this->img_text=$txt;
		   $this->img_text_size=$size;
		   $this->img_jd=$jiaodu;
		  if(file_exists($ttf)){
		   $this->img_ttf=$ttf;
		  }else{
		   exit('フォントファイル：'.$ttf.'存在しない！'); 
		  }
		   $this->imgyesno();
		}else{
	      exit('图片文件:'.$img.'存在しない');
	   } 
	 }
	 private function imgyesno(){	
		$this->img_info  =getimagesize($this->image);
		$this->img_width =$this->img_info[0];//图片宽
		$this->img_height=$this->img_info[1];//图片高
		//检测图片类型
		switch($this->img_info[2]){
             case 1:$this->img_im = imagecreatefromgif($this->image);break; 
             case 2:$this->img_im = imagecreatefromjpeg($this->image);break; 
             case 3:$this->img_im = imagecreatefrompng($this->image);break; 
             default:exit('画像フォーマットはウォーターマックできない'); 
		}
		   $this->img_text();
	 }
	 private function img_text(){	 
		 imagealphablending($this->img_im,true); 
		 //设定颜色
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
		 @unlink($this->image);//删除图片
		switch($this->img_info[2]) {//取得背景图片的格式 
          case 1:imagegif($this->img_im,$this->image);break; 
          case 2:imagejpeg($this->img_im,$this->image);break; 
          case 3:imagepng($this->img_im,$this->image);break; 
          default: exit('ウォーターマック画像失敗'); 
        } 
	 }
	 //显示图片
	 function img_show(){echo '<img src="'.$this->image.'" border="0" alt="'.$this->img_text.'" />';}
	 //释放内存
	 private function img_nothing(){
		 unset($this->img_info); 
         imagedestroy($this->img_im); 
	 }
 }
?>
