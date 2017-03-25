using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NC.HPS.Lib
{
    //************************************************************************
    /// <summary>
    /// ユーティリティクラス。
    /// </summary>
    //************************************************************************
    public class NCUtility
    {
        #region フィールド

        /// <summary>
        /// ランダムオブジェクトの取得
        /// </summary>
        private static Random random = null;

        /// <summary>
        /// メッセージ取得の宣言
        /// </summary>
        //protected static NCMessage s_Message = NCMessage.GetInstance("");


        /// <summary>
        /// 半角
        /// </summary>
        private static char[] SINGLE_CHAR = {
                                                '~', '`', '!', '@', '#',
                                                '$', '%', '^', '&', '*',
                                                '(', ')', '_', '+', '|',
                                                '-', '=', '\\', '{', '}',
                                                '[', ']', ':', '"', ';',
                                                '\'', '<', '>', '?', ',',
                                                '.', '/',
                                                'ｱ', 'ｲ', 'ｳ', 'ｴ', 'ｵ',
                                                'ｶ', 'ｷ', 'ｸ', 'ｹ', 'ｺ',
                                                'ｻ', 'ｼ', 'ｽ', 'ｾ', 'ｿ',
                                                'ﾀ', 'ﾁ', 'ﾂ', 'ﾃ', 'ﾄ',
                                                'ﾅ', 'ﾆ', 'ﾇ', 'ﾈ', 'ﾉ',
                                                'ﾊ', 'ﾋ', 'ﾌ', 'ﾍ', 'ﾎ',
                                                'ﾏ', 'ﾐ', 'ﾑ', 'ﾒ', 'ﾓ',
                                                'ﾔ', 'ﾕ', 'ﾖ',
                                                'ﾗ', 'ﾘ', 'ﾙ', 'ﾚ', 'ﾛ',
                                                'ﾜ', 'ｦ',
                                                'ﾝ', 'ｯ',
                                                'ｬ', 'ｨ', 'ｭ', 'ｪ', 'ｮ',
                                                'ﾞ', 'ﾟ',
                                                'ｰ',
                                                'a', 'b', 'c', 'd', 'e',
                                                'f', 'g', 'h', 'i', 'j',
                                                'k', 'l', 'm', 'n', 'o',
                                                'p', 'q', 'r', 's', 't',
                                                'u', 'v', 'w', 'x', 'y',
                                                'z',
                                                'A', 'B', 'C', 'D', 'E',
                                                'F', 'G', 'H', 'I', 'J',
                                                'K', 'L', 'M', 'N', 'O',
                                                'P', 'Q', 'R', 'S', 'T',
                                                'U', 'V', 'W', 'X', 'Y',
                                                'Z',
                                                '1', '2', '3', '4', '5',
                                                '6', '7', '8', '9', '0',
                                                ' '
                                            };

        #endregion

        #region コンストラクタ

        //************************************************************************

        #endregion

        #region public メソッド


        //************************************************************************
        /// <summary>
        /// 乱数を生成する。
        /// </summary>
        /// <returns>生成した乱数をString型で返す。</returns>
        //************************************************************************
        public static string GetRandomNumber(int argCount)
        {
            if (random == null)
            {
                random = new Random();
            }
            int randomNumber = random.Next(0, 999999999);
            if (argCount <= 0)
            {
                argCount = 0;
            }
            if (argCount <= 9)
            {
                return randomNumber.ToString().Substring(0, argCount);
            }
            return AddZero(randomNumber, argCount);
        }

        //************************************************************************
        /// <summary>
        /// 対象の文字列が指定した文字数になるように文字を最後尾に追加する。
        /// argStringがnullの場合は、argCharがargNum文字数となった文字列が返却される。
        /// argNumの数がargStringの文字数よりも小さい場合はargStrigを短くして返却される。
        /// </summary>
        /// <param name="argString">変換の対象となる文字列。</param>
        /// <param name="argNum">何桁に変換するか指定。</param>
        /// <param name="argChar">追加したい文字。</param>
        /// <returns>argNumで指定した桁数に変換した文字列。</returns>
        //************************************************************************
        public static string AddString(string argString, int argNum, char argChar)
        {
            // nullチェックを入れる
            if ((argString == null) || (argString.CompareTo("") == 0))
            {
                return new string(argChar, argNum);
            }
            StringBuilder strBuild = new StringBuilder(argString);
            if (argNum - argString.Length <= 0)
            {
                // 指定の文字数よりも長い場合は削除で後ろを短くする。
                strBuild.Remove(argNum - 1, argString.Length - argNum);
            }
            else
            {
                // 指定した長さの文字列になるようargCharを追加する。
                strBuild.Append(argChar, argNum - argString.Length);
            }
            return strBuild.ToString();
        }

        //************************************************************************
        /// <summary>
        /// 対象の文字列が指定した文字数になるように文字を先頭に挿入する。
        /// argStringがnullの場合は、argCharがargNum文字数となった文字列が返却される。
        /// argNumの数がargStringの文字数よりも小さい場合はargStrigを短くして返却される。
        /// </summary>
        /// <param name="argString">変換の対象となる文字列。</param>
        /// <param name="argNum">何桁に変換するか指定。</param>
        /// <param name="argChar">挿入したい文字。</param>
        /// <returns>argNumで指定した桁数に変換した文字列。</returns>
        //************************************************************************
        public static string InsString(string argString, int argNum, char argChar)
        {
            // nullチェックを入れる
            if ((argString == null) || (argString.CompareTo("") == 0))
            {
                return new string(argChar, argNum);
            }
            StringBuilder strBuild = new StringBuilder(argString);
            if (argNum - argString.Length <= 0)
            {
                // 指定の文字数よりも長い場合は削除で前から短くする。
                strBuild.Remove(0, argString.Length - argNum);
            }
            else
            {
                for (int i = argString.Length; i < argNum; i++)
                {
                    // 指定した長さの文字列になるようargCharを追加する。
                    strBuild.Insert(0, argChar);
                }
            }
            return strBuild.ToString();
        }

        //************************************************************************
        /// <summary>
        /// 対象の文字列の後ろに半角スペースを追加して、指定した桁数の文字列に変換する。
        /// </summary>
        /// <param name="argString">変換の対象となる文字列。</param>
        /// <param name="argNum">何桁に変換するか指定。</param>
        /// <returns>argNumで指定した桁数に変換した文字列。</returns>
        //************************************************************************
        public static string AddSpace(string argString, int argNum)
        {
            return AddString(argString, argNum, ' ');
        }

        //************************************************************************
        /// <summary>
        /// 対象の数値を左から「0」で埋めて指定した桁数の文字列に変換する。
        /// </summary>
        /// <param name="argInt">変換の対象となる数値。</param>
        /// <param name="argNum">何桁に変換するか指定。</param>
        /// <returns>argNumで指定した桁数に変換した文字列。</returns>
        //************************************************************************
        public static string AddZero(int argInt, int argNum)
        {
            return InsString(argInt.ToString(), argNum, '0');
        }

        //************************************************************************
        /// <summary>
        /// 対象の数値を左から「0」で埋めて指定した桁数の文字列に変換する。
        /// </summary>
        /// <param name="argString">変換の対象となる数値。</param>
        /// <param name="argNum">何桁に変換するか指定。</param>
        /// <returns>argNumで指定した桁数に変換した文字列。</returns>
        //************************************************************************
        public static string AddZero(string argString, int argNum)
        {
            return InsString(argString, argNum, '0');
        }

        //************************************************************************
        /// <summary>
        /// 値が0～9を許可する。
        /// </summary>
        /// <param name="argCheckValue">チェックしたい値。</param>
        /// <returns>数値の場合 true 、数値以外の場合は false を返す。</returns>
        //************************************************************************
        public static bool IsNumeric(string argCheckValue)
        {
            // nullの場合はfalseを返す
            if ((argCheckValue == null) || (argCheckValue.CompareTo("") == 0))
            {
                return false;
            }
            if (!Regex.IsMatch(argCheckValue, "^-?[0-9]+$"))
            {
                return false;
            }
            return true;
        }

        //************************************************************************
        /// <summary>
        /// 値が全て半角英字であるかチェックする。（空白が含まれる場合は false を返す）
        /// </summary>
        /// <param name="argCheckValue">チェックしたい値。</param>
        /// <returns>値が全て半角英字の場合 true 、半角英字以外が含まれている場合 false を返す。</returns>
        //************************************************************************
        public static bool IsAlpha(string argCheckValue)
        {
            // nullの場合はfalseを返す
            if ((argCheckValue == null) || (argCheckValue.CompareTo("") == 0))
            {
                return false;
            }
            if (!Regex.IsMatch(argCheckValue, "^[a-zA-Z]+$"))
            {
                return false;
            }
            return true;
        }

        //************************************************************************
        /// <summary>
        /// 値が全て半角英数字であるかチェックする。（空白が含まれる場合は false を返す）
        /// </summary>
        /// <param name="argCheckValue">チェックしたい値。</param>
        /// <returns>値が全て半角英数字の場合 true 、半角英数字以外が含まれている場合 false を返す。</returns>
        //************************************************************************
        public static bool IsAlphaOrNumeric(string argCheckValue)
        {
            // nullの場合はfalseを返す
            if ((argCheckValue == null) || (argCheckValue.CompareTo("") == 0))
            {
                return false;
            }
            if (!Regex.IsMatch(argCheckValue, "^[a-zA-Z0-9]+$"))
            {
                return false;
            }
            return true;
        }

        //************************************************************************
        /// <summary>
        /// 値が全て全角文字であるかチェックする。
        /// </summary>
        /// <param name="argCheckValue">チェックしたい値。</param>
        /// <returns>値が全て全角文字の場合 true 、全角文字以外が含まれている場合 false を返す。</returns>
        //************************************************************************
        public static bool IsDoubleByte(string argCheckValue)
        {
            // nullの場合はfalseを返す
            if ((argCheckValue == null) || (argCheckValue.CompareTo("") == 0))
            {
                return false;
            }
            if (!Regex.IsMatch(argCheckValue, "^[^\x00-\x7F]+$"))
            {
                return false;
            }
            return true;
        }


        //************************************************************************
        /// <summary>
        /// システム日付を取得する。(yyyymmdd形式で返却)
        /// </summary>
        /// <returns></returns>
        //************************************************************************
        public static string GetSystemDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        //************************************************************************
        /// <summary>
        /// システム時間を取得する。(hhmmss形式で返却)
        /// </summary>
        /// <returns></returns>
        //************************************************************************
        public static string GetSystemTime()
        {
            return DateTime.Now.ToString("HHmmss");
        }

        //************************************************************************
        /// <summary>
        /// メッセージの ":xxx " の部分を文字列の配列で入れ替える。
        /// 変換できなかった部分は半角スペースに変換する。
        /// </summary>
        /// <param name="argMsg">元のメッセージ</param>
        /// <param name="argStr">変更するためのパラメータ</param>
        /// <returns>変更後のメッセージ</returns>
        //************************************************************************
        public static string ReplaceMessage(string argMsg, string[] argStr)
        {
            string dispMessage = argMsg;

            if (argStr != null)
            {
                for (int i = 0, colon = 0, space = 0, start = 0; i < argStr.Length;)
                {
                    colon = dispMessage.IndexOf(':', 0);
                    space = dispMessage.IndexOf(" ", start);
                    if ((colon == -1) || (space == -1))
                    {
                        break;
                    }
                    if (space < colon)
                    {
                        start = space + 1;
                        continue;
                    }
                    dispMessage = dispMessage.Replace(
                        dispMessage.Substring(colon, space - colon + 1), argStr[i]);
                    i++;
                }
            }
            dispMessage = ReplaceMessageSpace(dispMessage);

            return dispMessage;
        }

        //************************************************************************
        /// <summary>
        /// メッセージの ":xxx " の部分を " "(半角スペース)に入れ替える。
        /// </summary>
        /// <param name="argMsg">元のメッセージ</param>		
        /// <returns>変更後のメッセージ</returns>
        //************************************************************************
        public static string ReplaceMessageSpace(string argMsg)
        {
            string dispMessage = argMsg;

            for (int colon = 0, space = 0, start = 0;;)
            {
                colon = dispMessage.IndexOf(':', 0);
                space = dispMessage.IndexOf(" ", start);
                if ((colon == -1) || (space == -1))
                {
                    break;
                }
                if (space < colon)
                {
                    start = space + 1;
                    continue;
                }
                dispMessage = dispMessage.Replace(
                    dispMessage.Substring(colon,
                                          space - colon + 1), " ");
            }
            return dispMessage;
        }

        //************************************************************************
        /// <summary>
        /// appSettingsから指定した情報を取得する。
        /// </summary>
        /// <param name="argkey">キー</param>
        /// <returns>キーに対応した値</returns>
        //************************************************************************
        public static string GetAppSettings(string argkey)
        {
            string value = "";

            if (argkey == null)
            {
                return value;
            }

            // appSettingsから指定した情報を取得する。
            value = ConfigurationManager.AppSettings[argkey];
            if (value == null)
            {
                value = "";
            }

            return value;
        }

        //************************************************************************
        /// <summary>
        /// 指定したファイルが存在するかどうかを確認します。
        /// </summary>
        /// <param name="fileName">確認するファイル。</param>
        /// <returns>path に既存のファイル名が格納されている場合は true 。それ以外の場合は false 。</returns>
        //************************************************************************
        public static bool IsExistFile(string fileName)
        {
            bool isExsit = false;
            try
            {
                string strIniPath = "";
                GetIniPath(ref strIniPath);
                string fullPath = strIniPath + "\\" + fileName;
                NCLogger.GetInstance().WriteDebugLog("EKK.ini path:" + fullPath);
                isExsit = File.Exists(fullPath);
                if (!isExsit)
                {
                    fullPath = Directory.GetCurrentDirectory() + "\\" + fileName;
                    isExsit = File.Exists(fullPath);
                }
            }
            catch (Exception)
            {
                isExsit = false;
            }
            return isExsit;
        }

        //************************************************************************
        /// <summary>
        /// ローカルIPを取得する。
        /// </summary>
        /// <returns>ローカルIP</returns>
        //************************************************************************
        public static string GetLocalIP()
        {
            string retIp = "";
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = entry.AddressList[0];
                byte[] ipBytes = ipAddress.GetAddressBytes();
                foreach (byte ip in ipBytes)
                {
                    retIp += AddZero(Convert.ToInt32(ip), 3);
                }
            }
            catch (Exception)
            {
                retIp = "";
            }
            return retIp;
        }


        //************************************************************************
        /// <summary>
        /// 文字列を短縮する.
        /// </summary>
        /// <param name="longString">文字列</param>
        /// <param name="maxSize">最大のサイズ</param>
        /// <returns>短縮後の文字列</returns>
        //************************************************************************
        public static string SubString(string longString, int maxSize)
        {
            if (longString != null && longString.Length != 0)
            {
                // 文字列--->文字配列
                char[] charArray = longString.ToCharArray();
                // 毎一文字検査の索引
                int index = 0;
                // 文字サイズ
                int size = 0;
                // 毎一文字の検査
                while (size < maxSize)
                {
                    if (!IsSingleByte(charArray[index]))
                    {
                        // この文字は２桁
                        size += 2;
                    }
                    else
                    {
                        // この文字は１桁
                        size ++;
                    }

                    if (size > maxSize)
                    {
                        // 若し、サイズは充分な。以後の文字を処理しない
                        break;
                    }

                    // 索引 ++
                    index ++;

                    if (index == charArray.Length)
                    {
                        // 若し索引は文字配列のサイズ、以後の文字はない.
                        break;
                    }
                }
                // リターン
                return longString.Substring(0, index);
            }
            else
            {
                // 文字列は null か ""
                return longString;
            }
        }

        //************************************************************************
        /// <summary>
        /// 文字列を短縮する.
        /// </summary>
        /// <param name="longString">文字列</param>
        /// <param name="start">文字列の開始位置</param>
        /// <param name="length">最大のサイズ</param>
        /// <returns>短縮後の文字列</returns>
        //************************************************************************
        public static string SubString(string longString, int start, int length)
        {
            if (start == 0)
            {
                return SubString(longString, length);
            }
            if (longString != null && longString.Length != 0)
            {
                // 文字列--->文字配列
                char[] charArray = longString.ToCharArray();
                // 毎一文字検査の索引
                int index = 0;
                // 文字サイズ
                int size = 0;
                // 毎一文字の検査
                while (size < start)
                {
                    if (!IsSingleByte(charArray[index]))
                    {
                        // この文字は２桁
                        size += 2;
                    }
                    else
                    {
                        // この文字は１桁
                        size ++;
                    }

                    if (size > start)
                    {
                        // 若し、サイズは充分な。以後の文字を処理しない
                        break;
                    }
                    // 索引 ++
                    index ++;


                    if (index == charArray.Length)
                    {
                        // 若し索引は文字配列のサイズ、以後の文字はない.
                        break;
                    }
                }
                longString = longString.Substring(index);
                if (longString.Length == 0)
                {
                    return "";
                }
                // 文字列--->文字配列
                charArray = longString.ToCharArray();
                // 毎一文字検査の索引
                index = 0;
                // 文字サイズ
                size = 0;
                // 毎一文字の検査
                while (size < length)
                {
                    if (!IsSingleByte(charArray[index]))
                    {
                        // この文字は２桁
                        size += 2;
                    }
                    else
                    {
                        // この文字は１桁
                        size ++;
                    }

                    if (size > length)
                    {
                        // 若し、サイズは充分な。以後の文字を処理しない
                        break;
                    }

                    // 索引 ++
                    index ++;

                    if (index == charArray.Length)
                    {
                        // 若し索引は文字配列のサイズ、以後の文字はない.
                        break;
                    }
                }
                // リターン
                return longString.Substring(0, index);
            }
            else
            {
                return longString;
            }
        }
        /// <summary>
        /// 配置ファイル名取得
        /// </summary>
        /// <returns></returns>
        public static string GetAppConfig()
        {
            return System.Windows.Forms.Application.ProductName + NCConst.NC_GLOBAL_FILE_EXT;
        }

        //************************************************************************
        /// <summary>
        /// 文字の桁の検査.
        /// </summary>
        /// <param name="value">文字</param>
        /// <returns>true:文字は1桁; false:2桁</returns>
        //************************************************************************
        public static bool IsSingleByte(char value)
        {
            if (Array.IndexOf(SINGLE_CHAR, value) >= 0)
            {
                return true;
            }
            else
            {
                return IsAlpha("" + value);
            }
        }

        #endregion

        #region ログのパラメーターを取得します

        //************************************************************************
        /// <summary>
        /// Ekk.iniファイルパスを取得します
        /// </summary>
        /// <param name="strIniPath">string Ekk.iniファイルパス</param>
        //************************************************************************
        private static void GetIniPath(ref string strIniPath)
        {
            try
            {
                NdnXmlConfig xmlConfig = null;
                xmlConfig = new NdnXmlConfig(GetAppConfig(), NCConst.CONFIG_FILE_DIR);
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
    }
}