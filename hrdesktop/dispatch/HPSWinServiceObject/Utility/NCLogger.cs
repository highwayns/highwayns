using System;
using System.IO;
using System.Text;

namespace NC.HPS.Lib
{

    #region ログ出力レベル列挙型
	//************************************************************************
	/// <summary>
	/// ログ出力レベル列挙型
	/// <remarks>
	/// ログ出力レベル列挙型
	/// </remarks>
	/// </summary>
	//************************************************************************
	public enum LogLevel {Error = 1, Warning = 2, Information = 3, Debug = 4};	
	#endregion

	//************************************************************************
	/// <summary>
	/// ログを書くクラス
	/// パス、ファイル名メイン、拡張子、ログファイル数、ログファイルサイズを受け取る。
	/// 指定されたパスのファイル名にログを出力する。
	/// ファイルが指定されたサイズにたせば、新しいログファイルを作る。
	/// ログファイルの個数は指定されたログファイル数を超える場合、一番古いファイルが削除される。
	/// </summary>
	//************************************************************************
	public class NCLogger
	{		
		#region フィールド
		/// <summary>
		/// ログ情報タイプの列挙型
		/// </summary>
		public enum LogInfoEnum {Start = 1, End = 2};
		private int    mLevel;
		private string mPath;
		private string mFileMain;
		private string mFileExt;
		private int    mFileCount;
		private int    mFileSize;
		private string mCurrFileNoExt;
		//現在のファイル名は　"c:\logger001.txt"のように、いつも1番目
		private string mCurrFullName;
		//ファイル可変部分の桁数、＝指定されたログファイル数の桁数
		private int		lenSeq;		
		private StreamWriter sw;
		private NdnPublicFunction m_pFunc = new NdnPublicFunction();
		private static NCLogger s_me = null;		
        private System.Diagnostics.EventLog m_EventLog = null;
		#endregion

		#region コンストラクタ
		//************************************************************************
		/// <summary>
		/// ログクラスの構成
		/// </summary>
		//************************************************************************
		private NCLogger()
		{
			// 
			// パラメータが指定されていない場合、Default値を指定する。
			//
			string strLogPath = "";
			string strLogFile = "";
			string strFileExt = "";
			int iLogSize = 0;
			int iLogCount = 0;
			int iLogLevel = 0;

			GetLogConfig(ref strLogPath, ref strLogFile, ref strFileExt, ref iLogCount, ref iLogSize, ref iLogLevel);

			Create(strLogPath, strLogFile, strFileExt, iLogCount, iLogSize, iLogLevel);
            
            m_EventLog   =   new   System.Diagnostics.EventLog();   
            m_EventLog.Source   =   System.Windows.Forms.Application.ProductName; 
	
		}

		//************************************************************************
		/// <summary>
		/// ログクラスの構成
		/// </summary>
		/// <param name="path">ファイルパス</param>
		/// <param name="FileMain">ファイルの名称</param>
		/// <param name="FileExt">ファイルの拡張子</param>
		/// <param name="FileCount">ファイルの数</param>
		/// <param name="FileSize">ファイルのサイズ</param>
		/// <param name="LogLevel">ログのレベル</param>
		//************************************************************************
		private NCLogger(string path, string FileMain, string FileExt,
			int FileCount, int FileSize, int LogLevel)
		{
			Create(path, FileMain, FileExt, FileCount, FileSize, LogLevel);
		}
		#endregion

		#region public メソッド
		//************************************************************************
		/// <summary>
		/// インスタンスを取得する。
		/// </summary>
		/// <returns>NCLoggerインスタンス</returns>
		//************************************************************************
		public static NCLogger GetInstance() 
		{
			if (s_me == null)
			{
				s_me = new NCLogger();				
			}
			if (s_me.sw == null) 
			{
				s_me.Open();
			}
			return s_me;
		}

		//************************************************************************
		/// <summary>
		/// デバッグログを出力する。
		/// </summary>
		/// <param name="log">ログ</param>
		//************************************************************************
		public void WriteDebugLog(string log)
		{
			Write(LogLevel.Debug, log);
		}

