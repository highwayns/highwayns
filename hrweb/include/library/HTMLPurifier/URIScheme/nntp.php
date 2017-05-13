<?php

class HTMLPurifier_URIScheme_nntp extends HTMLPurifier_URIScheme
{
    public $default_port = 119;

    public $browsable = false;

    public function doValidate(&$uri, $config, $context)
    {
        $uri->userinfo = null;
        $uri->query = null;
        return true;
    }
}

// vim: et sw=4 sts=4
