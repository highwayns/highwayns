<?php
 /*
 * 74cms 数据库操作
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
function write_head($table){
	global $db;
	$sql = '';
	$sql .= "DROP TABLE IF EXISTS `".$table."`;\r\n";
	$row = $db->getone("SHOW CREATE TABLE ".$table);
	$sql .= $row['Create Table'];
	return $sql;
}
function write_file($file, $sql){
	/*if(!$fp=@fopen($file, "w+")){
		adminmsg('打开目标文件出错');
	}
	if(!@fwrite($fp, $sql)){
		adminmsg('写入数据出错');
	}
	if(!@fclose($fp)){
		adminmsg('关闭目标文件出错');
	}*/
    @file_put_contents($file, $sql);
	return true;
}
function escape_str($str){
	$str=mysql_escape_string($str);
	$str=str_replace('\\\'','\'\'',$str);
	$str=str_replace("\\\\","\\\\\\\\",$str);
	$str=str_replace('$','\$',$str);
	return $str;
}
function get_sqlfile_info($file){
	$file_info = array('74cms_ver'=>'', 'mysql_ver'=> '', 'add_time'=>'');
    if (!$fp = @fopen($file,'rb'))
	{
		adminmsg("打开文件{$file}失败",0);
	}
    $str = fread($fp, 200);
    @fclose($fp);
    $arr = explode("\n", $str);
    foreach ($arr AS $val){
        $pos = strpos($val, ':');
        if ($pos > 0){
            $type = trim(substr($val, 0, $pos), "-\n\r\t ");
            $value = trim(substr($val, $pos+1), "/\n\r\t ");
            if ($type == '74CMS VERSION'){
                $file_info['74cms_ver'] = $value;
            }
            elseif ($type == 'Mysql VERSION'){
                $file_info['mysql_ver'] = substr($value,0,3);
            }
            elseif ($type == 'Create time'){
                $file_info['add_time'] = $value;
            }
        }
    }
    return $file_info;
}

function remove_comment($str)
{
    return (substr($str, 0, 2) != '--');
}
function get_rand_char($num)
{
    if (empty($num))
    {
        return false;
    }
    $string = 'ABCDEFGHIGKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';
    $str = '';
    for ($i = 0; $i < $num; $i++)
    {
        $pos = rand(0, 51);
        $str .= $string{$pos};
    }
    return $str;
}
function get_optimize_list()
	{
	global $db,$dbname;
	$row_arr = array();
	$result = $db->query("SHOW TABLE STATUS FROM `{$dbname}` WHERE Data_free>0");
	while($row = $db->fetch_array($result))
	{
		if ($row['Data_free']=="0")
		{
		$row['Data_free']="-";
		}
		if ($row['Data_free']>1 && $row['Data_free']<1024)
		{
		$row['Data_free']=$row['Data_free']." byte";
		}
		elseif($row['Data_free']>1024 && $row['Data_free']<1048576)
		{
		$row['Data_free']=number_format(($row['Data_free']/1024),1)." KB";
		}
		elseif($row['Data_free']>1048576)
		{
		$row['Data_free']=number_format(($row['Data_free']/1024/1024),1)." MB";
		}
		$row['Data_length']=$row['Data_length']+$row['Index_length'];
		//--
		if ($row['Data_length']=="0")
		{
		$row['Data_length']="-";
		}
		elseif($row['Data_length']<1048576)
		{
		$row['Data_length']=number_format(($row['Data_length']/1024),1)." KB";
		}
		elseif($row['Data_length']>1048576)
		{
		$row['Data_length']=number_format(($row['Data_length']/1024/1024),1)." MB";
		}
	$row_arr[] = $row;
	}
	return $row_arr;
}
?>
