using System;

namespace MySpaceID.SDK.OAuth.Common.Extensions
{
    [Flags]
    public enum OAuthProblemSubType
    {
        oauth_parameters_absent,
        oauth_parameters_rejected,
        oauth_acceptable_timestamps,
        oauth_acceptable_versions,
        oauth_problem_advice,
    }
}