using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;
using System.Collections;
using System.Threading;

namespace HPSManagement
{
    public partial class FormSendMail : Form
    {
        private CmWinServiceAPI db;
        public FormSendMail(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// 画面初始化
        /// </summary>
        /// <param e="sender"></param>
        /// <param name="e"></param>
        private void FormServer_Load(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetPublishVW(0, 0, "*", "", "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
                lblMailSendingN.Text = Convert.ToString(ds.Tables[0].Rows.Count);
            }

        }

        /// <summary>
        /// 邮件送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            startSend();
        }
        /**
         *设置送信状态
        **/
        private void setSend(String sendid, String Status, String address)
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
        private void startSend()
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
                    for (int ix = 0; ix < dataGridView1.SelectedRows.Count; ix++)
                    {
                        string ids = dataGridView1.SelectedRows[ix].Cells[0].Value.ToString();

                        DataSet ds = new DataSet();
                        String strWhere = "送信编号=" + ids;
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
                                Application.DoEvents();
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
        private MailPara getMailPara(DataSet ds, int idx, MailPara mp)
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
        private MailPara getMailPara(Hashtable ht, string ServerType)
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
        private void workQueue_UserWork(object sender, WorkQueue<MailPara>.EnqueueEventArgs e)
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
        private void setInvidMail(string mail)
        {
            String fieldlist = "ID,邮件地址,状况,消息";
            String valuelist = "'','" + mail + "','无效地址','送信不可'";
            int id = 0;
            db.SetInvalidCustomer(0, 0, fieldlist, "", valuelist, out id);
        }

    }
}
