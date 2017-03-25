<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdvanceApiDemo.aspx.cs"
    Inherits="Brickred.SocialAuth.NET.Demo.AdvanceApiDemo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-family: cambria;
        }
        h4
        {
            background: #dddddd;
            margin-top: 40px;
            padding: 10px 10px 10px 10px;
            color: #ffffff;
            font-weight: bold;
            font-family: verdana;
        }
        
        button, input[type="submit"]
        {
            background: #000000;
            color: #ffffff;
            font-size: 12px;
            font-weight: bold;
            padding: 5px 5px 5px 5px;
        }
    </style>
    <script src="scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#btnGetFriends').click(function (e) {
                e.preventDefault();
                $.ajax(
                    { url: "AdvanceApiDemo.aspx/getfriends",
                        dataType: "json",
                        type: "GET",
                        contentType: "application/json",
                        success: (function (data) {
                            var friendsJson = JSON.stringify(JSON.parse(data.d).data);
                            $('#spanFriends').html(friendsJson);
                        })
                    }
                );
            });
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:DropDownList runat="server" ID="lstProviders"></asp:DropDownList>
    <div>
        <h4>
            Step-1: Generate Redirect Url
        </h4>
        <asp:Button runat="server" ID="btnGetLoginUrl" Text="Step-1: Get Login Url" OnClick="btnGetLoginUrl_Click" />
        Call the <b>GetLoginRedirectURL()</b> API and redirect user manually to this Url.
        <br />
        <asp:HyperLink runat="server" ID="redirectUrl"></asp:HyperLink>
        <h4>
            Step-2: Get Access Token
        </h4>
        <input type="text" id="txtRedirectResponse" runat="server" size="100" placeholder="Copy the URL received upon user authentication" />
        <asp:Button runat="server" ID="btnConnect" Text="Step-2: Establish Connection" OnClick="btnConnect_Click" />
        <asp:Label runat="server" ID="lblAccessToken"></asp:Label>
        <h4>
            Step-3: Get User Profile With Postback
        </h4>
        <asp:Button runat="server" ID="Button1" Text="Get Profile" OnClick="btnGetProfile_Click" />
        <asp:Label runat="server" ID="lblProfileData"></asp:Label>
        <h4>
            Step-4: Get User Friends With AJAX
        </h4>
        <button id="btnGetFriends">
            Get Friends</button>
        <span id="spanFriends"></span>
    </div>
    </form>
</body>
</html>
