/**
 * pc/_common/webapp.form.js
 *
 * form関連
 *
 */
webapp.form = webapp.form || {

    /* 必須入力のフォーム動作
        .imj-form .imj-form-item-option
    ========================================================================== */
    requiredForm: function (selector_form, selector_option, class_button, class_button_disabled, class_input_ok) {

        //デフォルト
        var selector_form           = webapp.defaultArg(selector_form,          '.imj-form');
        var selector_option         = webapp.defaultArg(selector_option,        '.imj-form-item-option');
        var class_input_ok          = webapp.defaultArg(class_input_ok,         webapp.css + '-form-item__ok');
        var class_button            = webapp.defaultArg(class_button,           webapp.css + '-button--yellow');
        var class_button_disabled   = webapp.defaultArg(class_button_disabled,  webapp.css + '-button--disabled');

        $(selector_form).each(function(index, element) {

            var $selector_form = $(element);
            var $formInputs = $('input[type="text"], input[type="password"], textarea, select', $selector_form);
            var type;

            if ((webapp.ua.indexOf('msie') != -1 && webapp.version.indexOf('msie 8.') != -1) || (webapp.ua.indexOf('msie') != -1 && webapp.version.indexOf('msie 9.') != -1)){
                //IE8,IE9は常に有効

                //ボタン有効
                webapp.form.buttonOn($selector_form, class_button, class_button_disabled);
                $formInputs.addClass(class_input_ok);

                return true;
            }

            var changeInput = function(){

                //最新データ取得
                var $formInputs = $('input[type="text"], input[type="password"], textarea, select', $selector_form);
                var $formItems = $('.' + webapp.css + '-form-item', $selector_form);

                for (var i = 0; i < $formInputs.length; i++) {

                    if ($formInputs.eq(i).closest(selector_option).length > 0) {
                        //任意の場合はスキップ
                        continue;
                    }
                    if ($formInputs.eq(i).is(':hidden')) {
                        //表示していない場合はスキップ
                        continue;
                    }

                    type = $formInputs.eq(i).attr('type');
                    if ((type == 'text' || type == 'password' || type === undefined) && $formInputs.eq(i).val() == '') {
                        //ボタン無効
                        webapp.form.buttonOff($selector_form, class_button, class_button_disabled);
                        $formInputs.removeClass(class_input_ok);

                        return false;
                    }
                }

                //checkbox
                for (var i = 0; i < $formItems.length; i++) {
                    if ($formItems.eq(i).closest(selector_option).length > 0) {
                        //任意の場合はスキップ
                        continue;
                    }
                    if ($formItems.eq(i).is(':hidden')) {
                        //表示していない場合はスキップ
                        continue;
                    }

                    var $checkboxs = $('input[type="checkbox"]', $formItems.eq(i));

                    if ($checkboxs.length > 0) {
                        var bool = false;
                        for (var j = 0; j < $checkboxs.length; j++) {
                            if($checkboxs.eq(j).prop('checked')) {
                                bool = true;
                                break;
                            }
                        }
                        if (!bool) {
                            //ボタン無効
                            webapp.form.buttonOff($selector_form, class_button, class_button_disabled);
                            $formInputs.removeClass(class_input_ok);

                            return false;
                        }
                    }
                }

                //radio
                for (var i = 0; i < $formItems.length; i++) {
                    if ($formItems.eq(i).closest(selector_option).length > 0) {
                        //任意の場合はスキップ
                        continue;
                    }
                    if ($formItems.eq(i).is(':hidden')) {
                        //表示していない場合はスキップ
                        continue;
                    }

                    var $radios = $('input[type="radio"]', $formItems.eq(i));

                    if ($radios.length > 0) {
                        var bool = false;
                        for (var j = 0; j < $radios.length; j++) {
                            if($radios.eq(j).prop('checked')) {
                                bool = true;
                                break;
                            }
                        }
                        if (!bool) {
                            //ボタン無効
                            webapp.form.buttonOff($selector_form, class_button, class_button_disabled);
                            $formInputs.removeClass(class_input_ok);

                            return false;
                        }
                    }
                }

                //ボタン有効
                webapp.form.buttonOn($selector_form, class_button, class_button_disabled);
                $formInputs.addClass(class_input_ok);
            };

            //実行
            $(element).on('keyup change input', changeInput);

            changeInput();


            // ----------------------------------------------
            // 名前・電話番号・メールアドレス 3点セットバリデーション
            // ----------------------------------------------
            if (
                $('.' + webapp.css + '-form-item', $selector_form).length == 3
                && $('input[name="name"]', $selector_form).length == 1
                && $('input[name="tel"]', $selector_form).length == 1
                && $('input[name="email"]', $selector_form).length == 1
            ) {

                $('button:not(.' + webapp.css + '-form-item button)', $selector_form).attr('type', 'button'); //submitにしない。buttonにする
                $('button:not(.' + webapp.css + '-form-item button)', $selector_form).on('click', function () {

                    var valid_error = false;

                    var $input_name = $('input[name="name"]', $selector_form);
                    var $input_tel = $('input[name="tel"]', $selector_form);
                    var $input_email = $('input[name="email"]', $selector_form);

                    var name_value = $input_name.val();
                    var tel_value = $input_tel.val();
                    var email_value = $input_email.val();

                    //全角を半角に
                    tel_value = tel_value.replace(/[Ａ-Ｚａ-ｚ０-９－！”＃＄％＆’（）＝＜＞，．？＿［］｛｝＠＾～￥]/g, function(s){return String.fromCharCode(s.charCodeAt(0) - 0xFEE0)});
                    email_value = email_value.replace(/[Ａ-Ｚａ-ｚ０-９－！”＃＄％＆’（）＝＜＞，．？＿［］｛｝＠＾～￥]/g, function(s){return String.fromCharCode(s.charCodeAt(0) - 0xFEE0)});

                    //ハイフン変換
                    tel_value = tel_value.replace(/[‐－―～ー￣]/g, '-');

                    if (name_value == '') {
                        $input_name.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>お名前を正しく入力してください。</span></p>');
                        valid_error = true;
                    }

                    if (!tel_value.match(/^0[0-9]{1,4}\-?[0-9]{2,4}\-?[0-9]{3,4}$/)) {
                        $input_tel.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>お電話番号を正しく入力してください。</span></p>');
                        valid_error = true;

                    } else {
                        tel_value = tel_value.replace(/-/g, '');

                        var valid_length = (tel_value.match(/^0[1-9]0/)) ? 11 : 10;

                        if (tel_value.length != valid_length) {
                            $input_tel.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>お電話番号を正しく入力してください。</span></p>');
                            valid_error = true;
                        }
                    }
                    
                    if (!email_value.match(/^[a-zA-Z0-9.!#$%&\'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/)) {
                        $input_email.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>メールアドレスを正しく入力してください。</span></p>');
                        valid_error = true;
                    }

                    //ボタン無効
                    webapp.form.buttonOff($selector_form, class_button, class_button_disabled);
                    $formInputs.removeClass(class_input_ok);

                    if (valid_error) {
                        //失敗
                        return false;

                    } else {
                        //成功
                        $selector_form.submit();
                    }

                });
            }

        });

    },


    /* 名前・電話番号・メールアドレス 3点セットバリデーション requiredFormを利用しない場合
        .imj-form-validation
    ========================================================================== */
    validationFrom: function (selector_form) {

        //デフォルト
        var selector_form = webapp.defaultArg(selector_form, '.imj-form-validation');

        $(selector_form).each(function(index, element) {

            var $selector_form = $(element);

            if (
                $('input[name="name"]', $selector_form).length == 1
                && $('input[name="tel"]', $selector_form).length == 1
                && $('input[name="email"]', $selector_form).length == 1
            ) {

                if (!webapp.v1) {
                    $('button:not(.' + webapp.css + '-form-item button)', $selector_form).attr('type', 'button'); //submitにしない。buttonにする
                }
                $('button:not(.' + webapp.css + '-form-item button)', $selector_form).on('click', function () {

                    var valid_error = false;

                    var $input_name = $('input[name="name"]', $selector_form);
                    var $input_tel = $('input[name="tel"]', $selector_form);
                    var $input_email = $('input[name="email"]', $selector_form);

                    var name_value = $input_name.val();
                    var tel_value = $input_tel.val();
                    var email_value = $input_email.val();

                    //全角を半角に
                    tel_value = tel_value.replace(/[Ａ-Ｚａ-ｚ０-９－！”＃＄％＆’（）＝＜＞，．？＿［］｛｝＠＾～￥]/g, function(s){return String.fromCharCode(s.charCodeAt(0) - 0xFEE0)});
                    email_value = email_value.replace(/[Ａ-Ｚａ-ｚ０-９－！”＃＄％＆’（）＝＜＞，．？＿［］｛｝＠＾～￥]/g, function(s){return String.fromCharCode(s.charCodeAt(0) - 0xFEE0)});

                    //ハイフン変換
                    tel_value = tel_value.replace(/[‐－―～ー￣]/g, '-');

                    if (name_value == '') {
                        $input_name.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>お名前を正しく入力してください。</span></p>');
                        valid_error = true;
                    }

                    if (!tel_value.match(/^0[0-9]{1,4}\-?[0-9]{2,4}\-?[0-9]{3,4}$/)) {
                        $input_tel.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>電話番号を正しく入力してください。</span></p>');
                        if (tel_value) {
                            $input_tel.closest('.' + webapp.css + '-form-item').find('.imj-form-label.imj-form-label-error-hidden').hide(); //label消す
                        }
                        valid_error = true;

                    } else {
                        tel_value = tel_value.replace(/-/g, '');

                        var valid_length = (tel_value.match(/^0[1-9]0/)) ? 11 : 10;

                        if (tel_value.length != valid_length) {
                            $input_tel.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>電話番号を正しく入力してください。</span></p>');
                            if (tel_value) {
                                $input_tel.closest('.' + webapp.css + '-form-item').find('.imj-form-label.imj-form-label-error-hidden').hide(); //label消す
                            }
                            valid_error = true;
                        }
                    }
                    
                    if (!email_value.match(/^[a-zA-Z0-9.!#$%&\'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/)) {
                        $input_email.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>メールアドレスを正しく入力してください。</span></p>');
                        if (email_value) {
                            $input_email.closest('.' + webapp.css + '-form-item').find('.imj-form-label.imj-form-label-error-hidden').hide(); //label消す
                        }
                        valid_error = true;
                    }

                    //内容
                    if ($('textarea[name="message"]', $selector_form).length == 1) {
                        var $textarea_message = $('textarea[name="message"]', $selector_form);
                        if ($textarea_message.val() == '') {
                            $textarea_message.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>内容を入力してください</span></p>');
                            if ($textarea_message.val()) {
                                $textarea_message.closest('.' + webapp.css + '-form-item').find('.imj-form-label.imj-form-error-hidden-hidden').hide(); //label消す
                            }
                            valid_error = true;
                        }
                    }

                    if (valid_error) {
                        //失敗
                        return false;

                    } else {
                        //成功
                        $selector_form.submit();
                    }

                });
            }

        });

    },


    /* 御社名・お名前・メールアドレス 3点セットバリデーション requiredFormを利用しない場合。電話番号は任意
        .imj-form-validation2
    ========================================================================== */
    validationFrom2: function (selector_form) {

        //デフォルト
        var selector_form = webapp.defaultArg(selector_form, '.imj-form-validation2');

        $(selector_form).each(function(index, element) {

            var $selector_form = $(element);

            if (
                $('input[name="company"]', $selector_form).length == 1
                && $('input[name="name"]', $selector_form).length == 1
                && $('input[name="email"]', $selector_form).length == 1
            ) {

                $('button:not(.' + webapp.css + '-form-item button)', $selector_form).attr('type', 'button'); //submitにしない。buttonにする
                $('button:not(.' + webapp.css + '-form-item button)', $selector_form).on('click', function () {

                    var valid_error = false;

                    var $input_company = $('input[name="company"]', $selector_form);
                    var $input_name = $('input[name="name"]', $selector_form);
                    var $input_tel = $('input[name="tel"]', $selector_form);
                    var $input_email = $('input[name="email"]', $selector_form);

                    var company_value = $input_company.val();
                    var name_value = $input_name.val();
                    var tel_value = $input_tel.val();
                    var email_value = $input_email.val();

                    if (webapp.ie8() || webapp.ie9()) {
                        //プレースホルダ―比較処理（IE8,IE9）
                        if (company_value == $input_company.attr('placeholder')) {
                            company_value = '';
                            $input_company.val('');
                        }
                        if (name_value == $input_name.attr('placeholder')) {
                            name_value = '';
                            $input_name.val('');
                        }
                        if (tel_value == $input_tel.attr('placeholder')) {
                            tel_value = '';
                            $input_tel.val('');
                        }
                        if (email_value == $input_email.attr('placeholder')) {
                            email_value = '';
                            $input_email.val('');
                        }
                    }

                    //全角を半角に
                    tel_value = tel_value.replace(/[Ａ-Ｚａ-ｚ０-９－！”＃＄％＆’（）＝＜＞，．？＿［］｛｝＠＾～￥]/g, function(s){return String.fromCharCode(s.charCodeAt(0) - 0xFEE0)});
                    email_value = email_value.replace(/[Ａ-Ｚａ-ｚ０-９－！”＃＄％＆’（）＝＜＞，．？＿［］｛｝＠＾～￥]/g, function(s){return String.fromCharCode(s.charCodeAt(0) - 0xFEE0)});

                    //ハイフン変換
                    tel_value = tel_value.replace(/[‐－―～ー￣]/g, '-');

                    if (company_value == '') {
                        $input_company.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>御社名を正しく入力してください。</span></p>');
                        valid_error = true;
                    }

                    if (name_value == '') {
                        $input_name.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>お名前を正しく入力してください。</span></p>');
                        valid_error = true;
                    }

                    if (tel_value != '') {
                        if (!tel_value.match(/^0[0-9]{1,4}\-?[0-9]{2,4}\-?[0-9]{3,4}$/)) {
                            $input_tel.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>お電話番号を正しく入力してください。</span></p>');
                            valid_error = true;

                        } else {
                            tel_value = tel_value.replace(/-/g, '');

                            var valid_length = (tel_value.match(/^0[1-9]0/)) ? 11 : 10;

                            if (tel_value.length != valid_length) {
                                $input_tel.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>お電話番号を正しく入力してください。</span></p>');
                                valid_error = true;
                            }
                        }
                    }
                    
                    if (!email_value.match(/^[a-zA-Z0-9.!#$%&\'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/)) {
                        $input_email.addClass(webapp.css + '-form-item__error').after('<p class="' + webapp.css + '-form-item__error-text"><span>メールアドレスを正しく入力してください。</span></p>');
                        valid_error = true;
                    }

                    if (valid_error) {
                        //失敗
                        return false;

                    } else {
                        //成功
                        $selector_form.submit();
                    }

                });
            }

        });

    },


    /* 任意入力のフォーム動作
        .imj-form-option
    ========================================================================== */
    optionForm: function (selector_form, class_button, class_button_disabled) {

        //デフォルト
        var selector_form           = webapp.defaultArg(selector_form,                  '.imj-form-option');
        var class_button            = webapp.defaultArg(class_button,                   webapp.css + '-button--yellow');
        var class_button_disabled   = webapp.defaultArg(class_button_disabled,          webapp.css + '-button--disabled');

        $(selector_form).each(function(index, element) {

            var $selector_form = $(element);
            var type;

            if ((webapp.ua.indexOf('msie') != -1 && webapp.version.indexOf('msie 8.') != -1) || (webapp.ua.indexOf('msie') != -1 && webapp.version.indexOf('msie 9.') != -1)){
                //IE8,IE9は常に有効

                //ボタン有効
                webapp.form.buttonOn($selector_form, class_button, class_button_disabled);

                return true;
            }

            var changeInput = function(){

                //最新データ取得
                var $formInputs = $('input[type="text"], input[type="password"], input[type="radio"], textarea, select', $selector_form);

                for (var i = 0; i < $formInputs.length; i++) {
                    type = $formInputs.eq(i).attr('type');
                    if ((type == 'text' || type == 'password' || type === undefined) && $formInputs.eq(i).val() != '' || type == 'radio' && $formInputs.eq(i).prop('checked')) {
                        //ボタン有効
                        webapp.form.buttonOn($selector_form, class_button, class_button_disabled);

                        return false;
                    }
                }
                //ボタン無効
                webapp.form.buttonOff($selector_form, class_button, class_button_disabled);
            };

            //実行
            $(element).on('keyup change input', changeInput);

            changeInput();
        });
    },


    /* removeError
    ========================================================================== */
    removeError: function () {

        //alert用
        $(document).on('click', '.alert', function () {
            var $imcFormItem = $(this).closest('.' + webapp.css + '-form-item');
            $('.alert', $imcFormItem).removeClass('alert');
            $('.alert-msg', $imcFormItem).remove();
            $('input', $imcFormItem).focus();
            $imcFormItem.find('.imj-form-label.imj-form-label-error-hidden').show(); //label表示
            return false;
        });

        //imc-form-item__error用
        $(document).on('click', '.' + webapp.css + '-form-item__error', function () {
            var $imcFormItem = $(this).closest('.' + webapp.css + '-form-item');
            $('.' + webapp.css + '-form-item__error-text', $imcFormItem).remove();
            $('.' + webapp.css + '-form-item__error', $imcFormItem).removeClass(webapp.css + '-form-item__error').focus();
            $imcFormItem.find('.imj-form-label.imj-form-label-error-hidden').show(); //label表示
            return false;
        });
        $(document).on('click', '.' + webapp.css + '-form-item__error-text', function () {
            $(this).prev('.' + webapp.css + '-form-item__error').trigger('click');
        });
    },


    /* resetRadio
        .imj-form-resetradio
    ========================================================================== */
    resetRadio: function (selector) {

        var selector = webapp.defaultArg(selector, '.imj-form-resetradio');
        var vals = [];

        //値を保持
        for(var i = 0 ; i < $(selector).length; i++){
            vals.push($('input[type="radio"]:checked', $(selector).eq(i)).val());
        }

        //click
        $(selector).each(function(index, element) {
            $('input[type="radio"]', element).on('click', function(){
                if($(this).val() == vals[index]) {
                    $(this).prop('checked', false).change();
                    vals[index] = '';
                } else {
                    vals[index] = $(this).val();
                }
            });
        });
    },


    /* moveLabel
        .imj-form-label, .imj-form-target-label
    ========================================================================== */
    moveLabel: function (selector, target_selector) {

        var selector = webapp.defaultArg(selector, '.imj-form-label');
        var target_selector = webapp.defaultArg(target_selector, '.imj-form-target-label');

        $(selector).each(function(index, element) {

            var $selector = $(element);
            var $target_selector = $(target_selector).eq(index);
            var target_selector_height = $target_selector.height();

            var default_css = {
                'position': 'absolute',
                'top': '0',
                'left': '8px',
                'font-size': '16px',
                'line-height': target_selector_height + 'px'
            };

            var target_selector_height_plus = 16;
            if (webapp.v1) {
                target_selector_height_plus = 12;
            }
            var input_css = {
                'position': 'absolute',
                'top': target_selector_height / 2 * -1 + target_selector_height_plus,
                'left': '0',
                'font-size': '12px',
                'line-height': 1
            };

            $selector.css('cursor', 'text');

            if ($target_selector.val()) {
                $selector.css(input_css);
            } else {
                $selector.css(default_css);
            }

            $target_selector.focus(function(){
                $selector.animate(input_css, 300, function(){
                    $(this).css('color', '#000');
                });
            }).blur(function(){
                if ($target_selector.val() == '') {
                    $selector.animate(default_css, 300, function(){
                        $(this).css('color', '#999');
                    });
                }
            });

        });
    },


    /* buttonOff ボタン無効
        .imj-form
    ========================================================================== */
    buttonOff: function (selector_form, class_button, class_button_disabled) {

        //デフォルト
        var selector_form           = webapp.defaultArg(selector_form,                  '.imj-form');
        var class_button            = webapp.defaultArg(class_button,                   webapp.css + '-button--yellow');
        var class_button_disabled   = webapp.defaultArg(class_button_disabled,          webapp.css + '-button--disabled');

        $('button:not(.' + webapp.css + '-form-item button)', $(selector_form))
                    .css('cursor', 'auto')
                    .prop('disabled', true)
                    .removeClass(class_button)
                    .addClass(class_button_disabled);
    },


    /* buttonOn ボタン有効
        .imj-form
    ========================================================================== */
    buttonOn: function (selector_form, class_button, class_button_disabled) {

        //デフォルト
        var selector_form           = webapp.defaultArg(selector_form,                  '.imj-form');
        var class_button            = webapp.defaultArg(class_button,                   webapp.css + '-button--yellow');
        var class_button_disabled   = webapp.defaultArg(class_button_disabled,          webapp.css + '-button--disabled');

        $('button:not(.' + webapp.css + '-form-item button)', $(selector_form))
                    .css('cursor', 'pointer')
                    .prop('disabled', false)
                    .removeClass(class_button_disabled)
                    .addClass(class_button);
    }

};
