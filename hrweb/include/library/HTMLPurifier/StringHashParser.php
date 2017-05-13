<?php

class HTMLPurifier_StringHashParser
{

    public $default = 'ID';

    public function parseFile($file)
    {
        if (!file_exists($file)) {
            return false;
        }
        $fh = fopen($file, 'r');
        if (!$fh) {
            return false;
        }
        $ret = $this->parseHandle($fh);
        fclose($fh);
        return $ret;
    }

    public function parseMultiFile($file)
    {
        if (!file_exists($file)) {
            return false;
        }
        $ret = array();
        $fh = fopen($file, 'r');
        if (!$fh) {
            return false;
        }
        while (!feof($fh)) {
            $ret[] = $this->parseHandle($fh);
        }
        fclose($fh);
        return $ret;
    }

    protected function parseHandle($fh)
    {
        $state   = false;
        $single  = false;
        $ret     = array();
        do {
            $line = fgets($fh);
            if ($line === false) {
                break;
            }
            $line = rtrim($line, "\n\r");
            if (!$state && $line === '') {
                continue;
            }
            if ($line === '----') {
                break;
            }
            if (strncmp('--#', $line, 3) === 0) {
                // Comment
                continue;
            } elseif (strncmp('--', $line, 2) === 0) {
                // Multiline declaration
                $state = trim($line, '- ');
                if (!isset($ret[$state])) {
                    $ret[$state] = '';
                }
                continue;
            } elseif (!$state) {
                $single = true;
                if (strpos($line, ':') !== false) {
                    // Single-line declaration
                    list($state, $line) = explode(':', $line, 2);
                    $line = trim($line);
                } else {
                    // Use default declaration
                    $state  = $this->default;
                }
            }
            if ($single) {
                $ret[$state] = $line;
                $single = false;
                $state  = false;
            } else {
                $ret[$state] .= "$line\n";
            }
        } while (!feof($fh));
        return $ret;
    }
}

// vim: et sw=4 sts=4
