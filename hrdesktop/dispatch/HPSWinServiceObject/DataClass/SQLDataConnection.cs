namespace NC.HPS.Lib
{
	using System;
	using System.Data.SqlClient;

	//************************************************************************
	/// <summary>
	/// NdnDataConnection:データベースコネクション処理クラス
	/// </summary>
	//************************************************************************
	public class SQLDataConnection//:AbsConnection
	{
		#region フィールド
		/// <summary>
		/// データベースのコネクション
		/// </summary>
		private static SqlConnection s_conn = null;
        private static string ConnectionStr = "";

		#endregion

		/// <summary>
		/// データベースコネクションの構造体
		/// </summary>
		public SQLDataConnection()
		{
			s_conn = null;
		}
        //************************************************************************
        /// <summary>
        /// データベースのコネクションを取得する。
        /// </summary>
        /// <returns>データベースのコネクション</returns>
        //************************************************************************
        public static object GetConnection(string ConnStr)
        {
            // 若しデータベースのコネクションは利用不可、開く 。
            if (s_conn == null || ConnectionStr != ConnStr)
            {
                if (s_conn != null) CloseConnection();
                ConnectionStr = ConnStr;
                s_conn = OpenConnection();
            }
            // データベースのコネクションを返す
            return s_conn;
        }


		#region データベースのコネクションを閉じる
		//************************************************************************
		/// <summary>
		/// データベースのコネクションを閉じる
		/// </summary>
		//************************************************************************
		public static void CloseConnection()
		{
            try
            {
                if (s_conn != null)
                {
                    //DBクローズ
                    s_conn.Close();

                    NCLogger.GetInstance().WriteDebugLog("Sql Connection is closed.");
                }
            }
            catch
            {
            }
			finally
			{
				//初期値を設定
				s_conn = null;
			}
		}
		#endregion

		#region 指定のデータベースのコネクションを開く
		//************************************************************************
		/// <summary>
		/// 指定のデータベースのコネクションを開く
		/// </summary>
		/// <returns>データベースのコネクションを返す</returns>
		//************************************************************************
		private static SqlConnection OpenConnection()
		{
			try
			{
				// データベース接続パラメータを取得				
				string m_strConnectionString = GetConnectionString();				
				s_conn = new SqlConnection(m_strConnectionString);
				// DBオープン
				s_conn.Open();
				NCLogger.GetInstance().WriteDebugLog("Sql Connection is opened.");
			}
			catch (SqlException oex)
			{
				s_conn = null;
				NCLogger.GetInstance().WriteExceptionLog(oex);
				throw new NdnException("CM0410W");
			}
			catch(Exception err)
			{								
				s_conn = null;
				NCLogger.GetInstance().WriteExceptionLog(err);	
				throw new NdnException("CM0410W");
			}
			return s_conn;
		}
        //************************************************************************
        /// <summary>
        /// データベースの接続に必要な情報を取得します
        /// </summary>
        /// <returns>データベースのConnectStringを返す</returns>
        //************************************************************************
        protected static string GetConnectionString()
        {
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            string strConnectionString = null;
            if (xmlConfig.ReadXmlData("database", ConnectionStr, ref strConnectionString))
            {
                string[] temp = strConnectionString.Split(';');
                if (temp.Length>0)
                {
                    for (int idx = 0; idx < temp.Length;idx++ )
                    {
                        if (temp[idx].IndexOf("Password=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Password=", "");
                            temp[idx] = "Password=" + NCCryp.Decrypto(temp[idx]);
                            break;
                        }
                    }
                }
                strConnectionString = String.Join(";", temp);
                return strConnectionString;
            }
            else
            {
                return null;
            }
        }
        #endregion
		
	}
}