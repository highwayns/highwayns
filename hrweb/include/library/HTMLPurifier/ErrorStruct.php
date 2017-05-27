<?php

class HTMLPurifier_ErrorStruct
{

    const TOKEN     = 0;
    const ATTR      = 1;
    const CSSPROP   = 2;

    public $type;

    public $value;

    public $errors = array();

    public $children = array();

    public function getChild($type, $id)
    {
        if (!isset($this->children[$type][$id])) {
            $this->children[$type][$id] = new HTMLPurifier_ErrorStruct();
            $this->children[$type][$id]->type = $type;
        }
        return $this->children[$type][$id];
    }

    public function addError($severity, $message)
    {
        $this->errors[] = array($severity, $message);
    }
}

// vim: et sw=4 sts=4
