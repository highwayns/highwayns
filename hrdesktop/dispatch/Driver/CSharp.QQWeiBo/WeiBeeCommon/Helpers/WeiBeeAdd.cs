using System;
using System.IO;
using WeiBeeCommon.Core;

namespace WeiBeeCommon.Helpers
{
    public class WeiBeeAdd
    {
        public string Token;
        public string TokenSecret;
        public WeiBeeType WbType;
        public string MessageText;
        public string PictureFile;
        public Stream PictureFileStream;
        public DateTime ScheduledTime;

        public WeiBeeAdd(IWeiBee wb, string txt)
        {
            InitWeiBeeAdd(wb, txt);
        }

        public WeiBeeAdd(IWeiBee wb, string txt, string pic)
        {
            PictureFile = pic;
            InitWeiBeeAdd(wb, txt);
        }

        public WeiBeeAdd(string token, string secret, WeiBeeType type, string txt, string pic)
        {
            InitWeiBeeAdd(token, secret, type, txt, pic);
        }

        public WeiBeeAdd(string token, string secret, WeiBeeType type, string txt)
        {
            InitWeiBeeAdd(token, secret, type, txt, string.Empty);
        }

        private void InitWeiBeeAdd(string token, string secret, WeiBeeType type, string txt, string pic)
        {
            Token = token;
            TokenSecret = secret;
            WbType = type;
            MessageText = txt;
            PictureFile = pic;
            ScheduledTime = DateTime.UtcNow;
        }

        private void InitWeiBeeAdd(IWeiBee wb, string txt)
        {
            Token = wb.GetOAuth().Token;
            TokenSecret = wb.GetOAuth().TokenSecret;
            WbType = wb.UserType;
            MessageText = txt;
            ScheduledTime = DateTime.UtcNow;
        }
        /// <summary>
        /// Add one Twitter depends on the WeiBeeType and PictureFile
        /// </summary>
        /// <returns>wei bo id</returns>
        public string Add()
        {
            if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(TokenSecret) )
            {
                return string.Empty;
            }
            IWeiBee wb = WeiBeeFactory.CreateWeiBeeByType(WbType);
            wb.SetOAuth(Token, TokenSecret);
            string ret;
            if ( PictureFileStream != null)
            {
                ret = wb.AddPicture(MessageText, PictureFileStream);
            }
            else if (string.IsNullOrEmpty(PictureFile))
            {
                ret = wb.AddTwitter(MessageText);
            }
            else
            {
                ret = wb.AddPicture(MessageText, PictureFile);
            }
            return ret;
        }
    }
}
