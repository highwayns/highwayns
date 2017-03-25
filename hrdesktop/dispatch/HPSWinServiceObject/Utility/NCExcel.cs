using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
namespace NC.HPS.Lib
{

    public class NCExcel
    {
        private Missing miss = Missing.Value; //忽略的参数OLENULL 
        public static Missing MissValue = Missing.Value;
        private Excel.Application m_objExcel;//Excel应用程序实例 
        private Excel.Workbooks m_objBooks;//工作表集合 
        private Excel.Workbook m_objBook;//当前操作的工作表 
        private Excel.Worksheet sheet;//当前操作的表格 
        private string AList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// ファイル開く
        /// </summary>
        /// <param name="filename"></param>
        public void OpenExcelFile(string filename)
        {
            try
            {
                m_objExcel = new Excel.Application();
                m_objExcel.Workbooks.Open(
                filename,
                miss,
                true,
                miss,
                miss,
                miss,
                miss,
                miss,
                miss,
                miss,
                miss,
                miss,
                miss,
                miss,
                miss);
                m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
                m_objBook = m_objExcel.ActiveWorkbook;
                sheet = (Excel.Worksheet)m_objBook.ActiveSheet;
            }
            catch
            {
                try
                {
                    m_objExcel.Quit();
                }
                catch
                {
                }
            }

        }
        /// <summary>
        /// ファイル作成
        /// </summary>
        public void CreateExceFile()
        {
            m_objExcel = new Excel.Application();
            m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
            m_objBook = (Excel.Workbook)(m_objBooks.Add(miss));
            sheet = (Excel.Worksheet)m_objBook.ActiveSheet;
        }
        /// <summary>
        /// ファイル名前付け保存
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveAs(string FileName)
        {
            m_objBook.SaveAs(FileName, miss, miss, miss, miss,
            miss, Excel.XlSaveAsAccessMode.xlNoChange,
            Excel.XlSaveConflictResolution.xlLocalSessionChanges,
            miss, miss, miss, miss);
            m_objBook.Close(false, miss, miss);
            m_objExcel.Quit();
        }
        /// <summary>
        /// PDFファイルに保存
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveAsPDF(string FileName)
        {
            try
            {
                Excel.XlFixedFormatType targetType = Excel.XlFixedFormatType.xlTypePDF;
                m_objBook.ExportAsFixedFormat(targetType, FileName, Excel.XlFixedFormatQuality.xlQualityStandard, true, true,
                    miss, miss, false, miss);
            }
            catch
            {
                try
                {

                    m_objBook.Close(false, miss, miss);
                }
                catch
                {
                }
                try
                {
                    m_objExcel.Quit();
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// ファイル保存
        /// </summary>
        public void Save()
        {
            m_objBook.Save();
            m_objBook.Close(false, miss, miss);
            m_objExcel.Quit();
        }
        /// <summary>
        /// シート増加
        /// </summary>
        public void AddSheet(int count)
        {
            for (int i = 3; i < count; i++)
            {
                ((Excel.Worksheet)m_objBook.Worksheets.get_Item(i)).Copy(miss, m_objBook.Worksheets[i]);
            }
            for (int i = 3; i <= count; i++)
            {
                ((Excel.Worksheet)m_objBook.Worksheets.get_Item(i)).Name = "Sheet" + Convert.ToString(i);
            }
        }
        /// <summary>
        /// シート選択
        /// </summary>
        /// <param name="index"></param>
        public void SelectSheet(int index)
        {
            sheet = ((Excel.Worksheet)m_objBook.Worksheets.get_Item(index));
        }
        public string getSheetName()
        {
            return sheet.Name;
        }
        /// <summary>
        /// 印刷
        /// </summary>
        public void Print()
        {
            m_objBook.PrintOut(miss, miss, miss, miss, miss, miss, miss, miss);
        }
        /// <summary>
        /// 閉じる
        /// </summary>
        public void Close()
        {
            try
            {
                m_objBook.Close(false, miss, miss);
            }
            catch
            {
            }
            m_objExcel.Quit();
            GC.Collect();
        }
        /// <summary>
        /// 値設定
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        public void setValue(int x, int y, string text)
        {
            Excel.Range range = sheet.get_Range(this.GetAix(x, y), miss);
            range.set_Value(MissValue, text);
        }
        /// <summary>
        /// 値取得
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        public string getValue(int x, int y)
        {
            Excel.Range range = sheet.get_Range(this.GetAix(x, y), miss);
            if (range.get_Value(MissValue) != null)
            {
                return range.get_Value(MissValue).ToString();
            }
            return "";
        }
        /// <summary>
        /// 値取得
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        public string getValue(int x1, int y1, int x2, int y2)
        {
            Excel.Range range = sheet.get_Range(this.GetAix(x1, y1), this.GetAix(x2, y2));
            if (range.get_Value(MissValue) != null)
            {
                return range.get_Value(MissValue).ToString();
            }
            return "";
        }

        /// <summary>
        /// 位置取得
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string GetAix(int x, int y)
        {
            char[] AChars = AList.ToCharArray();
            string s = "";
            if (x <= 26)
            {
                s = AChars[x - 1].ToString();
            }
            else
            {
                int n = x / 26;
                int l = x - n * 26;
                if (l == 0)
                {
                    l = 26;
                    n = n - 1;
                }
                s = AChars[n - 1].ToString() + AChars[l - 1].ToString();
            }

            s = s + y.ToString();
            return s;
        }
    }

}