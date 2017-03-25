using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Security.Permissions;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.IO;

namespace NC.HPS.Lib
{
	/// <summary>
	/// アプリケーション・サービスの公開API。
	/// </summary>
	public class CmWinServiceAPI : MarshalByRefObject
	{


		#region 内部変数

        public DBAccess m_db = null;//データベースアクセス対象
        private static NdnPublicFunction pf = new NdnPublicFunction();

        private const string TBL_USER = "用户";//用户

        private const string TBL_MAG = "期刊";//期刊数据
        private const string TBL_PUBLISH = "期刊发行";//期刊发行数据
        private const string TBL_FILE = "期刊文件";//期刊文件数据

        private const string TBL_SUSER = "S用户";//S用户
        private const string TBL_SMAG = "S期刊";//S期刊数据
        private const string TBL_SPUBLISH = "S期刊发行";//S期刊发行数据
        private const string VW_SPUBLISH = "期刊发行视图";//S期刊发行视图
        private const string TBL_SSUBSCRIPT = "S期刊订阅";//S期刊订阅数据
        private const string VW_SSUBSCRIPT = "期刊订阅视图";//S期刊订阅视图
        private const string VW_SMAGVIEWER = "期刊阅览视图";//S期刊阅览视图
        
        private const string TBL_MAIL_SERVER = "邮件服务器";//邮件服务器数据
        private const string TBL_SEND = "期刊送信";//期刊送信数据
        private const string TBL_HISTORY = "期刊送信历史";//期刊送信历史
        private const string VW_PUBLISH = "期刊发行视图";//期刊发行视图数据
        private const string VW_HISTORY = "期刊发行历史视图";//期刊发行历史视图数据

        private const string TBL_FTP_SERVER = "FTP服务器";//FTP服务器数据
        private const string TBL_FTP_UPLOAD = "FTP上传";//FTP服务器上传数据
        private const string TBL_FTP_HISTORY = "FTP上传历史";//FTP服务器上传数据
        private const string VW_FTP_UPLOAD = "FTP上传视图";//FTP服务器上传数据视图
        private const string VW_FTP_HISTORY = "FTP上传历史视图";//FTP服务器上传数据视图

        private const string TBL_CUSTOMER = "客户";//客户
        private const string TBL_RFIDCUSTOMER = "RFID客户";//RFID客户
        private const string TBL_INVILIDCUSTOMER = "无效客户";//无效客户
        private const string TBL_CUSTOMERDB = "客户数据库";//客户数据库
        private const string TBL_CUSTOMERS_SYNC = "客户同步";//客户数据库同步
        private const string TBL_CUSTOMERS_HISTORY = "客户同步历史";//客户数据库同步历史
        private const string VW_CUSTOMERS_SYNC = "客户同步视图";//客户数据库同步视图
        private const string VW_CUSTOMERS_HISTORY = "客户同步历史视图";//客户数据库同步历史视图

        private const string TBL_MEDIA = "媒体";//媒体
        private const string TBL_MEDIA_PUBLISH = "媒体发布";//媒体发行
        private const string TBL_MEDIA_HISTORY = "媒体发布历史";//媒体发行历史
        private const string VW_MEDIA_PUBLISH = "媒体发布视图";//媒体发行视图
        private const string VW_MEDIA_HISTORY = "媒体发布历史视图";//媒体发行历史视图

        private const string TBL_TOOLCONFIG = "工具设置";//工具设置

        private const string VW_STOCK = "VW_STOCK";//库存视图
        private const string TBL_PRODUCT = "item";//产品
        private const string TBL_STOCK_BANLANCE = "stock_balance";//库存
        private const string TBL_STOCK_IMEXP = "stock_imexp";//出库入库
        public string UserID = null;//登陆用户ID
        public string UserRight = null;//登陆用户权限
        public string Language = "";//用户语言
        public Hashtable UserRightTable = new Hashtable();//用户语言变换用
        #endregion

		#region インスタンス作成・消滅

		/// <summary>
		/// 
		/// </summary>
		public CmWinServiceAPI()
		{
			try
			{
                if (m_db == null)
                {
                    m_db = new DBAccess();
                }
			}
			catch (Exception e)
			{
                NCLogger.GetInstance().WriteExceptionLog(e);
			}
		}

		/// <summary>
        /// CmWinServiceAPI
		/// </summary>
		static CmWinServiceAPI()
		{
		}

        #endregion        

		#region 公開APIの入り口

