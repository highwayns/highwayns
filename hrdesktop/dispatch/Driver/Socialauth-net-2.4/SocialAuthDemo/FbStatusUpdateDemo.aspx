<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FbStatusUpdateDemo.aspx.cs" Inherits="Brickred.SocialAuth.NET.Demo.FbStatusUpdateDemo" MasterPageFile="~/DemoSite.Master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" />

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h3>POST & VIEW STATUS UPDATES- Custom Feed Execution Demo (POST, OAuth 2.0)</h3>
    <div style="float:left">
        <asp:TextBox ID="txtStatus" runat="server" />
        <asp:Button ID="btnPOST" runat="server" Text="POST STATUS" onclick="btnPOST_Click" />
        <asp:Button ID="btnGET" runat="server" Text="REFRESH UPDATES" onclick="btnGET_Click" />
        <asp:Label ID="errLabel" runat="server" ForeColor="Red" />
        <div id="divUpdates" runat="server" style="font-family:Segoe Condensed;">
        </div>
    </div>
</asp:Content>