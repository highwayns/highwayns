function index(dir,templatedir)
{
	$(".lazyload div img").lazyload({ placeholder: templatedir+"images/index/84.gif", effect:"fadeIn" });
	$(".banner").KinSlideshow({
			moveStyle:"up",
			mouseEvent:"mouseover",
			isHasTitleFont:false,
			isHasTitleBar:false,
			btn:{btn_bgColor:"#FFFFFF",btn_bgHoverColor:"#1072aa",btn_fontColor:"#000000",btn_fontHoverColor:"#FFFFFF",btn_borderColor:"#cccccc",btn_borderHoverColor:"#1188c0",btn_borderWidth:0}
		});
	$("#index-search-button").click(function()
	{
		index_search_location();
	});
	function index_search_location()
	{
		$("body").append('<div id="pageloadingbox">页面加载中....</div><div id="pageloadingbg"></div>');
		$("#pageloadingbg").css("opacity", 0.5);

		 var sotype=$("#topsotype").val();
	 	if(sotype==1){
	 		var sotype_code = "QS_jobslist";
	 	}else{
	 		var sotype_code = "QS_resumelist";
	 	}
	 	var patrn=/^(请输入关键字)/i; 
		var key=$("#index-search-key").val();
		if (patrn.exec(key))
		{
		$("#index-search-key").val('');
		key='';
		}
		$.get(dir+"plus/ajax_search_location.php", {"act":sotype_code,"key":key},
			function (data,textStatus)
			 {
				 window.location.href=data;
			 }
		);
	}
	$("#index-search-key").focus(function()
	{
	 var patrn=/^(请输入关键字)/i; 
	 var key=$(this).val();
		if (patrn.exec(key))
		{
		$(this).css('color','').val('');
		} 
		$('input[id="index-search-key"]').keydown(function(e)
		{
		if(e.keyCode==13){
	   index_search_location()
		}
		});
	});
	$.get(dir+"plus/ajax_common.php", {"act":"ajaxcomlist","comrow":"14","jobrow":"3","showtype":"category","categoryid":$(".jobs_area .category_wrap ul li").first().attr('id')},
		function (data,textStatus)
		{	
			$(".jobs_content ul").html(data);
			$.joblisttip(".comtip",dir+"plus/ajax_common.php?act=joblisttip","载入中...",'comvtipshow');
		}
	);
	//下部左边栏点击效果
	$(".jobs_area .category_wrap ul li").click(function(){
		var idx=$(this).index(".jobs_area .category_wrap ul li");
		$(this).addClass("select").siblings().removeClass("select");
		//$(".tabbox>div").eq(idx).show();
		// $(".jobs_content>ul").eq(idx).show().siblings().hide();
		// $(".jobs_content").show().css('opacity',0.8);
		$.get(dir+"plus/ajax_common.php", {"act":"ajaxcomlist","comrow":"14","jobrow":"3","showtype":"category","categoryid":$(this).attr('id')},
			function (data,textStatus)
			{	
				$(".jobs_content ul").html(data);
				$.joblisttip(".comtip",dir+"plus/ajax_common.php?act=joblisttip","载入中...",'comvtipshow');
			}
		);		
	});
	// 职位选项卡
	$(".topjobs").click(function(){
		$(".topjobs").removeClass("active");
		$(this).addClass("active");
		$(".list_content ul").css("display","none");
		$("."+$(this).attr("listname")).css("display","block");
	});
	//选项卡切换
	$(".nav_item>li").each(function(){
		$(this).click(function(){
			$(this).addClass("active");
			$(this).siblings("li").removeClass("active");
			var bull_index = $(".nav_item>li").index(this);
			$(".bulletin>div.bull_content").eq(bull_index).show().siblings().not( ".bulletin_nav" ).hide();
		})
	});
	//职位简历切换点击事件
	$(".selemenu").click(function(){
		var txt = $(this).text();
		if (txt == "简历") {
			$("#topsotype").val("2");
			$(".seletxt").text("简历");
			$(".selemenu").text("职位");
			$(".selemenu").hide();
		}else{
			$("#topsotype").val("1");
			$(".seletxt").text("职位");
			$(".selemenu").text("简历");
			$(".selemenu").hide();
		};
	});
	// 职位简历切换
	$("#selectbox").hover(function(){
		$(this).find(".selemenu").show();
	}, function(){
		$(this).find(".selemenu").hide();
	});
}
function get_right_menu(arr){
	$(".leftMenu li").hover(function(){
		var liwidth=$(this).width();
		var leftMenuH=$(".leftMenu").height()+43;
		// 读取数据==显示
		var html = '<div class="show">';
		subclass = arr[$(this).attr("id")];
		if(subclass){
			html += MakeLi(subclass,$(this).attr("id"),arr);
		}
		html += '</div>';
		$(".show").html(html);		
		
		$(this).addClass("select").siblings().removeClass("select");
		//将html写入 show div中---先执行empty()
		$(".leftmenu_box").empty();
		$(".leftmenu_box").append(html).css({"top":"0","left":"220px","display":"block","overflow":"auto"});
		
	});
	$(".left").mouseleave(function(){
		$(".leftMenu li").removeClass("select");
		$(".leftmenu_box").css("display","none");
	});
}
function MakeLi(val,pid,top_arr)
{
			if (val=="")return false;
			arr=val.split("|");
			var htmlstr='';
				for (x in arr)
				{
				 var v=arr[x].split(",");
				thirdclass = top_arr[v[0]];
				htmlstr+='<div class="showbox"><div class="fl"><a target="_blank" title="'+v[1]+'" href="jobs/jobs-list.php?key=&jobcategory='+pid+'.'+v[0]+'.0">'+v[1]+'</a></div>';
				if(thirdclass){
					htmlstr+='<ul class="fr">';
					htmlstr += Make_Third_Li(thirdclass,v[0],pid);
					htmlstr+='</ul>';
				}
				htmlstr+='<div class="clear"></div></div>';
				}
			return htmlstr; 
}
function Make_Third_Li(val1,pid,gid)
{
			if (val1=="")return false;
			arr1=val1.split("|");
			var htmlstr1='';
				for (x1 in arr1)
				{
				 var v1=arr1[x1].split(",");
				htmlstr1+='<li><a target="_blank" title="'+v1[1]+'" href="jobs/jobs-list.php?key=&jobcategory='+gid+'.'+pid+'.'+v1[0]+'">'+v1[1]+'</a></li>';
				}
			return htmlstr1; 
}