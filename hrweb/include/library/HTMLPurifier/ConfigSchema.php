<?php

class HTMLPurifier_ConfigSchema
{
    public $defaults = array();

    public $defaultPlist;

    public $info = array();

    protected static $singleton;

    public function __construct()
    {
        $this->defaultPlist = new HTMLPurifier_PropertyList();
    }

    public static function makeFromSerial()
    {
        $contents = file_get_contents(HTMLPURIFIER_PREFIX . '/HTMLPurifier/ConfigSchema/schema.ser');
        $r = unserialize($contents);
        if (!$r) {
            $hash = sha1($contents);
            trigger_error("Unserialization of configuration schema failed, sha1 of file was $hash", E_USER_ERROR);
        }
        return $r;
    }

    public static function instance($prototype = null)
    {
        if ($prototype !== null) {
            HTMLPurifier_ConfigSchema::$singleton = $prototype;
        } elseif (HTMLPurifier_ConfigSchema::$singleton === null || $prototype === true) {
            HTMLPurifier_ConfigSchema::$singleton = HTMLPurifier_ConfigSchema::makeFromSerial();
        }
        return HTMLPurifier_ConfigSchema::$singleton;
    }

    public function add($key, $default, $type, $allow_null)
    {
        $obj = new stdclass();
        $obj->type = is_int($type) ? $type : HTMLPurifier_VarParser::$types[$type];
        if ($allow_null) {
            $obj->allow_null = true;
        }
        $this->info[$key] = $obj;
        $this->defaults[$key] = $default;
        $this->defaultPlist->set($key, $default);
    }

    public function addValueAliases($key, $aliases)
    {
        if (!isset($this->info[$key]->aliases)) {
            $this->info[$key]->aliases = array();
        }
        foreach ($aliases as $alias => $real) {
            $this->info[$key]->aliases[$alias] = $real;
        }
    }

    public function addAllowedValues($key, $allowed)
    {
        $this->info[$key]->allowed = $allowed;
    }

    public function addAlias($key, $new_key)
    {
        $obj = new stdclass;
        $obj->key = $new_key;
        $obj->isAlias = true;
        $this->info[$key] = $obj;
    }

    public function postProcess()
    {
        foreach ($this->info as $key => $v) {
            if (count((array) $v) == 1) {
                $this->info[$key] = $v->type;
            } elseif (count((array) $v) == 2 && isset($v->allow_null)) {
                $this->info[$key] = -$v->type;
            }
        }
    }
}

// vim: et sw=4 sts=4
