//Previous Function with jQuery
$(function() {
	// Add class for html tag depend on browser (only ie)
	var __add = function(name, ver){
		if(document.documentElement.className){ document.documentElement.className += ' '; }
		document.documentElement.className += name + (ver!='' ? ' ' + name+(ver*1).toString().replace('.','_') : '');
	}
	var userAgent = window.navigator.userAgent.toLowerCase();
	var appVersion = window.navigator.appVersion.toLowerCase();
	if( get = userAgent.match( /msie (\d+(\.\d+)?)/i ) )              { __add('ie', get[1]); }
	else if( get = userAgent.match( /Trident.+rv\:(\d+(\.\d+)?)/i ) ) { __add('ie', get[1]); }

	// Sidr JS
	$('#responsive-menu-button').sidr({
		name: 'sidr-main',
		source: '#gNav'
	});

	// Site Search
	$('#responsive-search-button').colorbox({inline:true,innerWidth:680,maxWidth:"90%"});

	//*FreeCall for Mobile Only* with CV tag by ThreeH
	var device = navigator.userAgent;
	if((device.indexOf('iPhone') > 0 && device.indexOf('iPad') == -1) || device.indexOf('iPod') > 0 || device.indexOf('Android') > 0){
		//$('#telHdrFC17 img, #telHdrFC img, #ftrFCBnr img, .telConsul img, .telFreecall_MO').wrap('<a href="tel:0120034150" onclick="ga(\'send\', \'event\', \'smartphone\', \'phone-number-tap\', \'main\'); goog_report_conversion(\'tel:0120-03-4150\'); yahoo_report_conversion(\'tel:0120-03-4150\');"></a>');
		$('#telHdrFC17 img, #telHdrFC img, #ftrFCBnr img, .telConsul img, .telFreecall_MO').wrap('<a href="javascript:void(0)" onclick="goog_report_conversion(\'tel:0120-03-4150\'); yahoo_report_conversion(undefined); ga(\'send\', \'event\', \'smartphone\', \'phone-number-tap\', \'main\');"></a>');
		$('#telTokyo, .telTokyo_MO').wrap('<a href="tel:0352205454"></a>');
		$('#telOsaka, .telOsaka_MO').wrap('<a href="tel:0676699600"></a>');
		$('#telNagoya, .telNagoya_MO').wrap('<a href="tel:0525333380"></a>');
		$('#telSapporo, .telSapporo_MO').wrap('<a href="tel:0115226173"></a>');
		$('#telFukuoka, .telFukuoka_MO').wrap('<a href="tel:0926869279"></a>');
	}

	//*Back To Top*
	var pagetop = $('#b2t');
	$(window).scroll(function () { if ($(this).scrollTop() > 300) { pagetop.fadeIn(); } else { pagetop.fadeOut(); } });
	pagetop.click(function () { $('body, html').animate({ scrollTop: 0 }, 500); return false; });

	//*Scroll In Page*
	$('a[href^="#"], area[href^="#"]').not('.nopscr a[href^="#"], a[href^="#"].nopscr, #b2t a').click(function(){
		var HashOffset = $(this.hash).offset().top;
		$('html, body').animate({scrollTop: HashOffset}, 1000);
		return false;
	});

	//*SVG<-PNG with Modernizr
	if (!Modernizr.svg){
		$('img').each(function() {
			$(this).attr('src', $(this).attr('src').replace(/\.svg/gi,'.png'));
		});
	}

});

//Add icon for extaernal link
//$('a[href]').each(function() {
//	if (!this.href.match(new RegExp('^(#|\/|(https?:\/\/' + location.hostname + '))'))) {
//		$(this).attr('target', '_blank');
//		$(this).addClass('icn_exlink');
//	}
//});

//Google Fonts
WebFontConfig = {
	google: { families: [ 'Droid+Sans:400,700:latin', 'Cuprum:400,700:latin' ] }
};
(function() {
	var wf = document.createElement('script');
	wf.src = ('https:' == document.location.protocol ? 'https' : 'http') +
		'://ajax.googleapis.com/ajax/libs/webfont/1/webfont.js';
	wf.type = 'text/javascript';
	wf.async = 'true';
	var s = document.getElementsByTagName('script')[0];
	s.parentNode.insertBefore(wf, s);
})();

//Image Box Fade
function imgFade(obj){
	$(obj).each(function(){
		$(this).hover(
			function(){
				$(this).fadeTo('fast','0.60');
			},
			function(){
				$(this).fadeTo('fast','1');
			}
		);
	});
}
