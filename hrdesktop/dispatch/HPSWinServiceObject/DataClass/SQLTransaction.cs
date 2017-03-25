namespace NC.HPS.Lib
{

	using System;
	using System.Data.SqlClient;


	//************************************************************************
	/// <summary>
	/// DBトランザクション処理クラス
	/// </summary>
	//************************************************************************
	public class SQLTransaction
	{

		#region フィールド

		/// <summary>
		/// データベースのトランザクション
		/// </summary>
		private SqlTransaction s_transaction = null;
		private SqlConnection s_conn = null;
		#endregion

		/// <summary>
		/// データベースのトランザクション
		/// </summary>
		public SqlTransaction Transaction
		{
			get {return s_transaction;}
		}

		public SQLTransaction(SqlConnection connection)
		{
			s_conn = connection;
		}
		#region public メソッド

		//************************************************************************
		/// <summary>
		/// データベースでトランザクションを開始する
		/// </summary>
		/// <returns>正常の場合はtrueを返す、それ以外の場合はfalse。</returns>
		//************************************************************************
		public  bool Begin()
		{
			bool bReturn = false;
			try
			{
				if (s_conn != null)
				{
					if (s_transaction == null)
					{
						s_transaction = s_conn.BeginTransaction();

						NCLogger.GetInstance().WriteDebugLog("Transaction is started.");
					}					
					bReturn = true;
				}
			}
			catch (Exception exp)
			{
				NCLogger.GetInstance().WriteExceptionLog(exp);
			}
			return bReturn;
		}

		//************************************************************************
		/// <summary>
		/// トランザクションを保留中の状態からコミットする
		/// </summary>
		/// <returns>正常の場合はtrueを返す、それ以外の場合はfalse。</returns>
		//************************************************************************
		public  bool Commit()
		{
			bool bReturn = false;
			try
			{
				if (s_transaction != null)
				{
					s_transaction.Commit();

					NCLogger.GetInstance().WriteDebugLog("Transaction is commited.");
					bReturn = true;
				}
			}
			catch (Exception exp)
			{
				NCLogger.GetInstance().WriteExceptionLog(exp);
			}
			finally
			{
				s_transaction = null;
			}
			return bReturn;
		}

		//************************************************************************
		/// <summary>
		/// トランザクションを保留中の状態からロールバックする
		/// </summary>
		/// <returns>正常の場合はtrueを返す、それ以外の場合はfalse。</returns>
		//************************************************************************
		public  bool Rollback()
		{
			bool bReturn = false;
			try
			{
				if (s_transaction != null)
				{
					s_transaction.Rollback();
					NCLogger.GetInstance().WriteDebugLog("Transaction is rollback.");
					bReturn = true;
				}
			}
			catch (Exception exp)
			{
				NCLogger.GetInstance().WriteExceptionLog(exp);
			}
			finally
			{
				s_transaction = null;
			}

			return bReturn;
		}

		//************************************************************************
		/// <summary>
		/// トランザクションが起動するかどうかを判断します。
		/// </summary>
		/// <returns>起動された場合はtrueを返す、それ以外の場合はfalse。</returns>
		//************************************************************************
		public  bool IsBegin()
		{						
			if (s_transaction != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  object GetCurrentTransaction()
        {
            if (s_transaction != null)
            {
                return s_transaction;
            }
            else
            {
                return null;
            }
        }
	    
		#endregion

	}
}
