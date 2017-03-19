<?php
 /*
 * 74cms 工具类
 * ============================================================================
 * 版权所有: 骑士网络，并保留所有权利。
 * 网站地址: http://www.74cms.com；
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
*/
if(!defined('IN_QISHI')) exit('Access Denied!');
class help {
    static function addslashes_deep($value)
    {
        if (empty($value))
        {
            return $value;
        }
        else
        {
            if (!get_magic_quotes_gpc())
            {
            $value=is_array($value) ? array_map("help::addslashes_deep", $value) : help::mystrip_tags(addslashes($value));
            }
            else
            {
            $value=is_array($value) ? array_map("help::addslashes_deep", $value) : help::mystrip_tags($value);
            }
            return $value;
        }
    }
    static function remove_xss($string)
    {  
        require_once(QISHI_ROOT_PATH.'include/library/HTMLPurifier.auto.php');
        $config = HTMLPurifier_Config::createDefault();
        $config->set('HTML.AllowedElements', array('div'=>true, 'table'=>true, 'tr'=>true, 'td'=>true, 'br'=>true,"src"=>true,"strong"=>true,"em"=>true,"img"=>true,"p"=>true));
        $config->set('HTML.Doctype', 'XHTML 1.0 Transitional'); 
        if(!isset($_SERVER["HTTP_X_REQUESTED_WITH"]) || strtolower($_SERVER["HTTP_X_REQUESTED_WITH"])!="xmlhttprequest"){
            $config->set('Core.Encoding', QISHI_CHARSET);
        }
        $purifier = new HTMLPurifier($config);
        return $purifier->purify($string);
    }
    static function mystrip_tags($string)
    {
        $string =  help::new_html_special_chars($string);
        $string =  help::remove_xss($string);
        return $string;
    }
    static function new_html_special_chars($string) {

        $string = str_replace(array('&amp;', '&quot;', '&lt;', '&gt;'), array('&', '"', '<', '>'), $string);
        return $string;
    }
    // 实体出库
    static function htmlspecialchars_($value)
    {
        if (empty($value))
        {
            return $value;
        }
        else
        {
            if(is_array($value)){
                foreach ($value as $k => $v) {
                    $value[$k] = self::htmlspecialchars_($v);
                }
            }else{
                $value = htmlspecialchars($value,ENT_QUOTES,QISHI_CHARSET);
            }
            return $value;
        }
    }
    //sql 过滤
    static function CheckSql($db_string,$querytype='select')
    {
        global $QS_pwdhash;
        $clean = '';
        $error='';
        $old_pos = 0;
        $pos = -1;
        $log_file = QISHI_ROOT_PATH.'/data/'.md5($QS_pwdhash).'_safe.txt';
        $userIP = getip();
        $getUrl =request_url();
        $time = date('Y-m-d H:i:s');
        if($querytype=='select')
        {
            $notallow1 = "[^0-9a-z@\._-]{1,}(sleep|benchmark|load_file|outfile)[^0-9a-z@\.-]{1,}";
            if(preg_match("/".$notallow1."/i", $db_string))
            {
                fputs(fopen($log_file,'a+'),"$userIP||$time\r\n$getUrl\r\n$db_string\r\nSelectBreak\r\n===========\r\n");
                exit("您输入的内容不符合要求请正确输入！");
            }
        }
        //完整的SQL检查
        while (TRUE)
        {
            $pos = strpos($db_string, '\'', $pos + 1);
            if ($pos === FALSE)
            {
                break;
            }
            $clean .= substr($db_string, $old_pos, $pos - $old_pos);
            while (TRUE)
            {
                $pos1 = strpos($db_string, '\'', $pos + 1);
                $pos2 = strpos($db_string, '\\', $pos + 1);
                if ($pos1 === FALSE)
                {
                    break;
                }
                elseif ($pos2 == FALSE || $pos2 > $pos1)
                {
                    $pos = $pos1;
                    break;
                }
                $pos = $pos2 + 1;
            }
            $clean .= '$s$';
            $old_pos = $pos + 1;
        }
        $clean .= substr($db_string, $old_pos);
        $clean = trim(strtolower(preg_replace(array('~\s+~s' ), array(' '), $clean)));
        if (strpos($clean, '@') !== FALSE  OR strpos($clean,'char(')!== FALSE OR strpos($clean,'"')!== FALSE 
        OR strpos($clean,'$s$$s$')!== FALSE)
        {
            $fail = TRUE;
            if(preg_match("#^create table#i",$clean)) $fail = FALSE;
            $error="unusual character";
        }
        elseif (strpos($clean, '/*') > 2 || strpos($clean, '--') !== FALSE || strpos($clean, '#') !== FALSE)
        {
            $fail = TRUE;
            $error="comment detect";
        }
        elseif (strpos($clean, 'sleep') !== FALSE && preg_match('~(^|[^a-z])sleep($|[^[a-z])~is', $clean) != 0)
        {
            $fail = TRUE;
            $error="slown down detect";
        }
        elseif (strpos($clean, 'benchmark') !== FALSE && preg_match('~(^|[^a-z])benchmark($|[^[a-z])~is', $clean) != 0)
        {
            $fail = TRUE;
            $error="slown down detect";
        }
        elseif (strpos($clean, 'load_file') !== FALSE && preg_match('~(^|[^a-z])load_file($|[^[a-z])~is', $clean) != 0)
        {
            $fail = TRUE;
            $error="file fun detect";
        }
        elseif (strpos($clean, 'into outfile') !== FALSE && preg_match('~(^|[^a-z])into\s+outfile($|[^[a-z])~is', $clean) != 0)
        {
            $fail = TRUE;
            $error="file fun detect";
        }
        if (!empty($fail))
        {
            fputs(fopen($log_file,'a+'),"$userIP||$time\r\n$getUrl\r\n$db_string\r\n$error\r\n===========\r\n");
            exit("您输入的内容不符合要求请正确输入！");
        }
        else
        {
            return $db_string;
        }
    }
}
?>