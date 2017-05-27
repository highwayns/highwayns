<?php
function tpl_block_capture($params, $content, &$tpl)
{
	extract($params);

	if (isset($name))
	{
		$buffer = $name;
	}
	else
	{
		$buffer = "'default'";
	}

	$tpl->_templatelite_vars['capture'][$buffer] = $content;
	if (isset($assign))
	{
		$tpl->assign($assign, $content);
	}
	return;
}
?>
