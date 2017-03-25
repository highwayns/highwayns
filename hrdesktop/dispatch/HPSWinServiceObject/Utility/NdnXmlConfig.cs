using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Xml;

namespace NC.HPS.Lib
{
    //************************************************************************
    /// <summary>
    /// ボタンのプロパティ
    /// </summary>
    //************************************************************************
    public class NdnButtonProperty
    {
        /// <summary>
        /// ボタンの名.
        /// </summary>
        public string strName;

        /// <summary>
        /// ボタンの値.
        /// </summary>
        public string strValue;

        /// <summary>
        /// ボタンは使用可能.
        /// </summary>
        public bool bEnabled;

        /// <summary>
        /// ボタンは目に見える.
        /// </summary>
        public bool bVisible;
    }

    //************************************************************************
    /// <summary>
    /// NdnXmlConfig の概要の説明です。
    /// </summary>
    //************************************************************************
    public class NdnXmlConfig
    {
        #region フィールド

        /// <summary>
        /// XMLファイル.
        /// </summary>
        protected string m_strXmlFile;

        #endregion

        #region コンストラクタ

        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="strXmlFile">XMLファイル.</param>
        //************************************************************************
        public NdnXmlConfig(string strXmlFile)
        {
            m_strXmlFile = strXmlFile;
        }

        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="strXmlFile">XMLファイル.</param>
        /// <param name="strDir">XMLファイルディレクトリー</param>
        //************************************************************************
        public NdnXmlConfig(string strXmlFile, string strDir)
        {
            m_strXmlFile = strDir + "\\" + strXmlFile;
        }

        #endregion

        #region public メソッド

        //************************************************************************
        /// <summary>
        /// XMLロード
        /// </summary>
        //************************************************************************
        public XmlDocument LoadXml()
        {
            XmlDocument xmlDocument = null;
            try
            {
                xmlDocument = new XmlDocument();
                xmlDocument.Load(m_strXmlFile);
            }
            catch (Exception err)
            {
                NCLogger.GetInstance().WriteExceptionLog(err);
                // NdnPublicFunction.WriteLog("", err.Message);
                xmlDocument = null;
            }

            return xmlDocument;
        }

        //************************************************************************
        /// <summary>
        /// バリュー取得する
        /// </summary>
        //************************************************************************
        public string GetValue()
        {
            return "";
        }

        //************************************************************************
        /// <summary>
        /// 指定のXMLファイルのデータを読み取り
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <returns>DialogResultを返す。</returns>
        //************************************************************************
        public bool ReadXmlData(ref DataSet dataSet)
        {
            bool bReturn = false;
            if (m_strXmlFile != null)
            {
                dataSet.ReadXml(m_strXmlFile);
                bReturn = true;
            }
            return bReturn;
        }

        //************************************************************************
        /// <summary>
        /// 指定のXMLファイルのデータを読み取り プロパティは”NAME"と”VALUE"の場合
        /// </summary>
        /// <param name="strSection">セクション</param>
        /// <param name="strName">ネーム</param>
        /// <param name="strValue">バリュー</param>
        /// <returns>boolを返す</returns>
        //************************************************************************
        public bool ReadXmlData(string strSection, string strName, ref string strValue)
        {
            bool bReturn = false;
            FileInfo fileInfo = null;
            XmlNode xmlNode;
            XmlAttributeCollection xmlAttributeCollection;
            try
            {
                fileInfo = new FileInfo(m_strXmlFile);
                if (fileInfo.Exists)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(m_strXmlFile);

                    xmlNode = xmlDocument.DocumentElement;
                    foreach (XmlNode iNode in xmlNode.ChildNodes)
                    {
                        xmlAttributeCollection = iNode.Attributes;
                        if (iNode.LocalName.Equals(strSection))
                        {
                            XmlAttribute xmlAttribute = xmlAttributeCollection["name"];
                            if (xmlAttribute.Value.Equals(strName))
                            {
                                xmlAttribute = xmlAttributeCollection["value"];
                                strValue = xmlAttribute.Value;
                                break;
                            }
                        }
                    }

                    if (strValue == null)
                    {
                        bReturn = false;
                    }
                    else
                    {
                        bReturn = true;
                    }
                }
            }
            catch (Exception err)
            {
                // NdnPublicFunction.WriteLog("", err.Message);
                NCLogger.GetInstance().WriteExceptionLog(err);
            }

