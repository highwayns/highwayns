using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    /// <summary>
    /// Class that defines the REST API ACTIVITY STREAM Endpoints.
    /// </summary>
    class RealTimeStreamEndpoints
    {

        /// <summary>
        /// get real time stream (O = Id of stream)
        /// </summary>
        public static readonly string GET_REALTIMESTREAM = "/stream/subscription/{0}";

        /// <summary>
        /// Post a new Real time stream
        /// </summary>
        public static readonly string POST_REALTIMESTREAM = "/stream/subscription";

        /// <summary>
        /// Delete all streams 
        /// </summary>
        public static readonly string DELETEALL_REALTIMESTREAM = "/stream/subscription/@all";

                
    }
}
