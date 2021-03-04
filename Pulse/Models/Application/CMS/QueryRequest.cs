
using Newtonsoft.Json;
namespace Pulse
{
    public class QueryRequest
    {
        [JsonProperty("email")]
        public string Email
        {
            get;
            set;
        }
        [JsonProperty("text_message")]
        public string Message
        {
            get;
            set;
        }
    }
}
