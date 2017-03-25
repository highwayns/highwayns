<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POSTdemo.aspx.cs" Inherits="Brickred.SocialAuth.NET.Demo.POSTdemo" MasterPageFile="~/DemoSite.Master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" />

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h3>POST & VIEW TWEETS - Custom Feed Execution Demo</h3>
    <div style="float:left">
        <asp:TextBox ID="txtTweet" runat="server" />
        <asp:Button ID="btnPOST" runat="server" Text="POST TWEET" onclick="btnPOST_Click" />
        <asp:Button ID="btnGET" runat="server" Text="REFRESH TWEETS" onclick="btnGET_Click" />
        <asp:Label ID="errLabel" runat="server" ForeColor="Red" />
        <div id="divTweets" runat="server" style="font-family:Segoe Condensed;">
        </div>
    </div>
</asp:Content>