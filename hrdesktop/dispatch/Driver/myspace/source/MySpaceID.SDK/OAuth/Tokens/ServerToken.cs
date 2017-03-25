using MySpaceID.SDK.OAuth.Common.Utils;

namespace MySpaceID.SDK.OAuth.Tokens
{
    public class ServerToken : OAuthToken
    {
        #region ctor
        public ServerToken() : base(GeneralUtil.GenerateKey(16), GeneralUtil.GenerateKey(32))
        {
        }
        #endregion
    }
}
