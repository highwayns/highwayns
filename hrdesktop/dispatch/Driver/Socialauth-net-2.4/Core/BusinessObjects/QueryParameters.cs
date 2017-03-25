/*
===========================================================================
Copyright (c) 2010 BrickRed Technologies Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sub-license, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
===========================================================================

*/
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System;

namespace Brickred.SocialAuth.NET.Core
{
    [Serializable]
    public class QueryParameter
    {
        private string key = null;
        private string value = null;

        public QueryParameter(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public string Key
        {
            get { return key; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }

    /// <summary>
    /// Comparer class used to perform the sorting of the query parameters
    /// </summary>
    public class QueryParameterComparer : IComparer<QueryParameter>
    {

        #region IComparer<QueryParameter> Members

        public int Compare(QueryParameter x, QueryParameter y)
        {
            if (x.Key == y.Key)
            {
                return string.Compare(x.Value, y.Value);
            }
            else
            {
                return string.Compare(x.Key, y.Key);
            }
        }

        #endregion
    }

    [Serializable]
    public class QueryParameters : ICollection<QueryParameter>
    {
        List<QueryParameter> queryparameters = new List<QueryParameter>();


        public string this[string key]
        {
            get { return queryparameters.Find(x => x.Key == key).Value; }
            set { queryparameters.RemoveAll(x => x.Key == key); queryparameters.Add(new QueryParameter(key, value)); }
        }

        public void AddRange(QueryParameters range, bool shouldOverride)
        {
            foreach (var item in range)
            {
                if (shouldOverride && queryparameters.Exists(x => x.Key == item.Key))
                    queryparameters.RemoveAll(x => x.Key == item.Key);
                queryparameters.Add(new QueryParameter(item.Key, item.Value));
            }
        }

        public void Add(string name, string value)
        {
            queryparameters.Add(new QueryParameter(name, value));
        }

        public void Sort()
        {
            queryparameters.Sort(new QueryParameterComparer());
        }

        public bool HasName(string name)
        {
            return queryparameters.Exists(x => x.Key == name);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            QueryParameter p = null;
            for (int i = 0; i < queryparameters.Count; i++)
            {
                p = queryparameters[i];
                sb.AppendFormat("{0}={1}", p.Key, p.Value);

                if (i < queryparameters.Count - 1)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        public string ToEncodedString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var qp in queryparameters)
            {
                sb.Append("&").Append(Utility.UrlEncode(qp.Key)).Append("=").Append(Utility.UrlEncode(qp.Value));
            }
            return sb.ToString().Substring(1);
        }

        public string Get(string key)
        {
            return (HasName(key) ? queryparameters.Find(x => x.Key == key).Value : "");
        }

        #region ICollection<QueryParameter> Members

        public void Add(QueryParameter item)
        {
            queryparameters.Add(item);
        }

        public void Clear()
        {
            queryparameters.Clear();
        }

        public bool Contains(QueryParameter item)
        {
            return queryparameters.Contains(item);
        }

        public void CopyTo(QueryParameter[] array, int arrayIndex)
        {
            queryparameters.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return queryparameters.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(QueryParameter item)
        {
            return queryparameters.Remove(item);
        }

        #endregion

        #region IEnumerable<QueryParameter> Members

        public IEnumerator<QueryParameter> GetEnumerator()
        {
            return queryparameters.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return queryparameters.GetEnumerator();
        }

        #endregion
    }

    public static class QueryParametersExt
    {
        public static string GetEncodedRequestString(this QueryParameters obj)
        {
            StringBuilder sb = new StringBuilder();
            foreach (QueryParameter qp in obj)
            {
                sb.Append(string.Format("{0}={1}&", qp.Key, Utility.HttpTransferEncode(qp.Value)));
            }
            return sb.ToString();
        }
    }

}
