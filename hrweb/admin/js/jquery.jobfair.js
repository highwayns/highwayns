//截取字符
function limit(objString,num)
{
	if (num==0)
	{
		return objString;
	}
	var objLength =objString.length;
	if(objLength > num){ 
	return objString.substring(0,num) + "...";
	} 
	return objString;
}
//打开行业(此函数仅限创建招聘会参会行业)
function OpentradeLayer(click_obj,input,input_cn,input_txt,showid,strlen)
{
	$(click_obj).click(function()
	{
			$(this).blur();
			$(this).parent("div").before("<div class=\"menu_bg_layer\"></div>");
			$(".menu_bg_layer").height($(document).height());
			$(".menu_bg_layer").css({ width: $(document).width(), position: "absolute",left:"0", top:"0","z-index":"0","background-color":"#000000"});
			//$(".menu_bg_layer").css("opacity",10);
			$(showid+" .OpenFloatBoxBg").css("opacity", 0.2);
			$(showid).show();			
			$(showid+" .OpenFloatBox").css({"left":($(document).width()-$(showid+" .OpenFloatBox").width())/2,"top":"150"});
			SetBoxBg(showid);
			$(showid+"  label").hover(function()	{$(this).css("background-color","#E3F0FF");},function(){	$(this).css("background-color","");});
				$(showid+"  label").unbind().click(function(){
						
						$(showid+"  :checkbox").parent().css("color","");
						$(showid+"  :checkbox[checked]").parent().css("color","#009900");
						
						
				});
		
	});
	//确定选择
	$(showid+" .Set").click(function()
	{
			var a_cn=new Array();
			var a_id=new Array();
			$(showid+" :checkbox[checked]").each(function(index){
			a_cn[index]=$(this).attr("title");
			a_id[index]=$(this).attr("value");
			});
			$(input_cn).val(limit(a_cn.join(","),strlen));
			$(input_txt).html(limit(a_cn.join("+"),strlen));
			if ($(input_cn).val()=="")$(input_cn).val("");
			$(input).val(a_id.join(","));
			 DialogClose(showid);
	});
	//设置阴影
	function SetBoxBg(showid)
	{
				var FloatBoxWidth=$(showid+" .OpenFloatBox").width();
				var FloatBoxHeight=$(showid+" .OpenFloatBox").height();
				var FloatBoxLeft=$(showid+" .OpenFloatBox").offset().left;
				var FloatBoxTop=$(showid+" .OpenFloatBox").offset().top;
				$(showid+" .OpenFloatBoxBg").css({display:"block",width:(FloatBoxWidth+12)+"px",height:(FloatBoxHeight+12)+"px"});
				$(showid+" .OpenFloatBoxBg").css({left:(FloatBoxLeft-5)+"px",top:(FloatBoxTop-5)+"px"});
	}
	//关闭
	$(showid+" .OpenFloatBox .DialogClose").hover(function(){$(this).addClass("spanhover")},function(){$(this).removeClass("spanhover")});
	$(showid+" .DialogClose").click(function(){DialogClose(showid);});
	function DialogClose(showid)
	{
		$(".menu_bg_layer").hide();
		$(showid).hide();
	}
	
}