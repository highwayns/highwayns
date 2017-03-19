/* 
* QSdialog 1.0
* http://www.74cms.com/
* Date: 2011-5-15 
* Requires jQuery
*/ 
(function($) {   
$.fn.QSdialog=function(options){
	var defaults = {
    DialogAddObj:"body",	
	DialogAddType:"append",	
	DialogClosed:"关闭",
	DialogTitle:"系统提示",
	DialogWidth:"420",
	DialogHeight:"auto",
	DialogCssName:"",
	DialogContent:"",
	DialogContentType:"text"
   }
    var options = $.extend(defaults,options);
	var AddObj=options.DialogAddObj;
	var temp_float=new String;
	temp_float="<div class=\"FloatBg\"  style=\"height:"+$(document).height()+"px;width:"+$(document).width()+"px;filter:alpha(opacity=0);opacity:0;\"></div>";
	temp_float+="<div class=\"FloatBox\">";
	temp_float+="<div class=\"Box\">";
	temp_float+="<div class=\"title\"><h4></h4><span class=\"DialogClose\" title=\"关闭\"></span></div>";
	temp_float+="<div class=\"content link_lan\"><div class=\"wait\"></div></div>";
	temp_float+="</div>";
	temp_float+="</div>";
	if (AddObj=="body")
	{
	$("body").append(temp_float);	
	}
	else
	{
		$(AddObj).html(temp_float);
	}
	$(AddObj+" .FloatBox .title h4").html(options.DialogTitle);
	var content=options.DialogContent;
	switch(options.DialogContentType){
	case "url":
	var content_array=content.split("?");
	$.ajax({
    type:content_array[0],
    url:content_array[1],
    data:content_array[2],
	error:function(){
	$(AddObj+" .FloatBox .content").html("error...");
	},
    success:function(html){
	//alert(html);
    $(AddObj+" .FloatBox .content").html(html);
    }
  	});
  	break;
  	case "text":
	$(AddObj+" .FloatBox .content").html(content);
	break;
	case "id":
	$(AddObj+" .FloatBox .content").html($(content).html());
	break;
	case "iframe":
	$(AddObj+" .FloatBox .content").html("<iframe src=\""+content+"\" width=\"100%\" height=\""+(parseInt(height)-30)+"px"+"\" scrolling=\"auto\" frameborder=\"0\" marginheight=\"0\" marginwidth=\"0\"></iframe>");
	}
	
function DialogClose()
{
	$(AddObj+" .FloatBg").hide();
	$(AddObj+" .FloatBox").hide();
}
$(".DialogClose").click(function(){DialogClose();});	
	this.click(function()
	{		
		$(AddObj+" .FloatBg").show().css("opacity", 0.1);
		var width=options.DialogWidth=="auto"?"auto":options.DialogWidth+"px";
		var height=options.DialogHeight=="auto"?"auto":options.DialogHeight+"px";
		$(AddObj+" .FloatBox").css({display:"block",left:(($(document).width())/2-(parseInt(width)/2))+"px",top:($(document).scrollTop()+120)+"px",width:width,height:height});
		$(AddObj+" .FloatBox .DialogClose").hover(function(){$(this).addClass("spanhover")},function(){$(this).removeClass("spanhover")});
		//alert(options.DialogWidth);
	});
}
})(jQuery); 