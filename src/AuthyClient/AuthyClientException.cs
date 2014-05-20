using System;
using System.Collections.Generic;

namespace AuthyClient
{
    public class AuthyClientException : Exception
    {
        public Dictionary<string, string> Errors { get; set; }

        public AuthyClientException(string response, Dictionary<string, string> errors) : base(response)
        {
            Errors = errors;
        }
    }
}