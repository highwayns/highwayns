/**
 * pc/_common/webapp.modal.lock.js
 *
 * モーダルウインドウ lock用（モーダルの表示、非表示タイミングを調整できる）
 * .imj-modal-lock
 * .imj-modal-lock__contents
 */
webapp.modal.lock = function (selector, selector_contents, seconds) {

    //デフォルトselector
    this.selector = webapp.defaultArg(selector, '.imj-modal-lock');
    this.selector_contents = webapp.defaultArg(selector_contents, '.imj-modal-lock__contents');

    //デフォルトタイム（秒）
    this.seconds = webapp.defaultArg(seconds, 5);

    this.modal = $(this.selector).css('visibility', 'visible').remodal();
    this.modal_contents = $(this.selector_contents);

    //モーダルの開始
    this.start = function () {
        this.modal.open();
    },

    //モーダルのコンテンツ表示
    this.view = function () {
        this.modal_contents.fadeIn('fast');
    },

    //モーダルの終了
    this.end = function () {

        var milliseconds = this.seconds * 1000;
        var self_modal = this.modal;
        var self_modal_contents = this.modal_contents;

        setTimeout(function(){
            if (self_modal.getState() == 'opened' || self_modal.getState() == 'opening') {
                self_modal_contents.fadeOut('fast');
                self_modal.close();
            }
        }, milliseconds);

        return false;
    }

};
