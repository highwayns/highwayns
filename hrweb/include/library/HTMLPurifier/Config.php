<?php

class HTMLPurifier_Config
{

    public $version = '4.6.0';

    public $autoFinalize = true;

    // protected member variables

    protected $serials = array();

    protected $serial;

    protected $parser = null;

    public $def;

    protected $definitions;

    protected $finalized = false;

    protected $plist;

    private $aliasMode;

    public $chatty = true;

    private $lock;

    public function __construct($definition, $parent = null)
    {
        $parent = $parent ? $parent : $definition->defaultPlist;
        $this->plist = new HTMLPurifier_PropertyList($parent);
        $this->def = $definition; // keep a copy around for checking
        $this->parser = new HTMLPurifier_VarParser_Flexible();
    }

    public static function create($config, $schema = null)
    {
        if ($config instanceof HTMLPurifier_Config) {
            // pass-through
            return $config;
        }
        if (!$schema) {
            $ret = HTMLPurifier_Config::createDefault();
        } else {
            $ret = new HTMLPurifier_Config($schema);
        }
        if (is_string($config)) {
            $ret->loadIni($config);
        } elseif (is_array($config)) $ret->loadArray($config);
        return $ret;
    }

    public static function inherit(HTMLPurifier_Config $config)
    {
        return new HTMLPurifier_Config($config->def, $config->plist);
    }

    public static function createDefault()
    {
        $definition = HTMLPurifier_ConfigSchema::instance();
        $config = new HTMLPurifier_Config($definition);
        return $config;
    }

    public function get($key, $a = null)
    {
        if ($a !== null) {
            $this->triggerError(
                "Using deprecated API: use \$config->get('$key.$a') instead",
                E_USER_WARNING
            );
            $key = "$key.$a";
        }
        if (!$this->finalized) {
            $this->autoFinalize();
        }
        if (!isset($this->def->info[$key])) {
            // can't add % due to SimpleTest bug
            $this->triggerError(
                'Cannot retrieve value of undefined directive ' . htmlspecialchars($key),
                E_USER_WARNING
            );
            return;
        }
        if (isset($this->def->info[$key]->isAlias)) {
            $d = $this->def->info[$key];
            $this->triggerError(
                'Cannot get value from aliased directive, use real name ' . $d->key,
                E_USER_ERROR
            );
            return;
        }
        if ($this->lock) {
            list($ns) = explode('.', $key);
            if ($ns !== $this->lock) {
                $this->triggerError(
                    'Cannot get value of namespace ' . $ns . ' when lock for ' .
                    $this->lock .
                    ' is active, this probably indicates a Definition setup method ' .
                    'is accessing directives that are not within its namespace',
                    E_USER_ERROR
                );
                return;
            }
        }
        return $this->plist->get($key);
    }

    public function getBatch($namespace)
    {
        if (!$this->finalized) {
            $this->autoFinalize();
        }
        $full = $this->getAll();
        if (!isset($full[$namespace])) {
            $this->triggerError(
                'Cannot retrieve undefined namespace ' .
                htmlspecialchars($namespace),
                E_USER_WARNING
            );
            return;
        }
        return $full[$namespace];
    }

    public function getBatchSerial($namespace)
    {
        if (empty($this->serials[$namespace])) {
            $batch = $this->getBatch($namespace);
            unset($batch['DefinitionRev']);
            $this->serials[$namespace] = sha1(serialize($batch));
        }
        return $this->serials[$namespace];
    }

    public function getSerial()
    {
        if (empty($this->serial)) {
            $this->serial = sha1(serialize($this->getAll()));
        }
        return $this->serial;
    }

    public function getAll()
    {
        if (!$this->finalized) {
            $this->autoFinalize();
        }
        $ret = array();
        foreach ($this->plist->squash() as $name => $value) {
            list($ns, $key) = explode('.', $name, 2);
            $ret[$ns][$key] = $value;
        }
        return $ret;
    }