		//************************************************************************
		/// <summary>
		/// 正常情報ログを出力する。
		/// </summary>
		/// <param name="log">ログ</param>
		//************************************************************************
		public void WriteInfoLog(string log)
		{
			Write(LogLevel.Information, log);
		}
		
		//************************************************************************
		/// <summary>
		/// メソッドの開始／終了ログを出力する。
		/// </summary>
		/// <param name="methodName">メソッドの名称</param>
		/// <param name="logInfo">開始終了区分：開始－LogInfoEnum.Start、終了－LogInfoEnum.End</param>
		//************************************************************************
		public void WriteInfoLog(string methodName, LogInfoEnum logInfo)
		{			
			if (logInfo == LogInfoEnum.Start)
			{
				WriteInfoLog(methodName +  "　開始");
			}	
			else if (logInfo == LogInfoEnum.End)
			{
				WriteInfoLog(methodName +  "　終了");
			}
		}

		//************************************************************************
		/// <summary>
		/// 警告情報ログを出力する。
		/// </summary>
		/// <param name="log">ログ</param>
		//************************************************************************
		public void WriteWarningLog(string log)
		{
			Write(LogLevel.Warning, log);
		}

		//************************************************************************
		/// <summary>
		/// エラー情報ログを出力する。
		/// </summary>
		/// <param name="log">ログ</param>
		//************************************************************************
		public void WriteErrorLog(string log)
		{
			Write(LogLevel.Error, log);
		}

		//************************************************************************
		/// <summary>
		/// 異常情報ログを出力する。
		/// </summary>
		/// <param name="ex">ログ</param>
		//************************************************************************
		public void WriteExceptionLog(Exception ex)
		{
			WriteExceptionLog(ex, false);
            //m_EventLog.WriteEntry(ex.ToString());
        }

		//************************************************************************
		/// <summary>
		/// 例外情報ログを出力する。
		/// </summary>
		/// <param name="ex">例外オブジェクト</param>
		/// <param name="isShowErrorForm">例外画面表示フラグ：true-表示、false-表示しない</param>
		//************************************************************************
		public void WriteExceptionLog(Exception ex, bool isShowErrorForm)
		{
			Write(ex);
			if (isShowErrorForm)
			{					
                //NdnErrorForm errorForm = new NdnErrorForm(ex);		
                //errorForm.Top = 0;
                //errorForm.ShowDialog();									
			}
		}		

		#region ファイルをCLOSEする
		//************************************************************************
		/// <summary>
		/// void Close()
		/// </summary>
		/// <remarks>
		/// ファイルをCLOSEする
		/// </remarks>
		/// <returns>なし</returns>
		//************************************************************************
		public void Close()
		{
			try
			{
				if(sw != null)
				{
					sw.Close();
				}
				sw = null;
			}
			catch
			{
			}
		}
		#endregion

		#region ログをファイルにFLUSHする
		//************************************************************************
		/// <summary>
		/// void Flush()
		/// </summary>
		/// <remarks>
		/// ログをファイルにFLUSHする
		/// </remarks>
		/// <returns>なし</returns>
		/// <example>
		/// <code></code>
		/// </example>
		//************************************************************************
		public void Flush()
		{
			try
			{
				if (sw != null)
				{
					sw.Flush();
				}
			}
			catch
			{
			}
		}
		#endregion
		#endregion

