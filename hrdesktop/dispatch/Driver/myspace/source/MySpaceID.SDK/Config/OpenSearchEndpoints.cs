using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    /// <summary>
    /// Class that defines the OPEN SEARCH Endpoints.
    /// </summary>
    class OpenSearchEndpoints
    {

        public static readonly string PEOPLE_OPENSEARCH = "/opensearch/people?{0}";
        public static readonly string IMAGES_OPENSEARCH = "/opensearch/images?{0}";
        public static readonly string VIDEOS_OPENSEARCH = "/opensearch/videos?{0}";

    }
}
