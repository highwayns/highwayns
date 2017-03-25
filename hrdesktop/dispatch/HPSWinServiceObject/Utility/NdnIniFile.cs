using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace NC.HPS.Lib
{
    //************************************************************************
    /// <summary>
    /// NdnIniFile の概要の説明です。
    /// </summary>
    //************************************************************************	
    public class NdnIniFile
    {
        private string m_strPath; //ini file path

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
                                                          int size, string filePath);

        #region ログのパラメーターを取得します

        //************************************************************************
        /// <summary>
        /// Ekk.iniファイルパスを取得します
        /// </summary>
        /// <param name="strIniPath">string Ekk.iniファイルパス</param>
        //************************************************************************
        private void GetIniPath(ref string strIniPath)
        {
            try
            {
                NdnXmlConfig xmlConfig = null;
                xmlConfig = new NdnXmlConfig(NCUtility.GetAppConfig(), NCConst.CONFIG_FILE_DIR);
                xmlConfig.ReadXmlData("log", "IniPath", ref strIniPath);
                if (strIniPath == "")
                {
                    strIniPath = Directory.GetCurrentDirectory();
                }
            }
            catch
            {
                strIniPath = Directory.GetCurrentDirectory();
            }
        }

        #endregion

        //************************************************************************
        /// <summary>
        /// ファイル初期化
        /// </summary>
        /// <param name="strIniName">ファイル名</param>
        //************************************************************************
        public NdnIniFile(string strIniName)
        {
            bool isExsit = false;
            string strIniPath = "";
            GetIniPath(ref strIniPath);
            m_strPath = strIniPath + "\\" + strIniName;
            isExsit = File.Exists(m_strPath);
            if (!isExsit)
            {
                m_strPath = Directory.GetCurrentDirectory() + "\\" + strIniName;
                isExsit = File.Exists(m_strPath);
            }
        }

        //************************************************************************
        /// <summary>
        /// ライトバリュー初期化
        /// </summary>
        /// <param name="Section">セクション</param>
        /// <param name="Key">キー</param>
        /// <param name="Value">バリュー</param>
        //************************************************************************
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, m_strPath);
        }

        //************************************************************************
        /// <summary>
        /// リードバリュー初期化
        /// </summary>
        /// <param name="Section">セクション</param>
        /// <param name="Key">キー</param>
        /// <param name="def">def</param>
        //************************************************************************
        public string IniReadValue(string Section, string Key, string def)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, def, temp, 255, m_strPath);
            return temp.ToString();
        }
    }
}