		#region protected メソッド
		//************************************************************************
		/// <summary>
		/// ログクラスを作成する
		/// </summary>
		/// <remarks>
		/// ログクラスの構成
		/// </remarks>
		/// <param name="path">ログファイルパス</param>
		/// <param name="FileMain">ログファイル名</param>
		/// <param name="FileExt">ログファイル拡張子</param>
		/// <param name="FileCount">ログファイル数</param>
		/// <param name="FileSize">ログファイルサイズ</param>
		/// <returns>なし</returns>
		//************************************************************************
		protected void Create(string path, string FileMain, string FileExt,
			int FileCount, int FileSize, int LogLevel)
		{
			sw = null;

			mLevel = LogLevel;

			//入力チェック
			if( path==null || FileMain==null || FileExt==null || FileCount<1 || FileSize <100 ||
				path.Equals("") || FileMain.Equals("") || FileExt.Equals("") )
			{
				// パラメータが無効な場合、Default値を指定する。
				mPath = "C:\\";
				mFileMain = "logger";
				mFileExt = ".txt";
				mFileCount = 3;
				mFileSize = 1024;

				mCurrFileNoExt = "logger1";
				mCurrFullName = "c:\\logger1.txt";
				lenSeq = 1;
				return;
			}

			if(!path.EndsWith("\\") )
			{
				path = path +"\\";		//後ろ"/"を追加
			}
			mPath = path;
			mFileMain = FileMain;
			mFileExt = FileExt;
			mFileCount = FileCount;
			mFileSize = FileSize;

			string  StrLen = FileCount.ToString();		//FileCount's桁数
			lenSeq = StrLen.Length;
			if (lenSeq > 4) lenSeq =4;

			mCurrFileNoExt = mFileMain + "0001".Substring(4 - lenSeq, lenSeq) ;  //index from 0
			mCurrFullName = mPath + mCurrFileNoExt + mFileExt;
			
		}
		#endregion			

		#region private メソッド
		#region ファイルが存在しなければ、新規する
		//************************************************************************
		/// <summary>
		/// ログファイルハンドルを取得する
		/// </summary>
		/// <remarks>
		/// ファイルが存在しなければ、新規する
		/// ファイルサイズがオーバーしていない場合、追加する
		/// ファイルサイズがオーバーした場合、
		/// log002-->log003, log001 -->log002のように移動、そしてlog001を新規する
		/// ログファイル数はオーバーする場合、一番古いファイルを削除する
		/// ファイルのStreamWriter対象を返す
		/// </remarks>
		/// <returns>StreamWriter</returns>
		//************************************************************************
		private StreamWriter getFileHandle()
		{
			try
			{	
				//普通：StreamWriter存在、且つファイルサイズがオーバーしない場合、すぐreturn
				if( sw != null )
				{	
					FileInfo currfi2 = new FileInfo(mCurrFullName);
					if( currfi2.Length < mFileSize)
					{
						return sw;
					}
				}

				//1回目実行する時、またはファイル切替の場合
				// DIRが存在しなければ、新規する
				DirectoryInfo di = new DirectoryInfo(mPath);
				if( !di.Exists )		
				{
					di.Create();
				}

				// ファイルが存在しなければ、新規する
				FileInfo currfi = new FileInfo(mCurrFullName);
				if( !currfi.Exists )
				{
					//sw = currfi.CreateText();
					sw = new StreamWriter(mCurrFullName, true, Encoding.Default, 2048 );
					return sw;
				}

				// ファイルサイズがオーバーしていない場合、追加する
				if( currfi.Length < mFileSize)
				{
					//sw = File.AppendText(mCurrFullName);
					sw = new StreamWriter(mCurrFullName, true, Encoding.Default, 2048 );
					//sw = new StreamWriter(currfi, true, Encoding.Default, 2048 );
					return sw;
				}

				//サイズ　オーバー、新規ログファイルが必要。
				if( sw != null )
				{
					sw.Close();	//まずCLOSE
					sw = null;
				}
			
				//Filterでログファイルの箇数だけを取得する
				int fCount = di.GetFiles( mFileMain + "*" + mFileExt ).Length;
				int upperCount;
				if (fCount >= mFileCount )
				{
					//ログファイル数がオーバー、一番古いファイルを削除する
					upperCount = mFileCount;	
				}
				else
				{
					//ログファイル数がオーバーしていないが、ファイルRENAME成功するため、
					//RENAME先のファイルを存在しないように、（普通は存在していないはず）削除する。
					upperCount = fCount+1;
				}

				string StrCount = "0000" +  upperCount.ToString();
				string StrMaxFile = mFileMain + StrCount.Substring(StrCount.Length-lenSeq, lenSeq) ;  //index from 0
				StrMaxFile = mPath + StrMaxFile + mFileExt;
				File.Delete(StrMaxFile);	//File.Delete 存在しなくても、例外はしません

				string fileMoto;
				string fileSaki;
				for(int  i= upperCount-1; i>=1; i-- )
				{
					fileMoto = "0000"+ i.ToString();
					fileMoto =  mFileMain + fileMoto.Substring(fileMoto.Length-lenSeq, lenSeq) ;  //index from 0
					fileMoto = mPath + fileMoto + mFileExt;

					fileSaki = "0000"+ (i+1).ToString();
					fileSaki =  mFileMain + fileSaki.Substring(fileSaki.Length-lenSeq, lenSeq) ;  //index from 0
					fileSaki = mPath + fileSaki + mFileExt;
					//make sure the fileMoto is exist.
					using (StreamWriter swr = File.AppendText(fileMoto)) {}

					//RENAME the file
					File.Move(fileMoto, fileSaki);
				}

			
				//File.Delete(mCurrFullName);
				File.Delete(mCurrFullName);
				//sw = File.CreateText(mCurrFullName);	//File.CreateTextある場合、上書き
				sw = new StreamWriter(mCurrFullName, true, Encoding.Default, 2048 );
				return sw;
	
			}
			catch(Exception ex)
			{
				ex.ToString();
				//Console.WriteLine("getFileHandle is failed: {0}", e.ToString());
			}
			return null;
		}
		#endregion

