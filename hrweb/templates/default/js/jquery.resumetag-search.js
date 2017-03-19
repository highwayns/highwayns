function allaround(dir,getstr){
	var checkedstr = "";
	fillTag("#divTagCate"); // 行业填充内容
	// 恢复行业选中条件
	if(getstr) {
		if(getstr[0]) {
			var recoverTradArray = getstr[0].split(",");
			$.each(recoverTradArray, function(index, val) {
				 $("#tagList a").each(function() {
					if(val == $(this).attr('cln')) {
						$(this).addClass('selectedcolor');
					}
				});
			});
			copyTradItem();
			var a_cn = new Array();
			$("#tagAcq a").each(function(index) {
				var checkText = $(this).attr('title');
				a_cn[index]=checkText;
			});
			$("#tagText").html(a_cn.join(","));
			$("#tagText").css("color","#4095ef");
			$("#jobsTag").css("border-color","#4095ef");
			checkedstr += '<a href="javascript:;" ty="tag" class="cnt"><span>'+$("#tagText").html()+'</span><i class="del"></i></a>';
		}
	}
	/* 行业列表点击显示到已选 */
	$("#tagList li a").unbind().live('click', function() {
		// 判断选择的数量是否超出
		if($("#tagList .selectedcolor").length >= 5) {
			$("#tagropcontent").show(0).delay(3000).fadeOut("slow");
		} else {
			$(this).addClass('selectedcolor');
			copyTradItem(); // 将行业已选的拷贝
		}
	});
	// 行业确定选择
	$("#tagSure").unbind().click(function() {
		var a_cn=new Array();
		var a_id=new Array();
		$("#tagAcq a").each(function(index) {
			var checkID = $(this).attr('rel');
			var checkText = $(this).attr('title');
			a_id[index]=checkID;
			a_cn[index]=checkText;
		});
		if (a_cn.length > 0) {
			$("#tagText").html(a_cn.join(","));
			$("#tagText").css("color","#4095ef");
			$("#jobsTag").css("border-color","#4095ef");
			$("#tag_cn").val(a_cn.join(","));
			$("#tag").val(a_id.join(","));
		} else {
			$("#tagText").html("请选择标签");
			$("#tagText").css("color","#cccccc");
			$("#jobsTag").css("border-color","#cccccc");
			$("#tag_cn").val("");
			$("#tag").val("");
		}
		$("#divTagCate").hide();
	});
	fillJobs("#divJobCate"); // 职位填充内容
	// 恢复职位选中条件
	if(getstr) {
		if(getstr[1]) {
			var recoverJobArray = getstr[1].split(",");
			$.each(recoverJobArray, function(index, val) {
				 var demojobArray = val.split(".");
				 if(demojobArray[2] == "0") { // 如果第三个参数是0 则只选到第二级
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
			$("#jobText").css("color","#4095ef");
			$("#jobsSort").css("border-color","#4095ef");
			checkedstr += '<a href="javascript:;" ty="jobs_id" class="cnt"><span>'+$("#jobText").html()+'</span><i class="del"></i></a>';
		}
	}
	/* 职位列表点击显示到已选 */
	$("#divJobCate li p a").unbind().live('click', function() {
		// 判断选择的数量是否超出
		if($("#divJobCate .selectedcolor").length >= 5) {
			$("#jobdropcontent").show(0).delay(3000).fadeOut("slow");
		} else {
			$(this).addClass('selectedcolor');
			copyJobItem(); // 将职位已选的拷贝
		}
	});
	$("#divJobCate .subcate a").unbind().live('click', function() {
		// 判断选择的数量是否超出
		if($("#divJobCate .selectedcolor").length >= 5) {
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
			// 如果选择的是二级分类将第三个参数补0
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
			$("#jobText").css("color","#4095ef");
			$("#jobsSort").css("border-color","#4095ef");
			$("#jobs_cn").val(a_cn.join(","));
			$("#jobs_id").val(a_id.join(","));
		} else {
			$("#jobText").html("请选择职位类别");
			$("#jobText").css("color","#cccccc");
			$("#jobsSort").css("border-color","#cccccc");
			$("#jobs_cn").val("");
			$("#jobs_id").val("");
		}
		$("#divJobCate").hide();
	});
	fillCity("#divCityCate"); // 地区内容填充
	// 恢复地区选中条件
	if(getstr) {
		if(getstr[2]) {
			var recoverCityArray = getstr[2].split(",");
			$.each(recoverCityArray, function(index, val) {
				 var democityArray = val.split(".");
				 if(democityArray[1] == 0) { // 如果第二个参数为 0 说明选择的是一级地区
				 	$(".citycatebox p a").each(function() {
				 		if(democityArray[0] == $(this).attr("rcoid")) {
				 			$(this).addClass('selectedcolor');
				 		}
				 	});
				 } else { // 选择的是二级地区
				 	$(".citycatebox .subcate a").each(function() {
				 		if(democityArray[1] == $(this).attr("rcoid")) {
				 			$(this).addClass('selectedcolor');
				 		}
				 	});
				 }
			});
			copyCityItem();
			var a_cn=new Array();
			$("#cityAcq a").each(function(index) {
				var checkText = $(this).attr('title');
				a_cn[index]=checkText;
			});
			$("#cityText").html(a_cn.join(","));
			$("#cityText").css("color","#4095ef");
			$("#jobsCity").css("border-color","#4095ef");
			checkedstr += '<a href="javascript:;" ty="district_id" class="cnt"><span>'+$("#cityText").html()+'</span><i class="del"></i></a>';
		}
	}
	/* 地区列表点击显示到已选 */
	$("#divCityCate li p a").unbind().live('click', function(){
		// 判断选择的数量是否超出
		if($("#divCityCate .selectedcolor").length >= 5) {
			$("#citydropcontent").show(0).delay(3000).fadeOut("slow");
		} else {
			$(this).addClass('selectedcolor');
			copyCityItem(); // 将地区已选的拷贝
		}
	});
	$("#divCityCate .subcate a").unbind().live('click', function() {
		// 判断选择的数量是否超出
		if($("#divCityCate .selectedcolor").length >= 5) {
			$("#citydropcontent").show(0).delay(3000).fadeOut("slow");
		} else {
			if($(this).attr("p") == "qb") {
				$(this).parent().prev().find('font a').addClass('selectedcolor');
				$(this).parent().find('a').removeClass('selectedcolor');
			} else {
				$(this).parent().prev().find('font a').removeClass('selectedcolor');
				$(this).addClass('selectedcolor');
			}
			copyCityItem(); // 将地区已选的拷贝
		}
	});
	// 地区确定选择
	$("#citySure").unbind().click(function() {
		var a_cn=new Array();
		var a_id=new Array();
		$("#cityAcq a").each(function(index) {
			// 如果选择的是一级地区将第二个参数补 0
			var chid = new Array();
			if($(this).attr('pid')) {
				chid = $(this).attr('pid').split(".");
				if(chid.length < 2) {
					chid.push(0);
				}
			}
			var checkID = chid.join(".");
			var checkText = $(this).attr('title');
			a_id[index]=checkID;
			a_cn[index]=checkText;
		});
		if (a_cn.length > 0) {
			$("#cityText").html(a_cn.join(","));
			$("#cityText").css("color","#4095ef");
			$("#jobsCity").css("border-color","#4095ef");
			$("#district_cn").val(a_cn.join(","));
			$("#district_id").val(a_id.join(","));
		} else {
			$("#cityText").html("请选择地区分类");
			$("#cityText").css("color","#cccccc");
			$("#jobsCity").css("border-color","#cccccc");
			$("#district_cn").val("");
			$("#district_id").val("");
		}
		$("#divCityCate").hide();
	});
	// 处理关键字搜索框
	$("#searckey").focus(function() {
		if($(this).val() == "请输入关键字") {
			$(this).val('');
		}
	}).blur(function() {
		if($(this).val() == "") {
			$(this).val('请输入关键字');
		}
	});
	// 关键字显示到以选择
	if($("#searckey").attr("data")) {
		checkedstr += '<a href="javascript:;" ty="key" class="cnt"><span>'+$("#searckey").attr("data")+'</span><i class="del"></i></a>';
	}
	// 点击搜索按钮
	$("#btnsearch").unbind().click(function() {
		search_location();
	});
	// 更多条件选项的点击
	$("#searoptions .opt").click(function(){
		var opt=$(this).attr('id');
		opt=opt.split("-");
		$("#searckeybox input[name="+opt[0]+"]").val(opt[1]);
		search_location();
	});
	// 职位列表页面选中条件的显示
	if(checkedstr != "") {
		$("#showselected").html(checkedstr);
		$("#jobselected").show();
	}
	$("#showselected .cnt").click(function(){
		var opt=$(this).attr('ty');
		$("#searckeybox input[name="+opt+"]").val('');
		setTimeout(function() {
			search_location();
		}, 1);
	});
	$("#clearallopt").click(function(){
		$("#searckeybox input[type='hidden']").val('');
		$("#searckeybox input[name='key']").val('');
		setTimeout(function() {
			search_location();
		}, 1);
	});
	// 搜索跳转
	function search_location() {
		var key=$("#searckeybox input[name=key]").val();
		if($("#searckeybox input[name=key]").val() == "请输入关键字") {
			key = '';
		}
		var tag=$("#searckeybox input[name=tag]").val();
		var jobcategory=$("#searckeybox input[name=jobs_id]").val();
		var citycategory=$("#searckeybox input[name=district_id]").val();
		var sort_1=$("#searckeybox input[name=sort]").val();
		var page=$("#searckeybox input[name=page]").val();
		$.get(dir+"plus/ajax_search_location.php", {"act":"QS_resumetag","key":key,"tag":tag,"jobcategory":jobcategory,"citycategory":citycategory,"sort":sort_1,"page":page},
			function (data,textStatus)
			 {	
				 window.location.href=data;
			 },"text"
		);
	}
}
/*
 * 74cms 职位搜索页面 行业内容的填充
|   @param: fillID      -- 填入的ID
*/
function fillTag(fillID){
	var tradli = '';
	$.each(QS_resumetag, function(index, val) {
		if(val) {
			var trads = val.split(",");
		 	tradli += '<li><a title="'+trads[1]+'" cln="'+trads[0]+'" href="javascript:;">'+trads[1]+'</a></li>';
		}
	});
	$(fillID+" ul").html(tradli);
}
/*
 * 74cms 职位搜索页面 拷贝行业已选
*/
function copyTradItem() {
	var tradacqhtm = '';
	$("#tagList .selectedcolor").each(function() {
		tradacqhtm += '<a href="javascript:;" rel="'+$(this).attr('cln')+'" title="'+$(this).attr('title')+'"><div class="text">'+$(this).attr('title')+'</div><div class="close" id="c-'+$(this).attr('cln')+'"></div></a>';
	});
	$("#tagAcq").html(tradacqhtm);
	// 已选项目绑定点击事件
	$("#tagAcq a").unbind().click(function() {
		var selval = $(this).attr('title');
		$("#tagList .selectedcolor").each(function() {
			if ($(this).attr('title') == selval) {
				$(this).removeClass('selectedcolor');
				copyTradItem();
			}
		});
	});
	// 清空
	$("#tagEmpty").unbind().click(function() {
		$("#tagAcq").html("");
		$("#tagList .selectedcolor").each(function() {
			$(this).removeClass('selectedcolor');
		});
	});
}
/*
 * 74cms 职位搜索页面 职位内容的填充
|   @param: fillID      -- 填入的ID
*/
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
			 		jobstr += '<p><font><a rcoid="'+sjobs[0]+'" pid="'+jobs[0]+'.'+sjobs[0]+'" title="'+sjobs[1]+'" href="javascript:;">'+sjobs[1]+'</a></font></p>';
			 		if(QS_jobs[sjobs[0]]) {
			 			jobstr += '<div class="subcate" style="display:none;">';
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
/*
 * 74cms 职位搜索页面 地区内容的填充
|   @param: fillID      -- 填入的ID
*/
function fillCity(fillID){
	var citystr = '';
	citystr += '<tr>';
	citystr += '<td><ul class="jobcatelist">';
	$.each(QS_city_parent, function(pindex, pval) {
		if(pval) {
			var citys = pval.split(",");
	 		citystr += '<li>';
	 		citystr += '<p><font><a rcoid="'+citys[0]+'" pid="'+citys[0]+'" title="'+citys[1]+'" href="javascript:;">'+citys[1]+'</a></font></p>';
	 		if(QS_city[citys[0]]) {
	 			citystr += '<div class="subcate" style="display:none;">';
	 			var ccitysArray = QS_city[citys[0]].split("|");
	 			citystr += '<a p="qb" href="javascript:;">不限</a>';
		 		$.each(ccitysArray, function(cindex, cval) {
		 			if(cval) {
			 			var ccitys = cval.split(",");
			 			citystr += '<a rcoid="'+ccitys[0]+'" title="'+ccitys[1]+'" pid="'+citys[0]+'.'+ccitys[0]+'" href="javascript:;">'+ccitys[1]+'</a>';
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
/*
 * 74cms 职位搜索页面 拷贝地区已选
*/
function copyCityItem() {
	var cityacqhtm = '';
	$("#divCityCate .selectedcolor").each(function() {
		cityacqhtm += '<a pid="'+$(this).attr('pid')+'" href="javascript:;" title="'+$(this).attr('title')+'"><div class="text">'+$(this).attr('title')+'</div><div class="close"></div></a>';
	});
	$("#cityAcq").html(cityacqhtm);
	// 已选项目绑定点击事件
	$("#cityAcq a").unbind().click(function() {
		var selval = $(this).attr('title');
		$("#divCityCate .selectedcolor").each(function() {
			if ($(this).attr('title') == selval) {
				$(this).removeClass('selectedcolor');
				copyCityItem();
			}
		});
	});
	// 清空
	$("#cityEmpty").unbind().click(function() {
		$("#cityAcq").html("");
		$("#divCityCate .selectedcolor").each(function() {
			$(this).removeClass('selectedcolor');
		});
	});
}
/*
 * 74cms 职位搜索页面 拷贝职位已选
*/
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