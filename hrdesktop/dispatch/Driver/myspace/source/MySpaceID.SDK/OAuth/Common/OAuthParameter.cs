using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using MySpaceID.SDK.OAuth.Common.Enums;
using MySpaceID.SDK.OAuth.Common.Extensions;
using MySpaceID.SDK.OAuth.Common.Utils;
using MySpaceID.SDK.OAuth.Common.Parameters;

namespace MySpaceID.SDK.OAuth.Common
{
    public class OAuthParameter : BaseSecurityParameter
    {
        #region public const - encoding
        public static readonly string UNRESERVED_CHARS = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
        public static readonly string RESERVED_CHARS = @"`!@#$%^&*()_-+=.~,:;'?/|\[] ";
        #endregion

        #region public const - parameters
        public static string OAUTH_CONSUMER_KEY = "oauth_consumer_key";
        public static string OAUTH_TOKEN = "oauth_token";
        public static string OAUTH_SIGNATURE_METHOD = "oauth_signature_method";
        public static string OAUTH_TIMESTAMP = "oauth_timestamp";
        public static string OAUTH_NONCE = "oauth_nonce";
        public static string OAUTH_VERSION = "oauth_version";

        public static string OAUTH_AUTH_PREFIX = "oauth_";
        public static string OAUTH_SIGNATURE = "oauth_signature";
        public static string OAUTH_TOKEN_SECRET = "oauth_token_secret";
        public static string OAUTH_REALM = "realm"; //optional

        public static string OAUTH_CALLBACK = "oauth_callback"; //optional
        public static string OAUTH_CALLBACK_CONFIRMED = "oauth_callback_confirmed";
        public static string OAUTH_VERIFIER = "oauth_verifier";
        #endregion

        #region public properties - collection

        //public NameValueCollection ParameterCollection { get; set; }
        public NameValueCollection UnknownParameterCollection { get; set; }

        ////form
        ////querystring
        ////header
        #endregion

        #region public properties
        public string ConsumerKey { get; set; }
        public string Token { get; set; }
        public string SignatureMethod { get; set; }
        public string Timestamp { get; set; }
        public string Nonce { get; set; }
        public string Version { get; set; }

        public string Signature { get; set; }
        public string Realm { get; set; }
        public string TokenSecret { get; set; }
        #endregion

        #region public typed properties
        public DateTime TimestampDate
        {
            get
            {
                return this.UtcTicksToDateTime(Convert.ToInt64(this.Timestamp));
            }
            set
            {
                this.Timestamp = this.DateTimeToUtcTicks(value).ToString();
            }
        }

        public OAuthVersionType VersionType
        {
            get
            {
                return GeneralUtil.StringToOAuthVersionType(this.Version);
            }
            set
            {
                this.Version = GeneralUtil.OAuthVersionTypeToString(value);
            }
        }
        #endregion

        #region ctor
        public OAuthParameter(string consumerKey, string token, string signatureMethod, string timestamp, string nonce, string version)
        {
            this.ConsumerKey = consumerKey;
            this.Token = token;
            this.SignatureMethod = signatureMethod;
            this.Timestamp = timestamp;
            this.Nonce = nonce;
            this.Version = version;
        }

        public OAuthParameter(string consumerKey, string token, SignatureMethodType signatureMethod, DateTime timestamp, string nonce, OAuthVersionType version)
        {
            this.ConsumerKey = consumerKey;
            this.Token = token;
            this.SignatureMethod = GeneralUtil.SignatureMethodTypeToString(signatureMethod);
            this.Timestamp = this.DateTimeToUtcTicks(timestamp).ToString();
            this.Nonce = nonce;
            this.Version = GeneralUtil.OAuthVersionTypeToString(version);
        }

