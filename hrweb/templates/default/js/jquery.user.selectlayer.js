function allaround(dir){
	if($("#divCityCate").length > 0) {
		fillCity("#divCityCate"); // 地区填充
		// 恢复地区选中条件
		if($("#residence").val()) {
			var scityid = $("#residence").val().split(".");
			if(scityid[1] == 0) {
				var dcityid = scityid[0];
				$("#divCityCate li p a").each(function() {
					if(dcityid == $(this).attr("rcoid")) {
						$(this).addClass('selectedcolor');
					}
				});
			} else {
				$("#divCityCate .citycatebox .subcate a").each(function() {
					if(scityid[1] == $(this).attr("rcoid")) {
						$(this).parent().prev().find('font a').addClass('selectedcolor');
						$(this).addClass('selectedcolor');
					}
				});
			}
		}
		/* 地区点击显示到已选 */
		$("#divCityCate li p a").unbind().live('click', function(){
			$("#divCityCate li p a").each(function() {
				$(this).removeClass('selectedcolor');
			});
			$(this).addClass('selectedcolor');
			var checkID = $(this).attr('pid');
			var checkText = $(this).attr('title');
			$("#cityText").html(checkText);
			$("#residence_cn").val(checkText);
			$("#residence").val(checkID);
			$("#divCityCate").hide();
		});
		$("#divCityCate .subcate a").unbind().live('click', function() {		
			$("#divCityCate .subcate a").each(function() {
				$(this).parent().prev().find('font a').removeClass('selectedcolor');
				$(this).removeClass('selectedcolor');
			});
			$(this).parent().prev().find('font a').addClass('selectedcolor');
			$(this).addClass('selectedcolor');
			var checkID = $(this).attr('pid');
			var checkText = $(this).attr('title');
			$("#cityText").html(checkText);
			$("#residence_cn").val(checkText);
			$("#residence").val(checkID);
			$("#divCityCate").hide();
		});
	}
	if($("#divCityCateH").length > 0) {
		fillCity("#divCityCateH"); // 籍贯填充
		// 恢复籍贯选中条件
		if($("#householdaddress").val()) {
			var scityid = $("#householdaddress").val().split(".");
			if(scityid[1] == 0) {
				var dcityid = scityid[0];
				$("#divCityCateH li p a").each(function() {
					if(dcityid == $(this).attr("rcoid")) {
						$(this).addClass('selectedcolor');
					}
				});
			} else {
				$("#divCityCateH .citycatebox .subcate a").each(function() {
					if(scityid[1] == $(this).attr("rcoid")) {
						$(this).parent().prev().find('font a').addClass('selectedcolor');
						$(this).addClass('selectedcolor');
					}
				});
			}
		}
		/* 籍贯点击显示到已选 */
		$("#divCityCateH li p a").unbind().live('click', function(){
			$("#divCityCateH li p a").each(function() {
				$(this).removeClass('selectedcolor');
			});
			$(this).addClass('selectedcolor');
			var checkID = $(this).attr('pid');
			var checkText = $(this).attr('title');
			$("#cityTextH").html(checkText);
			$("#householdaddress_cn").val(checkText);
			$("#householdaddress").val(checkID);
			$("#divCityCateH").hide();
		});
		$("#divCityCateH .subcate a").unbind().live('click', function() {		
			$("#divCityCateH .subcate a").each(function() {
				$(this).parent().prev().find('font a').removeClass('selectedcolor');
				$(this).removeClass('selectedcolor');
			});
			$(this).parent().prev().find('font a').addClass('selectedcolor');
			$(this).addClass('selectedcolor');
			var checkID = $(this).attr('pid');
			var checkText = $(this).attr('title');
			$("#cityTextH").html(checkText);
			$("#householdaddress_cn").val(checkText);
			$("#householdaddress").val(checkID);
			$("#divCityCateH").hide();
		});
	}
	if($("#divCityCateW").length > 0) {
		fillCity("#divCityCateW"); // 简历工作地区填充
		// 恢复简历工作地区选中条件
		if($("#sdistrict").val()) {
			var scityid = $("#sdistrict").val();
			if(scityid == 0) {
				var dcityid = $("#district").val();
				$("#divCityCateW li p a").each(function() {
					if(dcityid == $(this).attr("rcoid")) {
						$(this).addClass('selectedcolor');
					}
				});
			} else {
				$("#divCityCateW .citycatebox .subcate a").each(function() {
					if(scityid[1] == $(this).attr("rcoid")) {
						$(this).parent().prev().find('font a').addClass('selectedcolor');
						$(this).addClass('selectedcolor');
					}
				});
			}
		}
		/* 简历工作地区点击显示到已选 */
		$("#divCityCateW li p a").unbind().live('click', function(){
			$("#divCityCateH li p a").each(function() {
				$(this).removeClass('selectedcolor');
			});
			$(this).addClass('selectedcolor');
			var checkID = $(this).attr('pid').split(".");
			var checkText = $(this).attr('title');
			$("#cityTextW").html(checkText);
			$("#district_cn").val(checkText);
			$("#district").val(checkID[0]);
			$("#sdistrict").val(checkID[1]);
			$("#divCityCateW").hide();
		});
		$("#divCityCateW .subcate a").unbind().live('click', function() {		
			$("#divCityCateW .subcate a").each(function() {
				$(this).parent().prev().find('font a').removeClass('selectedcolor');
				$(this).removeClass('selectedcolor');
			});
			$(this).parent().prev().find('font a').addClass('selectedcolor');
			$(this).addClass('selectedcolor');
			var checkID = $(this).attr('pid').split(".");
			var checkText = $(this).attr('title');
			$("#cityTextW").html(checkText);
			$("#district_cn").val(checkText);
			$("#district").val(checkID[0]);
			$("#sdistrict").val(checkID[1]);
			$("#divCityCateW").hide();
		});
	}
	if($("#divTradCate").length > 0) {
		fillTrad("#divTradCate"); // 行业填充内容
		// 恢复行业选中条件
		if($("#trade").val()) {
			var recoverTradArray = $("#trade").val().split(",");
			$.each(recoverTradArray, function(index, val) {
				 $("#tradList a").each(function() {
					if(val == $(this).attr('cln')) {
						$(this).addClass('selectedcolor');
					}
				});
			});
			copyTradItem();
			var a_cn = new Array();
			$("#tradAcq a").each(function(index) {
				var checkText = $(this).attr('title');
				a_cn[index]=checkText;
			});
			$("#tradText").html(a_cn.join(","));
		}
		/* 行业列表点击显示到已选 */
		$("#tradList li a").unbind().live('click', function() {
			// 判断选择的数量是否超出
			if($("#tradList .selectedcolor").length >= 3) {
				$("#tradropcontent").show(0).delay(3000).fadeOut("slow");
			} else {
				$(this).addClass('selectedcolor');
				copyTradItem(); // 将行业已选的拷贝
			}
		});
		// 行业确定选择
		$("#tradSure").unbind().click(function() {
			var a_cn=new Array();
			var a_id=new Array();
			$("#tradAcq a").each(function(index) {
				var checkID = $(this).attr('rel');
				var checkText = $(this).attr('title');
				a_id[index]=checkID;
				a_cn[index]=checkText;
			});
			if (a_cn.length > 0) {
				$("#tradText").html(a_cn.join(","));
				$("#trade_cn").val(a_cn.join(","));
				$("#trade").val(a_id.join(","));
			} else {
				$("#tradText").html("请选择行业类别");
				$("#trade_cn").val("");
				$("#trade").val("");
			}
			$("#divTradCate").hide();
		});
	}
	if($("#divJobCate").length > 0) {
		fillJobs("#divJobCate"); // 职位填充内容
		// 恢复职位选中条件
		if($("#intention_jobs_id").val()) {
			var recoverJobArray = $("#intention_jobs_id").val().split(",");
			$.each(recoverJobArray, function(index, val) {
				 var demojobArray = val.split(".");
				 if(demojobArray[2] == "0") { // 如果第三个参数是 0 则只选到第二级
				 	$(".jobcatebox p a").each(function() {
				 		if(demojobArray[1] == $(this).attr("rcoid")) {
				 			$(this).addClass('selectedcolor');
				 		}
				 	});
				 } else {
				 	$(".jobcatebox .subcate a").each(function() {
				 		if(demojobArray[2] == $(this).attr("rcoid")) {
				 			$(this).addClass('selectedcolor');
				 		}
				 	});
				 }
			});
			copyJobItem();
			var a_cn=new Array();
			$("#jobAcq a").each(function(index) {
				var checkText = $(this).attr('title');
				a_cn[index]=checkText;
			});
			$("#jobText").html(a_cn.join(","));
		}
		/* 职位列表点击显示到已选 */
		$("#divJobCate li p a").unbind().live('click', function() {
			// 判断选择的数量是否超出
			if($("#divJobCate .selectedcolor").length >= 3) {
				$("#jobdropcontent").show(0).delay(3000).fadeOut("slow");
			} else {
				$(this).addClass('selectedcolor');
				copyJobItem(); // 将职位已选的拷贝
			}
		});
		$("#divJobCate .subcate a").unbind().live('click', function() {
			// 判断选择的数量是否超出
			if($("#divJobCate .selectedcolor").length >= 3) {
				$("#jobdropcontent").show(0).delay(3000).fadeOut("slow");
			} else {
				if($(this).attr("p") == "qb") {
					$(this).parent().prev().find('font a').addClass('selectedcolor');
					$(this).parent().find('a').removeClass('selectedcolor');
				} else {
					$(this).parent().prev().find('font a').removeClass('selectedcolor');
					$(this).addClass('selectedcolor');
				}
				copyJobItem(); // 将职位已选的拷贝
			}
		});
		// 职位确定选择
		$("#jobSure").unbind().click(function() {
			var a_cn=new Array();
			var a_id=new Array();
			$("#jobAcq a").each(function(index) {
				// 如果选择的是二级分类将第三个参数补 0
				var chid = new Array();
				if($(this).attr('pid')) {
					chid = $(this).attr('pid').split(".");
					if(chid.length < 3) {
						chid.push(0);
					}
				}
				var checkID = chid.join(".");
				var checkText = $(this).attr('title');
				a_id[index]=checkID;
				a_cn[index]=checkText;
			});
			if (a_cn.length > 0) {
				$("#jobText").html(a_cn.join(","));
				$("#intention_jobs").val(a_cn.join(","));
				$("#intention_jobs_id").val(a_id.join(","));
			} else {
				$("#jobText").html("请选择职位类别");
				$("#intention_jobs").val("");
				$("#intention_jobs_id").val("");
			}
			$("#divJobCate").hide();
		});
	}
}
function fillJobs(fillID){
	var jobstr = '';
	$.each(QS_jobs_parent, function(pindex, pval) {
		if(pval) {
			jobstr += '<tr>';
			var jobs = pval.split(",");
		 	jobstr += '<th>'+jobs[1]+'</th>';
		 	jobstr += '<td><ul class="jobcatelist">';
		 	var sjobsArray = QS_jobs[jobs[0]].split("|");
		 	$.each(sjobsArray, function(sindex, sval) {
		 		if(sval) {
		 			var sjobs = sval.split(",");
			 		jobstr += '<li>';
			 		jobstr += '<p class="jobp"><font><a class="joba" rcoid="'+sjobs[0]+'" pid="'+jobs[0]+'.'+sjobs[0]+'.0" title="'+sjobs[1]+'" href="javascript:;">'+sjobs[1]+'</a></font></p>';
			 		if(QS_jobs[sjobs[0]]) {
			 			jobstr += '<div class="subcate jobsub" style="display:none;">';
			 			var cjobsArray = QS_jobs[sjobs[0]].split("|");
			 			jobstr += '<a p="qb" href="javascript:;">不限</a>';
				 		$.each(cjobsArray, function(cindex, cval) {
				 			if(cval) {
					 			var cjobs = cval.split(",");
					 			jobstr += '<a rcoid="'+cjobs[0]+'" title="'+cjobs[1]+'" pid="'+jobs[0]+'.'+sjobs[0]+'.'+cjobs[0]+'" href="javascript:;">'+cjobs[1]+'</a>';
				 			}
				 		});
			 			jobstr += '</div>';
			 		}
			 		jobstr += '</li>';
		 		}
		 	});
		 	jobstr += '</ul></td>';
			jobstr += '</tr>';
		}
	});
	$(fillID+" tbody").html(jobstr);
	$(".jobcatelist li").each(function() {
		if($(this).find('.subcate').length <= 0) {
			$(this).find('font').css("background","none");
		}
	});
}
function copyJobItem() {
	var jobacqhtm = '';
	$("#divJobCate .selectedcolor").each(function() {
		jobacqhtm += '<a pid="'+$(this).attr('pid')+'" href="javascript:;" title="'+$(this).attr('title')+'"><div class="text">'+$(this).attr('title')+'</div><div class="close"></div></a>';
	});
	$("#jobAcq").html(jobacqhtm);
	// 已选项目绑定点击事件
	$("#jobAcq a").unbind().click(function() {
		var selval = $(this).attr('title');
		$("#divJobCate .selectedcolor").each(function() {
			if ($(this).attr('title') == selval) {
				$(this).removeClass('selectedcolor');
				copyJobItem();
			}
		});
	});
	// 清空
	$("#jobEmpty").unbind().click(function() {
		$("#jobAcq").html("");
		$("#divJobCate .selectedcolor").each(function() {
			$(this).removeClass('selectedcolor');
		});
	});
}
function copyTradItem() {
	var tradacqhtm = '';
	$("#tradList .selectedcolor").each(function() {
		tradacqhtm += '<a href="javascript:;" rel="'+$(this).attr('cln')+'" title="'+$(this).attr('title')+'"><div class="text">'+$(this).attr('title')+'</div><div class="close" id="c-'+$(this).attr('cln')+'"></div></a>';
	});
	$("#tradAcq").html(tradacqhtm);
	// 已选项目绑定点击事件
	$("#tradAcq a").unbind().click(function() {
		var selval = $(this).attr('title');
		$("#tradList .selectedcolor").each(function() {
			if ($(this).attr('title') == selval) {
				$(this).removeClass('selectedcolor');
				copyTradItem();
			}
		});
	});
	// 清空
	$("#tradEmpty").unbind().click(function() {
		$("#tradAcq").html("");
		$("#tradList .selectedcolor").each(function() {
			$(this).removeClass('selectedcolor');
		});
	});
}
function fillTrad(fillID){
	var tradli = '';
	$.each(QS_trade, function(index, val) {
		if(val) {
			var trads = val.split(",");
		 	tradli += '<li><a title="'+trads[1]+'" cln="'+trads[0]+'" href="javascript:;">'+trads[1]+'</a></li>';
		}
	});
	$(fillID+" ul").html(tradli);
}
// 地区填充
function fillCity(fillID){
	var citystr = '';
	citystr += '<tr>';
	citystr += '<td><ul class="jobcatelist">';
	$.each(QS_city_parent, function(pindex, pval) {
		if(pval) {
			var citys = pval.split(",");
	 		citystr += '<li>';
	 		citystr += '<p><font><a rcoid="'+citys[0]+'" pid="'+citys[0]+'.0" title="'+citys[1]+'" href="javascript:;">'+citys[1]+'</a></font></p>';
	 		if(QS_city[citys[0]]) {
	 			citystr += '<div class="subcate" style="display:none;">';
	 			var ccitysArray = QS_city[citys[0]].split("|");
		 		$.each(ccitysArray, function(cindex, cval) {
		 			if(cval) {
			 			var ccitys = cval.split(",");
			 			citystr += '<a rcoid="'+ccitys[0]+'" title="'+citys[1]+'/'+ccitys[1]+'" pid="'+citys[0]+'.'+ccitys[0]+'" href="javascript:;">'+ccitys[1]+'</a>';
		 			}
		 		});
	 			citystr += '</div>';
	 		}
	 		citystr += '</li>';
		}
	});
	citystr += '</ul></td>';
	citystr += '</tr>';
	$(fillID+" tbody").html(citystr);
	$(".jobcatelist li").each(function() {
		if($(this).find('.subcate').length <= 0) {
			$(this).find('font').css("background","none");
		}
	});
}
// 地区弹出框
function showCityBox(clickObjID,showID,cityPro,citySun,checkBox,hidID,hidVal,QSarrParent,QSarr,isDestruct) {
	$(clickObjID).click(function(){
		$(this).blur();
		$(this).before('<div class="menu_bg_layer" style="position:absolute;left:0px;top:0px;z-index:9;background-color:#000000;"></div>');
		$(".menu_bg_layer").css({"width":$(document).width(),"height":$(document).height(),"opacity":0.3});
		$(cityPro+" ul").html(getProvinceCity(QSarrParent));
		// 恢复选中项
		recoverChecked(citySun,checkBox,cityPro,QSarr);
		// 二级城市
		$(cityPro+" li").click(function(){
			// 判断顶级地区下有没有子地区
			var pRel = $(this).find('.cls_value').attr('rel');
			var pName = $(this).find('.cls_value').html();
			if (QSarr[pRel]) {
				$(this).addClass('current').siblings().removeClass('current');
				$(citySun).html(getSunCity(QSarr,pRel,pName));
				makeGrandCity(citySun,QSarr);
				// 三级城市
				showGrandCity(citySun,QSarr,checkBox,clickObjID,showID,hidID,hidVal,isDestruct);
			} else {
				var id = $(this).find('.cls_value').attr('rel');
				var val = $(this).find('.cls_value').html();
				var pid = $(this).find('.cls_value').attr('pid');
				var ptitle = $(this).find('.cls_value').attr('ptitle');
				$(checkBox).html(getCheckInfo(id,val,'',''));
				$(clickObjID).html(val);
				$(hidID).val(id);
				$(hidVal).val(val);
				if(isDestruct) {
					getDistrictId();
				}
				closeDialog(showID);
			}
		});
		// 三级城市
		showGrandCity(citySun,QSarr,checkBox,clickObjID,showID,hidID,hidVal,isDestruct);
		$(showID).show();
		$(".menu_bg_layer").click(function() {
			closeDialog(showID);
		});
		$(".cm_closeMsg").click(function() {
			closeDialog(showID);
		});
	});
}
// 猎头职能
function showHunterJobBox(clickObjID,showID,cityPro,citySun,checkBox,hidID,hidVal,QSarrParent,QSarr,isDestruct) {
	$(clickObjID).click(function(){
		$(this).blur();
		$(this).before('<div class="menu_bg_layer" style="position:absolute;left:0px;top:0px;z-index:9;background-color:#000000;"></div>');
		$(".menu_bg_layer").css({"width":$(document).width(),"height":$(document).height(),"opacity":0.3});
		$(cityPro+" ul").html(getProvinceCity(QSarrParent));
		// 恢复选中项
		recoverCheckedJob(citySun,checkBox,cityPro,QSarr);
		// 二级城市
		$(cityPro+" li").click(function(){
			var pRel = $(this).find('.cls_value').attr('rel');
			var pName = $(this).find('.cls_value').html();
			$(this).addClass('current').siblings().removeClass('current');
			$(citySun).html(getSunCity(QSarr,pRel,pName));
			$liCity = $(citySun+" li.parent_node");
			$liCity.click(function() {
				var id = $(this).find('.cls_value').attr('rel');
				var val = $(this).find('.cls_value').html();
				var pid = $(this).find('.cls_value').attr('pid');
				var ptitle = $(this).find('.cls_value').attr('ptitle');
				var index = $liCity.index(this);
				$liCity.each(function() {
					$(this).removeClass('current');
				});
				$liCity.eq(index).addClass('current');
				$(checkBox).append(getCheckInfo(id,val,pid,ptitle));
				$(hidID).val(pid);
				$(hidVal).val(ptitle);
				if(isDestruct) {
					getDistrictId();
				}
			});
		});
		$("#btn_tradsavejob").click(function(){
			$a_checkbox = $(checkBox+" a");
			var checkhtm = '';
			var a_cn=new Array();
			var a_id=new Array();
			$a_checkbox.each(function(index) {
				var checkVal = $(this).find('span').html();
				var lel = $(this).find('span').attr('rel');
				checkhtm+='<div class="input_checkbox"><span rel="'+lel+'">'+checkVal+'</span></div>';
				a_cn[index]=$(this).attr("gname");
				a_id[index]=$(this).attr("gid");
			});
			$("#jobs_checkbox .showchecked").html(checkhtm);
			$("#jobs_checkbox .showchecked span").click(function(){
				$(this).parent().remove();
				var slel = $(this).attr('rel');
				$a_checkbox.each(function(index) {
					var alel = $(this).find('span').attr('rel');
					var agid = $(this).attr('gid');
					if (alel == slel) {
						$(this).remove();
						var joid = $("#goodcategory").val().split("-");
						joid.splice($.inArray(agid,joid),1);
						$("#goodcategory").val(joid.join("-"));
						return false;
					}
				});
				$(showJobsTypeArea+" :checkbox[checked]").each(function() {
					if($(this).val() == slel){
						$(this).attr('checked',false);
					}
				});
			});
			$("#goodcategory_cn").val(a_cn.join(","));
			$("#goodcategory").val(a_id.join("-"));
			closeDialog(showID);
		});
		$(showID).show();
		$(".menu_bg_layer").click(function() {
			closeDialog(showID);
		});
		$(".cm_closeMsg").click(function() {
			closeDialog(showID);
		});
	});
}
function showHunterJobBoxD(clickObjID,showID,cityPro,citySun,checkBox,hidID,hidVal,QSarrParent,QSarr,isDestruct) {
	$(clickObjID).click(function(){
		$(this).blur();
		$(this).before('<div class="menu_bg_layer" style="position:absolute;left:0px;top:0px;z-index:9;background-color:#000000;"></div>');
		$(".menu_bg_layer").css({"width":$(document).width(),"height":$(document).height(),"opacity":0.3});
		$(cityPro+" ul").html(getProvinceCity(QSarrParent));
		// 恢复选中项
		recoverCheckedJob(citySun,checkBox,cityPro,QSarr);
		// 二级城市
		$(cityPro+" li").click(function(){
			var pRel = $(this).find('.cls_value').attr('rel');
			var pName = $(this).find('.cls_value').html();
			$(this).addClass('current').siblings().removeClass('current');
			$(citySun).html(getSunCity(QSarr,pRel,pName));
			$liCity = $(citySun+" li.parent_node");
			$liCity.click(function() {
				var id = $(this).find('.cls_value').attr('rel');
				var val = $(this).find('.cls_value').html();
				var pid = $(this).find('.cls_value').attr('pid');
				var ptitle = $(this).find('.cls_value').attr('ptitle');
				var index = $liCity.index(this);
				$liCity.each(function() {
					$(this).removeClass('current');
				});
				$liCity.eq(index).addClass('current');
				$(clickObjID).html(ptitle);
				$(checkBox).html(getCheckInfo(id,val,pid,ptitle));
				$(hidID).val(pid);
				$(hidVal).val(ptitle);
				if(isDestruct) {
					getDistrictId();
				}
			});
		});
		
		$(showID).show();
		$(".menu_bg_layer").click(function() {
			closeDialog(showID);
		});
		$(".cm_closeMsg").click(function() {
			closeDialog(showID);
		});
	});
}
// 恢复选中
function recoverCheckedJob(citySun,checkBox,cityPro,QSarr) {
	if($(checkBox+" a").length > 0) {
		$(checkBox+" a").each(function() {
			var pid = $(this).attr('gid').split(".");
			var pname = $(this).attr('gname').split("/");
			$(cityPro+" ul li").eq(pid[0]-1).addClass('current');
			$(citySun).html(getSunCity(QSarr,pid[0],pname[0]));
			var checkRel = $(this).find('span').attr("rel");
			$(citySun+" li.parent_node").each(function() {
				var sunRel = $(this).find('.cls_value').attr('rel');
				if(sunRel == checkRel) {
					$(this).addClass('current');
					return false;
				}
			});
			makeGrandCity(citySun,QSarr);
			$(citySun+" :input").each(function() {
				var grdVal = $(this).val();
				var grdRel = $(this).attr('rel');
				if(grdVal == checkRel) {
					$(this).attr("checked","checked");
					$(citySun+" li.parent_node").each(function() {
						var sunRel = $(this).find('.cls_value').attr('rel');
						if(sunRel == grdRel) {
							$(this).addClass('current');
						}
					});
					return false;
				}
			});
		});
	} else {
		$(cityPro+" ul li").eq(0).addClass('current');
		$(citySun).html(getSunCity(QSarr,"30","销售"));
	}
}
// 恢复选中
function recoverChecked(citySun,checkBox,cityPro,QSarr) {
	if($(checkBox+" a").length > 0) {
		$(checkBox+" a").each(function() {
			var pid = $(this).attr('gid').split(".");
			var pname = $(this).attr('gname').split("/");
			$(cityPro+" ul li").eq(pid[0]-1).addClass('current');
			$(citySun).html(getSunCity(QSarr,pid[0],pname[0]));
			var checkRel = $(this).find('span').attr("rel");
			$(citySun+" li.parent_node").each(function() {
				var sunRel = $(this).find('.cls_value').attr('rel');
				if(sunRel == checkRel) {
					$(this).addClass('current');
					return false;
				}
			});
			makeGrandCity(citySun,QSarr);
			$(citySun+" :input").each(function() {
				var grdVal = $(this).val();
				var grdRel = $(this).attr('rel');
				if(grdVal == checkRel) {
					$(this).attr("checked","checked");
					$(citySun+" li.parent_node").each(function() {
						var sunRel = $(this).find('.cls_value').attr('rel');
						if(sunRel == grdRel) {
							$(this).addClass('current');
						}
					});
					return false;
				}
			});
		});
	} else {
		$(cityPro+" ul li").eq(0).addClass('current');
		$(citySun).html(getSunCity(QSarr,"1","北京市"));
		makeGrandCity(citySun,QSarr);
	}
}
// 获取二级城市
function getSunCity(sunStr,id,pName){
	var sunCity = sunStr[id].split("|");
	var htmlstr='<ul style="width: 760px;" class="cf">';
	$.each(sunCity, function(index, val) {
		 var v = val.split(",");
		 var ptitle = pName+"/"+v[1];
		 var pid = id+"."+v[0];
		 if((index + 1)%5 ==0) {
		 	htmlstr+="<li id=\"li_city_"+v[0]+"\" class=\"parent_node\"><a id=\"p_child_value_"+v[0]+"\" rel=\""+v[0]+"\" href=\"javascript:;\" pid=\""+pid+"\" ptitle=\""+ptitle+"\" class=\"cls_value\">"+v[1]+"</a><i onclick=\"javascript:;\"></i></li></ul><ul style=\"width: 760px;\" class=\"cf\">";
		 } else {
		 	htmlstr+="<li id=\"li_city_"+v[0]+"\" class=\"parent_node\"><a id=\"p_child_value_"+v[0]+"\" rel=\""+v[0]+"\" href=\"javascript:;\" pid=\""+pid+"\" ptitle=\""+ptitle+"\" class=\"cls_value\">"+v[1]+"</a><i onclick=\"javascript:;\"></i></li>";
		 }
	});
	return htmlstr;
}
// 二级城市下插入三级城市
function makeGrandCity(ulStr,grandStr) {
	var ulCity = $(ulStr+" ul");
	$.each(ulCity, function() {
		 var liCity = $(this).find("li");
		 var lihtml = '';
		 $.each(liCity, function() {
		 	var Srel = $(this).find('.cls_value').attr('rel');
		 	var Stitle = $(this).find('.cls_value').attr('ptitle');
		 	var Spid = $(this).find('.cls_value').attr('pid');
		 	var val = getGrandCity(grandStr,Srel,Stitle,Spid);
		 	if (val != '') {
		 		lihtml+=val;
		 	}
		 });
		 $(this).after(lihtml);
	});
}
// 获取三级城市
function getGrandCity(grandStr,id,Stitle,Spid) {
	if(grandStr[id] != null) {
		var grandCity = grandStr[id].split("|");
		var htmlstr='<div id="'+id+'" style="display:none;" class="sx-sub sublist_node"><ul style="width: 760px;" class="cf">';
		$.each(grandCity, function(index, val) {
			 var v = val.split(",");
			 var sid = Spid+"."+v[0];
			 var sname = Stitle+"/"+v[1];
			 htmlstr+="<li><label><input onclick=\"removeClick(event);\" sid=\""+sid+"\" sname=\""+sname+"\" type=\"radio\" id=\"child_value_"+v[0]+"\" title=\""+v[1]+"\" rel=\""+id+"\" value=\""+v[0]+"\" class=\"cls_child\">"+v[1]+"</label></li>";
		});
		htmlstr+="</ul></div>";
		return htmlstr;
	} else {
		return '';
	}
}
// 三级城市
function showGrandCity(sunStr,cityStr,checkbox,clickObjID,showID,hidID,hidVal,isDestruct) {
	$liCity = $(sunStr+" li.parent_node");
	$liCity.click(function() {
		var id = $(this).find('.cls_value').attr('rel');
		var val = $(this).find('.cls_value').html();
		var pid = $(this).find('.cls_value').attr('pid');
		var ptitle = $(this).find('.cls_value').attr('ptitle');
		var index = $liCity.index(this);
		$liCity.each(function() {
			$(this).removeClass('current');
		});
		$liCity.eq(index).addClass('current');
		$(sunStr+" div").hide();
		if(isHavaGrand(cityStr,id)) {
			$("#"+id).show();
			$("#"+id+" li").click(function() {
				var labID = $(this).find('.cls_child').attr('value');
				var labVal = $(this).find('.cls_child').attr('title');
				var sid = $(this).find('.cls_child').attr('sid');
				var sname = $(this).find('.cls_child').attr('sname');
				$(checkbox).html(getCheckInfo(labID,labVal,sid,sname));
				$(clickObjID).html(sname);
				$(hidID).val(sid);
				$(hidVal).val(sname);
				if(isDestruct) {
					getDistrictId();
				}
				closeDialog(showID);
			});
		} else {
			$(checkbox).html(getCheckInfo(id,val,pid,ptitle));
			$(clickObjID).html(ptitle);
			$(hidID).val(pid);
			$(hidVal).val(ptitle);
			if(isDestruct) {
				getDistrictId();
			}
			closeDialog(showID);
		}
	});
}
// 关闭弹窗
function closeDialog(showID) {
	$(showID).hide();
	$(".menu_bg_layer").remove();
}
// 判断选择的数量是否超出
function getCheckNum(checkbox) {
	var chenkNum = $(checkbox+" a");
	if (chenkNum.length >= 3) {
		alert("最多可选3个");
		return false;
	} else {
		return true;
	}
}
// 获取选择信息
function getCheckInfo(id,val,pid,pname) {
	return '<a gid="'+pid+'" gname="'+pname+'" id="checked_value_'+id+'" class="sx-yx-cnt" href="javascript:;"><span rel="'+id+'">'+val+'</span><i id="checked_value_del_'+id+'" rel="'+id+'" class="del cls_checked_del"></i></a>';
}
// 是否有三级分类
function isHavaGrand(grandStr,id){
	if(grandStr[id] != null) {
		return true;
	} else {
		return false;
	}
}
// 获取省级城市
function getProvinceCity(proStr){
	var htmlstr='';
	$.each(proStr, function(index, val) {
		 var v = val.split(",");
		 htmlstr+="<li id=\"li_city_"+v[0]+"\" class=\"parent_node\"><a id=\"p_child_value_"+v[0]+"\" rel=\""+v[0]+"\" href=\"javascript:;\" class=\"cls_value\">"+v[1]+"</a><i onclick=\"javascript:;\"></i></li>";
	});
	return htmlstr;
}
// 取消冒泡
function removeClick(e){
    e.cancelBubble = true;
}
// 工作地区ID赋值
function getDistrictId() {
	var idArray = $("#districtID").val().split(".");
	$("#district").val(idArray[0]);
	$("#sdistrict").val(idArray[1]);
	if (idArray.length == 3) {
		$("#tdistrict").val(idArray[2]);
	} else {
		$("#tdistrict").val('');
	}
}
// 期望职位弹出框
function showIntentionJobsBox(clickObjID,showID,showJobsTypeArea,showGradJobsArea,checkBoxJobs,jobscheckbox,saveChecked,intentionJobscn,intentionJobsID,QSarrParent,QSarr) {
	$(clickObjID).click(function(){
		$(this).blur();
		$(this).before('<div class="menu_bg_layer" style="position:absolute;left:0px;top:0px;z-index:9;background-color:#000000;"></div>');
		$(".menu_bg_layer").css({"width":$(document).width(),"height":$(document).height(),"opacity":0.3});
		$(showJobsTypeArea).html(getParentJobs(QSarrParent,QSarr));
		makeGrandJob(showGradJobsArea,QSarr);
		recoverJob(checkBoxJobs,showJobsTypeArea);
		$(showID).show();
		// 点击二级职位分类
		$parnode_li = $(showJobsTypeArea+" li.parent_node");
		$parnode_li.live('click',function(){
			$parnode_li.each(function() {
				$(this).removeClass('current');
			});
			var pRel = $(this).find('.cls_value').attr('rel');
			var pName = $(this).find('.cls_value').html();
			$(this).addClass('current').siblings().removeClass('current');
			// 显示三级职位分类
			var showDivID = $parnode_li.index(this);
			$subnode_dir = $(showGradJobsArea+" div.sublist_node");
			$subnode_dir.each(function() {
				$(this).hide();
			});
			$subnode_dir.eq(showDivID).show();
			$(showGradJobsArea+" div.sublist_node :checkbox").unbind().click(function() {
				if($(this).attr("checked")) {
					if(getCheckNum(checkBoxJobs)){
						var labID = $(this).attr('value');
						var labVal = $(this).attr('title');
						var sid = $(this).attr('sid');
						var sname = $(this).attr('sname');
						var lrel = $(this).attr('rel');
						$(checkBoxJobs).append(getCheckJob(labID,labVal,sid,sname,lrel));
						$(checkBoxJobs+" i").unbind().click(function(){
							var ival =  $(this).attr('rel');
							$(this).parent().remove();
							$(showJobsTypeArea+" :checkbox[checked]").each(function() {
								if($(this).val() == ival){
									$(this).attr('checked',false);
								}
							});
						});
					} else {
						$(this).attr('checked',false);
					}
				} else {
					var selval = $(this).val();
					$(checkBoxJobs+" a").each(function() {
						var chval = $(this).find('span').attr('rel');
						if(chval == selval) {
							$(this).remove();
						}
					});
				}
			});
		});
		$(saveChecked).click(function(){
			$a_checkbox = $(checkBoxJobs+" a");
			var checkhtm = '';
			var a_cn=new Array();
			var a_id=new Array();
			$a_checkbox.each(function(index) {
				var checkVal = $(this).find('span').html();
				var lel = $(this).find('span').attr('rel');
				checkhtm+='<div class="input_checkbox"><span rel="'+lel+'">'+checkVal+'</span></div>';
				a_cn[index]=$(this).attr("gname");
				a_id[index]=$(this).attr("gid");
			});
			$(jobscheckbox+" .showchecked").html(checkhtm);
			$(jobscheckbox+" .showchecked span").click(function(){
				$(this).parent().remove();
				var slel = $(this).attr('rel');
				$a_checkbox.each(function(index) {
					var alel = $(this).find('span').attr('rel');
					var agid = $(this).attr('gid');
					if (alel == slel) {
						$(this).remove();
						var joid = $(intentionJobsID).val().split("-");
						joid.splice($.inArray(agid,joid),1);
						$(intentionJobsID).val(joid.join("-"));
						return false;
					}
				});
				$(showJobsTypeArea+" :checkbox[checked]").each(function() {
					if($(this).val() == slel){
						$(this).attr('checked',false);
					}
				});
			});
			$(intentionJobscn).val(a_cn.join(","));
			$(intentionJobsID).val(a_id.join("-"));
			closeDialog(showID);
		});
		$(".menu_bg_layer").click(function() {
			closeDialog(showID);
		});
		$(".cm_closeMsg").click(function() {
			closeDialog(showID);
		});
	});
}
// 恢复已选的期望职位
function recoverJob(checkBoxJobs,showJobsTypeArea) {
	if($(checkBoxJobs+" a").length > 0) {
		$(checkBoxJobs+" a").each(function() {
			var ival = $(this).find('span').attr('rel');
			var lid = $(this).find('span').attr('lid');
			$("#li_zhineng_"+lid).addClass('current');
			$(showJobsTypeArea+" div.sublist_node :checkbox").each(function() {
				if($(this).val() == ival) {
					$(this).attr('checked',true);
				}
			});
		});
	} else {
		return false;
	}
}
// 获取选择信息
function getCheckJob(id,val,pid,pname,lrel) {
	return '<a gid="'+pid+'" gname="'+pname+'" id="checked_value_'+id+'" class="sx-yx-cnt" href="javascript:;"><span rel="'+id+'" lid="'+lrel+'">'+val+'</span><i id="checked_value_del_'+id+'" rel="'+id+'" class="del cls_checked_del"></i></a>';
}
// 生成职位顶级分类
function getParentJobs(praStr,sunStr) {
	var htmstr = '';
	$.each(praStr, function(index, val) {
		var v = val.split(",");
		var v_cn = v[1].split("|");
		var arrhtm = v_cn.join("·");
		htmstr+='<div class="sx-cnt sx-cnt2"><div style="padding-top:10px;" class="sx-lev1-pd"><div class="sx-lev1-line"><div id="parent_value_'+v[0]+'" class="sx-lev1">'+arrhtm+'</div></div></div><div style="padding-bottom: 0px;" class="sx-nomal">'+getSunJobs(sunStr,v[0],v[1])+'</div></div>';
	});
	return htmstr;
}
// 生成职位二级分类
function getSunJobs(sunStr,id,pName){
	var sunJob = sunStr[id].split("|");
	var htmlstr='<ul style="width:760px;" class="cf">';
	$.each(sunJob, function(index, val) {
		 var v = val.split(",");
		 var ptitle = pName+"/"+v[1];
		 var pid = id+"."+v[0];
		 if((index + 1)%3 ==0) {
		 	htmlstr+="<li id=\"li_zhineng_"+v[0]+"\" class=\"parent_node\"><a id=\"child_value_"+v[0]+"\" rel=\""+v[0]+"\" href=\"javascript:;\" pid=\""+pid+"\" ptitle=\""+ptitle+"\" class=\"cls_value\">"+v[1]+"</a><i onclick=\"javascript:;\"></i></li></ul><ul style=\"width: 760px;\" class=\"cf\">";
		 } else {
		 	htmlstr+="<li id=\"li_zhineng_"+v[0]+"\" class=\"parent_node\"><a id=\"child_value_"+v[0]+"\" rel=\""+v[0]+"\" href=\"javascript:;\" pid=\""+pid+"\" ptitle=\""+ptitle+"\" class=\"cls_value\">"+v[1]+"</a><i onclick=\"javascript:;\"></i></li>";
		 }
	});
	return htmlstr;
}
// 获取三级职位分类
function getGrandJob(grandStr,id,Stitle,Spid) {
	if(grandStr[id] != null) {
		var grandCity = grandStr[id].split("|");
		var htmlstr='<div id="sublist_zhineng_'+id+'" style="display:none;" class="sx-sub sublist_node"><ul style="width:760px;" class="cf">';
		$.each(grandCity, function(index, val) {
			 var v = val.split(",");
			 var sid = Spid+"."+v[0];
			 var sname = Stitle+"/"+v[1];
			 htmlstr+="<li><label><input sid=\""+sid+"\" sname=\""+sname+"\" type=\"checkbox\" id=\"child_value_"+v[0]+"\" title=\""+v[1]+"\" rel=\""+id+"\" value=\""+v[0]+"\" class=\"cls_child\">"+v[1]+"</label></li>";
		});
		htmlstr+="</ul></div>";
		return htmlstr;
	} else {
		return '';
	}
}
// 二级职位分类下插入三级职位分类
function makeGrandJob(ulStr,grandStr) {
	var ulCity = $(ulStr+" ul");
	$.each(ulCity, function() {
		 var liCity = $(this).find("li");
		 var lihtml = '';
		 $.each(liCity, function() {
		 	var Srel = $(this).find('.cls_value').attr('rel');
		 	var Stitle = $(this).find('.cls_value').attr('ptitle');
		 	var Spid = $(this).find('.cls_value').attr('pid');
		 	var val = getGrandJob(grandStr,Srel,Stitle,Spid);
		 	if (val != '') {
		 		lihtml+=val;
		 	}
		 });
		 $(this).after(lihtml);
	});
}
// 期望行业弹出框
function showIntentionTradBox(clickObjID,showID,htmTrad,checkBoxTrad,btnSaveTrad,tradCN,tradID,showTradCheck,QSarr){
	$(clickObjID).click(function() {
		$(this).blur();
		$(this).before('<div class="menu_bg_layer" style="position:absolute;left:0px;top:0px;z-index:9;background-color:#000000;"></div>');
		$(".menu_bg_layer").css({"width":$(document).width(),"height":$(document).height(),"opacity":0.3});
		$(htmTrad).html(getParentTrad(QSarr));
		recoverTrad(checkBoxTrad,htmTrad);
		$(showID).show();
		$(htmTrad+" :checkbox").unbind().click(function(){
			if($(this).attr("checked")) {
				if(getCheckNum(checkBoxTrad)){
					var tid = $(this).val();
					var tname = $(this).attr('title');
					$(checkBoxTrad).append(getCheckTrad(tid,tname));
					$(checkBoxTrad+" i").unbind().click(function(){
						var ival =  $(this).attr('rel');
						$(this).parent().remove();
						$(htmTrad+" :checkbox[checked]").each(function() {
							if($(this).val() == ival){
								$(this).attr('checked',false);
							}
						});
					});
				} else {
					$(this).attr('checked',false);
				}
			} else {
				var selval = $(this).val();
				$(checkBoxTrad+" a").each(function() {
					var chval = $(this).find('span').attr('rel');
					if(chval == selval) {
						$(this).remove();
					}
				});
			}
		});
		$(btnSaveTrad).click(function(){
			$a_checkbox = $(checkBoxTrad+" a");
			var checkhtm = '';
			var a_cn=new Array();
			var a_id=new Array();
			$a_checkbox.each(function(index) {
				var checkVal = $(this).find('span').html();
				var checkRel = $(this).find('span').attr('rel');
				checkhtm+='<div class="input_checkbox"><span rel="'+checkRel+'">'+checkVal+'</span></div>';
				a_cn[index]=checkVal;
				a_id[index]=checkRel;
			});
			$(showTradCheck+" .showcheckoption").html(checkhtm);
			$(showTradCheck+" .showcheckoption span").click(function(){
				$(this).parent().remove();
				var slel = $(this).attr('rel');
				$a_checkbox.each(function(index) {
					var alel = $(this).find('span').attr('rel');
					if (alel == slel) {
						$(this).remove();
						var trid = $(tradID).val().split(",");
						trid.splice($.inArray(alel,trid),1);
						$(tradID).val(trid);
						return false;
					}
				});
				$(htmTrad+" :checkbox[checked]").each(function() {
					if($(this).val() == slel){
						$(this).attr('checked',false);
					}
				});

			});
			$(tradCN).val(a_cn.join(","));
			$(tradID).val(a_id.join(","));
			closeDialog(showID);
		});
		$(".menu_bg_layer").click(function() {
			closeDialog(showID);
		});
		$(".cm_closeMsg").click(function() {
			closeDialog(showID);
		});
	});
}
// 恢复已选的行业
function recoverTrad(checkBoxTrad,showTradArea) {
	if($(checkBoxTrad+" a").length > 0) {
		$(checkBoxTrad+" a").each(function() {
			var ival = $(this).find('span').attr('rel');
			$(showTradArea+" :checkbox").each(function() {
				if($(this).val() == ival) {
					$(this).attr('checked',true);
				}
			});
		});
	} else {
		return false;
	}
}
// 获得选中行业
function getCheckTrad(id,name){
	return '<a id="checked_value_'+id+'" class="sx-yx-cnt" href="javascript:;"><span rel="'+id+'">'+name+'</span><i id="checked_value_del_'+id+'" rel="'+id+'" class="del cls_checked_del"></i></a>';
}
// 生成行业分类
function getParentTrad(praStr) {
	var htmstr = '<div class="sx-cnt sx-cnt2"><div style="padding-bottom: 0px;" class="sx-nomal"><ul style="width: 760px;" class="cf">';
	$.each(praStr, function(index, val) {
		var v = val.split(",");
		htmstr+="<li style=\"border-top-width: 0px; padding: 0px 0px 3px 20px; width: 230px; text-align: left;\"><label><input type=\"checkbox\" id=\"child_value_"+v[0]+"\" title=\""+v[1]+"\" value=\""+v[0]+"\" class=\"cls_child\">"+v[1]+"</label></li>";
	});
	htmstr+='</ul></div></div>';
	return htmstr;
}
// 特长标签
function showTagBox(clickObjID,showID,htmTrad,checkBoxTag,btnSaveTag,tagID,showTagCheck,QSarr) {
	$(clickObjID).click(function() {
		$(this).blur();
		$(this).before('<div class="menu_bg_layer" style="position:absolute;left:0px;top:0px;z-index:9;background-color:#000000;"></div>');
		$(".menu_bg_layer").css({"width":$(document).width(),"height":$(document).height(),"opacity":0.3});
		$(htmTrad).html(getParentTag(QSarr));
		recoverTag(checkBoxTag,htmTrad);
		$(showID).show();
		$(htmTrad+" :checkbox").unbind().click(function(){
			if($(this).attr("checked")) {
				if(getCheckNum(checkBoxTag)){
					var tid = $(this).val();
					var tname = $(this).attr('title');
					$(checkBoxTag).append(getCheckTag(tid,tname));
					$(checkBoxTag+" i").unbind().click(function(){
						var ival =  $(this).attr('rel');
						$(this).parent().remove();
						$(htmTrad+" :checkbox[checked]").each(function() {
							if($(this).val() == ival){
								$(this).attr('checked',false);
							}
						});
					});
				} else {
					$(this).attr('checked',false);
				}
			} else {
				var selval = $(this).val();
				$(checkBoxTag+" a").each(function() {
					var chval = $(this).find('span').attr('rel');
					if(chval == selval) {
						$(this).remove();
					}
				});
			}
		});
		$(btnSaveTag).click(function(){
			$a_checkbox = $(checkBoxTag+" a");
			var checkhtm = '';
			var a_cn=new Array();
			var a_id=new Array();
			$a_checkbox.each(function(index) {
				var checkVal = $(this).find('span').html();
				var checkRel = $(this).find('span').attr('rel');
				checkhtm+='<div class="input_checkbox"><span>'+checkVal+'</span></div>';
				a_cn[index]=checkVal;
				a_id[index]=checkRel + "," +checkVal;
			});
			$(showTagCheck+" .showchecktag").html(checkhtm);
			$(tagID).val(a_id.join("|"));
			closeDialog(showID);
		});
		$(".menu_bg_layer").click(function() {
			closeDialog(showID);
		});
		$(".cm_closeMsg").click(function() {
			closeDialog(showID);
		});
	});
}
// 恢复已选的特长标签
function recoverTag(checkBoxTag,showTradArea) {
	if($(checkBoxTag+" a").length > 0) {
		$(checkBoxTag+" a").each(function() {
			var ival = $(this).find('span').attr('rel');
			$(showTradArea+" :checkbox").each(function() {
				if($(this).val() == ival) {
					$(this).attr('checked',true);
				}
			});
		});
	} else {
		return false;
	}
}
// 获得选中特长标签
function getCheckTag(id,name){
	return '<a id="checked_value_'+id+'" class="sx-yx-cnt" href="javascript:;"><span rel="'+id+'">'+name+'</span><i id="checked_value_del_'+id+'" rel="'+id+'" class="del cls_checked_del"></i></a>';
}
// 生成标签分类
function getParentTag(praStr) {
	var htmstr = '<div class="sx-cnt sx-cnt2"><div style="padding-bottom: 0px;" class="sx-nomal"><ul style="width: 760px;" class="cf">';
	$.each(praStr, function(index, val) {
		var v = val.split(",");
		htmstr+="<li style=\"border-top-width: 0px; padding: 0px 0px 3px 20px; width: 230px; text-align: left;\"><label><input type=\"checkbox\" id=\"child_value_"+v[0]+"\" title=\""+v[1]+"\" value=\""+v[0]+"\" class=\"cls_child\">"+v[1]+"</label></li>";
	});
	htmstr+='</ul></div></div>';
	return htmstr;
}
function chechkcli(chid,htmid){
		$(chid+" i").unbind().click(function(){
			var ival =  $(this).attr('rel');
			$(this).parent().remove();
			$(htmid+" :checkbox[checked]").each(function() {
				if($(this).val() == ival){
					$(this).attr('checked',false);
				}
			});
		});
	}
	// 恢复现居住地
	if($("#residence_cn").val()) {
		var pgsnameArr = new Array();
		var pgsname = '';
		var cityopthtm = '';
		var valval = $("#residence").val();
		 var citystr = valval.split('.');
		 var pname = '';
		 $.each(QS_city_parent, function(vindex, valv) {
		 	 var vid = valv.split(",");
		 	 if(citystr[0] == vid[0]) {
		 	 	pname = vid[1];
		 	 }
		 });
		 var gname = '';
		 var gns = QS_city[citystr[0]].split("|");
		 $.each(gns, function(gindex, galv) {
		 	 var gvid = galv.split(",");
		 	 if(citystr[1] == gvid[0]) {
		 	 	gname = gvid[1];
		 	 }
		 });
		 pgsname += pname + "/" + gname; 
		 cityopthtm += '<a href="javascript:;" class="sx-yx-cnt" id="checked_value_'+citystr[1]+'" gname="'+pgsname+'" gid="'+valval+'"><span rel="'+citystr[1]+'">'+gname+'</span><i class="del cls_checked_del" rel="'+citystr[1]+'" id="checked_value_del_'+citystr[1]+'"></i></a>';
		 if(QS_city[citystr[1]]) {
		 	var sname = '';
		 	var sns = QS_city[citystr[1]].split("|");
			 $.each(sns, function(sindex, salv) {
			 	 var svid = salv.split(",");
			 	 if(citystr[2] == svid[0]) {
			 	 	sname = svid[1];
			 	 }
			 });
			 pgsname += "/" + sname;
			 cityopthtm += '<a href="javascript:;" class="sx-yx-cnt" id="checked_value_'+citystr[2]+'" gname="'+pgsname+'" gid="'+valval+'"><span lid="'+citystr[1]+'" rel="'+citystr[2]+'">'+sname+'</span><i class="del cls_checked_del" rel="'+citystr[2]+'" id="checked_value_del_'+citystr[2]+'"></i></a>';
		 }
		 pgsnameArr.push(pgsname);
		$("#showCityBox").html(pgsnameArr.join(","));
		$("#box_checked").html(cityopthtm);
		chechkcli("#box_checked","#sx-nomal");
	}
	// 恢复籍贯
	if($("#householdaddress_cn").val()) {
		var pgsnameArr = new Array();
		var pgsname = '';
		var cityopthtm = '';
		var valval = $("#householdaddress").val();
		 var citystr = valval.split('.');
		 var pname = '';
		 $.each(QS_city_parent, function(vindex, valv) {
		 	 var vid = valv.split(",");
		 	 if(citystr[0] == vid[0]) {
		 	 	pname = vid[1];
		 	 }
		 });
		 var gname = '';
		 var gns = QS_city[citystr[0]].split("|");
		 $.each(gns, function(gindex, galv) {
		 	 var gvid = galv.split(",");
		 	 if(citystr[1] == gvid[0]) {
		 	 	gname = gvid[1];
		 	 }
		 });
		 pgsname += pname + "/" + gname; 
		 cityopthtm += '<a href="javascript:;" class="sx-yx-cnt" id="checked_value_'+citystr[1]+'" gname="'+pgsname+'" gid="'+valval+'"><span rel="'+citystr[1]+'">'+gname+'</span><i class="del cls_checked_del" rel="'+citystr[1]+'" id="checked_value_del_'+citystr[1]+'"></i></a>';
		 if(QS_city[citystr[1]]) {
		 	var sname = '';
		 	var sns = QS_city[citystr[1]].split("|");
			 $.each(sns, function(sindex, salv) {
			 	 var svid = salv.split(",");
			 	 if(citystr[2] == svid[0]) {
			 	 	sname = svid[1];
			 	 }
			 });
			 pgsname += "/" + sname;
			 cityopthtm += '<a href="javascript:;" class="sx-yx-cnt" id="checked_value_'+citystr[2]+'" gname="'+pgsname+'" gid="'+valval+'"><span lid="'+citystr[1]+'" rel="'+citystr[2]+'">'+sname+'</span><i class="del cls_checked_del" rel="'+citystr[2]+'" id="checked_value_del_'+citystr[2]+'"></i></a>';
		 }
		 pgsnameArr.push(pgsname);
		$("#showCityBoxHoldAddress").html(pgsnameArr.join(","));
		$("#box_checkedHoldAddress").html(cityopthtm);
		chechkcli("#box_checkedHoldAddress","#sx-nomalHoldAddress");
	}
	// 恢复工作地区
	if($("#district_cn").val()) {
		var pgsnameArr = new Array();
		var pgsname = '';
		var cityopthtm = '';
		var valval = $("#districtID").val();
		 var citystr = valval.split('.');
		 var pname = '';
		 $.each(QS_city_parent, function(vindex, valv) {
		 	 var vid = valv.split(",");
		 	 if(citystr[0] == vid[0]) {
		 	 	pname = vid[1];
		 	 }
		 });
		 var gname = '';
		 var gns = QS_city[citystr[0]].split("|");
		 $.each(gns, function(gindex, galv) {
		 	 var gvid = galv.split(",");
		 	 if(citystr[1] == gvid[0]) {
		 	 	gname = gvid[1];
		 	 }
		 });
		 pgsname += pname + "/" + gname; 
		 cityopthtm += '<a href="javascript:;" class="sx-yx-cnt" id="checked_value_'+citystr[1]+'" gname="'+pgsname+'" gid="'+valval+'"><span rel="'+citystr[1]+'">'+gname+'</span><i class="del cls_checked_del" rel="'+citystr[1]+'" id="checked_value_del_'+citystr[1]+'"></i></a>';
		 if(QS_city[citystr[1]]) {
		 	var sname = '';
		 	var sns = QS_city[citystr[1]].split("|");
			 $.each(sns, function(sindex, salv) {
			 	 var svid = salv.split(",");
			 	 if(citystr[2] == svid[0]) {
			 	 	sname = svid[1];
			 	 }
			 });
			 pgsname += "/" + sname;
			 cityopthtm += '<a href="javascript:;" class="sx-yx-cnt" id="checked_value_'+citystr[2]+'" gname="'+pgsname+'" gid="'+valval+'"><span lid="'+citystr[1]+'" rel="'+citystr[2]+'">'+sname+'</span><i class="del cls_checked_del" rel="'+citystr[2]+'" id="checked_value_del_'+citystr[2]+'"></i></a>';
		 }
		 pgsnameArr.push(pgsname);
		$("#showCityBoxDistrict").html(pgsnameArr.join(","));
		$("#box_checkedDistrict").html(cityopthtm);
		chechkcli("#box_checkedDistrict","#sx-nomalDistrict");
	}
	// 恢复期望职位
	if($("#intention_jobs").val()) {
		var jobstrarray = $("#intention_jobs_id").val().split("-");
		var pgsnameArr = new Array();
		var pgsname = '';
		var jobopthtm = '';
		var jobdivhtm = '';
		$.each(jobstrarray, function(index, val) {
			 var jobstr = val.split('.');
			 var pname = '';
			 $.each(QS_jobs_parent, function(vindex, valv) {
			 	 var vid = valv.split(",");
			 	 if(jobstr[0] == vid[0]) {
			 	 	pname = vid[1];
			 	 }
			 });
			 var gname = '';
			 var gns = QS_jobs[jobstr[0]].split("|");
			 $.each(gns, function(gindex, galv) {
			 	 var gvid = galv.split(",");
			 	 if(jobstr[1] == gvid[0]) {
			 	 	gname = gvid[1];
			 	 }
			 });
			 var sname = '';
			 var sns = QS_jobs[jobstr[1]].split("|");
			 $.each(sns, function(sindex, salv) {
			 	 var svid = salv.split(",");
			 	 if(jobstr[2] == svid[0]) {
			 	 	sname = svid[1];
			 	 }
			 });
			 pgsname += pname + "/" + gname + "/" + sname;
			 pgsnameArr.push(pgsname);
			 jobopthtm += '<a href="javascript:;" class="sx-yx-cnt" id="checked_value_'+jobstr[2]+'" gname="'+pgsname+'" gid="'+val+'"><span lid="'+jobstr[1]+'" rel="'+jobstr[2]+'">'+sname+'</span><i class="del cls_checked_del" rel="'+jobstr[2]+'" id="checked_value_del_'+jobstr[2]+'"></i></a>';
			 jobdivhtm += '<div class="input_checkbox"><span rel="'+jobstr[2]+'">'+sname+'</span></div>';
		});
		$("#jobs_checkbox .showchecked").html(jobdivhtm);
		$("#box_checkedJobs").html(jobopthtm);
		chechkcli("#box_checkedJobs","#showJobsType");
		$a_checkbox = $("#box_checkedJobs a");
		$("#jobs_checkbox .showchecked span").click(function(){
			var slel = $(this).attr('rel');
			$a_checkbox.each(function(index) {
				var alel = $(this).find('span').attr('rel');
				var agid = $(this).attr('gid');
				if (alel == slel) {
					$(this).remove();
					var joid = $("#intention_jobs_id").val().split("-");
					joid.splice($.inArray(agid,joid),1);
					$("#intention_jobs_id").val(joid.join("-"));
					return false;
				}
			});
			$("#showJobsType :checkbox[checked]").each(function() {
				if($(this).val() == slel){
					$(this).attr('checked',false);
				}
			});
			$(this).parent().remove();
		});
	}
	// 恢复期望行业
	if($("#trade_cn").val()) {
		var tradstr = $("#trade").val().split(",");
		var tradename = new Array();
		var tradopthtm = '';
		var traddivhtm = '';
		$.each(tradstr, function(index, val) {
			for(var i = 0;i < QS_trade.length;i++) {
				arr = QS_trade[i].split(",");
				if (arr[0] == val) {
					tradename.push(arr[1]);
					tradopthtm += '<a href="javascript:;" class="sx-yx-cnt" id="checked_value_'+arr[0]+'"><span rel="'+arr[0]+'">'+arr[1]+'</span><i class="del cls_checked_del" rel="'+arr[0]+'" id="checked_value_del_'+arr[0]+'"></i></a>';
					traddivhtm += '<div class="input_checkbox"><span rel="'+val+'">'+arr[1]+'</span></div>';
				}
			}
		});
		$("#box_checkedTrad").html(tradopthtm);
		$("#trade_checkbox .showcheckoption").html(traddivhtm);
		chechkcli("#box_checkedTrad","#showTradType");
		$a_checkbox = $("#box_checkedTrad a");
		$("#trade_checkbox .showcheckoption span").click(function(){
			var slel = $(this).attr('rel');
			$a_checkbox.each(function(index) {
				var alel = $(this).find('span').attr('rel');
				var agid = $(this).attr('gid');
				if (alel == slel) {
					$(this).remove();
					var joid = $("#trade").val().split("-");
					joid.splice($.inArray(agid,joid),1);
					$("#trade").val(joid.join("-"));
					return false;
				}
			});
			$("#showTradType :checkbox[checked]").each(function() {
				if($(this).val() == slel){
					$(this).attr('checked',false);
				}
			});
			$(this).parent().remove();
		});
	}
	// 恢复特长标签
	if($("#tag").val()){
		var tagarray = $("#tag").val().split("|");
		var tagotphtm = '';
		var ctagopt = '';
		$.each(tagarray, function(index, val) {
		 	var tagstr = val.split(",");
		 	tagotphtm += '<div class="input_checkbox"><span rel="'+tagstr[0]+'">'+tagstr[1]+'</span></div>';
		 	ctagopt += '<a id="checked_value_'+tagstr[0]+'" class="sx-yx-cnt" href="javascript:;"><span rel="'+tagstr[0]+'">'+tagstr[1]+'</span><i id="checked_value_del_'+tagstr[0]+'" rel="'+tagstr[0]+'" class="del cls_checked_del"></i></a>'
		});
		$("#tag_checkbox .showchecktag").html(tagotphtm);
		$("#box_checkedTag").html(ctagopt);
		chechkcli("#box_checkedTag","#showTag");
	}
	// 特长标签点击删除
	$("#tag_checkbox .input_checkbox span").live('click', function() {
		$(this).parent().remove();
		var rel = $(this).attr('rel');
		var relarray = new Array();
		relarray[0] = rel;
		relarray[1] = $(this).html();
		var arr = $("#tag").val().split("|");
		arr.splice($.inArray(relarray,arr),1);
		$("#tag").val(arr.join("|"));
		$tag_a = $("#box_checkedTag a");
		$tag_a.each(function(index, el) {
			var ctagrel = $(this).find('span').attr("rel");
			if(rel == ctagrel) {
				$(this).remove();
			}
		});
	});
	// 猎头行业恢复
	if($("#trade_cn").val()) {
		var tradstr = $("#trade").val().split(",");
		var tradename = new Array();
		var tradopthtm = '';
		var traddivhtm = '';
		$.each(tradstr, function(index, val) {
			for(var i = 0;i < QS_trade.length;i++) {
				arr = QS_trade[i].split(",");
				if (arr[0] == val) {
					tradename.push(arr[1]);
					tradopthtm += '<a href="javascript:;" class="sx-yx-cnt" id="checked_value_'+arr[0]+'"><span rel="'+arr[0]+'">'+arr[1]+'</span><i class="del cls_checked_del" rel="'+arr[0]+'" id="checked_value_del_'+arr[0]+'"></i></a>';
					traddivhtm += '<div class="input_checkbox"><span rel="'+val+'">'+arr[1]+'</span></div>';
				}
			}
		});
		$("#box_checkedTrad").html(tradopthtm);
		$("#trade_checkbox .showcheckoption").html(traddivhtm);
		chechkcli("#box_checkedTrad","#showTradType");
		$a_checkboxt = $("#box_checkedTrad a");
		$("#trade_checkbox .showcheckoption span").click(function(){
			var slel = $(this).attr('rel');alert(slel);
			$a_checkboxt.each(function(index) {
				var alel = $(this).find('span').attr('rel');
				var agid = $(this).attr('gid');
				if (alel == slel) {
					$(this).remove();
					var joid = $("#trade").val().split(",");
					joid.splice($.inArray(agid,joid),1);
					$("#trade").val(joid.join(","));
					return false;
				}
			});
			$("#showTradType :checkbox[checked]").each(function() {
				if($(this).val() == slel){
					$(this).attr('checked',false);
				}
			});
			$(this).parent().remove();
		});
	}
