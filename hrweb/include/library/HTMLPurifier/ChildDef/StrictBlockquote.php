<?php

class HTMLPurifier_ChildDef_StrictBlockquote extends HTMLPurifier_ChildDef_Required
{
    protected $real_elements;

    protected $fake_elements;

    public $allow_empty = true;

    public $type = 'strictblockquote';

    protected $init = false;

    public function getAllowedElements($config)
    {
        $this->init($config);
        return $this->fake_elements;
    }

    public function validateChildren($children, $config, $context)
    {
        $this->init($config);

        // trick the parent class into thinking it allows more
        $this->elements = $this->fake_elements;
        $result = parent::validateChildren($children, $config, $context);
        $this->elements = $this->real_elements;

        if ($result === false) {
            return array();
        }
        if ($result === true) {
            $result = $children;
        }

        $def = $config->getHTMLDefinition();
        $block_wrap_name = $def->info_block_wrapper;
        $block_wrap = false;
        $ret = array();

        foreach ($result as $node) {
            if ($block_wrap === false) {
                if (($node instanceof HTMLPurifier_Node_Text && !$node->is_whitespace) ||
                    ($node instanceof HTMLPurifier_Node_Element && !isset($this->elements[$node->name]))) {
                        $block_wrap = new HTMLPurifier_Node_Element($def->info_block_wrapper);
                        $ret[] = $block_wrap;
                }
            } else {
                if ($node instanceof HTMLPurifier_Node_Element && isset($this->elements[$node->name])) {
                    $block_wrap = false;

                }
            }
            if ($block_wrap) {
                $block_wrap->children[] = $node;
            } else {
                $ret[] = $node;
            }
        }
        return $ret;
    }

    private function init($config)
    {
        if (!$this->init) {
            $def = $config->getHTMLDefinition();
            // allow all inline elements
            $this->real_elements = $this->elements;
            $this->fake_elements = $def->info_content_sets['Flow'];
            $this->fake_elements['#PCDATA'] = true;
            $this->init = true;
        }
    }
}

// vim: et sw=4 sts=4
