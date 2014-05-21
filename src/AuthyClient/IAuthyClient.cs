namespace AuthyClient
{
    public interface IAuthyClient
    {
        /// <summary>
        /// Creates a user in your Authy account and returns the Authy User Id.
        /// </summary>
        /// <param name="email">Email address of user to setup.</param>
        /// <param name="cellphone">Cellphone number of user to setup, should be unique to this user.</param>
        /// <param name="countryCode">Default 1 (US), set to integer country code of cellphone.</param>
        /// <returns>Returns an Authy User Id to store. This is necessary for authenticating a user's token later</returns>
        /// <exception cref="AuthyClientException">Thrown when the Authy Api response is not HttpStatusCode 2xx</exception>
        string CreateAuthyUser(string email, string cellphone, int countryCode = 1);

        /// <summary>
        /// Creates a user in your Authy account and returns the Authy User Id.
        /// </summary>
        /// <param name="request">This is a request object for all the parameters to send to Authy. Ignore Id as this get's set by Authy.</param>
        /// <returns>Returns an Authy User Id to store. This is necessary for authenticating a user's token later</returns>
        /// <exception cref="AuthyClientException">Thrown when the Authy Api response is not HttpStatusCode 2xx</exception>
        string CreateAuthyUser(AuthyUser request);

        /// <summary>
        /// Verify's a user's token with the Authy Api
        /// </summary>
        /// <param name="authyUserId">Authy User Id. This is the value returned from creating the Authy User.</param>
        /// <param name="authyToken">This is the token that the user is providing for verification.</param>
        /// <returns>boolean representing if the user is authenticated.</returns>
        /// <exception cref="AuthyClientException">Thrown when the api response is not 200 (OK)</exception>
        bool VerifyUserToken(string authyUserId, string authyToken);
    }
}