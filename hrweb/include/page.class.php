<?php
 /*
 * 74cms 分页
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
class page{
 var $page_name="page";
 var $next_page='下一页';
 var $pre_page='上一页';
 var $first_page='首页';
 var $last_page='尾页';
 var $pre_bar='<<';
 var $next_bar='>>';
 var $format_left='';
 var $format_right='';
 var $pagebarnum=10;
 var $totalpage=0;
 var $nowindex=1;
 var $url="";
 var $offset=0;
 
 function page($array)
 {
  if(is_array($array))
  {
     if(!array_key_exists('total',$array))$this->error(__FUNCTION__,'need a param of total');
     $total=intval($array['total']);
     $perpage=(array_key_exists('perpage',$array))?intval($array['perpage']):10;
     $nowindex=(array_key_exists('nowindex',$array))?intval($array['nowindex']):'';
     $url=(array_key_exists('url',$array))?$array['url']:'';
     $alias = (array_key_exists('alias', $array)) ? $array['alias'] : '';
     $getarray = (array_key_exists('getarray', $array)) ? $array['getarray'] : '';
  }
  else
  	{
     $total=$array;
     $perpage=10;
     $nowindex='1';
     $url='';
     $alias = '';
     $getarray = '';

  }

  if((!is_int($total))||($total<0))$this->error(__FUNCTION__,$total.' is not a positive integer!');

  if((!is_int($perpage))||($perpage<=0))$this->error(__FUNCTION__,$perpage.' is not a positive integer!');

  if(!empty($array['page_name']))$this->set('page_name',$array['page_name']);

  $this->_set_nowindex($nowindex);

  $this->_set_url($url);

  $this->totalpage=ceil($total/$perpage);

  $this->offset=($this->nowindex-1)*$perpage;
  
  $this->alias = $alias;
  
  $this->getarray = $getarray;
 }

 function set($var,$value)
 {

  if(in_array($var,get_object_vars($this)))

     $this->$var=$value;

  else {

   $this->error(__FUNCTION__,$var." does not belong to PB_Page!");

  }

 }

 function next_page($style=''){
 	if($this->nowindex<$this->totalpage){
		return $this->_get_link($this->_get_url($this->nowindex+1),$this->next_page,$style);
	}
	return '<li><a class="'.$style.'">'.$this->next_page.'</a></li>';
 }

 function pre_page($style=''){
 	if($this->nowindex>1){
   	return $this->_get_link($this->_get_url($this->nowindex-1),$this->pre_page,$style);
  }
  return '<li><a class="'.$style.'">'.$this->pre_page.'</a></li>';
 }

 function first_page($style=''){
 	if($this->nowindex==1){
    	return '<li><a class="'.$style.'">'.$this->first_page.'</a></li>';
 	}
  return $this->_get_link($this->_get_url(1),$this->first_page,$style);
 }

 function last_page($style=''){
 	if($this->nowindex==$this->totalpage||$this->totalpage==0){

      return '<li><a class="'.$style.'">'.$this->last_page.'</a></li>';

  }

  return $this->_get_link($this->_get_url($this->totalpage),$this->last_page,$style);

 }


 function nowbar($style='',$nowindex_style='')

 {

  $plus=ceil($this->pagebarnum/2);

  if($this->pagebarnum-$plus+$this->nowindex>$this->totalpage)$plus=($this->pagebarnum-$this->totalpage+$this->nowindex);

  $begin=$this->nowindex-$plus+1;

  $begin=($begin>=1)?$begin:1;

  $return='';

  for($i=$begin;$i<$begin+$this->pagebarnum;$i++)

  {

   if($i<=$this->totalpage){

    if($i!=$this->nowindex)

        $return.=$this->_get_text($this->_get_link($this->_get_url($i),$i,$style));

    else

        $return.=$this->_get_text('<li><a class="'.$nowindex_style.'">'.$i.'</a></li>');

   }else{

    break;

   }

   $return.="\n";

  }

  unset($begin);

  return $return;

 }

 /**

  * 获取显示跳转按钮的代码

  *

  * @return string

  */

 function select()

 {

   $return='<select name="PB_Page_Select">';

  for($i=1;$i<=$this->totalpage;$i++)

  {

   if($i==$this->nowindex){

    $return.='<option value="'.$i.'" selected>'.$i.'</option>';

   }else{

    $return.='<option value="'.$i.'">'.$i.'</option>';

   }

  }

  unset($i);


  $return.='</select>';

  return $return;

 }



 /**

  * 获取mysql 语句中limit需要的值

  *

  * @return string

  */

 function offset()

 {

  return $this->offset;

 }



 /**

  * 控制分页显示风格（你可以增加相应的风格）

  *

  * @param int $mode

  * @return string

  */

 function show($mode=1)

 {

  switch ($mode)

  {

   case '1':

    $this->next_page='下一页';

    $this->pre_page='上一页';

    return $this->pre_page().$this->nowbar().$this->next_page().'第'.$this->select().'页';

    break;

   case '2':

    $this->next_page='下一页';

    $this->pre_page='上一页';

    $this->first_page='首页';

    $this->last_page='尾页';

    return $this->first_page().$this->pre_page().'[第'.$this->nowindex.'页]'.$this->next_page().$this->last_page().'第'.$this->select().'页';

    break;

   case '3':

    $this->next_page='下一页';

    $this->pre_page='上一页';

    $this->first_page='首页';

    $this->last_page='尾页';


    return $this->first_page()."".$this->pre_page()."".$this->nowbar("","select")."".$this->next_page()."".$this->last_page()."<li class=\"page_all\">".$this->nowindex."/".$this->totalpage."页</li><div class=\"clear\"></div>";

    break;

   case '4':

    $this->next_page='>';

    $this->pre_page='<';

    return "<span>".$this->nowindex."/".$this->totalpage."页</span>".$this->pre_page().$this->next_page()."<div class=\"clear\"></div>";

    break;

   case '5':

    return $this->pre_bar().$this->pre_page().$this->nowbar().$this->next_page().$this->next_bar();

    break;
	
	case '6':

    return "第".$this->nowindex."/".$this->totalpage."页";

    break;
  case '7':// 积分商城 小页

    $this->next_page='>';

    $this->pre_page='<';

    return $this->pre_page()."<li><b style='color:#ff9900'>".$this->nowindex."</b>/".$this->totalpage."页</li>".$this->next_page()."<div class=\"clear\"></div>";

    break;
  // 带跳转分页
  case '8':

    $this->next_page='下一页';

    $this->pre_page='上一页';

    $this->first_page='首页';

    $this->last_page='尾页';

    $this->go_page='跳转';

    return $this->first_page()."".$this->pre_page()."".$this->nowbar("","select")."".$this->next_page()."".$this->last_page()."<li class=\"page_all\">".$this->nowindex."/".$this->totalpage."页</li><li style='line-height:23px;'>".$this->go_page()."</li><div class=\"clear\"></div>";

    break;
  }

 }

  function go_page($style='')
  {
   $get=$this->getarray;
   $page_input="";
   foreach ($get as $key => $value)
   {

      if($key=="page")
      {
        continue;
      }
      $page_input.='<input type="hidden" name="'.$key.'" value="'.$value.'">';
   }
   return '<form method="GET" class="page_form">'.$page_input.'到 <input type="text" name="page" size="1" class="page_input"> 页 <input type="submit" class="page_submit" value="'.$this->go_page.'"></form>';
  }

 function _set_url($url="")

 {

  if(!empty($url)){

   $this->url=$url.((stristr($url,'?'))?'&':'?').$this->page_name."=";

  }else{

   if(empty($_SERVER['QUERY_STRING'])){

    $this->url=$this->request_url()."?".$this->page_name."=";

   }else{

    if(stristr($_SERVER['QUERY_STRING'],$this->page_name.'=')){

     $this->url=str_replace($this->page_name.'='.$this->nowindex,'',$this->request_url());

     $last=$this->url[strlen($this->url)-1];

     if($last=='?'||$last=='&'){

         $this->url.=$this->page_name."=";

     }else{

         $this->url.='&'.$this->page_name."=";

     }

    }else{

     $this->url=$this->request_url().'&'.$this->page_name.'=';

    }

   }

  }

 }
function _set_nowindex($nowindex)
{
	if(empty($nowindex))
	{
		   if(isset($_GET[$this->page_name]))
		   {
			$this->nowindex=intval($_GET[$this->page_name]);
		   }
	}
	else
	{
	   $this->nowindex=intval($nowindex);
	}
	$this->nowindex=$this->nowindex===0?1:$this->nowindex;
}

 function _get_url($pageno=1)
 {
 	if ($this->alias && $this->getarray)
	{
	$get=$this->getarray;
	$get['page']=$pageno;
	if ($get['key']) $get['key']=rawurlencode($get['key']);
	return url_rewrite($this->alias,$get);
	}
	else
	{
	return $this->url.$pageno;
	}
 }

 function _get_text($str)
 {
  return $this->format_left.$str.$this->format_right;
 }

 function _get_link($url,$text,$style='')
 {
  $style=(empty($style))?'':'class="'.$style.'"';
  return '<li><a '.$style.' href="'.$url.'">'.$text.'</a></li>';
 }
 function error($function,$errormsg)
 {
     die('Error in file <b>'.__FILE__.'</b> ,Function <b>'.$function.'()</b> :'.$errormsg);
 }
 function request_url()
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
}
?>