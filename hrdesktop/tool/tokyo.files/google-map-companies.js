/**
 * @author Kazuhiro Nigara 2017/01/10
 * list, list-static
 *
 */
$(function(){
    // GoogleMapCompanies
    var GoogleMapCompanies = function(id) {
        this.geocoder = new google.maps.Geocoder();
        this.number = 0;
        this.map;
        this.id = id;
        this.index = 0;
        this.lats = [];
        this.lngs = [];
    }
    GoogleMapCompanies.prototype.setData = function (name, address) {
        var self = this;
        (function(number) {
            setTimeout(function() {
                if (address !== undefined && address != null) {
                    var latlng, marker, infowindow;
                    self.geocoder.geocode({"address": address}, function(results, status) {
                        if (status === google.maps.GeocoderStatus.OK) { 
                            latlng = results[0].geometry.location;
                            self.lats.push(latlng.lat());
                            self.lngs.push(latlng.lng());

                            //地図
                            if (self.index == 0) {
                                self.map = new google.maps.Map(document.getElementById(self.id), {
                                    zoom: 10,
                                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                                    center: new google.maps.LatLng({lat: latlng.lat(), lng: latlng.lng()}),
                                    scrollwheel: false
                                });
                            }
                            //マーカー
                            marker = new google.maps.Marker({
                                position: new google.maps.LatLng({lat: latlng.lat(), lng: latlng.lng()}),
                                map: self.map
                            });
                            //ウインドウ
                            infowindow = new google.maps.InfoWindow({
                                content: name
                            });
                            //イベント
                            marker.addListener('click', function() {
                                infowindow.open(self.map, marker);
                            });
                            //範囲
                            self.map.fitBounds(new google.maps.LatLngBounds(
                                new google.maps.LatLng(Math.max.apply(null, self.lats), Math.min.apply(null, self.lngs)),
                                new google.maps.LatLng(Math.min.apply(null, self.lats), Math.max.apply(null, self.lngs))
                            ));

                            self.index++;
                        } else {
                            //console.log(status);
                        }
                    });
                }
            }, 600 * number);   //Geocoderのアクセス制限のため
            self.number++;
        })(self.number);
    };

    //実行
    var googleMapCompanies = new GoogleMapCompanies('google-map-companies');
    for (var i = 0; i < $('.google-map-company-names').length; i++) {
        var name = $('.google-map-company-names').eq(i).val();
        var address = $('.google-map-company-addresses').eq(i).val();
        googleMapCompanies.setData(name, address);   //名前,住所
    }
});