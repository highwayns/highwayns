/**
 * pc/list/index.js
 */
$(function(){

    //slider
    webapp.slider.normal('.imj-slider-reviews', {
        speed: 1000,
        controls: true,
        auto: true,
        pause: 4000,
        slideWidth: 360,
        slideMargin: 20,
        maxSlides: 2,
        moveSlides: 1,
        pager: false,
        autoHover: true,
        prevText: '<i class="fa fa-angle-left"></i>',
        prevSelector: '.imj-reviews-control-prev',
        nextText: '<i class="fa fa-angle-right"></i>',
        nextSelector: '.imj-reviews-control-next',
    });

    //modal
    webapp.modal.normal('.imj-modal-pickup');
    webapp.modal.normal('.imj-modal');
//    webapp.modal.normal('.imj-modal-1', '.imj-target-modal-1');
    webapp.modal.normal('.imj-modal-2', '.imj-target-modal-2');
//    $('#pref').on('change', function () {
//        if ($(this).val()) {
//            $('.imj-target-modal-close').trigger('click');
//            setTimeout(function(){
//                $('.imj-modal-2').trigger('click');
//            },500);
//        }
//    });
//    webapp.modal.normal('.imj-modal-consult', '.imj-target-modal-consult');
    //$('#xxx1,#xxx2,#xxx3,#xxx4').on('click', function () {
    $("input[name='nonpublic_info_input']").on('click', function () {
        var target = $("label[for='" + $(this).attr("id") + "']");
        $('#nonpublic_info_kind').text(target.text());
        $('#nonpublic_info_ppt').val('nonpublic_info_' + $(this).val());
        $('#nonpublic_info_type').val($(this).val());
        setTimeout(function(){
            modal = $('.imj-target-modal-consult').remodal();
            $('.imj-target-modal-consult').show();
            modal.open();
            setTimeout(function(){
                $("input[name='nonpublic_info_input']").prop('checked', false);
            }, 500);
        }, 500);
    });

    webapp.modal.normal('.imj-modal-badge', '.imj-target-modal-badge');
    $('.imj-badge-button-close').on('click', function () {
        $(this).parent().parent().hide();
    });

    //form
    webapp.form.requiredForm();
    webapp.form.removeError();
    webapp.form.resetRadio();
    webapp.form.validationFrom();
    webapp.form.optionForm('.imj-form-select'); //最適業者
    webapp.form.moveLabel();

    //open
    webapp.open.more('.imj-more');
    //webapp.open.normal('.imj-more-data', '.imj-target-more-data');
    webapp.open.slide('.imj-slide-side', '.imj-target-slide-side', 'side-form__item__name--up', 'side-form__item__name--down', function(){
        $(document.body).trigger('sticky_kit:recalc');
    });
    webapp.open.hover('.imj-slide-doc', '.imj-target-slide-doc');
    webapp.open.hover('.imj-open-hover', '.imj-target-open-hover');

    //balloon
    webapp.balloon.normal('.imj-balloon-side', null, 'right');
    webapp.balloon.normal('.imj-balloon-official', null, 'bottom');


    /* webapp.page 専用
    ========================================================================== */
    webapp.page = {

        //chart表示
        charts: {
            view: function () {
                if (webapp.ie8()) {
                    //IE8未対応
                    $('.radarchart').remove();
                    return;
                }
                for (var i = 0; i < $('.radarchart').length; i++) {
                    webapp.page.chart.chartIndex = i;
                    webapp.page.chart.view();
                }
            }
        },
        chart: {
            chartIndex: 0,
            view: function () {
                Chart.defaults.global.legend.display = false;
                Chart.defaults.global.defaultFontFamily = "'Century Gothic','ヒラギノ角ゴ Pro W3','Hiragino Kaku Gothic Pro','メイリオ',Meiryo,'ＭＳ Ｐゴシック',Osaka,sans-serif";
                Chart.defaults.global.responsive = false;

                var $chart = $('.radarchart').eq(this.chartIndex);
                var data = {
                    labels: $.parseJSON('[' + $('.chartItems').eq(this.chartIndex).val() + ']'),
                    datasets: [
                        {
                            label: "",
                            backgroundColor: "rgba(71,190,182,0.2)",
                            borderColor: "rgba(71,190,182,1)",
                            pointBackgroundColor: "rgba(71,190,182,1)",
                            pointBorderColor: "rgba(71,190,182,1)",
                            pointHoverBackgroundColor: "#fff",
                            pointHoverBorderColor: "rgba(71,190,182,1)",
                            borderWidth: 4,
                            pointBorderWidth: 3,
                            data: $.parseJSON('[' + $('.chartData').eq(this.chartIndex).val() + ']')
                        }
                    ],
                    scaleShowLabels: false
                };
                var options = {
                    scale: {
                        ticks: {
                            min: 0,
                            max: 5,
                            fontSize: 12
                        },
                        gridLines: {
                            color: "rgba(0, 0, 0, 0.2)",
                            lineWidth: 1,
                        },
                        pointLabels: {
                            fontSize: 10,
                            fontColor: "#2b374e",
                        },
                        angleLines: {
                            display: true,
                            color: "rgba(0, 0, 0, 0.2)",
                            lineWidth: 1
                        }
                    }
                };
                var myRadarChart = new Chart($chart, {
                    type: 'radar',
                    data: data,
                    options: options
                });

            }
        },

        //モーダル表示
        openModal: function(data) {

            var defaults = {
                small: false,
                hide: false,
                title: '',
                conditions: '',
                description: '',
                event: ''
            };
            var data = $.extend(defaults, data);

            // event tracking
            if (data.event === 'scroll') {
                //eventTracking.send('サービスポップアップ', 'スクロール', null, 'scroll');
                eventTracking.send('サービスポップアップ', 'スクロール', null, 'click');
            }
            else {
                eventTracking.send('サービスポップアップ', (data.event === 'sort' ? '並べ替え' : '絞り込み'));
            }

            // modal
            $imjModalPickup = $('.imj-modal-pickup');
            $imjTargetModalPickup = $('.imj-target-modal-pickup');
            if (data.small) {
                $imjTargetModalPickup.addClass('imc-modal--964');
            }
            if (data.hide) {
                $('.pickup2-company__no', $imjTargetModalPickup).hide();
                $('.modal-pickup2__list-title', $imjTargetModalPickup).hide();
            }
            if (data.title !== '') {
                $('.modal-pickup2__title', $imjTargetModalPickup).html(data.title);
            }
            if (data.conditions !== '') {
                $('.modal-pickup2__conditions', $imjTargetModalPickup).html(data.conditions);
            }
            if (data.description !== '') {
                $('.modal-pickup2__person__description', $imjTargetModalPickup).html(data.description);
            }
            $('.imj-modal-pickup').trigger('click').unbind();
        },

        /* 追従フッター
            .imj-fixed-footer-custom
        ======================================== */
        fixedFooter: function () {
            var $imjFixedFooter = $('.imj-fixed-footer-custom');
            if ($imjFixedFooter.length > 0) {

                //フッター部分を大きくする
                $('.imc-layout--footer').css('padding-bottom', '120px');

                //スタイル
                $imjFixedFooter.css({
                    'position': 'fixed',
                    'bottom': '-110px',
                    'left': 0,
                    'right': 0,
                    'z-index': 3000
                }).hover(
                    function () {
                        $(this).stop().animate({
                            'bottom': 0
                        });
                    },
                    function () {
                        $(this).stop().animate({
                            'bottom': '-56px'
                        });
                    }
                );

                //表示idでtop決定
                var top = 0;
                if ($('#no3').length > 0) {
                    top = $('#no3').offset().top;
                } else if ($('#no2').length > 0) {
                    top = $('#no2').offset().top;
                } else if ($('#no1').length > 0) {
                    top = $('#no1').offset().top;
                }

                //表示(点滅あり)
                var timer = null;
                $(window).on('load scroll', function(){
                    if ($(this).height() + $(this).scrollTop() > top) {
                        $imjFixedFooter.stop().animate({
                            'bottom': '-56px',
                            'opacity': '0.2'
                        });
                    } else {
                        $imjFixedFooter.stop().animate({
                            'bottom': '-110px',
                            'opacity': '0.2'
                        });
                    }

                    if (timer) {
                        clearTimeout(timer);
                    }

                    timer = setTimeout(function(){
                        if ($(this).height() + $(this).scrollTop() > top) {
                            $imjFixedFooter.stop().animate({
                                'bottom': '-56px',
                                'opacity': '1.0'
                            });
                        } else {
                            $imjFixedFooter.stop().animate({
                                'bottom': '-110px',
                                'opacity': '1.0'
                            });
                        }
                    }, 500) ;
                });
                /*
                //表示(点滅なし)
                $(window).on('load scroll', function(){
                    if ($(this).height() + $(this).scrollTop() > top) {
                        $imjFixedFooter.stop().animate({
                            'bottom': '-56px'
                        });
                    } else {
                        $imjFixedFooter.stop().animate({
                            'bottom': '-110px'
                        });
                    }
                });
                */
            }
        }
    };

    //chart表示 （検索完了時にも呼ぶ）
    webapp.page.charts.view();
    //footer
    webapp.page.fixedFooter();



    /* page 専用
    ========================================================================== */
    var page = {

        /* 詳細を見る
            .imj-more-data
        ======================================== */
        openSlide: function() {

            $(document).on('click', '.imj-more-data', function(){

                var $self = $(this);
                var $target_selector = $self.next();

                if ($target_selector.is(':hidden')) {
                    $target_selector.slideDown('normal', function(){
                        $self.text('詳細を閉じる');
                        $self.addClass('imc-caret--up');
                        $self.removeClass('imc-caret--down');
                    });

                } else {
                    $target_selector.slideUp('fast', function(){
                        $self.text('詳細を見る');
                        $self.addClass('imc-caret--down');
                        $self.removeClass('imc-caret--up');
                    });

                }

                return false;

            });
        },

        /* window.open
            .imj-window-open
        ======================================== */
        windowOpen: function () {
            if ($('.imj-window-open').length > 0) {
                $(document).on('click', '.imj-window-open', function(){
                    window.open(this.href, 'homepage', 'width=1080, height=600, resizable=yes, scrollbars=yes');
                    return false;
                });
            }
        },

        /* openModal
            .imj-open-modal
        ======================================== */
        modalOpened: false, // scrollイベントの重複検出防止用
        openModal: function () {
            if ($('.imj-open-modal').length > 0) {
                //$(window).on('load scroll', function(){
                $(window).on('scroll', function(){
                    if ((page.modalOpened === false) && ($(this).scrollTop() > $('.imj-open-modal').offset().top - 300)) {
                        if ($('#pickup-service-data').data('count') > 10) {

                            // check parameter
                            var query = document.location.search.substring(1);
                            var parameters = query.split('&');
                            var values;
                            var open = false;
                            for (var i = 0; i < parameters.length; i++) {
                                values = parameters[i].split('=');
                                if (values.length >= 2) {
                                    if (values[0] === 'pn' && values[1] !== '1') {
                                        open = false;
                                        break;
                                    }
                                    if (values[0].substr(0, 3) === 'qi_' || values[0] === 'srid' || values[0] === 'trt') {
                                        open = true;
                                    }
                                }
                            }
                            if (open) {
                                //No.1
                                page.modalOpened = true;
                                webapp.page.openModal({
                                    //small: true
                                    //title: '<span>リスティング代行</span>の<br>業者選びでお悩みですか？',
                                    //conditions: '<span>希望条件：</span>Eコマース対応 / WP対応 / 低価格',
                                    //description: 'いかがですか？この他にも、おすすめの業者はたくさんあります。<br>あなたの要望をコンシェルジュがお伺いし、<br>該当した○○社の中から、ぴったりの業者をご提案いたします。',
                                    event: 'scroll'
                                });
                            }
                        }
                        //else if (false) {
                        //    //No.2
                        //    webapp.page.openModal({
                        //        title: '<span>リスティング代行</span>の<br>業者選びでお悩みですか？',
                        //        description: 'いかがですか？この他にも、おすすめの業者はたくさんあります。<br>あなたの要望をコンシェルジュがお伺いし、<br>○○に強い、ぴったりの業者をご提案いたします。'
                        //    });
                        //}
                        //else if (false) {
                        //    //No.3
                        //    webapp.page.openModal({
                        //        title: '<span>リスティング代行</span>の<br>業者選びでお悩みですか？',
                        //        conditions: '<span>希望条件：</span>Eコマース対応 / WP対応 / 低価格',
                        //        description: 'いかがですか？この他にも、おすすめの業者はたくさんあります。<br>あなたの要望をコンシェルジュがお伺いし、<br><span>○○</span>に強い、ぴったりの業者をご提案いたします。'
                        //    });
                        //}
                    }
                });

                //if (false) {
                //    //No.8
                //    webapp.page.openModal({
                //        hide: true,
                //        title: '<span>○○の業者選びでお悩みですか？</span>',
                //        description: '上記3社をご覧いただきました。<br>この他にも、おすすめの業者はたくさんあります。<br>あなたの要望をコンシェルジュがお伺いし、<br>最適な業者をご提案いたします。'
                //    });
                //}
            }
        }

    }

    //実行
    page.openSlide();
    page.windowOpen();
    page.openModal();

});
