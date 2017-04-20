using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

namespace highwayns
{
    public partial class FormMicrosoft : Form
    {
        public FormMicrosoft()
        {
            InitializeComponent();
        }

        private string TranslateMethod(string authToken, string translating)
        {
            string translated = string.Empty;
            string from = "ja";
            string to = "en";
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" +
                System.Web.HttpUtility.UrlEncode(translating) + "&from=" + from + "&to=" + to;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            WebResponse response = null;
            try
            {
                response = httpWebRequest.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                    translated = (string)dcs.ReadObject(stream);
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
            return translated;
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            txtDestination.Text = TranslateMethod("", txtSource.Text);
        }

    }
}
