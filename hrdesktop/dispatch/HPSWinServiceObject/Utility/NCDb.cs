using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace NC.HPS.Lib
{
    /// <summary>
    /// 客户数据同期
    /// </summary>
    public class NCDb
    {
        /// <summary>
        /// 取得DB数据
        /// </summary>
        /// <param name="DBType">数据库类型</param>
        /// <param name="DBServer">数据库服务器地址</param>
        /// <param name="DBUser">用户</param>
        /// <param name="DBPassword">密码</param>
        /// <param name="DBName">数据库名</param>
        /// <param name="DBTableName">表名</param>
        /// <param name="fields">字段：第一个为邮件地址</param>
        /// <param name="ds">返回数据</param>
        /// <returns>是否成功</returns>
        public static Boolean GetAllDBData(String DBType,String DBServer,String DBUser,String DBPassword,String DBName,String DBTableName,String[] fields,ref DataSet ds)
        {
            switch (DBType)
            {
                case "OUTLOOK":
                    return getMailAddressFromOUTLOOK(fields, ref ds);
                case "MYSQL":
                    return getUserFromMySql(DBServer,DBUser,DBPassword,DBName,DBTableName,fields,ref ds);
                case "SQLSERVER":
                    return getUserFromSqlserver(DBServer, DBUser, DBPassword, DBName, DBTableName, fields, ref ds);
            }
            return false;
        }
        /// <summary>
        /// 从OutLook中取出用户
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static Boolean getMailAddressFromOUTLOOK(string[] fields,ref DataSet ds)
        {
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                //Get the MAPI namespace. 
                Outlook.NameSpace oNS = oApp.Session; 
                //Get the AddressLists collection. 
                Outlook.AddressLists oALs = oNS.AddressLists; 
                //Loop through the AddressLists collection. 
                Outlook.AddressList oAL; 
                for(int i=1;i<=oALs.Count;i++){
                    oAL = (Outlook.AddressList)oALs.GetEnumerator().Current;
                    //
                    oALs.GetEnumerator().MoveNext();
                } 

            }//end of try block
            catch (System.Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                return false;
                //MessageBox.Show("outllok " + ex.Message);
            }//end of catch
            return true;
        }//end of Email Method 
 
        /// <summary>
        /// 从SQLServer取得用户
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="uid">用户</param>
        /// <param name="pwd">密码</param>
        /// <param name="database">数据库</param>
        /// <param name="tablename">表</param>
        /// <param name="fields">字段名</param>
        /// <param name="mails">已有邮件</param>
        /// <param name="ds">返回数据</param>
        /// <returns></returns>
        public static Boolean getUserFromSqlserver(string server, string uid, string pwd, string database, string tablename, string[] fields, ref DataSet ds)
        {
            string connStr = String.Format("Data Source={0};Initial Catalog={3};User ID={1};Password={2}",
                server, uid, pwd, database);
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                conn.Open();
                string sql = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[t_mail]') AND type in (N'U'))
                                BEGIN CREATE TABLE t_mail (mail varchar(100)) END";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "select " + string.Join(",", fields) + " from " + tablename
                    + " where " + fields[0] + " not in (select mail from t_mail)";
                DataTable data = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    string mail = row[fields[0]].ToString();
                    sql = "INSERT INTO t_mail VALUES ('" + mail + "')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                ds.Tables.Clear(); ;
                ds.Tables.Add(data);
                conn.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 从MySQL取得用户
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="uid">用户</param>
        /// <param name="pwd">密码</param>
        /// <param name="database">数据库</param>
        /// <param name="tablename">表</param>
        /// <param name="fields">字段名</param>
        /// <param name="mails">已有邮件</param>
        /// <param name="ds">返回数据</param>
        /// <returns></returns>
        public static Boolean getUserFromMySql(string server,string uid,string pwd,string database,string tablename,string[] fields,ref DataSet ds)
        {
            string connStr = String.Format("server={0};uid={1};pwd={2};database={3}",
                server, uid, pwd, database);
            MySqlConnection conn = new MySqlConnection(connStr);
			try 
			{
				conn.Open();
                string sql = "CREATE TABLE t_mail (mail varchar(100)) IF not EXISTS t_mail";
				MySqlCommand cmd = new MySqlCommand(sql, conn);
				cmd.ExecuteNonQuery();

                sql = "select " + string.Join(",", fields) + " from " + tablename 
                    + " where "+fields[0]+" not in (select mail from t_mail)";
                DataTable data = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
                da.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    string mail = row[fields[0]].ToString();
                    sql = "INSERT INTO t_mail VALUES ('" + mail + "')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                ds.Tables.Clear(); ;
                ds.Tables.Add(data);
                conn.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                return false;
            }
            return true;
        }
    }
}
