using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTSDK.Tencent;
using OpenTSDK.Tencent.API;
using OpenTSDK.Tencent.Objects;
using System.Xml;

namespace OpenTSDK.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------------------------");
            Console.Write("请输入您的App_Key:");
            string appKey = Console.ReadLine();
            Console.Write("请输入您的App_Secret:");
            string appSecret = Console.ReadLine();
            Tencent.Run(appKey, appSecret);
        }
    }
}
