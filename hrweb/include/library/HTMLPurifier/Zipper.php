<?php


class HTMLPurifier_Zipper
{
    public $front, $back;

    public function __construct($front, $back) {
        $this->front = $front;
        $this->back = $back;
    }

    static public function fromArray($array) {
        $z = new self(array(), array_reverse($array));
        $t = $z->delete(); // delete the "dummy hole"
        return array($z, $t);
    }

    public function toArray($t = NULL) {
        $a = $this->front;
        if ($t !== NULL) $a[] = $t;
        for ($i = count($this->back)-1; $i >= 0; $i--) {
            $a[] = $this->back[$i];
        }
        return $a;
    }

    public function next($t) {
        if ($t !== NULL) array_push($this->front, $t);
        return empty($this->back) ? NULL : array_pop($this->back);
    }

    public function advance($t, $n) {
        for ($i = 0; $i < $n; $i++) {
            $t = $this->next($t);
        }
        return $t;
    }

    public function prev($t) {
        if ($t !== NULL) array_push($this->back, $t);
        return empty($this->front) ? NULL : array_pop($this->front);
    }

    public function delete() {
        return empty($this->back) ? NULL : array_pop($this->back);
    }

    public function done() {
        return empty($this->back);
    }

    public function insertBefore($t) {
        if ($t !== NULL) array_push($this->front, $t);
    }

    public function insertAfter($t) {
        if ($t !== NULL) array_push($this->back, $t);
    }

    public function splice($t, $delete, $replacement) {
        // delete
        $old = array();
        $r = $t;
        for ($i = $delete; $i > 0; $i--) {
            $old[] = $r;
            $r = $this->delete();
        }
        // insert
        for ($i = count($replacement)-1; $i >= 0; $i--) {
            $this->insertAfter($r);
            $r = $replacement[$i];
        }
        return array($old, $r);
    }
}
