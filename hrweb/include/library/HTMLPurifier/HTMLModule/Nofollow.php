<?php

class HTMLPurifier_HTMLModule_Nofollow extends HTMLPurifier_HTMLModule
{

    public $name = 'Nofollow';

    public function setup($config)
    {
        $a = $this->addBlankElement('a');
        $a->attr_transform_post[] = new HTMLPurifier_AttrTransform_Nofollow();
    }
}

// vim: et sw=4 sts=4
