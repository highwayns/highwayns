<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Brickred.SocialAuth.NET.Demo.Default"
    MasterPageFile="~/DemoSite.Master" %>

<%@ Register Src="SocialAuthLogin.ascx" TagName="SocialAuthLogin" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <h3>
            Please select one or more provider(s) from above!</h3> 
            <i><small>To enable automatic UI, Set LoginURL="" in Web.Config.</small></i>
<div id="divContent" runat="server"></div>
    <br /><br />

   
</asp:Content>
