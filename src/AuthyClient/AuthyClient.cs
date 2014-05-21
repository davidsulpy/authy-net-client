using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace AuthyClient
{
    /// <summary>
    /// Client to easily interface with the Authy HTTP API
    /// As documented here: http://docs.authy.com/
    /// </summary>
    public class AuthyApiClient : IAuthyClient
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;

        /// <summary>
        /// Construct an AuthyClient instance
        /// </summary>
        /// <param name="apiKey">authy api key, retrieve from authy dashboard</param>
        /// <param name="testMode">defaults to true, determines wither to use api.authy or sandbox-api.authy urls</param>
        public AuthyApiClient(string apiKey, bool testMode = true)
        {
            _apiKey = apiKey;
            _baseUrl = string.Format("{0}.authy.com/protected/json/", testMode ? "http://sandbox-api" : "https://api");
        }


        /// <summary>
        /// Creates a user in your Authy account and returns the Authy User Id.
        /// </summary>
        /// <param name="email">Email address of user to setup.</param>
        /// <param name="cellphone">Cellphone number of user to setup, should be unique to this user.</param>
        /// <param name="countryCode">Default 1 (US), set to integer country code of cellphone.</param>
        /// <returns>Returns an Authy User Id to store. This is necessary for authenticating a user's token later</returns>
        /// <exception cref="AuthyClientException">Thrown when the Authy Api response is not HttpStatusCode 2xx</exception>
        public string CreateAuthyUser(string email, string cellphone, int countryCode = 1)
        {
            return CreateAuthyUser(new AuthyUser(email, cellphone, countryCode));
        }

        /// <summary>
        /// Creates a user in your Authy account and returns the Authy User Id.
        /// </summary>
        /// <param name="request">This is a request object for all the parameters to send to Authy. Ignore Id as this get's set by Authy.</param>
        /// <returns>Returns an Authy User Id to store. This is necessary for authenticating a user's token later</returns>
        /// <exception cref="AuthyClientException">Thrown when the Authy Api response is not HttpStatusCode 2xx</exception>
        public string CreateAuthyUser(AuthyUser request)
        {
            var client = new RestClient(string.Format("{0}users/new?api_key={1}", _baseUrl, _apiKey));

            var authyRequest = new RestRequest(Method.POST);
            authyRequest.RequestFormat = DataFormat.Json;

            authyRequest.AddBody(new
                                 {
                                     user = new
                                            {
                                                email = request.Email,
                                                cellphone = request.Cellphone,
                                                country_code = request.CountryCode
                                            }
                                 });

            var response = client.Execute<AuthyResponse>(authyRequest);

            if ((int) response.StatusCode >= (int) HttpStatusCode.OK && (int) response.StatusCode < 300)
            {
                return response.Data.User.Id;
            }

            throw new AuthyClientException(response.Data.Message, response.Data.Errors);
        }

        /// <summary>
        /// Verify's a user's token with the Authy Api
        /// </summary>
        /// <param name="authyUserId">Authy User Id. This is the value returned from creating the Authy User.</param>
        /// <param name="authyToken">This is the token that the user is providing for verification.</param>
        /// <returns>boolean representing if the user is authenticated.</returns>
        /// <exception cref="AuthyClientException">Thrown when the api response is not 200 (OK)</exception>
        public bool VerifyUserToken(string authyUserId, string authyToken)
        {
            var client =
                new RestClient(string.Format("{0}verify/{1}/{2}?api_key={3}", _baseUrl, authyToken, authyUserId, _apiKey));

            var authyRequest = new RestRequest(Method.GET);
            authyRequest.RequestFormat = DataFormat.Json;

            var response = client.Execute<AuthyTokenResponse>(authyRequest);


            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return false;
                default:
                {
                    if (response.Data != null)
                    {
                        throw new AuthyClientException(response.Data.Token, response.Data.Errors);
                    }
                    else
                    {
                        throw new AuthyClientException("Unexpected response: "+ response.Content, new Dictionary<string, string>());
                    }
                }
                    
            }
        }
    }
}