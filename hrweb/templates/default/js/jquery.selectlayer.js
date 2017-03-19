//职位
function OpenCategory(objid,input_cn,input,dir,strlen,get)
{
	$(objid).unbind().click(function()
	{
			var clickobj=$(this);
			clickobj.blur();
			clickobj.parent("div").before("<div class=\"menu_bg_layer\"></div>");
			$(".menu_bg_layer").height($(document).height());
			$(".menu_bg_layer").css({ width: $(document).width(), position: "absolute",left:"0", top:"0","z-index":"0","background-color":"#000000"});
			$(".menu_bg_layer").css("opacity",0);
			var showid="#___catjobs";
			var showidval="___catjobs";
			var html='';
			var isempty=$(showid).html();
			if(isempty==null)
			{
				html+="<div id=\""+showidval+"\">";
				html+="<div class=\"OpenFloatBoxBg\" ></div>";
				html+="<div class=\"OpenFloatBox\">";
				html+="<div class=\"title\"><h4>请选择职位类别</h4><div class=\"DialogClose\" title=\"关闭\"></div></div>";
				html+="<div class=\"tip\">可多选，最多选择5项</div>";
				html+="<div class=\"content link_lan\"><div class=\"wait\"></div>";
				html+="</div>";
				html+="</div></div>";
				clickobj.parent("div").append(html);
				$(showid+" .OpenFloatBox").css("width","200");
				SetBoxPoint(showid);
				var appendli=false;
			}
			if(appendli==false)
			{
				$.getScript(dir+"data/cache_classify.js", function(){	
					html='';
					html+="<div class=\"txt\">";
					for(var i=0;i<QS_jobs_parent.length;i++)
					{
						arr  =QS_jobs_parent[i].split(",");
						html+="<div class=\"item\" id=\""+arr[0]+"\">";
						html+="<label title=\""+arr[1]+"\" class=\"titem\" >";
						html+="<input  type=\"checkbox\" value=\""+arr[0]+"\"  title=\""+arr[1]+"\" class=\"b\" />"+arr[1]+"";
						html+="</label>";
						html+="<div class=\"sitem\"></div>";
						html+="</div>";
					}
					html+="<div class=\"clear\"></div>";
					html+="</div>";					
					html+="<div class=\"txt\"><div class=\"selecteditem\"></div></div>";	
					html+="<div class=\"txt\">";
					html+="<div align=\"center\"><input type=\"button\"  class=\"but80 Set\" value=\"确定\" /></div>";
					html+="</div>";
					$(showid+" .content").html(html);
					$(showid+" .OpenFloatBox").css("width","650");
					SetBoxPoint(showid);
					SetBoxBg(showid);
					addhover(showid,input_cn,input,strlen,get);
				});
			}
			else
			{
				addhover(showid,input_cn,input,strlen,get);
			}
			//关闭	
		 	$(showid+" .DialogClose").unbind().hover(function(){$(this).addClass("spanhover")},function(){$(this).removeClass("spanhover")});
			$(showid+" .DialogClose").click(function(){DialogClose(showid);});
			//设置背景边框
			$(showid).show();			
			SetBoxBg(showid);
			///---
			function addhover(showid,input_cn,input,strlen,get)
			{		
				$(showid+" .item").unbind().hover(
				function(){
				$(this).find(".titem").addClass("titemhover");				
				var strclass=QS_jobs[$(this).attr("id")];
				var pid=$(this).attr("id");
				if (strclass)
				{
					$(this).find(".sitem").css("display","block");
					if ($(this).find(".sitem").html()=="")
					{
					$(this).find(".sitem").html(MakeLi(strclass,pid));
					}
				}
					$(showid+" .OpenFloatBox label").unbind().click(function()
					{
						if ($(this).attr("title"))
						{
								if ($(this).find(":checkbox").attr('checked'))
								{
								$(this).next().find(":checkbox").attr('checked',true);
								}
								else
								{
								$(this).next().find(":checkbox").attr('checked',false);
								}
						}
						else
						{
							if ($(this).parent().find(":checkbox[checked]").length>0)
							{
							$(this).parent().prev().find(":checkbox").attr('checked',false);
							}
						}
					Copycheckbox(showid);
					});	
				}
				,
				function()
				{
				$(showid+" .titem").removeClass("titemhover");
				$(showid+" .sitem").css("display","none");
				}
				);
				//确定选择
				$(showid+" .Set").unbind().click(function()
				{
						SetInput(showid,input_cn,input,strlen);
				});
				//get恢复
				var getadd=$(showid+" .get").length;
				if (get && getadd==0)
				{
							jobcategory=get.split("-");
							for( var i=0;i<jobcategory.length;++i)
							{
								var catid=jobcategory[i].split(".");
								var jscat=QS_jobs[catid[0]];
								if (jscat)
								{
									var html=MakeLi(jscat,catid[0]);		
									if ($(showid+" .item[id='"+catid[0]+"'] .sitem").html()=="")
									{
									$(showid+" .item[id='"+catid[0]+"'] .sitem").html(html);
									}
								}
								if (catid[1]=="0")
								{
								$(showid+"  :checkbox[value="+catid[0]+"]").attr("checked",true);
								$(showid+" .item[id='"+catid[0]+"'] .sitem :checkbox").attr("checked",true);
								}
								else
								{
								$(showid+" .sitem :checkbox[value='"+catid[1]+"']").attr("checked",true);
								}
							}
					$(showid).append("<div class=\"get\"></div>");				
					Copycheckbox(showid);
				}
				//end
				 
			}
	});	
}
//地区
function OpenCity(objid,input_cn,input,dir,strlen,get)
{
	$(objid).unbind().click(function()
	{
			var clickobj=$(this);
			clickobj.blur();
			clickobj.parent("div").before("<div class=\"menu_bg_layer\"></div>");
			$(".menu_bg_layer").height($(document).height());
			$(".menu_bg_layer").css({ width: $(document).width(), position: "absolute",left:"0", top:"0","z-index":"0","background-color":"#000000"});
			$(".menu_bg_layer").css("opacity",0);
			var showid="#___catcity";
			var showidval="___catcity";
			var html='';
			var isempty=$(showid).html();
			if(isempty==null)
			{
				html+="<div id=\""+showidval+"\">";
				html+="<div class=\"OpenFloatBoxBg\" ></div>";
				html+="<div class=\"OpenFloatBox\">";
				html+="<div class=\"title\"><h4>请选择地区</h4><div class=\"DialogClose\" title=\"关闭\"></div></div>";
				html+="<div class=\"tip\">可多选，最多选择5项</div>";
				html+="<div class=\"content link_lan\"><div class=\"wait\"></div>";
				html+="</div>";
				html+="</div></div>";
				clickobj.parent("div").append(html);
				$(showid+" .OpenFloatBox").css("width","200");
				SetBoxPoint(showid);
				var appendli=false;
			}
			if(appendli==false)
			{
				$.getScript(dir+"data/cache_classify.js", function(){	
					html='';
					html+="<div class=\"txt\">";
					for(var i=0;i<QS_city_parent.length;i++)
					{
						arr  =QS_city_parent[i].split(",");
						html+="<div class=\"item\" id=\""+arr[0]+"\">";
						html+="<label title=\""+arr[1]+"\" class=\"titem\" >";
						html+="<input  type=\"checkbox\" value=\""+arr[0]+"\"  title=\""+arr[1]+"\" class=\"b\" />"+arr[1]+"";
						html+="</label>";
						html+="<div class=\"sitem\"></div>";
						html+="</div>";
					}
					html+="<div class=\"clear\"></div>";
					html+="</div>";					
					html+="<div class=\"txt\"><div class=\"selecteditem\"></div></div>";	
					html+="<div class=\"txt\">";
					html+="<div align=\"center\"><input type=\"button\"  class=\"but80 Set\" value=\"确定\" /></div>";
					html+="</div>";
					$(showid+" .content").html(html);
					$(showid+" .OpenFloatBox").css("width","650");
					SetBoxPoint(showid);
					SetBoxBg(showid);
					addhover(showid,input_cn,input,strlen,get);
				});
			}
			else
			{
				addhover(showid,input_cn,input,strlen,get);
			}
			//关闭	
		 	$(showid+" .DialogClose").unbind().hover(function(){$(this).addClass("spanhover")},function(){$(this).removeClass("spanhover")});
			$(showid+" .DialogClose").click(function(){DialogClose(showid);});
			//设置背景边框
			$(showid).show();			
			SetBoxBg(showid);
			///---
			function addhover(showid,input_cn,input,strlen,get)
			{
				$(showid+" .item").unbind().hover(
				function(){
				$(this).find(".titem").addClass("titemhover");				
				var strclass=QS_city[$(this).attr("id")];
				var pid=$(this).attr("id");
				if (strclass)
				{
					$(this).find(".sitem").css("display","block");
					if ($(this).find(".sitem").html()=="")
					{
					$(this).find(".sitem").html(MakeLi(strclass,pid));
					}
				}
					$(showid+" .OpenFloatBox label").unbind().click(function()
					{
						if ($(this).attr("title"))
						{
								if ($(this).find(":checkbox").attr('checked'))
								{
								$(this).next().find(":checkbox").attr('checked',true);
								}
								else
								{
								$(this).next().find(":checkbox").attr('checked',false);
								}
						}
						else
						{
							if ($(this).parent().find(":checkbox[checked]").length>0)
							{
							$(this).parent().prev().find(":checkbox").attr('checked',false);
							}
						}
					Copycheckbox(showid);
					});	
				}
				,
				function()
				{
				$(showid+" .titem").removeClass("titemhover");
				$(showid+" .sitem").css("display","none");
				}
				);
				//确定选择
				$(showid+" .Set").unbind().click(function()
				{
						SetInput(showid,input_cn,input,strlen);
				});
				//get恢复
				var getadd=$(showid+" .get").length;
				if (get && getadd==0)
				{
							city=get.split("-");
							for( var i=0;i<city.length;++i)
							{
								var catid=city[i].split(".");
								var jscat=QS_city[catid[0]];
								if (jscat)
								{
									var html=MakeLi(jscat,catid[0]);		
									if ($(showid+" .item[id='"+catid[0]+"'] .sitem").html()=="")
									{
									$(showid+" .item[id='"+catid[0]+"'] .sitem").html(html);
									}
								}
								if (catid[1]=="0")
								{
								$(showid+"  :checkbox[value="+catid[0]+"]").attr("checked",true);
								$(showid+" .item[id='"+catid[0]+"'] .sitem :checkbox").attr("checked",true);
								}
								else
								{
								$(showid+" .sitem :checkbox[value='"+catid[1]+"']").attr("checked",true);
								}
							}
					$(showid).append("<div class=\"get\"></div>");
					Copycheckbox(showid);
				}
				//end
			}
	});	
}
//关闭
function DialogClose(showid)
{
	$(".menu_bg_layer").detach();
	$(showid).hide();
}
//背景边框
function SetBoxBg(showid)
{
	$(showid+" .OpenFloatBoxBg").css("opacity", 0.2);
	var FloatBoxWidth=$(showid+" .OpenFloatBox").width();
	var FloatBoxHeight=$(showid+" .OpenFloatBox").height();
	var FloatBoxLeft=$(showid+" .OpenFloatBox").offset().left;
	var FloatBoxTop=$(showid+" .OpenFloatBox").offset().top;
	$(showid+" .OpenFloatBoxBg").css({display:"block",width:(FloatBoxWidth+12)+"px",height:(FloatBoxHeight+12)+"px"});
	$(showid+" .OpenFloatBoxBg").css({left:(FloatBoxLeft-5)+"px",top:(FloatBoxTop-5)+"px"});
}
function SetBoxPoint(showid)
{
	var top=($(document).scrollTop())+120;
	var left=($(document).width()-$(showid+" .OpenFloatBox").width())/2;
	$(showid+" .OpenFloatBox").css({"left":left,"top":top,"z-index":"999"});
}
//生成小类
function MakeLi(val,pid)
{
			if (val=="")return false;
			arr=val.split("|");
			var htmlstr='';
				for (x in arr)
				{
				 var v=arr[x].split(",");
				htmlstr+="<label><input type=\"checkbox\" value=\""+v[0]+"\" title=\""+v[1]+"\" class=\"s\" id=\""+pid+"\"/>"+v[1]+"</label><br/>";
				}
			return htmlstr; 
}
//拷贝
function Copycheckbox(catbox)
{
					var htmlstr='&nbsp;&nbsp;&nbsp;已经选择分类：<span class=\"empty\">[清空已选]</span><br/>';
					$(catbox+" .item :checkbox[checked][class='b']").each(function(index){
					htmlstr+="<label><input class=\"b\"  type=\"checkbox\" value=\""+$(this).attr("value")+"\" title=\""+$(this).attr("title")+"\" checked/>"+$(this).attr("title")+"</label>";
					})
					$(catbox+" .item :checkbox[checked][class='s']").each(function(index){
					 if ($(this).parent().parent().prev().find(":checkbox").attr('checked')==false)
					 {						 
					htmlstr+="<label><input class=\"s\"  type=\"checkbox\" id=\""+$(this).attr("id")+"\" value=\""+$(this).attr("value")+"\" title=\""+$(this).attr("title")+"\" checked/>"+$(this).attr("title")+"</label>";
					 }
					})
					htmlstr+="<div class=\"clear\"></div>";
					$(catbox+" .selecteditem").html(htmlstr);
					if ($(catbox+" .item :checkbox[checked]").length>0)
					{
						$(catbox+" .selecteditem").css("display","block");
					}
					else
					{
						$(catbox+" .selecteditem").css("display","none");
					}
					//已选项目绑定click
					$(catbox+" .selecteditem :checkbox").unbind().click(function()
					{
						var selval=$(this).val();
							$(catbox+" .item :checkbox[checked]").each(function()
							{
								if ($(this).val()==selval)
								{
									$(this).attr("checked",false);
									if ($(this).attr("class")=="b")
									{
										$(this).parent().next().find(":checkbox").attr("checked",false);
									}									
									//重新克隆
									Copycheckbox(catbox);
								}	
							})
					});
					$(catbox+" .OpenFloatBox .item label :checkbox").parent().css("color","");
					$(catbox+" .OpenFloatBox .item label :checkbox[checked]").parent().css("color","#FF6600");
					$(catbox+" .OpenFloatBox .sitem :checkbox[checked]").each(function(index){
					 	$(this).parent().parent().prev().css("color","#FF6600");
					});
					SetBoxBg(catbox);
					//清空
					$(catbox+" .selecteditem .empty").unbind().click(function()
					{
						$(catbox+" .selecteditem").css("display","none");
						$(catbox+" .selecteditem").html("");
						$(catbox+" :checkbox[checked]").parent().css("color","");
						$(catbox+" :checkbox[checked]").parent().parent().prev().css("color","");
						$(catbox+" :checkbox[checked]").attr('checked',false);							
						SetBoxBg(catbox);
					});	
}
//设置表单
function SetInput(showid,input_cn,input,strlen)
{
	var a_cn=new Array();
	var a_id=new Array();
	var i=0;
	if ($(showid+" .OpenFloatBox .selecteditem :checkbox[checked]").length>5)
		{
			alert("不能超过5个选项");
			return false;
		}
		$(showid+" .OpenFloatBox .selecteditem :checkbox[checked]").each(function(index)
		{
			a_cn[index]=$(this).attr("title");	
			if ($(this).attr("class")=="b")
			{
									a_id[i]=$(this).val()+".0";
			}
			else
			{							
				a_id[i]=$(this).attr("id")+"."+$(this).val();
			}
			i++;
		});
		$(input_cn).val(limit(a_cn.join("+"),strlen));
		if ($(input_cn).val()=="")$(input_cn).val("未选择");
		$(input).val(a_id.join("-"));
		DialogClose(showid);
}
function OpenCategoryLayer(objid,showid,input_cn,input,QSarr,strlen)
{
	$(objid).click(function()
	{
			$(this).blur();
			$(this).parent("div").before("<div class=\"menu_bg_layer\"></div>");
			$(".menu_bg_layer").height($(document).height());
			$(".menu_bg_layer").css({ width: $(document).width(), position: "absolute",left:"0", top:"0","z-index":"0","background-color":"#000000"});
			$(".menu_bg_layer").css("opacity",0);
			$(showid+" .OpenFloatBoxBg").css("opacity", 0.2);
			$(showid).show();			
			$(showid+" .OpenFloatBox").css({"left":($(document).width()-$(showid+" .OpenFloatBox").width())/2,"top":"120","z-index":"999"});
			SetBoxBg(showid);
			$(showid+" .item").unbind().hover(
				function(){
				$(this).find(".titem").addClass("titemhover");				
				var strclass=QSarr[$(this).attr("id")];
				var pid=$(this).attr("id");
				if (strclass)
				{
					//alert("有小类");
					$(this).find(".sitem").css("display","block");
					if ($(this).find(".sitem").html()=="")
					{
					$(this).find(".sitem").html(MakeLi(strclass,pid));//生成LI
					}
				}
					$(showid+" .OpenFloatBox label").unbind().click(function()
					{
						if ($(this).attr("title"))
						{
							if ($(this).find(":checkbox").attr('checked'))
							{
							$(this).next().find(":checkbox").attr('checked',true);

							}
							else
							{
							$(this).next().find(":checkbox").attr('checked',false);
							}
						}
						else
						{
							if ($(this).parent().find(":checkbox[checked]").length>0)
							{
								$(this).parent().prev().find(":checkbox").attr('checked',false);
							}
						}
						CopyItem(showid);
					});	
				},
				function(){
				$(this).find(".titem").removeClass("titemhover");
				$(this).find(".sitem").css("display","none");
				}
			);
			$(showid+" .OpenFloatBox .DialogClose").unbind().hover(function(){$(this).addClass("spanhover")},function(){$(this).removeClass("spanhover")});
			$(showid+" .DialogClose").click(function(){DialogClose(showid);});
			//确定选择
			$(showid+" .Set").unbind().click(function()
			{
					SetInput(showid,input_cn,input,strlen);
			});	
			//关闭
			function DialogClose(showid)
			{
				$(".menu_bg_layer").hide();
				$(showid).hide();
			}
			//设置表单
			function SetInput(showid,input_cn,input,strlen)
			{
					var a_cn=new Array();
					var a_id=new Array();
					var i=0;
					if ($(showid+" .OpenFloatBox .selecteditem :checkbox[checked]").length>5)
					{
						alert("不能超过5个选项");
						return false;
					}
					$(showid+" .OpenFloatBox .selecteditem :checkbox[checked]").each(function(index)
					{
					    a_cn[index]=$(this).attr("title");	
						if ($(this).attr("class")=="b")
						{
								a_id[i]=$(this).val()+".0";							
						}
						else
						{							
							a_id[i]=$(this).attr("id")+"."+$(this).val();
						}
							i++;
					});
					$(input_cn).val(limit(a_cn.join("+"),strlen));
					if ($(input_cn).val()=="")$(input_cn).val("未选择");
					$(input).val(a_id.join("-"));
					DialogClose(showid);
			}
		
	});	
}
//拷贝
function CopyItem(showid)
{
					var htmlstr='&nbsp;&nbsp;&nbsp;已经选择分类：<span class=\"empty\">[清空已选]</span><br/>';
					$(showid+" .item :checkbox[checked][class='b']").each(function(index){
					htmlstr+="<label><input class=\"b\"  type=\"checkbox\" value=\""+$(this).attr("value")+"\" title=\""+$(this).attr("title")+"\" checked/>"+$(this).attr("title")+"</label>";
					})
					$(showid+" .item :checkbox[checked][class='s']").each(function(index){
					 if ($(this).parent().parent().prev().find(":checkbox").attr('checked')==false)
					 {						 
					htmlstr+="<label><input class=\"s\"  type=\"checkbox\" id=\""+$(this).attr("id")+"\" value=\""+$(this).attr("value")+"\" title=\""+$(this).attr("title")+"\" checked/>"+$(this).attr("title")+"</label>";
					 }
					})
					htmlstr+="<div class=\"clear\"></div>";
					$(showid+" .selecteditem").html(htmlstr);
					if ($(showid+" .item :checkbox[checked]").length>0)
					{
						$(showid+" .selecteditem").css("display","block");
					}
					else
					{
						$(showid+" .selecteditem").css("display","none");
					}
					//已选项目绑定click
					$(showid+" .selecteditem :checkbox").unbind().click(function()
					{
						var selval=$(this).val();
							$(showid+" .item :checkbox[checked]").each(function()
							{
								if ($(this).val()==selval)
								{
									$(this).attr("checked",false);
									if ($(this).attr("class")=="b")
									{
										$(this).parent().next().find(":checkbox").attr("checked",false);
									}									
									//重新克隆
									CopyItem(showid);
								}	
							})
					});
					$(showid+" .OpenFloatBox .item label :checkbox").parent().css("color","");
					$(showid+" .OpenFloatBox .item label :checkbox[checked]").parent().css("color","#FF6600");
					$(showid+" .OpenFloatBox .sitem :checkbox[checked]").each(function(index){
					 	$(this).parent().parent().prev().css("color","#FF6600");
					});
					SetBoxBg(showid);
					//清空
					$(showid+" .selecteditem .empty").unbind().click(function()
					{
							$(showid+" .selecteditem").css("display","none");
							$(showid+" .selecteditem").html("");
							$(showid+" :checkbox[checked]").parent().css("color","");
							$(showid+" :checkbox[checked]").parent().parent().prev().css("color","");
							$(showid+" :checkbox[checked]").attr('checked',false);							
							SetBoxBg(showid);
					});	
}
//截取字符
function limit(objString,num)
{
	var objLength =objString.length;
	if(objLength > num){ 
	return objString.substring(0,num) + "...";
	} 
	return objString;
}
function OpentradeLayer(objid,input_cn,input,dir,strlen,get)
{
	$(objid).unbind().click(function()
	{
			var clickobj=$(this);
			clickobj.blur();
			clickobj.parent("div").before("<div class=\"menu_bg_layer\"></div>");
			$(".menu_bg_layer").height($(document).height());
			$(".menu_bg_layer").css({ width: $(document).width(), position: "absolute",left:"0", top:"0","z-index":"0","background-color":"#000000"});
			$(".menu_bg_layer").css("opacity",0);
			var showid="#___Trade";
			var showidval="___Trade";
			var html='';
			var isempty=$(showid).html();
			if(isempty==null)
			{
				html+="<div id=\""+showidval+"\">";
				html+="<div class=\"OpenFloatBoxBg\" ></div>";
				html+="<div class=\"OpenFloatBox\">";
				html+="<div class=\"title\"><h4>请选择行业</h4><div class=\"DialogClose\" title=\"关闭\"></div></div>";
				html+="<div class=\"tip\">可多选，最多选择5项</div>";
				html+="<div class=\"content link_lan\"><div class=\"wait\"></div>";
				html+="</div>";
				html+="</div></div>";
				clickobj.parent("div").append(html);
				$(showid+" .OpenFloatBox").css("width","200");
				SetBoxPoint(showid);
				var appendli=false;
			}
			if(appendli==false)
			{
				$.getScript(dir+"data/cache_classify.js", function(){	
					html='';
					html+="<div class=\"txt\">";
					for(var i=0;i<QS_trade.length;i++)
					{
						arr  =QS_trade[i].split(",");
						html+="<div class=\"item\" id=\""+arr[0]+"\">";
						html+="<label title=\""+arr[1]+"\" class=\"titem\" >";
						html+="<input  type=\"checkbox\" value=\""+arr[0]+"\"  title=\""+arr[1]+"\" class=\"b\" />"+arr[1]+"";
						html+="</label>";
						html+="<div class=\"sitem\"></div>";
						html+="</div>";
					}
					html+="<div class=\"clear\"></div>";
					html+="</div>";					
					html+="<div class=\"txt\"><div class=\"selecteditem\"></div></div>";	
					html+="<div class=\"txt\">";
					html+="<div align=\"center\"><input type=\"button\"  class=\"but80 Set\" value=\"确定\" /></div>";
					html+="</div>";
					$(showid+" .content").html(html);
					$(showid+" .OpenFloatBox").css("width","650");
					SetBoxPoint(showid);
					SetBoxBg(showid);
					addhover(showid,input_cn,input,strlen,get);
				});
			}
			else
			{
				addhover(showid,input_cn,input,strlen,get);
			}
			//关闭	
		 	$(showid+" .DialogClose").unbind().hover(function(){$(this).addClass("spanhover")},function(){$(this).removeClass("spanhover")});
			$(showid+" .DialogClose").click(function(){DialogClose(showid);});
			//设置背景边框
			$(showid).show();	
			SetBoxBg(showid);
			//
			function addhover(showid,input_cn,input,strlen,get)
			{
				$(showid+"  label").hover(function()	{$(this).css("background-color","#E3F0FF");},function(){	$(this).css("background-color","");});
				$(showid+"  label").unbind().click(function(){
						if ($(showid+" .content :checkbox[checked]").length>5)
						{
							alert("不能超过5个选项");
							$(this).attr("checked",false);
							return false;
						}
						else
						{
						$(showid+"  :checkbox").parent().css("color","");
						$(showid+"  :checkbox[checked]").parent().css("color","#FF6600");
						}
				});
				//确定选择
				$(showid+" .Set").unbind().click(function()
				{
						var a_cn=new Array();
						var a_id=new Array();
						$(showid+" :checkbox[checked]").each(function(index){
						a_cn[index]=$(this).attr("title");
						a_id[index]=$(this).attr("value");
						});
						$(input_cn).val(limit(a_cn.join("+"),strlen));
						if ($(input_cn).val()=="")$(input_cn).val("请选择");
						$(input).val(a_id.join("-"));
						 DialogClose(showid);
				});
				//get恢复
				var getadd=$(showid+" .get").length;
				if (get && getadd==0)
				{
						get_trade=get.split("-");
						for( var i=0;i<get_trade.length;++i)
						{
							$(showid+" :checkbox").each(function(index)
							{
								if ($(this).val()==get_trade[i]) $(this).attr("checked",true)		
							});
						}
						$(showid+"  :checkbox").parent().css("color","");
						$(showid+"  :checkbox[checked]").parent().css("color","#FF6600");
						$(showid).append("<div class=\"get\"></div>");
				}
				//end				
			}
	});
}
function menulist(objid,input_cn,input,dir,type)
{
	$(objid).unbind().click(function()
	{
			var clickobj=$(this);
			clickobj.blur();
			clickobj.parent("div").css("position","relative");
			clickobj.parent("div").before("<div class=\"menu_bg_layer\"></div>");			
			$(".menu_bg_layer").height($(document).height());
			$(".menu_bg_layer").css({ width: $(document).width(), position: "absolute",left:"0", top:"0","z-index":"0","background-color":"#000000"});
			$(".menu_bg_layer").css("opacity",0);
			var showid="#___"+type;
			var showidval="___"+type;
			var html='';
			var isempty=$(showid).html();
			if(isempty==null)
			{
				html+="<div id=\""+showidval+"\" class=\"menu\">";
				html+="<ul>";
				html+="<li id=\"\" title=\"请选择\">加载中...</li>";
				html+="</ul>";
				html+="</div>";
				clickobj.parent("div").append(html);
				var appendli=false;
			}
			if(appendli==false)
			{
				$.getScript(dir+"data/cache_classify.js", function(){	
					html='';
					html+="<li id=\"\" title=\"不限制\">不限制</li>";
					if (type=="wage")
					{
						data=QS_wage;
					}
					else if(type=="education")
					{
						data=QS_education;
					}
					else if(type=="experience")
					{
						data=QS_experience;
					}
					else if(type=="settr")
					{
						data=new Array("3,三天内","7,一周内","30,一月内","90,三月内");
					}
					else if(type=="sex")
					{
						data=new Array("1,男","2,女");
					}
					else if(type=="companytype")
					{
						data=QS_companytype;
					}
					else if(type=="jobsnature")
					{
						data=QS_jobsnature;
					}
					else if(type=="scale")
					{
						data=QS_scale;
					}					
					for(var i=0;i<data.length;i++)
					{
						arr  =data[i].split(",");
						html+="<li id=\""+arr[0]+"\" title=\""+arr[1]+"\">"+arr[1]+"</li>";
					}
					$(showid+" ul").html(html);
					addhover(showid,input_cn,input);
				});
			}
			else
			{
				addhover(showid,input_cn,input);
			}
			$(showid).slideToggle("fast");
			//
			function addhover(showid,input_cn,input)
			{
				$(showid+" li").click(function(){
					$(input_cn).val($(this).attr("title"));
					$(input).val($(this).attr("id"));
					$(".menu_bg_layer").hide();
					$(showid).hide();
					$(showid).parent("div").css("position","");	
					$(this).css("background-color","");			
				});
				$(".menu_bg_layer").click(function(){
					$(".menu_bg_layer").detach();
					$(showid).hide();
					$(clickobj).parent("div").css("position","");
				});
				$(showid+" li").hover(
					function()
					{
					$(this).css("background-color","#DAECF5");
					},
					function()
					{
					$(this).css("background-color","");		
					}
				);
			}
	});
}