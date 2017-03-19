<?php

/*
 * Template Lite plugin converted from Smarty
 * -------------------------------------------------------------
 * Type:     modifier
 * Name:     date_format
 * Purpose:  format datestamps via strftime
 * Input:    string: input date string
 *           format: strftime format for output
 *           default_date: default date if $string is empty
 * -------------------------------------------------------------
 */

function tpl_modifier_date_format($string, $format="%b %e, %Y", $default_date=null)
{
	if (empty($string))
	{
	return "- -";
	}
	return strftime($format, $string);
}
?>
