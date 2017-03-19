function index(dir,templatedir) {
	// 图片延迟加载
	$(".lazyload div").find("img").lazyload({ placeholder: templatedir+"images/index/84.gif", effect:"fadeIn" });
	// 推荐职位
	$(".comtip").hover(function() {
		$(this).closest('.famous-items').css("zIndex",65).find('.famous-more-info').show();
	}, function() {
		$(this).closest('.famous-items').css("zIndex",0).find('.famous-more-info').hide();
	});
	//紧急招聘
	$(function(){
		var _wrap=$('#emergencybox');
		var _interval=2000;
		var _moving;
		_wrap.hover(function(){
		clearInterval(_moving);
		},function(){
		_moving=setInterval(function(){
		var _field=_wrap.find('li:first');
		var _h=_field.outerHeight()+13;
		_field.animate({marginTop:-_h+'px'},600,function(){
		_field.css('marginTop',0).appendTo(_wrap.find("ul"));
		})
		},_interval)
		}).trigger('mouseleave');
	});
	// 搜索提示
	var hotKey = $('#keyForIndexSearch').autocomplete({
		serviceUrl:dir+"plus/ajax_common.php?act=hotword",
		minChars:1, 
		maxHeight:400,
		width:278,
		zIndex: 1,
		deferRequestBy: 0 
	});
	// 首页中部格子广告
	$(".comimgtip").hover(function() {
		$(this).css("zIndex",2).find('.ad-more-info').show();
	}, function() {
		$(this).css("zIndex",0).find('.ad-more-info').hide();
	});
	// 搜索类型展开
	$('.search-type').hover(function(){
		$(this).find('.search-type-drop').show();
	}, function(){
		$(this).find('.search-type-drop').hide();
	});
	// 选择找工作还是找人才
	$('.search-type-drop').bind('click', function() {
		var stype = $(this).attr("code"), tit = $(this).attr("title"), data = $(this).attr("data"),
			hstype = $(".search-type-show").attr("code"), htit = $(".search-type-show").attr("title"), hdata = $(".search-type-show").attr("data");
		$("#btnForIndexSearch").attr("code",stype);
		$("#keyForIndexSearch").attr("placeholder",data);
		$(this).attr("code",hstype).attr("title",htit).attr("data",hdata).find("a").text(htit);
		$(".search-type-show").attr("code",stype).attr("title",tit).attr("data",data).find("span").text(tit);
		$('.search-type-drop').hide();
	});
	// 显示地区弹出框
	$(".search-area-box").click(function() {
		var backgroundHtm = '<div id="bonfire-pageloader-i"></div>';
		var html = jQuery('body');
		html.append(backgroundHtm);
		$('.aui_outer').show();
		jQuery('.aui_outer').css("width",jQuery(".aui_outer .aui_border").width());
	});
	// 关闭下拉
	$(".selector-close").die().live('click', function(event) {
		$('.aui_outer').hide();
		$("#bonfire-pageloader-i").remove();
	});
	// 回车搜索
	$('#keyForIndexSearch').keydown(function(e) {
		if (e.keyCode==13) {
			search_location(dir);
		}
	});
	// 搜索按钮点击
	$("#btnForIndexSearch").click(function() {
		search_location(dir);
	});
	// 格子广告位最右侧的广告去除右边距
	$(".ad-row .ad-31").each(function(index, el) {
		if ((index+1)%3 == 0) {
			$(this).addClass('nomr');
		};
	});
	$(".ad-row .ad-51").each(function(index, el) {
		if ((index+1)%5 == 0) {
			$(this).addClass('nomr');
		};
	});
	// 判断格子广告是否显示，添加对应的样式控制上边距
	if ($(".ad-row .ad-51").length > 0 || $(".ad-row .ad-31").length > 0) {
		$(".index-data-wrap-i7").addClass('index-data-wrap-nmt');
	};
	if ($(".ad-row .ad-31").length > 0) {
		if ($(".a23058d")) {
			$(".a23058d").addClass('ad-job-list-i7');
		};
	};
	// 名企推荐广告位图片加透明度
	$(".block-ad-wrap img").each(function(index, el) {
		$(this).css('opacity', '1.0');
	}).hover(function(){
        $(this).css({opacity:0.5});
    }, function(){
        $(this).css({opacity:1.0});
    });
    // 名企推荐广告位在不足六个的时候补齐6个
    var recommendAdNum = $(".block-ad-wrap .block-ad-item").length, forLength = parseInt(6 - recommendAdNum), fillingHtm = '';
    if (forLength) {
    	for (var i = 0; i < forLength; i++) {
    		fillingHtm += '<div class="block-ad-item f-left"><a href="javascript:;" target="_blank"><img original="'+templatedir+'images/index/84.jpg" src="'+templatedir+'images/index/84.jpg" alt="广告位招租" width="202" height="153" border="0" /></a></div>';
    	}
    	$(".block-ad-wrap").append(fillingHtm);
    }
}
// 搜索跳转
function search_location(dir) {
	generateBackground();
	var listype = $("#btnForIndexSearch").attr('code');
	var key=$("input[name=keyForIndexSearch]").val();
	var citycategory=$("input[name=citycategory]").val();
	var page=1;
	$.get(dir+"plus/ajax_search_location.php", {"act":listype,"key":key,"citycategory":citycategory,"page":page},
		function (data,textStatus)
		 {	
			 window.location.href=data;
		 },"text"
	);
}
// 正在加载
function generateBackground() {
	var backgroundHtm = '<div id="bonfire-pageloader"><div class="bonfire-pageloader-icon"></div></div>';
	var html = jQuery('body');
	html.append(backgroundHtm);
	jQuery(window).resize(function(){
		 resizenow();
	});
	function resizenow() {
		var browserwidth = jQuery(window).width();
		var browserheight = jQuery(window).height();
		jQuery('.bonfire-pageloader-icon').css('right', ((browserwidth - jQuery(".bonfire-pageloader-icon").width())/2)).css('top', ((browserheight - jQuery(".bonfire-pageloader-icon").height())/2 + $(document).scrollTop() - 109));
	};
	resizenow();
}

