/** 首页左侧职位类别下拉 */
jQuery.dropDownWidget = function(obj) {
	var c = $(obj).find(".job-sort-control"), l = $(obj).find(".job-sort-list"), lhtml = '';
	if (QS_jobs_parent) {
		$.each(QS_jobs_parent, function(index, val) {
			var dataValArr = splitData(val, ",");
			lhtml += '<div class="js-items" code="'+dataValArr[0]+'"><div class="js-level2 clearfix"><i class="level-icon icon'+(index+1)+' f-left"></i><span class="f-left">'+dataValArr[1]+'</span><i class="sort-arrow f-right"></i></div></div>';
		});
		l.html(lhtml);
	};
	var j = l.find('.js-items'), cwidth = $(obj).outerWidth(), cheight = l.outerHeight() + 14;
	j.last().addClass("js-items-last");
	j.bind('mouseenter', function(event) {
		j.removeClass('js-items-nrb').removeClass('js-items-trb').addClass('js-items-rb');
		var subclass = $(this).attr("code");
		var html = '<div class="show">';
		if (MakeLi(subclass)) {
			html += MakeLi(subclass);
		}
		html += '</div>';
		$(this).addClass('js-items-nrb').next().addClass('js-items-trb');
		$(".leftmenu_box").empty();
		$(".leftmenu_box").append(html).css({"top":"0","left":cwidth,"display":"block","overflow":"auto","height":cheight});
	});
	c.bind('click', function(event) {
		l.toggle();
	});
    $(obj).bind('mouseleave', function(event) {
		l.hide();			
		$(".leftmenu_box").hide();
		j.removeClass('js-items-nrb').removeClass('js-items-trb').removeClass('js-items-rb');        
    });
    function splitData(dataval, character) {
    	if (dataval) {return dataval.split(character)};
    }
    function MakeLi(subclass) {
    	if (!QS_jobs[subclass]) {return "无"};
    	var liArray = QS_jobs[subclass].split("|");
		var htmlstr='';
		if (liArray) {
			for (x in liArray) {
				if (liArray[x]) {
					var v=liArray[x].split(",");
					thirdclass = v[0];
					htmlstr+='<div class="showbox"><div class="fl"><a target="_blank" title="'+v[1]+'" href="jobs/jobs-list.php?key=&jobcategory='+subclass+'.'+v[0]+'.0">'+v[1]+'</a></div>';
					if(thirdclass){
						htmlstr+='<ul class="fr">';
						htmlstr += Make_Third_Li(subclass, thirdclass);
						htmlstr+='</ul>';
					}
					htmlstr+='<div class="clear"></div></div>';
				}
			}
		}
		return htmlstr; 
	}
	function Make_Third_Li(subclass, thirdclass) {
		if (!QS_jobs[thirdclass]) {return "无"};
		var tArray = QS_jobs[thirdclass].split("|");
		var htmlstr1='';
		if (tArray) {
			for (x1 in tArray) {
				if (tArray[x1]) {
					var v1=tArray[x1].split(",");
					htmlstr1+='<li><a target="_blank" title="'+v1[1]+'" href="jobs/jobs-list.php?key=&jobcategory='+subclass+'.'+thirdclass+'.'+v1[0]+'">'+v1[1]+'</a></li>';
				}
			}
		}
		return htmlstr1; 
	}
};