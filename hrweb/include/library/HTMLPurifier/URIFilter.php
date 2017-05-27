<?php

abstract class HTMLPurifier_URIFilter
{

    public $name;

    public $post = false;

    public $always_load = false;

    public function prepare($config)
    {
        return true;
    }

    abstract public function filter(&$uri, $config, $context);
}

// vim: et sw=4 sts=4
