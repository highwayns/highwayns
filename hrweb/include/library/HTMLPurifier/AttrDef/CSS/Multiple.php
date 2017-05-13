<?php

class HTMLPurifier_AttrDef_CSS_Multiple extends HTMLPurifier_AttrDef
{
    public $single;

    public $max;

    public function __construct($single, $max = 4)
    {
        $this->single = $single;
        $this->max = $max;
    }

    public function validate($string, $config, $context)
    {
        $string = $this->parseCDATA($string);
        if ($string === '') {
            return false;
        }
        $parts = explode(' ', $string); // parseCDATA replaced \r, \t and \n
        $length = count($parts);
        $final = '';
        for ($i = 0, $num = 0; $i < $length && $num < $this->max; $i++) {
            if (ctype_space($parts[$i])) {
                continue;
            }
            $result = $this->single->validate($parts[$i], $config, $context);
            if ($result !== false) {
                $final .= $result . ' ';
                $num++;
            }
        }
        if ($final === '') {
            return false;
        }
        return rtrim($final);
    }
}

// vim: et sw=4 sts=4
