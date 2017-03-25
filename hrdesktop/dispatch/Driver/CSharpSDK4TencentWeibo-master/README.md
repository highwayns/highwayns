CSharpSDK4TencentWeibo
======================

实现了Oauth1.0 登录，以及一些常见的接口。
腾讯微博开放平台C# SDK
支持环境 windows xp， windows 7  (VS2010, .NET 4.0,  低版本的.Net需要自己手动配置)


日期： 2011-06-20


工程配置：分成两个工程QWeiboSDK 和 QAPITool

QWeiboSDK 

SDK 分为两个层级
  第一级： QWeiboRequest.cs 提供了同步异步两种访问网络的基本接口（SyncRequest，AsyncRequest），这两个接口可配置性比较强一些

，同时需要配置的参数也比较多。
	第二级：在第一级接口上分别实现了各类api， 下面是对应关系
		 QauthKey.cs  	OAuth 授权
		 statuses.cs  	时间线
		 t.cs		微博相关
		 user.cs, 	帐户相关	
		 friends.cs	关系链相关
		 private.cs	私信相关
		 search.cs	搜索相关
		 trends.cs	热度，趋势
		 info.cs	数据更新相关
		 fav.cs		数据收藏
		 ht.cs		话题相关
		 tag.cs		标签相关
		 other.cs 	其他

上面的api 与 该api对应的uri有对应关系，比如，时间线的uri为 statuses/xxxx（statuses/home_timeline），  那么它对应的接口类

为statuses, 而对应的接口为xxxx() 
	  如home_timeline().

QAPITool
	是测试例程，选择了两个比较有代表性的接口做了应用演示，其一为主页时间线statuses/home_timeline（GET）, 其二是t/add_pic发表

一条带图片的微博（POST, 有图片）。 其他接口使用与这两个类似。ps 一下，例程中verifier要到浏览器中地址栏中去取。



编译选项：

目前提供两个编译选项 Debug 和 Release

