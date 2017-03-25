<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Finish_Auth.aspx.cs" Inherits="_8BitMusic.Finish_Auth" %>

<html>
<head>
<title>8-Bit Music</title>
	
<link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.6.0/build/reset/reset-min.css" />
<link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.6.0/build/fonts/fonts-min.css" />
<link rel="stylesheet" type="text/css" href="http://x.myspacecdn.com/modules/activity/static/css/activities_tyq3zov4.css" />
<link rel="stylesheet" type="text/css" href="/static/main.css" />

<script type="text/javascript" src="/static/js/myspaceid.rev.0.js" ></script>
<!--[if IE]><script type="text/javascript" src="http://remysharp.com/downloads/html5.js"></script><![endif]-->
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.js" ></script>

</head>
<body>

<script>
function closeWin() {
//  alert("closeWin() called");
//  alert(opener);

//window.opener.location.href = "profile.php";
//  window.opener.location.reload(true);
var rand = Math.random();

//alert(rand);

  window.opener.success(rand);
  self.close();
}
</script>





    <h1>Finishing Log In</h1>



    <br>



<script>closeWin();</script>
  </body>
</html>
