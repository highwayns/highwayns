using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using WeiBeeCommon.Core;

namespace WeiBeeCommon.DataTypes
{
    public class WeiBeeAccount
    {
        public string ConsumerKey;
        public string ConsumerSecret;
        public string Token;
        public string TokenSecret;
        public WeiBeeType UserType;
        public IWeiBee GetWeiBee()
        {
            var result = new WeiBeeQQ
                             {
                                 OAuth =
                                     {
                                         ConsumerKey = ConsumerKey,
                                         ConsumerSecret = ConsumerSecret,
                                         Token = Token,
                                         TokenSecret = TokenSecret
                                     }
                             };
            return result;
        }
        public class WeiBeeAccountComparer : IEqualityComparer<WeiBeeAccount>
        {
            public bool Equals(WeiBeeAccount x, WeiBeeAccount y)
            {
                return x.Token == y.Token;
            }

            public int GetHashCode(WeiBeeAccount obj)
            {
                return obj.Token.GetHashCode();
            }
        }
    }
    public class WeiBeeAccountHelper
    {
        public string Entityfile = "accounts.xml";
        public List<WeiBeeAccount> LoadAccounts()
        {
            var result = new List<WeiBeeAccount>();
            if (File.Exists(Entityfile))
            {
                using (var fs = new FileStream(Entityfile, FileMode.Open, FileAccess.Read))
                {
                    var root = new XmlRootAttribute("Accounts");
                    var ser = new XmlSerializer(typeof(List<WeiBeeAccount>), root);
                    result = ser.Deserialize(fs) as List<WeiBeeAccount>;
                }
            }
            else
            {
                var account = new WeiBeeAccount();
                account.ConsumerKey = "3587646579";
                account.ConsumerSecret = "fa997b0e0e3b048b5fda4f4627284af9";
                account.UserType = WeiBeeType.Sina;
                result.Add(account);
                var qq = new WeiBeeAccount();
                qq.ConsumerKey = "fb76ef232d6841e19f48428c12f36893";
                qq.ConsumerSecret = "b4ed74bf2f6961dcecef2cacca639262";
                qq.UserType = WeiBeeType.QQ;
                result.Add(qq);
                var sohu = new WeiBeeAccount();
                sohu.ConsumerKey = "up1cepg6Lfi8ix1qkKj4";
                sohu.ConsumerSecret = "VSgsP=vRno-3AvAYFvX-A(#zHZozCRdV!cFp#hyv";
                sohu.UserType = WeiBeeType.Sohu;
                result.Add(sohu);
                SaveAccounts(result);
            }
            return result;
        }
        public void SaveAccounts(List<WeiBeeAccount> entities)
        {
            using (var fs = new FileStream(Entityfile, FileMode.Create, FileAccess.Write))
            {
                var root = new XmlRootAttribute("Accounts");
                var ser = new XmlSerializer(typeof(List<WeiBeeAccount>), root);
                ser.Serialize(fs, entities);
            }
        }
    }
}