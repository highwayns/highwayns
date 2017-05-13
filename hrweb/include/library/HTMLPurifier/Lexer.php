<?php

class HTMLPurifier_Lexer
{

    public $tracksLineNumbers = false;

    // -- STATIC ----------------------------------------------------------

    public static function create($config)
    {
        if (!($config instanceof HTMLPurifier_Config)) {
            $lexer = $config;
            trigger_error(
                "Passing a prototype to
                HTMLPurifier_Lexer::create() is deprecated, please instead
                use %Core.LexerImpl",
                E_USER_WARNING
            );
        } else {
            $lexer = $config->get('Core.LexerImpl');
        }

        $needs_tracking =
            $config->get('Core.MaintainLineNumbers') ||
            $config->get('Core.CollectErrors');

        $inst = null;
        if (is_object($lexer)) {
            $inst = $lexer;
        } else {
            if (is_null($lexer)) {
                do {
                    // auto-detection algorithm
                    if ($needs_tracking) {
                        $lexer = 'DirectLex';
                        break;
                    }

                    if (class_exists('DOMDocument') &&
                        method_exists('DOMDocument', 'loadHTML') &&
                        !extension_loaded('domxml')
                    ) {
                        // check for DOM support, because while it's part of the
                        // core, it can be disabled compile time. Also, the PECL
                        // domxml extension overrides the default DOM, and is evil
                        // and nasty and we shan't bother to support it
                        $lexer = 'DOMLex';
                    } else {
                        $lexer = 'DirectLex';
                    }
                } while (0);
            } // do..while so we can break

            // instantiate recognized string names
            switch ($lexer) {
                case 'DOMLex':
                    $inst = new HTMLPurifier_Lexer_DOMLex();
                    break;
                case 'DirectLex':
                    $inst = new HTMLPurifier_Lexer_DirectLex();
                    break;
                case 'PH5P':
                    $inst = new HTMLPurifier_Lexer_PH5P();
                    break;
                default:
                    throw new HTMLPurifier_Exception(
                        "Cannot instantiate unrecognized Lexer type " .
                        htmlspecialchars($lexer)
                    );
            }
        }

        if (!$inst) {
            throw new HTMLPurifier_Exception('No lexer was instantiated');
        }

        // once PHP DOM implements native line numbers, or we
        // hack out something using XSLT, remove this stipulation
        if ($needs_tracking && !$inst->tracksLineNumbers) {
            throw new HTMLPurifier_Exception(
                'Cannot use lexer that does not support line numbers with ' .
                'Core.MaintainLineNumbers or Core.CollectErrors (use DirectLex instead)'
            );
        }

        return $inst;

    }

    // -- CONVENIENCE MEMBERS ---------------------------------------------

    public function __construct()
    {
        $this->_entity_parser = new HTMLPurifier_EntityParser();
    }

    protected $_special_entity2str =
        array(
            '&quot;' => '"',
            '&amp;' => '&',
            '&lt;' => '<',
            '&gt;' => '>',
            '&#39;' => "'",
            '&#039;' => "'",
            '&#x27;' => "'"
        );

    public function parseData($string)
    {
        // following functions require at least one character
        if ($string === '') {
            return '';
        }

        // subtracts amps that cannot possibly be escaped
        $num_amp = substr_count($string, '&') - substr_count($string, '& ') -
            ($string[strlen($string) - 1] === '&' ? 1 : 0);

        if (!$num_amp) {
            return $string;
        } // abort if no entities
        $num_esc_amp = substr_count($string, '&amp;');
        $string = strtr($string, $this->_special_entity2str);

        // code duplication for sake of optimization, see above
        $num_amp_2 = substr_count($string, '&') - substr_count($string, '& ') -
            ($string[strlen($string) - 1] === '&' ? 1 : 0);

        if ($num_amp_2 <= $num_esc_amp) {
            return $string;
        }

        // hmm... now we have some uncommon entities. Use the callback.
        $string = $this->_entity_parser->substituteSpecialEntities($string);
        return $string;
    }

    public function tokenizeHTML($string, $config, $context)
    {
        trigger_error('Call to abstract class', E_USER_ERROR);
    }

    protected static function escapeCDATA($string)
    {
        return preg_replace_callback(
            '/<!\[CDATA\[(.+?)\]\]>/s',
            array('HTMLPurifier_Lexer', 'CDATACallback'),
            $string
        );
    }

    protected static function escapeCommentedCDATA($string)
    {
        return preg_replace_callback(
            '#<!--//--><!\[CDATA\[//><!--(.+?)//--><!\]\]>#s',
            array('HTMLPurifier_Lexer', 'CDATACallback'),
            $string
        );
    }

    protected static function removeIEConditional($string)
    {
        return preg_replace(
            '#<!--\[if [^>]+\]>.*?<!\[endif\]-->#si', // probably should generalize for all strings
            '',
            $string
        );
    }

    protected static function CDATACallback($matches)
    {
        // not exactly sure why the character set is needed, but whatever
        return htmlspecialchars($matches[1], ENT_COMPAT, 'UTF-8');
    }

    public function normalize($html, $config, $context)
    {
        // normalize newlines to \n
        if ($config->get('Core.NormalizeNewlines')) {
            $html = str_replace("\r\n", "\n", $html);
            $html = str_replace("\r", "\n", $html);
        }

        if ($config->get('HTML.Trusted')) {
            // escape convoluted CDATA
            $html = $this->escapeCommentedCDATA($html);
        }

        // escape CDATA
        $html = $this->escapeCDATA($html);

        $html = $this->removeIEConditional($html);

        // extract body from document if applicable
        if ($config->get('Core.ConvertDocumentToFragment')) {
            $e = false;
            if ($config->get('Core.CollectErrors')) {
                $e =& $context->get('ErrorCollector');
            }
            $new_html = $this->extractBody($html);
            if ($e && $new_html != $html) {
                $e->send(E_WARNING, 'Lexer: Extracted body');
            }
            $html = $new_html;
        }

        // expand entities that aren't the big five
        $html = $this->_entity_parser->substituteNonSpecialEntities($html);

        // clean into wellformed UTF-8 string for an SGML context: this has
        // to be done after entity expansion because the entities sometimes
        // represent non-SGML characters (horror, horror!)
        $html = HTMLPurifier_Encoder::cleanUTF8($html);

        // if processing instructions are to removed, remove them now
        if ($config->get('Core.RemoveProcessingInstructions')) {
            $html = preg_replace('#<\?.+?\?>#s', '', $html);
        }

        return $html;
    }

    public function extractBody($html)
    {
        $matches = array();
        $result = preg_match('!<body[^>]*>(.*)</body>!is', $html, $matches);
        if ($result) {
            return $matches[1];
        } else {
            return $html;
        }
    }
}

// vim: et sw=4 sts=4
