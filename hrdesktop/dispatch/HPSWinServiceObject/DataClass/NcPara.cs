
namespace NC.HPS.Lib
{
    //************************************************************************
    /// <summary>
    /// NdnParaクラス：DBから取得のデータを定義する
    /// </summary>
    //************************************************************************
    public class NcPara
    {
        /// <summary>
        /// キー
        /// </summary>
        public string Key;

        /// <summary>
        /// データタイプ
        /// </summary>
        public object Type;

        /// <summary>
        /// データのサイズ
        /// </summary>
        public int Size;

        /// <summary>
        /// データの値
        /// </summary>
        public object Value;

        /// <summary>
        /// データの戻るフラグ：true－戻る、false－戻らない
        /// </summary>
        public bool OutPut;

        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="strKey">キー</param>
        /// <param name="oracleType">データのタイプ</param>
        /// <param name="iSize">データのサイズ</param>
        /// <param name="objValue">データの値</param>
        //************************************************************************
        public NcPara(string strKey, object oracleType, int iSize, object objValue)
        {
            Key = strKey;
            Type = oracleType;
            Size = iSize;
            Value = objValue;
            OutPut = false;
        }

        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="strKey">キー</param>
        /// <param name="oracleType">データのタイプ</param>
        /// <param name="iSize">データのサイズ</param>
        /// <param name="objValue">データの値</param>
        /// <param name="bOutPut">データの戻るフラグ</param>
        //************************************************************************
        public NcPara(string strKey, object oracleType, int iSize, object objValue, bool bOutPut)
        {
            Key = strKey;
            Type = oracleType;
            Size = iSize;
            Value = objValue;
            OutPut = bOutPut;
        }
    }
}