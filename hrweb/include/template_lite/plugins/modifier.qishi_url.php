<?php
function tpl_modifier_qishi_url($string)
{
	global $_CFG;
	if (strpos($string,","))
	{
		$val=explode(",",$string);
		if ($val[0]=="QS_user")
		{
			return get_member_url($val[1],true);
		}
		else
		{
			if (strpos($val[1],"-"))
			{
				$g=explode("-",$val[1]);
				if(!empty($g))
				{
					foreach($g as $v)
					{
						$vs=explode(":",$v);
						$getarray[$vs[0]]=$vs[1];
					}
				}
			}
			else
			{
			$g=explode(":",$val[1]);
			$getarray[$g[0]]=$g[1];
			}
			return url_rewrite($val[0],$getarray);
		}	
	}
	else
	{
	return url_rewrite($string);
	}
}
?>