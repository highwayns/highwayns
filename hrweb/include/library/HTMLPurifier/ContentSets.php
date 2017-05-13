<?php

class HTMLPurifier_ContentSets
{

    public $info = array();

    public $lookup = array();

    protected $keys = array();
    protected $values = array();

    public function __construct($modules)
    {
        if (!is_array($modules)) {
            $modules = array($modules);
        }
        // populate content_sets based on module hints
        // sorry, no way of overloading
        foreach ($modules as $module) {
            foreach ($module->content_sets as $key => $value) {
                $temp = $this->convertToLookup($value);
                if (isset($this->lookup[$key])) {
                    // add it into the existing content set
                    $this->lookup[$key] = array_merge($this->lookup[$key], $temp);
                } else {
                    $this->lookup[$key] = $temp;
                }
            }
        }
        $old_lookup = false;
        while ($old_lookup !== $this->lookup) {
            $old_lookup = $this->lookup;
            foreach ($this->lookup as $i => $set) {
                $add = array();
                foreach ($set as $element => $x) {
                    if (isset($this->lookup[$element])) {
                        $add += $this->lookup[$element];
                        unset($this->lookup[$i][$element]);
                    }
                }
                $this->lookup[$i] += $add;
            }
        }

        foreach ($this->lookup as $key => $lookup) {
            $this->info[$key] = implode(' | ', array_keys($lookup));
        }
        $this->keys   = array_keys($this->info);
        $this->values = array_values($this->info);
    }

    public function generateChildDef(&$def, $module)
    {
        if (!empty($def->child)) { // already done!
            return;
        }
        $content_model = $def->content_model;
        if (is_string($content_model)) {
            // Assume that $this->keys is alphanumeric
            $def->content_model = preg_replace_callback(
                '/\b(' . implode('|', $this->keys) . ')\b/',
                array($this, 'generateChildDefCallback'),
                $content_model
            );
            //$def->content_model = str_replace(
            //    $this->keys, $this->values, $content_model);
        }
        $def->child = $this->getChildDef($def, $module);
    }

    public function generateChildDefCallback($matches)
    {
        return $this->info[$matches[0]];
    }

    public function getChildDef($def, $module)
    {
        $value = $def->content_model;
        if (is_object($value)) {
            trigger_error(
                'Literal object child definitions should be stored in '.
                'ElementDef->child not ElementDef->content_model',
                E_USER_NOTICE
            );
            return $value;
        }
        switch ($def->content_model_type) {
            case 'required':
                return new HTMLPurifier_ChildDef_Required($value);
            case 'optional':
                return new HTMLPurifier_ChildDef_Optional($value);
            case 'empty':
                return new HTMLPurifier_ChildDef_Empty();
            case 'custom':
                return new HTMLPurifier_ChildDef_Custom($value);
        }
        // defer to its module
        $return = false;
        if ($module->defines_child_def) { // save a func call
            $return = $module->getChildDef($def);
        }
        if ($return !== false) {
            return $return;
        }
        // error-out
        trigger_error(
            'Could not determine which ChildDef class to instantiate',
            E_USER_ERROR
        );
        return false;
    }

    protected function convertToLookup($string)
    {
        $array = explode('|', str_replace(' ', '', $string));
        $ret = array();
        foreach ($array as $k) {
            $ret[$k] = true;
        }
        return $ret;
    }
}

// vim: et sw=4 sts=4
