using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Drawing;

namespace WeiBeeCommon.Core
{
    public enum Method { GET, POST, PUT, DELETE };
    public static class Utility
    {
        public class DictItem<TKey, TValue>
        {
            public TKey Key;
            public TValue Value;
        }

        /// <summary> Serialize a dictionary to XML and save to disk </summary>
        public static void SerializeDictionary<TKey, TValue>(string sFileName, Dictionary<TKey, TValue> Dict) {
            var SaveList = new List<DictItem<TKey, TValue>>();

            foreach (TKey Key in Dict.Keys)
                SaveList.Add(new DictItem<TKey, TValue> { Key = Key, Value = Dict[Key] });

            XmlSerializer Serializer = new XmlSerializer(typeof(List<DictItem<TKey, TValue>>));
            using (TextWriter Writer = new StreamWriter(sFileName)) {
                Serializer.Serialize(Writer, SaveList);
            }
        }

        /// <summary> Deserialize a dictionary from an xml file </summary>
        public static Dictionary<TKey, TValue> DeserializeDictionary<TKey, TValue>(string sFileName) {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DictItem<TKey, TValue>>));
            List<DictItem<TKey, TValue>> LoadList;

            using (FileStream fs = new FileStream(sFileName, FileMode.Open)) {
                LoadList = (List<DictItem<TKey, TValue>>)serializer.Deserialize(fs);
            }

                var LoadDict = new Dictionary<TKey, TValue>();

                foreach (DictItem<TKey, TValue> Item in LoadList)
                    LoadDict.Add(Item.Key, Item.Value);

                return LoadDict;
        }

        /// <summary> Access an object using Invoke if required </summary>
        public static void AccessInvoke(ISynchronizeInvoke ThisObject, Action Action) {
            if (ThisObject.InvokeRequired)
                ThisObject.Invoke(Action, null);
            else
                Action();
        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">The request object.</param>
        /// <returns>The response data.</returns>
        public static string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (HttpRequestValidationException exception)
            {
                Debug.WriteLine(exception.Message);
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                if (responseReader != null) responseReader.Close();
            }

            return responseData;
        }

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public static string WebRequest(Method method, string url, string postData)
        {
            StreamWriter requestWriter;
            string responseData = "";

            var webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.Method = method.ToString();
                webRequest.ServicePoint.Expect100Continue = false;

                if (method == Method.POST)
                {
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    requestWriter = new StreamWriter(webRequest.GetRequestStream());
                    try
                    {
                        requestWriter.Write(postData);
                    }
                    finally
                    {
                        requestWriter.Close();
                    }
                }
                responseData = WebResponseGet(webRequest);// UseAsyncEnumerator(webRequest);
            }
            return responseData;
        }

        const string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        public static string UrlEncode(string value)
        {
            var result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (UnreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    //some symbols produce > 2 char values so the system urlencoder must be used to get the correct data
                    if (String.Format("{0:X2}", (int)symbol).Length > 3)
                    {
                        // ReSharper disable PossibleNullReferenceException
                        result.Append(HttpUtility.UrlEncode(value.Substring(value.IndexOf(symbol), 1)).ToUpper());
                        // ReSharper restore PossibleNullReferenceException
                    }
                    else
                    {
                        result.Append('%' + String.Format("{0:X2}", (int)symbol));
                    }
                }
            }
            return result.ToString();
        }

        public static string GetShortenUrl(string longUrl)
        {
            const string urlformatter = "http://api.bit.ly/v3/shorten?login=stonepeter&apiKey=R_602a98ce2ef8feed660747065c1d7bae&longUrl={0}&format=xml";
            var bitlyRequest = string.Format(urlformatter, UrlEncode(longUrl));
            var xmlresult = WebRequest(Method.GET, bitlyRequest, null);
            var xml = new XmlDocument();
            xml.LoadXml(xmlresult);
            string shortenUrl = xml.GetElementsByTagName("url")[0].InnerText;

            return shortenUrl;
        }

        /// <summary>
        /// Replace all Urls with shorten one
        /// </summary>
        /// <param name="orginal">The orginal text string</param>
        /// <returns>If there are urls replace it with bit.ly shorten name</returns>
        public static string ReplaceUrlWithShorten(string orginal)
        {
            Regex urlRegex = new Regex(@"http://[^\s]*");
            MatchCollection matches = urlRegex.Matches(orginal);
            string result = orginal;
            
            foreach (Match match in matches)
            {
                string matchvalue = match.Value;
                result = result.Replace(matchvalue, GetShortenUrl(matchvalue));
            }
            return result;
        }

        public static DateTime ConvertToDateTime(string timestamp)
        {
            var dtbase = new DateTime(1970, 1, 1, 8, 0, 0, 0); // UTC +8
            return dtbase.AddSeconds(double.Parse(timestamp));
        }

        /// <summary>
        /// Generate the UNIX style timestamp for DateTime.UtcNow        
        /// </summary>
        /// <returns></returns>
        public static string GenerateTimeStamp(DateTime dt, bool isMiliseconds)
        {
            TimeSpan ts = dt - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (isMiliseconds)
            {
                ts = dt - new DateTime(1970, 1, 1, 8, 0, 0, 0);
                return Convert.ToInt64(ts.TotalMilliseconds).ToString();
            }
            else
            {
                return Convert.ToInt64(ts.TotalSeconds).ToString();
            }
        }

        /// <summary>
        /// Generate the UNIX style timestamp for DateTime.UtcNow        
        /// </summary>
        /// <returns></returns>
        public static string GenerateTimeStamp()
        {
            return GenerateTimeStamp(DateTime.UtcNow, false);
        }

        /// <summary>
        /// Parse DateTime from "2011/5/2 14:40"
        /// </summary>
        /// <param name="s">"2011/5/2 14:40"</param>
        /// <returns>Parsed DateTime if success, DateTime.Now if not success</returns>
        public static DateTime ParseDateTime(string s)
        {
            DateTime result = DateTime.Now;
            DateTime r;
            try
            {
                var datetime = s.Split(' ');
                var date = datetime[0].Split('/');
                var time = datetime[1].Split(':');
                int year = int.Parse(date[0]);
                int month = int.Parse(date[1]);
                int day = int.Parse(date[2]);
                int hour = int.Parse(time[0]);
                int minute = int.Parse(time[1]);
                result = new DateTime(year, month, day, hour, minute, 0);
            }
            catch { }

            return result;
        }

        public static string MergePicture(string picturefile1, string picturefile2)
        {
            string mergedFilename = Guid.NewGuid().ToString() + ".jpg";
            Bitmap bit1 = new Bitmap(picturefile1);
            Bitmap bit2 = new Bitmap(picturefile2);
            Bitmap bit = new Bitmap( (bit1.Width > bit2.Width) ? bit1.Width:bit2.Width, bit1.Height + bit2.Height);
            Graphics graph = Graphics.FromImage(bit);
            graph.DrawImage(bit1, 0, 0,bit1.Width,bit1.Height);
            graph.DrawImage(bit2, 0, bit1.Height,bit2.Width,bit2.Height);
            bit.Save(mergedFilename);

            return mergedFilename;
        }

    }
}

