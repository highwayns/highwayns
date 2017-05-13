<?php

abstract class HTMLPurifier_Token_Tag extends HTMLPurifier_Token
{
    public $is_tag = true;

    public $name;

    public $attr = array();

    public function __construct($name, $attr = array(), $line = null, $col = null, $armor = array())
    {
        $this->name = ctype_lower($name) ? $name : strtolower($name);
        foreach ($attr as $key => $value) {
            // normalization only necessary when key is not lowercase
            if (!ctype_lower($key)) {
                $new_key = strtolower($key);
                if (!isset($attr[$new_key])) {
                    $attr[$new_key] = $attr[$key];
                }
                if ($new_key !== $key) {
                    unset($attr[$key]);
                }
            }
        }
        $this->attr = $attr;
        $this->line = $line;
        $this->col = $col;
        $this->armor = $armor;
    }

    public function toNode() {
        return new HTMLPurifier_Node_Element($this->name, $this->attr, $this->line, $this->col, $this->armor);
    }
}

// vim: et sw=4 sts=4
