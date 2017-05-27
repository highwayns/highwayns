<?php

class HTMLPurifier_HTMLModule_Target extends HTMLPurifier_HTMLModule
{
    public $name = 'Target';

    public function setup($config)
    {
        $elements = array('a');
        foreach ($elements as $name) {
            $e = $this->addBlankElement($name);
            $e->attr = array(
                'target' => new HTMLPurifier_AttrDef_HTML_FrameTarget()
            );
        }
    }
}

// vim: et sw=4 sts=4
