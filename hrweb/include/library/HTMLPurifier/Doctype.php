<?php

class HTMLPurifier_Doctype
{
    public $name;

    public $modules = array();

    public $tidyModules = array();

    public $xml = true;

    public $aliases = array();

    public $dtdPublic;

    public $dtdSystem;

    public function __construct(
        $name = null,
        $xml = true,
        $modules = array(),
        $tidyModules = array(),
        $aliases = array(),
        $dtd_public = null,
        $dtd_system = null
    ) {
        $this->name         = $name;
        $this->xml          = $xml;
        $this->modules      = $modules;
        $this->tidyModules  = $tidyModules;
        $this->aliases      = $aliases;
        $this->dtdPublic    = $dtd_public;
        $this->dtdSystem    = $dtd_system;
    }
}

// vim: et sw=4 sts=4
