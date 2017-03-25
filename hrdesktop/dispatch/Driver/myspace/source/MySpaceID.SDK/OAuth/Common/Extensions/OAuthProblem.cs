using MySpaceID.SDK.OAuth.Common.Enums;
using MySpaceID.SDK.OAuth.Common.Utils;

namespace MySpaceID.SDK.OAuth.Common.Extensions
{
    public class OAuthProblem
    {
        public static readonly string OAUTH_PROBLEM = "oauth_problem";
        public static readonly string FORMAT_OAUTH_PROBLEM = "{0}={1}";
        public static readonly string FORMAT_RANGE = "{0}-{1}";
        public static readonly string FORMAT_PROBLEM_SUB = "{0}&{1}";

        public static string ToString(OAuthProblemType problemType)
        {
            return string.Format(FORMAT_OAUTH_PROBLEM, OAUTH_PROBLEM, problemType.ToString());
        }

        public static string ToString(OAuthProblemType problemType, OAuthProblemSubType problemSubType, string problemMessage)
        {
            return string.Format(FORMAT_PROBLEM_SUB, ToString(problemType), ToString(problemSubType, problemMessage));
        }

        public static string ToString(OAuthProblemType problemType, OAuthProblemSubType problemSubType, string startRange, string endRange)
        {
            return string.Format(FORMAT_PROBLEM_SUB, ToString(problemType), ToString(problemSubType, string.Format(FORMAT_RANGE, startRange, endRange)));
        }

        public static string ToString(OAuthProblemSubType problemSubType, string problemMessage)
        {
            return string.Format(FORMAT_OAUTH_PROBLEM, problemSubType.ToString(), problemMessage);
        }

        public static string ToString(OAuthProblemSubType problemSubType, string startRange, string endRange)
        {
            return ToString(problemSubType, string.Format(FORMAT_RANGE, startRange, endRange));
        }

        #region oauth problem reporting output

        public static string VersionRejected()
        {
            return ToString(OAuthProblemType.version_rejected, OAuthProblemSubType.oauth_acceptable_versions, GeneralUtil.OAuthVersionTypeToString(OAuthVersionType.Version1), GeneralUtil.OAuthVersionTypeToString(OAuthVersionType.Version1));
        }

        public static string ParameterAbsent(string missingParameters)
        {
            return ToString(OAuthProblemType.parameter_absent, OAuthProblemSubType.oauth_parameters_absent, missingParameters);
        }

        public static string NonceUsed()
        {
            return ToString(OAuthProblemType.nonce_used);
        }

        public static string TimeStampRefused(string startRange, string endRange)
        {
            return ToString(OAuthProblemType.timestamp_refused, OAuthProblemSubType.oauth_acceptable_timestamps, startRange, endRange);
        }

        public static string SignatureMethodRejected()
        {
            return ToString(OAuthProblemType.signature_method_rejected);
        }

        public static string SignatureInvalid()
        {
            return ToString(OAuthProblemType.signature_invalid);
        }

        public static string ConsumerKeyRefused()
        {
            return ToString(OAuthProblemType.consumer_key_refused);
        }

        public static string ConsumerKeyRejected()
        {
            return ToString(OAuthProblemType.consumer_key_rejected);
        }

        public static string ConsumerKeyUnknown()
        {
            return ToString(OAuthProblemType.consumer_key_unknown);
        }

        public static string TokenExpired()
        {
            return ToString(OAuthProblemType.token_expired);
        }

        public static string TokenRejected()
        {
            return ToString(OAuthProblemType.token_rejected);
        }

        public static string AdditionalAuthorizationRequired()
        {
            return ToString(OAuthProblemType.additional_authorization_required);
        }

        public static string PermissionDenied()
        {
            return ToString(OAuthProblemType.permission_denied);
        }

        public static string UserRefused()
        {
            return ToString(OAuthProblemType.user_refused);
        }

        #endregion
    }
}