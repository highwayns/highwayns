using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NC.HPS.Lib;
using System.Data;

namespace HPSKANJI
{
    public class DB
    {
        private const string TBL_CLASS = "tbl_Class";//课程类别
        private const string TBL_LESSON = "tbl_Lesson";//课程
        private const string TBL_CONTENT = "tbl_Content";//课程内容

        public CmWinServiceAPI db;
        public DB()
        {
        }

        public DB(CmWinServiceAPI db)
        {
            this.db = db;
        }
        #region GetClass
        /// <summary>
        /// GetClass 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetClass(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + db.UserID + "'";
            return db.m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_CLASS, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetCLASS
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetClass(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return db.m_db.SetData_(LoginID, dataid, fieldlist, TBL_CLASS, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetLesson
        /// <summary>
        /// GetLesson 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetLesson(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + db.UserID + "'";
            return db.m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_LESSON, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetLesson
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetLesson(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return db.m_db.SetData_(LoginID, dataid, fieldlist, TBL_LESSON, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

        #region GetContent
        /// <summary>
        /// GetContent 
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetContent(int LoginID, int dataid, string fieldlist,
                                  string wheresql, string ordersql,
                                  ref DataSet dataSet)
        {
            string where = wheresql;
            if (where == "") where = "1=1";
            where += " and UserID='" + db.UserID + "'";
            return db.m_db.GetDataList_(LoginID, dataid, fieldlist, TBL_CONTENT, where, ordersql, ref dataSet, NCConst.ConnectionString);
        }

        /// <summary>
        /// SetContent
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetContent(int LoginID, int dataid, string fieldlist,
                                   string wheresql, string valuesql,
                                  out int newdataid)
        {
            return db.m_db.SetData_(LoginID, dataid, fieldlist, TBL_CONTENT, wheresql, valuesql, out newdataid, NCConst.ConnectionString);
        }
        #endregion

    }
}
