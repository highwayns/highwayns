<?php

abstract class HTMLPurifier_Strategy_Composite extends HTMLPurifier_Strategy
{

    protected $strategies = array();

    public function execute($tokens, $config, $context)
    {
        foreach ($this->strategies as $strategy) {
            $tokens = $strategy->execute($tokens, $config, $context);
        }
        return $tokens;
    }
}

// vim: et sw=4 sts=4
