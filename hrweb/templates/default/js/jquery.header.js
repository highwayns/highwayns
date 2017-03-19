function header() {
	//二维码显示/隐藏
	var isShow;
	$('.webApp').mouseenter(function (e) {isShow = setTimeout(function () {$('.weMinBoxApp').show();e.stopPropagation()}, 300);}).parent().mouseleave(function (e) {
		clearTimeout(isShow);
		$('.weMinBoxApp',$(this)).hide();
	});
	$('.weChat').mouseenter(function (e) {isShow = setTimeout(function () {$('.weMinBoxWx').show();e.stopPropagation()}, 300);}).parent().mouseleave(function (e) {
		clearTimeout(isShow);
		$('.weMinBoxWx',$(this)).hide();
	})
	// 顶部nav展开
	$('.nav-list').hover(function(){
		$(this).find('.nav-li').css({'background-color':'#005eac'});
		$(this).find('.nav-more-drop').show();
	}, function(){
		$(this).find('.nav-li').attr('style', '');
		$(this).find('.nav-more-drop').hide();
	});
	// 名企列表
	$('.famous-items:nth-child(4n)').css({'margin-right':0});
	// 照片简历
	$('.photo-items:nth-child(7n)').css({'margin-right':0});
	// 广告位
	var adBlock = $('.ad-row');
	adBlock.each(function(){
		if ($(this).find('.ad-item').length == 3) {
			$(this).find('.ad-item:nth-child(3n)').css({'margin-right':0});
		}else if($(this).find('.ad-item').length == 5) {
			$(this).find('.ad-item:nth-child(5n)').css({'margin-right':0});
		};
	});
	// /* 分站管理*/
	// $(".local_station").live("click",function(){
	// 	if($(".sub_station").attr('data') == "hide") {
	// 		$(this).blur();
	// 		$(this).parent().parent().before("<div class=\"menu_bg_layer\"></div>");
	// 		$(".menu_bg_layer").height($(document).height());
	// 		$(".menu_bg_layer").css({ width: $(document).width(), position: "absolute",left:"0", top:"0","z-index":"77","background-color":"#000000"});
	// 		$(".menu_bg_layer").css("opacity",0);
	// 		$(".sub_station").show();
	// 		$(".sub_station").attr('data',"show");
	// 		$(".menu_bg_layer, .station_close").click(function() {
	// 			$(".sub_station").hide();
	// 			$(".sub_station").attr('data',"hide");
	// 			$(".menu_bg_layer").remove();
	// 		});
	// 	} else {
	// 		$(".sub_station").hide();
	// 		$(".sub_station").attr('data',"hide");
	// 	}
	// });
	//单数行变色
	$(".local_station>p").hover(function(){
		$(this).addClass("hover");
	},function(){
		$(this).removeClass("hover");
	});
	$(".sub_st_content li:last").css({
		"border-bottom":"0"
	}).find("div").css({"padding-bottom":"0"});
	// 导航固定
	// $(".top-nav-wrap").addClass('stuckMenu');
	// $(window).on('scroll', function() {
	// 	var t = $(".top-nav-wrap"), stickyHeight = parseInt(t.outerHeight()), vartop = parseInt(t.offset().top), varscroll = parseInt($(document).scrollTop());
	// 	if(vartop < varscroll){
	// 		$('.stuckMenu').addClass('isStuck');
	// 		$('.stuckMenu').next().css(
	// 			'margin-top', stickyHeight + 'px'
	// 		);
	// 	};
	// 	if(varscroll <= vartop){
	// 		stickyHeight = 0;
	// 		$('.stuckMenu').removeClass('isStuck');
	// 		$('.stuckMenu').next().css(
	// 			'margin-top', stickyHeight + 'px'
	// 		);
	// 	};
	// });
}