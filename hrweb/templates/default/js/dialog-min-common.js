/* 此js 中方法需要引用 dialog-min.js*/
/*
  delete_dialog  删除提示弹出框 
  @ className  触发事件的 class 名 .ctrl-del 
    单独删除 包含url 属性 删除链接
    批量删除 包含act 属性 表单提交地址
  @ form   批量删除提交表单的,表单id #form1
*/
function delete_dialog(className,form)
{
   $(''+className+'').on('click', function(e)
   {
    var url=$(this).attr('url');
    var act=$(this).attr('act');
    var myDialog=dialog();
    myDialog.title('系统提示');
    myDialog.content('<div class="del-dialog"><div class="tip-block"><span class="del-tips-text">删除后无法恢复，您确定要删除吗？</span></div></div><div class="center-btn-wrap"><input type="button" value="确定" class="btn-65-30blue btn-big-font DialogSubmit" /><input type="button" value="取消" class="btn-65-30grey btn-big-font DialogClose" /></div>');
    myDialog.width('300');
    myDialog.showModal();
    /* 关闭 */
    $(".DialogClose").live('click',function() {
      myDialog.close().remove();
    });
    // 确定
    $(".DialogSubmit").click(function() 
    {
      if(url)
      {
        window.location.href=url;
      }else{
        $(""+form+"").attr("action",act);
        $(""+form+"").submit();
      }
    });
  });
}
/*
  inviteJob_dialog 面试邀请弹出框
  @className  触发事件的 class 包含 resume_id属性 为简历 id
  @url 请求ajax 的php 地址
  @utype 会员类型
*/
function inviteJob_dialog(className,url)
{
  $(''+className+'').live('click', function(){
    var id=$(this).attr("resume_id");
    var tsTimeStamp= new Date().getTime();
    var url_=url+"?id="+id+"&act=invited&t="+tsTimeStamp;
    var myDialog = dialog();
    myDialog.title('邀请面试');
    myDialog.content("加载中...");
    myDialog.width('500');
    myDialog.showModal();
    jQuery.ajax({
        url: url_,
        success: function (data) {
            myDialog.content(data);
            /* 关闭 */
            $(".DialogClose").live('click',function() {
              myDialog.close().remove();
            });
            /* 邀请操作 */
            $(".DialogSubmit").click(function() 
            {
              var jobsid= $("#jobsid").val();
              if(jobsid=="")
              {
                dialog({
                    title: '系统提示',
                    content: "未选择职位！",
                    width:300
                }).show();
              }
              var pms_notice=$("#pms_notice").attr("checked");
              if(pms_notice) pms_notice=1;else pms_notice=0;
              var sms_notice=$("#sms_notice").attr("checked");
              if(sms_notice) sms_notice=1;else sms_notice=0;
              var interview_time =$("#interview_time").val();
              var notes = $("#notes").val();
              if(interview_time=="")
              {
                dialog({
                    title: '系统提示',
                    content: "面试时间不能为空！",
                    width:300
                }).show();
              }
              var current_time = Date.parse(new Date());
              var in_time = Date.parse(new Date(interview_time));
              if(in_time<current_time)
              {
                dialog({
                    title: '系统提示',
                    content: "面试时间不能小于当前时间！",
                    width:300
                }).show();
              }
              // 保存 申请
              if(jobsid && interview_time && (in_time>=current_time))
              {
                myDialog.content("正在提交...");
                $.get(url, {"jobs_id": jobsid,"id":id,"notes":notes,"pms_notice":pms_notice,"sms_notice":sms_notice,"interview_time":interview_time,"time":tsTimeStamp,"act":"invited_save"},
                function (rdata)
                {
                  myDialog.content(rdata);
                });
              }
            });
        }
    });
  });
}
/*
  companySendToEmail_dialog  企业会员中心 简历发送到邮箱
  @className  触发事件的 class 包含 属性 resume_id为简历id,uid为简历uid
  @url 发送邮件php地址
*/
function companySendToEmail_dialog(className,url)
{
  $(''+className+'').on('click', function(){
      var resume_id =$(this).attr("resume_id");
      var uid=$(this).attr('uid');
      var myDialog = dialog();
      myDialog.content('<div class="send-mail-dialog dialog-block"><div class="dialog-item clearfix"><div class="d-type f-left">电子邮箱：</div><div class="d-content f-left"><input type="text" class="edit-text" placeholder="只能输入一个邮箱地址" id="SendToEmail"/></div></div><div class="dialog-item clearfix"><div class="d-type f-left">&nbsp;</div><div class="d-content f-left"><input type="button" value="发送" class="btn-65-30blue btn-big-font DialogSubmit" /><input type="button" value="取消" class="btn-65-30grey btn-big-font DialogClose" /></div></div></div>');
      myDialog.title('发送到邮箱');
      myDialog.width('350');
      myDialog.showModal();
      $(".DialogClose").live('click',function() {
        myDialog.close().remove();
      });
      $(".DialogSubmit").click(function() 
      {
        var email= $("#SendToEmail").val();
        var reg = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
        isok= reg.test(email);
        if(isok)
        {
          myDialog.content("正在发送中..");  
          $.get(url+'?act=sendtoemail&uid='+uid+'&resume_id='+resume_id+'&email='+email+'', function(data) {
            if(data==1)
            {
              myDialog.content("该简历已经发送到您的邮箱！");
            }
            else
            {
              myDialog.content(data);
            }
          });
        }
        else
        {
          dialog({
                    title: '系统提示',
                    content: "请输入正确的邮箱！",
                    width:300
                }).show();
        }
      });
    })
}
/*
  企业推广 弹出框
*/
function set_promotion_dialog(className)
{
  $(''+className+'').on('click', function(){
    var catid = $(this).attr("catid");
    var jobid = $(this).attr("jobid");
    var url="company_ajax.php?act=set_promotion&catid="+catid+"&jobid="+jobid;
    var myDialog = dialog();
    myDialog.title('职位推广');
    myDialog.content("加载中...");
    myDialog.width('490');
    myDialog.showModal();
    jQuery.ajax({
        url: url,
        success: function (data) {
            myDialog.content(data);
            /* 关闭 */
            $(".DialogClose").live('click',function() {
              myDialog.close().remove();
            });
            /* 邀请操作 */
            $(".DialogSubmit").click(function() 
            {
              $(this).val("提交中..");
              $(this).attr("disabled","1");
              var jobid = $("#jobid").val();
              var catid = $("#catid").val();
              var days = $("#days").val();
              var pro_name = $("#pro_name").val();
              var val = $("#val").val();
              $.post("company_ajax.php?act=promotion_add_save",{jobid:jobid,catid:catid,days:days,val:val,pro_name:pro_name},
                function(result)
                {
                  myDialog.content(result);
                });
            });
        }
    });
  })
}
/*
  企业下载联系方式 弹出框
*/
function downResume_dialog(className,url,resume_id)
{
  $(''+className+'').on('click', function(){
    var id=resume_id;
    var tsTimeStamp= new Date().getTime();
    var url_=url+"?id="+id+"&act=download&t="+tsTimeStamp;
    var myDialog = dialog();
    myDialog.title('下载联系方式');
    myDialog.content("加载中...");
    myDialog.width('475');
    myDialog.showModal();
    $.get(url_, function(data){
        myDialog.content(data);
        /* 关闭 */
        $(".DialogClose").live('click',function() {
          myDialog.close().remove();
        });
    });
  });
}
/* 收藏简历 弹出*/
function favoritesResume_dialog(className,url)
{
  $(''+className+'').on('click', function(){
    var myDialog = dialog();
    myDialog.title('加入人才库');
    myDialog.content("加载中...");
    myDialog.width('500');
    myDialog.showModal();
    $.get(url, function(data){
        myDialog.content(data);
        /* 关闭 */
        $(".DialogClose").live('click',function() {
          myDialog.close().remove();
        });
    });
  });
}
/* 举报简历 弹出*/
function reportResume_dialog(className,url)
{
  $(''+className+'').on('click', function(){
    var resume_id = $(this).attr("resume_id");
    var fullname = $(this).attr("fullname");
    var resume_addtime = $(this).attr("resume_addtime");
    var url_=url+"?act=report&resume_id="+resume_id+"&full_name="+fullname+"&resume_addtime="+resume_addtime;
    var myDialog = dialog();
    myDialog.title('举报简历');
    myDialog.content("加载中...");
    myDialog.width('500');
    myDialog.showModal();
    $.get(url_, function(data){
        myDialog.content(data);
        /* 关闭 */
        $(".DialogClose").live('click',function() {
          myDialog.close().remove();
        });
    });
  });
}

