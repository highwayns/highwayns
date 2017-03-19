DROP TABLE IF EXISTS `qs_ad`;
CREATE TABLE `qs_ad` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `alias` varchar(80) NOT NULL,
  `is_display` tinyint(1) NOT NULL default '1',
  `category_id` smallint(5) NOT NULL,
  `type_id` smallint(5) NOT NULL,
  `title` varchar(100) NOT NULL,
  `note` varchar(230) NOT NULL,
  `show_order` int(10) unsigned NOT NULL default '50',
  `addtime` int(10) unsigned NOT NULL,
  `starttime` int(10) unsigned NOT NULL,
  `deadline` int(11) NOT NULL default '0',
  `text_content` varchar(250) NOT NULL,
  `text_url` varchar(250) NOT NULL,
  `text_color` varchar(60) NOT NULL,
  `img_path` varchar(250) NOT NULL,
  `img_url` varchar(250) NOT NULL,
  `img_explain` varchar(250) NOT NULL,
  `img_uid` int(10) NOT NULL default '0',
  `code_content` text NOT NULL,
  `flash_path` varchar(250) NOT NULL,
  `flash_width` int(10) unsigned NOT NULL,
  `flash_height` int(10) unsigned NOT NULL,
  `floating_type` tinyint(3) unsigned NOT NULL default '1',
  `floating_width` int(10) unsigned NOT NULL,
  `floating_height` int(10) unsigned NOT NULL,
  `floating_url` varchar(250) NOT NULL,
  `floating_path` varchar(250) NOT NULL,
  `floating_left` varchar(10) NOT NULL,
  `floating_right` varchar(10) NOT NULL,
  `floating_top` int(11) NOT NULL,
  `video_path` varchar(250) NOT NULL,
  `video_width` int(10) unsigned NOT NULL,
  `video_height` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `alias_starttime_deadline` (`alias`,`starttime`,`deadline`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||ad表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_admin`;
CREATE TABLE `qs_admin` (
  `admin_id` smallint(5) unsigned NOT NULL auto_increment,
  `admin_name` varchar(40) NOT NULL,
  `email` varchar(40) NOT NULL,
  `pwd` varchar(32) NOT NULL,
  `pwd_hash` varchar(30) NOT NULL,
  `purview` text NOT NULL,
  `rank` varchar(32) NOT NULL,
  `add_time` int(10) NOT NULL,
  `last_login_time` int(10) NOT NULL,
  `last_login_ip` varchar(15) NOT NULL,
  PRIMARY KEY  (`admin_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||admin表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_admin_log`;
CREATE TABLE `qs_admin_log` (
  `log_id` int(10) unsigned NOT NULL auto_increment,
  `admin_name` varchar(20) NOT NULL,
  `add_time` int(10) NOT NULL,
  `log_value` varchar(255) NOT NULL,
  `log_ip` varchar(20) NOT NULL,
  `log_type` tinyint(1) unsigned NOT NULL default '1',
  PRIMARY KEY  (`log_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||admin_log表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_ad_category`;
CREATE TABLE `qs_ad_category` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `alias` varchar(100) NOT NULL,
  `type_id` int(10) unsigned NOT NULL,
  `categoryname` varchar(100) NOT NULL,
  `admin_set` tinyint(3) unsigned NOT NULL default '0',
  `expense` smallint(5) unsigned NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||ad_category表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_article`;
CREATE TABLE `qs_article` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `type_id` tinyint(3) unsigned NOT NULL,
  `parentid` smallint(5) unsigned NOT NULL,
  `title` varchar(100) NOT NULL,
  `content` mediumtext NOT NULL,
  `tit_color` varchar(10) default NULL,
  `tit_b` tinyint(1) unsigned NOT NULL default '0',
  `Small_img` varchar(80) default NULL,
  `author` varchar(50) default NULL,
  `source` varchar(100) default NULL,
  `focos` tinyint(3) unsigned NOT NULL default '1',
  `is_display` tinyint(3) unsigned NOT NULL default '1',
  `is_url` varchar(200) NOT NULL default '0',
  `seo_keywords` varchar(100) default NULL,
  `seo_description` varchar(200) default NULL,
  `click` int(10) unsigned NOT NULL default '1',
  `addtime` int(10) unsigned NOT NULL,
  `article_order` smallint(5) unsigned NOT NULL default '0',
  `robot` tinyint(3) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`),
  KEY `type_id` (`type_id`,`article_order`,`id`),
  KEY `click` (`click`),
  KEY `focos_article_order` (`focos`,`article_order`,`id`),
  KEY `addtime` (`addtime`),
  KEY `parentid` (`parentid`,`article_order`,`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||qs_article表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_article_category`;
CREATE TABLE `qs_article_category` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `parentid` smallint(5) unsigned NOT NULL,
  `categoryname` varchar(80) NOT NULL,
  `category_order` smallint(5) unsigned NOT NULL default '0',
  `title` varchar(255) default NULL,
  `description` varchar(255) default NULL,
  `keywords` varchar(255) default NULL,
  `admin_set` tinyint(3) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||article_category表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_article_property`;
CREATE TABLE `qs_article_property` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `categoryname` varchar(30) NOT NULL,
  `category_order` smallint(5) unsigned NOT NULL default '0',
  `admin_set` tinyint(3) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||article_property表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_audit_reason`;
CREATE TABLE `qs_audit_reason` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `jobs_id` int(10) unsigned NOT NULL default '0',
  `company_id` int(10) unsigned NOT NULL default '0',
  `resume_id` int(10) unsigned NOT NULL default '0',
  `reason` varchar(255) NOT NULL,
  `addtime` int(10) NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `jobs_id` (`jobs_id`),
  KEY `company_id` (`company_id`),
  KEY `resume_id` (`resume_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||audit_reason表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_baiduxml`;
CREATE TABLE `qs_baiduxml` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL,
  `value` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||baiduxml表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_baidu_submiturl`;
CREATE TABLE `qs_baidu_submiturl` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL,
  `value` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||baidu_submiturl表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_captcha`;
CREATE TABLE `qs_captcha` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL,
  `value` varchar(200) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||captcha表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_category`;
CREATE TABLE `qs_category` (
  `c_id` int(10) unsigned NOT NULL auto_increment,
  `c_parentid` int(10) unsigned NOT NULL,
  `c_alias` char(30) NOT NULL,
  `c_name` char(30) NOT NULL,
  `c_order` int(10) NOT NULL,
  `c_index` char(1) NOT NULL,
  `c_note` char(60) NOT NULL,
  `stat_jobs` char(15) NOT NULL,
  `stat_resume` char(15) NOT NULL,
  PRIMARY KEY  (`c_id`),
  KEY `c_alias` (`c_alias`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||category表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_category_district`;
CREATE TABLE `qs_category_district` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `parentid` int(10) unsigned NOT NULL default '0',
  `categoryname` varchar(30) NOT NULL,
  `category_order` smallint(5) unsigned NOT NULL default '0',
  `stat_jobs` varchar(15) NOT NULL,
  `stat_resume` varchar(15) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||category_district表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_category_group`;
CREATE TABLE `qs_category_group` (
  `g_id` int(10) unsigned NOT NULL auto_increment,
  `g_alias` varchar(60) NOT NULL,
  `g_name` varchar(100) NOT NULL,
  `g_sys` tinyint(1) unsigned NOT NULL default '0',
  PRIMARY KEY  (`g_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||category_group表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_category_jobs`;
CREATE TABLE `qs_category_jobs` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `parentid` smallint(5) unsigned NOT NULL,
  `categoryname` varchar(80) NOT NULL,
  `category_order` smallint(5) unsigned NOT NULL default '0',
  `stat_jobs` varchar(15) NOT NULL,
  `stat_resume` varchar(15) NOT NULL,
  `content` text,
  PRIMARY KEY  (`id`),
  KEY `parentid` (`parentid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||category_jobs表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_category_major`;
CREATE TABLE `qs_category_major` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `parentid` int(10) NOT NULL,
  `categoryname` varchar(50) NOT NULL,
  `category_order` int(10) NOT NULL default '0',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||category_major表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_color`;
CREATE TABLE `qs_color` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `value` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||color表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_company_down_resume`;
CREATE TABLE `qs_company_down_resume` (
  `did` int(10) unsigned NOT NULL auto_increment,
  `resume_id` int(10) unsigned NOT NULL,
  `resume_name` varchar(60) NOT NULL,
  `resume_uid` int(10) unsigned NOT NULL,
  `company_uid` int(10) unsigned NOT NULL,
  `company_name` varchar(60) NOT NULL,
  `down_addtime` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`did`),
  KEY `resume_uid_rid` (`resume_uid`,`resume_id`),
  KEY `down_addtime` (`down_addtime`),
  KEY `company_uid_addtime` (`company_uid`,`down_addtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||company_down_resume表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_company_favorites`;
CREATE TABLE `qs_company_favorites` (
  `did` int(10) unsigned NOT NULL auto_increment,
  `resume_id` int(10) unsigned NOT NULL,
  `company_uid` int(10) unsigned NOT NULL,
  `favoritesa_ddtime` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`did`),
  KEY `company_uid` (`company_uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||company_favorites表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_company_interview`;
CREATE TABLE `qs_company_interview` (
  `did` int(10) unsigned NOT NULL auto_increment,
  `resume_id` int(10) unsigned NOT NULL,
  `resume_name` varchar(30) NOT NULL,
  `resume_addtime` int(10) NOT NULL,
  `resume_uid` int(10) unsigned NOT NULL,
  `jobs_id` int(10) unsigned NOT NULL,
  `jobs_name` varchar(60) NOT NULL,
  `jobs_addtime` int(10) NOT NULL,
  `company_id` int(10) unsigned NOT NULL,
  `company_name` varchar(60) NOT NULL,
  `company_addtime` int(10) NOT NULL,
  `company_uid` int(10) unsigned NOT NULL,
  `interview_addtime` int(10) unsigned NOT NULL,
  `notes` varchar(255) NOT NULL default '',
  `personal_look` tinyint(3) unsigned NOT NULL default '1',
  `interview_time` varchar(20) NOT NULL default '',
  PRIMARY KEY  (`did`),
  KEY `resume_uid_resumeid` (`resume_uid`,`resume_id`),
  KEY `company_uid_jobid` (`company_uid`,`jobs_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||company_interview表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_company_label_resume`;
CREATE TABLE `qs_company_label_resume` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) NOT NULL,
  `personal_uid` int(10) NOT NULL,
  `resume_id` int(10) NOT NULL,
  `resume_state` tinyint(1) NOT NULL default '0',
  `resume_state_cn` varchar(20) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||company_label_resume表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_company_profile`;
CREATE TABLE `qs_company_profile` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `tpl` varchar(60) NOT NULL,
  `companyname` varchar(60) NOT NULL,
  `nature` smallint(5) unsigned NOT NULL,
  `nature_cn` varchar(30) NOT NULL,
  `trade` smallint(5) unsigned NOT NULL,
  `trade_cn` varchar(30) NOT NULL,
  `district` smallint(5) unsigned NOT NULL,
  `sdistrict` smallint(5) unsigned NOT NULL,
  `district_cn` varchar(100) NOT NULL default '',
  `street` smallint(5) unsigned NOT NULL,
  `street_cn` varchar(50) NOT NULL,
  `scale` smallint(5) unsigned NOT NULL,
  `scale_cn` varchar(30) NOT NULL,
  `registered` varchar(150) NOT NULL,
  `currency` varchar(10) NOT NULL,
  `address` varchar(250) NOT NULL,
  `contact` varchar(100) NOT NULL,
  `telephone` varchar(130) NOT NULL,
  `email` varchar(100) NOT NULL,
  `website` varchar(100) NOT NULL,
  `license` varchar(120) NOT NULL,
  `certificate_img` varchar(80) NOT NULL,
  `logo` varchar(30) NOT NULL,
  `contents` text NOT NULL,
  `audit` tinyint(4) unsigned NOT NULL default '0',
  `map_open` tinyint(3) unsigned NOT NULL default '0',
  `map_x` varchar(50) NOT NULL,
  `map_y` varchar(50) NOT NULL,
  `map_zoom` tinyint(3) unsigned NOT NULL,
  `addtime` int(10) unsigned NOT NULL,
  `refreshtime` int(10) unsigned NOT NULL,
  `click` int(10) unsigned NOT NULL default '1',
  `user_status` tinyint(3) unsigned NOT NULL default '1',
  `yellowpages` tinyint(1) NOT NULL default '0',
  `contact_show` tinyint(1) unsigned NOT NULL default '0',
  `telephone_show` tinyint(1) unsigned NOT NULL default '0',
  `address_show` tinyint(1) unsigned NOT NULL default '0',
  `email_show` tinyint(1) unsigned NOT NULL default '0',
  `robot` tinyint(3) unsigned NOT NULL default '0',
  `comment` int(10) unsigned NOT NULL,
  `resume_processing` tinyint(3) NOT NULL default '100',
  `tag` varchar(60) NOT NULL,
  `wzp_tpl` tinyint(1) NOT NULL default '0',
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`),
  KEY `audit` (`audit`),
  KEY `companyname` (`companyname`),
  KEY `yellowpages` (`yellowpages`,`trade`),
  KEY `addtime` (`addtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||company_profile表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_config`;
CREATE TABLE `qs_config` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL,
  `value` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||config表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_consultant`;
CREATE TABLE `qs_consultant` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL default '',
  `pic` text,
  `qq` int(15) unsigned NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||consultant表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_crons`;
CREATE TABLE `qs_crons` (
  `cronid` smallint(5) unsigned NOT NULL auto_increment,
  `available` tinyint(1) unsigned NOT NULL,
  `admin_set` tinyint(1) unsigned NOT NULL default '0',
  `name` varchar(60) NOT NULL,
  `filename` varchar(60) NOT NULL,
  `lastrun` int(10) unsigned NOT NULL,
  `nextrun` int(10) unsigned NOT NULL,
  `weekday` tinyint(1) NOT NULL,
  `day` tinyint(2) NOT NULL,
  `hour` tinyint(2) NOT NULL,
  `minute` varchar(60) NOT NULL,
  PRIMARY KEY  (`cronid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||crons表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_explain`;
CREATE TABLE `qs_explain` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `type_id` smallint(5) unsigned NOT NULL,
  `title` varchar(100) NOT NULL,
  `content` mediumtext NOT NULL,
  `tit_color` varchar(10) default NULL,
  `tit_b` tinyint(1) NOT NULL default '0',
  `is_display` tinyint(3) unsigned NOT NULL default '1',
  `is_url` varchar(200) NOT NULL default '0',
  `seo_keywords` varchar(100) default NULL,
  `seo_description` varchar(200) default NULL,
  `click` int(11) NOT NULL default '1',
  `addtime` int(10) NOT NULL,
  `show_order` smallint(5) NOT NULL default '0',
  PRIMARY KEY  (`id`),
  KEY `type_id` (`type_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||explain表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_explain_category`;
CREATE TABLE `qs_explain_category` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `categoryname` varchar(80) NOT NULL,
  `category_order` smallint(5) unsigned NOT NULL default '0',
  `admin_set` tinyint(3) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||explain_category表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_feedback`;
CREATE TABLE `qs_feedback` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `infotype` tinyint(3) unsigned NOT NULL,
  `feedback` varchar(250) NOT NULL,
  `addtime` int(10) NOT NULL,
  `tel` varchar(255) NOT NULL default '',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

||-_-||feedback表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_help`;
CREATE TABLE `qs_help` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `type_id` tinyint(3) unsigned NOT NULL,
  `parentid` smallint(5) unsigned NOT NULL,
  `title` varchar(100) NOT NULL,
  `content` text NOT NULL,
  `click` int(10) unsigned NOT NULL default '1',
  `addtime` int(10) unsigned NOT NULL,
  `order` smallint(5) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`),
  KEY `type_id` (`type_id`,`order`,`id`),
  KEY `focos_article_order` (`order`,`id`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

||-_-||help表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_help_category`;
CREATE TABLE `qs_help_category` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `parentid` smallint(5) unsigned NOT NULL,
  `categoryname` varchar(80) NOT NULL,
  `category_order` smallint(5) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||help_category表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_hotword`;
CREATE TABLE `qs_hotword` (
  `w_id` int(10) unsigned NOT NULL auto_increment,
  `w_word` varchar(120) NOT NULL,
  `w_hot` int(10) unsigned NOT NULL default '1',
  PRIMARY KEY  (`w_id`),
  KEY `w_word` (`w_word`),
  KEY `w_hot` (`w_hot`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||hotword表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_hrtools`;
CREATE TABLE `qs_hrtools` (
  `h_id` int(10) unsigned NOT NULL auto_increment,
  `h_typeid` smallint(5) unsigned NOT NULL,
  `h_filename` varchar(200) NOT NULL,
  `h_fileurl` varchar(200) NOT NULL,
  `h_order` int(10) NOT NULL default '0',
  `h_color` varchar(7) NOT NULL,
  `h_strong` tinyint(1) NOT NULL default '0',
  PRIMARY KEY  (`h_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||hrtools表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_hrtools_category`;
CREATE TABLE `qs_hrtools_category` (
  `c_id` smallint(5) unsigned NOT NULL auto_increment,
  `c_name` varchar(80) NOT NULL,
  `c_order` int(11) NOT NULL default '0',
  `c_adminset` tinyint(3) NOT NULL default '0',
  PRIMARY KEY  (`c_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||hrtools_category表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_jobs`;
CREATE TABLE `qs_jobs` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `jobs_name` varchar(30) NOT NULL,
  `companyname` varchar(50) NOT NULL,
  `company_id` int(10) unsigned NOT NULL,
  `company_addtime` int(10) unsigned NOT NULL,
  `company_audit` tinyint(1) unsigned NOT NULL default '0',
  `recommend` tinyint(1) unsigned NOT NULL default '0',
  `emergency` tinyint(1) unsigned NOT NULL default '0',
  `highlight` varchar(7) NOT NULL,
  `stick` tinyint(1) NOT NULL,
  `nature` tinyint(3) unsigned NOT NULL,
  `nature_cn` varchar(30) NOT NULL,
  `sex` tinyint(1) unsigned NOT NULL default '3',
  `sex_cn` varchar(3) NOT NULL,
  `age` varchar(10) NOT NULL,
  `amount` smallint(5) unsigned NOT NULL,
  `topclass` smallint(5) unsigned NOT NULL,
  `category` smallint(5) unsigned NOT NULL,
  `subclass` smallint(5) unsigned NOT NULL,
  `category_cn` varchar(100) NOT NULL default '',
  `trade` smallint(5) unsigned NOT NULL,
  `trade_cn` varchar(30) NOT NULL,
  `scale` smallint(5) unsigned NOT NULL,
  `scale_cn` varchar(30) NOT NULL,
  `district` smallint(5) unsigned NOT NULL,
  `sdistrict` smallint(5) unsigned NOT NULL,
  `district_cn` varchar(100) NOT NULL,
  `street` int(10) unsigned NOT NULL,
  `street_cn` varchar(50) NOT NULL default '',
  `tag` varchar(50) NOT NULL,
  `tag_cn` varchar(100) NOT NULL,
  `education` smallint(5) unsigned NOT NULL,
  `education_cn` varchar(30) NOT NULL,
  `experience` smallint(5) unsigned NOT NULL,
  `experience_cn` varchar(30) NOT NULL,
  `wage` smallint(5) unsigned NOT NULL,
  `wage_cn` varchar(30) NOT NULL,
  `negotiable` tinyint(1) unsigned NOT NULL,
  `graduate` tinyint(1) unsigned NOT NULL default '0',
  `contents` text NOT NULL,
  `addtime` int(10) unsigned NOT NULL,
  `deadline` int(10) unsigned NOT NULL,
  `refreshtime` int(10) unsigned NOT NULL,
  `setmeal_deadline` int(10) unsigned NOT NULL default '0',
  `setmeal_id` smallint(5) unsigned NOT NULL,
  `setmeal_name` varchar(60) NOT NULL,
  `audit` tinyint(1) unsigned NOT NULL default '1',
  `display` tinyint(1) unsigned NOT NULL default '1',
  `click` int(10) unsigned NOT NULL default '1',
  `key` text NOT NULL,
  `user_status` tinyint(1) unsigned NOT NULL default '1',
  `robot` tinyint(1) unsigned NOT NULL default '0',
  `tpl` varchar(60) NOT NULL,
  `map_x` double(9,6) NOT NULL,
  `map_y` double(9,6) NOT NULL,
  `add_mode` tinyint(1) unsigned NOT NULL default '1',
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`),
  KEY `refreshtime` (`refreshtime`),
  KEY `addtime` (`addtime`),
  KEY `company_id` (`company_id`),
  KEY `deadline` (`deadline`),
  KEY `audit` (`audit`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||jobs表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_jobs_contact`;
CREATE TABLE `qs_jobs_contact` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `pid` int(10) unsigned NOT NULL,
  `contact` varchar(80) NOT NULL,
  `qq` varchar(20) NOT NULL,
  `telephone` varchar(80) NOT NULL,
  `address` varchar(80) NOT NULL,
  `email` varchar(80) NOT NULL,
  `notify` tinyint(3) unsigned NOT NULL,
  `notify_mobile` tinyint(3) NOT NULL,
  `contact_show` tinyint(1) unsigned NOT NULL default '0',
  `telephone_show` tinyint(1) unsigned NOT NULL default '0',
  `address_show` tinyint(1) unsigned NOT NULL default '0',
  `email_show` tinyint(1) unsigned NOT NULL default '0',
  `qq_show` tinyint(1) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`),
  KEY `pid` (`pid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||jobs_contact表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_jobs_search_hot`;
CREATE TABLE `qs_jobs_search_hot` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `recommend` tinyint(1) unsigned NOT NULL default '0',
  `emergency` tinyint(1) unsigned NOT NULL default '0',
  `nature` tinyint(3) unsigned NOT NULL,
  `sex` tinyint(1) unsigned NOT NULL default '3',
  `topclass` smallint(5) unsigned NOT NULL,
  `category` smallint(5) unsigned NOT NULL,
  `subclass` smallint(5) unsigned NOT NULL,
  `trade` smallint(5) unsigned NOT NULL,
  `district` smallint(5) unsigned NOT NULL,
  `sdistrict` smallint(5) unsigned NOT NULL,
  `street` smallint(5) unsigned NOT NULL,
  `education` smallint(5) unsigned NOT NULL,
  `experience` smallint(5) unsigned NOT NULL,
  `wage` smallint(5) unsigned NOT NULL,
  `scale` smallint(5) unsigned NOT NULL default '0',
  `graduate` tinyint(1) unsigned NOT NULL default '0',
  `refreshtime` int(10) unsigned NOT NULL,
  `click` int(10) unsigned NOT NULL default '1',
  `setmeal_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `click` (`click`),
  KEY `category_hot` (`category`,`click`),
  KEY `sdistrict_hot` (`sdistrict`,`click`),
  KEY `district_hot` (`district`,`click`),
  KEY `trade_hot` (`trade`,`click`),
  KEY `subclass_hot` (`subclass`,`click`),
  KEY `uid` (`uid`),
  KEY `refreshtime` (`refreshtime`),
  KEY `street_hot` (`street`,`click`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||jobs_search_hot表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_jobs_search_key`;
CREATE TABLE `qs_jobs_search_key` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `recommend` tinyint(1) unsigned NOT NULL default '0',
  `emergency` tinyint(1) unsigned NOT NULL default '0',
  `nature` tinyint(3) unsigned NOT NULL,
  `sex` tinyint(1) unsigned NOT NULL default '3',
  `topclass` smallint(5) unsigned NOT NULL,
  `category` smallint(5) unsigned NOT NULL,
  `subclass` smallint(5) unsigned NOT NULL,
  `trade` smallint(5) unsigned NOT NULL,
  `district` smallint(5) unsigned NOT NULL,
  `sdistrict` smallint(5) unsigned NOT NULL,
  `street` smallint(5) unsigned NOT NULL,
  `education` smallint(5) unsigned NOT NULL,
  `experience` smallint(5) unsigned NOT NULL,
  `wage` smallint(5) unsigned NOT NULL,
  `scale` smallint(5) unsigned NOT NULL default '0',
  `graduate` tinyint(1) unsigned NOT NULL default '0',
  `map_x` double(9,6) NOT NULL,
  `map_y` double(9,6) NOT NULL,
  `refreshtime` int(10) unsigned NOT NULL,
  `key` text NOT NULL,
  `likekey` varchar(220) NOT NULL,
  `setmeal_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `refreshtime` (`refreshtime`),
  KEY `uid` (`uid`),
  KEY `category` (`category`),
  KEY `subclass` (`subclass`),
  KEY `district` (`district`),
  KEY `sdistrict` (`sdistrict`),
  FULLTEXT KEY `key` (`key`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||jobs_search_key表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_jobs_search_rtime`;
CREATE TABLE `qs_jobs_search_rtime` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `recommend` tinyint(1) unsigned NOT NULL default '0',
  `emergency` tinyint(1) unsigned NOT NULL default '0',
  `nature` tinyint(3) unsigned NOT NULL,
  `sex` tinyint(1) unsigned NOT NULL default '3',
  `topclass` smallint(5) unsigned NOT NULL,
  `category` smallint(5) unsigned NOT NULL,
  `subclass` smallint(5) unsigned NOT NULL,
  `trade` smallint(5) unsigned NOT NULL,
  `district` smallint(5) unsigned NOT NULL,
  `sdistrict` smallint(5) unsigned NOT NULL,
  `street` smallint(5) unsigned NOT NULL,
  `education` smallint(5) unsigned NOT NULL,
  `experience` smallint(5) unsigned NOT NULL,
  `wage` smallint(5) unsigned NOT NULL,
  `scale` smallint(5) unsigned NOT NULL default '0',
  `graduate` tinyint(1) unsigned NOT NULL default '0',
  `map_x` double(9,6) NOT NULL,
  `map_y` double(9,6) NOT NULL,
  `refreshtime` int(10) unsigned NOT NULL,
  `setmeal_id` smallint(5) NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `refreshtime` (`refreshtime`),
  KEY `recommend_rtime` (`recommend`,`refreshtime`),
  KEY `emergency_rtime` (`emergency`,`refreshtime`),
  KEY `trade_rtime` (`trade`,`refreshtime`),
  KEY `sdistrict_rtime` (`sdistrict`,`refreshtime`),
  KEY `subclass_rtime` (`subclass`,`refreshtime`),
  KEY `district_rtime` (`district`,`refreshtime`),
  KEY `category_rtime` (`category`,`refreshtime`),
  KEY `uid` (`uid`),
  KEY `map` (`map_x`,`map_y`),
  KEY `street_rtime` (`street`,`refreshtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||jobs_search_rtime表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_jobs_search_scale`;
CREATE TABLE `qs_jobs_search_scale` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `recommend` tinyint(1) unsigned NOT NULL default '0',
  `emergency` tinyint(1) unsigned NOT NULL default '0',
  `nature` tinyint(3) unsigned NOT NULL,
  `sex` tinyint(1) unsigned NOT NULL default '3',
  `topclass` smallint(5) unsigned NOT NULL,
  `category` smallint(5) unsigned NOT NULL,
  `subclass` smallint(5) unsigned NOT NULL,
  `trade` smallint(5) unsigned NOT NULL,
  `scale` smallint(5) unsigned NOT NULL,
  `graduate` tinyint(1) unsigned NOT NULL default '0',
  `district` smallint(5) unsigned NOT NULL,
  `sdistrict` smallint(5) unsigned NOT NULL,
  `street` smallint(5) unsigned NOT NULL,
  `education` smallint(5) unsigned NOT NULL,
  `experience` smallint(5) unsigned NOT NULL,
  `wage` smallint(5) unsigned NOT NULL,
  `refreshtime` int(10) unsigned NOT NULL,
  `setmeal_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`),
  KEY `category_scale` (`category`,`scale`,`refreshtime`),
  KEY `subclass_scale` (`subclass`,`scale`,`refreshtime`),
  KEY `trade_scale` (`trade`,`scale`,`refreshtime`),
  KEY `scale` (`scale`,`refreshtime`),
  KEY `district_scale` (`district`,`scale`,`refreshtime`),
  KEY `sdistrict_scale` (`sdistrict`,`scale`,`refreshtime`),
  KEY `street_scale` (`street`,`scale`,`refreshtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||jobs_search_scale表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_jobs_search_stickrtime`;
CREATE TABLE `qs_jobs_search_stickrtime` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `recommend` tinyint(1) unsigned NOT NULL default '0',
  `emergency` tinyint(1) unsigned NOT NULL default '0',
  `stick` tinyint(1) NOT NULL default '0',
  `nature` tinyint(3) unsigned NOT NULL,
  `sex` tinyint(1) unsigned NOT NULL default '3',
  `topclass` smallint(5) unsigned NOT NULL,
  `category` smallint(5) unsigned NOT NULL,
  `subclass` smallint(5) unsigned NOT NULL,
  `trade` smallint(5) unsigned NOT NULL,
  `district` smallint(5) unsigned NOT NULL,
  `sdistrict` smallint(5) unsigned NOT NULL,
  `street` smallint(5) unsigned NOT NULL,
  `education` smallint(5) unsigned NOT NULL,
  `experience` smallint(5) unsigned NOT NULL,
  `wage` smallint(5) unsigned NOT NULL,
  `scale` smallint(5) unsigned NOT NULL default '0',
  `graduate` tinyint(1) unsigned NOT NULL default '0',
  `refreshtime` int(10) unsigned NOT NULL,
  `setmeal_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `stick_rtime` (`stick`,`refreshtime`),
  KEY `subclass_rtime` (`subclass`,`stick`,`refreshtime`),
  KEY `trade_rtime` (`trade`,`stick`,`refreshtime`),
  KEY `district_rtime` (`district`,`stick`,`refreshtime`),
  KEY `sdistrict_rtime` (`sdistrict`,`stick`,`refreshtime`),
  KEY `uid` (`uid`),
  KEY `category_rtime` (`category`,`stick`,`refreshtime`),
  KEY `stick_street` (`street`,`stick`,`refreshtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||jobs_search_stickrtime表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_jobs_search_wage`;
CREATE TABLE `qs_jobs_search_wage` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `recommend` tinyint(1) unsigned NOT NULL default '0',
  `emergency` tinyint(1) unsigned NOT NULL default '0',
  `nature` tinyint(3) unsigned NOT NULL,
  `sex` tinyint(1) unsigned NOT NULL default '3',
  `topclass` smallint(5) unsigned NOT NULL,
  `category` smallint(5) unsigned NOT NULL,
  `subclass` smallint(5) unsigned NOT NULL,
  `trade` smallint(5) unsigned NOT NULL,
  `district` smallint(5) unsigned NOT NULL,
  `sdistrict` smallint(5) unsigned NOT NULL,
  `street` smallint(5) unsigned NOT NULL,
  `education` smallint(5) unsigned NOT NULL,
  `experience` smallint(5) unsigned NOT NULL,
  `wage` smallint(5) unsigned NOT NULL,
  `scale` smallint(5) unsigned NOT NULL default '0',
  `graduate` tinyint(1) unsigned NOT NULL default '0',
  `refreshtime` int(10) unsigned NOT NULL,
  `setmeal_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `rtime_wage` (`refreshtime`,`wage`),
  KEY `uid` (`uid`),
  KEY `sdistrict_wage` (`sdistrict`,`wage`,`refreshtime`),
  KEY `district_wage` (`district`,`wage`,`refreshtime`),
  KEY `trade_wage` (`trade`,`wage`,`refreshtime`),
  KEY `subclass_wage` (`subclass`,`wage`,`refreshtime`),
  KEY `category_wage` (`category`,`wage`,`refreshtime`),
  KEY `street_wage` (`street`,`wage`,`refreshtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||jobs_search_wage表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_jobs_tag`;
CREATE TABLE `qs_jobs_tag` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `pid` int(10) unsigned NOT NULL,
  `tag` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `pid` (`pid`),
  KEY `tag` (`tag`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||jobs_tag表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_jobs_tmp`;
CREATE TABLE `qs_jobs_tmp` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `jobs_name` varchar(30) NOT NULL,
  `companyname` varchar(50) NOT NULL,
  `company_id` int(10) unsigned NOT NULL,
  `company_addtime` int(10) unsigned NOT NULL,
  `company_audit` tinyint(1) unsigned NOT NULL default '0',
  `recommend` tinyint(1) unsigned NOT NULL default '0',
  `emergency` tinyint(1) unsigned NOT NULL default '0',
  `highlight` varchar(7) NOT NULL,
  `stick` tinyint(1) NOT NULL,
  `nature` tinyint(3) unsigned NOT NULL,
  `nature_cn` varchar(30) NOT NULL,
  `sex` tinyint(1) unsigned NOT NULL default '3',
  `sex_cn` varchar(3) NOT NULL,
  `age` varchar(10) NOT NULL,
  `amount` smallint(5) unsigned NOT NULL,
  `topclass` smallint(5) unsigned NOT NULL,
  `category` smallint(5) unsigned NOT NULL,
  `subclass` smallint(5) unsigned NOT NULL,
  `category_cn` varchar(100) NOT NULL default '',
  `trade` smallint(5) unsigned NOT NULL,
  `trade_cn` varchar(30) NOT NULL,
  `scale` smallint(5) unsigned NOT NULL,
  `scale_cn` varchar(30) NOT NULL,
  `district` smallint(5) unsigned NOT NULL,
  `sdistrict` smallint(5) unsigned NOT NULL,
  `district_cn` varchar(100) NOT NULL,
  `street` int(10) unsigned NOT NULL,
  `street_cn` varchar(50) NOT NULL default '',
  `tag` varchar(50) NOT NULL,
  `tag_cn` varchar(100) NOT NULL,
  `education` smallint(5) unsigned NOT NULL,
  `education_cn` varchar(30) NOT NULL,
  `experience` smallint(5) unsigned NOT NULL,
  `experience_cn` varchar(30) NOT NULL,
  `wage` smallint(5) unsigned NOT NULL,
  `wage_cn` varchar(30) NOT NULL,
  `negotiable` tinyint(1) unsigned NOT NULL,
  `graduate` tinyint(1) unsigned NOT NULL default '0',
  `contents` text NOT NULL,
  `addtime` int(10) unsigned NOT NULL,
  `deadline` int(10) unsigned NOT NULL,
  `refreshtime` int(10) unsigned NOT NULL,
  `setmeal_deadline` int(10) unsigned NOT NULL default '0',
  `setmeal_id` smallint(5) unsigned NOT NULL,
  `setmeal_name` varchar(60) NOT NULL,
  `audit` tinyint(1) unsigned NOT NULL default '1',
  `display` tinyint(1) unsigned NOT NULL default '1',
  `click` int(10) unsigned NOT NULL default '1',
  `key` text NOT NULL,
  `user_status` tinyint(1) unsigned NOT NULL default '1',
  `robot` tinyint(1) unsigned NOT NULL default '0',
  `tpl` varchar(60) NOT NULL,
  `map_x` double(9,6) NOT NULL,
  `map_y` double(9,6) NOT NULL,
  `add_mode` tinyint(1) unsigned NOT NULL default '1',
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`),
  KEY `refreshtime` (`refreshtime`),
  KEY `addtime` (`addtime`),
  KEY `company_id` (`company_id`),
  KEY `deadline` (`deadline`),
  KEY `audit` (`audit`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||jobs_tmp表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_link`;
CREATE TABLE `qs_link` (
  `link_id` int(10) unsigned NOT NULL auto_increment,
  `type_id` tinyint(3) unsigned NOT NULL,
  `display` tinyint(1) unsigned NOT NULL default '1',
  `alias` varchar(30) NOT NULL,
  `link_name` varchar(255) NOT NULL,
  `link_url` varchar(255) NOT NULL,
  `link_logo` varchar(255) NOT NULL,
  `show_order` smallint(5) unsigned NOT NULL default '50',
  `Notes` varchar(255) default NULL,
  `app_notes` varchar(300) NOT NULL,
  PRIMARY KEY  (`link_id`),
  KEY `show_order` (`show_order`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||link表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_link_category`;
CREATE TABLE `qs_link_category` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `categoryname` varchar(80) NOT NULL,
  `c_sys` tinyint(1) unsigned NOT NULL default '0',
  `c_alias` varchar(30) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||link_category表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_locoyspider`;
CREATE TABLE `qs_locoyspider` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL,
  `value` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||locoyspider表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_mailconfig`;
CREATE TABLE `qs_mailconfig` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL,
  `value` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||mailconfig表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_mailqueue`;
CREATE TABLE `qs_mailqueue` (
  `m_id` int(10) unsigned NOT NULL auto_increment,
  `m_type` tinyint(3) unsigned NOT NULL default '0',
  `m_addtime` int(10) unsigned NOT NULL,
  `m_sendtime` int(10) unsigned NOT NULL default '0',
  `m_uid` int(10) unsigned NOT NULL default '0',
  `m_mail` varchar(80) NOT NULL,
  `m_subject` varchar(200) NOT NULL,
  `m_body` text NOT NULL,
  PRIMARY KEY  (`m_id`),
  KEY `m_uid` (`m_uid`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

||-_-||mailqueue表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_mail_templates`;
CREATE TABLE `qs_mail_templates` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL,
  `value` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||mail_templates表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_members`;
CREATE TABLE `qs_members` (
  `uid` int(10) unsigned NOT NULL auto_increment,
  `utype` tinyint(1) unsigned NOT NULL default '1',
  `username` varchar(60) NOT NULL default '',
  `email` varchar(80) NOT NULL,
  `email_audit` tinyint(1) unsigned NOT NULL default '0',
  `mobile` varchar(11) NOT NULL,
  `mobile_audit` tinyint(1) unsigned NOT NULL default '0',
  `password` varchar(100) NOT NULL default '',
  `pwd_hash` varchar(30) NOT NULL,
  `reg_time` int(10) unsigned NOT NULL,
  `reg_ip` varchar(15) NOT NULL,
  `last_login_time` int(10) unsigned NOT NULL,
  `last_login_ip` varchar(15) NOT NULL,
  `qq_openid` varchar(50) NOT NULL,
  `sina_access_token` varchar(50) NOT NULL,
  `taobao_access_token` varchar(50) NOT NULL,
  `qq_nick` varchar(100) NOT NULL,
  `sina_nick` varchar(100) NOT NULL,
  `taobao_nick` varchar(100) NOT NULL,
  `weixin_nick` varchar(100) NOT NULL,
  `qq_binding_time` int(10) NOT NULL,
  `sina_binding_time` int(10) NOT NULL,
  `taobao_binding_time` int(10) NOT NULL,
  `status` tinyint(3) unsigned NOT NULL default '1',
  `avatars` varchar(32) NOT NULL,
  `robot` tinyint(3) unsigned NOT NULL default '0',
  `consultant` smallint(5) unsigned NOT NULL,
  `weixin_openid` varchar(50) default NULL,
  `bindingtime` int(10) unsigned NOT NULL,
  `remind_email_time` int(10) unsigned default NULL,
  `imei` varchar(50) NOT NULL default '',
  `reg_type` tinyint(1) unsigned NOT NULL default '0',
  PRIMARY KEY  (`uid`),
  UNIQUE KEY `username` (`username`),
  KEY `email` (`email`),
  KEY `mobile` (`mobile`),
  KEY `qq_openid` (`qq_openid`),
  KEY `sina_access_token` (`sina_access_token`),
  KEY `taobao_access_token` (`taobao_access_token`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||members表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_members_charge_log`;
CREATE TABLE `qs_members_charge_log` (
  `log_id` int(10) unsigned NOT NULL auto_increment,
  `log_uid` int(10) NOT NULL,
  `log_username` varchar(60) NOT NULL,
  `log_addtime` int(10) NOT NULL,
  `log_value` varchar(255) NOT NULL,
  `log_amount` decimal(10,2) NOT NULL,
  `log_ismoney` tinyint(1) unsigned NOT NULL default '1',
  `log_type` tinyint(1) unsigned NOT NULL,
  `log_mode` tinyint(1) unsigned NOT NULL default '1',
  `log_utype` tinyint(1) unsigned NOT NULL default '1',
  PRIMARY KEY  (`log_id`),
  KEY `log_username` (`log_username`),
  KEY `log_addtime` (`log_addtime`),
  KEY `type_addtime` (`log_type`,`log_addtime`),
  KEY `uid_addtime` (`log_uid`,`log_addtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||members_charge_log表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_members_handsel`;
CREATE TABLE `qs_members_handsel` (
  `id` int(10) NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `htype` varchar(60) NOT NULL,
  `addtime` int(10) NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`,`htype`,`addtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||members_handsel表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_members_info`;
CREATE TABLE `qs_members_info` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `realname` varchar(30) NOT NULL,
  `sex` smallint(1) unsigned NOT NULL,
  `sex_cn` varchar(10) NOT NULL,
  `birthday` varchar(30) NOT NULL,
  `residence` varchar(30) NOT NULL default '',
  `education` smallint(5) unsigned NOT NULL,
  `education_cn` varchar(30) NOT NULL,
  `major` smallint(5) NOT NULL,
  `major_cn` varchar(50) NOT NULL,
  `experience` smallint(5) unsigned NOT NULL,
  `experience_cn` varchar(30) NOT NULL,
  `phone` varchar(50) NOT NULL,
  `email` varchar(50) NOT NULL default '',
  `height` varchar(5) NOT NULL default '',
  `householdaddress` varchar(30) NOT NULL default '',
  `marriage` smallint(5) unsigned NOT NULL,
  `marriage_cn` varchar(30) NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||members_info表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_members_log`;
CREATE TABLE `qs_members_log` (
  `log_id` int(10) unsigned NOT NULL auto_increment,
  `log_uid` int(10) NOT NULL,
  `log_username` varchar(60) NOT NULL,
  `log_addtime` int(10) NOT NULL,
  `log_value` varchar(255) NOT NULL,
  `log_ip` varchar(20) NOT NULL,
  `log_address` varchar(50) NOT NULL,
  `log_utype` tinyint(1) unsigned NOT NULL default '1',
  `log_type` smallint(5) unsigned NOT NULL default '1',
  `log_mode` tinyint(1) unsigned NOT NULL,
  `log_op_type` smallint(5) unsigned NOT NULL,
  `log_op_type_cn` varchar(20) NOT NULL,
  `log_op_used` varchar(20) NOT NULL,
  `log_op_leave` varchar(20) NOT NULL,
  PRIMARY KEY  (`log_id`),
  KEY `log_username` (`log_username`),
  KEY `log_addtime` (`log_addtime`),
  KEY `type_addtime` (`log_type`,`log_addtime`),
  KEY `utype_addtime` (`log_utype`,`log_addtime`),
  KEY `uid_addtime` (`log_uid`,`log_addtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||members_log表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_members_points`;
CREATE TABLE `qs_members_points` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `points` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||members_points表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_members_points_rule`;
CREATE TABLE `qs_members_points_rule` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `utype` tinyint(1) NOT NULL default '1',
  `title` varchar(100) NOT NULL,
  `name` varchar(100) NOT NULL,
  `operation` tinyint(1) NOT NULL default '2',
  `value` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||members_points_rule表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_members_setmeal`;
CREATE TABLE `qs_members_setmeal` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `effective` tinyint(3) unsigned NOT NULL default '0',
  `uid` int(10) unsigned NOT NULL,
  `setmeal_id` int(10) unsigned NOT NULL,
  `setmeal_name` varchar(200) NOT NULL,
  `days` int(10) unsigned NOT NULL,
  `expense` int(10) unsigned NOT NULL,
  `jobs_ordinary` int(10) unsigned NOT NULL,
  `download_resume_ordinary` int(10) unsigned NOT NULL,
  `download_resume_senior` int(10) unsigned NOT NULL,
  `interview_ordinary` int(10) unsigned NOT NULL,
  `interview_senior` int(10) unsigned NOT NULL,
  `talent_pool` int(10) unsigned NOT NULL,
  `recommend_num` int(10) unsigned NOT NULL,
  `recommend_days` int(10) unsigned NOT NULL,
  `stick_num` int(10) unsigned NOT NULL,
  `stick_days` int(10) unsigned NOT NULL,
  `emergency_num` int(10) unsigned NOT NULL,
  `emergency_days` int(10) unsigned NOT NULL,
  `highlight_num` int(10) unsigned NOT NULL,
  `highlight_days` int(10) unsigned NOT NULL,
  `jobsfair_num` int(10) unsigned NOT NULL,
  `change_templates` tinyint(1) unsigned NOT NULL default '0',
  `map_open` tinyint(1) unsigned NOT NULL default '0',
  `added` varchar(250) NOT NULL,
  `starttime` int(10) unsigned NOT NULL,
  `endtime` int(10) unsigned NOT NULL,
  `refresh_jobs_space` int(10) unsigned NOT NULL default '0',
  `refresh_jobs_time` int(10) unsigned NOT NULL default '0',
  `set_sms` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `effective_setmealid` (`effective`,`setmeal_id`),
  KEY `effective_endtime` (`effective`,`endtime`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||members_setmeal表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_navigation`;
CREATE TABLE `qs_navigation` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `alias` varchar(100) NOT NULL,
  `urltype` tinyint(3) unsigned NOT NULL default '0',
  `display` tinyint(3) unsigned NOT NULL default '0',
  `title` varchar(100) NOT NULL,
  `color` varchar(30) NOT NULL,
  `pagealias` varchar(100) NOT NULL,
  `list_id` varchar(30) NOT NULL,
  `tag` varchar(100) NOT NULL,
  `url` varchar(200) NOT NULL,
  `target` varchar(100) NOT NULL,
  `navigationorder` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||navigation表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_navigation_category`;
CREATE TABLE `qs_navigation_category` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `alias` varchar(100) NOT NULL,
  `categoryname` varchar(30) NOT NULL,
  `admin_set` tinyint(3) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||navigation_category表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_notice`;
CREATE TABLE `qs_notice` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `type_id` smallint(5) unsigned NOT NULL,
  `title` varchar(100) NOT NULL,
  `content` mediumtext NOT NULL,
  `tit_color` varchar(10) default NULL,
  `tit_b` tinyint(1) NOT NULL default '0',
  `is_display` tinyint(3) unsigned NOT NULL default '1',
  `is_url` varchar(200) NOT NULL default '0',
  `seo_keywords` varchar(100) default NULL,
  `seo_description` varchar(200) default NULL,
  `click` int(11) NOT NULL default '1',
  `addtime` int(10) NOT NULL,
  `sort` smallint(5) NOT NULL default '0',
  PRIMARY KEY  (`id`),
  KEY `type_id` (`type_id`,`sort`,`id`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

||-_-||notice表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_notice_category`;
CREATE TABLE `qs_notice_category` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `categoryname` varchar(80) NOT NULL,
  `sort` smallint(5) unsigned NOT NULL default '0',
  `admin_set` tinyint(3) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||notice_category表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_order`;
CREATE TABLE `qs_order` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `utype` tinyint(2) unsigned NOT NULL default '1',
  `pay_type` int(1) unsigned NOT NULL,
  `is_paid` tinyint(3) unsigned NOT NULL default '1',
  `oid` varchar(200) NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `payment_name` varchar(20) NOT NULL,
  `points` int(10) unsigned NOT NULL,
  `addtime` int(11) unsigned NOT NULL,
  `payment_time` int(10) unsigned NOT NULL,
  `description` varchar(150) NOT NULL,
  `setmeal` int(10) unsigned NOT NULL,
  `notes` varchar(150) NOT NULL,
  `week` smallint(5) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`),
  KEY `addtime` (`addtime`),
  KEY `payment_name` (`payment_name`),
  KEY `oid` (`oid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||order表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_page`;
CREATE TABLE `qs_page` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `systemclass` tinyint(3) unsigned NOT NULL default '0',
  `pagetpye` tinyint(3) unsigned NOT NULL default '1',
  `alias` varchar(60) NOT NULL,
  `pname` varchar(12) NOT NULL,
  `file` varchar(100) NOT NULL,
  `tpl` varchar(100) NOT NULL,
  `rewrite` varchar(200) NOT NULL,
  `url` tinyint(3) unsigned NOT NULL default '0',
  `caching` int(10) unsigned NOT NULL default '0',
  `tag` varchar(60) NOT NULL,
  `title` varchar(200) NOT NULL,
  `description` varchar(200) NOT NULL,
  `keywords` varchar(200) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||page表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_payment`;
CREATE TABLE `qs_payment` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `listorder` int(10) unsigned NOT NULL default '50',
  `typename` varchar(15) NOT NULL,
  `byname` varchar(50) NOT NULL,
  `p_introduction` varchar(100) NOT NULL,
  `notes` text,
  `partnerid` varchar(80) default NULL,
  `ytauthkey` varchar(100) default NULL,
  `fee` varchar(6) NOT NULL default '0',
  `parameter1` varchar(50) default NULL,
  `parameter2` varchar(50) default NULL,
  `parameter3` varchar(50) default NULL,
  `p_install` tinyint(3) unsigned NOT NULL default '1',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||payment表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_personal_favorites`;
CREATE TABLE `qs_personal_favorites` (
  `did` int(10) unsigned NOT NULL auto_increment,
  `personal_uid` int(10) unsigned NOT NULL,
  `jobs_id` int(10) unsigned NOT NULL,
  `jobs_name` varchar(100) NOT NULL,
  `addtime` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`did`),
  KEY `personal_uid` (`personal_uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||personal_favorites表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_personal_jobs_apply`;
CREATE TABLE `qs_personal_jobs_apply` (
  `did` int(10) unsigned NOT NULL auto_increment,
  `resume_id` int(10) unsigned NOT NULL,
  `resume_name` varchar(60) NOT NULL,
  `personal_uid` int(10) unsigned NOT NULL,
  `jobs_id` int(10) unsigned NOT NULL,
  `jobs_name` varchar(60) NOT NULL,
  `company_id` int(10) unsigned NOT NULL,
  `company_name` varchar(60) NOT NULL,
  `company_uid` int(10) unsigned NOT NULL,
  `apply_addtime` int(10) unsigned NOT NULL,
  `personal_look` tinyint(3) unsigned NOT NULL default '1',
  `notes` varchar(200) NOT NULL,
  `is_reply` tinyint(1) unsigned NOT NULL default '0',
  `is_apply` tinyint(1) NOT NULL default '0',
  PRIMARY KEY  (`did`),
  KEY `personal_uid_id` (`personal_uid`,`resume_id`),
  KEY `company_uid_jobid` (`company_uid`,`jobs_id`),
  KEY `company_uid_look` (`company_uid`,`personal_look`),
  KEY `personal_uid_addtime` (`personal_uid`,`apply_addtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||personal_jobs_apply表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_plug`;
CREATE TABLE `qs_plug` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `typename` varchar(15) NOT NULL,
  `plug_name` varchar(50) NOT NULL,
  `p_install` tinyint(3) unsigned NOT NULL default '1',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||plug表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_pms`;
CREATE TABLE `qs_pms` (
  `pmid` int(10) unsigned NOT NULL auto_increment,
  `msgtype` tinyint(1) unsigned NOT NULL default '1',
  `msgfrom` varchar(30) NOT NULL,
  `msgfromuid` int(10) unsigned NOT NULL,
  `msgtouid` int(10) unsigned NOT NULL,
  `msgtoname` varchar(30) NOT NULL,
  `message` varchar(250) NOT NULL,
  `dateline` int(10) NOT NULL,
  `new` tinyint(1) unsigned NOT NULL default '1',
  `replytime` int(10) NOT NULL,
  `replyuid` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`pmid`),
  KEY `msgfromuid` (`msgfromuid`),
  KEY `msgtouid` (`msgtouid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||pms表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_pms_sys`;
CREATE TABLE `qs_pms_sys` (
  `spmid` int(10) unsigned NOT NULL auto_increment,
  `spms_usertype` tinyint(1) unsigned NOT NULL default '0',
  `spms_type` tinyint(1) NOT NULL default '1',
  `message` varchar(250) NOT NULL,
  `dateline` int(10) NOT NULL,
  PRIMARY KEY  (`spmid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||pms_sys表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_pms_sys_log`;
CREATE TABLE `qs_pms_sys_log` (
  `lid` int(10) unsigned NOT NULL auto_increment,
  `loguid` int(10) unsigned NOT NULL,
  `pmid` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`lid`),
  KEY `loguid` (`loguid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||pms_sys_log表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_promotion`;
CREATE TABLE `qs_promotion` (
  `cp_id` int(10) unsigned NOT NULL auto_increment,
  `cp_available` tinyint(1) NOT NULL default '1',
  `cp_promotionid` int(10) unsigned NOT NULL,
  `cp_uid` int(10) unsigned NOT NULL,
  `cp_jobid` int(10) unsigned NOT NULL,
  `cp_days` int(10) unsigned NOT NULL,
  `cp_starttime` int(10) unsigned NOT NULL,
  `cp_endtime` int(10) unsigned NOT NULL,
  `cp_val` varchar(160) NOT NULL,
  `cp_hour` tinyint(2) unsigned NOT NULL,
  `cp_hour_cn` varchar(30) NOT NULL,
  PRIMARY KEY  (`cp_id`),
  KEY `cp_uid` (`cp_uid`),
  KEY `cp_endtime` (`cp_endtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||promotion表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_promotion_category`;
CREATE TABLE `qs_promotion_category` (
  `cat_id` int(10) unsigned NOT NULL auto_increment,
  `cat_available` tinyint(1) NOT NULL default '1',
  `cat_name` varchar(30) NOT NULL,
  `cat_type` tinyint(3) unsigned NOT NULL,
  `cat_minday` smallint(5) unsigned NOT NULL default '0',
  `cat_maxday` int(10) unsigned NOT NULL default '0',
  `cat_points` int(10) NOT NULL default '0',
  `cat_notes` text NOT NULL,
  `cat_order` int(10) NOT NULL default '0',
  PRIMARY KEY  (`cat_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||promotion_category表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_refresh_log`;
CREATE TABLE `qs_refresh_log` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `mode` tinyint(1) unsigned NOT NULL default '0',
  `addtime` int(10) unsigned NOT NULL,
  `type` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||refresh_log表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_report`;
CREATE TABLE `qs_report` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `jobs_id` int(10) unsigned NOT NULL,
  `jobs_name` varchar(150) NOT NULL,
  `jobs_addtime` int(10) unsigned NOT NULL,
  `report_type` int(10) NOT NULL,
  `audit` int(10) NOT NULL default '1',
  `content` varchar(250) NOT NULL,
  `addtime` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||report表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_report_resume`;
CREATE TABLE `qs_report_resume` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `resume_id` int(10) unsigned NOT NULL,
  `title` varchar(150) NOT NULL default '',
  `resume_addtime` int(10) unsigned NOT NULL,
  `report_type` int(10) NOT NULL,
  `audit` int(10) NOT NULL default '1',
  `content` varchar(250) NOT NULL,
  `addtime` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||report_resume表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume`;
CREATE TABLE `qs_resume` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `display` tinyint(3) unsigned NOT NULL default '1',
  `display_name` tinyint(3) unsigned NOT NULL default '1',
  `audit` tinyint(3) unsigned NOT NULL default '1',
  `title` varchar(80) NOT NULL,
  `fullname` varchar(15) NOT NULL,
  `sex` tinyint(3) unsigned NOT NULL,
  `sex_cn` varchar(3) NOT NULL,
  `nature` tinyint(3) unsigned NOT NULL,
  `nature_cn` varchar(30) NOT NULL,
  `trade` varchar(60) NOT NULL,
  `trade_cn` varchar(100) NOT NULL,
  `birthdate` smallint(4) unsigned NOT NULL,
  `residence` varchar(30) NOT NULL default '',
  `height` tinyint(3) unsigned NOT NULL,
  `marriage` tinyint(3) unsigned NOT NULL,
  `marriage_cn` varchar(5) NOT NULL,
  `experience` smallint(5) NOT NULL,
  `experience_cn` varchar(30) NOT NULL,
  `district_cn` varchar(100) NOT NULL default '',
  `wage` tinyint(5) unsigned NOT NULL,
  `wage_cn` varchar(30) NOT NULL,
  `householdaddress` varchar(30) NOT NULL default '',
  `education` smallint(5) unsigned NOT NULL,
  `education_cn` varchar(30) NOT NULL,
  `major` smallint(5) NOT NULL,
  `major_cn` varchar(50) NOT NULL,
  `tag` varchar(50) NOT NULL,
  `tag_cn` varchar(100) NOT NULL,
  `telephone` varchar(50) NOT NULL,
  `email` varchar(60) NOT NULL,
  `email_notify` tinyint(1) unsigned NOT NULL default '1',
  `intention_jobs` varchar(100) NOT NULL,
  `specialty` varchar(200) NOT NULL,
  `photo` tinyint(1) unsigned NOT NULL default '0',
  `photo_img` varchar(80) NOT NULL,
  `photo_audit` tinyint(1) unsigned NOT NULL default '1',
  `photo_display` tinyint(1) unsigned NOT NULL default '1',
  `addtime` int(10) unsigned NOT NULL,
  `refreshtime` int(10) unsigned NOT NULL,
  `talent` tinyint(1) unsigned NOT NULL default '1',
  `level` tinyint(1) unsigned NOT NULL,
  `complete_percent` tinyint(3) unsigned NOT NULL default '0',
  `current` tinyint(5) unsigned NOT NULL,
  `current_cn` varchar(50) NOT NULL default '',
  `word_resume` varchar(255) NOT NULL,
  `key` text NOT NULL,
  `click` int(10) unsigned NOT NULL default '1',
  `tpl` varchar(60) NOT NULL,
  `resume_from_pc` tinyint(1) unsigned NOT NULL default '0',
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`),
  KEY `refreshtime` (`refreshtime`),
  KEY `addtime` (`addtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_credent`;
CREATE TABLE `qs_resume_credent` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `pid` int(10) unsigned NOT NULL,
  `uid` int(10) unsigned NOT NULL,
  `name` varchar(255) character set gbk NOT NULL,
  `year` int(4) NOT NULL,
  `month` int(2) NOT NULL,
  `images` varchar(255) character set gbk NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_credent表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_district`;
CREATE TABLE `qs_resume_district` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `pid` int(10) unsigned NOT NULL,
  `district` int(10) unsigned NOT NULL,
  `sdistrict` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `pid` (`pid`),
  KEY `district` (`district`,`sdistrict`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_district表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_education`;
CREATE TABLE `qs_resume_education` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `pid` int(10) unsigned NOT NULL,
  `uid` int(10) unsigned NOT NULL,
  `startyear` smallint(4) unsigned NOT NULL,
  `startmonth` smallint(2) unsigned NOT NULL,
  `endyear` smallint(4) unsigned NOT NULL,
  `endmonth` smallint(2) unsigned NOT NULL,
  `school` varchar(50) NOT NULL,
  `speciality` varchar(50) NOT NULL,
  `education` smallint(5) unsigned NOT NULL,
  `education_cn` varchar(30) NOT NULL default '',
  `todate` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_education表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_img`;
CREATE TABLE `qs_resume_img` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `resume_id` int(10) unsigned NOT NULL,
  `img` varchar(50) character set sjis NOT NULL default '',
  `title` varchar(20) character set sjis NOT NULL default '',
  `addtime` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`),
  KEY `resume_id` (`resume_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_img表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_jobs`;
CREATE TABLE `qs_resume_jobs` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `pid` int(10) unsigned NOT NULL,
  `topclass` int(10) unsigned NOT NULL,
  `category` int(10) unsigned NOT NULL,
  `subclass` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `pid` (`pid`),
  KEY `category` (`category`,`subclass`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_jobs表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_language`;
CREATE TABLE `qs_resume_language` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `pid` int(10) unsigned NOT NULL,
  `uid` int(10) unsigned NOT NULL,
  `language` smallint(5) NOT NULL,
  `language_cn` varchar(50) character set gbk NOT NULL,
  `level` smallint(5) unsigned NOT NULL,
  `level_cn` varchar(50) character set gbk NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_language表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_search_key`;
CREATE TABLE `qs_resume_search_key` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `display` tinyint(1) NOT NULL default '1',
  `audit` tinyint(1) unsigned NOT NULL,
  `uid` int(10) unsigned NOT NULL default '0',
  `sex` tinyint(3) unsigned NOT NULL default '1',
  `nature` tinyint(3) unsigned NOT NULL default '0',
  `marriage` tinyint(3) unsigned NOT NULL default '0',
  `experience` smallint(5) NOT NULL default '0',
  `district` smallint(5) unsigned NOT NULL default '0',
  `sdistrict` smallint(5) unsigned NOT NULL default '0',
  `wage` tinyint(5) unsigned NOT NULL default '0',
  `education` smallint(5) unsigned NOT NULL default '0',
  `major` smallint(5) NOT NULL,
  `current` smallint(5) unsigned NOT NULL default '0',
  `photo` tinyint(1) unsigned NOT NULL default '0',
  `refreshtime` int(10) unsigned NOT NULL,
  `talent` tinyint(1) unsigned NOT NULL default '1',
  `key` text NOT NULL,
  `likekey` varchar(220) NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`),
  FULLTEXT KEY `key` (`key`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_search_key表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_search_rtime`;
CREATE TABLE `qs_resume_search_rtime` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `display` tinyint(1) NOT NULL default '1',
  `audit` tinyint(1) unsigned NOT NULL,
  `uid` int(10) unsigned NOT NULL default '0',
  `sex` tinyint(3) unsigned NOT NULL default '1',
  `nature` tinyint(3) unsigned NOT NULL default '0',
  `marriage` tinyint(3) unsigned NOT NULL default '0',
  `experience` smallint(5) NOT NULL default '0',
  `district` smallint(5) unsigned NOT NULL default '0',
  `sdistrict` smallint(5) unsigned NOT NULL default '0',
  `wage` smallint(5) unsigned NOT NULL default '0',
  `education` smallint(5) unsigned NOT NULL default '0',
  `major` smallint(5) NOT NULL,
  `current` smallint(5) unsigned NOT NULL default '0',
  `photo` tinyint(1) unsigned NOT NULL default '0',
  `refreshtime` int(10) unsigned NOT NULL,
  `talent` tinyint(1) unsigned NOT NULL default '1',
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`),
  KEY `refreshtime` (`refreshtime`),
  KEY `district_rtime` (`district`,`refreshtime`),
  KEY `photo_rtime` (`photo`,`refreshtime`),
  KEY `sdistrict_rtime` (`sdistrict`,`refreshtime`),
  KEY `talent_rtime` (`talent`,`refreshtime`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_search_rtime表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_tag`;
CREATE TABLE `qs_resume_tag` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `pid` int(10) unsigned NOT NULL,
  `tag` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `pid` (`pid`),
  KEY `tag` (`tag`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_tag表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_trade`;
CREATE TABLE `qs_resume_trade` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `uid` int(10) unsigned NOT NULL,
  `pid` int(10) unsigned NOT NULL,
  `trade` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `pid` (`pid`),
  KEY `trade` (`trade`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_trade表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_training`;
CREATE TABLE `qs_resume_training` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `pid` int(10) unsigned NOT NULL,
  `uid` int(10) unsigned NOT NULL,
  `startyear` smallint(4) unsigned NOT NULL,
  `startmonth` smallint(2) unsigned NOT NULL,
  `endyear` smallint(4) unsigned NOT NULL,
  `endmonth` smallint(2) unsigned NOT NULL,
  `agency` varchar(50) NOT NULL,
  `course` varchar(50) NOT NULL,
  `description` varchar(255) NOT NULL,
  `todate` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_training表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_resume_work`;
CREATE TABLE `qs_resume_work` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `pid` int(10) unsigned NOT NULL,
  `uid` int(10) unsigned NOT NULL,
  `startyear` smallint(4) unsigned NOT NULL,
  `startmonth` smallint(2) unsigned NOT NULL,
  `endyear` smallint(4) unsigned NOT NULL,
  `endmonth` smallint(2) unsigned NOT NULL,
  `companyname` varchar(50) NOT NULL,
  `jobs` varchar(30) NOT NULL,
  `achievements` varchar(255) NOT NULL,
  `todate` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||resume_work表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_setmeal`;
CREATE TABLE `qs_setmeal` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `display` tinyint(3) unsigned NOT NULL default '1',
  `apply` tinyint(3) unsigned NOT NULL default '1',
  `setmeal_name` varchar(200) NOT NULL,
  `days` int(10) unsigned NOT NULL default '0',
  `original_price` int(10) unsigned NOT NULL,
  `expense` int(10) unsigned NOT NULL,
  `jobs_ordinary` int(10) unsigned NOT NULL default '0',
  `download_resume_ordinary` int(10) unsigned NOT NULL default '0',
  `download_resume_senior` int(10) unsigned NOT NULL default '0',
  `interview_ordinary` int(10) unsigned NOT NULL default '0',
  `interview_senior` int(10) unsigned NOT NULL default '0',
  `talent_pool` int(10) unsigned NOT NULL default '0',
  `recommend_num` int(10) unsigned NOT NULL,
  `recommend_days` int(10) unsigned NOT NULL,
  `stick_num` int(10) unsigned NOT NULL,
  `stick_days` int(10) unsigned NOT NULL,
  `emergency_num` int(10) unsigned NOT NULL,
  `emergency_days` int(10) unsigned NOT NULL,
  `highlight_num` int(10) unsigned NOT NULL,
  `highlight_days` int(10) unsigned NOT NULL,
  `jobsfair_num` int(10) unsigned NOT NULL,
  `change_templates` tinyint(1) unsigned NOT NULL default '0',
  `map_open` tinyint(1) unsigned NOT NULL default '0',
  `added` varchar(255) NOT NULL,
  `show_order` int(10) unsigned NOT NULL default '0',
  `refresh_jobs_space` int(10) unsigned NOT NULL default '0',
  `refresh_jobs_time` int(10) unsigned NOT NULL default '0',
  `set_sms` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||setmeal表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_simple`;
CREATE TABLE `qs_simple` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `audit` tinyint(1) unsigned NOT NULL default '0',
  `pwd` varchar(60) NOT NULL,
  `pwd_hash` varchar(30) NOT NULL,
  `jobname` varchar(100) NOT NULL,
  `amount` smallint(3) unsigned NOT NULL default '0',
  `comname` varchar(100) NOT NULL,
  `contact` varchar(30) NOT NULL,
  `tel` varchar(100) NOT NULL,
  `district` smallint(5) unsigned NOT NULL,
  `district_cn` varchar(20) NOT NULL,
  `sdistrict` smallint(5) unsigned NOT NULL,
  `sdistrict_cn` varchar(20) NOT NULL,
  `detailed` text NOT NULL,
  `addtime` int(10) unsigned NOT NULL,
  `deadline` int(10) unsigned NOT NULL,
  `refreshtime` int(10) unsigned NOT NULL,
  `click` int(10) unsigned NOT NULL default '1',
  `addip` varchar(80) NOT NULL,
  `key` text NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `tel` (`tel`),
  KEY `audit_refreshtime` (`audit`,`refreshtime`),
  KEY `audit_click` (`audit`,`click`),
  KEY `deadline` (`deadline`),
  FULLTEXT KEY `key` (`key`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||simple表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_smsqueue`;
CREATE TABLE `qs_smsqueue` (
  `s_id` int(10) unsigned NOT NULL auto_increment,
  `s_type` tinyint(3) unsigned NOT NULL default '0',
  `s_addtime` int(10) unsigned NOT NULL,
  `s_sendtime` int(10) unsigned NOT NULL default '0',
  `s_uid` int(10) unsigned NOT NULL default '0',
  `s_mobile` text,
  `s_body` varchar(100) NOT NULL,
  PRIMARY KEY  (`s_id`),
  KEY `s_uid` (`s_uid`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

||-_-||smsqueue表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_sms_config`;
CREATE TABLE `qs_sms_config` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL,
  `value` varchar(200) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||sms_config表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_sms_templates`;
CREATE TABLE `qs_sms_templates` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL,
  `value` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||sms_templates表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_syslog`;
CREATE TABLE `qs_syslog` (
  `l_id` int(10) unsigned NOT NULL auto_increment,
  `l_type` tinyint(1) unsigned NOT NULL,
  `l_type_name` varchar(30) NOT NULL,
  `l_time` int(10) unsigned NOT NULL,
  `l_ip` varchar(20) NOT NULL,
  `l_address` varchar(50) NOT NULL,
  `l_page` text NOT NULL,
  `l_str` text NOT NULL,
  PRIMARY KEY  (`l_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||syslog表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_sys_email_log`;
CREATE TABLE `qs_sys_email_log` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `send_from` varchar(50) NOT NULL,
  `send_to` varchar(50) NOT NULL,
  `subject` varchar(100) NOT NULL,
  `body` varchar(255) NOT NULL,
  `state` smallint(3) NOT NULL,
  `sendtime` int(10) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||sys_email_log表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_text`;
CREATE TABLE `qs_text` (
  `id` smallint(5) unsigned NOT NULL auto_increment,
  `name` varchar(100) NOT NULL,
  `value` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||text表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_tpl`;
CREATE TABLE `qs_tpl` (
  `tpl_id` int(10) unsigned NOT NULL auto_increment,
  `tpl_type` tinyint(1) NOT NULL,
  `tpl_name` varchar(80) NOT NULL,
  `tpl_display` tinyint(1) NOT NULL default '1',
  `tpl_dir` varchar(80) NOT NULL,
  `tpl_val` int(10) NOT NULL default '0',
  PRIMARY KEY  (`tpl_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||tpl表创建成功！||-_-||

DROP TABLE IF EXISTS `qs_weixin_menu`;
CREATE TABLE `qs_weixin_menu` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `parentid` int(10) unsigned NOT NULL default '0',
  `title` varchar(30) NOT NULL,
  `key` varchar(30) NOT NULL,
  `type` varchar(10) NOT NULL,
  `url` varchar(255) NOT NULL,
  `menu_order` smallint(5) unsigned NOT NULL default '0',
  `status` tinyint(1) unsigned NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk;

||-_-||weixin_menu表创建成功！||-_-||
