<?php

class HTMLPurifier_PropertyList
{
    protected $data = array();

    protected $parent;

    protected $cache;

    public function __construct($parent = null)
    {
        $this->parent = $parent;
    }

    public function get($name)
    {
        if ($this->has($name)) {
            return $this->data[$name];
        }
        // possible performance bottleneck, convert to iterative if necessary
        if ($this->parent) {
            return $this->parent->get($name);
        }
        throw new HTMLPurifier_Exception("Key '$name' not found");
    }

    public function set($name, $value)
    {
        $this->data[$name] = $value;
    }

    public function has($name)
    {
        return array_key_exists($name, $this->data);
    }

    public function reset($name = null)
    {
        if ($name == null) {
            $this->data = array();
        } else {
            unset($this->data[$name]);
        }
    }

    public function squash($force = false)
    {
        if ($this->cache !== null && !$force) {
            return $this->cache;
        }
        if ($this->parent) {
            return $this->cache = array_merge($this->parent->squash($force), $this->data);
        } else {
            return $this->cache = $this->data;
        }
    }

    public function getParent()
    {
        return $this->parent;
    }

    public function setParent($plist)
    {
        $this->parent = $plist;
    }
}

// vim: et sw=4 sts=4
