<?php

class HTMLPurifier_URIScheme_http extends HTMLPurifier_URIScheme
{
    public $default_port = 80;

    public $browsable = true;

    public $hierarchical = true;

    public function doValidate(&$uri, $config, $context)
    {
        $uri->userinfo = null;
        return true;
    }
}

// vim: et sw=4 sts=4
