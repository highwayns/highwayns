<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetToken.aspx.cs" Inherits="Brickred.SocialAuth.NET.Demo.GetToken" MasterPageFile="~/DemoSite.Master" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.title
{
    height:20px;
    width:100%;
    background-color:#c0c0c0;
    color:#ffffff;
    padding:5px 5px 5px 5px;
    font-size:15px;
    font-weight:bold;
    font-family:Tahoma;
}
</style>
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
     <div>
     
     <h3>Save/Restore Tokens <sup><small>new in v2.1</small></sup></h3>
     SocialAuth v2.1 adds feature of storing & loading tokens. This feature allows to save authenticated user tokens into a repository (file/database) and connect user directly with provider without repeating authentication process.
     <br /><br />
     <span class="title">General Process</span><br />
     Following is a general process flow for this:
     <br /><br />
     1. User logs into a provider for first time<br />
     2. Access_Token is saved into some repository like database or file server along with some mechanism to identify associated user next time (likely a cookie-browser solution)<br />
     3. When same user opens website next time, call new LoadToken(<i>token</i>, <i>returnUrl</i>) to mark user as logged in and bypass authentication process.
   

     <br /><br /><span class="title">How to Try the Demo</span><br />
     
     1. Connect with any provider, click "download token" to save token on your disk.<br />
     2. Logout user.<br />
     3. Upload saved token and SocialAuth.NET will automatically log user in.<br /><br />
     

     
       
     This is a bare basic example that illustrates how LoadToken works.
        <br /><br />
        Connected Providers: <asp:DropDownList ID="ddlConnectedProviders" runat="server" />
        <asp:Button ID="btnDownload" runat="server" Text="Download Token" 
            onclick="btnDownload_Click" />
        <br />
        <br />
           Browse token file <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="btnUpload" 
            runat="server" Text="Restore Token" onclick="btnUpload_Click" />

     
        <br /><br /><span class="title">Things to remember!</span><br />
        1. Generally token received from providers are short lived. In order to use this feature, request for a long lived token. For example, Facebook requires "offline_access" scope to be added for a long lived token. Application will throw an exception when trying to execute a data feed with an expired token.<br />
        2. When storing tokens, it is a good practice to encrypt them before saving<br />
        3. Remember, using token and logging in user automatically is definitely not a good approach for site with sensitive data.
        4. SocialAuth.NET doesn't automatically refresh access token if it has expired. You can check ExpiresOn property to check when does the token expires and then call LoadToken accordingly. However, SocialAuth.NET too checks this and throws "Token has expired!" error. As a note, some providers do not provide expiry information. This usually happens when token is long lived. In such cases ExpiresOn property will contain DateTime.MinValue.
        
    </div>
</asp:Content>