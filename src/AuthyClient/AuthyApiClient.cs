using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RestSharp;

namespace AuthyClient
{
    /// <summary>
    /// Client to easily interface with the Authy HTTP API
    /// As documented here: http://docs.authy.com/
    /// </summary>
    public class AuthyApiClient : IAuthyApiClient
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
            cellphone = SanitizeNumber(cellphone);

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
                                                cellphone = SanitizeNumber(request.Cellphone),
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
        /// <param name="force">This dictates wether or not to force verification of a token regardless of registration status.</param>
        /// <returns>boolean representing if the user is authenticated.</returns>
        /// <exception cref="AuthyClientException">Thrown when the api response is not 200 (OK)</exception>
        public bool VerifyUserToken(string authyUserId, string authyToken, bool force = true)
        {
            if (!NumberIsValid(authyUserId))
                return false;

            var client =
                new RestClient(string.Format("{0}verify/{1}/{2}?api_key={3}&force={4}", _baseUrl, authyToken, authyUserId, _apiKey, force));

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
                        throw new AuthyClientException("Unexpected response: " + response.Content, new Dictionary<string, string>());
                    }
                }
                    
            }
        }

        /// <summary>
        /// Send a notification to the user for them to retrieve their token.
        /// For user's with the app installed and notifications enabled, this comes in the form of a push notification. If a user
        /// don't receive the push notification and would like to force that an SMS be sent containing the token, set forceSmsSend == true
        /// </summary>
        /// <param name="authyUserId">Authy User Id. Returned when creating an Authy User.</param>
        /// <param name="forceSmsSend">If false, a push notification is sent if the user has the Authy app, and an SMS is sent if they do not. If set to true, an SMS is sent regardless.</param>
        public void SendToken(string authyUserId, bool forceSmsSend = false)
        {
            if (!NumberIsValid(authyUserId))
                throw new AuthyClientException("AuthyUseId is expected to be digits only, but was: " + authyUserId, new Dictionary<string, string>());

            var client = new RestClient(string.Format("{0}sms/{1}?api_key={2}&force={3}", _baseUrl, authyUserId, _apiKey, forceSmsSend));

            var authyRequest = new RestRequest(Method.GET);
            authyRequest.RequestFormat = DataFormat.Json;

            var response = client.Execute<AuthySmsResponse>(authyRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new AuthyClientException("Unexpected response: " + response.Content, new Dictionary<string, string>());
            }
        }

        /// <summary>
        /// Ensure there are only non-digits in the string
        /// </summary>
        /// <param name="number">number to check</param>
        /// <returns>a number that has all non-digits removed.</returns>
        private string SanitizeNumber(string number)
        {
            return Regex.Replace(number, @"\D", string.Empty);
        }

        /// <summary>
        /// Check if a number contains any non-digits
        /// </summary>
        /// <param name="number">number to check</param>
        /// <returns>true number doesn't contain any non-digits; false if number does.</returns>
        private bool NumberIsValid(string number)
        {
            return !Regex.IsMatch(number, @"\D");
        }

        /// <summary>
        /// Ensure that the token lenght is not greater than 10 or less than 6.
        /// This is per a security advisory issued by Authy.
        /// </summary>
        /// <param name="token">Authy Token</param>
        /// <returns>true if token is valid; false if token is not.</returns>
        private bool TokenIsValid(string token)
        {
            if (token.Length < 6 || token.Length > 10)
                return false;
            
            return true;
        }
    }
}