using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReadLater5.CustomExceptions
{
    public class ErrorResponse
    {
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp => DateTime.UtcNow;

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}
