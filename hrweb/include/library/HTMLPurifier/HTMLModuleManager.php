<?php

class HTMLPurifier_HTMLModuleManager
{

    public $doctypes;

    public $doctype;

    public $attrTypes;

    public $modules = array();

    public $registeredModules = array();

    public $userModules = array();

    public $elementLookup = array();

    public $prefixes = array('HTMLPurifier_HTMLModule_');

    public $contentSets;

    public $attrCollections;

    public $trusted = false;

    public function __construct()
    {
        // editable internal objects
        $this->attrTypes = new HTMLPurifier_AttrTypes();
        $this->doctypes  = new HTMLPurifier_DoctypeRegistry();

        // setup basic modules
        $common = array(
            'CommonAttributes', 'Text', 'Hypertext', 'List',
            'Presentation', 'Edit', 'Bdo', 'Tables', 'Image',
            'StyleAttribute',
            // Unsafe:
            'Scripting', 'Object', 'Forms',
            // Sorta legacy, but present in strict:
            'Name',
        );
        $transitional = array('Legacy', 'Target', 'Iframe');
        $xml = array('XMLCommonAttributes');
        $non_xml = array('NonXMLCommonAttributes');

        // setup basic doctypes
        $this->doctypes->register(
            'HTML 4.01 Transitional',
            false,
            array_merge($common, $transitional, $non_xml),
            array('Tidy_Transitional', 'Tidy_Proprietary'),
            array(),
            '-//W3C//DTD HTML 4.01 Transitional//EN',
            'http://www.w3.org/TR/html4/loose.dtd'
        );

        $this->doctypes->register(
            'HTML 4.01 Strict',
            false,
            array_merge($common, $non_xml),
            array('Tidy_Strict', 'Tidy_Proprietary', 'Tidy_Name'),
            array(),
            '-//W3C//DTD HTML 4.01//EN',
            'http://www.w3.org/TR/html4/strict.dtd'
        );

        $this->doctypes->register(
            'XHTML 1.0 Transitional',
            true,
            array_merge($common, $transitional, $xml, $non_xml),
            array('Tidy_Transitional', 'Tidy_XHTML', 'Tidy_Proprietary', 'Tidy_Name'),
            array(),
            '-//W3C//DTD XHTML 1.0 Transitional//EN',
            'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'
        );

        $this->doctypes->register(
            'XHTML 1.0 Strict',
            true,
            array_merge($common, $xml, $non_xml),
            array('Tidy_Strict', 'Tidy_XHTML', 'Tidy_Strict', 'Tidy_Proprietary', 'Tidy_Name'),
            array(),
            '-//W3C//DTD XHTML 1.0 Strict//EN',
            'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'
        );

        $this->doctypes->register(
            'XHTML 1.1',
            true,
            // Iframe is a real XHTML 1.1 module, despite being
            // "transitional"!
            array_merge($common, $xml, array('Ruby', 'Iframe')),
            array('Tidy_Strict', 'Tidy_XHTML', 'Tidy_Proprietary', 'Tidy_Strict', 'Tidy_Name'), // Tidy_XHTML1_1
            array(),
            '-//W3C//DTD XHTML 1.1//EN',
            'http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd'
        );

    }

    public function registerModule($module, $overload = false)
    {
        if (is_string($module)) {
            // attempt to load the module
            $original_module = $module;
            $ok = false;
            foreach ($this->prefixes as $prefix) {
                $module = $prefix . $original_module;
                if (class_exists($module)) {
                    $ok = true;
                    break;
                }
            }
            if (!$ok) {
                $module = $original_module;
                if (!class_exists($module)) {
                    trigger_error(
                        $original_module . ' module does not exist',
                        E_USER_ERROR
                    );
                    return;
                }
            }
            $module = new $module();
        }
        if (empty($module->name)) {
            trigger_error('Module instance of ' . get_class($module) . ' must have name');
            return;
        }
        if (!$overload && isset($this->registeredModules[$module->name])) {
            trigger_error('Overloading ' . $module->name . ' without explicit overload parameter', E_USER_WARNING);
        }
        $this->registeredModules[$module->name] = $module;
    }

    public function addModule($module)
    {
        $this->registerModule($module);
        if (is_object($module)) {
            $module = $module->name;
        }
        $this->userModules[] = $module;
    }

    public function addPrefix($prefix)
    {
        $this->prefixes[] = $prefix;
    }

