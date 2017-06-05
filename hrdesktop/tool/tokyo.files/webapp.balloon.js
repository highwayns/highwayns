/**
 * pc/_common/webapp.balloon.js
 *
 * 吹き出し
 * 
 */
webapp.balloon = webapp.balloon || {
    
    /* 汎用
        .imj-balloon
    ========================================================================== */
    normal: function (selector, target_selector, position) {

        //デフォルトselector
        var selector = webapp.defaultArg(selector, '.imj-balloon');

        $(selector).each(function(index, element) {

            var $selector = $(element);
            var $target_selector;

            //デフォルトtarget_selector
            if (target_selector === undefined || target_selector == null) {
                $target_selector = $selector.next();
            } else {
                $target_selector = $(target_selector).eq(index);
            }

            //CSS
            if (position == 'right') {
                $target_selector.css({
                    'position': 'absolute',
                    'left': '35px',
                    'top': '-22px',
                    'z-index': '1000'
                });
            } else if (position == 'bottom') {
                $target_selector.css({
                    'position': 'absolute',
                    'left': '-58px',
                    'bottom': '12px',
                    'z-index': '1000'
                });
            } else {
                $target_selector.css({
                    'position': 'absolute',
                    'left': '-15px',
                    'bottom': '12px',
                    'z-index': '1000'
                });
            }
            
            $selector.hover(function(){
                $(this).append($target_selector).css({
                    position: 'relative',
                    cursor: 'pointer'
                });
                $target_selector.fadeIn('fast');
            },function(){
                $target_selector.fadeOut('fast');
            });

        });
    },


    /* どこからでも、同じ要素を開く
        .imj-balloon-one .imj-target-balloon-one
    ========================================================================== */
    one: function (selector, target_selector) {

        //デフォルトselector
        var selector = webapp.defaultArg(selector, '.imj-balloon-one');
        var target_selector = webapp.defaultArg(target_selector, '.imj-target-balloon-one');

        var $selector = $(selector);
        var $target_selector = $(target_selector);

        //CSS
        $target_selector.css({
            'position': 'absolute',
            'left': '-15px',
            'bottom': '12px',
            'z-index': '1000'
        });
        
        $selector.hover(function(){
            $(this).append($target_selector).css({
                position: 'relative',
                cursor: 'pointer'
            });
            $target_selector.fadeIn('fast');
        },function(){
            $target_selector.fadeOut('fast');
        });
    },


    /* クリックして開く
        .imj-balloon-click
    ========================================================================== */
    click: function (selector, target_selector, bool_parent) {

        //デフォルトselector
        var selector = webapp.defaultArg(selector, '.imj-balloon-click');

        $(selector).each(function(index, element) {

            var $selector = $(element);
            var $target_selector;

            //デフォルトtarget_selector
            if (target_selector === undefined || target_selector == null) {
                $target_selector = $selector.next();
            } else {
                $target_selector = $(target_selector).eq(index);
            }

            //CSS
            $target_selector.css({
                'position': 'absolute',
                'z-index': 10000
            });
            
            $selector.on('click', function(){
                var $elem;
                if (bool_parent == true) {
                    $elem = $(this).parent();
                } else {
                    $elem = $(this);
                }
                $elem.append($target_selector).css({
                    'position': 'relative',
                    'display': 'inline-block'
                });

                $target_selector.fadeIn('fast');

                //overlay
                webapp.overlay(true, 'rgba(43, 46, 56, 0.5)', function(){
                    $target_selector.fadeOut('fast');
                });
            });

        });
    },

};
