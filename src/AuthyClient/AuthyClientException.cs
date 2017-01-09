using System;
using System.Collections.Generic;
using System.Net;

namespace AuthyClient
{
    public class AuthyClientException : Exception
    {
        public Dictionary<string, string> Errors { get; set; }
        public HttpStatusCode? HttpErrorCode { get; }

        public AuthyClientException(string response, Dictionary<string, string> errors, HttpStatusCode? httpErrorCode = null) : base(response)
        {
            Errors = errors;
            HttpErrorCode = httpErrorCode;
        }
    }
}