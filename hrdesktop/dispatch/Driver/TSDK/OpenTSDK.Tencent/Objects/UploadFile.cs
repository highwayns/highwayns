/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  UploadFile
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Security;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadFile
    {
        /// <summary>
        /// 根据本地文件地址实例化
        /// </summary>
        /// <param name="filePath">本地文件地址，绝对地址</param>
        public UploadFile(string filePath)
        {
            this.FilePath = filePath;
            this.FileName = Path.GetFileName(filePath);
            this.ContentType = GetContentType(Path.GetExtension(filePath));
        }
        /// <summary>
        /// 根据本地文件地址与文件类型实例化
        /// </summary>
        /// <param name="filePath">本地文件地址，绝对地址</param>
        /// <param name="contentType">文件类型</param>
        public UploadFile(string filePath, string contentType)
        {
            this.FilePath = filePath;
            this.FileName = Path.GetFileName(filePath);
            this.ContentType = contentType;
        }
        /// <summary>
        /// 根据文件名、文件类型与文件流实现化
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="contentType">文件类型</param>
        /// <param name="stream">文件数据流</param>
        public UploadFile(string fileName, string contentType, Stream stream)
        {
            this.FileName = fileName;
            this.ContentType = contentType;
            this.FileStream = stream;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// 本地文件路径
        /// </summary>
        private string FilePath;
        /// <summary>
        /// 文件数据流
        /// </summary>
        private Stream FileStream;

        /// <summary>
        /// 将当前的文件数据写入到某个数据流中
        /// </summary>
        /// <param name="stream"></param>
        public void WriteTo(Stream stream)
        {
            byte[] buffer = new byte[512];
            int size = 0;
            if (!string.IsNullOrEmpty(this.FilePath)
                && File.Exists(this.FilePath))
            {
                //写入本地文件流
                using (System.IO.FileStream reader = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
                {
                    while ((size = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, size);
                    }
                }
            }
            if (this.FileStream != null)
            {
                //写入文件流
                while ((size = this.FileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, size);
                }
            }
        }

        /// <summary>
        /// 根据文件扩展名获取文件类型
        /// </summary>
        /// <param name="fileExt">文件扩展名</param>
        /// <returns></returns>
        private string GetContentType(string fileExt)
        {
            string contentType = "application/octetstream";
            if (!string.IsNullOrEmpty(fileExt))
            {
                fileExt = fileExt.ToLower();

                //尝试从注册表中取值
                bool hasError = false;
                try
                {
                    RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(fileExt);
                    if (regKey != null)
                    {
                        object v = regKey.GetValue("Content Type");
                        if (v != null) contentType = v.ToString();
                    }
                }
                catch (SecurityException)
                {
                    hasError = true;
                }
                catch (UnauthorizedAccessException)
                {
                    hasError = true;
                }

                if (hasError || string.IsNullOrEmpty(contentType))
                {
                    //处理常用图片文件的ContentType
                    contentType = GetCommonFileContentType(fileExt);
                }
            }

            return contentType;
        }
        /// <summary>
        /// 获取通用文件的文件类型
        /// </summary>
        /// <param name="fileExt">文件扩展名.如".jpg",".gif"等</param>
        /// <returns></returns>
        private string GetCommonFileContentType(string fileExt)
        {
            switch (fileExt)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                case ".png":
                    return "image/png";
                default:
                    return "application/octetstream";
            }
        }
    }
}
