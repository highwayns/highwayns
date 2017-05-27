<?php

class HTMLPurifier_TokenFactory
{
    // p stands for prototype

    private $p_start;

    private $p_end;

    private $p_empty;

    private $p_text;

    private $p_comment;

    public function __construct()
    {
        $this->p_start = new HTMLPurifier_Token_Start('', array());
        $this->p_end = new HTMLPurifier_Token_End('');
        $this->p_empty = new HTMLPurifier_Token_Empty('', array());
        $this->p_text = new HTMLPurifier_Token_Text('');
        $this->p_comment = new HTMLPurifier_Token_Comment('');
    }

    public function createStart($name, $attr = array())
    {
        $p = clone $this->p_start;
        $p->__construct($name, $attr);
        return $p;
    }

    public function createEnd($name)
    {
        $p = clone $this->p_end;
        $p->__construct($name);
        return $p;
    }

    public function createEmpty($name, $attr = array())
    {
        $p = clone $this->p_empty;
        $p->__construct($name, $attr);
        return $p;
    }

    public function createText($data)
    {
        $p = clone $this->p_text;
        $p->__construct($data);
        return $p;
    }

    public function createComment($data)
    {
        $p = clone $this->p_comment;
        $p->__construct($data);
        return $p;
    }
}

// vim: et sw=4 sts=4