            return bReturn;
        }

        //************************************************************************
        /// <summary>
        /// 指定のXMLファイルのデータを読み取り 
        /// </summary>
        /// <param name="strSection">セクション</param>
        /// <param name="strChildSection">サブセクション</param>
        /// <param name="strAttr1">属性1</param>
        /// <param name="strValue1">バリュー1</param>
        /// <param name="strAttr2">属性2</param>
        /// <param name="strValue2">バリュー2</param>
        /// <returns>boolを返す</returns>
        //************************************************************************
        public bool ReadXmlData(string strSection, string strChildSection, string strAttr1, string strValue1,
                                string strAttr2, ref string strValue2)
        {
            bool bReturn = false;
            FileInfo fileInfo = null;
            XmlNode xmlNode;
            XmlAttributeCollection xmlAttributeCollection;
            try
            {
                fileInfo = new FileInfo(m_strXmlFile);
                if (fileInfo.Exists)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(m_strXmlFile);

                    xmlNode = xmlDocument.DocumentElement;
                    foreach (XmlNode iNode in xmlNode.ChildNodes)
                    {
                        if (iNode.LocalName.Equals(strSection))
                        {
                            if (iNode.FirstChild.LocalName.Equals(strChildSection))
                            {
                                xmlAttributeCollection = iNode.FirstChild.Attributes;
                                XmlAttribute xmlAttribute = xmlAttributeCollection[strAttr1];
                                if (xmlAttribute.Value.Equals(strValue1))
                                {
                                    xmlAttribute = xmlAttributeCollection[strAttr2];
                                    strValue2 = xmlAttribute.Value;
                                    break;
                                }
                            }
                        }
                    }

                    if (strValue2 == null)
                    {
                        bReturn = false;
                    }
                    else
                    {
                        bReturn = true;
                    }
                }
            }
            catch (Exception err)
            {
                NCLogger.GetInstance().WriteExceptionLog(err);
                // NdnPublicFunction.WriteLog("", err.Message);
            }

