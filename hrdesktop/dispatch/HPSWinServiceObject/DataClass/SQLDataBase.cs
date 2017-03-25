using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace NC.HPS.Lib
{

	
	
	//************************************************************************
	/// <summary>
	/// NdnDataBaseクラス：データベースをアクセスする
	/// </summary>
	//************************************************************************
	public class SQLDataBase //:IDataBase
	{						

		#region private メソッド
		//************************************************************************
		/// <summary>
		/// DBアクセスエラーが発生する時、例外事件を処理する
		/// </summary>
		/// <param name="exp">DBアクセス例外</param>
		//************************************************************************
		private void ThrowSqlException(SqlException exp)
		{
			if (exp.Number == NCConst.Sql2627)       // 一意制約違反
			{
                throw new Exception("CM0400W"); 
			}
			else if (exp.Number == NCConst.Sql2702) // サービス名が存在しない
			{
				throw new Exception("CM0410W");
			}
			else                                   // その他エラー
			{
//				throw new NdnException("CM0900E");
				throw new Exception(exp.Message, exp);
			}
		}
		#endregion
	    
	    

		#region public メソッド

		#region プロシージャを実行して、オブジェクトを返す
		//************************************************************************
		/// <summary>
		/// プロシージャを実行して、オブジェクトを返す
		/// </summary>
		/// <param name="strSPName">プロシージャの名称</param>
		/// <param name="list">DBアクセスパラメータリスト</param>
		/// <param name="objReturn">戻るのオブジェクト</param>
		/// <returns>正常の場合はtrueを返す、それ以外の場合はfalse。</returns>
		//************************************************************************
        public bool ExecSp(string strSPName, ArrayList list, ref object objReturn,string ConnStr)
		{
			bool bReturn = false;
            SqlConnection conn = (SqlConnection)SQLDataConnection.GetConnection(ConnStr);
            SqlCommand dbCommand = new SqlCommand(strSPName, conn);
            SQLTransaction trans = new SQLTransaction(conn);
            if (trans.IsBegin())
            {
                dbCommand.Transaction = (SqlTransaction)trans.GetCurrentTransaction();
            }
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandTimeout = 36000;
			SqlParameter SqlParam = null;
			try
			{
				if (list != null)
				{
					IEnumerator Enumerator;
					Enumerator = list.GetEnumerator();
					NcPara para;
					while(Enumerator.MoveNext())
					{
						para = (NcPara)Enumerator.Current;
						if (Equals(para.OutPut, true))
						{
                            SqlParam = dbCommand.Parameters.Add(GetSqlParameter(para.Key, (SqlDbType)para.Type, para.Size, para.Value, para.OutPut));					
						}
						else
						{
                            dbCommand.Parameters.Add(GetSqlParameter(para.Key, (SqlDbType)para.Type, para.Size, para.Value, para.OutPut));
						}
					}
				}
			
				dbCommand.ExecuteNonQuery();
				if (SqlParam != null && objReturn != null) objReturn = SqlParam.Value;
				bReturn = true;
			}
			catch( Exception err)
			{
				// NdnPublicFunction.WriteLog(m_strLogFileName, err.Message);
				NCLogger.GetInstance().WriteExceptionLog(err);
			}
			finally
			{   
				dbCommand.Parameters.Clear();
			}
			return bReturn;
		}
		#endregion


		#region プロシージャを実行して、データセットを返す
		//************************************************************************
		/// <summary>
		/// プロシージャを実行して、データセットを返す
		/// </summary>
		/// <param name="strSPName">プロシージャの名称</param>
		/// <param name="list">DBアクセスパラメータリスト</param>
		/// <param name="dataSet">戻るのデータセット</param>
		/// <param name="strTable">テーブルの名前</param>
		/// <returns>正常の場合はtrueを返す、それ以外の場合はfalse。</returns>
		//************************************************************************
        public bool ExecSp(string strSPName, ArrayList list, ref DataSet dataSet, string strTable,string ConnStr)
		{
			bool bReturn = false;
			if (dataSet == null) dataSet = new DataSet();
            SqlConnection conn = (SqlConnection)SQLDataConnection.GetConnection(ConnStr);
            SqlCommand dbCommand = new SqlCommand(strSPName, conn);
            SQLTransaction trans = new SQLTransaction(conn);
            if (trans.IsBegin())
            {
                dbCommand.Transaction = (SqlTransaction)trans.GetCurrentTransaction();
            }
            dbCommand.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter dbAdapter = null;
			try
			{
				if (list != null)
				{
					IEnumerator Enumerator;
					Enumerator = list.GetEnumerator();
					NcPara para;
					while(Enumerator.MoveNext())
					{
						para = (NcPara)Enumerator.Current;
                        dbCommand.Parameters.Add(GetSqlParameter(para.Key, (SqlDbType)para.Type, para.Size, para.Value, para.OutPut));
					}
				}
				//アダプターの構成
				dbAdapter = new SqlDataAdapter(dbCommand);
			
				dbAdapter.Fill(dataSet, strTable);
				bReturn = true;
			}
			catch( Exception err)
			{
				// NdnPublicFunction.WriteLog(m_strLogFileName, err.Message);
				NCLogger.GetInstance().WriteExceptionLog(err);
			}
			finally
			{   
				dbCommand.Parameters.Clear();
				dbAdapter.Dispose();
			}
			return bReturn;
		}	
		#endregion
        #region プロシジャーパラメータを取得する
        //************************************************************************
        /// <summary>
        /// プロシジャーパラメータを取得する
        /// </summary>
        /// <param name="strName">パラメータの名前</param>
        /// <param name="SqlType">パラメータのタイプ</param>
        /// <param name="iSize">パラメータのサイズ</param>
        /// <param name="objValue">パラメータの値</param>
        /// <param name="bOutput">パラメータの出力フラグ</param>
        /// <returns>プロシージャパラメータ</returns>
        //************************************************************************
        public SqlParameter GetSqlParameter(string strName, SqlDbType SqlType, int iSize, object objValue, bool bOutput)
        {
            SqlParameter parameter = new SqlParameter(strName, SqlType);

            if (iSize > 0)
            {
                parameter.Size = iSize;
            }
            //パラメーターは出力の場合、パラメーターの型を設定
            if (bOutput)
            {
                if (strName == null || strName == "")
                {
                    parameter.Direction = ParameterDirection.ReturnValue;
                }
                else
                {
                    parameter.Direction = ParameterDirection.Output;
                }
            }
            else
            {
                parameter.Value = objValue;
            }
            return parameter;
        }
        #endregion

        #region プロシジャーパラメータを取得する
        //************************************************************************
        /// <summary>
        /// プロシジャーパラメータを取得する
        /// </summary>
        /// <param name="strName">パラメータの名前</param>
        /// <param name="objValue">パラメータの値</param>
        /// <returns>プロシージャパラメータを戻る</returns>
        //************************************************************************
        public SqlParameter GetSqlParameter(string strName, object objValue)
        {
            SqlParameter parameter = new SqlParameter(strName, (SqlDbType)objValue);

            return parameter;
        }
        #endregion
		#endregion		
	}
}