		#region ファイルをOPENする
		//************************************************************************
		/// <summary>
		/// void Open()
		/// </summary>
		/// <remarks>
		/// ファイルをOPENする
		/// 実際Writeする前にOPENを呼び出さなくてもいい
		/// </remarks>
		/// <returns>なし</returns>
		//************************************************************************
		private void Open()
		{
			getFileHandle();
		}
		#endregion

		#region msgをlevelでログファイルに出力する
		//************************************************************************
		/// <summary>
		/// void Write(LogLevel level, string msg)
		/// </summary>
		/// <remarks>
		/// msgをlevelでログファイルに出力する
		/// </remarks>
		/// <param name="level">ログ出力レベル列挙型、Error；Warning；Information；Debug</param>
		/// <param name="msg">メッセージ文字列</param>
		/// <returns>なし</returns>
		/// <example>
		/// <code></code>
		/// </example>
		//************************************************************************
		private void Write(LogLevel level, string msg)
		{
			Write(level, msg, false);
		}
		#endregion

		#region msgをlevelでログファイルに出力する
		//************************************************************************
		/// <summary>
		/// void Write(LogLevel level, string msg, bool close)
		/// </summary>
		/// <remarks>
		/// msgをlevelでログファイルに出力する
		/// </remarks>
		/// <param name="level">ログ出力レベル列挙型、Error；Warning；Information；Debug</param>
		/// <param name="msg">メッセージ文字列</param>
		/// <param name="close">「true」はファイルを閉じるし、リソースを解放します</param>
		/// <return>なし</return>
		/// <example>
		/// <code></code>
		/// </example>
		//************************************************************************
		private void Write(LogLevel level, string msg, bool close)
		{
			try
			{
				string strBuf = "";
				bool bReturn = GetLevelString(level, out strBuf);
				if (bReturn == false) return;
				StreamWriter sw = getFileHandle();
				if (sw != null) 
				{
					sw.WriteLine(GetTimeString() + "  " + strBuf + "  " + msg );
					//comment it in release version
					sw.Flush();
                    m_EventLog.WriteEntry(msg);
				}

				//ファイルを閉じるし、リソースを解放します
				if (close)
				{
					Close();
				}
			}
			catch
			{
			}

		}
		#endregion
		
		#region Exception 書式設定されたの文字列を書き出し
		//************************************************************************
		/// <summary>
		/// void Write(Exception exp)
		/// </summary>
		/// <remarks>
		/// Exception 書式設定されたの文字列を書き出し
		/// </remarks>
		/// <param name="exp">Exception 発生するエラーを表します</param>
		/// <returns>なし</returns>
		/// <example>
		/// <code></code>
		/// </example>
		//************************************************************************
		private void Write(Exception exp)
		{
			try
			{
				StreamWriter sw = getFileHandle();
				if (sw != null)
				{
					sw.WriteLine(String.Format("{0}\tError: \tFunction: {1}; \r\nMessage: {2}; \r\n{3}", GetTimeString(), exp.TargetSite, exp.Message, exp.StackTrace));
					sw.Flush(); 
				}
			}
			catch
			{
			}
		}
		
