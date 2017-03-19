// 加入人才库
function allfavorites(ajaxurl)
{
	//全选反选
	$("input[name='selectall']").unbind().click(function(){$("#infolists :checkbox").attr('checked',$(this).is(':checked'))});
	$(".add_favoritesr").unbind().click(function(){	
		var myDialog = dialog();
		var url=ajaxurl+"user/user_favorites_resume.php?id="+$(this).attr("resume_id")+"&act=add";
	    $.get(url, function(data){
	        myDialog.content(data);
	        myDialog.title('收藏简历');
	        myDialog.width('500');
	        myDialog.showModal();
	        /* 关闭 */
	        $(".DialogClose").live('click',function() {
	          myDialog.close().remove();
	        });
	    });
	});	
	
	$(".allfavorites").click(function()
	{
		var sltlength=$("#infolists input:checked").length;
		if ($("#infolists .infolist-row input:checked").length==0)
			{
				var myDialog = dialog();
				myDialog.content("请选择简历");
		        myDialog.title('系统提示');
		        myDialog.width('300');
		        myDialog.showModal();
			}
			else
			{
				var jidArr=new Array();
				$("#infolists .infolist-row :checkbox[checked]").each(function(index){jidArr[index]=$(this).val();});
				var myDialog = dialog();
				var url_=ajaxurl+"user/user_favorites_resume.php?id="+jidArr.join("-")+"&act=add";
			    $.get(url_, function(data){
			        myDialog.content(data);
			        myDialog.title('收藏简历');
			        myDialog.width('500');
			        myDialog.showModal();
			        /* 关闭 */
			        $(".DialogClose").live('click',function() {
			          myDialog.close().remove();
			        });
			    });
			}
	});
}