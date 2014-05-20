namespace AuthyClient
{
    public interface IAuthyClient
    {
        string CreateAuthyUser(AuthyUser request);
        bool VerifyUserToken(string authyUserId, string authyToken);
    }
}