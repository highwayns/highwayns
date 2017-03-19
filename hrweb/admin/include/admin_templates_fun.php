<?php
 /*
 * 74cms 管理中心 模板中心
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
 if(!defined('IN_QISHI'))
 {
 	die('Access Denied!');
 }
function get_templates_info($file){
	$file_info = array('name'=>'', 'version'=> '', 'author'=>'', 'authorurl'=>'');
    if (!$fp = @fopen($file,'rb'))
	{
		 return false;
	}
    $str = fread($fp, 200);
    @fclose($fp);
    $arr = explode("\n", $str);
    foreach ($arr as $val){
        $pos = strpos($val, ':');
        if ($pos > 0){
            $type = trim(substr($val, 0, $pos), "-\n\r\t ");
            $value = trim(substr($val, $pos+1), "/\n\r\t ");
            if ($type == 'name'){
                $file_info['name'] = $value;
            }
            elseif ($type == 'version'){
                $file_info['version'] = $value;
            }
            elseif ($type == 'author'){
                $file_info['author'] = $value;
            }
			 elseif ($type == 'authorurl'){
                $file_info['authorurl'] = $value;
            }
        }
    }
    return $file_info;
}
function get_user_tpl($type,$tpldir)
{
	global $db;
	$type=intval($type);
	$result = $db->query("select * from ".table('tpl')." where tpl_type='{$type}'");
	while($row = $db->fetch_array($result))
	{
	$row['info']=get_templates_info("../templates/".$tpldir."/".$row['tpl_dir']."/info.txt");
	$row_arr[] =$row;
	}
	return $row_arr;
}
function get_user_tpl_dir($type)
{
	global $db;
	$type=intval($type);
	$result = $db->query("select tpl_dir from ".table('tpl')." where tpl_type='{$type}'");
	while($row = $db->fetch_array($result))
	{
	$row_arr[] =$row['tpl_dir'];
	}
	return $row_arr;
}
?>