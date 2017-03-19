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
if(!defined('IN_QISHI')) exit('Access Denied!');
require_once(QISHI_ROOT_PATH.'include/help.class.php');
class mysql {
	var $linkid=null;
    function __construct($dbhost, $dbuser, $dbpw, $dbname = '', $dbcharset = 'gbk', $connect = 1) {
    	$this -> connect($dbhost, $dbuser, $dbpw, $dbname, $dbcharset, $connect);
    }

    function connect($dbhost, $dbuser, $dbpw, $dbname = '', $dbcharset = 'gbk', $connect=1){
    	$func = empty($connect) ? 'mysql_pconnect' : 'mysql_connect';
    	if(!$this->linkid = @$func($dbhost, $dbuser, $dbpw, true)){
    		$this->dbshow('Can not connect to Mysql!');
    	} else {
    		if($this->dbversion() > '4.1'){
    			mysql_query( "SET NAMES gbk");
    			if($this->dbversion() > '5.0.1'){
    				mysql_query("SET sql_mode = ''",$this->linkid);
					mysql_query("SET character_set_connection=".$dbcharset.", character_set_results=".$dbcharset.", character_set_client=binary", $this->linkid);
    			}
    		}
    	}
    	if($dbname){
    		if(mysql_select_db($dbname, $this->linkid)===false){
    			$this->dbshow("Can't select MySQL database($dbname)!");
    		}
    	}
    }

    function select_db($dbname){
    	return mysql_select_db($dbname, $this->linkid);
    }
    function query($sql){
        $sql=help::CheckSql($sql);

    	if(!$query=@mysql_query($sql, $this->linkid)){
    		$this->dbshow("Query error:$sql");
    	}else{
    		return $query;
    	}
    }
    function inserttable($tablename, $insertsqlarr, $returnid=0, $replace = false, $silent=0){
        $insertkeysql = $insertvaluesql = $comma = '';
        foreach ($insertsqlarr as $insert_key => $insert_value) {
            $insertkeysql .= $comma.'`'.$insert_key.'`';
            $insertvaluesql .= $comma.'\''.$insert_value.'\'';
            $comma = ', ';
        }
        $method = $replace?'REPLACE':'INSERT';
        $state = $this->query($method." INTO $tablename ($insertkeysql) VALUES ($insertvaluesql)", $silent?'SILENT':'');
        if($returnid && !$replace) {
            return $this->insert_id();
        }else {
            return $state;
        } 
    }
    function updatetable($tablename, $setsqlarr, $wheresqlarr, $silent=0)
    {

        $setsql = $comma = '';
        foreach ($setsqlarr as $set_key => $set_value) {
            if(is_array($set_value)) {
                $setsql .= $comma.'`'.$set_key.'`'.'=\''.$set_value[0].'\'';
            } else {
                $setsql .= $comma.'`'.$set_key.'`'.'=\''.$set_value.'\'';
            }
            $comma = ', ';
        }
        $where = $comma = '';
        if(empty($wheresqlarr)) {
            $where = '1';
        } elseif(is_array($wheresqlarr)) {
            foreach ($wheresqlarr as $key => $value) {
                $where .= $comma.'`'.$key.'`'.'=\''.$value.'\'';
                $comma = ' AND ';
            }
        } else {
            $where = $wheresqlarr;
        }
           
        return $this->query("UPDATE ".($tablename)." SET ".$setsql." WHERE ".$where, $silent?"SILENT":"");
    }
    function getall($sql, $type=MYSQL_ASSOC){
    	$query = $this->query($sql);
    	while($row = mysql_fetch_array($query,$type)){
    		$rows[] = $row;
    	}
    	return help::htmlspecialchars_($rows);
    }

    function getone($sql, $type=MYSQL_ASSOC){
    	$query = $this->query($sql,$this->linkid);
    	$row = mysql_fetch_array($query, $type);
        foreach ($row as $key => $value) {
           $row[$key]=$value;
        }
    	return help::htmlspecialchars_($row);
    }
	function get_total($sql)
	{
		$row = $this->getall($sql);
		$v=0;
		if (!empty($row) && is_array($row))
		{			
			foreach($row as $n)
			{
			$v=$v+$n['num'];
			}			
		}
		return $v;
 	}
    function getfirst($sql, $type=MYSQL_NUM) {
    	$query = $this->query($sql, $this->linkid);
    	$row = mysql_fetch_array($query, $type);
    	return $row[0];
    }
	function fetch_array($result,$type = MYSQL_ASSOC){
		return mysql_fetch_array($result,$type);
	}

    function affected_rows(){
    	return mysql_affected_rows($this->linkid);
    }

    function num_rows(){
    	return mysql_num_rows($this->linkid);
    }

    function num_fields($result){
    	return mysql_num_fields($result);
    }

    function insert_id(){
    	return mysql_insert_id($this->linkid);
    }

    function free_result(){
    	return mysql_free_result($this->linkid);
    }
	
	function escape_string($string)
    {
        if (PHP_VERSION >= '4.3')
        {
            return mysql_real_escape_string($string, $this->linkid);
        }
        else
        {
            return mysql_escape_string($string, $this->linkid);
        }
    }
    function error(){
    	return mysql_error($this->linkid);
    }

    function errno(){
    	return mysql_errno($this->linkid);
    }

    function close(){
    	return mysql_close($this->linkid);
    }

    function dbversion(){
    	return mysql_get_server_info($this->linkid);
    }

    function dbshow($err)
	{
    	if($err){
    		$info = "Error：".$err;			
    	}else{
    		$info = "Errno：".$this->errno()." Error：".$this->error();
    	}
    	exit($info);
        // exit("数据库错误,请联系网站管理员！");
    }
}
?>