<?php 
function tpl_function_in_array($params, &$tpl)
{
	extract($params);

	if (is_array($array))
	{
		if (in_array($match, $array))
		{
			return $returnvalue;
		}
	}
}
?>
