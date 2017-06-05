/**
 * pc/_common/webapp.slider.js
 *
 * スライダー関連
 * 
 */
webapp.slider = webapp.slider || {

    /* 汎用
        .imj-slider
        options http://bxslider.com/options
    ========================================================================== */
    normal: function (selector, options, bottom) {

        //デフォルト
        var selector = webapp.defaultArg(selector, '.imj-slider');
        var options = webapp.defaultArg(options, {
                speed: 1000,
                controls: false,
                auto: true,
                pause: 4000,
                autoHover: true
            });

        //bottom
        if (bottom) {
            options.onSliderLoad = function() {
                var $bxWrapper = $(selector).closest('.bx-wrapper');
                $('.bx-pager', $bxWrapper).css('bottom', bottom);
            };
        }

        $(selector).bxSlider(options);
        $('li', $(selector)).css('visibility', 'visible');

        //sticky
        //スライド実施後でなければ、動作しない
        webapp.sticky();
    },


    /* site用
        .imj-slider-site
        options http://bxslider.com/options
    ========================================================================== */
    site: function (selector, prev_selector, next_selector, options) {

        //デフォルト
        var selector = webapp.defaultArg(selector, '.imj-slider-site');
        var prev_selector = webapp.defaultArg(prev_selector, '.imj-slider-site-prev');
        var next_selector = webapp.defaultArg(next_selector, '.imj-slider-site-next');

        var options = webapp.defaultArg(options, {
            speed: 1000,
            controls: true,
            auto: true,
            pause: 4000,
            slideWidth: 170,
            maxSlides: 4,
            moveSlides: 1,
            autoHover: true,
            pager: false,
            slideMargin: 17,
            nextSelector: next_selector,
            prevSelector: prev_selector,
            nextText: '<i class="fa fa-angle-right fa-2x" aria-hidden="true"></i>',
            prevText: '<i class="fa fa-angle-left fa-2x" aria-hidden="true"></i>'
        });

        $(selector).bxSlider(options);
        $('li', $(selector)).css('visibility', 'visible');
    },


    /* keyv専用
        .imj-slider-keyv
        options http://bxslider.com/options
    ========================================================================== */
    keyv: function (selector, options) {

        //デフォルト
        var selector = webapp.defaultArg(selector, '.imj-slider-keyv');
        var options = webapp.defaultArg(options, {
                mode: 'fade',
                speed: 1600,
                controls: false,
                auto: true,
                pause: 8000,
                autoHover: true,
                onSliderLoad: function() {
                    var $bxWrapper = $(selector).closest('.bx-wrapper');
                    $('.bx-pager', $bxWrapper).css('bottom', '10px');
                    //$('.bx-default-pager a', $bxWrapper).css('background', '#fff');   //★
                }
            });

        $(selector).bxSlider(options);
        $('li', $(selector)).css('visibility', 'visible');
    }

};
