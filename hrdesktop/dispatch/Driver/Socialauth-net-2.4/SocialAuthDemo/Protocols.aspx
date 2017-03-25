<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Protocols.aspx.cs" Inherits="Brickred.SocialAuth.NET.Demo.Protocols"
    MasterPageFile="~/DemoSite.Master" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .bigText
        {
            font-size: 15px;
           
        }
        
    
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        Where SocialAuth1.x was built on provider specific architecture, SocialAuth2.0 is
        based on specifications. Supported specifications are OAuth1.0a, OAuth2.0 and OpenID+Oauth1.0
        Hybrid protocol. Following is a summary of which provider is implemeted with which
        protocol's specification. It is now very simple to add any new provider if it supports
        any of these 3 specifications.
    
    <div>
        <table>
            <tr>
                <td>
                    <img src="images/Socialauthicons/facebook.png" alt="facebook" />
                </td>
                <td class="bigText">
                    Implemented using OAuth 2.0
                </td>
                <td>
                    <img src="images/oauth2_small.png" alt="OAuth 2.0" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="images/Socialauthicons/msn.png" alt="msn" />
                </td>
                <td class="bigText">
                    Implemented using OAuth 2.0
                </td>
                <td>
                    <img src="images/oauth2_small.png" alt="OAuth 2.0" />
                </td>
            </tr>
             <tr>
                <td>
                    <img src="images/Socialauthicons/google.png" alt="google" />
                </td>
                <td class="bigText">
                    Implemented using OAuth2.0
                </td>
                <td>
                     <img src="images/oauth2_small.png" alt="OAuth 2.0" />
                </td>
            </tr>
             <tr>
                <td>
                    <img src="images/Socialauthicons/LinkedIn.png" alt="LinkedIn" />
                </td>
                <td class="bigText">
                    Implemented using OAuth 2.0
                </td>
                <td>
                    <img src="images/oauth2_small.png" alt="OAuth 2.0" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="images/Socialauthicons/yahoo.png" alt="yahoo" />
                </td>
                <td class="bigText">
                    Implemented using OAuth+OpenID
                </td>
                <td>
                    <img src="images/hybrid_small.png" alt="OAuth 1.0 Hybrid" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="images/Socialauthicons/twitter.png" alt="twitter" />
                </td>
                <td class="bigText">
                    Implemented using OAuth 1.0a
                </td>
                <td>
                    <img src="images/oauth_small.png" alt="OAuth 1.0a" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="images/Socialauthicons/myspace.png" alt="myspace" />
                </td>
                <td class="bigText">
                    Implemented using OAuth 1.0a
                </td>
                <td>
                    <img src="images/oauth_small.png" alt="OAuth 1.0a" />
                </td>
            </tr>
             <tr>
                <td>
                    <img src="images/Socialauthicons/googlehybrid.png" alt="google" />
                </td>
                <td class="bigText">
                    Implemented using OAuth+OpenID
                </td>
                <td>
                    <img src="images/hybrid_small.png" alt="OAuth 1.0 Hybrid" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="images/Socialauthicons/LinkedIn1.png" alt="LinkedIn" />
                </td>
                <td class="bigText">
                    Implemented using OAuth 1.0a
                </td>
                <td>
                    <img src="images/oauth_small.png" alt="OAuth 1.0a" />
                </td>
            </tr>
            
        </table>
    </div>
</asp:Content>
