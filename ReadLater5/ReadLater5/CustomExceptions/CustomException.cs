using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ReadLater5.CustomExceptions
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Error { get; set; }

        public string ContentType { get; set; } = @"application/json";

        public CustomException() { }

        public CustomException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public CustomException(HttpStatusCode statusCode, string message, string error)
            : base(message)
        {
            StatusCode = statusCode;
            Error = error;
        }

        public CustomException(HttpStatusCode statusCode, List<string> messages, string error)
            : base(string.Join(", ", messages))
        {
            StatusCode = statusCode;
            Error = error;
        }

        public CustomException(HttpStatusCode statusCode, string message)
          : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
