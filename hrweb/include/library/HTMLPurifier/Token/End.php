<?php

class HTMLPurifier_Token_End extends HTMLPurifier_Token_Tag
{
    public $start;

    public function toNode() {
        throw new Exception("HTMLPurifier_Token_End->toNode not supported!");
    }
}

// vim: et sw=4 sts=4
