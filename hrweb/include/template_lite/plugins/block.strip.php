<?php
function tpl_block_strip($params, $content, &$tpl)
{
	$_strip_search = array(
		"![\t ]+$|^[\t ]+!m",		// remove leading/trailing space chars
		'%[\r\n]+%m',			// remove CRs and newlines
	);
	$_strip_replace = array(
		'',
		'',
	);
	return preg_replace($_strip_search, $_strip_replace, $content);
}
?>
