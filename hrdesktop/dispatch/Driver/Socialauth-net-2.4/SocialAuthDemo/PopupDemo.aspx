<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupDemo.aspx.cs" Inherits="Brickred.SocialAuth.NET.Demo.PopupDemo"
    MasterPageFile="~/DemoSite.Master" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="scripts/jquery.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui.min.js"></script>
    
    <link rel="stylesheet" href="jquery.ui.all.css" type="text/css" />
    <style type="text/css">
        img
        {
            border: 0px;
        }
        
        #provider-container
        {
            height: 100px;
            width: 350px;
            background-color: #000000;
            color: #ffffff;
            display: none;
        }
    </style>
    <script type="text/javascript">

        //Document.Ready
        $(function () {
            $('#status').html('Select a provider');

            //Modal Dialog should open on click of login here
            $("#alogin").click(function () {
                setStatus("please select a provider", "");
                $("#provider-container").dialog({ modal: true });
                return false;
            });

        });

        //When a provider is selected:
        function login(providername) {
            setStatus("connecting with " + providername, "waiting");
            //add a progress logo
            var hWin = window.open("popupProcess.aspx?provider=" + providername, "socialauthlogin", "height=500,width=400,left=750,screenX=750,top=100,screenY=100, scrollbars=yes", true);
            var timerID = setInterval(function () {

                isPopupClosed = (hWin.closed); //hWin == null || 
                if (isPopupClosed) {
                    clearInterval(timerID)
                    //make AJAX call to check if user logged in or not. If he did, get friends list and display!
                    PageMethods.IsUserLoggedIn(isUserLoggedInResponse);
                }
            }, 3000);

            return false;
        }

        //SocialAuthUser.IsLoggedIn(provider type) response
        function isUserLoggedInResponse(data) {
            if (data == "True") {
                PageMethods.GetFriends(GetFriendsResponse)
                setStatus("connected..! getting friends", "waiting");
            }
            else {
                setStatus("Authentication process failed", "err");
            }
        }

        //Friends received from server
        function GetFriendsResponse(data) {
            var i = 0;
            if (data == "error") {
                setStatus("some error occurred", "error");
                return;
            }
            var friends = jQuery.parseJSON(data);
            $("#provider-container").dialog('close');
            $('#alogin').hide();
            $.each(friends, function (x) {
                //if profile picture exists show it along with name, else just show name!
                if (friends[x].ProfilePictureURL != "" && friends[x].ProfilePictureURL != null)
                    $('#friendsDiv').append("<img src='" + friends[x].ProfilePictureURL + "'/>" + friends[x].Name + "<br/>");
                else
                    $('#friendsDiv').append(friends[x].Name + "<br/>");
                if (++i == 10) {
                    //Show only up to 10 friends
                    return false;
                }
            });


        }

        //Just a refactoring to set message at bottom of jQuery modal dialog
        function setStatus(message, type) {
            if (type == "err") {
                img = "<img src='images/err.gif'/>"
            }
            else if (type == "waiting") {
                img = "<img src='images/waiting.gif' />"
            }
            else {
                img = "";
            }
            $('#status').html(img + " &nbsp;" + message);

        }

        //When close is clicked on modal
        function closeModelDialog() {
            $("#provider-container").dialog('close');
        }

    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <h3>
        Popup-Login Demo<sup><small>new in v2.2</small></sup></h3>
    Some of your friends are:
    <div id="friendsDiv" />
    <button id="alogin">
        Login in</button>
    <div id="provider-container" title="SocialAuth.NET Login">
        <center>
            <a href="" onclick="return login('facebook')">
                <img alt="login with facebook" src="images/SocialAuthIcons/facebook.png" /></a>
            <a href="" onclick="return login('google')">
                <img alt="login with google" src="images/SocialAuthIcons/google.png" /></a>
            <br />
            <a href="#" onclick="closeModelDialog()" style="color: Gray">Close</a>
            <br />
            <span id="status" />
        </center>
    </div>
</asp:Content>