            return bReturn;
        }


        //************************************************************************
        /// <summary>
        /// 新しいXMLファイルを作成
        /// ノードはNULL場合、新しいXMLファイルに追加する
        /// ノードはNOTNULL場合、新しいXMLファイルを作成する
        /// </summary>
        /// <param name="strRoot">ルート</param>
        /// <param name="strSection">セクション</param>
        /// <param name="strName">ネーム</param>
        /// <param name="strValue">バリュー</param>
        /// <returns>boolを返す</returns>
        //************************************************************************
        public bool CreateXmlFile(string strRoot, string strSection, string strName, string strValue)
        {
            bool bReturn = false;

            XmlTextWriter writer = null;

            try
            {
                writer = new XmlTextWriter(m_strXmlFile, System.Text.Encoding.Unicode);
                writer.Formatting = Formatting.Indented;

                writer.WriteStartDocument();
                writer.WriteStartElement(strRoot);

                if (strSection != null)
                {
                    writer.WriteStartElement(strSection);
                    writer.WriteAttributeString("name", strName);
                    writer.WriteAttributeString("value", strValue);
                }
                writer.WriteEndElement();
            }
            catch (Exception err)
            {
                // NdnPublicFunction.WriteLog("", err.Message);
                NCLogger.GetInstance().WriteExceptionLog(err);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
            return bReturn;
        }

        //************************************************************************
        /// <summary>
        /// 指定XMLの指定ノードの値を設定します
        /// 存在するノードを更新して、存在しないノードを挿入する
        /// </summary>
        /// <param name="strSection">セクション</param>
        /// <param name="strName">ネーム</param>
        /// <param name="strValue">バリュー</param>
        /// <returns>boolを戻す</returns>
        //************************************************************************
        public bool WriteValue(string strSection, string strName, string strValue)
        {
            bool bReturn = false;

            XmlDocument xmlDocument;
            XmlNode xmlNode;
            XmlElement xmlElement;
            XmlNode reXmlNode;

            try
            {
                xmlDocument = new XmlDocument();
                xmlDocument.Load(m_strXmlFile);
                xmlNode = xmlDocument.DocumentElement;

                foreach (XmlNode iNode in xmlNode.ChildNodes)
                {
                    if (iNode.LocalName.Equals(strSection) && iNode.Attributes["name"].Value==strName)
                    {
                        xmlNode = iNode;
                        break;
                    }
                    else if (iNode.Equals(xmlNode.LastChild))
                    {
                        xmlNode = null;
                    }
                }

                if (xmlNode == null)
                {
                    xmlElement = xmlDocument.CreateElement(strSection);
                    xmlElement.SetAttribute("name", strName);
                    xmlElement.SetAttribute("value", strValue);
                    xmlNode = xmlDocument.DocumentElement;
                    xmlNode.InsertAfter(xmlElement, xmlNode.LastChild);
                }
                else
                {
                    xmlElement = xmlDocument.CreateElement(strSection);
                    xmlElement.SetAttribute("name", strName);
                    xmlElement.SetAttribute("value", strValue);
                    reXmlNode = xmlDocument.DocumentElement;
                    reXmlNode.ReplaceChild(xmlElement, xmlNode);
                }
                xmlDocument.Save(m_strXmlFile);
                bReturn = true;
            }
            catch (Exception err)
            {
                string strrErr = err.Message;
                Console.WriteLine(strrErr);
            }
            return bReturn;
        }

        //************************************************************************
        /// <summary>
        /// 指定したLocalNameに一致するすべて子孫の要素リストを格納しているHashtableを返します
        /// </summary>
        /// <param name="strLocalName">ローカルネーム</param>
        /// <param name="hashtable">テーブル</param>
        /// <returns>boolを戻す</returns>
        //************************************************************************
        public bool GetElementsByName(string strLocalName, ref Hashtable hashtable)
        {
            bool bReturn = true;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(m_strXmlFile);
                XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName(strLocalName);
                foreach (XmlNode node in xmlNodeList)
                {
                    if (strLocalName.ToLower() == "button")
                    {
                        NdnButtonProperty buttonProperty = new NdnButtonProperty();
                        buttonProperty.strName = node.Attributes["name"].InnerText;
                        buttonProperty.strValue = node.Attributes["value"].InnerText;
                        buttonProperty.bEnabled = node.Attributes["enabled"].InnerText == "true";
                        buttonProperty.bVisible = node.Attributes["visible"].InnerText == "true";
                        hashtable.Add(node.Attributes["name"].InnerText, buttonProperty);
                    }
                    else
                    {
                        hashtable.Add(node.Attributes["name"].InnerText, node.Attributes["value"].InnerText);
                    }
                }
            }
            catch (Exception err)
            {
                bReturn = false;
                // NdnPublicFunction.WriteLog("", err.Message);
                NCLogger.GetInstance().WriteExceptionLog(err);
            }
            return bReturn;
        }

        #endregion

        #region private メソッド

        //************************************************************************
        /// <summary>
        /// アプリケーションのパスを取得します
        /// </summary>
        //************************************************************************
        private string GetAppPath()
        {
            string strAppPath = System.Windows.Forms.Application.StartupPath + "\\" + NCConst.XML_FILE_DIR;

            return strAppPath;
        }

        #endregion
    }
}