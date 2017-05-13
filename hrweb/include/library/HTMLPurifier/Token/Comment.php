<?php

class HTMLPurifier_Token_Comment extends HTMLPurifier_Token
{
    public $data;

    public $is_whitespace = true;

    public function __construct($data, $line = null, $col = null)
    {
        $this->data = $data;
        $this->line = $line;
        $this->col = $col;
    }

    public function toNode() {
        return new HTMLPurifier_Node_Comment($this->data, $this->line, $this->col);
    }
}

// vim: et sw=4 sts=4
