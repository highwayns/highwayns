<?php
function tpl_modifier_qishi_categoryname($string)
{
		global $db;
		if(strstr($string,":")){
			$val=explode(":",$string);
			$val1 = explode(",", $val[0]);
			$type = $val1[0];
			$cat_type = $val1[1];
			$param_arr = explode("_", $val[1]);
			$id_str = $param_arr[0];
			$id_arr = explode(".",$id_str);
			if($cat_type=="jobcategory"){
				if($id_arr[2]==0){
					$id = $id_arr[1];
					$cat=$db->getone("select categoryname from ".table('category_jobs')." WHERE  id='{$id}' LIMIT  1");
					return $cat['categoryname'];
				}else{
					$id = $id_arr[2];
					$cat=$db->getone("select categoryname from ".table('category_jobs')." WHERE  id='{$id}' LIMIT  1");
					return $cat['categoryname'];
				}
			}elseif($cat_type=="citycategory"){
				if($id_arr[1]==0){
					$id = $id_arr[0];
					$cat=$db->getone("select categoryname from ".table('category_district')." WHERE  id='{$id}' LIMIT  1");
					return $cat['categoryname'];
				}else{
					$id = $id_arr[1];
					$cat=$db->getone("select categoryname from ".table('category_district')." WHERE  id='{$id}' LIMIT  1");
					return $cat['categoryname'];
				}
			}elseif($cat_type=="trade"){
				$id = $id_arr[0];
				$_CAT=get_cache('category');
				return $_CAT[$type][$id]['categoryname'];
			}
		}else{
			$val=explode(",",$string);
			$type=trim($val[0]);
			$id=intval($val[1]);
			$len=intval($val[2]);
			if ($type=="QS_jobs")
			{
				$cat=$db->getone("select categoryname from ".table('category_jobs')." WHERE  id='{$id}' LIMIT  1");
				if ($len>0) $cat['categoryname']=cut_str($cat['categoryname'],$len,0,'');
				return $cat['categoryname'];
			}
			if ($type=="QS_jobs_floor")
			{
				$cat=$db->getone("select categoryname from ".table('category_jobs')." WHERE  id='{$id}' LIMIT  1");
				if ($len>0) $cat['categoryname']=cut_str($cat['categoryname'],$len,0,'');
				return $cat['categoryname'];
			}
			elseif ($type=="QS_district")
			{
				$cat=$db->getone("select categoryname from ".table('category_district')." WHERE  id='{$id}' LIMIT  1");
				if ($len>0) $cat['categoryname']=cut_str($cat['categoryname'],$len,0,'');
				return $cat['categoryname'];
			}
			elseif ($type=="QS_street")
			{
				$cat=$db->getone("select c_name from ".table('category')." WHERE  c_id='{$id}' LIMIT  1");
				$cat['categoryname']=$cat['c_name'];
				if ($len>0) $cat['categoryname']=cut_str($cat['categoryname'],$len,0,'');
				return $cat['categoryname'];
			}
			else
			{
				$_CAT=get_cache('category');
				if ($len>0) $_CAT[$type][$id]['categoryname']=cut_str($_CAT[$type][$id]['categoryname'],$len,0,'');
				return $_CAT[$type][$id]['categoryname'];
			}	
		}
			 
}
?>