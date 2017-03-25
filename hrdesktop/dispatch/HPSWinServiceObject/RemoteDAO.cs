/*
 システム  　：NEDESクライアント管理
 サブシステム：共通モジュール
 バージョン　：2.5.*
 著作権    　：ニューコン株式会社2008
 概要      　：サービス対象
 更新履歴  　：2007/12/25　　鄭軍　　新規
*/
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Management;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Data;

namespace NC.HPS.Lib
{
    /// <summary>
    /// RemoteDAO はAPPサーバと連結する関数を格納している。
    /// </summary>
    public class RemoteDAO 
    {
        /// <summary>
        /// リモート対象
        /// </summary>
        private CmWinServiceAPI m_srvApi = null;
        /// <summary>
        /// TCP
        /// </summary>
        private TcpChannel ch = null;


        /// <summary>
        /// DBClass contructor
        /// </summary>
        public RemoteDAO(string serviceUri)
        {
            ch = new TcpChannel();
            ChannelServices.RegisterChannel(ch, false);
            m_srvApi = (CmWinServiceAPI) Activator.GetObject(typeof (CmWinServiceAPI), serviceUri);
        }
        /// <summary>
        /// DBClass contructor
        /// </summary>
        public RemoteDAO(string serviceUri,TcpClientChannel tch)
        {
            ChannelServices.RegisterChannel(tch, false);
            m_srvApi = (CmWinServiceAPI) Activator.GetObject(typeof (CmWinServiceAPI), serviceUri);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (ch != null)
            {
                ChannelServices.UnregisterChannel(ch);
            }
            m_srvApi = null;
        }
    }
}