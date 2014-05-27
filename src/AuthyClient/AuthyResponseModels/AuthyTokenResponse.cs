using System.Collections.Generic;

namespace AuthyClient
{
    public class AuthyTokenResponse
    {
        public string Token { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}