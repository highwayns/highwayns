using System.Drawing;

namespace NC.HPS.Lib
{
	//************************************************************************
	/// <summary>
	/// 定数の宣言を格納。
	/// </summary>
	//************************************************************************
	public class NCConst
	{
		#region フィールド

		/// <summary>
		/// Sqlエラー：一意制約違反
		/// </summary>
		public const int Sql2627  = 2627;

		/// <summary>
		/// Sqlエラー：データベースが存在しない
		/// </summary>
		public const int Sql2702  = 2702;

        #region フィールド

        /// <summary>
        /// オラクルエラー：一意制約違反
        /// </summary>
        public const int Ora0001 = 1;

        /// <summary>
        /// オラクルエラー：リソースビジー
        /// </summary>
        public const int Ora0054 = 54;

        /// <summary>
        /// オラクルエラー：サービス名が存在しない
        /// </summary>
        public const int Ora12154 = 12154;

        /// <summary>
        /// オラクルエラー：ユーザ名／パスワード不正
        /// </summary>
        public const int Ora01017 = 1017;

        /// <summary>
        /// オラクルエラー：ＤＢ接続エラー（ネットワーク）
        /// </summary>
        public const int Ora12560 = 12560;

        /// <summary>
        /// オラクルエラー：TNS: パケット・ライターに障害が発生しました
        /// </summary>
        public const int Ora12571 = 12571;

        /// <summary>
        /// オラクルエラー：バインドエラー
        /// </summary>
        public const int Ora01008 = 1008;

        /// <summary>
        /// Oracleに接続されていません。
        /// </summary>
        public const int Ora03114 = 3114;
	    
		/// <summary>
		/// エラー時のバッチ戻り値
		/// </summary>
		public const int BatchError  = -1;

		/// <summary>
		/// メッセージの種類（結果通知）
		/// </summary>
		public const string ResultMsg = "i";

		/// <summary>
		/// メッセージの種類（問い合わせ1）
		/// </summary>
		public const string InquiryMsg1 = "q1";

		/// <summary>
		/// メッセージの種類（問い合わせ2）
		/// </summary>
		public const string InquiryMsg2 = "q2";

		/// <summary>
		/// メッセージの種類（警告1）
		/// </summary>
		public const string WarningMsg1 = "w1";

		/// <summary>
		/// メッセージの種類（警告2）
		/// </summary>
		public const string WarningMsg2 = "w2";	

		/// <summary>
		/// メッセージの種類（エラー）
		/// </summary>
		public const string Error = "e";

		/// <summary>
		/// configファイルの拡張子
		/// </summary>
		public const string NC_GLOBAL_FILE_EXT = ".config";
        /// <summary>
        /// 削除メールファイルの名前
        /// </summary>
        public const string DEL_MAIL_FILE_NAME = "del.xml";
        /// <summary>
        /// 本登録メールファイルの名前
        /// </summary>
        public const string MAIN_MAIL_FILE_NAME = "main.xml";
        /// <summary>
        /// 仮登録メールファイルの名前
        /// </summary>
        public const string TEMP_MAIL_FILE_NAME = "temp.xml";
        /// <summary>
        /// 解除メールファイルの名前
        /// </summary>
        public const string UNLOCK_MAIL_FILE_NAME = "unlock.xml";
        /// <summary>
        /// 警告メールファイルの名前
        /// </summary>
        public const string ALERT_FILE_NAME = "alert.xml";
        /// <summary>
        /// メッセージファイルの名前
        /// </summary>
        public const string MESSAGE_FILE_NAME = "NCMessage";
        /// <summary>
        /// メッセージファイルの名前
        /// </summary>
        public const string MESSAGE_FILE_EXT = ".xml";
        /// <summary>
        /// IConnectionString
        /// </summary>
        public const string IConnectionString = "IConnectionString";
        /// <summary>
        /// SConnectionString
        /// </summary>
        public const string SConnectionString = "SConnectionString";
        /// <summary>
        /// SConnectionString
        /// </summary>
        public const string ConnectionString = "ConnectionString";
        /// <summary>
		/// 配置フォルダ
		/// </summary>
		public const string CONFIG_FILE_DIR = @"C:\HPS\config\";
        /// <summary>
        /// ServNames
        /// </summary>
        public const string SERV_NAMES = "ServNames.ini";        
        /// <summary>
        /// 未送信フォルダ
        /// </summary>
        public const string NOT_SEND_FOLDER = @"未送信フォルダ";
        /// <summary>
        /// 送信失敗フォルダ
        /// </summary>
        public const string SENT_FOLDER = @"送信済みフォルダ";
        /// <summary>
        /// 送信済みフォルダ
        /// </summary>
        public const string FAIL_FOLDER = @"送信失敗フォルダ";
        /// <summary>
        /// メールファイル拡張子
        /// </summary>
        public const string MAIL_FILE_EXT = ".Mail";

