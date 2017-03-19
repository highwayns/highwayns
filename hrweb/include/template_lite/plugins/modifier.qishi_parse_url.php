<?php
function tpl_modifier_qishi_parse_url($string)
{
	global $_SERVER;
	$parse_url=parse_url(parse_url_request_url());
	$parse_url=$parse_url['query'];
	parse_str($parse_url,$urlarray);
	if (stripos($string,","))
	{
		$aget=explode(',',$string);
		foreach($aget as $value)
		{
		$val=explode(":",$value);
		$urlarray[$val[0]]=$val[1];
		}
	}
	else
	{
		$val=explode(":",$string);
		$urlarray[$val[0]]=$val[1];
	}	
	$urlarray['key']="";
	$urlarray['page']=1;
	return "?".http_build_query($urlarray);
}
function parse_url_request_url()
 {     
  	if (isset($_SERVER['REQUEST_URI']))     
    {        
   	 $url = $_SERVER['REQUEST_URI'];    
    }
	else
	{    
		  if (isset($_SERVER['argv']))        
			{           
			$url = $_SERVER['PHP_SELF'] .'?'. $_SERVER['argv'][0];      
			}         
		  else        
			{          
			$url = $_SERVER['PHP_SELF'] .'?'.$_SERVER['QUERY_STRING'];
			}  
    }    
    return $url; 
}
?>
