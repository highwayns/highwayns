<?php

function tpl_compiler_debug($params, &$tpl)
{
	if($params['output'])
	{
	    $debug_output = '$this->assign("_templatelite_debug_output", ' . $params['output'] . ');';
	}
	else
	{
		$debug_output = "";
	}

	if(!function_exists("generate_compiler_debug_output"))
	{
		require_once(TEMPLATE_LITE_DIR . "internal/compile.generate_compiler_debug_output.php");
	}
	$debug_output .= generate_compiler_debug_output($tpl);
	return $debug_output;
}

?>