		/// <summary>
		/// XMLフォルダ
		/// </summary>
		public const string XML_FILE_DIR = @"xml\";

		#endregion

		#region private メソッド
		//************************************************************************
		/// <summary>
		/// 16進の文字列を渡してColor構造体を取得する。
		/// </summary>
		/// <param name="argStr">16進文字列。</param>
		/// <returns>16進数で指定したColor構造体のメンバ。</returns>
		//************************************************************************
		private static Color GetColor(string argStr)
		{
			return ColorTranslator.FromHtml(argStr);
		}
		#endregion

		#region public メソッド
		//************************************************************************
		/// <summary>
		/// エラー時のメッセージの色を取得する。
		/// </summary>
		/// <returns>エラー時のメッセージカラー。</returns>
		//************************************************************************
		public static Color GetColorErrMsg()
		{
			// XMLファイルより色情報を取得予定。
			return ColorTranslator.FromHtml("#FF0000");
			//return Color.Red;
		}

		//************************************************************************
		/// <summary>
		/// 警告時のメッセージの色を取得する。
		/// </summary>
		/// <returns>警告時のメッセージカラー。</returns>
		//************************************************************************
		public static Color GetColorWarnMsg()
		{
			// XMLファイルより色情報を取得予定。
			return ColorTranslator.FromHtml("#0000FF");
			//return Color.Blue;
		}

		//************************************************************************
		/// <summary>
		/// 問い合わせ時のメッセージの色を取得する。
		/// </summary>
		/// <returns>問い合わせ時のメッセージカラー。</returns>
		//************************************************************************
		public static Color GetColorInquiryMsg()
		{
			// XMLファイルより色情報を取得予定。
			return ColorTranslator.FromHtml("#FF0000");
			//return Color.Red;
		}
		
		//************************************************************************
		/// <summary>
		/// 正常時のメッセージの色を取得する。
		/// </summary>
		/// <returns>正常時のメッセージカラー。</returns>
		//************************************************************************
		public static Color GetColorNmlMsg()
		{
			// XMLファイルより色情報を取得予定。
			return ColorTranslator.FromHtml("#000000");
			//return Color.Black;
		}

		//************************************************************************
		/// <summary>
		/// エラー時のテキストボックス背景色を取得する。
		/// </summary>
		/// <returns>エラー時のテキストボックス背景色。</returns>
		//************************************************************************
		public static Color GetColorErrTextBox()
		{
			// XMLファイルより色情報を取得予定。
			return ColorTranslator.FromHtml("#FFFF00");
			//return Color.Yellow;
		}

		//************************************************************************
		/// <summary>
		/// 正常時のテキストボックス背景色を取得する。
		/// </summary>
		/// <returns>エラー正常時のテキストボックス背景色。</returns>
		//************************************************************************
		public static Color GetColorNmlTextBox()
		{
			// XMLファイルより色情報を取得予定。
			return ColorTranslator.FromHtml("#FFFFFF");
			//return Color.White;
		}
		#endregion

    }
        #endregion
}
