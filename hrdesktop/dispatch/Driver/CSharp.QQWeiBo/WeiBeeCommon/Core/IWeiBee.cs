using System.Collections.Generic;
using System;
using System.IO;

namespace WeiBeeCommon.Core
{
    public interface IWeiBee
    {
        void SetOAuth(string token, string secret);
        string AddPicture(string text, string picture);
        string AddPicture(string text, Stream picture);
        string AddTwitter(string text);
        string GetUserId();
        bool IsEnabled { get; }
        WeiBeeType UserType { get; }
        OAuthBase GetOAuth();
    }
    public enum WeiBeeType
    {
        Sina,
        QQ,
        Sohu
    }
    public class WeiBeeFactory
    {
        /// <summary>
        /// Create WeiBeeQQ, WeiBeeSohu, or WeiBeeSina according to the WeiBeeType specified
        /// </summary>
        /// <param name="t">type of WeiBee</param>
        /// <returns>IWeiBee</returns>
        public static IWeiBee CreateWeiBeeByType(WeiBeeType t)
        {
            if (dict[t] == null) return null;
            return dict[t]();
        }

        static Dictionary<WeiBeeType, Func<IWeiBee>> dict = new Dictionary<WeiBeeType, Func<IWeiBee>>()
            { 
                {WeiBeeType.QQ, () => new WeiBeeQQ() }//,
               // {WeiBeeType.Sohu, () => new WeiBeeSohu() },
               // {WeiBeeType.Sina, () => new WeiBeeSina() }
            };
    }
}
