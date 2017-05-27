<?php

abstract class HTMLPurifier_TagTransform
{

    public $transform_to;

    abstract public function transform($tag, $config, $context);

    protected function prependCSS(&$attr, $css)
    {
        $attr['style'] = isset($attr['style']) ? $attr['style'] : '';
        $attr['style'] = $css . $attr['style'];
    }
}

// vim: et sw=4 sts=4
