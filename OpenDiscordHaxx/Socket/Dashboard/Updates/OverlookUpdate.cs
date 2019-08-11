using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class OverlookUpdate
    {
        [JsonProperty("accounts")]
        public int Accounts { get; set; }


        [JsonProperty("attacks")]
        public int Attacks { get; set; }
    }
}
