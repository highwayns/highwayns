<?php

class HTMLPurifier_Queue {
    private $input;
    private $output;

    public function __construct($input = array()) {
        $this->input = $input;
        $this->output = array();
    }

    public function shift() {
        if (empty($this->output)) {
            $this->output = array_reverse($this->input);
            $this->input = array();
        }
        if (empty($this->output)) {
            return NULL;
        }
        return array_pop($this->output);
    }

    public function push($x) {
        array_push($this->input, $x);
    }

    public function isEmpty() {
        return empty($this->input) && empty($this->output);
    }
}
