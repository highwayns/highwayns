namespace MySpaceID.SDK.OAuth.Common.Interfaces
{
    public interface IErrorResponse
    {
        bool HasError { get; }
        string GetError();
    }
}