using System;
using System.Collections.Generic;
using System.Text;

namespace NC.HPS.Lib
{
    public class NCFTP
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="upFile"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Boolean uploadFile(string url, string upFile, string username, string password)
        {
            Boolean ret = true;
            try
            {
                //アップロード先のURI
                Uri u = new Uri(url);

                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential(username, password);
                //MethodにWebRequestMethods.Ftp.UploadFile("STOR")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = true;
                //PASVモードを無効にする
                ftpReq.UsePassive = false;

                //ファイルをアップロードするためのStreamを取得
                System.IO.Stream reqStrm = ftpReq.GetRequestStream();
                //アップロードするファイルを開く
                System.IO.FileStream fs = new System.IO.FileStream(
                    upFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //アップロードStreamに書き込む
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int readSize = fs.Read(buffer, 0, buffer.Length);
                    if (readSize == 0)
                        break;
                    reqStrm.Write(buffer, 0, readSize);
                }
                fs.Close();
                reqStrm.Close();

                //FtpWebResponseを取得
                System.Net.FtpWebResponse ftpRes =
                    (System.Net.FtpWebResponse)ftpReq.GetResponse();
                //閉じる
                ftpRes.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="downFile"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Boolean download(string url, string downFile, string username, string password)
        {
            Boolean ret = true;
            try
            {
                //ダウンロードするファイルのURI
                Uri u = new Uri(url);

                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential(username, password);
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = true;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = false;

                //FtpWebResponseを取得
                System.Net.FtpWebResponse ftpRes =
                    (System.Net.FtpWebResponse)ftpReq.GetResponse();
                //ファイルをダウンロードするためのStreamを取得
                System.IO.Stream resStrm = ftpRes.GetResponseStream();
                //ダウンロードしたファイルを書き込むためのFileStreamを作成
                System.IO.FileStream fs = new System.IO.FileStream(
                    downFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                //ダウンロードしたデータを書き込む
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int readSize = resStrm.Read(buffer, 0, buffer.Length);
                    if (readSize == 0)
                        break;
                    fs.Write(buffer, 0, readSize);
                }
                fs.Close();
                resStrm.Close();

                //閉じる
                ftpRes.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                ret = false;
            }
            return ret;
        }
    }
}
