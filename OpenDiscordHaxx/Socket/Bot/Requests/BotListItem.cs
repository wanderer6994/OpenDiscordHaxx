using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class BotListItem
    {
        [JsonProperty("at")]
        public string At { get; set; }


        [JsonProperty("id")]
        public string Id { get; set; }


        [JsonProperty("verification")]
        public string Verification { get; set; }
    }
}
