/**
 * pc/_common/webapp.fixed.js
 *
 * バナーなどをついてくるようにする
 *
 */
webapp.fixed = webapp.fixed || {

    /* フッター用
        .imj-fixed-footer
    ========================================================================== */
    footer: function (selector, top) {

        //デフォルト
        var selector = webapp.defaultArg(selector, '.imj-fixed-footer');
        var top = webapp.defaultArg(top, 600);

        //bottom position
        var bottom = $('.imj-fixed-footer').height() + 15;

        var $selector = $(selector);

        //スタイル調整
        $selector.css({
            'position': 'fixed',
            'bottom': 0,
            'text-align': 'center',
            'width': '100%',
            'z-index': '3000'
        });

        //フッター部分を大きくする
        $('.imc-layout--footer').css('padding-bottom', '120px');

        //TOPへリンクの場所を調整
        $('.imc-button-top, a.imc-button-top').css('bottom', bottom + 'px');

        //実行
        var footerSlide = function (){
            if ($(this).scrollTop() > top) {
                $selector.slideDown('normal');
            } else {
                $selector.slideUp('normal');
            }
        }
        $(window).on('load scroll', footerSlide);

        //closeボタン 通常はimj-fixed-footer-close
        if ($(selector + '-close').length > 0) {
            var $close = $(selector + '-close');
            $close.on('click', function(){
                $selector.slideUp('normal');
                $(window).off('load scroll', footerSlide);
            });
        }
    },


    /* ヘッダー用
        .imj-fixed-header
    ========================================================================== */
    header: function (selector, top) {

        //デフォルト
        var selector = webapp.defaultArg(selector, '.imj-fixed-header');
        var top = webapp.defaultArg(top, 600);


        var $selector = $(selector);

        //スタイル調整
        $selector.css({
            'position': 'fixed',
            'top': 0,
            'text-align': 'center',
            'width': '100%',
            'z-index': '3000'
        });

        //実行
        $(window).on('load scroll', function (){
            if ($(this).scrollTop() > top) {
                $selector.fadeIn('fast');
            } else {
                $selector.fadeOut('fast');
            }
        });
    },


    /* 追従メニュー用
        .imj-fixed-menu
    ========================================================================== */
    menu: function (selector) {

        //デフォルト
        var selector = webapp.defaultArg(selector, '.imj-fixed-menu');

        var $selector = $(selector);
        var top = $selector.offset().top;

        //スタイル調整
        $selector.css({
            'position': 'fixed',
            'text-align': 'center',
            'width': '100%',
            'z-index': '1000'
        });

        //実行
        $(window).on('load scroll', function (){
            if ($(this).scrollTop() >= top) {
                $selector.css('position', 'fixed');
                $selector.css('top', 0);
            } else {
                //$selector.css('top', top - $(this).scrollTop());
                $selector.css('position', 'absolute');
                $selector.css('top', top);
            }
        });
    }

};
