<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomFeedDemo.aspx.cs"
    Inherits="Brickred.SocialAuth.NET.Demo.CustomFeedDemo" MasterPageFile="~/DemoSite.Master" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .largecontent
        {
            width:700px;
            font-size:12px;
            word-wrap:break-word;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>

    To try this demo, you must be logged into Facebook. <br />
    Also, add an additional scope "user_photos" to Web.config:<br /><br />
    <div style="background:#EEEEF7;padding:10px 0px 10px 0px">
    <code>
    &lt;add WrapperName="FacebookWrapper" ConsumerKey="152190004803645" ConsumerSecret="64c94bd02180b0ade85889b44b2ba7c4" <b>AdditionalScopes="user_photos"</b>/&gt;
    </code>
    </div>
        
        <asp:Button ID="btnCustomFeed" runat="server" OnClick="btnCustomFeed_Click" Text="Execute Custom Feed" /><br />
        <asp:Label ID="lblJson" runat="server" class="largecontent" /><br />
        <asp:Label ID="lblAlbum" runat="server" />
    </div>
    
</asp:Content>