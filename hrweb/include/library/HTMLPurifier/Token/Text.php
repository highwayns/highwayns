<?php

class HTMLPurifier_Token_Text extends HTMLPurifier_Token
{

    public $name = '#PCDATA';
    /**< PCDATA tag name compatible with DTD. */

    public $data;
    /**< Parsed character data of text. */

    public $is_whitespace;

    /**< Bool indicating if node is whitespace. */

    public function __construct($data, $line = null, $col = null)
    {
        $this->data = $data;
        $this->is_whitespace = ctype_space($data);
        $this->line = $line;
        $this->col = $col;
    }

    public function toNode() {
        return new HTMLPurifier_Node_Text($this->data, $this->is_whitespace, $this->line, $this->col);
    }
}

// vim: et sw=4 sts=4
