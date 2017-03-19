	var cut_div;  //裁减图片外框div
	var cut_img;  //裁减图片
	var imgdefw;  //图片默认宽度
	var imgdefh;  //图片默认高度
	var offsetx = 100; //图片位置位移x
	var offsety = -225; //图片位置位移y
	var divx = 320; //外框宽度
	var divy = 300; //外框高度
	var cutx = 120;  //裁减宽度
	var cuty = 150;  //裁减高度
	var zoom = 1; //缩放比例

	var zmin = 0.1; //最小比例
	var zmax = 1.2; //最大比例
	var grip_pos = 10; //拖动块位置0-最左 10 最右
	var img_grip; //拖动块
	var img_track; //拖动条
	var grip_y; //拖动块y值
	var grip_minx; //拖动块x最小值
	var grip_maxx; //拖动块x最大值	
//图片初始化
function imageinit(){
	cut_div = document.getElementById('cut_div');
	cut_img = document.getElementById('cut_img');
	imgdefw = cut_img.width;
	imgdefh = cut_img.height;
	if(imgdefw > divx){
		zoom = divx / imgdefw;
		cut_img.width = divx;
		cut_img.height = Math.round(imgdefh * zoom);
	}

	cut_img.style.left = Math.round((divx - cut_img.width) / 2);
	cut_img.style.top = Math.round((divy - cut_img.height) / 2) - divy;

	if(imgdefw > cutx){
		zmin = cutx / imgdefw;
	}else{
		zmin = 1;
	}
	zmax =  zmin > 0.25 ? 8.0: 4.0 / Math.sqrt(zmin);
	if(imgdefw > cutx){
		zmin = cutx / imgdefw;
		grip_pos = 5 * (Math.log(zoom * zmax) / Math.log(zmax));
	}else{
		zmin = 1;
		grip_pos = 5;
	}

	Drag.init(cut_div, cut_img);
	cut_img.onDrag = when_Drag;
}

//图片逐步缩放
function imageresize(flag){
    if(flag){
		zoom = zoom * 1.5;
	}else{
		zoom = zoom / 1.5;
	}
	if(zoom < zmin) zoom = zmin;
	if(zoom > zmax) zoom = zmax;
	cut_img.width = Math.round(imgdefw * zoom);
	cut_img.height = Math.round(imgdefh * zoom);
	checkcutpos();
	grip_pos = 5 * (Math.log(zoom * zmax) / Math.log(zmax));
	img_grip.style.left = (grip_minx + (grip_pos / 10 * (grip_maxx - grip_minx))) + "px";
}

//获得style里面定位
function getStylepos(e){  
	return {x:parseInt(e.style.left), y:parseInt(e.style.top)}; 
}

//获得绝对定位
function getPosition(e){  
	var t=e.offsetTop;  
	var l=e.offsetLeft;  
	while(e=e.offsetParent){  
		t+=e.offsetTop;  
		l+=e.offsetLeft;  
	}
	return {x:l, y:t}; 
}

//检查图片位置
function checkcutpos(){
	var imgpos = getStylepos(cut_img);
	
	max_x = Math.max(offsetx, offsetx + cutx - cut_img.clientWidth);
	min_x = Math.min(offsetx + cutx - cut_img.clientWidth, offsetx);
	if(imgpos.x > max_x) cut_img.style.left = max_x + 'px';
	else if(imgpos.x < min_x) cut_img.style.left = min_x + 'px';

	max_y = Math.max(offsety, offsety + cuty - cut_img.clientHeight);
	min_y = Math.min(offsety + cuty - cut_img.clientHeight, offsety);

	if(imgpos.y > max_y) cut_img.style.top = max_y + 'px';
	else if(imgpos.y < min_y) cut_img.style.top = min_y + 'px';
}

//图片拖动时触发
function when_Drag(clientX , clientY){
	checkcutpos();
}

//获得图片裁减位置
function getcutpos(){
	var imgpos = getStylepos(cut_img);
	var x = offsetx - imgpos.x;
	var y = offsety - imgpos.y;
	var cut_pos = document.getElementById('cut_pos');
	cut_pos.value = x + ',' + y + ',' + cut_img.width + ',' + cut_img.height;
	return true;
}

//缩放条初始化
function gripinit(){
	img_grip = document.getElementById('img_grip');
	img_track = document.getElementById('img_track');
	track_pos = getPosition(img_track);

	grip_y = track_pos.y;
	grip_minx = track_pos.x + 4;
	grip_maxx = track_pos.x + img_track.clientWidth - img_grip.clientWidth - 5;

	img_grip.style.left = (grip_minx + (grip_pos / 10 * (grip_maxx - grip_minx))) + "px";
	img_grip.style.top = grip_y + "px";

	Drag.init(img_grip, img_grip);
	img_grip.onDrag = grip_Drag;

}

//缩放条拖动时触发
function grip_Drag(clientX , clientY){
	var posx = clientX;
	img_grip.style.top = grip_y + "px";
	if(clientX < grip_minx){
		img_grip.style.left = grip_minx + "px";
		posx = grip_minx;
	}
	if(clientX > grip_maxx){
		img_grip.style.left = grip_maxx + "px";
		posx = grip_maxx;
	}

	grip_pos = (posx - grip_minx) * 10 / (grip_maxx - grip_minx);
	zoom = Math.pow(zmax, grip_pos / 5) / zmax;
	if(zoom < zmin) zoom = zmin;
	if(zoom > zmax) zoom = zmax;
	cut_img.width = Math.round(imgdefw * zoom);
	cut_img.height = Math.round(imgdefh * zoom);
	checkcutpos();
}

//页面载入初始化
function avatarinit(){
	imageinit();
	gripinit();
	//刷新裁剪后的图片
	document.getElementById('cutimg_l').src = document.getElementById('cutimg_l').src + '?' + (new   Date().getTime());
	document.getElementById('cutimg_m').src = document.getElementById('cutimg_m').src + '?' + (new   Date().getTime());
	//document.getElementById('cutimg_s').src = document.getElementById('cutimg_s').src + '?' + (new   Date().getTime());

}

if (document.all){
	window.attachEvent('onload',avatarinit);
}else{
	window.addEventListener('load',avatarinit,false);
} 