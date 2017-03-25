/*
 システム  　：HWDC
 サブシステム：ウェブサブシステム
 バージョン　：1.5.*　から
 著作権    　：Highway Co.Ltd.  Copyright 2006～2007
 概要      　：ウェブ
 更新履歴  　：2006/10/14　　鄭軍　　新規
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Collections;

namespace NC.HPS.Lib
{
    /// <summary>
    /// 受信処理
    /// </summary>
    public class NCPop3Mail
    {
        /// <summary>
        /// GetmailAddress from mail content
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public static string GetmailAddress(string mail)
        {
            string mailaddrs = "";
            string tempstr = mail.Replace("\n", "");
            string[] temp = tempstr.Split("\r".ToCharArray());
            if (temp != null && temp.Length > 0)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    string mailaddr = readmail(temp[i]);
                    if (mailaddr != "" && isEmail(mailaddr)) mailaddrs += mailaddr + ";";
                }
            }
            return mailaddrs;
        }
        /// <summary>
        /// filter
        /// </summary>
        /// <param name="mailaddrs"></param>
        /// <param name="protectedmails"></param>
        /// <returns></returns>
        public static string filter(string mailaddrs, string protectedmails)
        {
            string[] mail = mailaddrs.Split(";".ToCharArray());
            string[] protectedmail = protectedmails.Split(";".ToCharArray());
            Hashtable h = new Hashtable();
            if (mail != null && mail.Length > 0)
            {
                for (int i = 0; i < mail.Length; i++)
                    h[mail[i]] = mail[i];
            }
            if (protectedmail != null && protectedmail.Length > 0)
            {
                for (int i = 0; i < protectedmail.Length; i++)
                    h.Remove(protectedmail[i]);
            }
            string retmails = "";
            if (h.Keys.Count > 0)
            {
                string[] mails = new string[h.Keys.Count];
                h.Keys.CopyTo(mails, 0);
                for (int i = 0; i < h.Keys.Count; i++)
                {
                    if (mails[i] != "") retmails += mails[i] + ";";
                }
            }
            return retmails;
        }
        /// <summary>
        /// readmail from line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string readmail(string line)
        {
            string mailaddr = "";
            if (line.IndexOf("@") > 0)
            {
                string[] temp = line.Split("@".ToCharArray());
                if (temp.Length == 2)
                {
                    mailaddr = readmailname(temp[0]) + "@" + readmaildomain(temp[1]);
                }
            }
            return mailaddr;
        }
        /// <summary>
        /// readmailname
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string readmailname(string str)
        {
            string name = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (!isValidName(Convert.ToString(str[i])))
                {
                    name = "";
                }
                else name += str[i];
            }
            return name;
        }
        /// <summary>
        /// readmaildomain
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string readmaildomain(string str)
        {
            string domain = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (!isValidDomain(Convert.ToString(str[i])))
                {
                    return domain;
                }
                else domain += str[i];
            }
            return domain;
        }
        /// <summary>
        /// isValidName
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        public static bool isValidName(string name)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(name))
                return (true);
            else
                return (false);
        }
        /// <summary>
        /// isValidDomain
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        public static bool isValidDomain(string domain)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(domain))
                return (true);
            else
                return (false);
        }
        /// <summary>
        /// isEmail
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        public static bool isEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        /// <summary>
        /// POP3サーバーからメールをすべて受信する
        /// </summary>
        /// <param name="hostName">POP3サーバー名</param>
        /// <param name="portNumber">POP3サーバーのポート番号</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="passWord">パスワード</param>
        /// <param name="deleteMails">メールを削除するか</param>
        /// <returns>取得したメールの配列</returns>
        public static string[] Receive(string hostName,
            int portNumber,
            string userId,
            string passWord,
            bool deleteMails,
            int maxcount)
        {
            string[] mails;
            string msg = "";
            NetworkStream stream;

            //TcpClientの作成
            TcpClient client = new TcpClient();
            //タイムアウトの設定
            client.ReceiveTimeout = 10000;
            client.SendTimeout = 10000;

            try
            {
                //サーバーに接続
                client.Connect(hostName, portNumber);
                //ストリームの取得
                stream = client.GetStream();
                //受信
                msg = ReceiveData(stream);

                //USERの送信
                SendData(stream, "USER " + userId + "\r\n");
                //受信
                msg = ReceiveData(stream);

                //PASSの送信
                SendData(stream, "PASS " + passWord + "\r\n");
                //受信
                msg = ReceiveData(stream);

                //STATの送信
                SendData(stream, "STAT\r\n");
                //受信
                msg = ReceiveData(stream);
                //メール数の取得
                int mailsCount = int.Parse(msg.Split(' ')[1]);
                if (maxcount != 0 && mailsCount > maxcount)
                {
                    mailsCount = maxcount;
                }
                mails = new string[mailsCount];

                //すべてのメールの内容を受信
                for (int i = 1; i <= mailsCount; i++)
                {
                    //RETRの送信（メール本文を受信）
                    SendData(stream, "RETR " + i.ToString() + "\r\n");
                    //受信
                    msg = ReceiveData(stream, true);
                    mails[i - 1] = msg.Substring(msg.IndexOf("\r\n") + 2);

                    //メールを削除するか
                    if (deleteMails)
                    {
                        //DELEの送信（メールに削除マークを付ける）
                        SendData(stream, "DELE " + i.ToString() + "\r\n");
                        //受信
                        msg = ReceiveData(stream);
                    }
                }

                //QUITの送信
                SendData(stream, "QUIT\r\n");
                //受信
                msg = ReceiveData(stream);
            }
            catch
            {
                throw;
            }
            finally
            {
                //切断
                client.Close();
            }

            return mails;
        }

        //データを受信する
        private static string ReceiveData(
            NetworkStream stream,
            bool multiLines,
            int bufferSize,
            Encoding enc)
        {
            byte[] data = new byte[bufferSize];
            int len;
            string msg = "";

            //すべて受信する
            //(無限ループに陥る恐れあり)
            do
            {
                //受信
                len = stream.Read(data, 0, data.Length);
                //文字列に変換する
                msg += enc.GetString(data, 0, len);
            }
            while (stream.DataAvailable ||
                ((!multiLines || msg.StartsWith("-ERR")) &&
                    !msg.EndsWith("\r\n")) ||
                (multiLines && !msg.EndsWith("\r\n.\r\n")));

            //"-ERR"を受け取った時は例外をスロー
            if (msg.StartsWith("-ERR"))
                throw new ApplicationException("Received Error");

            //表示
            Console.Write("S: " + msg);

            return msg;
        }
        private static string ReceiveData(NetworkStream stream,
            bool multiLines,
            int bufferSize)
        {
            return ReceiveData(stream, multiLines, bufferSize,
                Encoding.GetEncoding(50220));
        }
        private static string ReceiveData(NetworkStream stream,
            bool multiLines)
        {
            return ReceiveData(stream, multiLines, 256);
        }
        private static string ReceiveData(NetworkStream stream)
        {
            return ReceiveData(stream, false);
        }

        //データを送信する
        private static void SendData(NetworkStream stream,
            string msg,
            Encoding enc)
        {
            //byte型配列に変換
            byte[] data = enc.GetBytes(msg);
            //送信
            stream.Write(data, 0, data.Length);

            //表示
            Console.Write("C: " + msg);
        }
        private static void SendData(NetworkStream stream,
            string msg)
        {
            SendData(stream, msg, Encoding.ASCII);
        }
        public static string GetContent(string htmltext,string name,string url)
        {
            string ret = "";
            string tempstr = htmltext.Replace("\n", "");
            string[] temp = tempstr.Split("\r".ToCharArray());
            if (temp != null && temp.Length > 0)
            {
                int count = 0;
                string intro = "";
                string point = "";
                string image = "";
                string price = "";
                string category = "";
                for (int i = 0; i < temp.Length; i++)
                {
                    if (point == "")
                    {
                        point = readPoint(temp[i]);
                        if (point != "") count++;
                    }
                    if (image == "")
                    {
                        image = readImage(temp[i], name.Substring(0, 8));
                        if (image != "")
                        {
                            count++;
                        }
                    }
                    if (price == "")
                    {
                        price = readPrice(temp[i]);
                        if (price != "")
                        {
                            count++;
                        }
                    }
                    if (category == "")
                    {
                        category = readCategory(temp[i]);
                        if (category != "")
                        {
                            count++;
                        }
                    }
                    if (intro=="")
                    {
                        intro = readIntroduce(temp[i]);
                        if (intro != "")
                        {
                            count++;
                        }
                    }
                    if (count == 5)
                    {
                        ret = name + ";" + price + ";" + point + ";" + intro + ";" + image + ";" + url + ";" + category;
                        //[product] ([name],[price],[discount],[introduce],[imageurl],[url],[category]
                        break;
                    }
                    
                }
            }
            return ret;
        }
        private static string readPoint(string line)
        {
            int index =line.IndexOf("％還元"); 
            if ( index> -1)
            {
                string ret = "";
                for (int i = index; i > 0; i--)
                {
                    string str = line.Substring(i - 1, 1);
                    if (str == ">") return ret;
                    ret = str + ret;
                }
            }
            return "";
        }
        private static string readImage(string line,string name)
        {
            int index = line.IndexOf("<img src=");
            if (index > -1)
            {
                index = line.IndexOf(name);
                if (index > -1)
                {
                    string[] tempstr = line.Split("\"".ToCharArray());
                    if (tempstr.Length > 0)
                    {
                        for (int i = 0; i < tempstr.Length; i++)
                        {
                            if (tempstr[i].IndexOf("http:") > -1) return tempstr[i];
                        }
                    }
                }
            }
            return "";
        }
        private static string readPrice(string line)
        {
            int index = line.IndexOf("特価：￥"); 
            if ( index> -1)
            {
                string str = between(line,"特価：￥", "(税込)");
                str = str.Replace("</font></b>", "");
                return str.Replace(",", "");
            }
            return "";            
        }
        private static string between(string str, string sub1, string sub2)
        {
            int index1 = str.IndexOf(sub1);
            int index2 = str.IndexOf(sub2, index1);
            string temp = str.Substring(index1, index2 - index1);
            temp=temp.Replace(sub1, "");
            return temp;
        }
        private static string readIntroduce(string line)
        {
            int index = line.IndexOf("font size=\"2\"");
            if (index > -1)
            {
                string ret = "";
                bool canadd = true;
                for (int i = 0; i < line.Length; i++)
                {
                    string str = line.Substring(i, 1);
                    if (str == "<")
                    {
                        canadd = false;
                        continue;
                    }
                    if (str == ">")
                    {
                        canadd = true;
                        continue;
                    }
                    if (canadd && str != " ")
                    {
                        ret += str;
                    }
                }
                if (ret.Length > 120) return ret.Substring(0, 120);
                else return ret;
            }
            return "";
        }
        private static string readCategory(string line)
        {
            int index = line.IndexOf("panlink");
            if (index > -1)
            {
                index = line.IndexOf("</a>");
                index = line.IndexOf("</a>", index+1);
                string ret = "";
                for (int i = index; i > 0; i--)
                {
                    if (line.Substring(i-1,1) == ">") break;
                    ret = line.Substring(i-1, 1) + ret;
                }
                if (ret == "")
                {
                    index = line.IndexOf("</a>", index + 1);
                    for (int i = index; i > 0; i--)
                    {
                        if (line.Substring(i-1, 1) == ">") return ret;
                        ret = line.Substring(i-1, 1) + ret;
                    }
                }
                return ret;
            }
            return "";
        }
    }


}