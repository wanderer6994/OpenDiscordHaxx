using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class StatusUpdate
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
