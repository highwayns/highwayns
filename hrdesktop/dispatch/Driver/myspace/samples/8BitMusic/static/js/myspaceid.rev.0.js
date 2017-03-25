
var msOptions = {
	api_base:'http://api.myspace.com/openid',
	server:"http://localhost:9090", //tied to app registration
	realm:'/',
	returnTo:'finish_auth.aspx',
	consumer:'18bed0f4247a4f79bc9941bfed5b534c',
	popupSize:{ width:580, height:600}
};
msOptions.popupPosition = {
	xOffset: function(){
		return(screen.width - msOptions.popupSize.width) / 2;
	},
	yOffset: function(){
		return (screen.height - msOptions.popupSize.height) / 2;
	}
};


//the myspaceid base object
MySpaceID = function (options){
var that = this;
that.options = options;

window.open(
	MySpaceID.Util.getPopupUrl(that.options),
	'MySpaceID',
	MySpaceID.Util.getWindowSizeStr(that.options)
	);
};

//a bunch of static methods.
MySpaceID.Util = {

	getPopupUrl:function(options){
		var _url  = options.api_base + "?";
			_url += "openid.ns=" + MySpaceID.Util.urlencode("http://specs.openid.net/auth/2.0");
			_url += "&openid.ns.oauth=" + MySpaceID.Util.urlencode("http://specs.openid.net/extensions/oauth/1.0");
			_url += "&openid.oauth.consumer=" + MySpaceID.Util.urlencode(options.consumer);
			_url += "&openid.mode=checkid_setup";
			_url += "&openid.claimed_id=" + MySpaceID.Util.urlencode("http://specs.openid.net/auth/2.0/identifier_select");
			_url += "&openid.identity=" + MySpaceID.Util.urlencode("http://specs.openid.net/auth/2.0/identifier_select");
			_url += "&openid.return_to=" + MySpaceID.Util.urlencode(options.server + options.realm + options.returnTo);
			_url += "&openid.realm=" + MySpaceID.Util.urlencode(options.server + options.realm);
		return _url;
	},
	getWindowSizeStr:function(options){
		return	"width="	+options.popupSize.width+
			",height="	+options.popupSize.height+
			",left="	+options.popupPosition.xOffset()+
			",top="		+options.popupPosition.yOffset();
	},

	urlencode: function ( str ) {
	    // http://kevin.vanzonneveld.net
	    // +   original by: Philip Peterson
	    // +   improved by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
	    // +      input by: AJ
	    // +   improved by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
	    // +   improved by: Brett Zamir
	    // %          note: info on what encoding functions to use from: http://xkr.us/articles/javascript/encode-compare/
	    // *     example 1: urlencode('Kevin van Zonneveld!');
	    // *     returns 1: 'Kevin+van+Zonneveld%21'
	    // *     example 2: urlencode('http://kevin.vanzonneveld.net/');
	    // *     returns 2: 'http%3A%2F%2Fkevin.vanzonneveld.net%2F'
	    // *     example 3: urlencode('http://www.google.nl/search?q=php.js&ie=utf-8&oe=utf-8&aq=t&rls=com.ubuntu:en-US:unofficial&client=firefox-a');
	    // *     returns 3: 'http%3A%2F%2Fwww.google.nl%2Fsearch%3Fq%3Dphp.js%26ie%3Dutf-8%26oe%3Dutf-8%26aq%3Dt%26rls%3Dcom.ubuntu%3Aen-US%3Aunofficial%26client%3Dfirefox-a'

	    var histogram = {}, tmp_arr = [];
	    var ret = str.toString();

	    var replacer = function(search, replace, str) {
	        var tmp_arr = [];
	        tmp_arr = str.split(search);
	        return tmp_arr.join(replace);
	    };

	    // The histogram is identical to the one in urldecode.
	    histogram["'"]   = '%27';
	    histogram['(']   = '%28';
	    histogram[')']   = '%29';
	    histogram['*']   = '%2A';
	    histogram['~']   = '%7E';
	    histogram['!']   = '%21';
	    histogram['%20'] = '+';

	    // Begin with encodeURIComponent, which most resembles PHP's encoding functions
	    ret = encodeURIComponent(ret);

	    for (search in histogram) {
	        replace = histogram[search];
	        ret = replacer(search, replace, ret); // Custom replace. No regexing
	    }

	    // Uppercase for full PHP compatibility
	    return ret.replace(/(\%([a-z0-9]{2}))/g, function(full, m1, m2) {
	        return "%"+m2.toUpperCase();
	    });

	    return ret;
	}

};