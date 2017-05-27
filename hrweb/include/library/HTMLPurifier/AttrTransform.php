<?php


abstract class HTMLPurifier_AttrTransform
{

    abstract public function transform($attr, $config, $context);

    public function prependCSS(&$attr, $css)
    {
        $attr['style'] = isset($attr['style']) ? $attr['style'] : '';
        $attr['style'] = $css . $attr['style'];
    }

    public function confiscateAttr(&$attr, $key)
    {
        if (!isset($attr[$key])) {
            return null;
        }
        $value = $attr[$key];
        unset($attr[$key]);
        return $value;
    }
}

// vim: et sw=4 sts=4
