using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpaceID.SDK.OAuth.Common.Enums;
using System.IO;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Config;

namespace MySpaceID.SDK.Api
{
    public abstract class BaseApi
    {
        public SecurityContext Context { get; set; }

        public BaseApi(SecurityContext context)
        {
            this.Context = context;
        }

        protected bool ValidatePositiveId(int value, string key)
        {
            if (value < 1)
                throw new MySpaceException(string.Format("{0} must be greater than 0", key),
                    MySpaceExceptionType.REQUEST_FAILED);
            return true;

        }

        protected bool ValidatePageParameters(int page, int pageSize)
        {
            if (page < 1)
                throw new MySpaceException("Page value must be greater than 0", MySpaceExceptionType.REQUEST_FAILED);
            if (pageSize < 1)
                throw new MySpaceException("PageSize value must be greater than 0", MySpaceExceptionType.REQUEST_FAILED);
            return true;

        }

        /// <summary>
        /// Returns the Query string of dictionary Object passed
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        protected string getQueryParamFromDictionary(Dictionary<string, string> dic)
        {
            var queryParam = string.Empty;

            if (dic != null && dic.Count > 0)
            {
                foreach (var item in dic)
                {
                    queryParam += string.Format("{0}={1}&", item.Key, item.Value);
                }
                queryParam = queryParam.Remove(queryParam.Length - 1);
            }
            return queryParam;

        }

        public byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

    }
}
