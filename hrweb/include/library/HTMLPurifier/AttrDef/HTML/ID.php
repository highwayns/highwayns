<?php


class HTMLPurifier_AttrDef_HTML_ID extends HTMLPurifier_AttrDef
{

    // selector is NOT a valid thing to use for IDREFs, because IDREFs
    // *must* target IDs that exist, whereas selector #ids do not.

    protected $selector;

    public function __construct($selector = false)
    {
        $this->selector = $selector;
    }

    public function validate($id, $config, $context)
    {
        if (!$this->selector && !$config->get('Attr.EnableID')) {
            return false;
        }

        $id = trim($id); // trim it first

        if ($id === '') {
            return false;
        }

        $prefix = $config->get('Attr.IDPrefix');
        if ($prefix !== '') {
            $prefix .= $config->get('Attr.IDPrefixLocal');
            // prevent re-appending the prefix
            if (strpos($id, $prefix) !== 0) {
                $id = $prefix . $id;
            }
        } elseif ($config->get('Attr.IDPrefixLocal') !== '') {
            trigger_error(
                '%Attr.IDPrefixLocal cannot be used unless ' .
                '%Attr.IDPrefix is set',
                E_USER_WARNING
            );
        }

        if (!$this->selector) {
            $id_accumulator =& $context->get('IDAccumulator');
            if (isset($id_accumulator->ids[$id])) {
                return false;
            }
        }

        // we purposely avoid using regex, hopefully this is faster

        if (ctype_alpha($id)) {
            $result = true;
        } else {
            if (!ctype_alpha(@$id[0])) {
                return false;
            }
            // primitive style of regexps, I suppose
            $trim = trim(
                $id,
                'A..Za..z0..9:-._'
            );
            $result = ($trim === '');
        }

        $regexp = $config->get('Attr.IDBlacklistRegexp');
        if ($regexp && preg_match($regexp, $id)) {
            return false;
        }

        if (!$this->selector && $result) {
            $id_accumulator->add($id);
        }

        // if no change was made to the ID, return the result
        // else, return the new id if stripping whitespace made it
        //     valid, or return false.
        return $result ? $id : false;
    }
}

// vim: et sw=4 sts=4
