function replaceHtmlString(value)
{
    value = $('<div>').text(value).html();
    value = value.replace(/(\r\n|\r|\n)/g, '<br />');

    return value;
}

function replaceThousandSeparator(value)
{
    return value.toString().replace(/([0-9])(?=([0-9]{3})+$)/g, '$1,');
}

function alertAjaxError(jqXHR)
{
//    var message = 'Ajax通信エラー\n\n';
//    if (jqXHR.status) {
//        message += jqXHR.status;
//        if ((jqXHR.status == 515) || (jqXHR.status == 517)) {
//            var code = null;
//            if (typeof(jqXHR.responseJSON) !== 'undefined') {
//                code = jqXHR.responseJSON.code;
//            } else {
//                code = $.parseJSON(jqXHR.responseText).code;
//            }
//            message += '-' + code;
//        }
//    } else {
//        message += 'サーバーに接続できません。';
//    }
//
//    alert(message);
}