<?php
 
 function template_prefilter_showinfoheader($tpl_source, &$template_object)
 {
	return '<!-- Template Lite '.$template_object->_version.' '.strftime("%Y-%m-%d %H:%M %Z").' -->'."\n\n".$tpl_source; 
 }
?>
