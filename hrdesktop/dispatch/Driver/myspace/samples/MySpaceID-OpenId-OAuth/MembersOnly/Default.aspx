<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MySpaceID_OpenId_OAuth.MembersOnly.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>MySpaceID-OpenID-OAuth Hybrid</title>
</head>
<body>
    <form id="form1" runat="server">
   
    <div id="divLoggedIn" runat="server" visible="true">
        <h3>Your MySpace Profile Information</h3>
        <asp:Image ID="imageProfile" runat="server"  />
        <ul>
            <li>Name: <asp:Label ID="lblName" runat="server" /></li>
            <li>Profile Url: <asp:Label ID="lblProfileUrl" runat="server" /></li>
            <li>Gender: <asp:Label ID="lblGender" runat="server" /></li>
            <li>Age: <asp:Label ID="lblAge" runat="server" /></li>
            <li>Marital Status: <asp:Label ID="lblMarital" runat="server" /></li>
            <li>City: <asp:Label ID="lblCity" runat="server" /></li>
            <li>Postal Code: <asp:Label ID="lblPostalCode" runat="server" /></li>
            <li>Region: <asp:Label ID="lblRegion" runat="server" /></li>
            <li>Country: <asp:Label ID="lblCountry" runat="server" /></li>
        </ul>
        <h3>Your Photos</h3>
        <div id="divPhotos" runat="server" />
    </div>
    </form>
</body>
</html>
