/**
 *
 * 最適業者の絞り込み
 * @author Takano 2016/11/29
 * @author Yosuke Sugawara 2017/01/05
 *
 */
jQuery(function($) {

    /**
     * htdocs/main/js/v2/pc/list/index.jsからコピペ
     * @author Yosuke Sugawara 2017/01/05
     */
    function displayRaderChart(chartIndex) {
        Chart.defaults.global.legend.display = false;
        Chart.defaults.global.defaultFontFamily = "'Century Gothic','ヒラギノ角ゴ Pro W3','Hiragino Kaku Gothic Pro','メイリオ',Meiryo,'ＭＳ Ｐゴシック',Osaka,sans-serif";
        Chart.defaults.global.responsive = false;

        var $chart = $('#modal-pickup__list .radarchart').eq(chartIndex);
        var data = {
            labels: $.parseJSON('[' + $('#modal-pickup__list .chartItems').eq(chartIndex).val() + ']'),
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
                    data: $.parseJSON('[' + $('#modal-pickup__list .chartData').eq(chartIndex).val() + ']')
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

    function extractServices() {

        // event tracking
        if (typeof eventTracking === 'object') {
            var label = '';
            for (var i = 1; i <= 3; i++) {
                if ($('#point' + i).val() !== 0 && $('#point' + i).val() !== '') {
                    if (label != '') {
                        label += ' ';
                    }
                    label += $('#point' + i + ' option:selected').text();
                }
            }
            label += ' ';
            label += $('#pref option:selected').text();
            eventTracking.send('サービス3社絞り込み', label);
        }

        // extract services
        var form = $('#services-extract-form');
        $.ajax({
            async: true,
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            dataType: 'json',
            success: function(apiResult) {
                //console.log('extractServices()ajax success');
                //console.log(apiResult);
                if (apiResult.status == 'OK') {
                    var results = apiResult.results;
                    //console.log('results=' + results);
                    $('#modal-pickup__list').html(results);

                    /*
                     * Bug fix
                     * @author Yosuke Sugawara 2017/01/05
                     */
                    {
                        // webapp.page.charts.view();

                        if (webapp.ie8()) {
                            //IE8未対応
                            $('#modal-pickup__list .radarchart').remove();
                            return;
                        }

                        for (var i = 0; i < $('#modal-pickup__list .radarchart').length; i++) {
                            displayRaderChart(i);
                        }
                    }

                    $('.imj-target-modal-close').trigger('click');
                    setTimeout(function(){
                        $('.imj-modal-2').trigger('click');
                    },500);
                }
            },
            error: alertAjaxError,
            complete: function () {
            }
        });
    }

    // click button
    $('.imj-modal-1').on('click', function () {
        var modal;
        if ($('#point1').val() !== '' || $('#point2').val() !== '' || $('#point3').val() !== '') {
            if (($('#point1').val() !== '' && $('#point1').val() === $('#point2').val()) || ($('#point2').val() !== '' && $('#point2').val() === $('#point3').val()) || ($('#point3').val() !== '' && $('#point3').val() === $('#point1').val())) {
                modal = $('.imj-target-modal-error').remodal();
                $('.imj-target-modal-error').show();
                modal.open();
            }

            // 地域軸の無いリストページに対応 @author Yosuke Sugawara 2016/12/27
            else if (!$('#category_pref')) {
                extractServices();
            }

            else if ($('#category_pref').val() !== '') {
                $('#pref').val($('#category_pref').val()); // for event tracking
                $('#area').val($('#category_pref').val()); // for ajax
                extractServices();
            }
            else {
                $('#pref').val('');
                modal = $('.imj-target-modal-1').remodal();
                $('.imj-target-modal-1').show();
                modal.open();
            }
        }
        return false;
    });

    // select prefecture
    $('#pref').on('change', function () {
        //console.log($(this).val() + ' ' + $('#area').val());
        if ($(this).val()) {
            $('#area').val($(this).val());
            extractServices();
        }
    });
});