<?php
function tpl_modifier_string_format()
{
	$_args = func_get_args();
	$string = array_shift($_args);
	return vsprintf($string, $_args);
}
?>
