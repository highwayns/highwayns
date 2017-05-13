<?php

class HTMLPurifier_StringHash extends ArrayObject
{
    protected $accessed = array();

    public function offsetGet($index)
    {
        $this->accessed[$index] = true;
        return parent::offsetGet($index);
    }

    public function getAccessed()
    {
        return $this->accessed;
    }

    public function resetAccessed()
    {
        $this->accessed = array();
    }
}

// vim: et sw=4 sts=4
