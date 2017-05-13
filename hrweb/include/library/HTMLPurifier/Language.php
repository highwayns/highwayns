<?php

class HTMLPurifier_Language
{

    public $code = 'en';

    public $fallback = false;

    public $messages = array();

    public $errorNames = array();

    public $error = false;

    public $_loaded = false;

    protected $config;

    protected $context;

    public function __construct($config, $context)
    {
        $this->config  = $config;
        $this->context = $context;
    }

    public function load()
    {
        if ($this->_loaded) {
            return;
        }
        $factory = HTMLPurifier_LanguageFactory::instance();
        $factory->loadLanguage($this->code);
        foreach ($factory->keys as $key) {
            $this->$key = $factory->cache[$this->code][$key];
        }
        $this->_loaded = true;
    }

    public function getMessage($key)
    {
        if (!$this->_loaded) {
            $this->load();
        }
        if (!isset($this->messages[$key])) {
            return "[$key]";
        }
        return $this->messages[$key];
    }

    public function getErrorName($int)
    {
        if (!$this->_loaded) {
            $this->load();
        }
        if (!isset($this->errorNames[$int])) {
            return "[Error: $int]";
        }
        return $this->errorNames[$int];
    }

    public function listify($array)
    {
        $sep      = $this->getMessage('Item separator');
        $sep_last = $this->getMessage('Item separator last');
        $ret = '';
        for ($i = 0, $c = count($array); $i < $c; $i++) {
            if ($i == 0) {
            } elseif ($i + 1 < $c) {
                $ret .= $sep;
            } else {
                $ret .= $sep_last;
            }
            $ret .= $array[$i];
        }
        return $ret;
    }

    public function formatMessage($key, $args = array())
    {
        if (!$this->_loaded) {
            $this->load();
        }
        if (!isset($this->messages[$key])) {
            return "[$key]";
        }
        $raw = $this->messages[$key];
        $subst = array();
        $generator = false;
        foreach ($args as $i => $value) {
            if (is_object($value)) {
                if ($value instanceof HTMLPurifier_Token) {
                    // factor this out some time
                    if (!$generator) {
                        $generator = $this->context->get('Generator');
                    }
                    if (isset($value->name)) {
                        $subst['$'.$i.'.Name'] = $value->name;
                    }
                    if (isset($value->data)) {
                        $subst['$'.$i.'.Data'] = $value->data;
                    }
                    $subst['$'.$i.'.Compact'] =
                    $subst['$'.$i.'.Serialized'] = $generator->generateFromToken($value);
                    // a more complex algorithm for compact representation
                    // could be introduced for all types of tokens. This
                    // may need to be factored out into a dedicated class
                    if (!empty($value->attr)) {
                        $stripped_token = clone $value;
                        $stripped_token->attr = array();
                        $subst['$'.$i.'.Compact'] = $generator->generateFromToken($stripped_token);
                    }
                    $subst['$'.$i.'.Line'] = $value->line ? $value->line : 'unknown';
                }
                continue;
            } elseif (is_array($value)) {
                $keys = array_keys($value);
                if (array_keys($keys) === $keys) {
                    // list
                    $subst['$'.$i] = $this->listify($value);
                } else {
                    // associative array
                    // no $i implementation yet, sorry
                    $subst['$'.$i.'.Keys'] = $this->listify($keys);
                    $subst['$'.$i.'.Values'] = $this->listify(array_values($value));
                }
                continue;
            }
            $subst['$' . $i] = $value;
        }
        return strtr($raw, $subst);
    }
}

// vim: et sw=4 sts=4
