<?php

class HTMLPurifier_HTMLModule_Presentation extends HTMLPurifier_HTMLModule
{

    public $name = 'Presentation';

    public function setup($config)
    {
        $this->addElement('hr', 'Block', 'Empty', 'Common');
        $this->addElement('sub', 'Inline', 'Inline', 'Common');
        $this->addElement('sup', 'Inline', 'Inline', 'Common');
        $b = $this->addElement('b', 'Inline', 'Inline', 'Common');
        $b->formatting = true;
        $big = $this->addElement('big', 'Inline', 'Inline', 'Common');
        $big->formatting = true;
        $i = $this->addElement('i', 'Inline', 'Inline', 'Common');
        $i->formatting = true;
        $small = $this->addElement('small', 'Inline', 'Inline', 'Common');
        $small->formatting = true;
        $tt = $this->addElement('tt', 'Inline', 'Inline', 'Common');
        $tt->formatting = true;
    }
}

// vim: et sw=4 sts=4
