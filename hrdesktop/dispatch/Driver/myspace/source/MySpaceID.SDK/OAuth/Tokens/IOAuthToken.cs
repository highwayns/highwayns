
namespace MySpaceID.SDK.OAuth.Tokens
{
    public interface IOAuthToken : IToken
    {
        string TokenSecret { get; set; }

        string ToQueryString();
    }
}
