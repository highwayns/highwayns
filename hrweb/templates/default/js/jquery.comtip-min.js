/**
Requires jQuery
*/
jQuery.joblisttip= function(obj,ajaxurl,loading,css) {  
    $(obj).unbind().hover(    
        function(event)
		{
			var uid = this.id;
			if (uid=="" || uid=="0")
			{
				return false;
			}
			event.stopPropagation(); // do something 
			var domtitle = this.title;
            this.title = ''; 
			if (domtitle=='' || domtitle=='null')
			{
				$(this).append( '<div class="'+ css +'"><div class="tipboxtit"></div><div class="tipboxtxt"><div class="tipboxtits">招聘岗位：</div><ul>' + loading + '</ul></div></div>' );
				$(this).css("position","relative").css("position","relative").fadeIn(10);	
					var insertobj=$(this);
					$.get(ajaxurl, {"uid":uid},
						function (data,textStatus)
						{
						data=data?data:"暂无职位...";
						insertobj.find("ul").html(data);
						domtitle=data;
						
						}
				);				
			}
			else
			{
				$(this).append( '<div class="'+ css +'"><div class="tipboxtit"></div><div class="tipboxtxt"><div class="tipboxtits">招聘岗位：</div><ul>' + domtitle+ '</ul></div></div>' );
				$(this).css("position","relative").css("position","relative").fadeIn(10); 
			} 			           
        },
        function()
		{
					if ($("."+css).find("ul").html()==loading)
					{
						this.title = '';
					}
					else
					{
						this.title = $("."+css).find("ul").html();
					}        			
					$(this).css("position","");
					$("."+css).fadeOut("slow").remove();
        }
    );
	
    
};
