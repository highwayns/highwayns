<?php

class HTMLPurifier_HTMLModule_Text extends HTMLPurifier_HTMLModule
{
    public $name = 'Text';

    public $content_sets = array(
        'Flow' => 'Heading | Block | Inline'
    );

    public function setup($config)
    {
        // Inline Phrasal -------------------------------------------------
        $this->addElement('abbr', 'Inline', 'Inline', 'Common');
        $this->addElement('acronym', 'Inline', 'Inline', 'Common');
        $this->addElement('cite', 'Inline', 'Inline', 'Common');
        $this->addElement('dfn', 'Inline', 'Inline', 'Common');
        $this->addElement('kbd', 'Inline', 'Inline', 'Common');
        $this->addElement('q', 'Inline', 'Inline', 'Common', array('cite' => 'URI'));
        $this->addElement('samp', 'Inline', 'Inline', 'Common');
        $this->addElement('var', 'Inline', 'Inline', 'Common');

        $em = $this->addElement('em', 'Inline', 'Inline', 'Common');
        $em->formatting = true;

        $strong = $this->addElement('strong', 'Inline', 'Inline', 'Common');
        $strong->formatting = true;

        $code = $this->addElement('code', 'Inline', 'Inline', 'Common');
        $code->formatting = true;

        // Inline Structural ----------------------------------------------
        $this->addElement('span', 'Inline', 'Inline', 'Common');
        $this->addElement('br', 'Inline', 'Empty', 'Core');

        // Block Phrasal --------------------------------------------------
        $this->addElement('address', 'Block', 'Inline', 'Common');
        $this->addElement('blockquote', 'Block', 'Optional: Heading | Block | List', 'Common', array('cite' => 'URI'));
        $pre = $this->addElement('pre', 'Block', 'Inline', 'Common');
        $pre->excludes = $this->makeLookup(
            'img',
            'big',
            'small',
            'object',
            'applet',
            'font',
            'basefont'
        );
        $this->addElement('h1', 'Heading', 'Inline', 'Common');
        $this->addElement('h2', 'Heading', 'Inline', 'Common');
        $this->addElement('h3', 'Heading', 'Inline', 'Common');
        $this->addElement('h4', 'Heading', 'Inline', 'Common');
        $this->addElement('h5', 'Heading', 'Inline', 'Common');
        $this->addElement('h6', 'Heading', 'Inline', 'Common');

        // Block Structural -----------------------------------------------
        $p = $this->addElement('p', 'Block', 'Inline', 'Common');
        $p->autoclose = array_flip(
            array("address", "blockquote", "center", "dir", "div", "dl", "fieldset", "ol", "p", "ul")
        );

        $this->addElement('div', 'Block', 'Flow', 'Common');
    }
}

// vim: et sw=4 sts=4
