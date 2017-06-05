/**
 *
 * サービスのwebサイトアクセス
 * @author Takano 2017/01/10
 * @author Kazuhiro Nigara 2017/04/13
 *
 */
jQuery(function($) {

    // initial setting
    serviceWebsiteVisitInitialize();

    // click link
    $('.service-hp-link').on('click', function(){serviceWebsiteVisit(this);});
});

//service website visit initial setting
function serviceWebsiteVisitInitialize() {
    $(".article").each(function(i) {
        var target = this;
        var serviceId = $(target).data('sid');
        //console.log('serviceWebsiteVisit() serviceId=' + serviceId);
        
        if (serviceId === undefined || serviceId == '') {
            return true;
        }

        // call ajax
        $.ajax({
            async: true,
            type: 'get',
            url: apiUrl + '/service_website_visit?type=inq&sid=' + serviceId ,
            data: '',
            dataType: 'json',
            success: function(apiResult) {
                //console.log('serviceWebsiteVisit()ajax success');
                //console.log(apiResult.results);
                if (apiResult.results === 'visited') {
                    $(target).find('.service-hp-link').addClass('hp-link--visited');
                    $(target).addClass('article--visited');
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alertAjaxError(XMLHttpRequest);
            },
            complete: function () {
            }
        });
    });
}

// service website visit
function serviceWebsiteVisit(target) {
    var serviceId = $(target).data('sid');
    //var userId = $(target).data('uid');
    //console.log('serviceWebsiteVisit() serviceId=' + serviceId + ' userId=' + userId);

    // event tracking
    eventTracking.send('サービスwebサイトアクセス', serviceId);

    // change card style
    //$(target).addClass('hp-link--visited');
    //$(target).parents('.article').addClass('article--visited');
    $(target).closest('.article').find('.service-hp-link').addClass('hp-link--visited');
    $(target).closest('.article').addClass('article--visited');

    // call ajax
    $.ajax({
        async: true,
        type: 'get',
        url: apiUrl + '/service_website_visit?type=set&sid=' + serviceId ,
        data: '',
        dataType: 'json',
        success: function(apiResult) {
            //console.log('serviceWebsiteVisit()ajax success');
            //console.log(apiResult.results);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alertAjaxError(XMLHttpRequest);
        },
        complete: function () {
        }
    });
}
