<?php

abstract class HTMLPurifier_DefinitionCache
{
    public $type;

    public function __construct($type)
    {
        $this->type = $type;
    }

    public function generateKey($config)
    {
        return $config->version . ',' . // possibly replace with function calls
               $config->getBatchSerial($this->type) . ',' .
               $config->get($this->type . '.DefinitionRev');
    }

    public function isOld($key, $config)
    {
        if (substr_count($key, ',') < 2) {
            return true;
        }
        list($version, $hash, $revision) = explode(',', $key, 3);
        $compare = version_compare($version, $config->version);
        // version mismatch, is always old
        if ($compare != 0) {
            return true;
        }
        // versions match, ids match, check revision number
        if ($hash == $config->getBatchSerial($this->type) &&
            $revision < $config->get($this->type . '.DefinitionRev')) {
            return true;
        }
        return false;
    }

    public function checkDefType($def)
    {
        if ($def->type !== $this->type) {
            trigger_error("Cannot use definition of type {$def->type} in cache for {$this->type}");
            return false;
        }
        return true;
    }

    abstract public function add($def, $config);

    abstract public function set($def, $config);

    abstract public function replace($def, $config);

    abstract public function get($config);

    abstract public function remove($config);

    abstract public function flush($config);

    abstract public function cleanup($config);
}

// vim: et sw=4 sts=4
