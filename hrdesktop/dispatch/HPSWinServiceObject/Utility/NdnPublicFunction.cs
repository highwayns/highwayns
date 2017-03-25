using System;
using System.Text;
using System.Text.RegularExpressions;

namespace NC.HPS.Lib
{
    //************************************************************************
    /// <summary>
    /// NdnPublicFunction の概要の説明です。
    /// </summary>
    //************************************************************************
    public class NdnPublicFunction
    {
        public const string YYYYMMDD = "yyyyMMdd";
        public const string YYYYSlashMMSlashDD = "yyyy/MM/dd";
        public const string CYYYYMMDD = "yyyy年MM月dd日";
        public const string HHMMSS = "HH:mm";
        Random rnd = new Random();
        public bool IsDate(string strDatetime)
        {
            bool result = true;
            DateTime date;

            try
            {
                date = Convert.ToDateTime(strDatetime);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public bool IsNumeric(string strInt)
        {
            try
            {
                Int64.Parse(strInt);
            }
            catch
            {
                return false;
            }
            return true;
        }
        
        public bool IsEmpty(string strValue)
        {
            if (strValue == null || strValue == "")
            {
                return true;
            }
            return false;
        }

        public bool IsMaxLength(string strTxt,int length)
        {
            Encoding eCod = Encoding.GetEncoding("Shift_JIS");

            int eInt = 0;
            //全角が含まれる場合があるのでバイトサイズを取得
            eInt = eCod.GetByteCount(strTxt);
            if (eInt > length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsMail(string str,bool canBeEmpty)
        {
            if(IsEmpty(str))
            {
                if(canBeEmpty)return true;
                else return false;
            }
            else 
            {
                string[] mails = str.Split(";".ToCharArray());
                for (int i = 0; i < mails.Length; i++)
                {
                    if (mails[i].IndexOf("@") < 1)
                    {
                        return false;
                    }
                    string[] tempstrs = mails[i].Split("@".ToCharArray());
                    if (tempstrs.Length != 2 || tempstrs[1].IndexOf(".") < 1)
                    {
                        return false;
                    }
                    string[] domains = tempstrs[1].Split(".".ToCharArray());
                    if (domains.Length < 2 || domains.Length > 4)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //************************************************************************
        /// <summary>
        /// 半角の検査.
        /// </summary>
        /// <param name="value">文字</param>
        /// <returns>true:半角; false:全角</returns>
        //************************************************************************
        public bool IsSingleStr(string strTxt)
        {
            Encoding eCod = Encoding.GetEncoding("Shift_JIS");
            
            if (eCod.GetByteCount(strTxt)> strTxt.Length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //************************************************************************
        /// <summary>
        /// パスワードの検査.（英数字3桁以上、16桁以下）
        /// </summary>
        /// <param name="strTxt">パスワード</param>
        /// <returns>true; false;</returns>
        //************************************************************************
        public bool IsPassWord(string strTxt)
        {
            System.String str = @"^[a-zA-Z0-9]{3,16}$";
            System.Text.RegularExpressions.Regex reg = new Regex(str);
            return reg.IsMatch(strTxt);
        }

        //************************************************************************
        /// <summary>
        /// 電話番号の検査.（20桁以下）
        /// </summary>
        /// <param name="strTxt">電話番号</param>
        /// <returns>true; false;</returns>
        //************************************************************************
        public bool IsPhone(string strTxt)
        {
            System.String str = @"^[-0-9]{0,20}$";
            System.Text.RegularExpressions.Regex reg = new Regex(str);
            return reg.IsMatch(strTxt);
        }

        //************************************************************************
        /// <summary>
        /// StringからIntへ変換する
        /// </summary>
        /// <param name="strValue">文字列</param>
        /// <param name="iDefault">デフォルト値</param>
        /// <returns>int </returns>
        //************************************************************************
        //StringからIntへ変換する
        public int StringToInt(string strValue, int iDefault)
        {
            int iReturn = iDefault;
            try
            {
                iReturn = Convert.ToInt32(strValue);
            }
            catch
            {
            }
            return iReturn;
        }

        //************************************************************************
        /// <summary>
        /// StringからLongへ変換する
        /// </summary>
        /// <param name="strValue">文字列</param>
        /// <param name="lDefault">デフォルト値</param>
        /// <returns>long </returns>
        //************************************************************************
        public long StringToLong(string strValue, long lDefault)
        {
            long lReturn = lDefault;
            try
            {
                lReturn = Convert.ToInt64(strValue);
            }
            catch
            {
            }
            return lReturn;
        }

        //************************************************************************
        /// <summary>
        /// StringからDoubleへ変換する
        /// </summary>
        /// <param name="strValue">文字列</param>
        /// <param name="dDefault">デフォルト値</param>
        /// <returns>double </returns>
        //************************************************************************
        public double StringToDouble(string strValue, double dDefault)
        {
            double dReturn = dDefault;
            try
            {
                dReturn = Convert.ToDouble(strValue);
            }
            catch
            {
            }
            return dReturn;
        }

        //************************************************************************
        /// <summary>
        /// StringからDateTimeへ変換する
        ///フォーマットの型：yyyy/MM/dd HH:mm:ss
        ///					yyyy/MM/dd
        ///strCultureName:"en-US"							
        ///				"ru-RU"	
        ///				"ja-JP"
        /// </summary>
        /// <param name="strValue">文字列</param>
        /// <returns>DateTime </returns>
        //************************************************************************
        public DateTime StringToDateTime(string strValue)
        {
            DateTime dtReturn = DateTime.Now;
            try
            {
                strValue = GetStringDate(strValue, 1);
                dtReturn = Convert.ToDateTime(strValue);
            }
            catch
            {
            }
            return dtReturn;
        }

        //************************************************************************
        /// <summary>
        /// 日付データフォーマット転換 mode(1:YYYYMMDD -> YYYY/MM/DD   2:YYYY/MM/DD -> YYYYMMDD)
        /// </summary>
        /// <param name="strVale">文字列</param>
        /// <param name="iMode">モード</param>
        /// <returns>string </returns>
        //************************************************************************
        public string GetStringDate(string strVale, int iMode)
        {
            string strReturn = strVale;

            try
            {
                if (1 == iMode)
                {
                    if (null != strVale && 8 == strVale.Length)
                    {
                        strReturn =
                            String.Format("{0,-4}/{1,-2}/{2,-2}", strVale.Substring(0, 4), strVale.Substring(4, 2),
                                          strVale.Substring(6));
                    }
                }
                else if (2 == iMode)
                {
                    strVale.Trim();

                    string[] arrayTemp = new String[3];
                    char[] cToken = new char[1] {'/'};
                    arrayTemp = strVale.Split(cToken);

                    strReturn =
                        String.Format("{0,4}{1,2}{2,2}", arrayTemp[0], arrayTemp[1].PadLeft(2, '0'),
                                      arrayTemp[2].PadLeft(2, '0'));
                }
            }
            catch
            {
            }
            return strReturn;
        }

        //************************************************************************
        /// <summary>
        /// 日付データが文字列に変換する
        /// </summary>
        /// <param name="dtValue">日付データ</param>
        /// <param name="iMode">モード</param>
        /// <returns>string </returns>
        //************************************************************************
        public string DateTimeToString(DateTime dtValue, int iMode)
        {
            string strReturn = "";
            try
            {
                switch (iMode)
                {
                    case 0:
                        strReturn = dtValue.ToString(YYYYMMDD);
                        break;
                    case 1:
                        strReturn = dtValue.ToString(YYYYSlashMMSlashDD);
                        break;
                    case 2:
                        strReturn = dtValue.ToString(CYYYYMMDD);
                        break;
                    case 3:
                        strReturn = dtValue.ToString(HHMMSS);
                        break;
                    case 4:
                        strReturn = ("日月火水木金土").Substring(int.Parse(dtValue.DayOfWeek.ToString("d")), 1);
                        break;
                    case 5:
                        strReturn = dtValue.DayOfWeek.ToString().Substring(0, 3).ToUpper();
                        break;
                    default:
                        break;
                }
            }
            catch
            {
            }
            return strReturn;
        }
                       
        #region object 表す double を返します

        //************************************************************************
        /// <summary>
        /// double ObjectToDouble(object objValue)
        /// </summary>
        /// <remarks>
        /// object 表す doubleを返します
        /// </remarks>
        /// <param name="objValue">object 値</param>
        /// <returns>double タイプの値</returns>
        /// <example>
        /// <code></code>
        /// </example>
        //************************************************************************
        public double ObjectToDouble(object objValue)
        {
            double dReturn = 0;
            if (null != objValue)
            {
                try
                {
                    dReturn = Convert.ToDouble(objValue);
                }
                catch
                {
                    dReturn = 0;
                }
            }
            return dReturn;
        }

        #endregion

        #region object 表す string を返します

        //************************************************************************
        /// <summary>
        /// string ObjectToString(object objValue)
        /// </summary>
        /// <remarks>
        /// object 表す stringを返します
        /// </remarks>
        /// <param name="objValue">object 値</param>
        /// <returns>string タイプの値</returns>
        /// <example>
        /// <code></code>
        /// </example>
        //************************************************************************
        public string ObjectToString(object objValue)
        {
            string strReturn = "";
            if (null != objValue)
            {
                try
                {
                    strReturn = objValue.ToString();
                }
                catch
                {
                    strReturn = "";
                }
            }
            return strReturn;
        }

        #endregion

        #region object 表す Int32 を返します

        //************************************************************************
        /// <summary>
        /// int ObjectToInt(object objValue)
        /// </summary>
        /// <remarks>
        /// object 表す Int32 を返します
        /// </remarks>
        /// <param name="objValue">object 値</param>
        /// <returns>Int32 タイプの値</returns>
        /// <example>
        /// <code></code>
        /// </example>
        //************************************************************************
        public int ObjectToInt(object objValue)
        {
            int iReturn = 0;
            if (null != objValue)
            {
                try
                {
                    iReturn = Convert.ToInt32(objValue);
                }
                catch
                {
                    iReturn = 0;
                }
            }
            return iReturn;
        }

        #endregion

        #region object 表す long を返します

        //************************************************************************
        /// <summary>
        /// long ObjectToLong(object objValue)
        /// </summary>
        /// <remarks>
        /// object を表す long を返します
        /// </remarks>
        /// <param name="objValue">object 値</param>
        /// <returns>long タイプの値</returns>
        /// <example>
        /// <code></code>
        /// </example>
        //************************************************************************
        public long ObjectToLong(object objValue)
        {
            long lValue = 0;

            if (objValue != null)
            {
                try
                {
                    //数値化
                    lValue = Convert.ToInt64(objValue.ToString());
                }
                catch
                {
                    //初期値設定
                    lValue = 0;
                }
            }

            return lValue;
        }

        #endregion

        #region object 表す 日付を返します

        //************************************************************************
        /// <summary>
        /// DateTime ObjectToDateTime(object objValue)
        /// </summary>
        /// <remarks>
        /// object 表す 日付を返します
        /// </remarks>
        /// <param name="objValue">object 値</param>
        /// <returns>DateTime タイプの値</returns>
        /// <example>
        /// <code></code>
        /// </example>
        //************************************************************************
        public DateTime ObjectToDateTime(object objValue)
        {
            DateTime dt = DateTime.Now;
            if (objValue != null)
            {
                try
                {
                    dt = Convert.ToDateTime(objValue);
                }
                catch
                {
                }
            }
            return dt;
        }

        #endregion

        #region DateTime 指定した書式の string を返します

        //************************************************************************
        /// <summary>
        /// string GetDateTimeString()
        /// </summary>
        /// <remarks>
        /// DateTime 指定した書式の string を返します
        /// </remarks>
        /// <param name="dt">指定した時刻</param>
        /// <returns>DateTime 指定した書式の string </returns>
        /// <example>
        /// <code></code>
        /// </example>
        //************************************************************************
        public string GetDateTimeString(DateTime dt)
        {
            string strReturn = "";
            strReturn =
                String.Format("[{0:000#}-{1:0#}-{2:0#} {3:0#}:{4:0#}:{5:0#}:{6:00#}] ", dt.Year, dt.Month, dt.Day,
                              dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
            return strReturn;
        }

        #endregion

        #region 唯一ファイル名取得
        /// <summary>
        /// 唯一ファイル名取得
        /// </summary>
        /// <returns></returns>
        public string GetUniquelyString()
        {
            const int RANDOM_MAX_VALUE = 1000;
            string strTemp, strYear, strMonth, strDay, strHour, strMinute, strSecond, strMillisecond;

            DateTime dt = DateTime.Now;
            int rndNumber = rnd.Next(RANDOM_MAX_VALUE);
            strYear = dt.Year.ToString();
            strMonth = (dt.Month > 9) ? dt.Month.ToString() : "0" + dt.Month.ToString();
            strDay = (dt.Day > 9) ? dt.Day.ToString() : "0" + dt.Day.ToString();
            strHour = (dt.Hour > 9) ? dt.Hour.ToString() : "0" + dt.Hour.ToString();
            strMinute = (dt.Minute > 9) ? dt.Minute.ToString() : "0" + dt.Minute.ToString();
            strSecond = (dt.Second > 9) ? dt.Second.ToString() : "0" + dt.Second.ToString();
            strMillisecond = dt.Millisecond.ToString();

            strTemp = strYear + strMonth + strDay + "_" + strHour + strMinute + strSecond + "_" + strMillisecond + "_" + rndNumber.ToString();

            return strTemp;
        }
        #endregion


    }
}