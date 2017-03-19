//地区
function OpenCity(objid,input_cn,input,inputs,titstr)
{
	$(objid).unbind().click(function()
	{
			$(this).blur();
			html="";
			html+="<div class=\"selectbox\" id=\"selectbox\">";
			html+="<div class=\"titbox\">";
			html+="<div class=\"lefttit\">"+titstr+"</div>";
			html+="<div class=\"unrestricted\">不限制</div>";
			html+="<div class=\"closs\"></div>";
			html+="</div>  ";
			html+="<div class=\"listbox\" id=\"listboxb\">";
			html+="</div>";
			html+="<div class=\"listbox\" id=\"listboxs\">";
			html+="</div>";
			html+="</div>";
			if ($("#selectbox").html()==null)
			{
				$("body").append(html);
				$("#selectbox #listboxb").html(MakeLiB(QS_city_parent));
				$("#selectbox  .unrestricted").click( function () {
								$(input_cn).html('不限制');
								$(input).val('');
								$(inputs).val('');
								$("#selectbox #listboxb .li .t2").removeClass("h");
								$("#selectbox #listboxb").show();
								$("#selectbox #listboxs").hide();
								$("#selectbox").hide();
					});	
				//绑定关闭
				$("#selectbox .closs").click( function () { 
					$("#selectbox").hide();
				});
				$("#selectbox #listboxb .li").click( function () {
					$("#selectbox #listboxb .li .t2").removeClass("h");
					$(this).find(".t2").addClass("h");		
					$("#selectbox #listboxb").hide();
					$("#selectbox #listboxs").show();
					$(input).val($(this).attr('id'));
					if(QS_city[$(this).attr('id')]) {
						$("#selectbox #listboxs").html(MakeLi(QS_city[$(this).attr('id')]));
						$("#selectbox #listboxs .goback").click( function () {
							$("#selectbox #listboxb").show();
							$("#selectbox #listboxs").hide();	
						});								   
						$("#selectbox #listboxs .li").click( function () {
							$("#selectbox #listboxs .li .t2").removeClass("h");
							$(this).find(".t2").addClass("h");		
							$(input_cn).html($(this).attr('title'));
							$(inputs).val($(this).attr('id'));
							$("#selectbox").hide();
						});
					} else {
						$(input_cn).html($(this).attr('title'));
						$(inputs).val("0");
						$("#selectbox").hide();
					}
				});
			}
			else
			{
				$("#selectbox").show();
			}

			
	});
}
//生成大类
function MakeLiB(arr)
{
			if (arr=="")return false;
			var htmlstr='';
				for (x in arr)
				{
				 var v=arr[x].split(",");
				htmlstr+="<div class=\"li\"  title=\""+v[1]+"\" id=\""+v[0]+"\"><div class=\"t1\">"+v[1]+"</div><div class=\"t2\"></div><div class=\"clear\"></div></div>";
				}
			return htmlstr; 
}
//生成小类
function MakeLi(val)
{
			if (val=="")return false;
			arr=val.split("|");
			var htmlstr='<div class=\"goback\">返回上级分类</div>';
				for (x in arr)
				{
				 var v=arr[x].split(",");
				htmlstr+="<div class=\"li\"  title=\""+v[1]+"\" id=\""+v[0]+"\"><div class=\"t1\">"+v[1]+"</div><div class=\"t2\"></div><div class=\"clear\"></div></div>";
				}
			return htmlstr; 
}
//返回上一页
$(document).ready(function()
{
	$("#pageback").click( function () { 
	window.history.go(-1);
	});
});

