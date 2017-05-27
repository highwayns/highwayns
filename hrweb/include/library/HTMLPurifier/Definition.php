<?php

abstract class HTMLPurifier_Definition
{

    public $setup = false;

    public $optimized = null;

    public $type;

    abstract protected function doSetup($config);

    public function setup($config)
    {
        if ($this->setup) {
            return;
        }
        $this->setup = true;
        $this->doSetup($config);
    }
}

// vim: et sw=4 sts=4
