<?php

class HTMLPurifier_AttrDef_Clone extends HTMLPurifier_AttrDef
{
    protected $clone;

    public function __construct($clone)
    {
        $this->clone = $clone;
    }

    public function validate($v, $config, $context)
    {
        return $this->clone->validate($v, $config, $context);
    }

    public function make($string)
    {
        return clone $this->clone;
    }
}

// vim: et sw=4 sts=4
