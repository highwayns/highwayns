<?php

class HTMLPurifier_HTMLModule_Object extends HTMLPurifier_HTMLModule
{
    public $name = 'Object';

    public $safe = false;

    public function setup($config)
    {
        $this->addElement(
            'object',
            'Inline',
            'Optional: #PCDATA | Flow | param',
            'Common',
            array(
                'archive' => 'URI',
                'classid' => 'URI',
                'codebase' => 'URI',
                'codetype' => 'Text',
                'data' => 'URI',
                'declare' => 'Bool#declare',
                'height' => 'Length',
                'name' => 'CDATA',
                'standby' => 'Text',
                'tabindex' => 'Number',
                'type' => 'ContentType',
                'width' => 'Length'
            )
        );

        $this->addElement(
            'param',
            false,
            'Empty',
            null,
            array(
                'id' => 'ID',
                'name*' => 'Text',
                'type' => 'Text',
                'value' => 'Text',
                'valuetype' => 'Enum#data,ref,object'
            )
        );
    }
}

// vim: et sw=4 sts=4
