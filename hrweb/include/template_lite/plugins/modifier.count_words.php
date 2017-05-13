<?php


function tpl_modifier_count_words($string)
{
    // split text by ' ',\r,\n,\f,\t
    $split_array = preg_split('/\s+/',$string);
    // count matches that contain alphanumerics
    $word_count = preg_grep('/[a-zA-Z0-9\\x80-\\xff]/', $split_array);

    return count($word_count);
}

?>
