<?php

class HTMLPurifier_ConfigSchema_Interchange_Directive
{

    public $id;

    public $type;

    public $default;

    public $description;

    public $typeAllowsNull = false;

    public $allowed;

    public $aliases = array();

    public $valueAliases;

    public $version;

    public $deprecatedUse;

    public $deprecatedVersion;

    public $external = array();
}

// vim: et sw=4 sts=4
