using System;
using System.Net;
using System.IO;

namespace WeiBeeCommon.Core
{
    static class WebHelper
    {
        public const string HTTPGET = "GET";
        public const string HTTPPOST = "POST";

        //public static byte[] GetBytesFromURL(string sURL) {
        //    return CopyStreamToByteArray(GetWebResponse(sURL, HTTPGET));
        //}

        //public static Stream GetWebResponse(String sURL, String sMethod) {
        //    return GetWebResponse(sURL, sMethod, "", "");
        //}

        //public static Stream GetWebResponse(String sURL, String sMethod, string sUserName, string sPassword) {
        //    HttpWebRequest request = WebRequest.Create(sURL) as HttpWebRequest;
        //    request.Method = sMethod;

        //    if (!string.IsNullOrEmpty(sUserName) && !string.IsNullOrEmpty(sPassword)) 
        //        request.Credentials = new NetworkCredential(sUserName, sPassword);

        //    HttpWebResponse Response = (HttpWebResponse)request.GetResponse();
        //    return Response.GetResponseStream();
        //}

        /// <summary> For copying a non-seekable stream to a byte array </summary>
        private static byte[] CopyStreamToByteArray(Stream Stream) {
            byte[] Buffer = new byte[1024];
            int iBytesRead;

            using (MemoryStream OutStream = new MemoryStream()) {

                do {
                    iBytesRead = Stream.Read(Buffer, 0, Buffer.Length);
                    OutStream.Write(Buffer, 0, iBytesRead);
                } while (iBytesRead > 0);

                return OutStream.ToArray();
            }
        }

        /// <summary> UrlDecodes a string without requiring System.Web </summary>
        /// <remarks> This is to avoid including system.web which means it can use the client only version
        /// of the framework, giving a potentially smaller framework download</remarks>
        public static string UrlDecode(string text) {
            // pre-process for + - signs space formatting since System.Uri doesn't handle it
            // TODO use regular expressions here
            text = text.Replace("+", "");
            text = text.Replace("-", "");
            text = text.Replace("%", "");
            text = text.Replace("&lt", "<");
            text = text.Replace("&gt", ">");
            return text;
        }

    }
}