/*地区选择弹出层填充数据*/
function city_filldata(fillID, data_resourcesP, data_resources, resultlist, showID, resultshowID, resulthidId) {
	var cityhtm = '', result_datapool = new Array();

	var sourse_city_length = parseInt(data_resourcesP.length);
	var rows_city = 0;
	var subscriptnum_city = 0;
	// 计算总行数
	if((sourse_city_length%5) == 0) {
		rows_city = sourse_city_length / 5;
	} else {
		if (sourse_city_length > 5*((sourse_city_length / 5) + 1)) {
			rows_city = (sourse_city_length / 5) + 1;
		} else {
			rows_city = (sourse_city_length / 5);
		}
	}
	for (var i = 0; i < rows_city; i++) {
		cityhtm += '<div class="data-row item-list data-row-nob clearfix">';
		cityhtm += '<div class="data-row-side-r615"><ul>';
		for (var j = 0; j < 5; j++) {
			if (data_resourcesP[subscriptnum_city]) {
				var citylist_array = data_resourcesP[subscriptnum_city].split(",");
				cityhtm += '<li><a title="'+citylist_array[1]+'" href="javascript:;" data="'+citylist_array[0]+'.0,'+citylist_array[1]+'" rel="'+citylist_array[0]+'.0" class="cat"><i class="data-icon data-icon-expend"></i>'+citylist_array[1]+'</a><a href="javascript:;" class="cat-touch"><label title="'+citylist_array[1]+'"><input type="checkbox" data="'+citylist_array[0]+'.0,'+citylist_array[1]+'" rel="'+citylist_array[0]+'.0" class="checkbox" name="LocalDataMultiC">'+citylist_array[1]+'</label></a></li>';
				subscriptnum_city ++;
			};
		};
		cityhtm += '</ul></div>';
		cityhtm += '</div>';
	};
	$(fillID).html(cityhtm);
	// 全部恢复
	if ($(resulthidId).val().length > 0) {
		var recoverid_array = $(resulthidId).val().split("_"),
			recovercn_array = new Array();
		$.each(recoverid_array, function(index, val) {
			var vspArray = val.split("."),
				vue = vspArray[1];
			if (vue == 0) {
				$.each(data_resourcesP, function(indexso, valso) {
					var vuesoArray = valso.split(",");
					if (vspArray[0] == vuesoArray[0]) {
						recovercn_array.push(vuesoArray[1]);
					}
				});
			} else {
				if (data_resources[vspArray[0]]) {
					var vuesArray = data_resources[vspArray[0]].split("|");
					$.each(vuesArray, function(indexson, valson) {
						var vuesonArray = valson.split(",");
						if (vue == vuesonArray[0]) {
							recovercn_array.push(vuesonArray[1]);
						}
					});
				};
			}
		});
		for (var i = 0; i < recoverid_array.length; i++) {
			result_datapool[i] = recoverid_array[i]+","+recovercn_array[i];
		};
		copyitem();
		copy_selected();
		var cata_array = $(fillID + " a.cat");
		$.each(cata_array, function() {
			var catcobj = $(this), catcheckedid_array = splitdata(catcobj.attr("data"));
			$.each(result_datapool, function(indexdp,valdp) {
				var rdatapool_array = splitdata(valdp);
				if (catcheckedid_array[0]==rdatapool_array[0] && rdatapool_array[1] !=0) {
					catcobj.addClass('cat-checked');
				} else if(catcheckedid_array[0]==rdatapool_array[0] && rdatapool_array[1] ==0) {
					catcobj.addClass('cat-checked');
					catcobj.next().find("input").attr("checked", !0);
				}
			})
		});
	}
	$(fillID + " a.cat").unbind().on("click",function() {
		var ace = $(this), cityarrayid_array = ace.attr("rel").split(".");
		if (data_resources[cityarrayid_array[0]]) { // 判断是否有二级地区
			var tcityhtm = '<div class="data-sub"><table cellpadding="0" cellspacing="0"><tbody>';
			var tcity_array = data_resources[cityarrayid_array[0]].split("|");
			var sourse_length = parseInt(tcity_array.length);
			var rows = 0;
			var subscriptnum = 0;
			if((sourse_length%2) == 0) {
				rows = sourse_length / 2;
			} else {
				rows = (sourse_length / 2) + 1;
			}
			for (var i = 0; i < rows; i++) {
				tcityhtm += '<tr>';
				for (var j = 0; j <= 2; j++) {
					if (tcity_array[subscriptnum]) {
						var tcitylist_array = tcity_array[subscriptnum].split(",");
						tcityhtm += '<td><a class="cat" data="'+cityarrayid_array[0]+'.'+tcitylist_array[0]+','+tcitylist_array[1]+'" href="javascript:;"><label title="'+tcitylist_array[1]+'"><input type="checkbox" class="checkbox" rel="'+cityarrayid_array[0]+'.'+tcitylist_array[0]+'" data="'+cityarrayid_array[0]+'.'+tcitylist_array[0]+','+tcitylist_array[1]+'" />'+tcitylist_array[1]+'</label></a></td>';
						subscriptnum ++;
					}
				};
				tcityhtm += '</tr>';
			};
			tcityhtm += '</tbody></table></div>';
			$(fillID).append(tcityhtm);
			var d = $(".data-sub"), l = $(this).closest("li");
			var g = l.find(".cat-touch input"), s = d.find("input");
			// 判断是否有选中
			var rco_resultlist = $(resultlist + " span");
			if (rco_resultlist.length > 0) {
				$.each(rco_resultlist, function() {
					var dsc = $(this).attr("data-code"),
					 	dscode_array = splitdata(dsc);
					if(dscode_array[1] == 0) {
						$.each(g, function() {
							if (dsc == $(this).attr("data")) {
							 	var idsc = $(this).is(':checked');
							 	$(this).attr("checked", idsc);
							 	s.prop({"checked":idsc, "disabled":idsc});
							}
						})
					} else {
						$.each(s, function() {
							if (dsc == $(this).attr("data")) {
							 	$(this).attr("checked", !0);
							}
						})
					}
				})
			} else {
				g.prop("checked", false);
			}
			var pleft = $(fillID).offset().left,
				ptop = $(fillID).offset().top,
				pwidth = $(fillID).outerWidth(),
				pheight = $(fillID).outerHeight(),
				ileft = $(this).offset().left,
				itop = $(this).offset().top,
				iwidth = $(this).outerWidth(),
				iheight = $(this).outerHeight(),
				dwidth = d.outerWidth(),
				dheight = d.outerHeight(),
				comparetop = parseInt(itop - ptop),
				dcleft = parseInt(ileft - pleft);
			var pbv = 2, bpv = 1;
			if($.browser.mozilla) {
				pbv = 2;
				bpv = 1;
			}
			l.addClass('cat-active');
			var n = $(this).next();
			pheight - dheight - iheight < comparetop ? (n.removeClass("sub").addClass("sub"), d.css({left:(pwidth-dwidth < dcleft ? (pwidth-dwidth-((pleft+pwidth)-(ileft+iwidth))) : dcleft),top:(comparetop + iheight) - bpv})) : (n.removeClass("sub").addClass("sub"), d.css({left:(pwidth-dwidth < dcleft ? (pwidth-dwidth-((pleft+pwidth)-(ileft+iwidth))) : dcleft),top:(comparetop + iheight) - bpv}));
			n.bind('mouseenter', function() {
				var e = $(this).closest("li");
				e.attr("data-active", !0)
			}).bind('mouseleave', function() {
				var a = $(this).closest("li");
				a.removeAttr("data-active"), setTimeout(function() {
					a.attr("data-active") || ($(fillID).find(".data-sub").remove(), a.removeClass("cat-active"))
				}, 200)
			});
			d.bind('mouseenter', function() {
				l.attr("data-active", !0)
			}).bind('mouseleave', function() {
				l.removeAttr("data-active"), setTimeout(function() {
					l.attr("data-active") || (d.remove(), l.removeClass("cat-active"))
				}, 200)
			});
			g.unbind('click').bind('click', function() {
				var ic = $(this).is(':checked'), gdata = $(this).attr("data");
				s.prop({"checked":ic, "disabled":ic});
				if (ic) {
					var gdata_array = splitdata(gdata);
					for (var i = 0; i < result_datapool.length; i++) {
						if (result_datapool[i]) {
							var icgdata_array = splitdata(result_datapool[i]);
							if (gdata_array[0] == icgdata_array[0]) {
								result_datapool.splice(i,1);
								i = -1;
							}
						}
					}
					result_datapool.push(gdata);
					if(isexceed()) {
						ace.addClass('cat-checked');
					} else {
						$(this).attr("checked",false);
						result_datapool.splice($.inArray(gdata,result_datapool),1);
					}
				} else {
					result_datapool.splice($.inArray(gdata,result_datapool),1);
					ace.removeClass('cat-checked');
				}
				copyitem();
			});
			s.unbind('click').bind('click', function() {
				var sic = $(this).is(':checked'), sdata = $(this).attr("data");
				if (sic) {
					result_datapool.push(sdata);
					isexceed() ? ace.addClass('cat-checked') : result_datapool.splice($.inArray(sdata,result_datapool),1);
				} else {
					result_datapool.splice($.inArray(sdata,result_datapool),1);
					var chnum = d.find(' :checkbox[checked]').length;
					if (chnum <= 0) {
						ace.removeClass('cat-checked');
					}
				}
				copyitem();
			});
		} else{
			var l = $(this).closest("li");
			var g = l.find(".cat-touch input");
			// 判断是否有选中
			var rco_resultlist = $(resultlist + " span");
			if (rco_resultlist.length > 0) {
				$.each(rco_resultlist, function() {
					var dsc = $(this).attr("data-code"),
					 	dscode_array = splitdata(dsc);
					if(dscode_array[0] == 0) {
						$.each(g, function() {
							if (dsc == $(this).attr("data")) {
							 	var idsc = $(this).is(':checked');
							 	$(this).attr("checked", idsc);
							}
						})
					}
				})
			} else {
				g.prop("checked", false);
			}
			l.addClass('cat-active');
			var n = $(this).next();
			n.bind('mouseenter', function() {
				var e = $(this).closest("li");
				e.attr("data-active", !0)
			}).bind('mouseleave', function() {
				var a = $(this).closest("li");
				a.removeAttr("data-active"), setTimeout(function() {
					a.attr("data-active") || ($(fillID).find(".data-sub").remove(), a.removeClass("cat-active"))
				}, 200)
			});
			g.unbind('click').bind('click', function() {
				var ic = $(this).is(':checked'), gdata = $(this).attr("data");
				if (ic) {
					result_datapool.push(gdata);
					if(isexceed()) {
						ace.addClass('cat-checked');
					} else {
						$(this).attr("checked",false);
						result_datapool.splice($.inArray(gdata,result_datapool),1);
					}
				} else {
					result_datapool.splice($.inArray(gdata,result_datapool),1);
					ace.removeClass('cat-checked');
				}
				copyitem();
			});
		};
	});
	// 复制选中项目
	function copyitem() {
		var resulthtm = '';
		$.each(result_datapool, function(indexrd,valrd) {
			var result_data_array = valrd.split(",");
			resulthtm += '<span data-code="'+result_data_array[0]+','+result_data_array[1]+'">'+result_data_array[1]+'<i class="data-icon data-icon-close"></i></span>';
		});
		$(resultlist).html(resulthtm);
		var spi = $(resultlist + " span");
		spi.unbind().bind('click', function() {
			var spidata = $(this).attr("data-code"),
				apid_array = splitdata(spidata);
			result_datapool.splice($.inArray(spidata,result_datapool),1);
			var catchecked_array = $(fillID + " a.cat-checked");
			$.each(catchecked_array, function() {
				var catcobj = $(this), catcheckedid_array = splitdata(catcobj.attr("data"));
				if(apid_array[0]==catcheckedid_array[0] && apid_array[1]==0){
					catcobj.removeClass('cat-checked');
					catcobj.next().find("input").attr("checked", false);
				} else if (apid_array[0]==catcheckedid_array[0] && apid_array[1]!=0) {
					var rdanum = 0;
					$.each(result_datapool, function(indexdp,valdp) {
						var rdatapool_array = splitdata(valdp);
						if (apid_array[0]==rdatapool_array[0] && apid_array[1]!=0) {
							rdanum ++;
						}
					});
					if (rdanum <= 0) {
						catcobj.removeClass('cat-checked');
					}
				}
			});
			copyitem();
		});
		$("#arscity").html(result_datapool.length);
		if (result_datapool.length > 0) {
			$(".cla").show();
			$(".cla").click(function() {
				result_datapool.splice(0,result_datapool.length);
				var catchecked_array = $(fillID + " a.cat-checked");
				$.each(catchecked_array, function(index, val) {
					$(this).removeClass('cat-checked');
				});
				copyitem();
			})
		} else {
			$(".cla").hide();
		}
	}
	// 分割data 返回数组
	function splitdata(arr) {
		if (arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		};
	}
	// 判断选择数量是否超出
	function isexceed() {
		if(result_datapool.length > 3) {
			alert("最多可选3个");
			return false;
		} else {
			return true;
		}
	}
	// 复制选中 赋值
	function copy_selected() {
		var selectedhtm = "";
		var selectedid_array = new Array();
		var selectedcn_array = new Array();
		for (var i = 0; i < result_datapool.length; i++) {
			if (result_datapool[i]) {
				var data_array = result_datapool[i].split(",");
				selectedid_array[i]=data_array[0];
				selectedcn_array[i]=data_array[1];
			};
		};
		selectedcn_array.length > 0 ? selectedhtm = selectedcn_array.join("+") : selectedhtm = "请输入工作地区";
		$(resultshowID).val(selectedhtm);
		$(resulthidId).val(selectedid_array.join("_"));
		$('.aui_outer').hide();
	}
	// 点击确定
	$("#ct-selector-save").click(function() {
		copy_selected();
		$("#bonfire-pageloader-i").remove();
	});
}