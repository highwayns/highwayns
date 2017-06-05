(function($) {
	"use strict";

	// Top Menu
	function initTopMenu() {
		$('ul.top-menu').superfish({
			popUpSelector: 'ul,.sf-megamenu-inner',
			delay: 100,
			animation: {
				opacity: 'show',
				height: 'show'
			},
			speed: 'fast',
			cssArrows: false,
			disableHI: true
		});
	}

	// Primary Menu
	function initPrimaryMenu() {
		$('ul.primary-menu').superfish({
			popUpSelector: 'ul,.sf-megamenu-inner',
			delay: 100,
			animation: {
				opacity: 'show',
				height: 'show'
			},
			speed: 'fast',
			cssArrows: false,
			disableHI: true
		});
	}

	// Sticky Menu
	function initStickyMenu() {
		$('ul.sticky-menu').superfish({
			popUpSelector: 'ul,.sf-megamenu-inner',
			delay: 100,
			animation: {
				opacity: 'show',
				height: 'show'
			},
			speed: 'fast',
			cssArrows: false,
			disableHI: true
		});
	}

	// Responsive Top Menu
	function initMobileTopMenu() {
		$('.top-mobile-navigation .top-mobile-menu li').each(function() {
			$(this).has('ul').prepend('<span class="top-submenu-toggle"><i class="fa fa-angle-down"></i></span>');
			$(this).find('ul').hide();
		});

		$('.top-mobile-menu .top-submenu-toggle').click(function(e) {
			e.stopPropagation();
			$(this).next().next().slideToggle(300, Waypoint.refreshAll);
			if ($(this).children().hasClass('fa-angle-down')) {
				$(this).children().removeClass('fa-angle-down').addClass('fa-angle-up');
			} else {
				$(this).children().removeClass('fa-angle-up').addClass('fa-angle-down');
			}
		});

		if( $('.top-mobile-menu-icon').length ) {
			$('.top-mobile-menu-icon').parent().addClass('top-bar-padding').css('min-height', '44px');
		}

		$('.top-mobile-menu-icon').click(function() {
			$('.top-mobile-navigation .top-mobile-navigation-inner').slideToggle(300, Waypoint.refreshAll);
		});
	}

	// Responsive Primary Menu
	function initMobilePrimaryMenu() {
		if( $('.primary-mobile-trigger').length ) {
			$('.primary-mobile-trigger').sidr({
				name: 'sidr-primary',
				source: '#sidr-primary',
				renaming: false
			});

			$('.primary-mobile-menu li').each(function() {
				$(this).has('ul').prepend('<span class="primary-submenu-toggle"><i class="fa fa-angle-down"></i></span>');
				$(this).find('ul').hide();
			});

			$('.primary-mobile-menu .primary-submenu-toggle').click(function(e) {
				e.stopPropagation();
				$(this).next().next().slideToggle(300);
				if ($(this).children().hasClass('fa-angle-down')) {
					$(this).children().removeClass('fa-angle-down').addClass('fa-angle-up');
				} else {
					$(this).children().removeClass('fa-angle-up').addClass('fa-angle-down');
				}
			});

			$('.sidr-primary-close').click(function() {
				$.sidr('close', 'sidr-primary');
				return false;
			});
		}
	}

	// Responsive Primary Menu
	function initSidrResizeFix() {
		if( $(window).width() > 991 ) {
			$.sidr('close', 'sidr-primary');
		}
	}

	// Sticky Header
	function initStickyHeader() {
		if( $('#sticky-header').length ) {
        	$('.init-sticky-header').waypoint({
			    handler: function(direction) {
			        if (direction === 'down') {
			            $('#sticky-header').addClass('init-sticky');
			            if( $('#wpadminbar').length ) {
			            	$('.sticky-header').css('top', $('#wpadminbar').outerHeight());
			            }
			        } else if (direction === 'up') {
			            $('#sticky-header').removeClass('init-sticky');
			        }
			    }
			});
		}
	}

	// Header Search
	function initHeaderSearch() {
		$('.overlay-search-trigger').click(function() {
			$('.overlay-search').fadeToggle().find('input').focus();
			return false;
		});

		$('.overlay-search-close').click(function() {
			$('.overlay-search').fadeOut();
			return false;
		});
	}

	// Featured Slider
	function initFeaturedSlider() {
		$('#featured-slider').each(function() {
			var loop = $(this).attr('data-loop');
			var navigation = $(this).attr('data-navigation');
			var dots = $(this).attr('data-dots');
			var autoplay = $(this).attr('data-autoplay');
			var pauseOnHover = $(this).attr('data-pausehover');
			var owlTimeout = parseInt($(this).attr('data-timeout'));
			
			if ( loop == 'true' ) {
				var owlLoop = true
			} else {
				var owlLoop = false
			}
			
			if ( navigation == 'true' ) {
				var owlNav = true
			} else {
				var owlNav = false
			}
			
			if ( dots == 'true' ) {
				var owlDots = true
			} else {
				var owlDots = false
			}
			
			if ( autoplay == 'true' ) {
				var owlAutoplay = true
			} else {
				var owlAutoplay = false
			}
			
			if ( pauseOnHover == 'true' ) {
				var owlPauseOnHover = true
			} else {
				var owlPauseOnHover = false
			}

			$(this).owlCarousel({
				loop: owlLoop,
				margin: 0,
				autoplay: owlAutoplay,
				autoplayTimeout: owlTimeout,
				autoplayHoverPause: owlPauseOnHover,
				nav: owlNav,
				navText: ['<i class="fa fa-chevron-left"></i>','<i class="fa fa-chevron-right"></i>'],
				dots: owlDots,
				items: 1,
				autoHeight: false
			});
			
		});
	}

	// Featured Slider
	function initFeaturedCarousel() {
		$('#featured-carousel').each(function() {
			var loop = $(this).attr('data-loop');
			var navigation = $(this).attr('data-navigation');
			var autoplay = $(this).attr('data-autoplay');
			var pauseOnHover = $(this).attr('data-pausehover');
			var owlTimeout = parseInt($(this).attr('data-timeout'));
			var mobile = parseInt($(this).attr('data-mobile'));
			var tablet = parseInt($(this).attr('data-tablet'));
			var tabletsmall = parseInt($(this).attr('data-tabletsmall'));
			var desktopsmall = parseInt($(this).attr('data-desktopsmall'));
			var desktop = parseInt($(this).attr('data-desktop'));
			
			if ( loop == 'true' ) {
				var owlLoop = true
			} else {
				var owlLoop = false
			}
			
			if ( navigation == 'true' ) {
				var owlNav = true
			} else {
				var owlNav = false
			}
			
			if ( autoplay == 'true' ) {
				var owlAutoplay = true
			} else {
				var owlAutoplay = false
			}
			
			if ( pauseOnHover == 'true' ) {
				var owlPauseOnHover = true
			} else {
				var owlPauseOnHover = false
			}

			$(this).owlCarousel({
				loop: owlLoop,
				margin: 0,
				autoplay: owlAutoplay,
				autoplayTimeout: owlTimeout,
				autoplayHoverPause: owlPauseOnHover,
				nav: owlNav,
				navText: ['<i class="fa fa-chevron-left"></i>','<i class="fa fa-chevron-right"></i>'],
				dots: false,
				responsive:{
					0:{
						items: mobile
					},
					480:{
						items: tabletsmall
					},
					768:{
						items: tablet
					},
					992:{
						items: desktopsmall
					},
					1200:{
						items: desktop
					}
				},
				autoHeight: false
			});
			
		});
	}

	// Tooltip
	function initToolTip() {
		$('[data-toggle="tooltip"]').tooltip({
			container: 'body'
		});
	}

	// Popover
	function initPopover() {
		$('[data-toggle="popover"]').popover({
			container: 'body',
			animation: true
		});
	}

	// Scroll To Top
	function initScrollToTop() {
	    $(window).scroll(function() {
	        if( $(this).scrollTop() > 100 ) {
	            $('.scroll-to-top').fadeIn();
	        } else {
	            $('.scroll-to-top').fadeOut();
	        }
	    });

	    $('.scroll-to-top').click(function() {
	        $('html, body').animate({
	            scrollTop: 0
	        }, 800);
	        return false;
	    });
	}

	// Lightbox
	function initLightbox() {
		$('.img-lightbox').magnificPopup({
			type: 'image'
		});

		$('.gallery-lightbox').magnificPopup({
			delegate: 'a',
			type: 'image',
			gallery: {
				enabled: true
			}
		});

		$('.video-lightbox').magnificPopup({
			disableOn: 700,
			type: 'iframe',
			removalDelay: 160,
			preloader: false,
			fixedContentPos: false
		});
	}

	// Gallery Slider
	function initGalleryslider() {
        $('.post-gallery-wrapper').flexslider({
			animation: 'fade',
			animationSpeed: 500,
			slideshow: true,
			smoothHeight: true,
			controlNav: true,
			prevText: '<i class="fa fa-chevron-left"></i>',
			nextText: '<i class="fa fa-chevron-right"></i>'
		});
	}

	// Post Slider Widget
	function initPostsliderWidget() {
        $('.post-slider-widget').flexslider({
			animation: 'fade',
			animationSpeed: 500,
			slideshow: true,
			smoothHeight: true,
			controlNav: true,
			prevText: '<i class="fa fa-chevron-left"></i>',
			nextText: '<i class="fa fa-chevron-right"></i>'
		});
	}

	// Instagram Slider Widget
	function initInstagramSliderWidget() {
        $('.instagram-slider-widget').flexslider({
			animation: 'fade',
			animationSpeed: 500,
			slideshow: true,
			smoothHeight: true,
			controlNav: false,
			prevText: '<i class="fa fa-chevron-left"></i>',
			nextText: '<i class="fa fa-chevron-right"></i>'
		});
	}

	// Masonry
	function initBlogMasonry() {
		var $container = $('.blog-masonry');
		$container.imagesLoaded(function() {
			$container.masonry({
				itemSelector : '.blog-masonry .post-entry',
				columnWidth : '.blog-masonry .post-entry',
				isAnimated : true,
			});
		});
	}

	// Bootsrap Modal
	function initBootstrapModal() {
		$('.themepixels-modal').each( function() {
			$('#wrapper').append( $( this ) );
		});
	}

	// Skillbar
	function initSkillbar() {
    	$('.skill-bar').each(function(){
			$(this).find('.skill-bar-bar').animate({ width: $(this).attr('data-percent') }, 1500 );
		});
	}

	// Preloader
	function initPreloader() {
		if( $('#preloader').length ) {
			$('body').addClass('preloader-running');
		}
	}

	function hidePreloader() {
		if( $('#preloader').length ) {
			$('#preloader-inner').fadeOut();
			$('#preloader').delay(350).fadeOut('slow');
			$('body').removeClass('preloader-running');
			$('body').addClass('preloader-completed');
		}
	}

	// Tabs Widget
	function initTabsWidget() {
		$('.tabs-widget').each( function() {
			$(this).find('a[data-toggle="tab"]:first').tab('show');
		});
	}

	$(document).ready(function() {
		
		initPreloader();
		initMobileTopMenu();
		initTopMenu();
		initMobilePrimaryMenu();
		initPrimaryMenu();
		initStickyMenu();
		initStickyHeader();
		initFeaturedSlider();
		initFeaturedCarousel();
		initToolTip();
		initPopover();
		initHeaderSearch();
		initScrollToTop();
		initLightbox();
		initBlogMasonry();
		initGalleryslider();
		initPostsliderWidget();
		initInstagramSliderWidget();
		initSkillbar();
		
	});
	
	$(window).resize(function() {
		initSidrResizeFix();
	});
	
	$(window).load(function() {
		hidePreloader();
		initBootstrapModal();
		initTabsWidget();
	});

})(jQuery);