<?php

class HTMLPurifier_AttrDef_CSS_Composite extends HTMLPurifier_AttrDef
{

    public $defs;

    public function __construct($defs)
    {
        $this->defs = $defs;
    }

    public function validate($string, $config, $context)
    {
        foreach ($this->defs as $i => $def) {
            $result = $this->defs[$i]->validate($string, $config, $context);
            if ($result !== false) {
                return $result;
            }
        }
        return false;
    }
}

// vim: et sw=4 sts=4
