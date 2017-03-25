<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MySpaceID_OpenId_OAuth.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MySpaceID using the OpenID+OAuth Hybrid</title>
</head>
<body>
    <form id="form1" runat="server">
    <h2>MySpaceID Login</h2>
	<div>
	<asp:Label ID="lblErrorMessage" runat="server" Visible=false ForeColor="Red" />
	<br />
	<asp:Label ID="Label1" runat="server" Text="OpenID Login" />
	<asp:TextBox ID="openIdBox" Width="300px" Text="http://www.myspace.com/VANITY_NAME" runat="server" />
	<asp:Button ID="loginButton" runat="server" Text="Login" OnClick="loginButton_Click" />
	</div>
	
	<h2>MySpaceID Direct Identity</h2>
	<div>
	<asp:HyperLink ID="linkDirectIdentity" runat=server ImageUrl="~/images/loginwid.jpg" />	
	</div>
    
	<asp:CustomValidator runat="server" ID="openidValidator" ErrorMessage="Invalid OpenID Identifier"
		ControlToValidate="openIdBox" EnableViewState="false" OnServerValidate="openidValidator_ServerValidate" />
	<br />
	<asp:Label ID="loginFailedLabel" runat="server" EnableViewState="False" Text="Login failed"
		Visible="False" />
	<asp:Label ID="loginCanceledLabel" runat="server" EnableViewState="False" Text="Login canceled"
		Visible="False" />
    </form>
</body>
</html>
