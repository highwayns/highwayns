<?php

// Enum = Enumerated
class HTMLPurifier_AttrDef_Enum extends HTMLPurifier_AttrDef
{

    public $valid_values = array();

    protected $case_sensitive = false; // values according to W3C spec

    public function __construct($valid_values = array(), $case_sensitive = false)
    {
        $this->valid_values = array_flip($valid_values);
        $this->case_sensitive = $case_sensitive;
    }

    public function validate($string, $config, $context)
    {
        $string = trim($string);
        if (!$this->case_sensitive) {
            // we may want to do full case-insensitive libraries
            $string = ctype_lower($string) ? $string : strtolower($string);
        }
        $result = isset($this->valid_values[$string]);

        return $result ? $string : false;
    }

    public function make($string)
    {
        if (strlen($string) > 2 && $string[0] == 's' && $string[1] == ':') {
            $string = substr($string, 2);
            $sensitive = true;
        } else {
            $sensitive = false;
        }
        $values = explode(',', $string);
        return new HTMLPurifier_AttrDef_Enum($values, $sensitive);
    }
}

// vim: et sw=4 sts=4
