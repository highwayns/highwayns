using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NC.HPS.Lib;
using System.Data;

namespace HPSBid
{
    public class DB
    {
        private const string VW_EMP = "VW_雇员";//雇员视图
        private const string TBL_EMP = "雇员";//雇员
        private const string TBL_WORK = "职历";//职历
        private const string TBL_SEND = "派遣";//派遣
        private const string TBL_SCHOOL = "学历";//学历
        private const string TBL_TRAIN = "资格";//资格

        public CmWinServiceAPI db;
        public DB()
        {
        }

        public DB(CmWinServiceAPI db)
        {
            this.db = db;
        }

        #region GetEmpVW
        /// <summary>
        /// GetEmpVW 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetEmpVW(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + db.UserID + "'";
            return db.m_db.GetDataList_(LoginID, dataid, fieldlist, VW_EMP, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }
        /// <summary>
        /// GetEmp 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetEmp(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + db.UserID + "'";
            return db.m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_EMP, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetEmp
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetEmp(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return db.m_db.SetData_(LoginID, dataid, fieldlist, TBL_EMP, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetWork
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
        public bool GetWork(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + db.UserID + "'";
            return db.m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_WORK, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetWork
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetWork(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return db.m_db.SetData_(LoginID, dataid, fieldlist, TBL_WORK, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetSchool
        /// <summary>
        /// GetSchool 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetSchool(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + db.UserID + "'";
            return db.m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_SCHOOL, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetSchool
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetSchool(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return db.m_db.SetData_(LoginID, dataid, fieldlist, TBL_SCHOOL, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetTrain
        /// <summary>
        /// GetTrain 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetTrain(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + db.UserID + "'";
            return db.m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_TRAIN, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetTrain
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetTrain(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return db.m_db.SetData_(LoginID, dataid, fieldlist, TBL_TRAIN, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetSend
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
        public bool GetSend(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + db.UserID + "'";
            return db.m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_SEND, where, ordersql, ref dataSet, NCConst.ConnectionString);
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
            return db.m_db.SetData_(LoginID, dataid, fieldlist, TBL_SEND, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion
    }
}
