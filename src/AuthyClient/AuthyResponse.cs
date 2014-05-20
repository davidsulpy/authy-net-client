using System.Collections.Generic;

namespace AuthyClient
{
    public class AuthyResponse
    {
        public AuthyUser User { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}