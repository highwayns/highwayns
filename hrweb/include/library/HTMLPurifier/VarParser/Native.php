<?php

class HTMLPurifier_VarParser_Native extends HTMLPurifier_VarParser
{

    protected function parseImplementation($var, $type, $allow_null)
    {
        return $this->evalExpression($var);
    }

    protected function evalExpression($expr)
    {
        $var = null;
        $result = eval("\$var = $expr;");
        if ($result === false) {
            throw new HTMLPurifier_VarParserException("Fatal error in evaluated code");
        }
        return $var;
    }
}

// vim: et sw=4 sts=4
