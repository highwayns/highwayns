<?php

class HTMLPurifier_AttrTransform_BoolToCSS extends HTMLPurifier_AttrTransform
{
    protected $attr;

    protected $css;

    public function __construct($attr, $css)
    {
        $this->attr = $attr;
        $this->css = $css;
    }

    public function transform($attr, $config, $context)
    {
        if (!isset($attr[$this->attr])) {
            return $attr;
        }
        unset($attr[$this->attr]);
        $this->prependCSS($attr, $this->css);
        return $attr;
    }
}

// vim: et sw=4 sts=4
