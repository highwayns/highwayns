using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NC.HPS.Lib;
using System.Data;

namespace HPSConsultant
{
    public class DB
    {
        private const string TBL_COMPANY = "会社";//会社

        public CmWinServiceAPI db;
        public DB()
        {
        }

        public DB(CmWinServiceAPI db)
        {
            this.db = db;
        }
        #region GetCompany
        /// <summary>
        /// GetCompany 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetCompany(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            return db.m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_COMPANY, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetCompany
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetCompany(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return db.m_db.SetData_(LoginID, dataid, fieldlist, TBL_COMPANY, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

    }
}
