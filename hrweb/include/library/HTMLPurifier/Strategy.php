<?php



abstract class HTMLPurifier_Strategy
{

    abstract public function execute($tokens, $config, $context);
}

// vim: et sw=4 sts=4
