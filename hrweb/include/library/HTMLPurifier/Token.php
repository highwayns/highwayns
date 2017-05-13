<?php

abstract class HTMLPurifier_Token
{
    public $line;

    public $col;

    public $armor = array();

    public $skip;

    public $rewind;

    public $carryover;

    public function __get($n)
    {
        if ($n === 'type') {
            trigger_error('Deprecated type property called; use instanceof', E_USER_NOTICE);
            switch (get_class($this)) {
                case 'HTMLPurifier_Token_Start':
                    return 'start';
                case 'HTMLPurifier_Token_Empty':
                    return 'empty';
                case 'HTMLPurifier_Token_End':
                    return 'end';
                case 'HTMLPurifier_Token_Text':
                    return 'text';
                case 'HTMLPurifier_Token_Comment':
                    return 'comment';
                default:
                    return null;
            }
        }
    }

    public function position($l = null, $c = null)
    {
        $this->line = $l;
        $this->col = $c;
    }

    public function rawPosition($l, $c)
    {
        if ($c === -1) {
            $l++;
        }
        $this->line = $l;
        $this->col = $c;
    }

    abstract public function toNode();
}

// vim: et sw=4 sts=4
