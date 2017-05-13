<?php

class HTMLPurifier_DoctypeRegistry
{

    protected $doctypes;

    protected $aliases;

    public function register(
        $doctype,
        $xml = true,
        $modules = array(),
        $tidy_modules = array(),
        $aliases = array(),
        $dtd_public = null,
        $dtd_system = null
    ) {
        if (!is_array($modules)) {
            $modules = array($modules);
        }
        if (!is_array($tidy_modules)) {
            $tidy_modules = array($tidy_modules);
        }
        if (!is_array($aliases)) {
            $aliases = array($aliases);
        }
        if (!is_object($doctype)) {
            $doctype = new HTMLPurifier_Doctype(
                $doctype,
                $xml,
                $modules,
                $tidy_modules,
                $aliases,
                $dtd_public,
                $dtd_system
            );
        }
        $this->doctypes[$doctype->name] = $doctype;
        $name = $doctype->name;
        // hookup aliases
        foreach ($doctype->aliases as $alias) {
            if (isset($this->doctypes[$alias])) {
                continue;
            }
            $this->aliases[$alias] = $name;
        }
        // remove old aliases
        if (isset($this->aliases[$name])) {
            unset($this->aliases[$name]);
        }
        return $doctype;
    }

    public function get($doctype)
    {
        if (isset($this->aliases[$doctype])) {
            $doctype = $this->aliases[$doctype];
        }
        if (!isset($this->doctypes[$doctype])) {
            trigger_error('Doctype ' . htmlspecialchars($doctype) . ' does not exist', E_USER_ERROR);
            $anon = new HTMLPurifier_Doctype($doctype);
            return $anon;
        }
        return $this->doctypes[$doctype];
    }

    public function make($config)
    {
        return clone $this->get($this->getDoctypeFromConfig($config));
    }

    public function getDoctypeFromConfig($config)
    {
        // recommended test
        $doctype = $config->get('HTML.Doctype');
        if (!empty($doctype)) {
            return $doctype;
        }
        $doctype = $config->get('HTML.CustomDoctype');
        if (!empty($doctype)) {
            return $doctype;
        }
        // backwards-compatibility
        if ($config->get('HTML.XHTML')) {
            $doctype = 'XHTML 1.0';
        } else {
            $doctype = 'HTML 4.01';
        }
        if ($config->get('HTML.Strict')) {
            $doctype .= ' Strict';
        } else {
            $doctype .= ' Transitional';
        }
        return $doctype;
    }
}

// vim: et sw=4 sts=4
