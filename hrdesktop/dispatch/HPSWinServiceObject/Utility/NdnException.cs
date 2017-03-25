namespace NC.HPS.Lib
{
    using System;

    //************************************************************************
    /// <summary>
    /// NdnExceptionクラスはExceptionクラスです。
    /// NdnExceptionクラスはSystem.ApplicationExceptionから継承するクラスです。
    /// </summary>
    //************************************************************************
    public class NdnException : ApplicationException
    {
        private const string m_xmlFileName = "NCMessage.xml"; // "NCMessage.jp.xml";

        private const string m_xmlLocalName = "message";

        private string m_strCode = null; //コード

        private string m_strMessage = null; //メッセージ

        private string[] m_strMessageArray; //メッセージ配列

//		private string m_strDetailMessage;//表示されたい詳細メッセージ

        //************************************************************************
        /// <summary>
        /// エラーコードだけを投げます
        /// </summary>
        /// <param name="strErrorCode">エラーメッセージ</param>
        /// <returns>void</returns>
        //************************************************************************
        public NdnException(
            string strErrorCode
            )
        {
            m_strCode = strErrorCode;
            NdnXmlConfig ndnXmlConfig = new NdnXmlConfig(m_xmlFileName);
            ndnXmlConfig.ReadXmlData(m_xmlLocalName, m_strCode, ref m_strMessage);
        }

        //
        // METHOD:
        // NdnException
        // DESCRIPTION:
        // エラーコードとエラーメッセージのパラメータ（String型）を投げます
        // PARAMETERS:
        // string strErrorCode エラーコード
        // string strErrorMessage エラーメッセージ
        // RETURN:
        // void
        //
//		public NdnException(
//								string strErrorCode,
//								string strErrorMessage
//								)
//		{
//
//			
//		}
        //************************************************************************
        /// <summary>
        /// エラーコードとエラーメッセージのパラメータ（配列型）を投げます
        /// </summary>
        /// <param name="strErrorCode">エラーメッセージ</param>
        /// <param name="strErrorMessage">エラーメッセージ</param>
        /// <returns>void</returns>
        //************************************************************************
        public NdnException(
            string strErrorCode,
            string[] strErrorMessage
            )
        {
            m_strCode = strErrorCode;

            NdnXmlConfig ndnXmlConfig = new NdnXmlConfig(m_xmlFileName);
            ndnXmlConfig.ReadXmlData(m_xmlLocalName, m_strCode, ref m_strMessage);
            Format(m_strMessage, strErrorMessage);
        }

        //************************************************************************
        /// <summary>
        /// コードを取得または設定します。
        /// </summary>
        /// <value>コード</value>
        //************************************************************************
        public string Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }

        //************************************************************************
        /// <summary>
        /// オーバーライドされた Message プロパティ。
        /// このプロパティは、追加したフィールドの値と共に
        /// 例外をテキスト形式で適切に示します。
        /// </summary>
        //************************************************************************
        public override string Message
        {
            get
            {
                string strMsg;

                strMsg = m_strMessage;

                return strMsg;
            }
        }

        //************************************************************************
        /// <summary>
        /// メッセージを設定します。
        /// </summary>
        /// <param name="strMessage">メッセージ</param>
        //************************************************************************
        public void SetMessage(string strMessage)
        {
            m_strMessage = strMessage;
        }

        //************************************************************************
        // <summary>
        // 詳細情報を取得または設定します。
        // </summary>
        //************************************************************************
//		public string DetailMessage
//		{
//			get
//			{
//				return m_strDetailMessage;
//			}
//			set
//			{
//				m_strDetailMessage = value;
//			}
//		}

        //************************************************************************
        /// <summary>
        /// 引数情報を取得または設定します。
        /// </summary>
        //************************************************************************
        public string[] MessagePara
        {
            get { return m_strMessageArray; }
            set { m_strMessageArray = value; }
        }

        private string Format(string strMsg, string[] strMessageExt)
        {
            try
            {
                m_strMessage = strMsg;

                if (strMessageExt != null)
                {
                    m_strMessage = string.Format(strMsg, strMessageExt);
                }
            }
            catch (ArgumentNullException)
            {
            }
            catch (FormatException)
            {
            }
            catch (Exception)
            {
            }

            return m_strMessage;
        }

        //************************************************************************
        /// <summary>
        ///		引数情報をフォーマットする
        /// </summary>
        /// <param name="strMsg">メッセージ</param>
        /// <returns>メッセージ</returns>
        //************************************************************************
        public string FormatMessage(string strMsg)
        {
            if (strMsg == null)
            {
                return null;
            }

            strMsg = strMsg.Trim();
            if (strMsg.Length > 0)
            {
                m_strMessage = Format(strMsg, m_strMessageArray);
            }
            else
            {
                m_strMessage = "";
            }

            return m_strMessage;
        }
    }
}