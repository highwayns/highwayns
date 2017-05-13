<?php

class HTMLPurifier_Context
{

    private $_storage = array();

    public function register($name, &$ref)
    {
        if (array_key_exists($name, $this->_storage)) {
            trigger_error(
                "Name $name produces collision, cannot re-register",
                E_USER_ERROR
            );
            return;
        }
        $this->_storage[$name] =& $ref;
    }

    public function &get($name, $ignore_error = false)
    {
        if (!array_key_exists($name, $this->_storage)) {
            if (!$ignore_error) {
                trigger_error(
                    "Attempted to retrieve non-existent variable $name",
                    E_USER_ERROR
                );
            }
            $var = null; // so we can return by reference
            return $var;
        }
        return $this->_storage[$name];
    }

    public function destroy($name)
    {
        if (!array_key_exists($name, $this->_storage)) {
            trigger_error(
                "Attempted to destroy non-existent variable $name",
                E_USER_ERROR
            );
            return;
        }
        unset($this->_storage[$name]);
    }

    public function exists($name)
    {
        return array_key_exists($name, $this->_storage);
    }

    public function loadArray($context_array)
    {
        foreach ($context_array as $key => $discard) {
            $this->register($key, $context_array[$key]);
        }
    }
}

// vim: et sw=4 sts=4