		#endregion

		#region ログのパラメーターを取得します
		//************************************************************************
		/// <summary>
		/// ログのパラメーターを取得します
		/// </summary>
		/// <remarks>
		/// ログのパラメーターを取得します
		/// </remarks>
		/// <param name="strLogPath">string ログファイルパス</param>
		/// <param name="strLogFile">string ログファイル名</param>
		/// <param name="strFileExt">string ログファイル拡張子</param>
		/// <param name="iLogCount">int ログファイル数</param>
		/// <param name="iLogSize">int ログファイルサイズ</param>
		/// <returns>なし</returns>
		/// <example>
		/// <code></code>
		/// </example>
		//************************************************************************
		private void GetLogConfig(ref string strLogPath, ref string strLogFile, ref string strFileExt, ref int iLogCount, ref int iLogSize, ref int iLogLevel)
		{
			try
			{
				string strValue = "";
				NdnXmlConfig xmlConfig = null;
				xmlConfig = new NdnXmlConfig(NCUtility.GetAppConfig(), NCConst.CONFIG_FILE_DIR);
				xmlConfig.ReadXmlData("log", "LogPath", ref strLogPath);
				xmlConfig.ReadXmlData("log", "LogFile", ref strLogFile);
				xmlConfig.ReadXmlData("log", "LogExt", ref strFileExt);
				xmlConfig.ReadXmlData("log", "LogCount", ref strValue);
				iLogCount = m_pFunc.StringToInt(strValue, 8);
				xmlConfig.ReadXmlData("log", "LogSize", ref strValue);
				iLogSize = m_pFunc.StringToInt(strValue, 4096000);
				xmlConfig.ReadXmlData("log", "LogLevel", ref strValue);
				iLogLevel = m_pFunc.StringToInt(strValue, 3);
			
			}
			catch
			{
			}
		}
		#endregion

		#region 指定したエラーレベル対応のメッセージを返します
		//************************************************************************
		/// <summary>
		/// void GetLogConfig(ref string strLogPath, ref string strLogFile, ref string strFileExt, ref int iLogCount, ref int iLogSize)
		/// </summary>
		/// <remarks>
		/// 指定したエラーレベル対応のメッセージを返します
		/// </remarks>
		/// <param name="level">ログレベル列挙型</param>
		/// <param name="strLevel">out:ログレベル対応のメッセージ文字列</param>
		/// <returns>bool 指定したログ出力レベルより低いときに、「true」を返します</returns>
		/// <example>
		/// <code></code>
		/// </example>
		//************************************************************************
		private bool GetLevelString(LogLevel level, out string strLevel)
		{
			bool bReturn = false;
			strLevel = "";
			if ((int)level <= mLevel)
			{
				bReturn = true;
				//エラーレベル
				if (level == LogLevel.Error)
				{
					strLevel = "Error:";
				}
					//警告
				else if (level == LogLevel.Warning)
				{
					strLevel = "Warning:";
				}
					//インフォーメーション
				else if (level == LogLevel.Information)
				{
					strLevel = "Information:";
				}
					//デバッグ
				else if (level == LogLevel.Debug)
				{
					strLevel = "Debug:";
				}
			}
			return bReturn;
		}
		#endregion

		#region DateTime 指定した書式の string を返します
		//************************************************************************
		/// <summary>
		/// string GetTimeString()
		/// </summary>
		/// <remarks>
		/// DateTime 指定した書式の string を返します
		/// </remarks>
		/// <returns>string:値をそれと等価な文字列形式に変換します</returns>
		/// <example>
		/// <code></code>
		/// </example>
		//************************************************************************
		private string GetTimeString()
		{
			DateTime dt = DateTime.Now;			
			string strReturn = String.Format("[{0:000#}-{1:0#}-{2:0#} {3:0#}:{4:0#}:{5:0#}.{6:00#}] ", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
			return strReturn;
		}
		#endregion		
		#endregion
	}
}
