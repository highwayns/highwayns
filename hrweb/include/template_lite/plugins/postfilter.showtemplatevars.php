<?php
 function template_postfilter_showtemplatevars($compiled, &$template_object)
 {
     $compiled = "<pre>\n<?php print_r(\$this->_vars); ?>\n</pre>" . $compiled;
     return $compiled;
 }
?>
