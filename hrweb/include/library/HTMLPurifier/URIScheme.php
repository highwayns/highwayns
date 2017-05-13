<?php

abstract class HTMLPurifier_URIScheme
{

    public $default_port = null;

    public $browsable = false;

    public $secure = false;

    public $hierarchical = false;

    public $may_omit_host = false;

    abstract public function doValidate(&$uri, $config, $context);

    public function validate(&$uri, $config, $context)
    {
        if ($this->default_port == $uri->port) {
            $uri->port = null;
        }
        // kludge: browsers do funny things when the scheme but not the
        // authority is set
        if (!$this->may_omit_host &&
            // if the scheme is present, a missing host is always in error
            (!is_null($uri->scheme) && ($uri->host === '' || is_null($uri->host))) ||
            // if the scheme is not present, a *blank* host is in error,
            // since this translates into '///path' which most browsers
            // interpret as being 'http://path'.
            (is_null($uri->scheme) && $uri->host === '')
        ) {
            do {
                if (is_null($uri->scheme)) {
                    if (substr($uri->path, 0, 2) != '//') {
                        $uri->host = null;
                        break;
                    }
                    // URI is '////path', so we cannot nullify the
                    // host to preserve semantics.  Try expanding the
                    // hostname instead (fall through)
                }
                // first see if we can manually insert a hostname
                $host = $config->get('URI.Host');
                if (!is_null($host)) {
                    $uri->host = $host;
                } else {
                    // we can't do anything sensible, reject the URL.
                    return false;
                }
            } while (false);
        }
        return $this->doValidate($uri, $config, $context);
    }
}

// vim: et sw=4 sts=4
