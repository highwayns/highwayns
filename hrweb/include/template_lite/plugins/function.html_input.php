<?php
function tpl_function_html_input($params, &$tpl)
{
	require_once("shared.escape_chars.php");
	$name = null;
	$value = '';
	$password = false;
	$extra = '';

	foreach($params as $_key => $_value)
	{
		switch($_key)
		{
			case 'name':
			case 'value':
				$$_key = $_value;
				break;
			case 'password':
				$$_key = true;
				break;
			default:
				if(!is_array($_key))
				{
					$extra .= ' ' . $_key . '="' . tpl_escape_chars($_value) . '"';
				}
				else
				{
					$tpl->trigger_error("html_input: attribute '$_key' cannot be an array");
				}
		}
	}

	if (!isset($name) || empty($name))
	{
		$tpl->trigger_error("html_input: missing 'name' parameter");
		return;
	}
	
	$toReturn = '<input type="';
	$toReturn .= $password ? 'password' : 'text';
	$toReturn .= '" name="' . tpl_escape_chars($name) . '" value="' . tpl_escape_chars($value) . '" ' . $extra . ' />';
	return $toReturn;
}
?>
