using System;
using System.Collections.Generic;
using System.Text;
using OFFICECORE = Microsoft.Office.Core;
using WINWORD =   Microsoft.Office.Interop.Word;
using System.IO;
namespace NC.HPS.Lib
{

    public class NCWord
    {
        /// <summary>
        /// 转换word为pdf
        /// </summary>
        /// <param name="filename">doc文件路径</param>
        /// <param name="savefilename">pdf保存路径</param>
        public static void ConvertWordPDF(object filename, object savefilename)
        {
            Object Nothing = System.Reflection.Missing.Value;
            //创建一个名为WordApp的组件对象
            WINWORD.Application wordApp = new WINWORD.Application();
            WINWORD.Document doc = null;
            try
            {
                //创建一个名为WordDoc的文档对象并打开
                doc = wordApp.Documents.Open(ref filename, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //设置保存的格式
                object filefarmat = WINWORD.WdSaveFormat.wdFormatPDF;
                //保存为PDF
                doc.SaveAs(ref savefilename, ref filefarmat, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            }
            finally
            {

                try
                {
                    //关闭文档对象
                    doc.Close(ref Nothing, ref Nothing, ref Nothing);
                }
                catch
                {
                }
                try
                {
                    //推出组建
                    wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
                }
                catch
                {
                }
                GC.Collect();
            }
        }
    }
}