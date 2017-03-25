using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    /// <summary>
    /// Specifies what type of Application being used to make calls into the MySpace Developer Platform.  OnSite applications do not need an Access Token because access is implicitly implied
    /// when the user installs the onsite application. MySpace applications do not have implicit user-access; hence any calls made with this ApplicationType must have valid AccessTokens.
    /// </summary>
    public enum ApplicationType
    {
        /// <summary>
        /// OnSite applications do not need an Access Token because access is implicitly implied
        /// when the user installs the onsite application.
        /// </summary>
        OnSite,
        /// <summary>
        /// MySpaceID applications do not have implicit user-access; hence any calls made with this ApplicationType must have valid AccessTokens.
        /// </summary>
        MySpaceID
    }
}
