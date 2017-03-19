/**
 * Base class of Drag
 * @example:
 * Drag.init( header_element, element );
 */
var Drag = {
	// 瀵硅繖涓猠lement鐨勫紩鐢紝涓€娆″彧鑳芥嫋鎷戒竴涓狤lement
	obj: null , 
	/**
	 * @param: elementHeader	used to drag..
	 * @param: element			used to follow..
	 */
	init: function(elementHeader, element) {
		// 灏?start 缁戝畾鍒?onmousedown 浜嬩欢锛屾寜涓嬮紶鏍囪Е鍙?start
		elementHeader.onmousedown = Drag.start;
		// 灏?element 瀛樺埌 header 鐨?obj 閲岄潰锛屾柟渚?header 鎷栨嫿鐨勬椂鍊欏紩鐢?		elementHeader.obj = element;
		// 鍒濆鍖栫粷瀵圭殑鍧愭爣锛屽洜涓轰笉鏄?position = absolute 鎵€浠ヤ笉浼氳捣浠€涔堜綔鐢紝浣嗘槸闃叉鍚庨潰 onDrag 鐨勬椂鍊?parse 鍑洪敊浜?		if(isNaN(parseInt(element.style.left))) {
			element.style.left = "0px";
		}
		if(isNaN(parseInt(element.style.top))) {
			element.style.top = "0px";
		}
		// 鎸備笂绌?Function锛屽垵濮嬪寲杩欏嚑涓垚鍛橈紝鍦?Drag.init 琚皟鐢ㄥ悗鎵嶅府瀹氬埌瀹為檯鐨勫嚱鏁?		element.onDragStart = new Function();
		element.onDragEnd = new Function();
		element.onDrag = new Function();
	},
	// 寮€濮嬫嫋鎷界殑缁戝畾锛岀粦瀹氬埌榧犳爣鐨勭Щ鍔ㄧ殑 event 涓?	start: function(event) {
		var element = Drag.obj = this.obj;
		// 瑙ｅ喅涓嶅悓娴忚鍣ㄧ殑 event 妯″瀷涓嶅悓鐨勯棶棰?		event = Drag.fixE(event);
		// 鐪嬬湅鏄笉鏄乏閿偣鍑?		if(event.which != 1){
			// 闄や簡宸﹂敭閮戒笉璧蜂綔鐢?			return true ;
		}
		// 鍙傜収杩欎釜鍑芥暟鐨勮В閲婏紝鎸備笂寮€濮嬫嫋鎷界殑閽╁瓙
		element.onDragStart();
		// 璁板綍榧犳爣鍧愭爣
		element.lastMouseX = event.clientX;
		element.lastMouseY = event.clientY;
		// 缁戝畾浜嬩欢
		document.onmouseup = Drag.end;
		document.onmousemove = Drag.drag;
		return false ;
	}, 
	// Element姝ｅ湪琚嫋鍔ㄧ殑鍑芥暟
	drag: function(event) {
		event = Drag.fixE(event);
		if(event.which == 0 ) {
		 	return Drag.end();
		}
		// 姝ｅ湪琚嫋鍔ㄧ殑Element
		var element = Drag.obj;
		// 榧犳爣鍧愭爣
		var _clientX = event.clientY;
		var _clientY = event.clientX;
		// 濡傛灉榧犳爣娌″姩灏变粈涔堥兘涓嶄綔
		if(element.lastMouseX == _clientY && element.lastMouseY == _clientX) {
			return	false ;
		}
		// 鍒氭墠 Element 鐨勫潗鏍?		var _lastX = parseInt(element.style.top);
		var _lastY = parseInt(element.style.left);
		// 鏂扮殑鍧愭爣
		var newX, newY;
		// 璁＄畻鏂扮殑鍧愭爣锛氬師鍏堢殑鍧愭爣+榧犳爣绉诲姩鐨勫€煎樊
		newX = _lastY + _clientY - element.lastMouseX;
		newY = _lastX + _clientX - element.lastMouseY;
		// 淇敼 element 鐨勬樉绀哄潗鏍?		element.style.left = newX + "px";
		element.style.top = newY + "px";
		// 璁板綍 element 鐜板湪鐨勫潗鏍囦緵涓嬩竴娆＄Щ鍔ㄤ娇鐢?		element.lastMouseX = _clientY;
		element.lastMouseY = _clientX;
		// 鍙傜収杩欎釜鍑芥暟鐨勮В閲婏紝鎸傛帴涓?Drag 鏃剁殑閽╁瓙
		element.onDrag(newX, newY);
		return false;
	},
	// Element 姝ｅ湪琚噴鏀剧殑鍑芥暟锛屽仠姝㈡嫋鎷?	end: function(event) {
		event = Drag.fixE(event);
		// 瑙ｉ櫎浜嬩欢缁戝畾
		document.onmousemove = null;
		document.onmouseup = null;
		// 鍏堣褰曚笅 onDragEnd 鐨勯挬瀛愶紝濂界Щ闄?obj
		var _onDragEndFuc = Drag.obj.onDragEnd();
		// 鎷栨嫿瀹屾瘯锛宱bj 娓呯┖
		Drag.obj = null ;
		return _onDragEndFuc;
	},
	// 瑙ｅ喅涓嶅悓娴忚鍣ㄧ殑 event 妯″瀷涓嶅悓鐨勯棶棰?	fixE: function(ig_) {
		if( typeof ig_ == "undefined" ) {
			ig_ = window.event;
		}
		if( typeof ig_.layerX == "undefined" ) {
			ig_.layerX = ig_.offsetX;
		}
		if( typeof ig_.layerY == "undefined" ) {
			ig_.layerY = ig_.offsetY;
		}
		if( typeof ig_.which == "undefined" ) {
			ig_.which = ig_.button;
		}
		return ig_;
	}
};