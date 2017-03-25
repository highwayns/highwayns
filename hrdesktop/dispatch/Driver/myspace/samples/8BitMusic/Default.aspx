<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_8BitMusic.Default" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>8-Bit Music</title>
    <link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.6.0/build/reset/reset-min.css" />
    <link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.6.0/build/fonts/fonts-min.css" />
    <link rel="stylesheet" type="text/css" href="http://x.myspacecdn.com/modules/activity/static/css/activities_tyq3zov4.css" />
    <link rel="stylesheet" type="text/css" href="/static/main.css" />

    <script type="text/javascript" src="/static/js/myspaceid.rev.0.js"></script>

    <!--[if IE]><script type="text/javascript" src="http://remysharp.com/downloads/html5.js"></script><![endif]-->

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.js"></script>

</head>
<body>
    <div id="login" class='rounded-corner-bottom'>
        <div class='logout'>
            <span class="<%= this.IsLoggedIn() ? "logout_isSignedin":"logout_isSignedout" %>"><a
                href="/logout.aspx" title="signout">[x] signout</a> </span>
        </div>
        <div class='login'>
            <span class="<%= this.IsLoggedIn() ? "login_isSignedin":"login_isSignedout" %>"><b>please
                login:</b><br>
                <br>
                <a href="#login" class="msid__login" onclick="p2()">
                    <img src="/static/images/myspaceid.png" alt="Login with MySpaceID" />
                </a></span>
        </div>
    </div>
    <div id="branding" class='rounded-corner-bottom'>
        8-Bit Music</div>
    <ul id="nav">
        <li><a href="/">HOME</a></li>
        <li><a href="#">MAP</a></li>
        <li><a href="#">SONGS</a></li>
        <li><a href="#">PROFILE</a></li>
        <li><a href="#">FEATURED</a></li>
    </ul>
    <div id="content_loggedOut" class="<%= this.IsLoggedIn() ? "welcome_LoggedIn":"welcome_LoggedOut" %>">
        <div style="background-color: #CCCCCC; padding: 3px;">
            <div style="background-color: #FFFFFF; text-align: center; padding: 25px;">
                Welcome to 8-Bit Music: a community to share and discover video game tunes. Please
                login!<br>
                <br>
                <img src="/static/images/profile_pic.png" />
            </div>
        </div>
    </div>
    <div id="content" class="<%= this.IsLoggedIn() ? "content_loggedIn":"content_loggedOut" %>">
        <ul id="cols">
            <li id="profile" class='rounded-corner-left'>
                <%

                    if (this.IsLoggedIn() && this.ExtendedProfile != null)
                    {

                        string html_aboutme = this.ExtendedProfile.person.aboutme != null ? this.ExtendedProfile.person.aboutme : "8-bit kid is a cool chap.  well above lorem ipsum text for his about me.  this should be filled in dynamically from the profile about me.";
                        string link_profilePic = this.ExtendedProfile.person.thumbnailUrl;
                        string link_profileMorePics = string.Format("http://viewmorepics.myspace.com/index.cfm?fuseaction=user.viewAlbums&friendID={0}", this.UserId);
                        string link_profileMoreVids = string.Format("http://vids.myspace.com/index.cfm?fuseaction=vids.channel&channelID={0}", this.UserId);
                        string link_profileMorePlay = string.Format("http://music.myspace.com/index.cfm?fuseaction=music.singleplaylist&friendid={0}", this.UserId);
                        string html_desiretomeet = this.ExtendedProfile.person.relationshipstatus;

                %>
                <div id="profile-view">
                    <img id="profile-pic" src="<%=link_profilePic %>" alt="<%=this.ExtendedProfile.person.displayName %>" />
                    <div id="caption">
                        <div id="caption-left">
                            <a href="<%= link_profileMorePics %>" style="margin-right: 3px;">pics</a>/ <a href="<%= link_profileMoreVids %>"
                                style="margin-left: 3px;">video</a>/ <a href="<%=link_profileMorePlay %>" style="margin-left: 3px;">
                                    music</a>
                        </div>
                        <div id="caption-right">
                            <img src="/mock/profile_icon.png" alt="yay blue" />
                            <img src="/mock/level_icon.png" alt="uber" />
                        </div>
                    </div>
                </div>
                <h1>
                    <span class="display-name">
                        <%=this.ExtendedProfile.person.displayName%></span></h1>
                <h2>
                    <%=this.ExtendedProfile.person.age%>,
                    <%=this.ExtendedProfile.person.currentlocation %>
                </h2>
                <p id="about-me">
                    <%= html_aboutme%>
                </p>
                <p id="desiretomeet">
                    <%=html_desiretomeet %>
                </p>
                <%
                    }
                    else
                    {

                    }
                %>
            </li>
            <li id="basic-info">
                <% if (this.IsLoggedIn() && this.ExtendedProfile != null)
                   {
	
                %>
                <h1>
                    <span class="display-name">
                        <%= this.ExtendedProfile.person.displayName %></span>'s Basic Info</h1>
                <h2>
                    General:</h2>
                <p id="General">
                    <%= (this.ExtendedProfile.person.interests) %></p>
                <h2>
                    Music:</h2>
                <p id="music">
                    <%=ConvertStringArrayToStringJoin(this.ExtendedProfile.person.music)%></p>
                <h2>
                    Movies:</h2>
                <p id="movies">
                    <%=ConvertStringArrayToStringJoin(this.ExtendedProfile.person.movies)%></p>
                <h2>
                    Television:</h2>
                <p id="television">
                    <%=ConvertStringArrayToStringJoin(this.ExtendedProfile.person.tvshows)%></p>
                <h2>
                    Books:</h2>
                <p id="books">
                    <%=ConvertStringArrayToStringJoin(this.ExtendedProfile.person.books)%></p>
                <%
                    }
                   else
                   {

                   }
                %>
            </li>
            <li id="activities" class='rounded-corner-right'>
                <% if (this.IsLoggedIn() && this.ExtendedProfile != null && this.FriendActivities != null)
                   {// && this.FriendActivities!=null ){
	
                %>
                <h1>
                    <span class='display-name'>Friend's or Band's Activities</span></h1>
                <%= GetFriendActivitiesHtml()%>
                <%
                    }
                   else
                   {
                %>ACTIVITIES ERROR<%
                                      }
                %>
            </li>
        </ul>
    </div>

    <script>
        function p2() {
            var ms = new MySpaceID(msOptions);
        }

        function success(rand) {
            //alert(rand);
            window.location.href = "http://localhost:9090";
        }

        function failed(rand) {
            $('#login div.logout span').addClass("logout_isSignedout");
            $('#login div.logout span').removeClass("logout_isSignedin");
        }
    </script>

    <div id="copyright">
        &copy; 2008-2009. 8-Bit Music. All Rights Reserved.<br>
        This site supports OpenID authentication. <a href="http://www.openid.net" target="_blank">
            Learn More</a></div>
</body>
</html>