        public OAuthParameter(NameValueCollection values)
        {
            //required
            try
            {
                this.ConsumerKey = GetRequiredParameter(values, OAUTH_CONSUMER_KEY);
                //TODO: only required for /access_token
                //this.Token = GetRequiredParameter(values, OAUTH_TOKEN);
                this.Token = values[OAUTH_TOKEN] ?? string.Empty;
                this.SignatureMethod = GetRequiredParameter(values, OAUTH_SIGNATURE_METHOD);
                this.Timestamp = GetRequiredParameter(values, OAUTH_TIMESTAMP);
                this.Nonce = GetRequiredParameter(values, OAUTH_NONCE);
                this.Version = GetRequiredParameter(values, OAUTH_VERSION);
            }
            catch(Exception ex)
            {
                this.HandlerError(OAuthProblem.ParameterAbsent(ex.Message));
                return;
            }

            //validation
            if (this.VersionType != OAuthVersionType.Version1)
            {
                this.HandlerError(OAuthProblem.VersionRejected());
                return;
            }

            if (this.PreviousUsedNonce())
            {
                this.HandlerError(OAuthProblem.NonceUsed());
                return;
            }

            if (!this.IsValidTimeStamp())
            {
                var startTime = this.DateTimeToUtcTicks(DateTime.Now).ToString();
                //TODO: fix this 15
                var endTime = this.DateTimeToUtcTicks(DateTime.Now.AddMinutes(15)).ToString();

                this.HandlerError(OAuthProblem.TimeStampRefused(startTime, endTime));
                return;
            }

            if (!this.IsValidSignatureMethod())
            {
                this.HandlerError(OAuthProblem.SignatureMethodRejected());
                return;
            }

            //optional
            this.TokenSecret = values[OAUTH_TOKEN_SECRET];
            this.Realm = values[OAUTH_REALM];
            this.Signature = values[OAUTH_SIGNATURE];

            var copyList = new NameValueCollection(values);
            copyList.Remove(OAUTH_CONSUMER_KEY);
            copyList.Remove(OAUTH_TOKEN);
            copyList.Remove(OAUTH_SIGNATURE_METHOD);
            copyList.Remove(OAUTH_TIMESTAMP);
            copyList.Remove(OAUTH_NONCE);
            copyList.Remove(OAUTH_VERSION);

            copyList.Remove(OAUTH_TOKEN_SECRET);
            copyList.Remove(OAUTH_REALM);
            copyList.Remove(OAUTH_SIGNATURE);
            
            //unknown
            this.UnknownParameterCollection = new NameValueCollection();
            this.UnknownParameterCollection.Add(copyList);
        }
        #endregion

        #region public utils

        public bool PreviousUsedNonce()
        {
            return this.PreviousUsedNonce(this.Nonce);
        }

        public bool PreviousUsedNonce(string nonce)
        {
            //TODO: lookup nonce is its previously used
            return false;
        }