        #region GetUser
        /// <summary>
        /// GetUser 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetUser(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
            }
            else if (UserRight == "普通用户")
            {
                where += " and UpperUserID='"+UserID+"'";
            }

            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_USER, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetUser
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetUser(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_USER, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetSUser
        /// <summary>
        /// GetSUser 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetSUser(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_SUSER, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetSUser
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetSUser(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_SUSER, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetMag
        /// <summary>
        /// GetMag 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetMag(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_MAG, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetMag
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetMag(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_MAG, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetSMag
        /// <summary>
        /// GetSMag 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetSMag(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_SMAG, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetSMag
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetSMag(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_SMAG, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetPublish
        /// <summary>
        /// GetPublish 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetPublish(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_PUBLISH, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetPublish
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetPublish(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_PUBLISH, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetSPublish
        /// <summary>
        /// GetSPublish 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetSPublish(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_SPUBLISH, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetSPublishVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetSPublishVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_SPUBLISH, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetSPublish
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetSPublish(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_SPUBLISH, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetSSubscript
        /// <summary>
        /// GetSSubscript 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetSSubscript(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_SSUBSCRIPT, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetSSubscriptVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetSSubscriptVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_SSUBSCRIPT, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetSMagViewerVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetSMagViewerVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_SMAGVIEWER, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetSSubscript
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetSSubscript(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_SSUBSCRIPT, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetFile
        /// <summary>
        /// GetFile 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetFile(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "服务器管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_FILE, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// SetFile
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetFile(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_FILE, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetMailServer
        /// <summary>
        /// GetMailSever
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetMailServer(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_MAIL_SERVER, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetMailServer
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetMailServer(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_MAIL_SERVER, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetSend
        /// <summary>
        /// GetSend 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetSend(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_SEND, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetPublishVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetPublishVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_PUBLISH, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetSend
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetSend(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_SEND, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetHistory
        /// <summary>
        /// GetSever
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetHistory(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_HISTORY, wheresql, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// GetHistoryVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetHistoryVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_HISTORY, wheresql, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetHistory
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetHistory(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_HISTORY, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetFTPServer
        /// <summary>
        /// GetFTPServer
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetFTPServer(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_FTP_SERVER, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetFTPServer
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetFTPServer(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_FTP_SERVER, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetFTPUpload
        /// <summary>
        /// GetFtpUpload 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetFTPUpload(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_FTP_UPLOAD, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetFTPUploadVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetFTPUploadVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_FTP_UPLOAD, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetFTPUpload
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetFTPUpload(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_FTP_UPLOAD, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetFTPHistory
        /// <summary>
        /// GetSever
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetFTPHistory(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_FTP_HISTORY, wheresql, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// GetFTPHistoryVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetFTPHistoryVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_FTP_HISTORY, wheresql, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetFTPHistory
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetFTPHistory(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_FTP_HISTORY, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetCustomer
        /// <summary>
        /// GetCustomer 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetCustomer(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_CUSTOMER, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetCustomer
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetCustomer(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_CUSTOMER, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion
        
        #region GetRFIDCustomer
        /// <summary>
        /// GetCustomer 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetRFIDCustomer(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_RFIDCUSTOMER, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetRFIDCustomer
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetRFIDCustomer(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_RFIDCUSTOMER, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetInvalidCustomer
        /// <summary>
        /// GetInvalidCustomer 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetInvalidCustomer(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_INVILIDCUSTOMER, wheresql, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetInvalidCustomer
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetInvalidCustomer(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_INVILIDCUSTOMER, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetCustomerDB
        /// <summary>
        /// GetCustomerDB 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetCustomerDB(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_CUSTOMERDB, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetCustomerDB
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetCustomerDB(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_CUSTOMERDB, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetCustomersSync
        /// <summary>
        /// GetCustomersSync 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetCustomersSync(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_CUSTOMERS_SYNC, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetCustomersSyncVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetCustomersSyncVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_CUSTOMERS_SYNC, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetCustomersSync
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetCustomersSync(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_CUSTOMERS_SYNC, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetCustomersHistory
        /// <summary>
        /// GetCustomersHistory 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetCustomersHistory(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_CUSTOMERS_HISTORY, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetCustomersHistoryVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetCustomersHistoryVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_CUSTOMERS_HISTORY, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetCustomerHistory
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetCustomerHistory(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_CUSTOMERS_HISTORY, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetMedia
        /// <summary>
        /// GetMedia 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetMedia(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_MEDIA, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetMedia
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetMedia(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_MEDIA, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetMediaPublish
        /// <summary>
        /// GetMediaPublish 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetMediaPublish(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_MEDIA_PUBLISH, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetMediaPublishVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetMediaPublishVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_MEDIA_PUBLISH, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetMediaPublish
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetMediaPublish(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_MEDIA_PUBLISH, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetMediaHistory
        /// <summary>
        /// GetMediaHistory 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetMediaHistory(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_MEDIA_HISTORY, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetMediaHistoryVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetMediaHistoryVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            if (UserRight == "超级管理员")
            {
            }
            else if (UserRight == "客户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "期刊管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "用户管理员")
            {
                where += " and 1!=1";
            }
            else if (UserRight == "普通用户")
            {
                where += " and UserID='" + UserID + "'";
            }
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_MEDIA_HISTORY, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetMediaHistory
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetMediaHistory(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_MEDIA_HISTORY, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetToolConfig
        /// <summary>
        /// GetToolConfig 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetToolConfig(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + UserID + "'";
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_TOOLCONFIG, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetToolConfig
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetToolConfig(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_TOOLCONFIG, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetProduct
        /// <summary>
        /// GetStocVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetStocVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + UserID + "'";
            return m_db.GetDataList_(LoginID, dataid, fieldlist, VW_STOCK, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetProduct 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetProduct(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + UserID + "'";
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_PRODUCT, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetProduct
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetProduct(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_PRODUCT, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetStockBanlance
        /// <summary>
        /// GetWork 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetStockBanlance(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + UserID + "'";
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_STOCK_BANLANCE, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetStockBanlance
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetStockBanlance(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_STOCK_BANLANCE, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetStockImexp
        /// <summary>
        /// GetWorkSend 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetStockImexp(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + UserID + "'";
            return m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_STOCK_IMEXP, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetStockImexp
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetStockImexp(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return m_db.SetData_(LoginID, dataid, fieldlist, TBL_STOCK_IMEXP, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #endregion
    }
}