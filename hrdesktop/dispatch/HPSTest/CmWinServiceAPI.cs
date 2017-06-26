/*
 システム  　：ヒューマンエラー改善支援システム
 サブシステム：共通モジュール
 バージョン　：1.0.*
 著作権    　：ニューコン株式会社2007
 概要      　：サービス対象
 更新履歴  　：2007/12/25　　鄭軍　　新規
*/
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Security.Permissions;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.IO;
using System.Data.OleDb;
using NC.HPS.Lib;

namespace NC.HEDAS.Lib
{
	/// <summary>
	/// アプリケーション・サービスの公開API。
	/// </summary>
	public class CmWinServiceAPI : MarshalByRefObject
	{


		#region 内部変数

        private static OleDbConnection Connection;

        #endregion

		#region インスタンス作成・消滅

		/// <summary>
		/// 初期化
		/// </summary>
		public CmWinServiceAPI()
		{
			try
			{
                string constr=null;
                NdnXmlConfig xmlConfig;
                string appName = Path.GetFileNameWithoutExtension( System.Windows.Forms.Application.ExecutablePath);
                xmlConfig = new NdnXmlConfig(string.Format(NCConst.CONFIG_FILE_DIR, appName) + NCUtility.GetAppConfig());

                if (!xmlConfig.ReadXmlData("database", "ConnectionString", ref constr))
                {
                    string msg = string.Format(NCMessage.GetInstance("").GetMessageById("CM0440E",""), "サンプル間隙（秒）");
                    NCLogger.GetInstance().WriteErrorLog(msg);
                }

                Connection = new OleDbConnection(constr);
                Connection.Open();
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

        /// <summary>
        /// Insert、UpdateなどSQL文実行
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public bool ExeSQL(string strSQL)
        {
            bool resultState = false;

            //Connection.Open();
            //if (Connection.State != ConnectionState.Open) Connection.Open();
            OleDbTransaction myTrans = Connection.BeginTransaction();
            OleDbCommand command = new OleDbCommand(strSQL, Connection, myTrans);

            try
            {
                command.ExecuteNonQuery();
                myTrans.Commit();
                resultState = true;
            }
            catch(Exception ex)
            {
                myTrans.Rollback();
                resultState = false;
            }
            finally
            {
                //Connection.Close();
            }
            return resultState;
        }
        /// <summary>
        /// データ検索
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public DataSet ReturnDataSet(string strSQL)
        {
            //Connection.Open();
            //if (Connection.State != ConnectionState.Open) Connection.Open();
            DataSet dataSet = new DataSet();
            OleDbDataAdapter OleDbDA = new OleDbDataAdapter(strSQL, Connection);
            OleDbDA.Fill(dataSet, "objDataSet");

            //Connection.Close();
            return dataSet;
        }

        #endregion

    }
}