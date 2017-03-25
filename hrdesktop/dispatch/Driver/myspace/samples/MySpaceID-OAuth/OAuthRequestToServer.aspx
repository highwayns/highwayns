<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OAuthRequestToServer.aspx.cs" Inherits="MySpaceID_OAuth.OAuthRequestToServer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="btnRequest" runat="server" onclick="btnRequest_Click" 
            Text="Make OAuth Request" />
    
    </div>
    </form>
</body>
</html>
