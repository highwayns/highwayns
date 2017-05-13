<?php


abstract class HTMLPurifier_AttrDef
{

    public $minimized = false;

    public $required = false;

    abstract public function validate($string, $config, $context);

    public function parseCDATA($string)
    {
        $string = trim($string);
        $string = str_replace(array("\n", "\t", "\r"), ' ', $string);
        return $string;
    }

    public function make($string)
    {
        // default implementation, return a flyweight of this object.
        // If $string has an effect on the returned object (i.e. you
        // need to overload this method), it is best
        // to clone or instantiate new copies. (Instantiation is safer.)
        return $this;
    }

    protected function mungeRgb($string)
    {
        return preg_replace('/rgb\((\d+)\s*,\s*(\d+)\s*,\s*(\d+)\)/', 'rgb(\1,\2,\3)', $string);
    }

    protected function expandCSSEscape($string)
    {
        // flexibly parse it
        $ret = '';
        for ($i = 0, $c = strlen($string); $i < $c; $i++) {
            if ($string[$i] === '\\') {
                $i++;
                if ($i >= $c) {
                    $ret .= '\\';
                    break;
                }
                if (ctype_xdigit($string[$i])) {
                    $code = $string[$i];
                    for ($a = 1, $i++; $i < $c && $a < 6; $i++, $a++) {
                        if (!ctype_xdigit($string[$i])) {
                            break;
                        }
                        $code .= $string[$i];
                    }
                    // We have to be extremely careful when adding
                    // new characters, to make sure we're not breaking
                    // the encoding.
                    $char = HTMLPurifier_Encoder::unichr(hexdec($code));
                    if (HTMLPurifier_Encoder::cleanUTF8($char) === '') {
                        continue;
                    }
                    $ret .= $char;
                    if ($i < $c && trim($string[$i]) !== '') {
                        $i--;
                    }
                    continue;
                }
                if ($string[$i] === "\n") {
                    continue;
                }
            }
            $ret .= $string[$i];
        }
        return $ret;
    }
}

// vim: et sw=4 sts=4
