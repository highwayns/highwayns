<?php

class HTMLPurifier_Node_Comment extends HTMLPurifier_Node
{
    public $data;

    public $is_whitespace = true;

    public function __construct($data, $line = null, $col = null)
    {
        $this->data = $data;
        $this->line = $line;
        $this->col = $col;
    }

    public function toTokenPair() {
        return array(new HTMLPurifier_Token_Comment($this->data, $this->line, $this->col), null);
    }
}