        public bool IsValidTimeStamp()
        {
            try
            {
                this.UtcTicksToDateTime(Convert.ToInt64(this.Timestamp));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsValidSignatureMethod()
        {
            if (this.SignatureMethod == GeneralUtil.SignatureMethodTypeToString(SignatureMethodType.HMAC_SHA1))
            {
                return true;
            }
            
            if (this.SignatureMethod == GeneralUtil.SignatureMethodTypeToString(SignatureMethodType.PLAINTEXT))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region public static methods - encoding
        public static string UrlEncode(string value)
        {
            var result = new StringBuilder();

            if (string.IsNullOrEmpty(value))
                return string.Empty;

            foreach (var symbol in value)
            {
                if (UNRESERVED_CHARS.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else if (RESERVED_CHARS.IndexOf(symbol) != -1)
                {
                    result.Append('%' + string.Format("{0:X2}", (int)symbol));
                }
                else
                {
                    var encoded = HttpUtility.UrlEncode(symbol.ToString());

                    if (!string.IsNullOrEmpty(encoded))
                    {
                        result.Append(encoded);
                    }
                }
            }

            return result.ToString();
        }
        #endregion

        #region public static methods - load
        public static OAuthParameter FromQueryString(string queryString)
        {
            var queryStringValues = HttpUtility.ParseQueryString(queryString);

            if (!ContainsRequiredParameters(queryStringValues))
            {
                throw new ArgumentException("missing some parameter in the querystring");
            }

            return FromNameValueCollection(queryStringValues);
        }

        public static OAuthParameter FromHeaders(string headers)
        {
            if (string.IsNullOrEmpty(headers))
            {
                throw new ArgumentNullException("headers");
            }

            var str = headers;

            if (!str.StartsWith(OAuthConstants.AUTHORIZATION_OAUTH, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("not an oauth authorization header");
            }

            str = str.Trim(OAuthConstants.AUTHORIZATION_OAUTH.ToCharArray()).TrimStart().Trim();

            //Content-Type
            //application/x-www-form-urlencoded

            //add form parameters

            var list = new NameValueCollection();

            foreach (var param in str.Split(HEADER_PARAMETER_SEPERATOR.ToCharArray()))
            {
                var item = param.Split(HEADER_VALUE_SEPERATOR.ToCharArray());

                if (item.Length == 2)
                {
                    var val = HttpUtility.UrlDecode(item[1]);
                    list.Add(item[0], val);
                } 
                else
                {
                    throw new ArgumentException("item is malformed: " + param);
                }
            }

            if (!ContainsRequiredParameters(list))
            {
                throw new ArgumentException("missing some parameter in the header");
            }

            return new OAuthParameter(list);
        }

        public static OAuthParameter FromNameValueCollection(NameValueCollection list)
        {
            return new OAuthParameter(list);
        }

        public static OAuthParameter FromHttpContext(NameValueCollection headers, NameValueCollection queryString)
        {
            var schemeType = DetermineSchemeType(headers, queryString);
            OAuthParameter oAuthParameter;

            switch (schemeType)
            {
                case AuthorizationSchemeType.Header:
                    //check headers
                    var authorization = headers[OAuthConstants.AUTHORIZATION_HEADER];

                    if (!string.IsNullOrEmpty(authorization))
                    {
                        oAuthParameter = FromHeaders(authorization);
                        return oAuthParameter;
                    }

                    return null;
                case AuthorizationSchemeType.QueryString:
                    //check querystring
                    oAuthParameter = FromNameValueCollection(queryString);
                    return oAuthParameter;
                case AuthorizationSchemeType.Body:
                    //check the body
                    throw new NotImplementedException();
                default:
                    return null;
                    //throw new ArgumentOutOfRangeException();
            }
        }

        public override NameValueCollection ToCollection()
        {
            var list = new NameValueCollection();

            if (!string.IsNullOrEmpty(this.ConsumerKey))
                list.Add(OAUTH_CONSUMER_KEY, this.ConsumerKey);

            if (!string.IsNullOrEmpty(this.Token))
                list.Add(OAUTH_TOKEN, this.Token);

            if (!string.IsNullOrEmpty(this.SignatureMethod))
                list.Add(OAUTH_SIGNATURE_METHOD, this.SignatureMethod);

            if (!string.IsNullOrEmpty(this.Timestamp))
                list.Add(OAUTH_TIMESTAMP, this.Timestamp);

            if (!string.IsNullOrEmpty(this.Nonce))
                list.Add(OAUTH_NONCE, this.Nonce);

            if (!string.IsNullOrEmpty(this.Version))
                list.Add(OAUTH_VERSION, this.Version);

            if (!string.IsNullOrEmpty(this.Signature))
                list.Add(OAUTH_SIGNATURE, this.Signature);

            if (!string.IsNullOrEmpty(this.TokenSecret))
                list.Add(OAUTH_TOKEN_SECRET, this.TokenSecret);

            if (!string.IsNullOrEmpty(this.Realm))
                list.Add(OAUTH_REALM, this.Realm);

            return list;
        }

        public static AuthorizationSchemeType DetermineSchemeType(NameValueCollection headers, NameValueCollection queryString)
        {
            //check? in header
            if (headers.Get(OAuthConstants.AUTHORIZATION_HEADER) != null)
            {
                var authorization = headers[OAuthConstants.AUTHORIZATION_HEADER];

                if (!string.IsNullOrEmpty(authorization))
                {
                    if (authorization.StartsWith(OAuthConstants.AUTHORIZATION_OAUTH, StringComparison.OrdinalIgnoreCase))
                    {
                        return AuthorizationSchemeType.Header;
                    }
                }
            }

            //check? in querystring
            if (ContainsAnyParameter(queryString))
            {
                return AuthorizationSchemeType.QueryString;
            }

            //check? in body
            //TODO: check body for authorization info

            return AuthorizationSchemeType.Unknown;
        }

        public static bool ContainsRequiredParameters(NameValueCollection list)
        {
            var hasRequired = true;

            var array = new ArrayList(list.AllKeys);
            hasRequired &= array.Contains(OAUTH_CONSUMER_KEY);
            hasRequired &= array.Contains(OAUTH_TOKEN);
            hasRequired &= array.Contains(OAUTH_SIGNATURE_METHOD);
            hasRequired &= array.Contains(OAUTH_TIMESTAMP);
            hasRequired &= array.Contains(OAUTH_NONCE);
            hasRequired &= array.Contains(OAUTH_VERSION);

            return hasRequired;
        }

        public static bool ContainsAnyParameter(NameValueCollection list)
        {
            var hasAny = false;

            var array = new ArrayList(list.AllKeys);
            hasAny |= array.Contains(OAUTH_CONSUMER_KEY);
            hasAny |= array.Contains(OAUTH_TOKEN);
            hasAny |= array.Contains(OAUTH_SIGNATURE_METHOD);
            hasAny |= array.Contains(OAUTH_TIMESTAMP);
            hasAny |= array.Contains(OAUTH_NONCE);
            hasAny |= array.Contains(OAUTH_VERSION);

            return hasAny;
        }
        #endregion
    }
}
