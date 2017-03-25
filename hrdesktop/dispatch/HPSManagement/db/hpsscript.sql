insert into 用户(UserID,	UserName,	UserPwd,	UserRight,	UpperUserID)
values('admin001',	'admin001',	'HpMfeOisg5Cu1Im/jYONXg==',	'超级管理员',	NULL);
GO	
insert into 用户(UserID,	UserName,	UserPwd,	UserRight,	UpperUserID)
values('user001',	'user001',	'npJSwa97qPpHZs07SPSC1g==',	'客户管理员',	'admin001');
GO
insert into 用户(UserID,	UserName,	UserPwd,	UserRight,	UpperUserID)
values('user002',	'user002',	'npJSwa97qPpHZs07SPSC1g==',	'期刊管理员',	'admin001');
GO
insert into 用户(UserID,	UserName,	UserPwd,	UserRight,	UpperUserID)
values('user003',	'user003',	'npJSwa97qPpHZs07SPSC1g==',	'用户管理员',	'admin001');
GO
insert into 用户(UserID,	UserName,	UserPwd,	UserRight,	UpperUserID)
values('user004',	'user004',	'npJSwa97qPpHZs07SPSC1g==',	'普通用户',	'user003');
GO