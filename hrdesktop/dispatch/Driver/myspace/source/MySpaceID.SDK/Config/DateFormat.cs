using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    /// <summary>
    /// Signifies what time of format dates will be returned in from the API. Refer here for more information: http://developerwiki.myspace.com/index.php?title=Date%2C_Time_and_Timezone_Formats
    /// </summary>
    public enum DateFormat
    {
        ISO8601 = 1,
        GMT = 2,
        EPOCH = 3,
        UTC = 4
    }
}
