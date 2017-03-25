using System;
using System.Collections.Generic;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using System.Collections;

namespace NC.HPS.Lib
{
    public class NCMail
    {
        #region 送信
        /// <summary>
        /// 送信
        /// </summary>
        public static Boolean SendEmail(String subject, string body, string htmlbody, string picfile, string address, string user, string password, string from, string to, string servertype)
        {
            Boolean ret = false;
            List<String> attachmentfiles = new List<string>();
            if (!String.IsNullOrEmpty(picfile))
            {
                attachmentfiles.Add(picfile);
            }
            List<String> recipients = new List<string>();
            recipients.Add(to);
            switch (servertype)
            {
                //sendEMailThroughOUTLOOK(subject+" from outlook", body, attachmentfiles, recipients);
                case "OUTLOOK":
                    ret = sendEMailThroughOUTLOOK(subject, body, htmlbody, attachmentfiles, recipients);
                    break;
                //sendEMailThroughHotMail("tei952@hotmail.com", "zjhuen1915", subject + " from hotmail", body, attachmentfiles, recipients);
                case "HOTMAIL":
                    ret = sendEMailThroughHotMail(from, password, subject, body ,htmlbody, attachmentfiles, recipients);
                    break;
                //sendEMailThroughYahoo("tei952@yahoo.com", "X0x9q01915", subject + " from yahoo", body, attachmentfiles, recipients);
                case "YAHOO":
                    ret = sendEMailThroughYahoo(from, password, subject, body, htmlbody, attachmentfiles, recipients);
                    break;
                //sendEMailThroughAOL("tei952@aol.com", "x0x9q0", subject + " from AOL", body, attachmentfiles, recipients);
                case "AOL":
                    ret = sendEMailThroughAOL(from, password, subject, body ,htmlbody, attachmentfiles, recipients);
                    break;
                //sendEMailThroughGmail("tei952@gamil.com","tei952", "x0x9q0", subject + " from gmail", body, attachmentfiles, recipients);
                case "GMAIL":
                    ret = sendEMailThroughGmail(from, user, password, subject, body, htmlbody, attachmentfiles, recipients);
                    break;
            }
            return ret;
        }
        /// <summary>
        /// get received mail from out llok
        /// </summary>
        public static ArrayList getMailFromOutlook()
        {
            ArrayList ret = new ArrayList(); 
            String filters = "zhengjun@chojo.co.jp;tei952@hotmail.com";
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.NameSpace outlookNS = oApp.GetNamespace("MAPI");
                Outlook.MAPIFolder inboxFolder
                = outlookNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
                foreach(object obj in inboxFolder.Items)
                {
                    Outlook.MailItem item = obj as Outlook.MailItem;
                    if (item!=null && item.Body != null && item.Body != "")
                    {
                        String mailaddress = NCPop3Mail.GetmailAddress(item.Body);
                        mailaddress = NCPop3Mail.filter(mailaddress, filters);
                        if (mailaddress != null && mailaddress != "")
                        {
                            String[] mails = mailaddress.Split(';');
                            foreach (String mail in mails)
                            {
                                if (mail.Trim() != "") ret.Add(mail);
                            }
                        }
                    }
                    if (item != null && item.HTMLBody != null && item.HTMLBody != "")
                    {
                        String mailaddress = NCPop3Mail.GetmailAddress(item.HTMLBody);
                        mailaddress = NCPop3Mail.filter(mailaddress, filters);
                        if (mailaddress != null && mailaddress != "")
                        {
                            String[] mails = mailaddress.Split(';');
                            foreach (String mail in mails)
                            {
                                if (mail.Trim() != "") ret.Add(mail);
                            }
                        }
                    }
                }
                oApp = null;
            }//end of try block
            catch (System.Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
            }//end of catch
            return ret;
        }
        //method to send email to outlook
        public static Boolean sendEMailThroughOUTLOOK(String subject, String body, String htmlbody, List<String> attachmentfiles, List<String> recipients)
        {
            Boolean ret = true;
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                if (String.IsNullOrEmpty(htmlbody))
                {
                    // Set HTMLBody. 
                    //add the body of the email
                    oMsg.Body = body;
                    //Add an attachment.
                    int iPosition = (int)oMsg.Body.Length + 1;
                    int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                    //now attached the file
                    foreach (String attachmentfile in attachmentfiles)
                    {
                        String sDisplayName = Path.GetFileName(attachmentfile);
                        oMsg.Attachments.Add(attachmentfile, iAttachType, iPosition, sDisplayName);
                    }
                }
                else
                {
                    // Set HTMLBody. 
                    //add the body of the email
                    oMsg.HTMLBody = htmlbody;
                    //Add an attachment.
                    int iPosition = (int)oMsg.HTMLBody.Length + 1;
                    int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                    //now attached the file
                    foreach (String attachmentfile in attachmentfiles)
                    {
                        String sDisplayName = Path.GetFileName(attachmentfile);
                        oMsg.Attachments.Add(attachmentfile, iAttachType, iPosition, sDisplayName);
                    }
                }
                //Subject line
                oMsg.Subject = subject;
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                foreach (String recipient in recipients)
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(recipient);
                    oRecip.Resolve();
                    //oRecip = null;
                }
                // Send.
                oMsg.Send();
                // Clean up.
                oRecips = null;
                oMsg = null;
                oApp = null;
            }//end of try block
            catch (System.Exception ex)
            {
                ret = false;
                NCLogger.GetInstance().WriteExceptionLog(ex);
            }//end of catch
            return ret;
        }//end of Email Method  
        //method to send email to HOTMAIL
        public static Boolean sendEMailThroughHotMail(String sender, String password, String subject, String body, String htmlbody, List<String> attachmentfiles, List<String> recipients)
        {
            Boolean ret = true;
            try
            {
                //Mail Message
                MailMessage mM = new MailMessage();
                //Mail Address
                mM.From = new MailAddress(sender);
                //receiver email id
                foreach (String recipient in recipients)
                {
                    mM.To.Add(recipient);
                }
                //subject of the email
                mM.Subject = subject;
                //deciding for the attachment
                foreach (String attachmentfile in attachmentfiles)
                {
                    mM.Attachments.Add(new Attachment(attachmentfile));
                }
                if (!String.IsNullOrEmpty(htmlbody))
                {
                    //add the body of the email
                    mM.Body = htmlbody;
                    mM.IsBodyHtml = true;
                }
                else
                {
                    //add the body of the email
                    mM.Body = body;
                    mM.IsBodyHtml = false;
                }
                //SMTP client
                SmtpClient sC = new SmtpClient("smtp.live.com");
                //port number for Hot mail
                sC.Port = 587;
                //credentials to login in to hotmail account
                sC.Credentials = new System.Net.NetworkCredential(sender, password);
                //enabled SSL
                sC.EnableSsl = true;
                //Send an email
                sC.Send(mM);
            }//end of try block
            catch (Exception ex)
            {
                ret = false;
                //MessageBox.Show("Hotmail " + ex.Message);
                NCLogger.GetInstance().WriteExceptionLog(ex);
            }//end of catch
            return ret;
        }//end of Email Method 
        //Method to send email from YAHOO!!
        public static Boolean sendEMailThroughYahoo(String sender, String password, String subject, String body, String htmlbody, List<String> attachmentfiles, List<String> recipients)
        {
            Boolean ret = true;
            try
            {
                //mail message
                MailMessage mM = new MailMessage();
                //Mail Address
                mM.From = new MailAddress(sender);
                foreach (String recipient in recipients)
                {
                    mM.To.Add(recipient);
                }
                //subject of the email
                mM.Subject = subject;
                //deciding for the attachment
                foreach (String attachmentfile in attachmentfiles)
                {
                    mM.Attachments.Add(new Attachment(attachmentfile));
                }
                if (!String.IsNullOrEmpty(htmlbody))
                {
                    //add the body of the email
                    mM.Body = htmlbody;
                    mM.IsBodyHtml = true;
                }
                else
                {
                    //add the body of the email
                    mM.Body = body;
                    mM.IsBodyHtml = false;
                }
                //SMTP 
                SmtpClient SmtpServer = new SmtpClient();
                //your credential will go here
                SmtpServer.Credentials = new System.Net.NetworkCredential(sender, password);
                //port number to login yahoo server
                SmtpServer.Port = 587;
                //yahoo host name
                SmtpServer.Host = "smtp.mail.yahoo.com";
                //Send the email
                SmtpServer.Send(mM);
            }//end of try block
            catch (Exception ex)
            {
                ret = false;
                NCLogger.GetInstance().WriteExceptionLog(ex);
                //MessageBox.Show("yahoo " + ex.Message);
            }//end of catch
            return ret;
        }//end of Yahoo Email MethodHotMail
        //Method to send email from YAHOO!!
        public static Boolean sendEMailThroughAOL(String sender, String password, String subject, String body, String htmlbody, List<String> attachmentfiles, List<String> recipients)
        {
            Boolean ret = true;
            try
            {
                //mail message
                MailMessage mM = new MailMessage();
                //Mail Address
                mM.From = new MailAddress(sender);
                foreach (String recipient in recipients)
                {
                    mM.To.Add(recipient);
                }
                //subject of the email
                mM.Subject = subject;
                //deciding for the attachment
                foreach (String attachmentfile in attachmentfiles)
                {
                    mM.Attachments.Add(new Attachment(attachmentfile));
                }
                if (!String.IsNullOrEmpty(htmlbody))
                {
                    //add the body of the email
                    mM.Body = htmlbody;
                    mM.IsBodyHtml = true;
                }
                else
                {
                    //add the body of the email
                    mM.Body = body;
                    mM.IsBodyHtml = false;
                }
                //SMTP 
                SmtpClient SmtpServer = new SmtpClient();
                //your credential will go here
                SmtpServer.Credentials = new System.Net.NetworkCredential(sender, password);
                //port number to login yahoo server
                SmtpServer.Port = 587;
                //yahoo host name
                SmtpServer.Host = "smtp.aol.com";
                //Send the email
                SmtpServer.Send(mM);
            }//end of try block
            catch (Exception ex)
            {
                ret = false;
                NCLogger.GetInstance().WriteExceptionLog(ex);
                //MessageBox.Show("AOL " + ex.Message);
            }//end of catch
            return ret;
        }//end of AOLEmail Method
        //method to send email to Gmail
        public static Boolean sendEMailThroughGmail(String sender, String user, String password, String subject, String body, String htmlbody, List<String> attachmentfiles, List<String> recipients)
        {
            Boolean ret = true;
            try
            {
                //Mail Message
                MailMessage mM = new MailMessage();
                //Mail Address
                mM.From = new MailAddress(sender);
                foreach (String recipient in recipients)
                {
                    mM.To.Add(recipient);
                }
                //subject of the email
                mM.Subject = subject;
                //deciding for the attachment
                foreach (String attachmentfile in attachmentfiles)
                {
                    mM.Attachments.Add(new Attachment(attachmentfile));
                }
                if (!String.IsNullOrEmpty(htmlbody))
                {
                    //add the body of the email
                    mM.Body = htmlbody;
                    mM.IsBodyHtml = true;
                }
                else
                {
                    //add the body of the email
                    mM.Body = body;
                    mM.IsBodyHtml = false;
                }
                //SMTP client
                SmtpClient sC = new SmtpClient("smtp.gmail.com");
                //port number for Gmail mail
                sC.Port = 587;
                //credentials to login in to Gmail account
                sC.Credentials = new NetworkCredential(user, password);
                //enabled SSL
                sC.EnableSsl = true;
                //Send an email
                sC.Send(mM);
            }//end of try block
            catch (Exception ex)
            {
                ret = false;
                NCLogger.GetInstance().WriteExceptionLog(ex);
                //MessageBox.Show("GAMIL " + ex.Message);
            }//end of catch
            return ret;
        }//end of /method to send email to Gmail
        #endregion
    }
}
