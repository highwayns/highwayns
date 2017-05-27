<?php

class HTMLPurifier_Node_Element extends HTMLPurifier_Node
{
    public $name;

    public $attr = array();

    public $children = array();

    public $empty = false;

    public $endCol = null, $endLine = null, $endArmor = array();

    public function __construct($name, $attr = array(), $line = null, $col = null, $armor = array()) {
        $this->name = $name;
        $this->attr = $attr;
        $this->line = $line;
        $this->col = $col;
        $this->armor = $armor;
    }

    public function toTokenPair() {
        // XXX inefficiency here, normalization is not necessary
        if ($this->empty) {
            return array(new HTMLPurifier_Token_Empty($this->name, $this->attr, $this->line, $this->col, $this->armor), null);
        } else {
            $start = new HTMLPurifier_Token_Start($this->name, $this->attr, $this->line, $this->col, $this->armor);
            $end = new HTMLPurifier_Token_End($this->name, array(), $this->endLine, $this->endCol, $this->endArmor);
            //$end->start = $start;
            return array($start, $end);
        }
    }
}

