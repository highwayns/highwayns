using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    /// <summary>
    /// Class that defines the REST API V2 Endpoints.
    /// </summary>
    class V2Endpoints
    {
        public static readonly string SELF_URL = "/v2/people/@me/@self?format=json";
        public static readonly string SELF_WITH_FIELDS_URL = "/v2/people/@me/@self?format=json&fields={0}";
        public static readonly string FRIENDS_URL = "/v2/people/@me/@friends?format=json";
    }
}
