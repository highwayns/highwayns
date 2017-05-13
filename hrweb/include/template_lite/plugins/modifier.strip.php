<?php
function tpl_modifier_strip($string, $replace = ' ')
{
	return preg_replace('!\s+!', $replace, $string);
}
?>
