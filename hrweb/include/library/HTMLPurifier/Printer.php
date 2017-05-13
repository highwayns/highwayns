<?php

// OUT OF DATE, NEEDS UPDATING!
// USE XMLWRITER!

class HTMLPurifier_Printer
{

    protected $generator;

    protected $config;

    public function __construct()
    {
    }

    public function prepareGenerator($config)
    {
        $all = $config->getAll();
        $context = new HTMLPurifier_Context();
        $this->generator = new HTMLPurifier_Generator($config, $context);
    }

    // function render() {}

    protected function start($tag, $attr = array())
    {
        return $this->generator->generateFromToken(
            new HTMLPurifier_Token_Start($tag, $attr ? $attr : array())
        );
    }

    protected function end($tag)
    {
        return $this->generator->generateFromToken(
            new HTMLPurifier_Token_End($tag)
        );
    }

    protected function element($tag, $contents, $attr = array(), $escape = true)
    {
        return $this->start($tag, $attr) .
            ($escape ? $this->escape($contents) : $contents) .
            $this->end($tag);
    }

    protected function elementEmpty($tag, $attr = array())
    {
        return $this->generator->generateFromToken(
            new HTMLPurifier_Token_Empty($tag, $attr)
        );
    }

    protected function text($text)
    {
        return $this->generator->generateFromToken(
            new HTMLPurifier_Token_Text($text)
        );
    }

    protected function row($name, $value)
    {
        if (is_bool($value)) {
            $value = $value ? 'On' : 'Off';
        }
        return
            $this->start('tr') . "\n" .
            $this->element('th', $name) . "\n" .
            $this->element('td', $value) . "\n" .
            $this->end('tr');
    }

    protected function escape($string)
    {
        $string = HTMLPurifier_Encoder::cleanUTF8($string);
        $string = htmlspecialchars($string, ENT_COMPAT, 'UTF-8');
        return $string;
    }

    protected function listify($array, $polite = false)
    {
        if (empty($array)) {
            return 'None';
        }
        $ret = '';
        $i = count($array);
        foreach ($array as $value) {
            $i--;
            $ret .= $value;
            if ($i > 0 && !($polite && $i == 1)) {
                $ret .= ', ';
            }
            if ($polite && $i == 1) {
                $ret .= 'and ';
            }
        }
        return $ret;
    }

    protected function getClass($obj, $sec_prefix = '')
    {
        static $five = null;
        if ($five === null) {
            $five = version_compare(PHP_VERSION, '5', '>=');
        }
        $prefix = 'HTMLPurifier_' . $sec_prefix;
        if (!$five) {
            $prefix = strtolower($prefix);
        }
        $class = str_replace($prefix, '', get_class($obj));
        $lclass = strtolower($class);
        $class .= '(';
        switch ($lclass) {
            case 'enum':
                $values = array();
                foreach ($obj->valid_values as $value => $bool) {
                    $values[] = $value;
                }
                $class .= implode(', ', $values);
                break;
            case 'css_composite':
                $values = array();
                foreach ($obj->defs as $def) {
                    $values[] = $this->getClass($def, $sec_prefix);
                }
                $class .= implode(', ', $values);
                break;
            case 'css_multiple':
                $class .= $this->getClass($obj->single, $sec_prefix) . ', ';
                $class .= $obj->max;
                break;
            case 'css_denyelementdecorator':
                $class .= $this->getClass($obj->def, $sec_prefix) . ', ';
                $class .= $obj->element;
                break;
            case 'css_importantdecorator':
                $class .= $this->getClass($obj->def, $sec_prefix);
                if ($obj->allow) {
                    $class .= ', !important';
                }
                break;
        }
        $class .= ')';
        return $class;
    }
}

// vim: et sw=4 sts=4
