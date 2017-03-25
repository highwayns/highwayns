using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Brickred.SocialAuth.NET.Demo
{
    public partial class Default : System.Web.UI.Page
    {

        List<UserProfile> profiles = new List<UserProfile>();

        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateTableByRows();
            
        }

        protected void lbtnLogout_Click(object sender, EventArgs e)
        {
            SocialAuthUser.GetCurrentUser().Logout("Default.aspx");
        }

        void UpdateTableByRows()
        {
            List<PROVIDER_TYPE> providers = SocialAuthUser.GetConnectedProviders();

            //var result = SocialAuthUser.GetCurrentUser().ExecuteFeed(
            //    "http://api.linkedin.com/v1/people/~/connections:(headline,first-name,last-name,educations)",
            //    TRANSPORT_METHOD.GET, PROVIDER_TYPE.LINKEDIN);

            foreach (var provider in providers)
                profiles.Add(SocialAuthUser.GetCurrentUser().GetProfile(provider));

            foreach (UserProfile p in profiles)
            {
                HtmlGenericControl tableContainer = new HtmlGenericControl("div");
                tableContainer.Attributes.Add("style", "float:left;padding:10px 10px 10px 10px;width:400px;height:210px;");

                Table tbl = new Table() { CellSpacing = 0 };
                tbl.Attributes.Add("style", "width:100%");
                //Header Row
                TableHeaderRow trHeader = new TableHeaderRow();
                TableCell tc = new TableHeaderCell() { Text = p.Provider.ToString() };
                tc.ColumnSpan = 2;
                trHeader.Cells.Add(tc);
                tbl.Rows.Add(trHeader);



                //Start adding Values
                TableRow tr = new TableRow();

                ////Provider
                //tr.Cells.Add(new TableCell() { Text = "Provider" });
                //tr.Cells.Add(new TableCell() { Text = p.Provider.ToString() });

                //tbl.Rows.Add(tr);
                //tr = new TableRow();

                //Best Possible Identifier
                tr.Cells.Add(new TableCell() { Text = "Best Possible Identifier", CssClass = "altTr" });
                tr.Cells.Add(new TableCell() { Text = p.GetIdentifier() });

                tbl.Rows.Add(tr);
                tr = new TableRow();

                //ID
                tr.Cells.Add(new TableCell() { Text = "ID" });
                tr.Cells.Add(new TableCell() { Text = p.ID });

                tbl.Rows.Add(tr);
                tr = new TableRow();

                ////FirstName
                //tr.Cells.Add(new TableCell() { Text = "FirstName", CssClass = "altTr" });
                //tr.Cells.Add(new TableCell() { Text = p.FirstName });
                //tbl.Rows.Add(tr);
                //tr = new TableRow();

                ////LastName
                //tr.Cells.Add(new TableCell() { Text = "LastName" });
                //tr.Cells.Add(new TableCell() { Text = p.LastName });
                //tbl.Rows.Add(tr);
                //tr = new TableRow();

                //FullName
                tr.Cells.Add(new TableCell() { Text = "FullName", CssClass = "altTr" });
                tr.Cells.Add(new TableCell() { Text = p.FullName });
                tbl.Rows.Add(tr);
                tr = new TableRow();

                ////DisplayName
                //tr.Cells.Add(new TableCell() { Text = "DisplayName" });
                //tr.Cells.Add(new TableCell() { Text = p.DisplayName });
                //tbl.Rows.Add(tr);
                //tr = new TableRow();

                //Email
                tr.Cells.Add(new TableCell() { Text = "Email" });
                tr.Cells.Add(new TableCell() { Text = p.Email });

                tbl.Rows.Add(tr);
                tr = new TableRow();

                ////Gender
                //tr.Cells.Add(new TableCell() { Text = "Gender" });
                //tr.Cells.Add(new TableCell() { Text = p.Gender });

                //tbl.Rows.Add(tr);
                //tr = new TableRow();


                ////ProfileURL
                //tr.Cells.Add(new TableCell() { Text = "ProfileURL", CssClass = "altTr" });
                //tr.Cells.Add(new TableCell() { Text = p.ProfileURL });

                //tbl.Rows.Add(tr);
                //tr = new TableRow();


                //ProfilePictureURL
                
                tr.Cells.Add(new TableCell() { Text = "ProfilePictureURL", CssClass = "altTr"  });
                TableCell profilePicCell = new TableCell ();
                if (!string.IsNullOrEmpty(p.ProfilePictureURL))
                    profilePicCell.Text = "<img src=\"" + p.ProfilePictureURL + "\" height=\"100\" width=\"100\">";
                else
                    profilePicCell.Text = "<img src=\"images/notavailable.png\" />";
                tr.Cells.Add(profilePicCell);

                tbl.Rows.Add(tr);
                //tr = new TableRow();


                ////Country
                //tr.Cells.Add(new TableCell() { Text = "Country", CssClass = "altTr" });
                //tr.Cells.Add(new TableCell() { Text = p.Country });

                //tbl.Rows.Add(tr);
                //tr = new TableRow();


                ////Language
                //tr.Cells.Add(new TableCell() { Text = "Language" });
                //tr.Cells.Add(new TableCell() { Text = p.Language });

                //tbl.Rows.Add(tr);
                tableContainer.Controls.Add(tbl);
                divContent.Controls.Add(tableContainer);

                
            }



            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //tbl.RenderControl(hw);
            //this.profileTable.InnerHtml = sb.ToString();
        }

        void UpdateTableByColumns()
        {
            List<PROVIDER_TYPE> providers = SocialAuthUser.GetConnectedProviders();


            foreach (var provider in providers)
                profiles.Add(SocialAuthUser.GetCurrentUser().GetProfile(provider));

            Table tbl = new Table() { CellSpacing = 0 };
            //Header Row
            TableHeaderRow trHeader = new TableHeaderRow();
            trHeader.Cells.Add(new TableHeaderCell() { Text = "Provider/Feature" });
            providers.ForEach(x => trHeader.Cells.Add(new TableCell() { Text = x.ToString() }));
            tbl.Rows.Add(trHeader);



            //Start adding Values
            TableRow tr = new TableRow();

            //Provider
            tr.Cells.Add(new TableCell() { Text = "Provider" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.Provider.ToString() });

            tbl.Rows.Add(tr);
            tr = new TableRow();

            //Best Possible Identifier
            tr.Cells.Add(new TableCell() { Text = "Best Possible Identifier", CssClass = "altTr" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.GetIdentifier() });

            tbl.Rows.Add(tr);
            tr = new TableRow();

            //ID
            tr.Cells.Add(new TableCell() { Text = "ID" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.ID });

            tbl.Rows.Add(tr);
            tr = new TableRow();


            //FirstName
            tr.Cells.Add(new TableCell() { Text = "FirstName", CssClass = "altTr" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.FirstName });

            tbl.Rows.Add(tr);
            tr = new TableRow();

            //LastName
            tr.Cells.Add(new TableCell() { Text = "LastName" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.LastName });

            tbl.Rows.Add(tr);
            tr = new TableRow();

            //FullName
            tr.Cells.Add(new TableCell() { Text = "FullName", CssClass = "altTr" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.FullName });

            tbl.Rows.Add(tr);
            tr = new TableRow();

            //DisplayName
            tr.Cells.Add(new TableCell() { Text = "DisplayName" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.DisplayName });

            tbl.Rows.Add(tr);
            tr = new TableRow();

            //Email
            tr.Cells.Add(new TableCell() { Text = "Email", CssClass = "altTr" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.Email });

            tbl.Rows.Add(tr);
            tr = new TableRow();

            //Gender
            tr.Cells.Add(new TableCell() { Text = "Gender" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.Gender });

            tbl.Rows.Add(tr);
            tr = new TableRow();


            //ProfileURL
            tr.Cells.Add(new TableCell() { Text = "ProfileURL", CssClass = "altTr" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.ProfileURL });

            tbl.Rows.Add(tr);
            tr = new TableRow();


            //ProfilePictureURL
            tr.Cells.Add(new TableCell() { Text = "ProfilePictureURL" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.ProfilePictureURL });

            tbl.Rows.Add(tr);
            tr = new TableRow();


            //Country
            tr.Cells.Add(new TableCell() { Text = "Country", CssClass = "altTr" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.Country });

            tbl.Rows.Add(tr);
            tr = new TableRow();


            //Language
            tr.Cells.Add(new TableCell() { Text = "Language" });
            foreach (var p in profiles)
                tr.Cells.Add(new TableCell() { Text = p.Language });

            tbl.Rows.Add(tr);

            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //tbl.RenderControl(hw);
            //this.profileTable.InnerHtml = sb.ToString();
        }
    }
}