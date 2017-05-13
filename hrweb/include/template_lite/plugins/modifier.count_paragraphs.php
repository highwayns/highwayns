<?php


function tpl_modifier_count_paragraphs($string)
{
    // count \r or \n characters
    return count(preg_split('/[\r\n]+/', $string));
}

?>