    public function setup($config)
    {
        $this->trusted = $config->get('HTML.Trusted');

        // generate
        $this->doctype = $this->doctypes->make($config);
        $modules = $this->doctype->modules;

        // take out the default modules that aren't allowed
        $lookup = $config->get('HTML.AllowedModules');
        $special_cases = $config->get('HTML.CoreModules');

        if (is_array($lookup)) {
            foreach ($modules as $k => $m) {
                if (isset($special_cases[$m])) {
                    continue;
                }
                if (!isset($lookup[$m])) {
                    unset($modules[$k]);
                }
            }
        }

        // custom modules
        if ($config->get('HTML.Proprietary')) {
            $modules[] = 'Proprietary';
        }
        if ($config->get('HTML.SafeObject')) {
            $modules[] = 'SafeObject';
        }
        if ($config->get('HTML.SafeEmbed')) {
            $modules[] = 'SafeEmbed';
        }
        if ($config->get('HTML.SafeScripting') !== array()) {
            $modules[] = 'SafeScripting';
        }
        if ($config->get('HTML.Nofollow')) {
            $modules[] = 'Nofollow';
        }
        if ($config->get('HTML.TargetBlank')) {
            $modules[] = 'TargetBlank';
        }

        // merge in custom modules
        $modules = array_merge($modules, $this->userModules);

        foreach ($modules as $module) {
            $this->processModule($module);
            $this->modules[$module]->setup($config);
        }

        foreach ($this->doctype->tidyModules as $module) {
            $this->processModule($module);
            $this->modules[$module]->setup($config);
        }

        // prepare any injectors
        foreach ($this->modules as $module) {
            $n = array();
            foreach ($module->info_injector as $injector) {
                if (!is_object($injector)) {
                    $class = "HTMLPurifier_Injector_$injector";
                    $injector = new $class;
                }
                $n[$injector->name] = $injector;
            }
            $module->info_injector = $n;
        }

        // setup lookup table based on all valid modules
        foreach ($this->modules as $module) {
            foreach ($module->info as $name => $def) {
                if (!isset($this->elementLookup[$name])) {
                    $this->elementLookup[$name] = array();
                }
                $this->elementLookup[$name][] = $module->name;
            }
        }

        // note the different choice
        $this->contentSets = new HTMLPurifier_ContentSets(
            // content set assembly deals with all possible modules,
            // not just ones deemed to be "safe"
            $this->modules
        );
        $this->attrCollections = new HTMLPurifier_AttrCollections(
            $this->attrTypes,
            // there is no way to directly disable a global attribute,
            // but using AllowedAttributes or simply not including
            // the module in your custom doctype should be sufficient
            $this->modules
        );
    }

    public function processModule($module)
    {
        if (!isset($this->registeredModules[$module]) || is_object($module)) {
            $this->registerModule($module);
        }
        $this->modules[$module] = $this->registeredModules[$module];
    }

    public function getElements()
    {
        $elements = array();
        foreach ($this->modules as $module) {
            if (!$this->trusted && !$module->safe) {
                continue;
            }
            foreach ($module->info as $name => $v) {
                if (isset($elements[$name])) {
                    continue;
                }
                $elements[$name] = $this->getElement($name);
            }
        }

        // remove dud elements, this happens when an element that
        // appeared to be safe actually wasn't
        foreach ($elements as $n => $v) {
            if ($v === false) {
                unset($elements[$n]);
            }
        }

        return $elements;

    }

    public function getElement($name, $trusted = null)
    {
        if (!isset($this->elementLookup[$name])) {
            return false;
        }

        // setup global state variables
        $def = false;
        if ($trusted === null) {
            $trusted = $this->trusted;
        }

        // iterate through each module that has registered itself to this
        // element
        foreach ($this->elementLookup[$name] as $module_name) {
            $module = $this->modules[$module_name];

            // refuse to create/merge from a module that is deemed unsafe--
            // pretend the module doesn't exist--when trusted mode is not on.
            if (!$trusted && !$module->safe) {
                continue;
            }

            // clone is used because, ideally speaking, the original
            // definition should not be modified. Usually, this will
            // make no difference, but for consistency's sake
            $new_def = clone $module->info[$name];

            if (!$def && $new_def->standalone) {
                $def = $new_def;
            } elseif ($def) {
                // This will occur even if $new_def is standalone. In practice,
                // this will usually result in a full replacement.
                $def->mergeIn($new_def);
            } else {
                // :TODO:
                // non-standalone definitions that don't have a standalone
                // to merge into could be deferred to the end
                // HOWEVER, it is perfectly valid for a non-standalone
                // definition to lack a standalone definition, even
                // after all processing: this allows us to safely
                // specify extra attributes for elements that may not be
                // enabled all in one place.  In particular, this might
                // be the case for trusted elements.  WARNING: care must
                // be taken that the /extra/ definitions are all safe.
                continue;
            }

            // attribute value expansions
            $this->attrCollections->performInclusions($def->attr);
            $this->attrCollections->expandIdentifiers($def->attr, $this->attrTypes);

            // descendants_are_inline, for ChildDef_Chameleon
            if (is_string($def->content_model) &&
                strpos($def->content_model, 'Inline') !== false) {
                if ($name != 'del' && $name != 'ins') {
                    // this is for you, ins/del
                    $def->descendants_are_inline = true;
                }
            }

            $this->contentSets->generateChildDef($def, $module);
        }

        // This can occur if there is a blank definition, but no base to
        // mix it in with
        if (!$def) {
            return false;
        }

        // add information on required attributes
        foreach ($def->attr as $attr_name => $attr_def) {
            if ($attr_def->required) {
                $def->required_attr[] = $attr_name;
            }
        }
        return $def;
    }
}

// vim: et sw=4 sts=4
