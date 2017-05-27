<?php

class HTMLPurifier_AttrTransform_EnumToCSS extends HTMLPurifier_AttrTransform
{
    protected $attr;

    protected $enumToCSS = array();

    protected $caseSensitive = false;

    public function __construct($attr, $enum_to_css, $case_sensitive = false)
    {
        $this->attr = $attr;
        $this->enumToCSS = $enum_to_css;
        $this->caseSensitive = (bool)$case_sensitive;
    }

    public function transform($attr, $config, $context)
    {
        if (!isset($attr[$this->attr])) {
            return $attr;
        }

        $value = trim($attr[$this->attr]);
        unset($attr[$this->attr]);

        if (!$this->caseSensitive) {
            $value = strtolower($value);
        }

        if (!isset($this->enumToCSS[$value])) {
            return $attr;
        }
        $this->prependCSS($attr, $this->enumToCSS[$value]);
        return $attr;
    }
}

// vim: et sw=4 sts=4
