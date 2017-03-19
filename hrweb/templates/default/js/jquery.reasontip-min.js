/**
Vertigo Tip by www.vertigo-project.com
Requires jQuery
*/

 function vtip_reason(dir,reason) {   
    this.xOffset = -10; // x distance from mouse
    this.yOffset = 15; // y distance from mouse       
    $(".reason").unbind().hover(   
        function(e) {
            this.t = "载入中...";
            this.title = ''; 
            this.top = (e.pageY + yOffset);
			this.left = (e.pageX);
			$('body').css("cursor","help");
			var id= $(this).attr('id');
            $('body').append( '<p id="reason" class="reason-'+id+'">' + this.t + '</p>' );
			$.get(dir+"plus/ajax_audit_reason.php", {"act":reason,"id":id},
			function (data,textStatus){$(".reason-"+id).html(data);}	);
			var divX=this.left+$('p#reason').width();
			var documentwidth=$(document).width()-100;
			if (divX>documentwidth)
			{
					var RY=$(document).width()-e.pageX; 
				
				 $('p#reason').css("top", this.top+"px").css("right", RY+"px").fadeIn(0);	
			}
			else
			{
				$('p#reason').css("top", this.top+"px").css("left", this.left+"px").fadeIn(0);		
			}
            	   
        },
        function() {
            this.title = this.t;
			$('body').css("cursor","");
            $("p#reason").fadeOut("slow").remove();
        }
    ).mousemove(
        function(e) {
         this.top = (e.pageY + yOffset);
         this.left = (e.pageX);
		 var divX=this.left+$('p#reason').width();
		var documentwidth=$(document).width()-100;
		if (divX>documentwidth)
			{
				var RY=$(document).width()-e.pageX;
         		$("p#reason").css("top", this.top+"px").css("right",RY+"px"); 
			}
			else
			{
				$("p#reason").css("top", this.top+"px").css("left", this.left+"px"); 
			}
        }
    );            
    
};

