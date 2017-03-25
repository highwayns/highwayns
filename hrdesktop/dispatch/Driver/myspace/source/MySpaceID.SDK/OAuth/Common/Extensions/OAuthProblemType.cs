namespace MySpaceID.SDK.OAuth.Common.Extensions
{
    public enum OAuthProblemType
    {
        version_rejected = 0,
        parameter_absent,
        parameter_rejected,
        timestamp_refused,
        nonce_used,
        signature_method_rejected, //5
        signature_invalid,
        consumer_key_unknown,
        consumer_key_rejected,
        consumer_key_refused,
        token_used, //10
        token_expired,
        token_revoked,
        token_rejected,
        additional_authorization_required,
        permission_unknown, //15
        permission_denied,
        user_refused,
    }
}