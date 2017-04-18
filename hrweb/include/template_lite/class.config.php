<?php

class config {
	var $overwrite 			= false;	// overwrite variables of the same name? if false, an array will be created
	var $booleanize			= true;		// turn true/false, yes/no, on/off, into 1/0
	var $fix_new_lines		= true;		// turns \r\n into \n?
	var $read_hidden		= true;		// read hidden sections?

	var $_db_qstr_regexp		= null;
	var $_bool_true_regexp		= null;
	var $_bool_false_regexp		= null;
	var $_qstr_regexp		= null;

	function config()
	{
		$this->_db_qstr_regexp = '"[^"\\\\]*(?:\\\\.[^"\\\\]*)*"';
		$this->_bool_true_regexp = 'true|yes|on';
		$this->_bool_false_regexp = 'false|no|off';
		$this->_qstr_regexp = '(?:' . $this->_db_qstr_regexp . '|' . $this->_bool_true_regexp . '|' . $this->_bool_false_regexp . ')';
	}

	function config_load($file, $section_name = null, $var_name = null)
	{
		$_result = array();
		$contents = file_get_contents($file);
		if (empty($contents))
		{
			die("Could not open $file");
		}

		// insert new line into beginning of file
		$contents = "\n" . $contents;
		// fix new-lines
		if ($this->fix_new_lines)
		{
			$contents = str_replace("\r\n","\n",$contents);
		}

		// match globals
		if (preg_match("/^(.*?)(\n\[|\Z)/s", $contents, $match))
		{
			$_result["globals"] = $this->_parse_config_section($match[1]);
		}

		// match sections
		if (preg_match_all("/^\[(.*?)\]/m", $contents, $match))
		{
			foreach ($match[1] as $section)
			{
				if ($section{0} == '.' && !$this->read_hidden)
				{
					continue;
				}
				preg_match("/\[".preg_quote($section)."\](.*?)(\n\[|\Z)/s",$contents,$match);
				if ($section{0} == '.')
				{
					$section = substr($section, 1);
				}
				$_result[$section] = $this->_parse_config_section($match[1]);
			}
		}


		if (!empty($var_name))
		{
			if (empty($section_name))
			{
				return $_result["globals"][$var_name];
			}
			else
			{
				if(isset($_result[$section_name][$var_name]))
				{
					return $_result[$section_name][$var_name];
				}
				else
				{
					return array();
				}
			}
		}
		else
		{
			if (empty($section_name))
			{
				return $_result;
			}
			else
			{
				if(isset($_result[$section_name]))
				{
					return $_result[$section_name];
				}
				else
				{
					return array();
				}
			}
		}
	}

	function _parse_config_section($body)
	{
		$_result = array();
		preg_match_all('!(\n\s*[a-zA-Z0-9_]+)\s*=\s*(' . $this->_qstr_regexp . ')!s', $body, $ini);
		$keys = $ini[1];
		$values = $ini[2];
		for($i = 0, $for_max = count($ini[0]); $i < $for_max; $i++)
		{
			if ($this->booleanize)
			{
				if (preg_match('/^(' . $this->_bool_true_regexp . ')$/i', $values[$i]))
				{
					$values[$i] = true;
				}
				elseif (preg_match('/^(' . $this->_bool_false_regexp . ')$/i', $values[$i]))
				{
					$values[$i] = false;
				}
			}
			if (!is_numeric($values[$i]) && !is_bool($values[$i]))
			{
				$values[$i] = str_replace("\n",'',stripslashes(substr($values[$i], 1, -1)));
			}
			if ($this->overwrite || !isset($_result[trim($keys[$i])]))
			{
				$_result[trim($keys[$i])] = $values[$i];
			}
			else
			{
				if (!is_array($_result[trim($keys[$i])]))
				{
					$_result[trim($keys[$i])] = array($_result[trim($keys[$i])]);
				}
				$_result[trim($keys[$i])][] = $values[$i];
			}
		}
		return $_result;
	}
}
?>
