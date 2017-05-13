<?php

class HTMLPurifier_Length
{

    protected $n;

    protected $unit;

    protected $isValid;

    protected static $allowedUnits = array(
        'em' => true, 'ex' => true, 'px' => true, 'in' => true,
        'cm' => true, 'mm' => true, 'pt' => true, 'pc' => true
    );

    public function __construct($n = '0', $u = false)
    {
        $this->n = (string) $n;
        $this->unit = $u !== false ? (string) $u : false;
    }

    public static function make($s)
    {
        if ($s instanceof HTMLPurifier_Length) {
            return $s;
        }
        $n_length = strspn($s, '1234567890.+-');
        $n = substr($s, 0, $n_length);
        $unit = substr($s, $n_length);
        if ($unit === '') {
            $unit = false;
        }
        return new HTMLPurifier_Length($n, $unit);
    }

    protected function validate()
    {
        // Special case:
        if ($this->n === '+0' || $this->n === '-0') {
            $this->n = '0';
        }
        if ($this->n === '0' && $this->unit === false) {
            return true;
        }
        if (!ctype_lower($this->unit)) {
            $this->unit = strtolower($this->unit);
        }
        if (!isset(HTMLPurifier_Length::$allowedUnits[$this->unit])) {
            return false;
        }
        // Hack:
        $def = new HTMLPurifier_AttrDef_CSS_Number();
        $result = $def->validate($this->n, false, false);
        if ($result === false) {
            return false;
        }
        $this->n = $result;
        return true;
    }

    public function toString()
    {
        if (!$this->isValid()) {
            return false;
        }
        return $this->n . $this->unit;
    }

    public function getN()
    {
        return $this->n;
    }

    public function getUnit()
    {
        return $this->unit;
    }

    public function isValid()
    {
        if ($this->isValid === null) {
            $this->isValid = $this->validate();
        }
        return $this->isValid;
    }

    public function compareTo($l)
    {
        if ($l === false) {
            return false;
        }
        if ($l->unit !== $this->unit) {
            $converter = new HTMLPurifier_UnitConverter();
            $l = $converter->convert($l, $this->unit);
            if ($l === false) {
                return false;
            }
        }
        return $this->n - $l->n;
    }
}

// vim: et sw=4 sts=4
