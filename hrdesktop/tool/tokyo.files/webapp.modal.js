/**
 * pc/_common/webapp.modal.js
 *
 * モーダルウインドウ
 * 
 */
webapp.modal = webapp.modal || {
    
    /* 汎用
        .imj-modal
    ========================================================================== */
    normal: function (selector, target_selector) {

        //デフォルトselector
        var selector = webapp.defaultArg(selector, '.imj-modal');

        $(selector).each(function(index, element) {

            var $selector = $(element);
            var $target_selector;

            //デフォルトtarget_selector
            if (target_selector === undefined || target_selector == null) {
                $target_selector = $selector.next();
            } else {
                $target_selector = $(target_selector).eq(index);
            }

            var modal = $target_selector.remodal();

            $selector.on('click', function () {
                $target_selector.show();
                modal.open();
                return false;
            });

        });
        
    },


    /* 自動的に○秒表示される
        .imj-modal-timer
    ========================================================================== */
    timer: function (selector, seconds) {

        //デフォルトselector
        var selector = webapp.defaultArg(selector, '.imj-modal-timer');

        //デフォルトタイム（秒）
        var seconds = webapp.defaultArg(seconds, 5);

        var milliseconds = seconds * 1000;

        var modal = $(selector).css('visibility', 'visible').remodal();

        modal.open();

        setTimeout(function(){
            if (modal.getState() == 'opened' || modal.getState() == 'opening') {
                modal.close();
            }
        }, milliseconds);

        return false;
        
    }

};
