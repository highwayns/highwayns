/**
 *
 * @author Yosuke Sugawara
 * @author Kazuhiro Nigara 2017/03/15
 * @author Takano 2016/12/19
 * @author Takano 2016/12/27 event tracking
 *
 */
jQuery(function($) {
    //var WebappModalLock = new webapp.modal.lock('.imj-modal-lock', '.imj-modal-lock__contents', 1);  //endは1秒かける

    //loading
    var $img_loading = $('<img src="//s3-ap-northeast-1.amazonaws.com/unilabo-imitsu-static/image/v2/common/img/img_loading.gif" alt="ローディング" width="64" height="64">');
    $img_loading.css({
        position: 'fixed',
        top: '40%',
        left: 0,
        right: 0,
        margin: 'auto',
        'z-index': 9999
    }).hide().appendTo('body');

    function updateServices(pushStateExecuteFlag, sortText) {

        //WebappModalLock.start();
        $img_loading.show();

        var form = $('#service-search-form');

        $.ajax({
            async: true,
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            dataType: 'json',
            success: function(apiResult) {
                if (apiResult.status == 'OK') {
                    var results = apiResult.results;

                    if (pushStateExecuteFlag) {
                        if (window.history && window.history.pushState) {
                            if (typeof cvPanelApp === 'object') {
                                cvPanelApp.pushState(null, null, results.current_url);
                            }
                            else {
                                window.history.pushState(null, null, results.current_url);
                            }
                        }
                    }

                    $('.searched-service-total-count').text(replaceThousandSeparator(results.service_total_count));
                    $('.searched-service-display-count-begin').text(results.service_total_count > 0 ? '1' : '0');
                    $('.searched-service-display-count-end').text(results.service_total_count < 10 ? results.service_total_count : '10');

                    $('#services-contents').empty();

                    if (results.services.length > 0) {
                        $('#services-contents').html(results.service_list_html);
                        $('#pickup-service-list').html(results.pickup_service_list_html);
                        webapp.page.charts.view();
                        //webapp.open.normal('.imj-more-data', '.imj-target-more-data');
                        webapp.balloon.normal('.imj-balloon-official', null, 'bottom');
                        webapp.open.hover('.imj-slide-doc', '.imj-target-slide-doc');

                        // initial setting 2016/02/13
                        serviceWebsiteVisitInitialize();

                        // click link
                        $('.service-hp-link').on('click', function(){serviceWebsiteVisit(this);});

                        // modal
                        if ($('.imj-open-modal').length > 0) {
                            if ($('#pickup-service-data').data('count') > 0 && $('#pickup-service-data').data('count') <= 10) {

                                //No.4
                                webapp.page.openModal({
                                    small: ($('#pickup-service-data').data('count') <= 2 ? true : false),
                                    //hide: false,
                                    //title: '<span>' + results.service_total_count + '社が該当いたしました</span>',
                                    //conditions: '<span>希望条件：</span>Eコマース対応 / WP対応 / 低価格',
                                    //description: '◯（カテゴリ名）の業者をお探しですか？<br>この他にも、おすすめの業者はたくさんあります。<br>あなたの要望をコンシェルジュがお伺いし、<br>最適な業者をご提案いたします。'
                                    event: ($('#trt').val() === 'srt' ? 'sort' : 'filtering')
                                });
                            }
                            //else if (false) {
                            //    //No.5-1 = No.7
                            //    $('.imj-target-modal-pickup .modal-pickup2__list__item:nth-child(1)').remove();   //これは、チャートを減らすためにとりあえずいれた。
                            //    $('.imj-target-modal-pickup .modal-pickup2__list__item:nth-child(2)').remove();   //これは、チャートを減らすためにとりあえずいれた。
                            //    webapp.page.openModal({
                            //        small: true,
                            //        hide: true,
                            //        title: '<span>1社が該当いたしました</span>',
                            //        conditions: '<span>希望条件：</span>Eコマース対応 / WP対応 / 低価格',
                            //        description: '◯◯（カテゴリ名）の業者をお探しですか？<br>この他にも、おすすめの業者はたくさんあります。<br>あなたの要望をコンシェルジュがお伺いし、<br>最適な業者をご提案いたします。'
                            //    });
                            //} else if (false) {
                            //    //No.5-2 = No.6
                            //    $('.imj-target-modal-pickup .modal-pickup2__list__item:nth-child(2)').remove();   //これは、チャートを減らすためにとりあえずいれた。
                            //    webapp.page.openModal({
                            //        small: true,
                            //        hide: true,
                            //        title: '<span>2社が該当いたしました</span>',
                            //        conditions: '<span>希望条件：</span>Eコマース対応 / WP対応 / 低価格',
                            //        description: 'いかがですか？<br>この他にも、おすすめの業者はたくさんあります。<br>あなたの要望をコンシェルジュがお伺いし、<br>最適な業者をご提案いたします。'
                            //    });
                            //} else if (false) {
                            //    //No.5-3 = No.5
                            //    webapp.page.openModal({
                            //        small: false,
                            //        hide: true,
                            //        title: '<span>3社が該当いたしました</span>',
                            //        conditions: '<span>希望条件：</span>Eコマース対応 / WP対応 / 低価格',
                            //        description: '◯◯（カテゴリ名）の業者をお探しですか？<br>この他にも、おすすめの業者はたくさんあります。<br>あなたの要望をコンシェルジュがお伺いし、<br>最適な業者をご提案いたします。'
                            //    });
                            //}
                        }

                    } // if (results.services.length > 0)

                    /*
                    if (results.service_total_count > 0) {
                        if (typeof sortText !== 'undefined' && sortText !== null && sortText !== '') {
                            $('#service-search-results .total-number').html('<span>' + sortText + '</span> で並べ替えました');
                        }
                        else {
                            $('#service-search-results .total-number').html('絞り込みの結果、<span>' + replaceThousandSeparator(results.service_total_count) + '</span> 件が該当しました');
                        }
                    } else {
                        $('#service-search-results .total-number').html('絞り込みの結果、該当するサービスが見つかりませんでした');
                    }
                    */
                    //WebappModalLock.view();
                    $('#service-search-results').hide();

                    $('html, body').animate({
                        scrollTop: $('#services').offset().top
                    }, 500, 'swing');


                } // if (apiResult.status == 'OK')
            },
            error: alertAjaxError,
            complete: function () {
                $('.imj-google-map-companies').hide();
                webapp.page.fixedFooter();
                //WebappModalLock.end();
                $img_loading.fadeOut('slow');
            }
        });
    }

    $('#service-search-form select, #service-search-form input').on('change', function() {

        // event tracking
        var title = $(this).parents('.side-form__item').children('p').text().match(/\s*(\S*).*/)[1];
        if ($(this).prop('type').toLowerCase() === 'radio') {
            eventTracking.send('サービス絞り込みradio' + ($(this).prop('checked') ? '選択' : '選択解除'), title + '=' + $(this).parent().text().match(/\s*(\S*).*/)[1]);
        }
        else if ($(this).prop('type').toLowerCase() === 'checkbox') {
            eventTracking.send('サービス絞り込みcheckbox' + ($(this).prop('checked') ? '選択' : '選択解除'), title + '=' + $(this).parent().text().match(/\s*(\S*).*/)[1]);
        }
        else if ($(this).prop('tagName').toUpperCase() === 'SELECT'){
            eventTracking.send('サービス絞り込みselect', title + '=' + $(this).find('option[value="' + $(this).val() + '"]').text().match(/\s*(\S*).*/)[1]);
        }

        // update services
        $('#trt').val('');
        updateServices(true);
    });

    // ソートボタン表示
    var i = 0;
    var length = $('.sort__list__item').length;
    $('.sort__list__item').each(function() {
        if (i > 0 && !$(this).hasClass('sort-others')) {
            var moveFlag = false;
            if ($('.sort__list').width() > 410) {
                if ($('.sort-others').css('display') == 'none') {
                    if (i < (length - 2)) {
                        $('.sort-others').css('display', '');
                        $('.sort-others__list').hide();
                        moveFlag = true;
                    }
                }
                else {
                    moveFlag = true;
                }
            }
            if (moveFlag) {
                $(this).removeClass('sort__list__item');
                $(this).appendTo('.sort-others__list ul');
                $(this).addClass('sort-others__list__item');

                if ($(this).children('span').length) {
                    $('.sort__list__item__caret').replaceWith('<span class="sort__list__item__caret">' + $(this).text() + '</span>');
                }
                else {
                    $(this).css('display', '');
                }
            }
            else {
                $(this).css('display', '');
            }
        }
        i++;
    });

    // ソートボタンクリック
    $('.sort__list a').on('click', function(){sort_services($(this));});
    $('.sort__list__item__caret').off('click');

    function sort_services(target) {
        var sortText = target.text();

        // event tracking
        if (typeof eventTracking === 'object') {
            eventTracking.send('サービス並べ替え', sortText);
        }

        // radar chart id
        var srid = target.parent().val();
        if (srid === 0) {
            srid = '';
        }
        $('#srid').val(srid);
        $('#trt').val('srt');
        change_sort_selection(srid);
        updateServices(true, sortText);
    }

    function change_sort_selection(srid) {

        // target
        var target = $('.sort__list__item[value="' + srid + '"]:first');
        if (target.size() === 0) {
            target = $('.sort-others__list__item[value="' + srid + '"]:first');
        }
        if (target.size() === 0) {
            srid = '';
            target = $('.sort__list__item[value=""]:first');
        }
        var sortText = target.text();

        // reset selected item
        var selected = $('.sort__list span:first');
        if (selected.hasClass('sort__list__item__caret')) {
            selected = $('.sort-others__list span:first');
            if (selected.parent().val() == srid) {
                return sortText;
            }
            selected.parent().css('display', '');
            $('.sort__list__item__caret').replaceWith('<a class="sort__list__item__caret">その他のランキング</a>');
        }
        else {
            if (selected.parent().val() == srid) {
                return sortText;
            }
        }
        selected.replaceWith('<a>' + selected.text() + '</a>');

        // set target item
        if (target.hasClass('sort-others__list__item')) {
            target.css('display', 'none');
            $('.sort__list__item__caret').replaceWith('<span class="sort__list__item__caret">' + target.text() + '</span>');
        }
        target.html('<span>' + target.text() + '</span>');
        $('.sort__list a').off('click');
        $('.sort__list a').on('click', function(){sort_services($(this));});
        $('.sort__list__item__caret').off('click');
        $('.sort-others__list').hide();
        return sortText;
    }

    $(window).on('popstate', function() {

        // CVパネル動作時は無視
        if ((typeof cvPanelApp === 'object') && cvPanelApp.cvpPropagatePopState == false) {
            return;
        }
        var srid = '';
        var trt = '';

        // 一度、検索フォームの全ての選択を解除
        $('#service-search-form select').val('');
        $('#service-search-form input[type=radio]').prop('checked', false);
        $('#service-search-form input[type=checkbox]').prop('checked', false);

        // GETパラメータで指定されている値を検索フォームに設定
        {
            var getParameters = location.search.slice(1).split('&');

            $(getParameters).each(function() {
                var questionId     = null;
                var choiceAnswerId = null;
                {
                    var values = this.split('=');

                    if (values.length == 2) {
                        var regx = /qi_(\d+)/;
                        if (regx.test(values[0])) {
                            questionId     = values[0].match(regx)[1];
                            choiceAnswerId = values[1];
                        }

                        // sort radar chart id
                        else if (values[0] === 'srid') {
                            srid = values[1];
                        }

                        // trigger type
                        else if (values[0] === 'trt') {
                            trt = values[1];
                        }
                    }
                }

                // 値が指定されているなら検索フォームに設定
                if (choiceAnswerId) {
                    var item = $('#choice-answer-' + choiceAnswerId);

                    if (item.length) {
                        item.prop('checked', true); // ラジオボタン / チェックボックス
                    } else {
                        $('#service-search-form select[name=qi_' + questionId + ']').val(choiceAnswerId); // セレクトボックス
                    }
                }
            });
        }

        // change sort selection
        var sortText = change_sort_selection(srid);
        $('#srid').val(srid);
        $('#trt').val('srt');

        // update services
        if (trt === 'srt') {
            updateServices(false, sortText);
        }
        else {
            updateServices(false);
        }
    });

});
