using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;
using System.Data;
using System.Text;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using NC.HPS.Lib;


namespace HPSWinService
{
    internal class NGProcess
    {
        #region 变量
        /// <summary>
        /// 时钟
        /// </summary>
        private static System.Timers.Timer process_timer = null;

        /// <summary>
        /// 计数器
        /// </summary>
        private static int time_span = 0;
        /// <summary>
        /// 执行频率(单位分)
        /// </summary>
        private static string notice_span = "1";
        /// <summary>
        /// 本地文件
        /// </summary>
        private static string local_file = @"c:\cjw\filelist.ini";
        /// <summary>
        /// 远程文件
        /// </summary>
        private static string remote_file = @"filelist.ini";
        /// <summary>
        /// FTP服务器
        /// </summary>
        private static string ftpserver = null;
        /// <summary>
        /// 用户名
        /// </summary>
        private static string userName = null;
        /// <summary>
        /// 用户密码
        /// </summary>
        private static string password = null;
        /**
         * 语言
         **/
        private static string language = "";
        /// <summary>
        /// 数据库存取对象
        /// </summary>
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        internal static bool Init()
        {
            NCLogger.GetInstance().WriteInfoLog("Init Start");
            if (!GetConfigValue())
            {
                NCLogger.GetInstance().WriteInfoLog("not GetConfigValue");
                return false;
            }
            try
            {

                process_timer = new System.Timers.Timer();
                process_timer.Interval = 30000;
                process_timer.Elapsed += new System.Timers.ElapsedEventHandler(DoUploadProcess);
                process_timer.Elapsed += new System.Timers.ElapsedEventHandler(DoDownloadProcess);
                process_timer.Elapsed += new System.Timers.ElapsedEventHandler(DoFtpUploadProcess);
                process_timer.Elapsed += new System.Timers.ElapsedEventHandler(DoCustomerSyncProcess);
                process_timer.Elapsed += new System.Timers.ElapsedEventHandler(DoSendMailProcess);
                process_timer.Start();


            }
            catch(Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
            }
            NCLogger.GetInstance().WriteInfoLog("Init end");
            return true;
        }
        /// <summary>
        /// 配置
        /// </summary>
        /// <returns></returns>
        internal static bool GetConfigValue()
        {
            bool ret = true;
            ///取得配置信息
            NCLogger.GetInstance().WriteInfoLog("GetConfigValue Start");
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.ReadXmlData("config", "language", ref language))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("notice", "Span", ref notice_span))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "ftpserver", ref ftpserver))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "ftpuser", ref userName))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "ftppassword", ref password))
            {
                ret = false;
            }
            password = NCCryp.Decrypto(password);

            NCLogger.GetInstance().WriteInfoLog("GetConfigValue end");
            return ret;
        }
        #endregion

        #region 送信
        /**
         *设置送信状态
        **/
        private static void setSend(String sendid, String Status, String address)
        {
            int id = 0;
            String wheresql = "送信编号=" + sendid;
            String valuesql = "服务器名称='" + address + "',送信状态='" + Status + "'";
            db.SetSend(0, 1, "", wheresql, valuesql, out id);
        }
        struct MailPara
        {
            public string sendid;
            public string name;
            public string subject;
            public string body;
            public string htmlbody;
            public string picfile;
            public string address;
            public string user;
            public string password;
            public string from;
            public string to;
            public string servertype;
            public string attachement;
            public string isHtml;
        }
        /// <summary>
        /// 开始送信
        /// </summary>
        private static void startSend()
        {
            WorkQueue<MailPara> workQueue_Outlook = new WorkQueue<MailPara>(5);
            workQueue_Outlook.UserWork += new UserWorkEventHandler<MailPara>(workQueue_UserWork);
            workQueue_Outlook.WorkSequential = true;

            WorkQueue<MailPara> workQueue_HotMail = new WorkQueue<MailPara>(5);
            workQueue_HotMail.UserWork += new UserWorkEventHandler<MailPara>(workQueue_UserWork);
            workQueue_HotMail.WorkSequential = true;

            WorkQueue<MailPara> workQueue_GMail = new WorkQueue<MailPara>(5);
            workQueue_GMail.UserWork += new UserWorkEventHandler<MailPara>(workQueue_UserWork);
            workQueue_GMail.WorkSequential = true;

            WorkQueue<MailPara> workQueue_YMail = new WorkQueue<MailPara>(5);
            workQueue_YMail.UserWork += new UserWorkEventHandler<MailPara>(workQueue_UserWork);
            workQueue_YMail.WorkSequential = true;

            WorkQueue<MailPara> workQueue_AMail = new WorkQueue<MailPara>(5);
            workQueue_AMail.UserWork += new UserWorkEventHandler<MailPara>(workQueue_UserWork);
            workQueue_AMail.WorkSequential = true;

            ThreadPool.QueueUserWorkItem(o =>
            {
                DataSet ds_Server = new DataSet();
                if (db.GetMailServer(0, 0, "*", "", "", ref ds_Server) && ds_Server.Tables[0].Rows.Count > 0)
                {
                    Hashtable ht = new Hashtable();
                    foreach (DataRow dr in ds_Server.Tables[0].Rows)
                    {

                        MailPara mp = new MailPara();
                        mp.name = dr["名称"].ToString();
                        mp.address = dr["地址"].ToString();
                        //mp.port = dr["端口"].ToString();
                        mp.user = dr["用户"].ToString();
                        mp.password = dr["密码"].ToString();
                        mp.from = dr["送信人地址"].ToString();
                        mp.servertype = dr["服务器类型"].ToString().Trim();
                        mp.attachement = dr["添付文件"].ToString();
                        mp.isHtml = dr["HTML"].ToString();
                        ht[mp.servertype] = mp;
                    }
                    DataSet ds = new DataSet();
                    String strWhere = "送信状态='未送信'";
                    if (db.GetPublishVW(0, 0, "*", strWhere, "", ref ds))
                    {
                        int idx = 0;
                        while (idx < ds.Tables[0].Rows.Count)
                        {
                            if (workQueue_Outlook.getCount() < 5)
                            {
                                MailPara mp = getMailPara(ht, "OUTLOOK");
                                if (mp.servertype != null)
                                {
                                    mp = getMailPara(ds, idx, mp);
                                    workQueue_Outlook.EnqueueItem(mp);
                                    idx++;
                                    if (idx >= ds.Tables[0].Rows.Count) break;
                                }
                            }
                            if (workQueue_HotMail.getCount() < 5)
                            {
                                MailPara mp = getMailPara(ht, "HOTMAIL");
                                if (mp.servertype != null)
                                {
                                    mp = getMailPara(ds, idx, mp);
                                    workQueue_HotMail.EnqueueItem(mp);
                                    idx++;
                                    if (idx >= ds.Tables[0].Rows.Count) break;
                                }
                            }
                            if (workQueue_YMail.getCount() < 5)
                            {
                                MailPara mp = getMailPara(ht, "YAHOO");
                                if (mp.servertype != null)
                                {
                                    mp = getMailPara(ds, idx, mp);
                                    workQueue_YMail.EnqueueItem(mp);
                                    idx++;
                                    if (idx >= ds.Tables[0].Rows.Count) break;
                                }
                            }
                            if (workQueue_GMail.getCount() < 5)
                            {
                                MailPara mp = getMailPara(ht, "GMAIL");
                                if (mp.servertype != null)
                                {
                                    mp = getMailPara(ds, idx, mp);
                                    workQueue_GMail.EnqueueItem(mp);
                                    idx++;
                                    if (idx >= ds.Tables[0].Rows.Count) break;
                                }
                            }
                            if (workQueue_AMail.getCount() < 5)
                            {
                                MailPara mp = getMailPara(ht, "AOL");
                                if (mp.servertype != null)
                                {
                                    mp = getMailPara(ds, idx, mp);
                                    workQueue_AMail.EnqueueItem(mp);
                                    idx++;
                                    if (idx >= ds.Tables[0].Rows.Count) break;
                                }
                            }
                        }
                    }

                }

            });
        }
        /// <summary>
        /// getMailPara
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="idx"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        private static MailPara getMailPara(DataSet ds, int idx, MailPara mp)
        {
            mp.sendid = ds.Tables[0].Rows[idx]["送信编号"].ToString();
            mp.to = ds.Tables[0].Rows[idx]["mail"].ToString();
            mp.subject = ds.Tables[0].Rows[idx]["名称"].ToString()
                + ds.Tables[0].Rows[idx]["发行期号"].ToString()
                + "[" + ds.Tables[0].Rows[idx]["发行日期"].ToString() + "] By "
                + mp.name;
            //mp.pdffile = ds.Tables[0].Rows[idx]["文件链接"].ToString();
            mp.body = ds.Tables[0].Rows[idx]["文本内容"].ToString();
            mp.htmlbody = ds.Tables[0].Rows[idx]["邮件内容"].ToString();
            mp.picfile = ds.Tables[0].Rows[idx]["图片链接"].ToString();
            if (mp.isHtml != "Y")
            {
                mp.htmlbody = null;
            }
            if (mp.attachement != "Y")
            {
                mp.picfile = null;
            }
            return mp;
        }
        /// <summary>
        /// getMailPara
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="MailType"></param>
        /// <returns></returns>
        private static MailPara getMailPara(Hashtable ht, string ServerType)
        {
            MailPara mp = new MailPara();
            if (ht[ServerType] != null)
            {
                mp.address = ((MailPara)ht[ServerType]).address;
                mp.user = ((MailPara)ht[ServerType]).user;
                mp.password = ((MailPara)ht[ServerType]).password;
                mp.from = ((MailPara)ht[ServerType]).from;
                mp.servertype = ((MailPara)ht[ServerType]).servertype;
                mp.attachement = ((MailPara)ht[ServerType]).attachement;
                mp.isHtml = ((MailPara)ht[ServerType]).isHtml;
            }
            return mp;
        }
        /// <summary>
        /// workQueue_UserWork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void workQueue_UserWork(object sender, WorkQueue<MailPara>.EnqueueEventArgs e)
        {
            MailPara mp = (MailPara)e.Item;
            setSend(mp.sendid, "送信中", mp.address);
            if (NCMail.SendEmail(mp.subject, mp.body, mp.htmlbody, mp.picfile, mp.address, mp.user, mp.password, mp.from, mp.to, mp.servertype))
            {
                setSend(mp.sendid, "送信完", mp.address);
            }
            else
            {
                setInvidMail(mp.to);
            }
            Thread.Sleep(15);
        }
        /// <summary>
        /// 设置无效邮件
        /// </summary>
        /// <param name="mail"></param>
        private static void setInvidMail(string mail)
        {
            String fieldlist = "ID,邮件地址,状况,消息";
            String valuelist = "'','" + mail + "','无效地址','送信不可'";
            int id = 0;
            db.SetInvalidCustomer(0, 0, fieldlist, "", valuelist, out id);
        }
        /// <summary>
        /// 邮件送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void DoSendMailProcess(object sender, ElapsedEventArgs e)
        {
            NCLogger.GetInstance().WriteInfoLog("DoSendMailProcess Start");
            time_span++;
            if (time_span.ToString() == notice_span)
            {
                time_span = 0;
                try
                {
                    process_timer.Stop();
                    startSend();
                }
                catch (Exception ex)
                {
                    NCLogger.GetInstance().WriteExceptionLog(ex);
                }
                finally
                {
                    process_timer.Start();
                }
            }
            NCLogger.GetInstance().WriteInfoLog("DoSendMailProcess end");
        }


        #endregion
        
        #region 释放
        /// <summary>
        /// 释放
        /// </summary>
        internal static void Dispose()
        {
            NCLogger.GetInstance().WriteInfoLog("Dispose Start");
            process_timer.Stop();
            process_timer.Close();
            process_timer.Dispose();
            NCLogger.GetInstance().WriteInfoLog("Dispose end");
        }
        #endregion

        #region 本系统文档上传处理
        /**
         *ＦＴＰアップロードタスク 
         *アップロードしないタスクを実行する
        */
        private static void FtpUpload()
        {
                DataSet ds = new DataSet();
                String strWhere = "上传状态='未上传'";
                if (db.GetFTPUploadVW(0, 0, "*", strWhere, "", ref ds))
                {
                    NCFTP ftp = new NCFTP();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        String filename = dr["上传名称"].ToString();
                        String url = dr["FTP文件"].ToString();
                        string uploadid = dr["上传编号"].ToString();
                        setUpload(uploadid, "上传中");
                        if (ftp.uploadFile(url, filename, dr["用户"].ToString(), dr["密码"].ToString()))
                        {
                            setUpload(uploadid, "上传完");
                        }
                        else
                        {
                            setUpload(uploadid, "未上传");
                        }
                    }
                }
        }
        /**
         *设置上传状态
        **/
        private static void setUpload(String uploadid, String Status)
        {
            int id = 0;
            String wheresql = "上传编号=" + uploadid;
            String valuesql = "上传状态='" + Status + "'";
            db.SetFTPUpload(0, 1, "", wheresql, valuesql, out id);
        }
        /// <summary>
        /// 文档上传处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void DoFtpUploadProcess(object sender, ElapsedEventArgs e)
        {
            NCLogger.GetInstance().WriteInfoLog("DoFtpUploadProcess Start");
            time_span++;
            if (time_span.ToString() == notice_span)
            {
                time_span = 0;
                try
                {
                    process_timer.Stop();
                    FtpUpload();
                }
                catch (Exception ex)
                {
                    NCLogger.GetInstance().WriteExceptionLog(ex);
                }
                finally
                {
                    process_timer.Start();
                }
            }
            NCLogger.GetInstance().WriteInfoLog("DoFtpUploadProcess end");
        }
        #endregion

        #region お客データ同期
        /**
         * お客データ同期
         */ 
        private static void CustomerSync()
        {
                DataSet ds = new DataSet();
                if (db.GetCustomerDB(0, 0, "*", "", "", ref ds) && ds.Tables[0].Rows.Count > 0)
                {
                    for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                    {
                        string[] fields = new string[14];
                        fields[0] = ds.Tables[0].Rows[idx]["mail"].ToString();
                        fields[1] = ds.Tables[0].Rows[idx]["Cname"].ToString();
                        fields[2] = ds.Tables[0].Rows[idx]["name"].ToString();
                        fields[3] = ds.Tables[0].Rows[idx]["postcode"].ToString();
                        fields[4] = ds.Tables[0].Rows[idx]["address"].ToString();
                        fields[5] = ds.Tables[0].Rows[idx]["tel"].ToString();
                        fields[6] = ds.Tables[0].Rows[idx]["fax"].ToString();
                        fields[7] = ds.Tables[0].Rows[idx]["kind"].ToString();
                        fields[8] = ds.Tables[0].Rows[idx]["format"].ToString();
                        fields[9] = ds.Tables[0].Rows[idx]["scale"].ToString();
                        fields[10] = ds.Tables[0].Rows[idx]["CYMD"].ToString();
                        fields[11] = ds.Tables[0].Rows[idx]["other"].ToString();
                        fields[12] = ds.Tables[0].Rows[idx]["web"].ToString();
                        fields[13] = ds.Tables[0].Rows[idx]["jCName"].ToString();

                        String strId = ds.Tables[0].Rows[idx]["id"].ToString();
                        String strDBType = ds.Tables[0].Rows[idx]["dbType"].ToString();
                        String strServerName = ds.Tables[0].Rows[idx]["ServerName"].ToString();
                        String strUserName = ds.Tables[0].Rows[idx]["UserName"].ToString();
                        String strPassword = ds.Tables[0].Rows[idx]["Password"].ToString();
                        String strDBName = ds.Tables[0].Rows[idx]["DBName"].ToString();
                        String strTableName = ds.Tables[0].Rows[idx]["TableName"].ToString();
                        if (NCDb.GetAllDBData(strDBType, strServerName, strUserName,
                            strPassword, strDBName, strTableName, fields, ref ds) && ds.Tables.Count > 0)
                        {
                            int successCount = 0;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                int id = 0;
                                String valueList = "'" + ds.Tables[0].Rows[i][fields[1]] + "','" + ds.Tables[0].Rows[i][fields[2]]
                                    + "','" + ds.Tables[0].Rows[i][fields[3]] + "','"
                                    + ds.Tables[0].Rows[i][fields[4]] + "','" + ds.Tables[0].Rows[i][fields[5]]
                                    + "','" + ds.Tables[0].Rows[i][fields[6]] + "','" + ds.Tables[0].Rows[i][fields[7]]
                                    + "','" + ds.Tables[0].Rows[i][fields[8]] + "','" + ds.Tables[0].Rows[i][fields[9]]
                                    + "','" + ds.Tables[0].Rows[i][fields[10]] + "','" + ds.Tables[0].Rows[i][fields[11]]
                                    + "','" + ds.Tables[0].Rows[i][fields[12]] + "','" + ds.Tables[0].Rows[i][fields[13]]
                                    + "','" + ds.Tables[0].Rows[i][fields[13]] + "','"
                                    + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','Y','" + db.UserID + "'";
                                if (db.SetCustomer(0, 0, "Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID",
                                                     "", valueList, out id))
                                {
                                    successCount++;
                                }
                            }

                        }
                    }
                }

        }
        /// <summary>
        /// 客户同期处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void DoCustomerSyncProcess(object sender, ElapsedEventArgs e)
        {
            NCLogger.GetInstance().WriteInfoLog("DoCustomerSyncProcess Start");
            time_span++;
            if (time_span.ToString() == notice_span)
            {
                time_span = 0;
                try
                {
                    process_timer.Stop();
                    CustomerSync();
                }
                catch (Exception ex)
                {
                    NCLogger.GetInstance().WriteExceptionLog(ex);
                }
                finally
                {
                    process_timer.Start();
                }
            }
            NCLogger.GetInstance().WriteInfoLog("DoCustomerSyncProcess end");
        }
        #endregion

        #region 交换文档上传下载处理
        /// <summary>
        /// 文档上传处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void DoUploadProcess(object sender, ElapsedEventArgs e)
        {
            NCLogger.GetInstance().WriteInfoLog("DoUploadProcess Start");
            time_span++;
            if (time_span.ToString() == notice_span)
            {
                time_span = 0;
                try
                {
                    process_timer.Stop();
                    UploadProcess();
                }
                catch (Exception ex)
                {
                    NCLogger.GetInstance().WriteExceptionLog(ex);
                }
                finally
                {
                    process_timer.Start();
                }
            }
            NCLogger.GetInstance().WriteInfoLog("DoUploadProcess end");
        }
        /// <summary>
        /// 文档下载处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void DoDownloadProcess(object sender, ElapsedEventArgs e)
        {
            NCLogger.GetInstance().WriteInfoLog("DoDownloadProcess Start");
            time_span++;
            if (time_span.ToString() == notice_span)
            {
                time_span = 0;
                try
                {
                    process_timer.Stop();
                    DownloadProcess();
                }
                catch (Exception ex)
                {
                    NCLogger.GetInstance().WriteExceptionLog(ex);
                }
                finally
                {
                    process_timer.Start();
                }
            }
            NCLogger.GetInstance().WriteInfoLog("DoDownloadProcess end");
        }
        /// <summary>
        /// 上传处理
        /// </summary>
        /// <returns></returns>
        private static bool UploadProcess()
        {
            string[] files = getFileListFromDB();
            foreach (string file in files)
            {
                IDictionary<string, string> info = getInforFromDB(file);
                AddInforToPDF(file, info);
                uploadFile(file);
                updateFilelist(file);
            }
            return true;
        }

        /// <summary>
        /// 下载处理
        /// </summary>
        /// <returns></returns>
        private static bool DownloadProcess()
        {
            string[] files = getFileListFromFtp();
            {
                foreach (string file in files)
                {
                    if (!isFileExits(file))
                    {
                        if (downloadFile(file))
                        {
                            return RegistFile(file);
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///  读取文件
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="filename"></param>
        private static void ReadFile(out byte[] fileContent, string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            fileContent = new byte[fs.Length];

            fs.Read(fileContent, 0, fileContent.Length);

            fs.Close();
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="filename"></param>
        private static void WritetoFile(byte[] fileContent, string filename)
        {
            if (fileContent != null)
            {
                FileStream fs = new FileStream(filename, FileMode.Create);
                BinaryWriter w = new BinaryWriter(fs);
                w.Write(fileContent);
                w.Close();
                fs.Close();
            }
        }
        /// <summary>
        /// 取得要上传文件列表
        /// </summary>
        /// <returns></returns>
        private static string[] getFileListFromDB()
        {
            DataSet ds = new DataSet();
            List<string> files = new List<string>(); 
            String fieldlist = "本地文件";
            if (db.GetSPublish(0, 0, fieldlist, "期刊状态='未上传'", "", ref ds) && ds.Tables[0].Rows.Count > 0)
            {
                files.Add( ds.Tables[0].Rows[0]["S发行编号"].ToString());
            }
            return files.ToArray();
        }
        /// <summary>
        /// 取得要下载文件列表
        /// </summary>
        /// <returns></returns>
        private static string[] getFileListFromFtp()
        {
            byte[] content = null;
            NCFTP ftp = new NCFTP();
            if (ftp.download(Path.Combine(ftpserver, remote_file), local_file, userName, password))
            {
                ReadFile(out content, local_file);
                string remotefilelist = System.Text.Encoding.Default.GetString(content);
                string[] files = remotefilelist.Split(';');
                return files;
            }
            return null;
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file"></param>
        private static void uploadFile(string file)
        {
            NCFTP ftp = new NCFTP();
            string url = Path.Combine(ftpserver, Path.GetFileName(file));
            ftp.uploadFile(url,file, userName, password);
        }
        /// <summary>
        /// 文件信息取得
        /// </summary>
        /// <param name="file"></param>
        private static IDictionary<string, string> getInforFromDB(string file)
        {
            IDictionary<string, string> info = new Dictionary<string,string>();
            DataSet ds = new DataSet();
            String fieldlist = "S发行编号,S期刊编号,期刊编号,发行编号,发行期号,发行日期,文件链接,本地文件,FTP文件,图片链接,本地图片,FTP图片,文本内容,邮件内容,期刊状态,UserID";
            if (db.GetSPublish(0, 0, fieldlist, "本地文件='" + file +"'", "", ref ds) && ds.Tables[0].Rows.Count > 0)
            {
                info.Add("S发行编号", ds.Tables[0].Rows[0]["S发行编号"].ToString());
                info.Add("S期刊编号", ds.Tables[0].Rows[0]["S期刊编号"].ToString());
                info.Add("期刊编号", ds.Tables[0].Rows[0]["期刊编号"].ToString());
                info.Add("发行期号", ds.Tables[0].Rows[0]["发行期号"].ToString());
                info.Add("发行日期", ds.Tables[0].Rows[0]["发行日期"].ToString());
                info.Add("文件链接", ds.Tables[0].Rows[0]["文件链接"].ToString());
                info.Add("本地文件", ds.Tables[0].Rows[0]["本地文件"].ToString());
                info.Add("FTP文件", ds.Tables[0].Rows[0]["FTP文件"].ToString());
                info.Add("图片链接", ds.Tables[0].Rows[0]["图片链接"].ToString());
                info.Add("本地图片", ds.Tables[0].Rows[0]["本地图片"].ToString());
                info.Add("FTP图片", ds.Tables[0].Rows[0]["FTP图片"].ToString());
                info.Add("文本内容", ds.Tables[0].Rows[0]["文本内容"].ToString());
                info.Add("邮件内容", ds.Tables[0].Rows[0]["邮件内容"].ToString());
                info.Add("期刊状态", ds.Tables[0].Rows[0]["期刊状态"].ToString());
                info.Add("UserID", ds.Tables[0].Rows[0]["UserID"].ToString());
            }
            return info;
        }
        /// <summary>
        /// 文件信息取得
        /// </summary>
        /// <param name="file"></param>
        private static IDictionary<string, string> getInforFromFile(string file)
        {
            IDictionary<string, string> info = new Dictionary<string, string>();
            NCPDF.GetInfor(file, ref info);
            return info;
        }
        /// <summary>
        /// 向PDF文件中加入信息
        /// </summary>
        /// <param name="file"></param>
        /// <param name="infor"></param>
        private static void AddInforToPDF(string file, IDictionary<string, string> infor)
        {
            string dest = file+".tmp";
            if (NCPDF.AddInfor(file, dest, infor))
            {
                File.Copy(dest, file, true);
                File.Delete(dest);
            }

        }
        /// <summary>
        /// 向PDF文件中加入信息
        /// </summary>
        /// <param name="file"></param>
        /// <param name="infor"></param>
        private static void AddInforToDB(string file, IDictionary<string, string> infor)
        {
            DataSet ds = new DataSet();
            if (db.GetSPublish(0, 0, "*", "本地文件='" + file + "'", "", ref ds) && ds.Tables[0].Rows.Count > 0)
            {
            }
            else
            {
                int id=0;
                String fieldlist = "S发行编号,S期刊编号,期刊编号,发行编号,发行期号,发行日期,文件链接,本地文件,FTP文件,图片链接,本地图片,FTP图片,文本内容,邮件内容,期刊状态,UserID";
                String valuelist = "'" 
                    + infor["S发行编号"] + "','" 
                    + infor["S期刊编号"] + "',"
                    + infor["期刊编号"] + ","
                    + infor["发行编号"] + ",'"
                    + infor["发行期号"] + "','"
                    + infor["发行日期"] + "','"
                    + infor["文件链接"] + "','"
                    + infor["本地文件"] + "','"
                    + infor["FTP文件"] + "','"
                    + infor["图片链接"] + "','"
                    + infor["本地图片"] + "','"
                    + infor["FTP图片"] + "','"
                    + infor["文本内容"] + "','"
                    + infor["邮件内容"] + "','"
                    + infor["期刊状态"] + "','"
                    + infor["UserID"] + "'";
                db.SetSPublish(0, 0, fieldlist,
                                     "", valuelist, out id);
            }

        }
        /// <summary>
        /// 文件一览更新
        /// </summary>
        /// <param name="file"></param>
        private static void updateFilelist(string file)
        {
            byte[] content = null;
            NCFTP ftp = new NCFTP();
            if (ftp.download(Path.Combine(ftpserver, remote_file), local_file, userName, password))
            {
                ReadFile(out content, local_file);
                string remotefilelist = System.Text.Encoding.Default.GetString(content);
                remotefilelist += ";" + Path.GetFileName(file);
                WritetoFile(System.Text.Encoding.Default.GetBytes(remotefilelist), file);
                ftp.uploadFile(Path.Combine(ftpserver, remote_file), local_file, userName, password);
            }
        }
        /// <summary>
        /// 文件存在判断
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool isFileExits(string file)
        {
            DataSet ds = new DataSet();
            if (db.GetSPublish(0, 0, "*", "本地文件='" + file + "'", "", ref ds) && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool downloadFile(string file)
        {
            NCFTP ftp = new NCFTP();
            string src = Path.GetFileName(file);
            if (ftp.download(Path.Combine(ftpserver, src), file, userName, password))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 文件注册
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool RegistFile(string file)
        {
            IDictionary<string,string> infor = getInforFromFile(file);
            AddInforToDB(file, infor);
            return true;
        }
        #endregion
        
    }

}