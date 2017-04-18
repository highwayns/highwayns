<?php
function tpl_compiler_tplheader($arguments, &$tpl)
{
    return "\necho '" . $tpl->_file . " compiled at " . date('Y-m-d H:M'). "';";
}
?>
