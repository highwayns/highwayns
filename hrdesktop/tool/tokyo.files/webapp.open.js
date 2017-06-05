/**
 * pc/_common/webapp.open.js
 *
 * 開く
 * 
 */
webapp.open = webapp.open || {

    /* 汎用
        .imj-open
    ========================================================================== */
    normal: function(selector, target_selector) {

        //デフォルトselector
        var selector = webapp.defaultArg(selector, '.imj-open');

        $(selector).each(function(index, element) {

            var $selector = $(element);
            var $target_selector;

            $selector.click(function(){

                //デフォルトtarget_selector
                if (target_selector === undefined || target_selector == null) {
                    $target_selector = $selector.next();
                } else {
                    $target_selector = $(target_selector).eq(index);
                }

                $(this).hide();
                $target_selector.fadeIn();
                return false;

            });

        });

    },


    /* もっと見る
        .imj-more
    ========================================================================== */
    more: function(selector) {

        //デフォルト
        var selector = webapp.defaultArg(selector, '.imj-more');

        $(selector).click(function(){
            $(this).hide().next().slideDown('fast');
            return false;
        });
    },


    /* .imj-radio
    ========================================================================== */
    radio: function(open_value, selector, target_selector) {

        //デフォルトselector
        var selector = webapp.defaultArg(selector, '.imj-radio');
        var target_selector = webapp.defaultArg(target_selector, '.imj-target-radio');

        $(selector).click(function(){

            $(selector).prop('checked', false);
            $(this).prop('checked', true);

            if ($(this).val() == open_value) {
                $(target_selector).show();
            } else {
                $(target_selector).hide();
            }

            return true;

        });

    },


    /* .imj-slide-open
    ========================================================================== */
    slide: function(selector, target_selector, class_close, class_open, func) {

        //デフォルトselector
        var selector = webapp.defaultArg(selector, '.imj-slide-open');
        if (func === undefined || func == null) {
            func = function(){};
        }

        $(selector).each(function(index, element) {

            var $selector = $(element);
            var $target_selector;

            $selector.click(function(){

                //デフォルトtarget_selector
                if (target_selector === undefined || target_selector == null) {
                    $target_selector = $selector.next();
                } else {
                    $target_selector = $(target_selector).eq(index);
                }

                if ($target_selector.is(':hidden')) {
                    if (class_close === undefined || class_close == null) {
                        $target_selector.slideDown('normal', function(){
                            func($selector, $target_selector);
                        });
                    } else {
                        $target_selector.slideDown('normal', function(){
                            if (class_open !== undefined && class_open != null) {
                                $selector.removeClass(class_open);
                            }
                            $selector.addClass(class_close);
                            func($selector, $target_selector);
                        });
                    }
                    
                } else {
                    if (class_close === undefined || class_close == null) {
                        $target_selector.slideUp('fast', function(){
                            func($selector, $target_selector);
                        });
                    } else {
                        $target_selector.slideUp('fast', function(){
                            if (class_open !== undefined && class_open != null) {
                                $selector.addClass(class_open);
                            }
                            $selector.removeClass(class_close);
                            func($selector, $target_selector);
                        });
                    }
                    
                }

                return false;

            });

        });

    },


    /* .imj-open-hover
    ========================================================================== */
    hover: function(selector, target_selector) {

        //デフォルトselector
        var selector = webapp.defaultArg(selector, '.imj-open-hover');

        $(selector).each(function(index, element) {

            var $selector = $(element);
            var $target_selector;

            $selector.hover(function () {

                //デフォルトtarget_selector
                if (target_selector === undefined || target_selector == null) {
                    $target_selector = $selector.next();
                } else {
                    $target_selector = $(target_selector).eq(index);
                }

                $target_selector.stop(true, false).slideDown('fast');
                return false;

            },
            function () {

                //デフォルトtarget_selector
                if (target_selector === undefined || target_selector == null) {
                    $target_selector = $selector.next();
                } else {
                    $target_selector = $(target_selector).eq(index);
                }

                $target_selector.stop(true, false).slideUp('fast');
                return false;
            });

        });

    }

};