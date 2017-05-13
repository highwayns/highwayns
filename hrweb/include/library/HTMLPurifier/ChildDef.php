<?php

abstract class HTMLPurifier_ChildDef
{
    public $type;

    public $allow_empty;

    public $elements = array();

    public function getAllowedElements($config)
    {
        return $this->elements;
    }

    abstract public function validateChildren($children, $config, $context);
}

// vim: et sw=4 sts=4
