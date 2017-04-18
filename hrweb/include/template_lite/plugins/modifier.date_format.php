<?php


function tpl_modifier_date_format($string, $format="%b %e, %Y", $default_date=null)
{
	if (empty($string))
	{
	return "- -";
	}
	return strftime($format, $string);
}
?>
