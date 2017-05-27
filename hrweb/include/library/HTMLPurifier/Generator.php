<?php

class HTMLPurifier_Generator
{

    private $_xhtml = true;

    private $_scriptFix = false;

    private $_def;

    private $_sortAttr;

    private $_flashCompat;

    private $_innerHTMLFix;

    private $_flashStack = array();

    protected $config;

    public function __construct($config, $context)
    {
        $this->config = $config;
        $this->_scriptFix = $config->get('Output.CommentScriptContents');
        $this->_innerHTMLFix = $config->get('Output.FixInnerHTML');
        $this->_sortAttr = $config->get('Output.SortAttr');
        $this->_flashCompat = $config->get('Output.FlashCompat');
        $this->_def = $config->getHTMLDefinition();
        $this->_xhtml = $this->_def->doctype->xml;
    }

    public function generateFromTokens($tokens)
    {
        if (!$tokens) {
            return '';
        }

        // Basic algorithm
        $html = '';
        for ($i = 0, $size = count($tokens); $i < $size; $i++) {
            if ($this->_scriptFix && $tokens[$i]->name === 'script'
                && $i + 2 < $size && $tokens[$i+2] instanceof HTMLPurifier_Token_End) {
                // script special case
                // the contents of the script block must be ONE token
                // for this to work.
                $html .= $this->generateFromToken($tokens[$i++]);
                $html .= $this->generateScriptFromToken($tokens[$i++]);
            }
            $html .= $this->generateFromToken($tokens[$i]);
        }

        // Tidy cleanup
        if (extension_loaded('tidy') && $this->config->get('Output.TidyFormat')) {
            $tidy = new Tidy;
            $tidy->parseString(
                $html,
                array(
                   'indent'=> true,
                   'output-xhtml' => $this->_xhtml,
                   'show-body-only' => true,
                   'indent-spaces' => 2,
                   'wrap' => 68,
                ),
                'utf8'
            );
            $tidy->cleanRepair();
            $html = (string) $tidy; // explicit cast necessary
        }

        // Normalize newlines to system defined value
        if ($this->config->get('Core.NormalizeNewlines')) {
            $nl = $this->config->get('Output.Newline');
            if ($nl === null) {
                $nl = PHP_EOL;
            }
            if ($nl !== "\n") {
                $html = str_replace("\n", $nl, $html);
            }
        }
        return $html;
    }

    public function generateFromToken($token)
    {
        if (!$token instanceof HTMLPurifier_Token) {
            trigger_error('Cannot generate HTML from non-HTMLPurifier_Token object', E_USER_WARNING);
            return '';

        } elseif ($token instanceof HTMLPurifier_Token_Start) {
            $attr = $this->generateAttributes($token->attr, $token->name);
            if ($this->_flashCompat) {
                if ($token->name == "object") {
                    $flash = new stdclass();
                    $flash->attr = $token->attr;
                    $flash->param = array();
                    $this->_flashStack[] = $flash;
                }
            }
            return '<' . $token->name . ($attr ? ' ' : '') . $attr . '>';

        } elseif ($token instanceof HTMLPurifier_Token_End) {
            $_extra = '';
            if ($this->_flashCompat) {
                if ($token->name == "object" && !empty($this->_flashStack)) {
                    // doesn't do anything for now
                }
            }
            return $_extra . '</' . $token->name . '>';

        } elseif ($token instanceof HTMLPurifier_Token_Empty) {
            if ($this->_flashCompat && $token->name == "param" && !empty($this->_flashStack)) {
                $this->_flashStack[count($this->_flashStack)-1]->param[$token->attr['name']] = $token->attr['value'];
            }
            $attr = $this->generateAttributes($token->attr, $token->name);
             return '<' . $token->name . ($attr ? ' ' : '') . $attr .
                ( $this->_xhtml ? ' /': '' ) // <br /> v. <br>
                . '>';

        } elseif ($token instanceof HTMLPurifier_Token_Text) {
            return $this->escape($token->data, ENT_NOQUOTES);

        } elseif ($token instanceof HTMLPurifier_Token_Comment) {
            return '<!--' . $token->data . '-->';
        } else {
            return '';

        }
    }

    public function generateScriptFromToken($token)
    {
        if (!$token instanceof HTMLPurifier_Token_Text) {
            return $this->generateFromToken($token);
        }
        // Thanks <http://lachy.id.au/log/2005/05/script-comments>
        $data = preg_replace('#//\s*$#', '', $token->data);
        return '<!--//--><![CDATA[//><!--' . "\n" . trim($data) . "\n" . '//--><!]]>';
    }

    public function generateAttributes($assoc_array_of_attributes, $element = '')
    {
        $html = '';
        if ($this->_sortAttr) {
            ksort($assoc_array_of_attributes);
        }
        foreach ($assoc_array_of_attributes as $key => $value) {
            if (!$this->_xhtml) {
                // Remove namespaced attributes
                if (strpos($key, ':') !== false) {
                    continue;
                }
                // Check if we should minimize the attribute: val="val" -> val
                if ($element && !empty($this->_def->info[$element]->attr[$key]->minimized)) {
                    $html .= $key . ' ';
                    continue;
                }
            }
            // Workaround for Internet Explorer innerHTML bug.
            // Essentially, Internet Explorer, when calculating
            // innerHTML, omits quotes if there are no instances of
            // angled brackets, quotes or spaces.  However, when parsing
            // HTML (for example, when you assign to innerHTML), it
            // treats backticks as quotes.  Thus,
            //      <img alt="``" />
            // becomes
            //      <img alt=`` />
            // becomes
            //      <img alt='' />
            // Fortunately, all we need to do is trigger an appropriate
            // quoting style, which we do by adding an extra space.
            // This also is consistent with the W3C spec, which states
            // that user agents may ignore leading or trailing
            // whitespace (in fact, most don't, at least for attributes
            // like alt, but an extra space at the end is barely
            // noticeable).  Still, we have a configuration knob for
            // this, since this transformation is not necesary if you
            // don't process user input with innerHTML or you don't plan
            // on supporting Internet Explorer.
            if ($this->_innerHTMLFix) {
                if (strpos($value, '`') !== false) {
                    // check if correct quoting style would not already be
                    // triggered
                    if (strcspn($value, '"\' <>') === strlen($value)) {
                        // protect!
                        $value .= ' ';
                    }
                }
            }
            $html .= $key.'="'.$this->escape($value).'" ';
        }
        return rtrim($html);
    }

    public function escape($string, $quote = null)
    {
        // Workaround for APC bug on Mac Leopard reported by sidepodcast
        // http://htmlpurifier.org/phorum/read.php?3,4823,4846
        if ($quote === null) {
            $quote = ENT_COMPAT;
        }
        return htmlspecialchars($string, $quote, 'UTF-8');
    }
}

// vim: et sw=4 sts=4
