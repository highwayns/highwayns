using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    /// <summary>
    /// Exception class to handle exceptions unique to MySpaceID SDK>
    /// </summary>
    class MySpaceException : Exception {
        /// <summary>
        /// Internal error code.
        /// </summary>
        public MySpaceExceptionType Code { get; set; }
        /// <summary>
        /// HTTP response
        /// </summary>
        public string Response { get; set; }
     
        public MySpaceException(string message, MySpaceExceptionType code): this(message,code,null)
        {

        }
        public MySpaceException(string message, MySpaceExceptionType code, string response): base(message)
        {
            this.Code = code;
            this.Response = response;
        }
 }
}
