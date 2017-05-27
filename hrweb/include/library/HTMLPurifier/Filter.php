<?php


class HTMLPurifier_Filter
{

    public $name;

    public function preFilter($html, $config, $context)
    {
        return $html;
    }

    public function postFilter($html, $config, $context)
    {
        return $html;
    }
}

// vim: et sw=4 sts=4
