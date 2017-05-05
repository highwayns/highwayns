<?php
define('IN_HIGHWAY', true);
require_once(dirname(__FILE__).'/../data/config.php');
require_once(dirname(__FILE__).'/include/admin_common.inc.php');
$act = !empty($_REQUEST['act']) ? trim($_REQUEST['act']) : 'xmllist';
$smarty->assign('act',$act);
$smarty->assign('pageheader',"Baiduプレートフォーム");
if($act == 'xmllist')
{
$xmlset=get_cache('baiduxml');
$flist=array();
$xmldir = '../'.$xmlset['xmldir'];
$trimxmldir=ltrim($xmldir,'../');
$trimxmldir=ltrim($trimxmldir,'..\\');
$flist[] =$xmlset['indexname'];
$opendir=opendir($xmldir);
while($file = readdir($opendir))
{
	if(strpos($file,'.xml')!==false && $file!==$xmlset['indexname'])
	{
	$flist[] = $file;
	}
}
foreach($flist as $key => $file)
{
	if (file_exists($xmldir.$file))
	{
	$flistd[$key]['file_type'] = $file==$xmlset['indexname']?'<span style="color:#FF6600">Indexファイル</span>':'資源ファイル';
	$flistd[$key]['file_size'] = round(filesize($xmldir.$file)/1024/1024,2);
	$flistd[$key]['file_time'] = filemtime($xmldir.$file);	
	$flistd[$key]['file_url'] = $_CFG['site_domain'].$_CFG['site_dir'].$trimxmldir.$file;
	$flistd[$key]['file_name']  = $file;
	}
}
$smarty->assign('flist',$flistd);
$smarty->display('baiduxml/admin_baiduxml_li.htm');
}
elseif($act == 'set')
{
$smarty->assign('xml',get_cache('baiduxml'));
$smarty->display('baiduxml/admin_baiduxml_set.htm');
}
elseif($act == 'setsave')
{
		$_POST['xmlmax']=intval($_POST['xmlmax']);
		$_POST['xmlpagesize']=intval($_POST['xmlpagesize'])==0?1:intval($_POST['xmlpagesize']);
		foreach($_POST as $k => $v)
		{
		!$db->query("UPDATE ".table('baiduxml')." SET value='{$v}' WHERE name='{$k}'")?adminmsg('保存失敗', 1):"";
		}
		refresh_cache('baiduxml');
		write_log("Baiduプレートフォーム配置変更", $_SESSION['admin_name'],3);
		adminmsg("保存成功！",2);
}
elseif($act == 'del')
{
	$xmlset=get_cache('baiduxml');
	$xmldir = '../'.$xmlset['xmldir'];
	$file_name=$_POST['file_name'];
	if (empty($file_name))
	{
	adminmsg("文書を選択してください！",1);
	}
	if (!is_array($file_name)) $file_name=array($file_name);
	foreach($file_name as $f )
	{
	@unlink($xmldir.$f);
	}
	write_log("Baiduプレートフォーム文書を削除する", $_SESSION['admin_name'],3);
	adminmsg("削除成功！",2);
}
elseif($act == 'make')
{
	$xmlset=get_cache('baiduxml');
	$xmldir = '../'.$xmlset['xmldir'];
	$xmlorder=$xmlset['order'];
	if ($xmlorder=='1')
	{
	$sqlorder=" ORDER BY `addtime` DESC ";
	}
	else
	{
	$sqlorder=" ORDER BY `refreshtime` DESC ";
	}
	$jid=intval($_GET['jid']);
	$total=intval($_GET['total']);
	$err=intval($_GET['err']);
	$pageli=intval($_GET['pageli'])>0?intval($_GET['pageli']):1;
	$sqllimit=" LIMIT {$total},{$xmlset['xmlpagesize']}";
	if ($xmlset['xmlmax']>0 && $xmlset['xmlmax']<$xmlset['xmlpagesize'])
	{
		$sqllimit=" LIMIT {$total},{$xmlset['xmlmax']}";
	}
	require_once(HIGHWAY_ROOT_PATH.'include/baiduxml.class.php');
	$baiduxml = new BaiduXML();
	$result = $db->query("select * from ".table('jobs').$wheresql.$sqlorder.$sqllimit);	
	while($row = $db->fetch_array($result))
	{
	$total++;
	$contact=$db->getone("SELECT * from ".table('jobs_contact')." where pid = '{$row['id']}' LIMIT 1");
	$com=$db->getone("SELECT * from ".table('company_profile')." where id = '{$row['company_id']}' LIMIT 1");
	$category=$db->getone("SELECT * FROM ".table('category_jobs')." where id=".$row['category']." LIMIT 1");
	$subclass=$db->getone("SELECT * FROM ".table('category_jobs')." where id=".$row['subclass']." LIMIT 1");
	$row['jobs_url']=url_rewrite('HW_jobsshow',array('id'=>$row['id']));
	
	$x=array($row['jobs_url'],date("Y-m-d",$row['refreshtime']),$row['jobs_name'],date("Y-m-d",$row['deadline']),$row['contents'],$row['nature_cn'],   str_replace('/','',$row['district_cn']),$row['companyname'],$contact['email'],$category['categoryname'],$subclass['categoryname'],$row['education_cn'],$row['experience_cn'],date("Y-m-d",$row['addtime']),date("Y-m-d",$row['deadline']),str_replace('~','-',$row['wage_cn']),$row['trade_cn'],$com['nature_cn'],$_CFG['site_name'],$_CFG['site_domain'].$_CFG['site_dir']);
	foreach ($x as $key => $value) {
		$x[$key] = strip_tags(str_replace("&","&amp;",$value));
	}
	if (in_array('',$x))
	{
	$err++;
	continue;
	}
	$baiduxml->XML_url($x);
	
	$rowid=$row['id'];
	}
	if (empty($rowid))
	{
		if ($total===0)
		{
		adminmsg("生成できるデータがありません！",1);
		}
		else
		{
			for($b=1;$b<$pageli;$b++)
			{
				$xmlfile=$xmldir.$xmlset['xmlpre'].$b.'.xml';
				$xmlfile=ltrim($xmlfile,'../');
				$xmlfile=ltrim($xmlfile,'..\\');
				$atime=filemtime($xmldir.$xmlset['xmlpre'].$b.'.xml');
				$atime=date("Y-m-d",$atime);
				$index[]=array($_CFG['site_domain'].$_CFG['site_dir'].$xmlfile,$atime);
			}
			$baiduxml->XML_index_put($xmldir.$xmlset['indexname'],$index);
			$link[0]['text'] = "結果閲覧";
			$link[0]['href'] = '?act=xmllist';
			$pageli--;
			$total=$total-$err;
			adminmsg("生成完了！生成{$pageli}件資源文書，1件インデックス文書，{$total}件職生成成功，{$err}件職位生成失敗",2,$link);
		}	
	}
	else
	{
		$xmlname=$xmldir.$xmlset['xmlpre'].$pageli.'.xml';
		if ($baiduxml->XML_put($xmlname))
		{
		$pageli++;
		$link[0]['text'] = "システム自動続く...";
		$link[0]['href'] = "?act=make&total=".$total."&pageli=".$pageli."&err=".$err;
		adminmsg("{$xmlname}生成成功,システム自動続く...", 1,$link,true,2);
		exit();
		}
		else
		{
		$link[0]['text'] = "一覧に戻る";
		$link[0]['href'] = '?act=xmllist';
		adminmsg("生成失敗！",1,$link);
		}
	}	
}
?>
