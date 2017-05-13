<?php

abstract class HTMLPurifier_Injector
{

    public $name;

    protected $htmlDefinition;

    protected $currentNesting;

    protected $currentToken;

    protected $inputZipper;

    public $needed = array();

    protected $rewindOffset = false;

    public function rewindOffset($offset)
    {
        $this->rewindOffset = $offset;
    }

    public function getRewindOffset()
    {
        $r = $this->rewindOffset;
        $this->rewindOffset = false;
        return $r;
    }

    public function prepare($config, $context)
    {
        $this->htmlDefinition = $config->getHTMLDefinition();
        // Even though this might fail, some unit tests ignore this and
        // still test checkNeeded, so be careful. Maybe get rid of that
        // dependency.
        $result = $this->checkNeeded($config);
        if ($result !== false) {
            return $result;
        }
        $this->currentNesting =& $context->get('CurrentNesting');
        $this->currentToken   =& $context->get('CurrentToken');
        $this->inputZipper    =& $context->get('InputZipper');
        return false;
    }

    public function checkNeeded($config)
    {
        $def = $config->getHTMLDefinition();
        foreach ($this->needed as $element => $attributes) {
            if (is_int($element)) {
                $element = $attributes;
            }
            if (!isset($def->info[$element])) {
                return $element;
            }
            if (!is_array($attributes)) {
                continue;
            }
            foreach ($attributes as $name) {
                if (!isset($def->info[$element]->attr[$name])) {
                    return "$element.$name";
                }
            }
        }
        return false;
    }

    public function allowsElement($name)
    {
        if (!empty($this->currentNesting)) {
            $parent_token = array_pop($this->currentNesting);
            $this->currentNesting[] = $parent_token;
            $parent = $this->htmlDefinition->info[$parent_token->name];
        } else {
            $parent = $this->htmlDefinition->info_parent_def;
        }
        if (!isset($parent->child->elements[$name]) || isset($parent->excludes[$name])) {
            return false;
        }
        // check for exclusion
        for ($i = count($this->currentNesting) - 2; $i >= 0; $i--) {
            $node = $this->currentNesting[$i];
            $def  = $this->htmlDefinition->info[$node->name];
            if (isset($def->excludes[$name])) {
                return false;
            }
        }
        return true;
    }

    protected function forward(&$i, &$current)
    {
        if ($i === null) {
            $i = count($this->inputZipper->back) - 1;
        } else {
            $i--;
        }
        if ($i < 0) {
            return false;
        }
        $current = $this->inputZipper->back[$i];
        return true;
    }

    protected function forwardUntilEndToken(&$i, &$current, &$nesting)
    {
        $result = $this->forward($i, $current);
        if (!$result) {
            return false;
        }
        if ($nesting === null) {
            $nesting = 0;
        }
        if ($current instanceof HTMLPurifier_Token_Start) {
            $nesting++;
        } elseif ($current instanceof HTMLPurifier_Token_End) {
            if ($nesting <= 0) {
                return false;
            }
            $nesting--;
        }
        return true;
    }

    protected function backward(&$i, &$current)
    {
        if ($i === null) {
            $i = count($this->inputZipper->front) - 1;
        } else {
            $i--;
        }
        if ($i < 0) {
            return false;
        }
        $current = $this->inputZipper->front[$i];
        return true;
    }

    public function handleText(&$token)
    {
    }

    public function handleElement(&$token)
    {
    }

    public function handleEnd(&$token)
    {
        $this->notifyEnd($token);
    }

    public function notifyEnd($token)
    {
    }
}

// vim: et sw=4 sts=4
