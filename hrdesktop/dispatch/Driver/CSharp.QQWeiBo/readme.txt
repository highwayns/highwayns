2011年6月30日，三个改进
1、修正appkey放在配置文件内发不了微博的问题
2、图片文件不用上传到服务器
3、将发送结果值直接显示在网页上

应网友要求，发布一个Visual Studio 2008可以打开的腾讯微博开放API的SDK
有如下几个改进
1.使用Visual Studio 2008
2.去除Wintell.Threading.dll依赖，删除了相关代码

演示地址 http://t.qq1800.com/demo.aspx
SDK源代码下载 http://code.google.com/p/txweibo-sdk-csharp/downloads/list
联系我：http://t.qq.com/microsoftmediaroom 腾讯微博开发者群：87926499 （找笨笨石头.NET) email:qq1800@gmail.com

配置
	1. 微博callback url设置：把web.config里的webroot的值改成你的站点的Default.aspx的实际URL，如http://t.qq1800.com/demo.aspx
	2. Appkey, 改用自己的腾讯微博appkey，把web.config中的appkey和appsecret改成自己从open.t.qq.com申请到的值
	
	
代码说明：
	1. WeiBeeCommon - 微博SDK公共库
		a. Core目录 － 微博核心功能
			i. oAuth.cs - OAuth基础类
			ii. oAuthQQ.cs - 腾讯微博oAuth类
			iii. IWeiBee.cs - 微博基础类，所有微博功能要实现的接口
			iv. WeiBeeQQ.cs - 腾讯微博类，发微博，获取时间线等实现
			v. Utility.cs - 公用的基础方法
			vi. WebHelper.cs - 更基础的web帮助方法
		b. Helpers
			i. WeiBeeAdd.cs - 发微博（包括图片文字）数据类型
		
	2. DemoWeb目录，Web应用示例程序
		a. Images\*.* 图片资源
		b. Web.config - 网站配置文件，要修改appSettings里的值
		c. demo.aspx, demo.aspx.cs - http://t.qq1800.com/demo.aspx 示例网站入口及发微博，发图片微博

