//全选反选
$("input[name='selectall']").die().live('click',function(){$("#infolists :checkbox").attr('checked',$(this).is(':checked'))});
// 申请职位
function apply_jobs(ajaxurl)
{
	$(".deliver").click(function()
	{
		var sltlength='';
		sltlength=$("#infolists .info-list-wrap input:checked").length;
		if (sltlength==0)
		{
			var myDialog = dialog();
			myDialog.content("请选择职位");
	        myDialog.title('系统提示');
	        myDialog.width('300');
	        myDialog.showModal();
		}
		else
		{
			var jidArr=new Array();
			$("#infolists .info-list-wrap :checkbox[checked]").each(function(index){jidArr[index]=$(this).val();});
			var url_=ajaxurl+"user/user_apply_jobs.php?id="+jidArr.join("-")+"&act=app";
			var myDialog = dialog();
			myDialog.title('申请职位');
			myDialog.content("加载中...");
			myDialog.width('500');
			myDialog.showModal();
			$.get(url_, function(data){
				myDialog.content(data);
				/* 关闭 */
				$(".DialogClose").live('click',function() {
					myDialog.close().remove();
				});
			});
		}
	});
	//单个申请职位
	$(".app_jobs").unbind().click(function(){
		var url_=ajaxurl+"user/user_apply_jobs.php?id="+$(this).attr("jobs_id")+"&act=app";
		var myDialog = dialog();
		myDialog.title('申请职位');
		myDialog.content("加载中...");
		myDialog.width('500');
		myDialog.showModal();
		$.get(url_, function(data){
			myDialog.content(data);
			/* 关闭 */
			$(".DialogClose").live('click',function() {
				myDialog.close().remove();
			});
		});
	});
}
// 收藏职位
function favorites(ajaxurl)
{	
	$(".collecter").click(function()
	{
		var sltlength='';
		sltlength=$("#infolists .info-list-wrap input:checked").length;
		if (sltlength==0)
		{
			var myDialog = dialog();
			myDialog.content("请选择职位");
	        myDialog.title('系统提示');
	        myDialog.width('300');
	        myDialog.showModal();
		}
		else
		{
			var jidArr=new Array();
			$("#infolists .info-list-wrap :checkbox[checked]").each(function(index){jidArr[index]=$(this).val();});
			var myDialog = dialog();
			var url_=ajaxurl+"user/user_favorites_job.php?id="+jidArr.join("-")+"&act=add";
		    $.get(url_, function(data){
		        myDialog.content(data);
		        myDialog.title('加入收藏');
		        myDialog.width('500');
		        myDialog.showModal();
		        /* 关闭 */
		        $(".DialogClose").live('click',function() {
		          myDialog.close().remove();
		        });
		    });
		}
	});
	// 单个收藏职位
	$(".add_favorites").unbind().click(function(){
		var myDialog = dialog();
		var url_=ajaxurl+"user/user_favorites_job.php?id="+$(this).attr("jobs_id")+"&act=add";
	    $.get(url_, function(data){
	        myDialog.content(data);
	        myDialog.title('加入收藏');
	        myDialog.width('500');
	        myDialog.showModal();
	        /* 关闭 */
	        $(".DialogClose").live('click',function() {
	          myDialog.close().remove();
	        });
	    });
	});
}