using System.Collections;

namespace NC.HPS.Lib
{
    /// <summary>
    /// NCArrayList の概要の説明です。
    /// </summary>
    public class NcArrayList : CollectionBase
    {
        public NcPara this[int index]
        {
            get { return (NcPara) List[index]; }
            set { List[index] = value; }
        }

        public int Add(NcPara value)
        {
            return (List.Add(value));
        }

        public int IndexOf(NcPara value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, NcPara value)
        {
            List.Insert(index, value);
        }

        public void Remove(NcPara value)
        {
            List.Remove(value);
        }


        public bool Contains(NcPara value)
        {
            return (List.Contains(value));
        }

        public bool Contains(string key)
        {
            foreach (NcPara para in List)
            {
                if (para.Key == key)
                {
                    return true;
                }
            }

            return false;
        }
    }
}