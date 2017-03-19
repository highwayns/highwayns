/***
 * 寮瑰嚭灞傛彃浠?***/
function popWin(obj){
	var _z=9000;//鏂板璞＄殑z-index
	var _mv=false;//绉诲姩鏍囪
	var _x,_y;//榧犳爣绂绘帶浠跺乏涓婅鐨勭浉瀵逛綅缃?	
	var _obj= $("#"+obj);
	var _wid= _obj.width();
	var _hei= _obj.height();
	var _tit= _obj.find(".aui_title");
	var _cls =_obj.find(".aui_close");
	var docE =document.documentElement;
	var left=($(window).width() - _obj.outerWidth())/2;
	var top =($(window).height() - _obj.outerHeight())/2;
	_obj.css({	"position":"absolute","left":left,"top":top,"display":"block","z-index":_z-(-1)});
			
	_tit.mousedown(function(e){
		_mv=true;
		_x=e.pageX-parseInt(_obj.css("left"));//鑾峰緱宸﹁竟浣嶇疆
		_y=e.pageY-parseInt(_obj.css("top"));//鑾峰緱涓婅竟浣嶇疆
		_obj.css({	"z-index":_z-(-1)}).fadeTo(50,.5);//鐐瑰嚮鍚庡紑濮嬫嫋鍔ㄥ苟閫忔槑鏄剧ず
	});
	_tit.mouseup(function(e){
		_mv=false;
		_obj.fadeTo("fast",1);//鏉惧紑榧犳爣鍚庡仠姝㈢Щ鍔ㄥ苟鎭㈠鎴愪笉閫忔槑				 
	
	});
	
	$(document).mousemove(function(e){
		if(_mv){
			var x=e.pageX-_x;//绉诲姩鏃舵牴鎹紶鏍囦綅缃绠楁帶浠跺乏涓婅鐨勭粷瀵逛綅缃?			if(x<=0){x=0};
			x=Math.min($(document).width()-_wid,x)-5;
			var y=e.pageY-_y;
			if(y<=0){y=0};
			y=Math.min($(document).height()-_hei,y)-5;
			_obj.css({
				top:y,left:x
			});//鎺т欢鏂颁綅缃?		}
	});

			_cls.live("click",function(){
			$("#maskLayer").remove();
			_obj.hide();
	});
			
	$('<div id="maskLayer"></div>').appendTo("body").css({
		"background":"#000","opacity":".4","top":0,"left":0,"position":"absolute","zIndex":"8000"
	});
	reModel();
	$(window).bind("resize",function(){reModel();});
	$(document).keydown(function(event) {
		if (event.keyCode == 27) {
			$("#maskLayer").remove();
			_obj.hide();
		}
	});
	function reModel(){
		var b = docE? docE : document.body,
		height = b.scrollHeight > b.clientHeight ? b.scrollHeight : b.clientHeight,
		width = b.scrollWidth > b.clientWidth ? b.scrollWidth : b.clientWidth;
		$("#maskLayer").css({
			"height": height,"width": width
		});
	};
}