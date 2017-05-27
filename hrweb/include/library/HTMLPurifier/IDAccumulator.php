<?php

class HTMLPurifier_IDAccumulator
{

    public $ids = array();

    public static function build($config, $context)
    {
        $id_accumulator = new HTMLPurifier_IDAccumulator();
        $id_accumulator->load($config->get('Attr.IDBlacklist'));
        return $id_accumulator;
    }

    public function add($id)
    {
        if (isset($this->ids[$id])) {
            return false;
        }
        return $this->ids[$id] = true;
    }

    public function load($array_of_ids)
    {
        foreach ($array_of_ids as $id) {
            $this->ids[$id] = true;
        }
    }
}

// vim: et sw=4 sts=4
