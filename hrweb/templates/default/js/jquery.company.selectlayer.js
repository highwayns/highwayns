/*职位选择弹出层填充数据*/
function job_filldata(fillID, data_resourcesP, data_resources, resultlist, showID, resultshowID, resulthidId, dir) {
	var jobhtm = '', result_datapool = new Array();
	$.each(data_resourcesP, function(indexp, valp) {
		 var pjob_array = valp.split(",");
		 	jobhtm += '<div class="data-row data-row-odd item-list clearfix">';
		 jobhtm += '<div class="data-row-side">'+pjob_array[1]+'</div>';
		 jobhtm += '<div class="data-row-side-r"><ul>';
		 var job_array = data_resources[pjob_array[0]].split("|");
		 $.each(job_array, function(index, val) {
		 	 var joblist_array = val.split(",");
		 	 jobhtm += '<li><a title="'+joblist_array[1]+'" href="javascript:;" data="'+pjob_array[0]+'.'+joblist_array[0]+'.0,'+joblist_array[1]+'" rel="'+pjob_array[0]+'.'+joblist_array[0]+'" class="cat"><i class="data-icon data-icon-expend"></i>'+joblist_array[1]+'</a><a href="javascript:;" class="cat-touch"><label title="'+joblist_array[1]+'" data="'+pjob_array[0]+'.'+joblist_array[0]+'.0,'+joblist_array[1]+'" rel="'+pjob_array[0]+'.'+joblist_array[0]+'">'+joblist_array[1]+'</label></a></li>';
		 });
		 jobhtm += '</ul></div>';
		 jobhtm += '</div>';
	});
	$(fillID).html(jobhtm);
	// 全部恢复
	if ($(resulthidId).val().length > 0) {
		var resid = $("#category").val();
		$(fillID + " a.cat").each(function(index, el) {
			var resrelidArray = $(this).attr("rel").split(".");
			if (resid == resrelidArray[1]) {
				$(this).addClass('cat-checked');
			}
		})
	}
	$(fillID + " a.cat").unbind().on("click",function() {
		var ace = $(this), jobarrayid_array = ace.attr("rel").split("."), datashtm = ace.attr("data"), datashtmArray = datashtm.split(",");
		if (data_resources[jobarrayid_array[1]]) { // 判断是否有三级分类
			var tjobhtm = '<div class="data-sub"><table cellpadding="0" cellspacing="0"><tbody>';
			var tjob_array = data_resources[jobarrayid_array[1]].split("|");
			var sourse_length = parseInt(tjob_array.length);
			var rows = 0;
			var subscriptnum = 0, tm = 0;
			if((sourse_length%2) == 0) {
				rows = sourse_length / 2;
			} else {
				rows = (sourse_length / 2) + 1;
			}
			for (var i = 0; i <= rows; i++) {
				tjobhtm += '<tr>';
				for (var j = 0; j < 2; j++) {
					if (tjob_array[subscriptnum]) {
						if (tm == 0) {
							tjobhtm += '<td><a class="cat" data="'+datashtm+'" href="javascript:;"><label title="'+datashtmArray[1]+'" rel="'+datashtmArray[0]+'" data="'+datashtm+'"><font style="font-weight:bold;">不限</font></label></a></td>';
							tm ++;
						} else {
							var tjoblist_array = tjob_array[subscriptnum].split(",");
							tjobhtm += '<td><a class="cat" data="'+jobarrayid_array[0]+'.'+jobarrayid_array[1]+'.'+tjoblist_array[0]+','+tjoblist_array[1]+'" href="javascript:;"><label title="'+tjoblist_array[1]+'" rel="'+jobarrayid_array[0]+'.'+jobarrayid_array[1]+'.'+tjoblist_array[0]+'" data="'+jobarrayid_array[0]+'.'+jobarrayid_array[1]+'.'+tjoblist_array[0]+','+tjoblist_array[1]+'">'+tjoblist_array[1]+'</label></a></td>';
								subscriptnum ++;
						}
					}
				};
				tjobhtm += '</tr>';
			};
			tjobhtm += '</tbody></table></div>';
			$(fillID).append(tjobhtm);
		} else{
			var tjobhtm = '<div class="data-sub"><table cellpadding="0" cellspacing="0"><tbody>';
			tjobhtm += '<tr><td><a class="cat" data="'+datashtm+'" href="javascript:;"><label title="'+datashtmArray[1]+'" rel="'+datashtmArray[0]+'" data="'+datashtm+'"><font style="font-weight:bold;">不限</font></label></a></td></tr>';
			tjobhtm += '</tbody></table></div>';
			$(fillID).append(tjobhtm);
		};
		var d = $(".data-sub"), l = $(this).closest("li"), s = d.find("label");
		// 判断是否有选中
		if ($(resulthidId).val().length > 0) {
			var rgsid = $("#subclass").val();
			s.each(function(index, el) {
				var rgsdrelArray = $(this).attr("rel").split(".");
				if (rgsid == rgsdrelArray[2]) {
					$(this).addClass('gselect');
				}
			});
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
		$.browser.msie = /msie/.test(navigator.userAgent.toLowerCase());
		if($.browser.msie) {
			pbv = 1;
			bpv = 2;
		}
		l.addClass('cat-active');
		var n = $(this).next();
		pheight - dheight - iheight < comparetop ? (n.removeClass("sub").addClass("sup"), d.css({left:(pwidth-dwidth < dcleft ? (pwidth-dwidth-((pleft+pwidth)-(ileft+iwidth))) : dcleft),top:(comparetop + iheight)-dheight-iheight + pbv})) : (n.removeClass("sup").addClass("sub"), d.css({left:(pwidth-dwidth < dcleft ? (pwidth-dwidth-((pleft+pwidth)-(ileft+iwidth))) : dcleft),top:(comparetop + iheight) - bpv}));
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
		s.unbind('click').bind('click', function() {
			$(fillID + " a.cat").removeClass('cat-checked');
			ace.addClass('cat-checked');
			var sic = $(this).is(':checked'), sdata = $(this).attr("data"), sdataArray = sdata.split(",");
			var checkIDArray = sdataArray[0].split(".");
			$("#template").show();
			if (checkIDArray[2] == 0) {
				$("#JobRequInfoTemplate").html('<a data="'+checkIDArray[1]+'" href="javascript:void(0);">'+sdataArray[1]+'</a>');
			} else {
				$("#JobRequInfoTemplate").html('<a data="'+checkIDArray[2]+'" href="javascript:void(0);">'+sdataArray[1]+'</a>');
			}
			$("#JobRequInfoTemplate a").unbind().die().live('click', function() {
				var aid = $(this).attr("data");
				$.get("company_jobs.php?act=get_content_by_jobs_cat&id="+aid, function(data) {
					if (data == "-1") {
						editor.html('');
						editor.sync();
					} else {
						editor.html(data);
						editor.sync();
					}
				});
			});
			$("#jobText").html(sdataArray[1]);
			$("#category_cn").val(sdataArray[1]);
			$("#topclass").val(checkIDArray[0]);
			$("#category").val(checkIDArray[1]);
			$("#subclass").val(checkIDArray[2]);
			$('.aui_outer').hide();
			$(".ucc-default").removeClass('aui_is_show');
		});
	});
	// 分割data 返回数组
	function splitdata(arr) {
		if(arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		}
	}
}
/*地区选择弹出层填充数据*/
function city_filldata(fillID, data_resourcesP, data_resources, resultlist, showID, resultshowID, resulthidId, dir) {
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
				cityhtm += '<li><a title="'+citylist_array[1]+'" href="javascript:;" data="'+citylist_array[0]+'.0,'+citylist_array[1]+'" rel="'+citylist_array[0]+'.0" class="cat"><i class="data-icon data-icon-expend"></i>'+citylist_array[1]+'</a><a href="javascript:;" class="cat-touch"><label title="'+citylist_array[1]+'" data="'+citylist_array[0]+'.0,'+citylist_array[1]+'" rel="'+citylist_array[0]+'.0">'+citylist_array[1]+'</label></a></li>';
				subscriptnum_city ++;
			};
		};
		cityhtm += '</ul></div>';
		cityhtm += '</div>';
	};
	$(fillID).html(cityhtm);
	// 全部恢复
	if ($(resulthidId).val().length > 0) {
		var resid = $("#district").val();
		$(fillID + " a.cat").each(function(index, el) {
			var resrelidArray = $(this).attr("rel").split(".");
			if (resid == resrelidArray[0]) {
				$(this).addClass('cat-checked');
			}
		})
	}
	$(fillID + " a.cat").unbind().on("click",function() {
		var ace = $(this), cityarrayid_array = ace.attr("rel").split("."), datashtm = ace.attr("data"), datashtmArray = datashtm.split(",");
		if (data_resources[cityarrayid_array[0]]) { // 判断是否有二级地区
			var tcityhtm = '<div class="data-sub"><table cellpadding="0" cellspacing="0"><tbody>';
			var tcity_array = data_resources[cityarrayid_array[0]].split("|");
			var sourse_length = parseInt(tcity_array.length);
			var rows = 0;
			var subscriptnum = 0, tm = 0;
			if((sourse_length%2) == 0) {
				rows = sourse_length / 2;
			} else {
				rows = (sourse_length / 2) + 1;
			}
			for (var i = 0; i < rows; i++) {
				tcityhtm += '<tr>';
				for (var j = 0; j <= 2; j++) {
					if (tcity_array[subscriptnum]) {
						if (tm == 0) {
							tcityhtm += '<td><a class="cat" data="'+datashtm+'" href="javascript:;"><label title="'+datashtmArray[1]+'" rel="'+datashtmArray[0]+'" data="'+datashtm+'"><font style="font-weight:bold;">不限</font></label></a></td>';
							tm ++;
						} else {
							var tcitylist_array = tcity_array[subscriptnum].split(",");
							tcityhtm += '<td><a class="cat" data="'+cityarrayid_array[0]+'.'+tcitylist_array[0]+','+datashtmArray[1]+'/'+tcitylist_array[1]+'" href="javascript:;"><label title="'+tcitylist_array[1]+'" rel="'+cityarrayid_array[0]+'.'+tcitylist_array[0]+'" data="'+cityarrayid_array[0]+'.'+tcitylist_array[0]+','+datashtmArray[1]+'/'+tcitylist_array[1]+'">'+tcitylist_array[1]+'</label></a></td>';
							subscriptnum ++;
						}
					}
				};
				tcityhtm += '</tr>';
			};
			tcityhtm += '</tbody></table></div>';
			$(fillID).append(tcityhtm);
			
		} else{
			var tcityhtm = '<div class="data-sub"><table cellpadding="0" cellspacing="0"><tbody>';
			tcityhtm += '<tr><td><a class="cat" data="'+datashtm+'" href="javascript:;"><label title="'+datashtmArray[1]+'" rel="'+datashtmArray[0]+'" data="'+datashtm+'"><font style="font-weight:bold;">不限</font></label></a></td></tr>';
			tcityhtm += '</tbody></table></div>';
			$(fillID).append(tcityhtm);
		};
		var d = $(".data-sub"), l = $(this).closest("li"), s = d.find("label");
		// 判断是否有选中
		if ($(resulthidId).val().length > 0) {
			var rgsid = $("#sdistrict").val();
			s.each(function(index, el) {
				var rgsdrelArray = $(this).attr("rel").split(".");
				if (rgsid == rgsdrelArray[1]) {
					$(this).addClass('gselect');
				}
			});
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
		$.browser.msie = /msie/.test(navigator.userAgent.toLowerCase());
		if($.browser.msie) {
			pbv = 1;
			bpv = 2;
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
		s.unbind('click').bind('click', function() {
			$(fillID + " a.cat").removeClass('cat-checked');
			ace.addClass('cat-checked');
			var sic = $(this).is(':checked'), sdata = $(this).attr("data"), sdataArray = sdata.split(",");
			var checkIDArray = sdataArray[0].split(".");
			$("#cityText").html(sdataArray[1]);
			$("#district_cn").val(sdataArray[1]);
			$("#district").val(checkIDArray[0]);
			$("#sdistrict").val(checkIDArray[1]);
			$('.aui_outer').hide();
			$(".ucc-default").removeClass('aui_is_show');
		});
	});
	// 分割data 返回数组
	function splitdata(arr) {
		if (arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		};
	}
}
/*行业选择弹出层填充数据*/
function trade_filldata(fillID, data_resources, showID, resultlist, resultshowID, resultID, dir) {
	var tradhtm = '';
	var sourse_length = parseInt(data_resources.length);
	var rows = 0;
	var subscriptnum = 0;
	if((sourse_length%4) == 0) {
		rows = sourse_length / 4;
	} else {
		rows = (sourse_length / 4) + 1;
	}
	for (var i = 0; i < rows; i++) {
		tradhtm += '<tr>';
		for (var j = 0; j < 4; j++) {
			if (data_resources[subscriptnum]) {
				var trad_array = data_resources[subscriptnum].split(",");
				tradhtm += '<td><label class="selectra" data="'+trad_array[0]+','+trad_array[1]+'">'+trad_array[1]+'</label></td>';
				subscriptnum ++;
			};
		};
		tradhtm += '</tr>';
	};
	$(fillID).html(tradhtm);
	// 恢复选中
	if ($(resultID).val().length > 0) {
		var rgsid = $(resultID).val();
		$(fillID+" label").each(function(index, el) {
			var rgsdrelArray = $(this).attr("data").split(",");
			if (rgsid == rgsdrelArray[0]) {
				$(this).addClass('gselectra');
			}
		});
	}
	// 单选
	$(fillID+" label").unbind().live("click",function(event) {
		$(fillID+" label").removeClass('gselectra');
		$(this).addClass('gselectra');
		var checkMsgArray = $(this).attr("data").split(",");
		$("#tradText").html(checkMsgArray[1]);
		$("#trade_cn").val(checkMsgArray[1]);
		$("#trade").val(checkMsgArray[0]);
		$('.aui_outer').hide();
		$("#maskLayerLoad").remove();
		$(".ucc-default").removeClass('aui_is_show');
	});
	// 分割data 返回数组
	function splitdata(arr) {
		if(arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		}
	}
}
/*所在街道选择弹出层填充数据*/
function street_filldata(fillID, resultID) {
	// 恢复选中
	if ($(resultID).val().length > 0) {
		var rgsid = $(resultID).val();
		$(fillID+" label").each(function(index, el) {
			var rgsdrelArray = $(this).attr("data").split(",");
			if (rgsid == rgsdrelArray[0]) {
				$(this).addClass('gselectra');
			}
		});
	}
	// 单选
	$(fillID+" label").unbind().live("click",function(event) {
		$(fillID+" label").removeClass('gselectra');
		$(this).addClass('gselectra');
		var checkMsgArray = $(this).attr("data").split(",");
		$("#streetText").html(checkMsgArray[1]);
		$("#street_cn").val(checkMsgArray[1]);
		$("#street").val(checkMsgArray[0]);
		$('.aui_outer').hide();
		$("#maskLayerLoad").remove();
		$(".ucc-default").removeClass('aui_is_show');
	});
	// 分割data 返回数组
	function splitdata(arr) {
		if(arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		}
	}
}
/*职位亮点选择弹出层填充数据*/
function tag_filldata(fillID, data_resources, showID, resultlist, resultshowID, resultID, dir) {
	var tradhtm = '';
	var sourse_length = parseInt(data_resources.length);
	var rows = 0;
	var subscriptnum = 0;
	if((sourse_length%4) == 0) {
		rows = sourse_length / 4;
	} else {
		rows = (sourse_length / 4) + 1;
	}
	for (var i = 0; i < rows; i++) {
		tradhtm += '<tr>';
		for (var j = 0; j < 4; j++) {
			if (data_resources[subscriptnum]) {
				var trad_array = data_resources[subscriptnum].split(",");
				tradhtm += '<td><label><input type="checkbox" data="'+trad_array[0]+','+trad_array[1]+'" name="item-list" class="input-checkbox">'+trad_array[1]+'</label></td>';
				subscriptnum ++;
			};
		};
		tradhtm += '</tr>';
	};
	$(fillID).html(tradhtm);
	// 恢复选中
	if ($(resultID).val().length > 0) {
		var idarray = $(resultID).val().split(",");
		$.each(idarray, function(index, val) {
			$(fillID+" :checkbox").unbind().each(function() {
				var boxdata_array = $(this).attr("data").split(",");
				if(val == boxdata_array[0]) {
					$(this).attr('checked',true);
				}
			});
		});
		copyitem();
		copy_selected();
		isexceed($(fillID+" :checkbox[checked]").length);
		// 单选
		$(fillID+" label :checkbox").unbind().live("click",function(event) {
			if ($(this).find(":checkbox").attr('checked')) {
				$(this).find(":checkbox").attr('checked',false);
				isexceed($(fillID+" :checkbox[checked]").length);
			} else {
				$(this).find(":checkbox").attr('checked',true);
				isexceed($(fillID+" :checkbox[checked]").length);
			}
		});
	};
	// 单选
	$(fillID+" label :checkbox").unbind().live("click",function(event) {
		if ($(this).find(":checkbox").attr('checked')) {
			$(this).find(":checkbox").attr('checked',false);
			isexceed($(fillID+" :checkbox[checked]").length);
		} else {
			$(this).find(":checkbox").attr('checked',true);
			isexceed($(fillID+" :checkbox[checked]").length);
		}
	});
	// 复制选中项目
	function copyitem() {
		var resulthtm = '';
		$(fillID+" :checkbox[checked]").unbind().each(function(index) {
			var result_data_array = $(this).attr("data").split(",");
			resulthtm += '<span data-code="'+result_data_array[0]+','+result_data_array[1]+'">'+result_data_array[1]+'<i class="data-icon data-icon-close"></i></span>';
		});
		$(resultlist).html(resulthtm);
		var spi = $(resultlist + " span");
		spi.unbind().bind('click', function() {
			var spidata = $(this).attr("data-code"),
				apid_array = splitdata(spidata);
			$(fillID+" :checkbox[checked]").unbind().each(function(index) {
				var result_data_array = $(this).attr("data").split(",");
				if (apid_array[0] == result_data_array[0]) {
					$(this).attr('checked',false);
					isexceed($(fillID+" :checkbox[checked]").length);
				}
			});
			copyitem();
		});
		$("#arstrade").html($(fillID+" :checkbox[checked]").length);
		if ($(fillID+" :checkbox[checked]").length > 0) {
			$(".cla").show();
			$(".cla").click(function() {
				$(fillID+" :checkbox").unbind().each(function(index) {
					$(this).attr('disabled',false);
					$(this).attr('checked',false);
				});
				copyitem();
			})
		} else {
			$(".cla").hide();
		}
	}
	// 复制选中 赋值
	function copy_selected() {
		var tagotphtm = '';
		var selectedid_array = new Array();
		var selectedcn_array = new Array();
		$(fillID+" :checkbox[checked]").unbind().each(function(index) {
			var data_array = $(this).attr("data").split(",");
			selectedid_array[index]=data_array[0];
			selectedcn_array[index]=data_array[1];
			tagotphtm += '<div class="input_checkbox"><span rel="'+data_array[0]+'">'+data_array[1]+'</span></div>';
		});
		$(".showchecktag").html(tagotphtm);
		$(resultID).val(selectedid_array.join(","));
		$("#tag_cn").val(selectedcn_array.join(","));
	}
	// 分割data 返回数组
	function splitdata(arr) {
		if(arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		}
	}
	/*判断选中的数量是否超出最大限制*/
	function isexceed(num) {
		if(num >= 5) {
			$(fillID+" :checkbox").each(function(index, el) {
				if (!$(this).attr('checked')) {
					$(this).attr('disabled',"disabled");
				}
			});
		} else {
			$(fillID+" :checkbox").each(function(index, el) {
				$(this).attr('disabled',false);
			});
		}
		copyitem();
	}
	// 点击确定
	$("#tag-selector-save").click(function() {
		copy_selected();
		$('.aui_outer').hide();
		$("#maskLayerLoad").remove();
		$(".ucc-default").removeClass('aui_is_show');
	});
}
// 一些js的集合
function allaround(dir) {
	// 显示下拉
	$(".ucc-default").click(function() {
		if (!$(this).hasClass('aui_is_show')) {
			$('.aui_outer').hide();
			$(".ucc-default").removeClass('aui_is_show');
			$(this).addClass('aui_is_show');
			$(this).parent().find('.aui_outer').show();
		} else {
			$(this).removeClass('aui_is_show');
			$(this).parent().find('.aui_outer').hide();
		}
	});
	$(document).delegate("body", "click", function(e){
		var _con = $(".ucc-default"), _caui = $(".aui_outer");
		if(!_con.is(e.target) && _caui.has(e.target).length === 0){
			$('.aui_outer').hide();
			$(".ucc-default").removeClass('aui_is_show');
		}
	});
	// 关闭下拉
	$(".selector-close").die().live('click', function(event) {
		$('.aui_outer').hide();
		$(".ucc-default").removeClass('aui_is_show');
	});
}