    public function set($key, $value, $a = null)
    {
        if (strpos($key, '.') === false) {
            $namespace = $key;
            $directive = $value;
            $value = $a;
            $key = "$key.$directive";
            $this->triggerError("Using deprecated API: use \$config->set('$key', ...) instead", E_USER_NOTICE);
        } else {
            list($namespace) = explode('.', $key);
        }
        if ($this->isFinalized('Cannot set directive after finalization')) {
            return;
        }
        if (!isset($this->def->info[$key])) {
            $this->triggerError(
                'Cannot set undefined directive ' . htmlspecialchars($key) . ' to value',
                E_USER_WARNING
            );
            return;
        }
        $def = $this->def->info[$key];

        if (isset($def->isAlias)) {
            if ($this->aliasMode) {
                $this->triggerError(
                    'Double-aliases not allowed, please fix '.
                    'ConfigSchema bug with' . $key,
                    E_USER_ERROR
                );
                return;
            }
            $this->aliasMode = true;
            $this->set($def->key, $value);
            $this->aliasMode = false;
            $this->triggerError("$key is an alias, preferred directive name is {$def->key}", E_USER_NOTICE);
            return;
        }

        // Raw type might be negative when using the fully optimized form
        // of stdclass, which indicates allow_null == true
        $rtype = is_int($def) ? $def : $def->type;
        if ($rtype < 0) {
            $type = -$rtype;
            $allow_null = true;
        } else {
            $type = $rtype;
            $allow_null = isset($def->allow_null);
        }

        try {
            $value = $this->parser->parse($value, $type, $allow_null);
        } catch (HTMLPurifier_VarParserException $e) {
            $this->triggerError(
                'Value for ' . $key . ' is of invalid type, should be ' .
                HTMLPurifier_VarParser::getTypeName($type),
                E_USER_WARNING
            );
            return;
        }
        if (is_string($value) && is_object($def)) {
            // resolve value alias if defined
            if (isset($def->aliases[$value])) {
                $value = $def->aliases[$value];
            }
            // check to see if the value is allowed
            if (isset($def->allowed) && !isset($def->allowed[$value])) {
                $this->triggerError(
                    'Value not supported, valid values are: ' .
                    $this->_listify($def->allowed),
                    E_USER_WARNING
                );
                return;
            }
        }
        $this->plist->set($key, $value);

        // reset definitions if the directives they depend on changed
        // this is a very costly process, so it's discouraged
        // with finalization
        if ($namespace == 'HTML' || $namespace == 'CSS' || $namespace == 'URI') {
            $this->definitions[$namespace] = null;
        }

        $this->serials[$namespace] = false;
    }

    private function _listify($lookup)
    {
        $list = array();
        foreach ($lookup as $name => $b) {
            $list[] = $name;
        }
        return implode(', ', $list);
    }

    public function getHTMLDefinition($raw = false, $optimized = false)
    {
        return $this->getDefinition('HTML', $raw, $optimized);
    }

    public function getCSSDefinition($raw = false, $optimized = false)
    {
        return $this->getDefinition('CSS', $raw, $optimized);
    }

    public function getURIDefinition($raw = false, $optimized = false)
    {
        return $this->getDefinition('URI', $raw, $optimized);
    }

