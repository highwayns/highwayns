/**
 * pc/_common/webapp.js
 */
var webapp = webapp || {

    /* css 通常はimc、mycを利用する場合はmycをセットする
    ========================================================================== */
    css: 'imc',
    

    /* main から利用する場合を吸収する
    ========================================================================== */
    v1: false,


    /* ua
    ========================================================================== */
    ua: window.navigator.userAgent.toLowerCase(),


    /* version
    ========================================================================== */
    version: window.navigator.appVersion.toLowerCase(),


    /* ie8か否か
    ========================================================================== */
    ie8: function () {
        return (this.ua.indexOf('msie') != -1 && this.version.indexOf('msie 8.') != -1) ? true : false
    },


    /* ie9か否か
    ========================================================================== */
    ie9: function () {
        return (this.ua.indexOf('msie') != -1 && this.version.indexOf('msie 9.') != -1) ? true : false
    },


    /* fullScreen 利用可能有無
    ========================================================================== */
    fullScreen: (document.fullscreenEnabled || document.webkitFullscreenEnabled || document.mozFullScreenEnabled || document.msFullscreenEnabled) ? true : false,


    /* history 利用可能有無
    ========================================================================== */
    history: (window.history && window.history.pushState) ? true : false,


    /* requestAnimationFrame 利用可能有無
    ========================================================================== */
    requestAnimationFrame: (window.requestAnimationFrame) ? true : false,


    /* defaultArg
    ========================================================================== */
    defaultArg: function (arg, value) {
        if (arg === undefined || arg == null) {
            return value;
        } else {
            return arg;
        }
    },


    /* IE9以下のplaceholder
    ========================================================================== */
    placeholder: function() {
        $("[placeholder]").ahPlaceholder({
            placeholderColor : '#aaa',
            placeholderAttr : 'placeholder',
            likeApple : false
        });
    },


    /* グローバルナビ
        .imj-user .imj-target-user
    ========================================================================== */
    globalNavi: function() {
        $('.imj-user').hover(function () {
            $('.imj-target-user').stop(true, false).slideDown('fast');
            return false;
        },
        function () {
            $('.imj-target-user').stop(true, false).slideUp('fast');
            return false;
        });
    },


    /* グローバルナビ カテゴリー用
        .imj-gmenu .imj-target-gmenu
    ========================================================================== */
    globalNaviCategory: function() {
        $('.imj-gmenu').hover(function () {
            $('.imj-gmenu-white', this).show();
            $('.imj-target-gmenu', this).stop(true, false).fadeIn('fast');
            return false;
        },
        function () {
            $('.imj-gmenu-white', this).hide();
            $('.imj-target-gmenu').stop(true, false).fadeOut('normal');
            return false;
        });

        $('.imj-gmenu').append('<div class="imj-gmenu-white" style="display: none;"></div>');
        $('.imj-gmenu-white').css({
            'position': 'absolute',
            'z-index': 101,
            'top': '34px',
            'left': 0,
            'background': '#fff',
            'height': '1px',
            'width': '100%'
        });
    },


    /* パンくずプルダウン
        .imj-breadcrumb-list
    ========================================================================== */
    breadcrumb: function() {
        $('.imj-breadcrumb-list').on('click', function () {
            var $next = $(this).next(), $prev = $(this).prev();

            $next.slideToggle('fast');
            $next.on('click', function(e){
                e.stopPropagation();
            });
            $prev.on('click', function(e){
                e.stopPropagation();
            });
            $(document).on('click', function(){
                $next.stop(true, false).slideUp('fast');
            });
            return false;
        });
    },


    /* 内部リンク
        .imj-link
    ========================================================================== */
    link: function() {
        $('.imj-link').click(function(){
            var href= $(this).attr('href');
            var target = $(href == '#' || href == '' ? 'html' : href);
            $('html, body').animate({scrollTop:target.offset().top}, 800, 'swing');
            return false;
        });
    },


    /* TOPへリンク
        .imj-link-top
    ========================================================================== */
    topLink: function() {
        $(window).on('load scroll', function (){
            if ($(this).scrollTop() > 600) {
                $('.imj-link-top').fadeIn();
            } else {
                $('.imj-link-top').fadeOut();
            }
        });
    },


    /* オーバーレイ
        .imj-overlay
    ========================================================================== */
    overlay: function(bool, background, func) {
        if (bool) {
            var bg;
            if (background === undefined || background == null) {
                bg = 'rgba(43, 46, 56, 0.9)';
            } else {
                bg = background;
            }
            if ($('.imj-overlay').length == 0) {
                $('body').append('<div class="imj-overlay"></div>');
            }
            $('.imj-overlay').css({
                'z-index': 9999,
                "position": "fixed",
                "top": 0,
                "left": 0,
                "width": "100%",
                "height": "120%",
                "background": bg,
            }).fadeIn('fast');

            $('.imj-overlay').on('click', function(){
                $(this).fadeOut('fast').remove();
                if (func !== undefined && func != null) {
                    func();
                }
            });
        } else {
            if ($('.imj-overlay').length > 0) {
                $('.imj-overlay').fadeOut('fast').remove();
            }
        }
    },


    /* サイド追従
        .imj-sticky
    ======================================== */
    sticky: function () {

        if ($('.imj-sticky').length == 1) {
            $('.imj-sticky').css('margin-bottom', '120px');
            $('.imj-sticky').stick_in_parent();
        }
        
    },


    /* ロゴ移動
            .imj-move-logo
    ======================================== */
    moveLogo: function () {
        if ($('.imj-move-logo').length > 0) {
            var $imjMoveLogo = $('.imj-move-logo');
            var pos_x = 0;
            if (webapp.ie8() || webapp.ie9()) {
                setInterval(function(){
                    if (pos_x <= -2023) pos_x = 0;
                    pos_x -= 0.2;
                    $imjMoveLogo.css('background-position', pos_x + 'px center');
                }, 1);
            } else {
                $imjMoveLogo.css({
                    'animation': 'movelogo 30s linear reverse infinite',
                    '-webkit-animation': 'movelogo 30s linear reverse infinite',
                    '-moz-animation': 'movelogo 30s linear reverse infinite'
                });
            }
        }
    },

};
