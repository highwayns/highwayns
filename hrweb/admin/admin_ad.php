<?php
 /*
 * 74cms 广告管理
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
define('IN_QISHI', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
require_once(ADMIN_ROOT_PATH.'include/admin_ad_fun.php');
require_once(ADMIN_ROOT_PATH.'include/upload.php');
$ads_updir="../data/comads/";
$ads_dir=$_CFG['site_dir']."data/comads/";
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'list';
if($act == 'list')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"ad_show");
	require_once(QISHI_ROOT_PATH.'include/page.class.php');
	$key=isset($_GET['key'])?trim($_GET['key']):"";
	$key_type=isset($_GET['key_type'])?intval($_GET['key_type']):"";
	if ($key && $key_type>0)
	{
		
		if     ($key_type===1)$wheresql=" WHERE a.title like '%{$key}%'";
	}
	else
	{
		$category_id=isset($_GET['category_id'])?intval($_GET['category_id']):"";
		if ($category_id>0)
		{
		$wheresql=empty($wheresql)?" WHERE a.category_id= ".$category_id:$wheresql." AND a.category_id= ".$category_id;
		}
		$settr=$_GET['settr'];
		if ($settr<>"")
		{
			$wheresql.=empty($wheresql)?" WHERE ":" AND  ";
			$days=intval($settr);
			$settr=strtotime($days." day");
			if ($days===0)
			{
			$wheresql.=" a.deadline< ".time()." AND a.deadline>0 ";
			}
			else
			{
			$wheresql.=" a.deadline< ".$settr." AND  a.deadline>".time()." ";
			}		
		}
		$is_display=isset($_GET['is_display'])?$_GET['is_display']:"";
		if ($is_display<>'')
		{
		$is_display=intval($is_display);
		$wheresql=empty($wheresql)?" WHERE a.is_display= ".$is_display:$wheresql." AND a.is_display= ".$is_display;
		}
	}
	$joinsql=" LEFT JOIN  ".table('ad_category')." AS c ON  a.category_id=c.id ";
	$total_sql="SELECT COUNT(*) AS num FROM ".table('ad')." AS a " .$joinsql.$wheresql;
	$total_val=$db->get_total($total_sql);
	$page = new page(array('total'=>$total_val, 'perpage'=>$perpage,'getarray'=>$_GET));
	$currenpage=$page->nowindex;
	$offset=($currenpage-1)*$perpage;
	$smarty->assign('list',get_ad_list($offset,$perpage,$joinsql.$wheresql));
	$smarty->assign('ad_category',get_ad_category());
	$smarty->assign('page',$page->show(3));
	$smarty->assign('total',$total_val);
	$smarty->assign('pageheader',"广告管理");	
	$smarty->display('ads/admin_ad_list.htm');
}
//添加广告
elseif($act == 'ad_add')
{
	check_permissions($_SESSION['admin_purview'],"ad_add");
	$smarty->assign('datefm',convert_datefm(time(),1));
	$smarty->assign('ad_category',get_ad_category());
	$smarty->assign('pageheader',"广告管理");
	get_token();
	$smarty->display('ads/admin_ad_add.htm');
}
//保存添加广告
elseif($act == 'ad_add_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"ad_add");
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('您没有填写标题！',1);
	$setsqlarr['is_display']=trim($_POST['is_display'])?trim($_POST['is_display']):0;
	$setsqlarr['category_id']=trim($_POST['category_id'])?trim($_POST['category_id']):adminmsg('您没有填写广告分类！',1);
	$setsqlarr['type_id']=trim($_POST['type_id'])?trim($_POST['type_id']):adminmsg('您没有填写广告类型！',1);
	$setsqlarr['alias']=trim($_POST['alias'])?trim($_POST['alias']):adminmsg('参数错误，调用ID不存在！',1);
	$setsqlarr['show_order']=intval($_POST['show_order']);
	$setsqlarr['note']=trim($_POST['note']);	
		if ($_POST['starttime']=="")
		{
		$setsqlarr['starttime']=0;
		}
		else
		{
		$setsqlarr['starttime']=intval(convert_datefm($_POST['starttime'],2));
		}
		if ($_POST['deadline']=="")
		{
		$setsqlarr['deadline']=0;
		}
		else
		{
		$setsqlarr['deadline']=intval(convert_datefm($_POST['deadline'],2));
		}
	//文字
	if ($setsqlarr['type_id']=="1")
	{
	$setsqlarr['text_content']=trim($_POST['text_content'])?trim($_POST['text_content']):adminmsg('您没有填写文字内容！',1);
	$setsqlarr['text_url']=trim($_POST['text_url']);
	$setsqlarr['text_color']=trim($_POST['tit_color']);
	}
	//图片
	elseif ($setsqlarr['type_id']=="2")
	{
		if (empty($_FILES['img_file']['name']) && empty($_POST['img_path']))
		{
		adminmsg('请上传图片或者填写图片路径！',1);
		}
		if ($_FILES['img_file']['name'])
		{
			$ads_updir=$ads_updir.date("Y/m/d/");
			make_dir($ads_updir);
			$setsqlarr['img_path']=_asUpFiles($ads_updir,"img_file",1000,'gif/jpg/bmp/png',true);
			if (empty($setsqlarr['img_path']))
			{
			adminmsg('上传文件失败！',1);
			}
			$setsqlarr['img_path']=$ads_dir.date("Y/m/d/").$setsqlarr['img_path'];
		}
		else
		{
			$setsqlarr['img_path']=trim($_POST['img_path']);
		}
	$setsqlarr['img_url']=trim($_POST['img_url']);
	$setsqlarr['img_explain']=trim($_POST['img_explain']);
	$setsqlarr['img_uid']=intval($_POST['img_uid']);
	}
	//代码
	elseif ($setsqlarr['type_id']=="3")
	{
	$setsqlarr['code_content']=trim($_POST['code_content'])?trim($_POST['code_content']):adminmsg('您没有填写代码！',1);
	}
	//FLASH
	elseif ($setsqlarr['type_id']=="4")
	{
	$setsqlarr['flash_width']=!empty($_POST['flash_width'])?intval($_POST['flash_width']):adminmsg('您没有填写flash宽度！',1);
	$setsqlarr['flash_height']=!empty($_POST['flash_height'])?intval($_POST['flash_height']):adminmsg('您没有填写flash高度！',1);
		if (empty($_FILES['flash_file']['name']) && empty($_POST['flash_path']))
			{
			adminmsg('请上传FLASH或者填写FLASH路径！',1);
			}
			if ($_FILES['flash_file']['name'])
			{
				$ads_updir=$ads_updir.date("Y/m/d/");
				make_dir($ads_updir);
				$setsqlarr['flash_path']=_asUpFiles($ads_updir,"flash_file",1000,'swf/SWF',true);
				if (empty($setsqlarr['flash_path']))
				{
				adminmsg('上传文件失败！',1);
				}
				$setsqlarr['flash_path']=$ads_dir.date("Y/m/d/").$setsqlarr['flash_path'];
			}
			else
			{
				$setsqlarr['flash_path']=trim($_POST['flash_path']);
			}
	}
	//对联
	elseif ($setsqlarr['type_id']=="5")
	{
	$setsqlarr['floating_type']=$_POST['floating_type']?trim($_POST['floating_type']):1;	
	$setsqlarr['floating_url']=trim($_POST['floating_url']);
	$setsqlarr['floating_width']=$_POST['floating_width']?intval($_POST['floating_width']):adminmsg('您没有填写宽度！',1);
	$setsqlarr['floating_height']=$_POST['floating_height']?intval($_POST['floating_height']):adminmsg('您没有填写高度！',1);
	$setsqlarr['floating_left']=$_POST['floating_left']<>""?intval($_POST['floating_left']):"";
	$setsqlarr['floating_right']=$_POST['floating_right']<>""?intval($_POST['floating_right']):"";
	if ($setsqlarr['floating_left']==="" && $setsqlarr['floating_right']==="") adminmsg('左边距和右边距至少填写一项！',1);
	$setsqlarr['floating_top']=$_POST['floating_top']?intval($_POST['floating_top']):0;
		if (empty($_FILES['floating_file']['name']) && empty($_POST['floating_path']))
		{
		adminmsg('请上传文件或者填写路径！',1);
		}
		if ($_FILES['floating_file']['name'])
		{
			if ($setsqlarr['floating_type']==1)
			{
			$filetype="gif/jpg/bmp/png";
			}
			else
			{
			$filetype="swf";
			}
			$ads_updir=$ads_updir.date("Y/m/d/");
			make_dir($ads_updir);
			$setsqlarr['floating_path']=_asUpFiles($ads_updir,"floating_file",1000,$filetype,true);
			if (empty($setsqlarr['floating_path']))
			{
			adminmsg('上传文件失败！',1);
			}
			$setsqlarr['floating_path']=$ads_dir.date("Y/m/d/").$setsqlarr['floating_path'];
		}
		else
		{
			$setsqlarr['floating_path']=trim($_POST['floating_path']);
		}
	}
	//视频
	elseif ($setsqlarr['type_id']=="6")
	{
	$setsqlarr['video_width']=$_POST['video_width']?intval($_POST['video_width']):adminmsg('您没有填写宽度！',1);
	$setsqlarr['video_height']=$_POST['video_height']?intval($_POST['video_height']):adminmsg('您没有填写高度！',1);
		if (empty($_FILES['video_file']['name']) && empty($_POST['video_path']))
		{
		adminmsg('请上传文件或者填写路径！',1);
		}
		if ($_FILES['video_file']['name'])
		{
			$ads_updir=$ads_updir.date("Y/m/d/");
			make_dir($ads_updir);
			$setsqlarr['video_path']=_asUpFiles($ads_updir,"video_file",5000,"swf/flv/f4v",true);
			if (empty($setsqlarr['video_path']))
			{
			adminmsg('上传文件失败！',1);
			}
			$setsqlarr['video_path']=$ads_dir.date("Y/m/d/").$setsqlarr['video_path'];
		}
		else
		{
			$setsqlarr['video_path']=trim($_POST['video_path']);
		}
	}
	$setsqlarr['addtime']=$timestamp;
	$link[0]['text'] = "继续添加";
	$link[0]['href'] ="?act=ad_add&category_id=".$_POST['category_id']."&type_id=".$_POST['type_id']."&alias=".$_POST['alias'];
	$link[1]['text'] = "返回广告列表";
	$link[1]['href'] ="?act=";
	if(!$db->inserttable(table('ad'),$setsqlarr))
	{
		//填写管理员日志
		write_log("后台添加广告失败", $_SESSION['admin_name'],3);
		adminmsg("添加失败！",0);
	}
	else
	{
		//填写管理员日志
		write_log("后台成功添加广告", $_SESSION['admin_name'],3);
		adminmsg("添加成功！",2,$link);
	}
}
//修改广告
elseif($act == 'edit_ad')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"ad_edit");
	$id=!empty($_GET['id'])?intval($_GET['id']):adminmsg('没有广告id！',1);
	$ad=get_ad_one($id);
	$smarty->assign('ad',$ad);
	$smarty->assign('ad_category',get_ad_category());//广告位分类列表
	$smarty->assign('url',$_SERVER['HTTP_REFERER']);
	$smarty->assign('pageheader',"广告管理");
	$smarty->display('ads/admin_ad_edit.htm');
	 
}
//保存:修改广告
elseif($act == 'ad_edit_save')
{
	check_token();
	check_permissions($_SESSION['admin_purview'],"ad_edit");
	$setsqlarr['title']=trim($_POST['title'])?trim($_POST['title']):adminmsg('您没有填写标题！',1);
	$setsqlarr['is_display']=trim($_POST['is_display'])?trim($_POST['is_display']):0;
	$setsqlarr['category_id']=trim($_POST['category_id'])?trim($_POST['category_id']):adminmsg('您没有填写广告分类！',1);
	$setsqlarr['type_id']=trim($_POST['type_id'])?trim($_POST['type_id']):adminmsg('您没有填写广告类型！',1);
	$setsqlarr['alias']=trim($_POST['alias'])?trim($_POST['alias']):adminmsg('参数错误，调用ID不存在！',1);
	$setsqlarr['show_order']=intval($_POST['show_order']);
	$setsqlarr['note']=trim($_POST['note']);	
		if ($_POST['starttime']=="")
		{
		$setsqlarr['starttime']=0;
		}
		else
		{
		$setsqlarr['starttime']=intval(convert_datefm($_POST['starttime'],2));
		}
		if ($_POST['deadline']=="")
		{
		$setsqlarr['deadline']=0;
		}
		else
		{
		$setsqlarr['deadline']=intval(convert_datefm($_POST['deadline'],2));
		}
	//文字
	if ($setsqlarr['type_id']=="1")
	{
	$setsqlarr['text_content']=trim($_POST['text_content'])?trim($_POST['text_content']):adminmsg('您没有填写文字内容！',1);
	$setsqlarr['text_url']=trim($_POST['text_url']);
	$setsqlarr['text_color']=trim($_POST['tit_color']);
	}
	//图片
	elseif ($setsqlarr['type_id']=="2")
	{
		if (empty($_FILES['img_file']['name']) && empty($_POST['img_path']))
		{
		adminmsg('请上传图片或者填写图片路径！',1);
		}
		if ($_FILES['img_file']['name'])
		{
			$ads_updir=$ads_updir.date("Y/m/d/");
			make_dir($ads_updir);
			$setsqlarr['img_path']=_asUpFiles($ads_updir,"img_file",1000,'gif/jpg/bmp/png',true);
			if (empty($setsqlarr['img_path']))
			{
			adminmsg('上传文件失败！',1);
			}
			$setsqlarr['img_path']=$ads_dir.date("Y/m/d/").$setsqlarr['img_path'];
		}
		else
		{
			$setsqlarr['img_path']=trim($_POST['img_path']);
		}
	$setsqlarr['img_url']=trim($_POST['img_url']);
	$setsqlarr['img_explain']=trim($_POST['img_explain']);
	$setsqlarr['img_uid']=intval($_POST['img_uid']);
	}
	//代码
	elseif ($setsqlarr['type_id']=="3")
	{
	$setsqlarr['code_content']=trim($_POST['code_content'])?trim($_POST['code_content']):adminmsg('您没有填写代码！',1);
	}
	//FLASH
	elseif ($setsqlarr['type_id']=="4")
	{
	$setsqlarr['flash_width']=!empty($_POST['flash_width'])?intval($_POST['flash_width']):adminmsg('您没有填写flash宽度！',1);
	$setsqlarr['flash_height']=!empty($_POST['flash_height'])?intval($_POST['flash_height']):adminmsg('您没有填写flash高度！',1);
		if (empty($_FILES['flash_file']['name']) && empty($_POST['flash_path']))
			{
			adminmsg('请上传FLASH或者填写FLASH路径！',1);
			}
			if ($_FILES['flash_file']['name'])
			{
				$ads_updir=$ads_updir.date("Y/m/d/");
				make_dir($ads_updir);
				$setsqlarr['flash_path']=_asUpFiles($ads_updir,"flash_file",1000,'swf/SWF',true);
				if (empty($setsqlarr['flash_path']))
				{
				adminmsg('上传文件失败！',1);
				}
				$setsqlarr['flash_path']=$ads_dir.date("Y/m/d/").$setsqlarr['flash_path'];
			}
			else
			{
				$setsqlarr['flash_path']=trim($_POST['flash_path']);
			}
	}
	//对联
	elseif ($setsqlarr['type_id']=="5")
	{
	$setsqlarr['floating_type']=$_POST['floating_type']?trim($_POST['floating_type']):1;	
	$setsqlarr['floating_url']=trim($_POST['floating_url']);
	$setsqlarr['floating_width']=$_POST['floating_width']?intval($_POST['floating_width']):adminmsg('您没有填写宽度！',1);
	$setsqlarr['floating_height']=$_POST['floating_height']?intval($_POST['floating_height']):adminmsg('您没有填写高度！',1);
	$setsqlarr['floating_left']=$_POST['floating_left']<>""?intval($_POST['floating_left']):"";
	$setsqlarr['floating_right']=$_POST['floating_right']<>""?intval($_POST['floating_right']):"";
	if ($setsqlarr['floating_left']==="" && $setsqlarr['floating_right']==="") adminmsg('左边距和右边距至少填写一项！',1);
	$setsqlarr['floating_top']=$_POST['floating_top']?intval($_POST['floating_top']):0;
		if (empty($_FILES['floating_file']['name']) && empty($_POST['floating_path']))
		{
		adminmsg('请上传文件或者填写路径！',1);
		}
		if ($_FILES['floating_file']['name'])
		{
			if ($setsqlarr['floating_type']==1)
			{
			$filetype="gif/jpg/bmp/png";
			}
			else
			{
			$filetype="swf";
			}
			$ads_updir=$ads_updir.date("Y/m/d/");
			make_dir($ads_updir);
			$setsqlarr['floating_path']=_asUpFiles($ads_updir,"floating_file",1000,$filetype,true);
			if (empty($setsqlarr['floating_path']))
			{
			adminmsg('上传文件失败！',1);
			}
			$setsqlarr['floating_path']=$ads_dir.date("Y/m/d/").$setsqlarr['floating_path'];
		}
		else
		{
			$setsqlarr['floating_path']=trim($_POST['floating_path']);
		}
	}
	//视频
	elseif ($setsqlarr['type_id']=="6")
	{
	$setsqlarr['video_width']=$_POST['video_width']?intval($_POST['video_width']):adminmsg('您没有填写宽度！',1);
	$setsqlarr['video_height']=$_POST['video_height']?intval($_POST['video_height']):adminmsg('您没有填写高度！',1);
		if (empty($_FILES['video_file']['name']) && empty($_POST['video_path']))
		{
		adminmsg('请上传文件或者填写路径！',1);
		}
		if ($_FILES['video_file']['name'])
		{
			$ads_updir=$ads_updir.date("Y/m/d/");
			make_dir($ads_updir);
			$setsqlarr['video_path']=_asUpFiles($ads_updir,"video_file",5000,"swf/flv/f4v",true);
			if (empty($setsqlarr['video_path']))
			{
			adminmsg('上传文件失败！',1);
			}
			$setsqlarr['video_path']=$ads_dir.date("Y/m/d/").$setsqlarr['video_path'];
		}
		else
		{
			$setsqlarr['video_path']=trim($_POST['video_path']);
		}
	}
	$setsqlarr['addtime']=$timestamp;
	$link[0]['text'] = "返回列表";
	$link[0]['href'] =trim($_POST['url']);
	$wheresql=" id='".intval($_POST['id'])."' "; 
	if(!$db->updatetable(table('ad'),$setsqlarr,$wheresql))
	{
		//填写管理员日志
		write_log("后台修改广告失败", $_SESSION['admin_name'],3);
		adminmsg("修改失败！",0);
	}
	else
	{
		//填写管理员日志
		write_log("后台修改广告成功", $_SESSION['admin_name'],3);
		adminmsg("修改成功！",2,$link);
	}
	
}
//删除广告
elseif($act=='del_ad')
{
	check_permissions($_SESSION['admin_purview'],"ad_del");
	$id=$_REQUEST['id'];
	check_token();
	if (empty($id)) adminmsg("请选择项目！",0);
	if ($num=del_ad($id))
	{
	adminmsg("删除成功！共删除".$num."行",2);
	}
	else
	{
	adminmsg("删除失败！".$num,1);
	}
}
//广告位管理
elseif($act=='ad_category')
{
	check_permissions($_SESSION['admin_purview'],"ad_category");
	$smarty->assign('act',$act);//标签ID
	$smarty->assign('list',get_ad_category());
	$smarty->assign('pageheader',"广告管理");
	get_token();
	$smarty->display('ads/admin_ad_category.htm');
}
//添加广告位
elseif($act=='ad_category_add')
{
	get_token();
	check_permissions($_SESSION['admin_purview'],"ad_category");
	$smarty->assign('pageheader',"添加广告位");
	$smarty->display('ads/admin_ad_category_add.htm');
}
//保存添加广告位
elseif($act=='ad_category_add_save')
{
	check_permissions($_SESSION['admin_purview'],"ad_category");
	check_token();
	$link[0]['text'] = "返回上一页";
	$link[0]['href'] ="?act=ad_category";
	$setsqlarr['categoryname']=$_POST['categoryname']?trim($_POST['categoryname']):adminmsg('您没有广告位名称！',1);
	$setsqlarr['expense'] = intval($_POST['expense']);
	$setsqlarr['alias']=$_POST['alias']?trim($_POST['alias']):adminmsg('您没有填写调用名称！',1);
	substr($setsqlarr['alias'],0,3)=='QS_'?adminmsg('自定义广告位调用名称不允许 QS_ 开头！',1):'';
	ck_category_alias($setsqlarr['alias'])?adminmsg('调用名称已经存在，请换一个调用名称！',1):'';
	$setsqlarr['type_id']=$_POST['type_id']?intval($_POST['type_id']):adminmsg('您没有选择广告类型！',1);
	if(!$db->inserttable(table('ad_category'),$setsqlarr))
	{
		//填写管理员日志
		write_log("后台添加广告位失败", $_SESSION['admin_name'],3);
		adminmsg("添加失败！",0);
	}
	else
	{
		//填写管理员日志
		write_log("后台成功添加广告位", $_SESSION['admin_name'],3);
		adminmsg("添加成功！",2,$link);
	}
}
//修改广告位
elseif($act=='edit_ad_category')
{
	check_permissions($_SESSION['admin_purview'],"ad_category");
	$ad_category = get_ad_category_one($_GET['id']);
	if($ad_category['admin_set']=="1"){
		switch($ad_category['type_id']){
			case 1:
				$type = "文字";break;
			case 2:
				$type = "图片";break;
			case 3:
				$type = "代码";break;
			case 4:
				$type = "FLASH";break;
			case 5:
				$type = "浮动";break;
			case 6:
				$type = "视频";break;
			default:
				$type = "文字";break;
		}
		$smarty->assign('type',$type);
	}
	
	$smarty->assign('ad_category',get_ad_category_one($_GET['id']));
	$smarty->assign('pageheader',"广告管理");
	get_token();
	$smarty->display('ads/admin_ad_category_edit.htm');
}
//保存 修改的广告位
elseif($act=='ad_category_edit_save')
{
	check_permissions($_SESSION['admin_purview'],"ad_category");
	check_token();
	$link[0]['text'] = "返回广告位列表";
	$link[0]['href'] ="?act=ad_category";
	if(intval($_POST['admin_set'])!=1){
		$setsqlarr['categoryname']=trim($_POST['categoryname'])?trim($_POST['categoryname']):adminmsg('您没有广告位名称！',1);
		$setsqlarr['alias']=trim($_POST['alias'])?trim($_POST['alias']):adminmsg('您没有填写调用名称！',1);
		substr($setsqlarr['alias'],0,3)=='QS_'?adminmsg('自定义广告位调用名称不允许 QS_ 开头！',1):'';
		ck_category_alias($setsqlarr['alias'],$_POST['id'])?adminmsg('调用名称已经存在，请换一个调用名称！',1):'';
		$setsqlarr['type_id']=trim($_POST['type_id'])?trim($_POST['type_id']):adminmsg('您没有选择广告类型！',1);
	}
	$setsqlarr['expense'] = intval($_POST['expense']);
	$wheresql=" id='".intval($_POST['id'])."'";
		if ($db->updatetable(table('ad_category'),$setsqlarr,$wheresql))
		{
			if(intval($_POST['admin_set'])!=1){
				$adaliasarr['alias']=$setsqlarr['alias'];//同时修改此分类下所有广告的alias
				$wheresql=" category_id='".intval($_POST['id'])."'";
				$db->updatetable(table('ad'),$adaliasarr,$wheresql);
			}
			//填写管理员日志
			write_log("后台成功修改广告位", $_SESSION['admin_name'],3);
			adminmsg("修改成功！",2,$link);
		}
		else
		{
			adminmsg("修改失败！",0);
		}
}
//删除广告位
elseif($act=='del_ad_category')
{
	check_permissions($_SESSION['admin_purview'],"ad_category");
	check_token();
	$id=!empty($_GET['id'])?$_GET['id']:adminmsg("你没有选择广告位！",1);
		if ($id)
		{
			!del_ad_category($id)?adminmsg("删除失败！",0):adminmsg("删除成功！",2);
		}
}
elseif($act == 'management')
{	
	$id=intval($_GET['id']);
	$u=get_adv_user($id);
	if (!empty($u))
	{
		unset($_SESSION['uid']);
		unset($_SESSION['username']);
		unset($_SESSION['utype']);
		unset($_SESSION['uqqid']);
		setcookie("QS[uid]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		setcookie("QS[username]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		setcookie("QS[password]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		setcookie("QS[utype]","",time() - 3600,$QS_cookiepath, $QS_cookiedomain);
		unset($_SESSION['activate_username']);
		unset($_SESSION['activate_email']);
		
		$_SESSION['uid']=$u['uid'];
		$_SESSION['username']=$u['username'];
		$_SESSION['utype']=$u['utype'];
		$_SESSION['uqqid']="1";
		setcookie('QS[uid]',$u['uid'],0,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[username]',$u['username'],0,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[password]',$u['password'],0,$QS_cookiepath,$QS_cookiedomain);
		setcookie('QS[utype]',$u['utype'], 0,$QS_cookiepath,$QS_cookiedomain);
		header("Location:".get_member_url($u['utype']));
	}	
} 
?>