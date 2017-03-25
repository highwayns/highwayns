<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManualLogin.aspx.cs" Inherits="Brickred.SocialAuth.NET.Demo.ManualLogin"
    MasterPageFile="~/DemoSite.Master" %>

<%@ Register Src="SocialAuthLogin.ascx" TagName="SocialAuthLogin" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Login(providerName) {
            window.location.href = 'socialauth/login.sauth?returnUrl=ManualLogin.aspx&p=' + providerName 
        }

    </script>
    <style type="text/css">
        .tdControl
        {
            background-color: Purple;
            color: White;
            width: 60px;
            text-align: center;
        }
        
         .tdControl :hover
        {
            background-color: Yellow;
            color: black;
            cursor:hand;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <!----------------------------------------------------------------
     To use this page for login instead of automatically generated UI,
    change LoginUrl from blank to "ManualLogin.aspx" in Web.Config!
     <Authentication Enabled="true" LoginUrl="ManualLogin.aspx" DefaultUrl="Welcome.aspx" />
    !---------------------------------------------------------------->
        
        <h3>
            ASP.NET controls</h3>
        <asp:Button ID="btnFacebook" runat="server" OnClick="btn_Click" Text="Facebook" />
        <asp:Button ID="btnGoogle" runat="server" OnClick="btn_Click" Text="Google" />
        <asp:Button ID="btnYahoo" runat="server" OnClick="btn_Click" Text="Yahoo" />
        <asp:Button ID="btnMSN" runat="server" OnClick="btn_Click" Text="MSN" />
        <br />
        <br />
        <h3>
            Simple HTML Controls</h3>
        <table border="0" style="background: orange; font-size: 12px;"
            cellpadding="5" cellspacing="2">
            <tr>
                <td class="tdControl" onclick="Login('facebook')">
                    Facebook
                </td>
                <td class="tdControl"  onclick="Login('google')">
                    Google
                </td>
                <td  class="tdControl" onclick="Login('yahoo')">
                    Yahoo
                </td>
                <td  class="tdControl" onclick="Login('msn')">
                    MSN
                </td>
                <td  class="tdControl" onclick="Login('twitter')">
                    Twitter
                </td>
            </tr>
        </table>
        <br />
        <br />
        <h3>
            SocialAuth.NET User Control</h3>
    </div>
    <uc1:SocialAuthLogin ID="SocialAuthLogin1" runat="server" />
</asp:Content>
