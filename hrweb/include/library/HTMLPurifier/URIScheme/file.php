<?php

class HTMLPurifier_URIScheme_file extends HTMLPurifier_URIScheme
{
    public $browsable = false;

    public $may_omit_host = true;

    public function doValidate(&$uri, $config, $context)
    {
        // Authentication method is not supported
        $uri->userinfo = null;
        // file:// makes no provisions for accessing the resource
        $uri->port = null;
        // While it seems to work on Firefox, the querystring has
        // no possible effect and is thus stripped.
        $uri->query = null;
        return true;
    }
}

// vim: et sw=4 sts=4
