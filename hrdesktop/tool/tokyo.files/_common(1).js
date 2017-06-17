/**
 * pc/_common.js
 */

//for IE8
if(!Object.keys){
    Object.keys = function(obj){
        var keys = [];
        for(var i in obj){
            if(obj.hasOwnProperty(i)){
                keys.push(i);
            }
        }
        return keys;
    };
}

$(function(){

    webapp.placeholder();
    webapp.globalNavi();
    webapp.globalNaviCategory();
    webapp.breadcrumb();
    webapp.link();          //内部リンク
    webapp.topLink();       //トップへリンク
    webapp.moveLogo();      //ロゴ移動

    //sticky
    if ($('.imj-slider-buyer').length == 0) {
        //サイドに利用者コンテンツ（スライド）がない場合実施、ある場合はスライドを実装してから、実施している
        webapp.sticky();
    }
    
});
