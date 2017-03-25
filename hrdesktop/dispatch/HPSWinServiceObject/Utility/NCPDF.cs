using System;
using System.Collections.Generic;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;

namespace NC.HPS.Lib
{
    public class NCPDF
    {

        /// <summary>
        /// Get Infor to PDF
        /// </summary>
        /// <param name="srcPdfPath"></param>
        /// <param name="desPdfPath"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool GetInfor(string srcPdfPath, ref IDictionary<string, string> info)
        {
            PdfReader reader = new PdfReader(srcPdfPath);
            info = reader.Info;
           return true;
        }
        /// <summary>
        /// Add Infor to PDF
        /// </summary>
        /// <param name="srcPdfPath"></param>
        /// <param name="desPdfPath"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool AddInfor(string srcPdfPath,string desPdfPath, IDictionary<string,string> info)
        {
            PdfReader reader = new PdfReader(srcPdfPath); 
            // PdfStamperを作成。
            PdfStamper stamper = new PdfStamper(reader, new FileStream(desPdfPath, FileMode.Create));
            // 一時格納用のハッシュテーブル。
            //Hashtable info = new Hashtable();
            // ハッシュに値を設定する
            //info["Title"] = "超極秘文書　マジェスティック１２";
            //info["Author"] = "少年密偵　禿田正太郎";
            // Stamperにハッシュテーブルの内容を入れて
            stamper.MoreInfo = info;
            // Stamperを閉じればア～ラ不思議ファイルの出来上がり
            stamper.FormFlattening = true;
            stamper.Close();
            reader.Close();
            return true;
        }
        /// <summary>
        /// 文件加密
        /// </summary>
        /// <param name="strSrcPdfPath"></param>
        /// <param name="strDstPdfPath"></param>
        /// <param name="strPasswd"></param>
        /// <returns></returns>
        public static bool DecriptPdfDoc(
        string strSrcPdfPath, //ソースファイル 
        string strDstPdfPath, //変換後の保存先
        string strPasswd) //現在のownerパス　
        {
            PdfReader reader;
            int intPageNum;
            int i = 0;
            bool ret = false;
            byte[] bytOwnPass = Encoding.UTF8.GetBytes(strPasswd);
            try
            {
                string sReadPassword = "";//新しいuserパス
                string sWritePassword = strPasswd;//新しいownerパス
                reader = new PdfReader(strSrcPdfPath, bytOwnPass);
                intPageNum = reader.NumberOfPages;
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(strDstPdfPath, FileMode.Create));
                writer.Open();
                writer.SetEncryption(PdfWriter.STRENGTH40BITS, sReadPassword, sWritePassword, PdfWriter.AllowCopy | PdfWriter.AllowPrinting);
                document.Open();
                PdfContentByte cb = writer.DirectContent;
                while (i < intPageNum)
                {
                    document.NewPage();
                    PdfImportedPage page1 = writer.GetImportedPage(reader, ++i);
                    cb.AddTemplate(page1, 1f, 0, 0, 1f, 0, 0);
                }
                document.Close();
                reader.Close();
                writer.Close();
                ret = true;
            }
            catch (Exception de)
            {
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 分割PDF文件
        /// </summary>
        /// <param name="sourcePdfPath"></param>
        /// <param name="outputPdfPath"></param>
        /// <param name="startPage"></param>
        /// <param name="endPage"></param>
        public static Boolean ExtractPages(string sourcePdfPath, string outputPdfPath,
            int startPage, int endPage)
        {
            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;
            try
            {
                // Intialize a new PdfReader instance with the contents of the source Pdf file:
                reader = new PdfReader(sourcePdfPath);

                // For simplicity, I am assuming all the pages share the same size
                // and rotation as the first page:
                sourceDocument = new Document(reader.GetPageSizeWithRotation(startPage));

                // Initialize an instance of the PdfCopyClass with the source 
                // document and an output file stream:
                pdfCopyProvider = new PdfCopy(sourceDocument,
                    new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

                sourceDocument.Open();

                // Walk the specified range and add the page copies to the output file:
                for (int i = startPage; i <= endPage; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }
                sourceDocument.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 合并PDF文件
        /// </summary>
        /// <param name="destinationFile"></param>
        /// <param name="sourceFiles"></param>
        /// <returns></returns>
        public static Boolean MergeFiles(string destinationFile, string[] sourceFiles)
        {

            try
            {
                int f = 0;
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(sourceFiles[f]);
                // we retrieve the total number of pages
                int n = reader.NumberOfPages;
                //Console.WriteLine("There are " + n + " pages in the original file.");
                // step 1: creation of a document-object
                Document document = new Document(reader.GetPageSizeWithRotation(1));
                // step 2: we create a writer that listens to the document
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create));
                // step 3: we open the document
                document.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;
                int rotation;
                // step 4: we add content
                while (f < sourceFiles.Length)
                {
                    int i = 0;
                    while (i < n)
                    {
                        i++;
                        document.SetPageSize(reader.GetPageSizeWithRotation(i));
                        document.NewPage();
                        page = writer.GetImportedPage(reader, i);
                        rotation = reader.GetPageRotation(i);
                        if (rotation == 90 || rotation == 270)
                        {
                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                        }
                        else
                        {
                            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                        }
                        //Console.WriteLine("Processed page " + i);
                    }
                    f++;
                    if (f < sourceFiles.Length)
                    {
                        reader = new PdfReader(sourceFiles[f]);
                        // we retrieve the total number of pages
                        n = reader.NumberOfPages;
                        //Console.WriteLine("There are " + n + " pages in the original file.");
                    }
                }
                // step 5: we close the document
                document.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 取得PDF页数
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static int CountPageNo(string strFileName)
        {
            try
            {
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(strFileName);
                // we retrieve the total number of pages
                return reader.NumberOfPages;
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
            }
            return 0;
        }
    }
}
