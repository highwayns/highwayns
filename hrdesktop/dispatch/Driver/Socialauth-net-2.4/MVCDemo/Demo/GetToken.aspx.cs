using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Brickred.SocialAuth.NET.Demo
{
    public partial class GetToken : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SocialAuthUser.GetConnectedProviders().ForEach(x =>
                {
                    ddlConnectedProviders.Items.Add(new ListItem(x.ToString()));
                });
        }

     
   

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (ddlConnectedProviders.Items.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "noprovider", "alert('You are not connected with any provider')", true);
                return;
            }

            var token = SocialAuthUser.GetCurrentUser().GetConnection((PROVIDER_TYPE)Enum.Parse(typeof(PROVIDER_TYPE), ddlConnectedProviders.SelectedItem.Value)).GetConnectionToken();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(Server.MapPath("~/temptokens/MyFile.token"),
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.Write);
            formatter.Serialize(stream, token);
            stream.Close();
            stream.Dispose();


            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", "MyFile.token"));
            Response.WriteFile("~/temptokens/MyFile.token");
            Response.End();
            
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            SocialAuthUser sauser = new SocialAuthUser();
            MemoryStream ms = new MemoryStream(FileUpload1.FileBytes);
            IFormatter formatter = new BinaryFormatter();
            Token token = (Token)formatter.Deserialize(ms);
            sauser.LoadToken(token,"default.aspx");
        }
    }
}