    public function getDefinition($type, $raw = false, $optimized = false)
    {
        if ($optimized && !$raw) {
            throw new HTMLPurifier_Exception("Cannot set optimized = true when raw = false");
        }
        if (!$this->finalized) {
            $this->autoFinalize();
        }
        // temporarily suspend locks, so we can handle recursive definition calls
        $lock = $this->lock;
        $this->lock = null;
        $factory = HTMLPurifier_DefinitionCacheFactory::instance();
        $cache = $factory->create($type, $this);
        $this->lock = $lock;
        if (!$raw) {
            // full definition
            // ---------------
            // check if definition is in memory
            if (!empty($this->definitions[$type])) {
                $def = $this->definitions[$type];
                // check if the definition is setup
                if ($def->setup) {
                    return $def;
                } else {
                    $def->setup($this);
                    if ($def->optimized) {
                        $cache->add($def, $this);
                    }
                    return $def;
                }
            }
            // check if definition is in cache
            $def = $cache->get($this);
            if ($def) {
                // definition in cache, save to memory and return it
                $this->definitions[$type] = $def;
                return $def;
            }
            // initialize it
            $def = $this->initDefinition($type);
            // set it up
            $this->lock = $type;
            $def->setup($this);
            $this->lock = null;
            // save in cache
            $cache->add($def, $this);
            // return it
            return $def;
        } else {
            // raw definition
            // --------------
            // check preconditions
            $def = null;
            if ($optimized) {
                if (is_null($this->get($type . '.DefinitionID'))) {
                    // fatally error out if definition ID not set
                    throw new HTMLPurifier_Exception(
                        "Cannot retrieve raw version without specifying %$type.DefinitionID"
                    );
                }
            }
            if (!empty($this->definitions[$type])) {
                $def = $this->definitions[$type];
                if ($def->setup && !$optimized) {
                    $extra = $this->chatty ?
                        " (try moving this code block earlier in your initialization)" :
                        "";
                    throw new HTMLPurifier_Exception(
                        "Cannot retrieve raw definition after it has already been setup" .
                        $extra
                    );
                }
                if ($def->optimized === null) {
                    $extra = $this->chatty ? " (try flushing your cache)" : "";
                    throw new HTMLPurifier_Exception(
                        "Optimization status of definition is unknown" . $extra
                    );
                }
                if ($def->optimized !== $optimized) {
                    $msg = $optimized ? "optimized" : "unoptimized";
                    $extra = $this->chatty ?
                        " (this backtrace is for the first inconsistent call, which was for a $msg raw definition)"
                        : "";
                    throw new HTMLPurifier_Exception(
                        "Inconsistent use of optimized and unoptimized raw definition retrievals" . $extra
                    );
                }
            }
            // check if definition was in memory
            if ($def) {
                if ($def->setup) {
                    // invariant: $optimized === true (checked above)
                    return null;
                } else {
                    return $def;
                }
            }
            // if optimized, check if definition was in cache
            // (because we do the memory check first, this formulation
            // is prone to cache slamming, but I think
            // guaranteeing that either /all/ of the raw
            // setup code or /none/ of it is run is more important.)
            if ($optimized) {
                // This code path only gets run once; once we put
                // something in $definitions (which is guaranteed by the
                // trailing code), we always short-circuit above.
                $def = $cache->get($this);
                if ($def) {
                    // save the full definition for later, but don't
                    // return it yet
                    $this->definitions[$type] = $def;
                    return null;
                }
            }
            // check invariants for creation
            if (!$optimized) {
                if (!is_null($this->get($type . '.DefinitionID'))) {
                    if ($this->chatty) {
                        $this->triggerError(
                            'Due to a documentation error in previous version of HTML Purifier, your ' .
                            'definitions are not being cached.  If this is OK, you can remove the ' .
                            '%$type.DefinitionRev and %$type.DefinitionID declaration.  Otherwise, ' .
                            'modify your code to use maybeGetRawDefinition, and test if the returned ' .
                            'value is null before making any edits (if it is null, that means that a ' .
                            'cached version is available, and no raw operations are necessary).  See ' .
                            '<a href="http://htmlpurifier.org/docs/enduser-customize.html#optimized">' .
                            'Customize</a> for more details',
                            E_USER_WARNING
                        );
                    } else {
                        $this->triggerError(
                            "Useless DefinitionID declaration",
                            E_USER_WARNING
                        );
                    }
                }
            }
            // initialize it
            $def = $this->initDefinition($type);
            $def->optimized = $optimized;
            return $def;
        }
        throw new HTMLPurifier_Exception("The impossible happened!");
    }

    private function initDefinition($type)
    {
        // quick checks failed, let's create the object
        if ($type == 'HTML') {
            $def = new HTMLPurifier_HTMLDefinition();
        } elseif ($type == 'CSS') {
            $def = new HTMLPurifier_CSSDefinition();
        } elseif ($type == 'URI') {
            $def = new HTMLPurifier_URIDefinition();
        } else {
            throw new HTMLPurifier_Exception(
                "Definition of $type type not supported"
            );
        }
        $this->definitions[$type] = $def;
        return $def;
    }

    public function maybeGetRawDefinition($name)
    {
        return $this->getDefinition($name, true, true);
    }

    public function maybeGetRawHTMLDefinition()
    {
        return $this->getDefinition('HTML', true, true);
    }

    public function maybeGetRawCSSDefinition()
    {
        return $this->getDefinition('CSS', true, true);
    }

    public function maybeGetRawURIDefinition()
    {
        return $this->getDefinition('URI', true, true);
    }

    public function loadArray($config_array)
    {
        if ($this->isFinalized('Cannot load directives after finalization')) {
            return;
        }
        foreach ($config_array as $key => $value) {
            $key = str_replace('_', '.', $key);
            if (strpos($key, '.') !== false) {
                $this->set($key, $value);
            } else {
                $namespace = $key;
                $namespace_values = $value;
                foreach ($namespace_values as $directive => $value2) {
                    $this->set($namespace .'.'. $directive, $value2);
                }
            }
        }
    }

