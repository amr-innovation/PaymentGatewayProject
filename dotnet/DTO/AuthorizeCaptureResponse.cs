using Newtonsoft.Json;

namespace server.Models
{
    public class AuthorizeCaptureResponse
    {
        [JsonProperty("clientSecret")]
        public string clientSecret { get; set; }
    }
}
