using System;
using MySpaceID.SDK.OAuth.Common.Enums;

namespace MySpaceID.SDK.OAuth.Common.Utils
{
    public static class GeneralUtil
    {
        public static readonly string VERSION1_TEXT = "1.0";
        public static readonly string VERSION2_TEXT = "2.0";

        public static readonly string PLAINTEXT = "PLAINTEXT";
        public static readonly string HMAC_SHA1 = "HMAC-SHA1";
        public static readonly string RSA_SHA1 = "RSA-SHA1";

        public static string GenerateKey()
        {
            return GenerateKey(32);
        }

        public static string GenerateKey(int size)
        {
            //Base64.encode64(OpenSSL::Random.random_bytes(size)).gsub(/\W/,'')
            return string.Empty;
        }

        public static string OAuthVersionTypeToString(OAuthVersionType versionType)
        {
            switch (versionType)
            {
                case OAuthVersionType.Version1:
                    return VERSION1_TEXT;
                case OAuthVersionType.Version2:
                    return VERSION2_TEXT;
                default:
                    throw new ArgumentOutOfRangeException("versionType");
            }
        }

        public static OAuthVersionType StringToOAuthVersionType(string versionType)
        {
            if (string.IsNullOrEmpty(versionType))
            {
                throw new ArgumentNullException("versionType");
            }

            if (versionType == VERSION1_TEXT)
            {
                return OAuthVersionType.Version1;
            }
            
            if (versionType == VERSION2_TEXT)
            {
                return OAuthVersionType.Version2;
            }

            throw new ArgumentOutOfRangeException("versionType");
        }

        public static string SignatureMethodTypeToString(SignatureMethodType signatureType)
        {
            switch (signatureType)
            {
                case SignatureMethodType.PLAINTEXT:
                    return PLAINTEXT;
                case SignatureMethodType.HMAC_SHA1:
                    return HMAC_SHA1;
                case SignatureMethodType.RSA_SHA1:
                    return RSA_SHA1;
                default:
                    throw new ArgumentOutOfRangeException("signatureType");
            }
        }

        public static SignatureMethodType StringToSignatureMethodType(string signatureType)
        {
            if (string.IsNullOrEmpty(signatureType))
            {
                throw new ArgumentNullException("signatureType");
            }

            signatureType = signatureType.Trim().ToUpper();

            if (signatureType == PLAINTEXT)
            {
                return SignatureMethodType.PLAINTEXT;
            }

            if (signatureType == HMAC_SHA1)
            {
                return SignatureMethodType.HMAC_SHA1;
            }

            if (signatureType == RSA_SHA1)
            {
                return SignatureMethodType.RSA_SHA1;
            }

            throw new ArgumentOutOfRangeException("signatureType");
        }

    }
}
