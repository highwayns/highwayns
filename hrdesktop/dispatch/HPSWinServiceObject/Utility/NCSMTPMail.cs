using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Specialized;
namespace NC.HPS.Lib
{
    /// <summary>
    /// メール内容
    /// </summary>
    public class NCSMTPMail
    {
        private string subject = "";
        private string from = "";
        private string fromuser = "";
        private string env_from = "";
        private string env_to = "";
        private string replyto = "";
        private StringCollection receivers = new StringCollection();
        private StringCollection ccs = new StringCollection();
        private StringCollection bccs = new StringCollection();
        private StringCollection attachments=new StringCollection();
        private MailEncodings mailEncoding=MailEncodings.ISO_2022_JP;
        private MailTypes mailType=MailTypes.Html;
        //private byte[] mailBody=null;
        private string mailBody = null;
        private string headmessage = "";
        
        /// <summary>
        /// メールHEAD
        /// </summary>
        public string HeadMessage
        {
            get { return headmessage; }
            set { headmessage = value; }
        }
        /// <summary>
        /// メールEnvFrom
        /// </summary>
        public string EnvFrom
        {
            get { return this.env_from; }
            set { this.env_from = value; }
        }
        /// <summary>
        /// メールFrom
        /// </summary>
        public string From
        {
            get { return this.from; }
            set { this.from = value; }
        }
        /// <summary>
        /// メールFromUser
        /// </summary>
        public string FromUser
        {
            get { return this.fromuser; }
            set { this.fromuser = value; }
        }
        /// <summary>
        /// メールEnvTo
        /// </summary>
        public string EnvTo
        {
            get { return this.env_to; }
            set { this.env_to = value; }
        }
        /// <summary>
        /// メールReplyTo
        /// </summary>
        public string ReplyTo
        {
            get { return this.replyto; }
            set { this.replyto = value; }
        }
        /// <summary>
        /// メールTO
        /// </summary>
        public StringCollection Receivers
        {
            get{return this.receivers;}
        }
        /// <summary>
        /// メールCCs
        /// </summary>
        public StringCollection CCs
        {
            get { return this.ccs; }
        }
        /// <summary>
        /// メールBCCs
        /// </summary>
        public StringCollection BCCs
        {
            get { return this.bccs; }
        }
        /// <summary>
        /// メール標題
        /// </summary>
        public string Subject
        {
            get{return this.subject;}
            set{this.subject=value;}
        }
        /// <summary>
        /// メール添付
        /// </summary>
        public StringCollection Attachments
        {
            get{return this.attachments;}
        }
        /// <summary>
        /// メールコード
        /// </summary>
        public MailEncodings MailEncoding
        {
            get{return this.mailEncoding;}
            set{this.mailEncoding=value;}
        }
        /// <summary>
        /// メールタイプ
        /// </summary>
        public MailTypes MailType
        {
            get{return this.mailType;}
            set{this.mailType=value;}
        }
        /// <summary>
        /// メールBODY
        /// </summary>
        //public byte[] MailBody
        //{
        //    get{return this.mailBody;}
        //    set{this.mailBody=value;}
        //}
        public string MailBody
        {
            get { return this.mailBody; }
            set { this.mailBody = value; }
        }
        /// <summary>
        /// メールコード
        /// </summary>
        public enum MailEncodings
        {
            SHIFT_JIS,
            ASCII,
            Unicode,
            UTF_8,
            ISO_2022_JP
        }
        /// <summary>
        /// メールタイプ
        /// </summary>
        public enum MailTypes
        {
            Html,
            Text
        }
        /// <summary>
        /// メール認定タイプ
        /// </summary>
        public enum SmtpValidateTypes
        {
            /// <summary>
            /// 不需要验证
            /// </summary>
            None,
            /// <summary>
            /// 通用的auth login验证
            /// </summary>
            Login,
            /// <summary>
            /// 通用的auth plain验证
            /// </summary>
            Plain,
            /// <summary>
            /// CRAM-MD5验证
            /// </summary>
            CRAMMD5
        }
        /// <summary>
        /// StringCollection ｰ>string
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public string StrColToStr(StringCollection sc)
        {
            string ret = "";
            if (sc == null || sc.Count == 0) return ret;
            foreach (string str in sc)
            {
                ret += str +";";
            }
            return ret.Substring(0, ret.Length - 1); 
        }
        /// <summary>
        /// メール送信
        /// </summary>
        public class KSN_Smtp
        {
            #region "member fields"
            /// <summary>
            /// 连接对象
            /// </summary>
            private TcpClient tc;
            /// <summary>
            /// 网络流
            /// </summary>
            private NetworkStream ns;
            /// <summary>
            /// 错误的代码字典
            /// </summary>
            private StringDictionary errorCodes = new StringDictionary();
            /// <summary>
            /// 操作执行成功后的响应代码字典
            /// </summary>
            private StringDictionary rightCodes = new StringDictionary();
            /// <summary>
            /// 执行过程中错误的消息
            /// </summary>
            private string errorMessage = "";
            /// <summary>
            /// 记录操作日志
            /// </summary>
            private string logs = "";
            /// <summary>
            /// 主机登陆的验证方式
            /// </summary>
            private StringCollection validateTypes = new StringCollection();
            /// <summary>
            /// 换行常数
            /// </summary>
            private const string CRLF = "\r\n";
            private string serverName = "smtp";
            private string logPath = null;
            private string userid = null;
            private string password = null;
            private string mailEncodingName = "SHIFT_JIS";
            private bool sendIsComplete = false;
            private SmtpValidateTypes smtpValidateType = SmtpValidateTypes.Login;
            #endregion
            #region "propertys"
            /// <summary>
            /// 获取最后一此程序执行中的错误消息
            /// </summary>
            public string ErrorMessage
            {
                get { return this.errorMessage; }
            }
            /// <summary>
            /// 获取或设置日志输出路径
            /// </summary>
            public string LogPath
            {
                get
                {
                    return this.logPath;
                }
                set { this.logPath = value; }
            }
            /// <summary>
            /// 获取日志
            /// </summary>
            public string Log
            {
                get
                {
                    return this.logs;
                }
            }
            /// <summary>
            /// 获取或设置登陆smtp服务器的帐号
            /// </summary>
            public string UserID
            {
                get { return this.userid; }
                set { this.userid = value; }
            }
            /// <summary>
            /// 获取或设置登陆smtp服务器的密码
            /// </summary>
            public string Password
            {
                get { return this.password; }
                set { this.password = value; }
            }
            /// <summary>
            /// 获取或设置要使用登陆Smtp服务器的验证方式
            /// </summary>
            public SmtpValidateTypes SmtpValidateType
            {
                get { return this.smtpValidateType; }
                set { this.smtpValidateType = value; }
            }
            #endregion
            #region "construct functions"
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="server">主机名</param>
            /// <param name="port">端口</param>
            public KSN_Smtp(string server, int port)
            {
                tc = new TcpClient(server, port);
                ns = tc.GetStream();
                this.serverName = server;
                this.initialFields();
            }
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="ip">主机ip</param>
            /// <param name="port">端口</param>
            public KSN_Smtp(IPAddress ip, int port)
            {
                IPEndPoint endPoint = new IPEndPoint(ip, port);
                tc = new TcpClient(endPoint);
                ns = tc.GetStream();
                this.serverName = ip.ToString();
                this.initialFields();
            }
            #endregion
            #region "methods"
            private void initialFields() //初始化连接
            {
                logs = "================" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "===============" + CRLF;
                //*****************************************************************
                //错误的状态码
                //*****************************************************************
                errorCodes.Add("421", "サーバ準備中です。");
                errorCodes.Add("432", "パスワード変換が必要です。");
                errorCodes.Add("450", "メールバックス不可用ですので、要求の操作完了できません。");
                errorCodes.Add("451", "要求の操作は未実行。");
                errorCodes.Add("452", "メモリ不足ですので、要求の操作完了できません。");
                errorCodes.Add("454", "一時認定失敗。");
                errorCodes.Add("500", "メールアドレスが間違いました。");
                errorCodes.Add("501", "パラメータが間違いました。");
                errorCodes.Add("502", "命令実現ができません。");
                errorCodes.Add("503", "命令の順番が間違いました。");
                errorCodes.Add("504", "命令のパラメータが間違いました。");
                errorCodes.Add("530", "認定が必要です。");
                errorCodes.Add("534", "認定体制は簡単すぎです。");
                errorCodes.Add("538", "認定体制の暗号化が必要です。");
                errorCodes.Add("550", "メールバックス不可用ですので、現在の操作完了できません。");
                errorCodes.Add("551", "地元のユーザではありません、<forward-path>を試してください。");
                errorCodes.Add("552", "メモリ過量、設定の操作が完了できません。");
                errorCodes.Add("553", "メールバックスの名前不可用です");
                errorCodes.Add("554", "転送失敗。");
                errorCodes.Add("535", "ユーザ認定失敗。");
                //****************************************************************
                //操作执行成功后的状态码
                //****************************************************************
                rightCodes.Add("220", "サービス準備が終わりました。");
                rightCodes.Add("221", "サービスの転送が閉じました。");
                rightCodes.Add("235", "認定成功。");
                rightCodes.Add("250", "要求の操作が完了しました。");
                rightCodes.Add("251", "地元のユーザではありません、<forward-path>に転送つもりです。");
                rightCodes.Add("334", "サーバBase64認定を答えました。");
                rightCodes.Add("354", "メール受信開始、<CRLF>.<CRLF>で終わり。");
                //读取系统回应
                StreamReader reader = new StreamReader(ns);
                logs += reader.ReadLine() + CRLF;
            }
            /// <summary>
            /// 向SMTP发送命令
            /// </summary>
            /// <param name="cmd"></param>
            private string sendCommand(string cmd, bool isMailData)
            {
                if (cmd != null && cmd.Trim() != string.Empty)
                {
                    byte[] cmd_b = null;
                    if (!isMailData)//不是邮件数据
                        cmd += CRLF;

                    logs += cmd;
                    //开始写入邮件数据
                    if (!isMailData)
                    {
                        cmd_b = Encoding.ASCII.GetBytes(cmd);
                        ns.Write(cmd_b, 0, cmd_b.Length);
                    }
                    else
                    {
                        cmd_b = Encoding.GetEncoding(this.mailEncodingName).GetBytes(cmd);
                        ns.BeginWrite(cmd_b, 0, cmd_b.Length, new AsyncCallback(this.asyncCallBack), null);
                    }
                    //读取服务器响应
                    StreamReader reader = new StreamReader(ns);
                    string response = reader.ReadLine();
                    logs += response + CRLF;
                    //检查状态码
                    string statusCode = response.Substring(0, 3);
                    bool isExist = false;
                    bool isRightCode = true;
                    foreach (string err in this.errorCodes.Keys)
                    {
                        if (statusCode == err)
                        {
                            isExist = true;
                            isRightCode = false;
                            break;
                        }
                    }
                    foreach (string right in this.rightCodes.Keys)
                    {
                        if (statusCode == right)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    //根据状态码来处理下一步的动作
                    if (!isExist) //不是合法的SMTP主机
                    {
                        this.setError("適切SMTPホストではありません、またはサービスを拒否しました。");
                    }
                    else if (!isRightCode)//命令没能成功执行
                    {
                        this.setError(statusCode + ":" + this.errorCodes[statusCode]);
                    }
                    else //命令成功执行
                    {
                        this.errorMessage = "";
                    }
                    return response;
                }
                else
                {
                    return null;
                }
            }
            /// <summary>
            /// 通过auth login方式登陆smtp服务器
            /// </summary>
            private void landingByLogin()
            {
                string base64UserId = this.convertBase64String(this.UserID, "ASCII");
                string base64Pass = this.convertBase64String(this.Password, "ASCII");
                //握手
                this.sendCommand("helo " + this.serverName, false);
                //开始登陆
                this.sendCommand("auth login", false);
                this.sendCommand(base64UserId, false);
                this.sendCommand(base64Pass, false);
            }
            /// <summary>
            /// 通过auth plain方式登陆服务器
            /// </summary>
            private void landingByPlain()
            {
                string NULL = ((char)0).ToString();
                string loginStr = NULL + this.UserID + NULL + this.Password;
                string base64LoginStr = this.convertBase64String(loginStr, "ASCII");
                //握手
                this.sendCommand("helo " + this.serverName, false);
                //登陆
                this.sendCommand(base64LoginStr, false);
            }
            /// <summary>
            /// 通过auth CRAM-MD5方式登陆
            /// </summary>
            private void landingByCRAMMD5()
            {
                //握手
                this.sendCommand("helo " + this.serverName, false);
                //登陆
                string response = this.sendCommand("auth CRAM-MD5", false);
                //得到服务器返回的标识
                string identifier = response.Remove(0, 4);
                //用MD5加密标识
                //KSN_MACTripleDES mac=new KSN_MACTripleDES();
                //mac.Key=this.Password;
                //mac.Data=Encoding.ASCII.GetBytes(identifier);
                //string hash=mac.GetHashValue();
                //发送用户帐号信息
                //this.sendCommand(this.UserID+" "+hash,false);
            }
            /// <summary>
            /// 发送邮件
            /// </summary>
            /// <returns></returns>
            public bool SendMail(NCSMTPMail mail)
            {
                bool isSended = true;
                this.mailEncodingName = mail.MailEncoding.ToString().Replace("_", "-").ToLower();
                try
                {
                    //检测发送邮件的必要条件
                    if (mail.From=="" && mail.EnvFrom=="")
                    {
                        this.setError("送信人を設定しません。");
                    }
                    if (mail.Receivers.Count == 0)
                    {
                        this.setError("受信人は一件もありません。");
                    }
                    if (this.SmtpValidateType != SmtpValidateTypes.None)
                    {
                        if (this.userid == null || this.password == null)
                        {
                            this.setError("smtp認定が必要です、でもアカウントを設定しません。");
                        }
                    }
                    //开始登陆
                    switch (this.SmtpValidateType)
                    {
                        case SmtpValidateTypes.None:
                            this.sendCommand("helo " + this.serverName, false);
                            break;
                        case SmtpValidateTypes.Login:
                            this.landingByLogin();
                            break;
                        case SmtpValidateTypes.Plain:
                            this.landingByPlain();
                            break;
                        case SmtpValidateTypes.CRAMMD5:
                            this.landingByCRAMMD5();
                            break;
                        default:
                            break;
                    }
                    //初始化邮件会话(对应SMTP命令mail)
                    if (mail.EnvFrom != "")
                    {
                        this.sendCommand("mail from:<" + mail.EnvFrom + ">", false);
                    }
                    else
                    {
                        this.sendCommand("mail from:<" + mail.From + ">", false);
                    }
                    //标识收件人(对应SMTP命令Rcpt)
                    foreach (string receive in mail.Receivers)
                    {
                        this.sendCommand("rcpt to:<" + receive + ">", false);
                    }
                    if (mail.CCs != null && mail.CCs.Count > 0)
                    {
                        foreach (string receive in mail.CCs)
                        {
                            this.sendCommand("rcpt to:<" + receive + ">", false);
                        }
                    }
                    if (mail.BCCs != null && mail.BCCs.Count > 0)
                    {
                        foreach (string receive in mail.BCCs)
                        {
                            this.sendCommand("rcpt to:<" + receive + ">", false);
                        }
                    }
                    //标识开始输入邮件内容(Data)
                    this.sendCommand("data", false);
                    //开始编写邮件内容
                    //ENVELOPE ("date" "subject" from sender reply-to to cc bcc in-reply-to "messageID")
                    // modified by zhengjun 20071130 for No3 start
                    //string message = "Date:" + DateTime.Now.ToString("ddd, dd MMM yyyy hh:mm:ss") + " GMT +0400" + CRLF;
                    string message = "Date:" + System.DateTime.Now.ToLocalTime().ToString("R").Replace("GMT","JST") +CRLF;
                    // modified by zhengjun 20071130 for No3 end
                    // modified by zhengjun 20071130 for No6 start
                    //message += "Subject:" + mail.Subject + CRLF;
                    message += "Subject:" + string.Format("=?{0}?B?{1}?=", this.mailEncodingName,mail.Subject) + CRLF;
                    // modified by zhengjun 20071130 for No6 end
                    // modified by zhengjun 20071130 for No5 start
                    if (mail.FromUser != "")
                    {
                        message += "From:" + string.Format("=?{0}?B?{1}?=", this.mailEncodingName, mail.FromUser) + "<" + mail.From + ">" + CRLF;
                    }
                    else
                    {
                        message += "From:" + mail.From + CRLF;
                    }                    
                    if (mail.ReplyTo != "")
                    {
                        message += "Reply-To:" + mail.ReplyTo + CRLF;
                    }
                    // modified by zhengjun 20071130 for No5 end
                    if (mail.EnvTo != "")
                    {
                        message += "To:" + mail.EnvTo + CRLF;
                    }
                    else
                    {
                        message += "To:" + mail.StrColToStr(mail.Receivers) + CRLF;
                    }
                    // modified by zhengjun 20071130 for No4 start
                    if (mail.CCs != null && mail.CCs.Count>0)
                    {
                        message += "Cc:" + mail.StrColToStr(mail.CCs) + CRLF;
                    }
                    if (mail.BCCs != null && mail.BCCs.Count > 0)
                    {
                        message += "Bcc:" + mail.StrColToStr(mail.BCCs) + CRLF;
                    }
                    //message += "In-Reply-To:" + CRLF;
                    // modified by zhengjun 20071130 for No4 end
                    message += "MIME-Version:1.0" + CRLF;
                    if (mail.Attachments.Count == 0)//没有附件
                    {
                        if (mail.MailType == MailTypes.Text) //文本格式
                        {
                            message += "Content-Type:text/plain;" + CRLF + " ".PadRight(8, ' ') + "charset=\"" +
                            this.mailEncodingName + "\"" + CRLF;
                            //message += "Content-Transfer-Encoding:base64" + CRLF + CRLF;
                            //mail.HeadMessage = message;
                            //if (mail.MailBody != null)
                            //    message += Convert.ToBase64String(mail.MailBody, 0, mail.MailBody.Length) + CRLF + CRLF + CRLF + "." + CRLF;
                            message += "Content-Transfer-Encoding:7bit" + CRLF + CRLF;
                            mail.HeadMessage = message;
                            if (mail.MailBody != null)
                                message += mail.MailBody + CRLF + CRLF + CRLF + "." + CRLF;
                        }
                        else//Html格式
                        {
                            message += "Content-Type:multipart/alertnative;" + CRLF + " ".PadRight(8, ' ') + "boundary"
                            + "=\"=====003_Dragon310083331177_=====\"" + CRLF + CRLF + CRLF;
                            message += "This is a multi-part message in MIME format" + CRLF + CRLF;
                            message += "--=====003_Dragon310083331177_=====" + CRLF;
                            message += "Content-Type:text/html;" + CRLF + " ".PadRight(8, ' ') + "charset=\"" +
                            this.mailEncodingName + "\"" + CRLF;
                            //message += "Content-Transfer-Encoding:base64" + CRLF + CRLF;
                            message += "Content-Transfer-Encoding:7bit" + CRLF + CRLF;
                            if (mail.MailBody != null)
                                //message += Convert.ToBase64String(mail.MailBody, 0, mail.MailBody.Length) + CRLF + CRLF;
                                message += mail.MailBody + CRLF + CRLF;
                            message += "--=====003_Dragon310083331177_=====--" + CRLF + CRLF + CRLF + "." + CRLF;
                        }
                    }
                    else//有附件
                    {
                        //处理要在邮件中显示的每个附件的数据
                        StringCollection attatchmentDatas = new StringCollection();
                        foreach (string path in mail.Attachments)
                        {
                            if (!File.Exists(path))
                            {
                                this.setError("添付ファイルが見つかりません。" + path);
                            }
                            else
                            {
                                //得到附件的字节流
                                FileInfo file = new FileInfo(path);
                                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                                if (fs.Length > (long)int.MaxValue)
                                {
                                    this.setError("添付ファイルのサイズを超えた。");
                                }
                                byte[] file_b = new byte[(int)fs.Length];
                                fs.Read(file_b, 0, file_b.Length);
                                fs.Close();
                                string attatchmentMailStr = "Content-Type:application/octet-stream;" + CRLF + " ".PadRight(8, ' ') + "name=" +
                                "\"" + file.Name + "\"" + CRLF;
                                attatchmentMailStr += "Content-Transfer-Encoding:base64" + CRLF;
                                attatchmentMailStr += "Content-Disposition:attachment;" + CRLF + " ".PadRight(8, ' ') + "filename=" +
                                "\"" + file.Name + "\"" + CRLF + CRLF;
                                attatchmentMailStr += Convert.ToBase64String(file_b, 0, file_b.Length) + CRLF + CRLF;
                                attatchmentDatas.Add(attatchmentMailStr);
                            }
                        }
                        //设置邮件信息
                        if (mail.MailType == MailTypes.Text) //文本格式
                        {
                            message += "Content-Type:multipart/mixed;" + CRLF + " ".PadRight(8, ' ') + "boundary=\"=====001_Dragon320037612222_=====\""
                            + CRLF + CRLF;
                            message += "This is a multi-part message in MIME format." + CRLF + CRLF;
                            message += "--=====001_Dragon320037612222_=====" + CRLF;
                            message += "Content-Type:text/plain;" + CRLF + " ".PadRight(8, ' ') + "charset=\"" + this.mailEncodingName + "\"" + CRLF;
                            //message += "Content-Transfer-Encoding:base64" + CRLF + CRLF;
                            message += "Content-Transfer-Encoding:7bit" + CRLF + CRLF;
                            if (mail.MailBody != null)
                                //message += Convert.ToBase64String(mail.MailBody, 0, mail.MailBody.Length) + CRLF;
                                message += mail.MailBody + CRLF;
                            foreach (string s in attatchmentDatas)
                            {
                                message += "--=====001_Dragon320037612222_=====" + CRLF + s + CRLF + CRLF;
                            }
                            message += "--=====001_Dragon320037612222_=====--" + CRLF + CRLF + CRLF + "." + CRLF;
                        }
                        else
                        {
                            message += "Content-Type:multipart/mixed;" + CRLF + " ".PadRight(8, ' ') + "boundary=\"=====001_Dragon255511664284_=====\""
                            + CRLF + CRLF;
                            message += "This is a multi-part message in MIME format." + CRLF + CRLF;
                            message += "--=====001_Dragon255511664284_=====" + CRLF;
                            message += "Content-Type:text/html;" + CRLF + " ".PadRight(8, ' ') + "charset=\"" + this.mailEncodingName + "\"" + CRLF;
                            message += "Content-Transfer-Encoding:base64" + CRLF + CRLF;
                            if (mail.MailBody != null)
                                //message += Convert.ToBase64String(mail.MailBody, 0, mail.MailBody.Length) + CRLF + CRLF;
                                message += mail.MailBody + CRLF + CRLF;
                            for (int i = 0; i < attatchmentDatas.Count; i++)
                            {
                                message += "--=====001_Dragon255511664284_=====" + CRLF + attatchmentDatas[i] + CRLF + CRLF;
                            }
                            message += "--=====001_Dragon255511664284_=====--" + CRLF + CRLF + CRLF + "." + CRLF;
                        }
                    }
                    //发送邮件数据
                    
                    this.sendCommand(message, true);
                    if (this.sendIsComplete)
                        this.sendCommand("QUIT", false);
                }
                catch
                {
                    isSended = false;
                }
                finally
                {
                    this.disconnect();
                    //输出日志文件
                    if (this.LogPath != null)
                    {
                        FileStream fs = null;
                        if (File.Exists(this.LogPath))
                        {
                            fs = new FileStream(this.LogPath, FileMode.Append, FileAccess.Write);
                            this.logs = CRLF + CRLF + this.logs;
                        }
                        else fs = new FileStream(this.LogPath, FileMode.Create, FileAccess.Write);
                        byte[] logPath_b = Encoding.GetEncoding("shift-jis").GetBytes(this.logs);
                        fs.Write(logPath_b, 0, logPath_b.Length);
                        fs.Close();
                    }
                }
                return isSended;
            }
            /// <summary>
            /// 异步写入数据
            /// </summary>
            /// <param name="result"></param>
            private void asyncCallBack(IAsyncResult result)
            {
                if (result.IsCompleted)
                    this.sendIsComplete = true;
            }
            /// <summary>
            /// 关闭连接
            /// </summary> 
            private void disconnect()
            {
                try
                {
                    ns.Close();
                    tc.Close();
                }
                catch
                {
                    ;
                }
            }
            /// <summary>
            /// 设置出现错误时的动作
            /// </summary>
            /// <param name="errorStr"></param>
            private void setError(string errorStr)
            {
                this.errorMessage = errorStr;
                logs += this.errorMessage + CRLF + "【メール処理中止しました】" + CRLF;
                this.disconnect();
                throw new ApplicationException("");
            }
            /// <summary>
            ///将字符串转换为base64
            /// </summary>
            /// <param name="str"></param>
            /// <param name="encodingName"></param>
            /// <returns></returns>
            private string convertBase64String(string str, string encodingName)
            {
                byte[] str_b = Encoding.GetEncoding(encodingName).GetBytes(str);
                return Convert.ToBase64String(str_b, 0, str_b.Length);
            }
            #endregion
        }
    }

}
