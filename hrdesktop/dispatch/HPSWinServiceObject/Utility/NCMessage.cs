namespace NC.HPS.Lib
{	
	using System;
	using System.Xml;
	using System.Reflection;


	//************************************************************************
	/// <summary>
	/// XMLファイルよりメッセージを取得するクラス
	/// </summary>
	//************************************************************************
	public  class NCMessage
	{
		#region フィールド
		//XMLファイルパス
		//private static string m_documentPath = NCConst.CONFIG_FILE_DIR + NCConst.MESSAGE_FILE_NAME;
		private static string[] m_msgId = new string[600]; 
		private static string[] m_msgAlias = new string[600]; 
		private static string[] m_msg = new string[600]; 
		private static string[] m_msgOutput = new string[600]; 
		private static string[] m_msgNotice = new string[600]; 
		private static int m_count=0;
		private static NCMessage s_ncMessage = null;
		/// <summary>
		/// ロガーの宣言。
		/// </summary>
		//private static CMLogger s_logger = CMLogger.GetInstance();
		// クラス名を取得
		private static readonly string s_fqcn = MethodBase.GetCurrentMethod().DeclaringType.FullName;
		#endregion

		#region コンストラクタ
		//************************************************************************
		/// <summary>
		/// NCMessageクラスのコンストラクタ。
		/// </summary>
		//************************************************************************
		private NCMessage(String language)
		{
            ReadXml(language);
		}
		#endregion

		#region private メソッド
		//************************************************************************
		/// <summary>
		/// メッセージXMLファイルからメッセージ情報を取得し、配列にセットする。
		/// </summary>
		//************************************************************************
        private static void ReadXml(String language) 
		{
			
			try
			{
                String m_documentPath = NCConst.CONFIG_FILE_DIR + NCConst.MESSAGE_FILE_NAME;
                if (language != null && language != "" && language != "en")
                    m_documentPath += "." + language + NCConst.MESSAGE_FILE_EXT;
                else
                    m_documentPath +=  NCConst.MESSAGE_FILE_EXT;
				//XMLリーダを初期化
				XmlTextReader reader = new XmlTextReader( m_documentPath );
				try
				{
					//ループでXMLファイル情報を取得する
					while (reader.Read())
					{
						//ノードタイプを判断する
						switch (reader.NodeType)
						{
								//XMLタグの場合
							case XmlNodeType.Element:
								if (reader.LocalName == "Message") 
								{
									if( reader.MoveToFirstAttribute() )
									{
										do
										{
											//メッセージIDを取得
											if (reader.Name == "id") 
											{
												m_msgId[m_count] = reader.Value;
											}
											//エリアスを取得
											if (reader.Name == "alias") 
											{
												m_msgAlias[m_count] = reader.Value;
											}
											//メッセージを取得
											if (reader.Name == "msg") 
											{
												m_msg[m_count] = reader.Value;
											}
											//出力形式を取得
											if (reader.Name == "output") 
											{												
												m_msgOutput[m_count] = reader.Value;
											}
											//通知対象を取得
											if (reader.Name == "notice") 
											{
												m_msgNotice[m_count] = reader.Value;
											}	
										} while( reader.MoveToNextAttribute() );							
										m_count = m_count + 1;
									}						
								}

								break;
								//テキストの場合
							case XmlNodeType.Text:
								break;
								//終了タグの場合
							case XmlNodeType.EndElement:
								break;
						}
					}
				}
				catch(Exception err)
				{
					//取得失敗時、配列を初期化する
					m_msgId = null;
					m_msgAlias = null;
					m_msg = null;
					m_msgOutput = null;
					m_msgNotice = null;
					m_msgId = new string[600]; 
					m_msgAlias = new string[600]; 
					m_msg = new string[600]; 
					m_msgOutput = new string[600]; 
					m_msgNotice = new string[600]; 
					m_count = 0;		
					NCLogger.GetInstance().WriteExceptionLog(err);
				}
				finally
				{
					reader.Close();
					reader = null;
				}
			}
			finally
			{
			}


		}
		#endregion

		#region public メソッド
		//************************************************************************
		/// <summary>
		/// NCMessageクラスのインスタンスを取得する。
		/// </summary>
		//************************************************************************
		public static NCMessage GetInstance(String langauge) 
		{
			if( s_ncMessage == null )
			{
                s_ncMessage = new NCMessage(langauge);
			}
			return s_ncMessage;
		}

		//************************************************************************
		/// <summary>
		/// メッセージＩＤからメッセージなどを取得する。
		/// argMsgIdがnullの場合は、メッセージ、出力形式、通知対象は空文字で返却される。
		/// </summary>
		/// <param name="argMsgId">取得する対象となるメッセージＩＤ。</param>
		/// <param name="msg">メッセージ。</param>
		/// <param name="output">出力形式。</param>
		/// <param name="notice">イベントログの通知対象。</param>
		/// <returns>1:取得失敗　0:取得成功</returns>
		//************************************************************************
        public string GetMessageById(string argMsgId,String language) 
		{
			string msg = "";
			//output = "";
			//notice = "";
			//int result = 1;
			if (argMsgId == null) 
			{
                return msg;
			}
			if (m_count == 0) 
			{
                ReadXml(language);
			}
			if (m_count > 0) 
			{
				for(int i = 0; i < m_count; i++)
				{
					if (m_msgId[i] != null) 
					{
						if (m_msgId[i] == argMsgId) 
						{
							msg = m_msg[i];
							//output = m_msgOutput[i];
							//notice = m_msgNotice[i];
							//result = 0;
                            return msg;
						}
					}
				}
			}
            return msg;

		}

		//************************************************************************
		// <summary>
		// メッセージＩＤからメッセージなどを取得する。
		// argMsgIdがnullの場合は、メッセージ、出力形式、通知対象は空文字で返却される。
		// </summary>
		// <param name="argMsgId">取得する対象となるメッセージエリアス。</param>
		// <param name="msg">メッセージID。</param>
		// <param name="msg">メッセージ。</param>
		// <param name="output">出力形式。</param>
		// <param name="notice">イベントログの通知対象。</param>
		// <returns>1:取得失敗　0:取得成功</returns>
		//************************************************************************
		public  int GetMessageByAlias (string argMsgAlias,out string msgid,out string msg,out string output,out string notice,string language) 
		{
			msg = "";
			output = "";
			notice = "";
			msgid = "";
			int result = 1;
			if (argMsgAlias == null) 
			{
				return result;
			}
			if (m_count == 0) 
			{
                ReadXml(language);
			}
			if (m_count > 0) 
			{
				for(int i = 0; i < m_count; i++)
				{
					if (m_msgAlias[i] != null) 
					{
						if (m_msgAlias[i] == argMsgAlias) 
						{
							msg = m_msg[i];
							output = m_msgOutput[i];
							notice = m_msgNotice[i];
							msgid = m_msgId[i];
							result = 0;
							return result;
						}
					}
				}
			}
			return result;

		}
		#endregion
	}
}
