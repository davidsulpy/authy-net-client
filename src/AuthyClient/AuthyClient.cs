using System.Net;
using RestSharp;

namespace AuthyClient
{
    public class AuthyClient
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public AuthyClient(string apiKey, bool testMode = true)
        {
            _apiKey = apiKey;
            _baseUrl = string.Format("{0}.authy.com/protected/json/", testMode ? "http://sandbox-api" : "https://api");
        }

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
    }
}