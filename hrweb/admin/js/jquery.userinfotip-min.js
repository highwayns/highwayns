/**
Vertigo Tip by www.vertigo-project.com
Requires jQuery
*/

this.vtip_userinfo = function() {    
    this.xOffset = -10; // x distance from mouse
    this.yOffset = 15; // y distance from mouse       
    $(".userinfo").unbind().hover(    
        function(e) {
            this.t = "载入中...";
            this.title = ''; 
            this.top = (e.pageY + yOffset);
			this.left = (e.pageX);
			$('body').css("cursor","help");
			var id= $(this).attr('id');
            $('body').append( '<p id="userinfo" class="userinfo-'+id+'">' + this.t + '</p>' );
			$.get("admin_ajax.php", {"act":"get_user_info","id":id},
			function (data,textStatus){$(".userinfo-"+id).html(data);}	);
			var divX=this.left+$('p#userinfo').width();
			var documentwidth=$(document).width()-100;
			if (divX>documentwidth)
			{
					var RY=$(document).width()-e.pageX; 
				
				 $('p#userinfo').css("top", this.top+"px").css("right", RY+"px").fadeIn(0);	
			}
			else
			{
				$('p#userinfo').css("top", this.top+"px").css("left", this.left+"px").fadeIn(0);		
			}
            	   
        },
        function() {
            this.title = this.t;
			$('body').css("cursor","");
            $("p#userinfo").fadeOut("slow").remove();
        }
    ).mousemove(
        function(e) {
         this.top = (e.pageY + yOffset);
         this.left = (e.pageX);
		 var divX=this.left+$('p#userinfo').width();
		var documentwidth=$(document).width()-100;
		if (divX>documentwidth)
			{
				var RY=$(document).width()-e.pageX;
         		$("p#userinfo").css("top", this.top+"px").css("right",RY+"px"); 
			}
			else
			{
				$("p#userinfo").css("top", this.top+"px").css("left", this.left+"px"); 
			}
        }
    );            
    
};

jQuery(document).ready(function($){vtip_userinfo();}) 