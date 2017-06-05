/**
 *
 * @author Yosuke Sugawara
 *
 */
jQuery(function($) {
    var serviceResultImageOffset = 16;


    $('.imj-service-result-image-append').on('click', function() {
        var target = $(this);

        $.ajax({
            async: true,
            type: 'get',
            url: apiUrl + '/service_result_images_get',
            data: {
                category_type: target.data('category-type'),
                offset: serviceResultImageOffset,
            },
            dataType: 'json',
            success: function(apiResult) {
                if (apiResult.status == 'OK') {
                    var results = apiResult.results;

                    {
                        var html = '';
                        $.each(results.service_result_images, function() {
                            html += '<li class="imc-list__item imc-list__item--inline imc-list__item--list">';
                                html += '<a href="#" class="imc-hover-border imj-modal">';
                                    html += '<div class="imc-item-site">';
                                        html += '<figure class="imc-item-site__image"><img src="' + imageServerUrl + '?f=' + this.image_saved_file + '&w=178&h=132&ct=top&adir=service_results_image&id=' + this.service_id + '" alt="' + this.company_name + '" width="178" height="132"></figure>';
                                        html += '<p class="imc-item-site__title">制作</p>';
                                        html += '<p class="imc-item-site__description">' + this.company_name + '</p>';
                                    html += '</div>';
                                html += '</a>';
                                html += '<div class="imc-modal imc-modal--0" style="display: none;">';
                                    html += '<button data-remodal-action="close" class="imc-modal__close"></button>';
                                    html += '<img width="608" height="480" src="' + imageServerUrl + '?f=' + this.image_saved_file + '&w=608&h=480&ct=top&adir=service_results_image&id=' + this.service_id + '">';
                                html += '</div>';
                            html += '</li>';
                        });

                        $('#service-results-images ul.imj-images').append(html);

                        webapp.modal.normal('.imj-modal');
                    }

                    serviceResultImageOffset += 16;

                    if (serviceResultImageOffset >= results.total_count) {
                        target.hide();
                    }
                } // if (apiResult.status == 'OK')
            },
            error: alertAjaxError,
        });
    });
});