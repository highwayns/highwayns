<?php

class HTMLPurifier_ChildDef_Custom extends HTMLPurifier_ChildDef
{
    public $type = 'custom';

    public $allow_empty = false;

    public $dtd_regex;

    private $_pcre_regex;

    public function __construct($dtd_regex)
    {
        $this->dtd_regex = $dtd_regex;
        $this->_compileRegex();
    }

    protected function _compileRegex()
    {
        $raw = str_replace(' ', '', $this->dtd_regex);
        if ($raw{0} != '(') {
            $raw = "($raw)";
        }
        $el = '[#a-zA-Z0-9_.-]+';
        $reg = $raw;

        // COMPLICATED! AND MIGHT BE BUGGY! I HAVE NO CLUE WHAT I'M
        // DOING! Seriously: if there's problems, please report them.

        // collect all elements into the $elements array
        preg_match_all("/$el/", $reg, $matches);
        foreach ($matches[0] as $match) {
            $this->elements[$match] = true;
        }

        // setup all elements as parentheticals with leading commas
        $reg = preg_replace("/$el/", '(,\\0)', $reg);

        // remove commas when they were not solicited
        $reg = preg_replace("/([^,(|]\(+),/", '\\1', $reg);

        // remove all non-paranthetical commas: they are handled by first regex
        $reg = preg_replace("/,\(/", '(', $reg);

        $this->_pcre_regex = $reg;
    }

    public function validateChildren($children, $config, $context)
    {
        $list_of_children = '';
        $nesting = 0; // depth into the nest
        foreach ($children as $node) {
            if (!empty($node->is_whitespace)) {
                continue;
            }
            $list_of_children .= $node->name . ',';
        }
        // add leading comma to deal with stray comma declarations
        $list_of_children = ',' . rtrim($list_of_children, ',');
        $okay =
            preg_match(
                '/^,?' . $this->_pcre_regex . '$/',
                $list_of_children
            );
        return (bool)$okay;
    }
}

// vim: et sw=4 sts=4