/*
  个人申请职位 
*/
function applyJob_dialog(className,url)
{
    $(''+className+'').on('click', function(){
      var jobs_id = $(this).attr("jobs_id");
      var url_=url+"?id="+jobs_id+"&act=app";
      var myDialog = dialog();
      myDialog.title('申请职位');
      myDialog.content("加载中...");
      myDialog.width('500');
      myDialog.showModal();
      $.get(url_, function(data){
          myDialog.content(data);
          /* 关闭 */
          $(".DialogClose").live('click',function() {
            myDialog.close().remove();
          });
      });
    });
}
/*
  个人 收藏职位  
*/
function favoritesJob_dialog(className,url)
{
  $(''+className+'').on('click', function(){
    var myDialog = dialog();
    $.get(url, function(data){
        myDialog.content(data);
        myDialog.title('加入收藏');
        myDialog.width('500');
        myDialog.showModal();
        /* 关闭 */
        $(".DialogClose").live('click',function() {
          myDialog.close().remove();
        });
    });
  });
}
/*
  个人 举报职位 
*/
function reportJob_dialog(className,url)
{
  $(''+className+'').on('click', function(){
    var jobs_id = $(this).attr("jobs_id");
    var jobs_name = $(this).attr("jobs_name");
    var jobs_addtime = $(this).attr("jobs_addtime");
    var url_=url+'?act=report&jobs_id='+jobs_id+'&jobs_name='+jobs_name+'&jobs_addtime='+jobs_addtime;
    var myDialog = dialog();
    myDialog.title('举报职位');
    myDialog.content("加载中...");
    myDialog.width('500');
    myDialog.showModal();
    $.get(url_, function(data){
        myDialog.content(data);
        /* 关闭 */
        $(".DialogClose").live('click',function() {
          myDialog.close().remove();
        });
    });
  });
}
/*
  邮箱验证 弹出框
*/
function auditEmail_dialog(className,url)
{
  $(''+className+'').on('click', function(){
    var myDialog = dialog();
    myDialog.title('邮箱验证');
    myDialog.content("加载中...");
    myDialog.width('500');
    myDialog.showModal();
    $.get(url, function(data){
        myDialog.content(data);
        /* 关闭 */
        $(".DialogClose").live('click',function() {
          myDialog.close().remove();
        });
    });
  });
}
// 验证 手机弹出 
function auditMobile_dialog(className,url)
{
  $(''+className+'').on('click', function(){
    var myDialog = dialog();
    myDialog.title('手机验证');
    myDialog.content("加载中...");
    myDialog.width('500');
    myDialog.showModal();
    $.get(url, function(data){
        myDialog.content(data);
        /* 关闭 */
        $(".DialogClose").live('click',function() {
          myDialog.close().remove();
        });

    });
  });
}
/* 修改手机 验证身份 */
function editMobile_dialog(className,url)
{
  $(''+className+'').on('click', function(){
    var myDialog = dialog();
    myDialog.title('验证身份');
    myDialog.content("加载中...");
    myDialog.width('500');
    myDialog.showModal();
    var ajax_url_ = url.split(".");
    var ajax_url =ajax_url_[0];
    $.get(url, function(data){
        myDialog.content(data);
        /* 关闭 */
        $(".DialogClose").live('click',function() {
          myDialog.close().remove();
        });
        // 验证操作
        $("#postverify").live('click', function(){
          if($("#mobile_verifycode").val()=='')
          {
            dialog({
              title: '系统提示',
              content: '请填写效验码',
              width:'300px'
            }).showModal();
          }
          else
          {
            $.post("../../plus/ajax_verify_old_mobile.php", {"verifycode": $("#mobile_verifycode").val(),"send_key": $("#send_key").val(),"time":new Date().getTime(),"act":"verify_code"},
            function (data_v,textStatus)
            {
              if (data_v=="success")
              { 
                  myDialog.close().remove();
                  
                  var d = dialog();
                  d.title('身份验证');
                  d.content("加载中...");
                  d.width('500');
                  d.showModal();
                  $.get(ajax_url+'.php?act=edit_mobile', function(rst){
                  d.content(rst);
                });
              }
              else
              {
                dialog({
                title: '系统提示',
                content: '验证码填写错误',
                width:'300px'
                }).showModal();
              }
            })   
          }
        });
    });
  });
}
// 招聘会 在线预订 弹框
function boothjobfair_dialog(className,url,id)
{
  $(''+className+'').on('click', function(){
    if(!id)
    {
      var jobfair_id = $(this).attr("jobfair_id");
      var url_=url+'?act=booth&id='+jobfair_id;
    }
    else
    {
      var url_=url+'?act=booth&id='+id;
    }
    var myDialog = dialog();
    $.get(url_, function(data){
        myDialog.content(data);
        myDialog.title('温馨提示');
        myDialog.width('400');
        myDialog.showModal();
        /* 关闭 */
        $(".DialogClose").live('click',function() {
          myDialog.close().remove();
        });
    });
  });
}
// 招聘会 地图定位 弹框
function jobfairmap_dialog(className,url)
{
  $(''+className+'').on('click', function(){
    var map_x = $(this).attr("map_x");
    var map_y = $(this).attr("map_y"); 
    var map_zoom = $(this).attr("map_zoom"); 
    var title = $(this).attr("title_"); 
    var address = $(this).attr("address"); 
    var url_=url+"&map_x="+map_x+"&map_y="+map_y+"&map_zoom="+map_zoom+"&companyname="+title+"&address="+address;
    var myDialog = dialog();
    $.get(url_, function(data){
        myDialog.content(data);
        myDialog.title('地图定位');
        myDialog.width('660');
        myDialog.showModal();
        /* 关闭 */
        $(".ui-dialog-close").live('click',function() {
          myDialog.close().remove();
          window.location.reload();
        });
    });
  });
}