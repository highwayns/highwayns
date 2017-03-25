/*
 システム  　：PCパトロールシステムCJW（バッチ部分）
 サブシステム：共通モジュール
 バージョン　：1.0.*
 著作権    　：ニューコン株式会社2007
 概要      　：データベースアクセス
 更新履歴  　：2007/08/20　　鄭軍　　新規
*/
using System;
using System.Collections.Generic;
using System.Text;
using NC.HPS.Lib;
using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace NC.HPS.Lib
{
    public class DBAccess
    {
        #region private 部分
        /// <summary>
        /// NdnDataBase
        /// </summary>
        private SQLDataBase m_dataBase = new SQLDataBase();
        #endregion

        #region public 部分
        /// <summary>
        /// GetDataList_ データまたはデータ一覧取得方法
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="tablename">テーブル名前</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="ordersql">ＯＲＤＥＲ文</param>
        /// <param name="dataSet">データセット</param>
        /// <returns>成功の場合ＴＵＲＥ</returns>
        public bool GetDataList_(int LoginID, int dataid, string fieldlist,
                                  string tablename, string wheresql, string ordersql,
                                  ref DataSet dataSet,string Connstr)
        {
            NCLogger.GetInstance().WriteDebugLog("GetDataList_");
            NCLogger.GetInstance().WriteDebugLog("tablename=" + tablename);
            NCLogger.GetInstance().WriteDebugLog("fieldlist=" + fieldlist);
            NCLogger.GetInstance().WriteDebugLog("wheresql=" + wheresql);
            NCLogger.GetInstance().WriteDebugLog("ordersql=" + ordersql);
            bool bReturn = false;

            try
            {
                ArrayList list = new ArrayList();
                NcPara para = new NcPara("@iOPID", SqlDbType.Int, 16, LoginID);
                list.Add(para);
                para = new NcPara("@iCIP", SqlDbType.Int, 16, 0);
                list.Add(para);
                para = new NcPara("@iDATAID", SqlDbType.Int, 16, dataid);
                list.Add(para);
                para = new NcPara("@iFLDL", SqlDbType.Char, 1000, fieldlist);
                list.Add(para);
                para = new NcPara("@iTBLNM", SqlDbType.Char, 30, tablename);
                list.Add(para);
                para = new NcPara("@iWSQL", SqlDbType.Char, 1000, wheresql);
                list.Add(para);
                para = new NcPara("@iOSQL", SqlDbType.Char, 100, ordersql);
                list.Add(para);
                para = new NcPara("@oRCD", SqlDbType.Int, 16, null, true);
                list.Add(para);
                // modified by zhengjun 20071114 for error no.5 start
                //bReturn = m_dataBase.ExecSp("dbo.GetDataList", list, ref dataSet, tablename, Connstr);
                bReturn = m_dataBase.ExecSp("GetDataList", list, ref dataSet, tablename, Connstr);
                // modified by zhengjun 20071114 for error no.5 end
            }
            catch (Exception exp)
            {
                NCLogger.GetInstance().WriteExceptionLog(exp);
            }
            return bReturn;
        }
        /// <summary>
        /// SetData_ データ作成または更新
        /// </summary>
        /// <param name="LoginID">ユーザＩＤ</param>
        /// <param name="dataid">データＩＤ</param>
        /// <param name="fieldlist">フィールドリスト</param>
        /// <param name="tablename">テーブル名</param>
        /// <param name="wheresql">ＷＨＥＲＥ文</param>
        /// <param name="valuesql">ＶＡＬＵＥ文</param>
        /// <param name="newdataid">新規するときのＩＤ</param>
        /// <returns>成功場合ＴＲＵＥ</returns>
        public bool SetData_(int LoginID, int dataid, string fieldlist,
                                  string tablename, string wheresql, string valuesql,
                                  out int newdataid,string Connstr)
        {
            NCLogger.GetInstance().WriteDebugLog("GetDataList_");
            NCLogger.GetInstance().WriteDebugLog("tablename=" + tablename);
            NCLogger.GetInstance().WriteDebugLog("wheresql=" + wheresql);
            NCLogger.GetInstance().WriteDebugLog("valuesql=" + valuesql);
            bool bReturn = false;
            newdataid = dataid;
            try
            {
                ArrayList list = new ArrayList();
                NcPara para = new NcPara("@iOPID", SqlDbType.Int, 16, LoginID);
                list.Add(para);
                para = new NcPara("@iCIP", SqlDbType.Int, 16, 0);
                list.Add(para);
                para = new NcPara("@iDATAID", SqlDbType.Int, 16, dataid);
                list.Add(para);
                para = new NcPara("@iFLDL", SqlDbType.Char, 1000, fieldlist);
                list.Add(para);
                para = new NcPara("@iTBLNM", SqlDbType.Char, 30, tablename);
                list.Add(para);
                para = new NcPara("@iWSQL", SqlDbType.Char, 7000, wheresql);
                list.Add(para);
                para = new NcPara("@iVSQL", SqlDbType.Char, 7000, valuesql);
                list.Add(para);
                para = new NcPara("@oRCD", SqlDbType.Int, 16, newdataid, true);
                list.Add(para);
                object refobj = new object();
                // modified by zhengjun 20071114 for error no.5 start
                //bReturn = m_dataBase.ExecSp("dbo.SetData", list, ref refobj, Connstr);
                bReturn = m_dataBase.ExecSp("SetData", list, ref refobj, Connstr);
                // modified by zhengjun 20071114 for error no.5 end
                if (bReturn)
                {
                    newdataid = int.Parse(refobj.ToString());
                }
            }
            catch (Exception exp)
            {
                NCLogger.GetInstance().WriteExceptionLog(exp);
            }
            return bReturn;
        }
        #endregion
    }
}