    public static function getAllowedDirectivesForForm($allowed, $schema = null)
    {
        if (!$schema) {
            $schema = HTMLPurifier_ConfigSchema::instance();
        }
        if ($allowed !== true) {
            if (is_string($allowed)) {
                $allowed = array($allowed);
            }
            $allowed_ns = array();
            $allowed_directives = array();
            $blacklisted_directives = array();
            foreach ($allowed as $ns_or_directive) {
                if (strpos($ns_or_directive, '.') !== false) {
                    // directive
                    if ($ns_or_directive[0] == '-') {
                        $blacklisted_directives[substr($ns_or_directive, 1)] = true;
                    } else {
                        $allowed_directives[$ns_or_directive] = true;
                    }
                } else {
                    // namespace
                    $allowed_ns[$ns_or_directive] = true;
                }
            }
        }
        $ret = array();
        foreach ($schema->info as $key => $def) {
            list($ns, $directive) = explode('.', $key, 2);
            if ($allowed !== true) {
                if (isset($blacklisted_directives["$ns.$directive"])) {
                    continue;
                }
                if (!isset($allowed_directives["$ns.$directive"]) && !isset($allowed_ns[$ns])) {
                    continue;
                }
            }
            if (isset($def->isAlias)) {
                continue;
            }
            if ($directive == 'DefinitionID' || $directive == 'DefinitionRev') {
                continue;
            }
            $ret[] = array($ns, $directive);
        }
        return $ret;
    }

    public static function loadArrayFromForm($array, $index = false, $allowed = true, $mq_fix = true, $schema = null)
    {
        $ret = HTMLPurifier_Config::prepareArrayFromForm($array, $index, $allowed, $mq_fix, $schema);
        $config = HTMLPurifier_Config::create($ret, $schema);
        return $config;
    }

    public function mergeArrayFromForm($array, $index = false, $allowed = true, $mq_fix = true)
    {
         $ret = HTMLPurifier_Config::prepareArrayFromForm($array, $index, $allowed, $mq_fix, $this->def);
         $this->loadArray($ret);
    }

    public static function prepareArrayFromForm($array, $index = false, $allowed = true, $mq_fix = true, $schema = null)
    {
        if ($index !== false) {
            $array = (isset($array[$index]) && is_array($array[$index])) ? $array[$index] : array();
        }
        $mq = $mq_fix && function_exists('get_magic_quotes_gpc') && get_magic_quotes_gpc();

        $allowed = HTMLPurifier_Config::getAllowedDirectivesForForm($allowed, $schema);
        $ret = array();
        foreach ($allowed as $key) {
            list($ns, $directive) = $key;
            $skey = "$ns.$directive";
            if (!empty($array["Null_$skey"])) {
                $ret[$ns][$directive] = null;
                continue;
            }
            if (!isset($array[$skey])) {
                continue;
            }
            $value = $mq ? stripslashes($array[$skey]) : $array[$skey];
            $ret[$ns][$directive] = $value;
        }
        return $ret;
    }

    public function loadIni($filename)
    {
        if ($this->isFinalized('Cannot load directives after finalization')) {
            return;
        }
        $array = parse_ini_file($filename, true);
        $this->loadArray($array);
    }

    public function isFinalized($error = false)
    {
        if ($this->finalized && $error) {
            $this->triggerError($error, E_USER_ERROR);
        }
        return $this->finalized;
    }

    public function autoFinalize()
    {
        if ($this->autoFinalize) {
            $this->finalize();
        } else {
            $this->plist->squash(true);
        }
    }

    public function finalize()
    {
        $this->finalized = true;
        $this->parser = null;
    }

    protected function triggerError($msg, $no)
    {
        // determine previous stack frame
        $extra = '';
        if ($this->chatty) {
            $trace = debug_backtrace();
            // zip(tail(trace), trace) -- but PHP is not Haskell har har
            for ($i = 0, $c = count($trace); $i < $c - 1; $i++) {
                // XXX this is not correct on some versions of HTML Purifier
                if ($trace[$i + 1]['class'] === 'HTMLPurifier_Config') {
                    continue;
                }
                $frame = $trace[$i];
                $extra = " invoked on line {$frame['line']} in file {$frame['file']}";
                break;
            }
        }
        trigger_error($msg . $extra, $no);
    }

    public function serialize()
    {
        $this->getDefinition('HTML');
        $this->getDefinition('CSS');
        $this->getDefinition('URI');
        return serialize($this);
    }

}

// vim: et sw=4 sts=4
