/*
===========================================================================
Copyright (c) 2010 BrickRed Technologies Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sub-license, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
===========================================================================

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using Newtonsoft.Json.Linq;
using Brickred.SocialAuth.NET.Core;


public partial class Welcome : System.Web.UI.Page
{
    public string Provider;
    public string Pid;
    public string Identifier;
    public string Username;
    public string Displayname;
    public string Email;
    public string FirstName;
    public string LastName;
    public string Fullname;
    public string DateOfBirth;
    public string Gender;
    public string ProfileURL;
    public string ProfilePicture;
    public string Country;
    public string Language;
    public string ContactsCount;
    public bool IsSTSaware;
    public string AccessToken;

    protected void Page_Load(object sender, EventArgs e)
    {

        //Required to be done when using custom mode
        //if (!SocialAuthUser.IsLoggedIn())
        //    SocialAuthUser.RedirectToLoginPage("ManualLogin.aspx");


        foreach (PROVIDER_TYPE p in SocialAuthUser.GetConnectedProviders())
        {
            divConnections.Controls.Add(new Literal(){Text = "<br>Connected to: <b>" + p.ToString() + "</b> with identifier <b>" +
                                         SocialAuthUser.GetCurrentUser().GetProfile(p).GetIdentifier() + "</b>"});
            LinkButton logoutBtn = new LinkButton() { Text = "[Logout from " + p.ToString() + "]", CommandArgument = p.ToString()};
            logoutBtn.Command += new CommandEventHandler(btnIndividualLogout_Click);
            divConnections.Controls.Add(logoutBtn);
        }

        if (SocialAuthUser.IsLoggedIn())
        {
            IsSTSaware = HttpContext.Current.ApplicationInstance.IsSTSaware();
            Provider = User.Identity.GetProvider();
            Pid = User.Identity.GetProfile().ID;
            Identifier = User.Identity.GetProfile().GetIdentifier();
            Username = User.Identity.GetProfile().Username;
            Displayname = User.Identity.GetProfile().DisplayName;
            Email = User.Identity.GetProfile().Email;
            Fullname = User.Identity.GetProfile().FullName;
            FirstName = User.Identity.GetProfile().FirstName;
            LastName = User.Identity.GetProfile().LastName;
            DateOfBirth = User.Identity.GetProfile().DateOfBirth;
            Gender = User.Identity.GetProfile().Gender.ToString();
            ProfileURL = User.Identity.GetProfile().ProfileURL;
            ProfilePicture = User.Identity.GetProfile().ProfilePictureURL;
            Country = User.Identity.GetProfile().Country;
            Language = User.Identity.GetProfile().Language;
            AccessToken = SocialAuthUser.GetCurrentUser().GetAccessToken();
            bool IsAlternate = false;

            try
            {
                User.Identity.GetContacts().ForEach(
                x =>
                {
                    HtmlTableRow tr = new HtmlTableRow();
                    tr.Attributes.Add("class", (IsAlternate) ? "dark" : "light");
                    tr.Cells.Add(new HtmlTableCell() { InnerText = x.Name });
                    tr.Cells.Add(new HtmlTableCell() { InnerText = x.Email });
                    tr.Cells.Add(new HtmlTableCell() { InnerText = x.ProfileURL });
                    tblContacts.Rows.Add(tr);
                    IsAlternate = !IsAlternate;
                }

                );
                ContactsCount = (tblContacts.Rows.Count - 1).ToString();


            }
            catch (Exception ex)
            {
                contacts.InnerHtml = "<error>" + ex.Message + "</error>";
            }

        }
        else
        {

            Response.Write("You are not logged in..");
        }
    }
    
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        SocialAuthUser.GetCurrentUser().Logout("ManualLogin.aspx");
    }

    protected void btnIndividualLogout_Click(object sender, CommandEventArgs e)
    {
        PROVIDER_TYPE provider = (PROVIDER_TYPE) Enum.Parse(typeof (PROVIDER_TYPE), e.CommandArgument.ToString());
        SocialAuthUser.GetCurrentUser().Logout(providerType: provider);
        Response.Redirect(HttpContext.Current.Request.Url.ToString());
    }
}

