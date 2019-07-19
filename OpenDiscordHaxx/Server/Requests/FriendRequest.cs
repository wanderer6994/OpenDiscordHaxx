using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class FriendRequest : Request
    {
        [JsonProperty("user")]
        public string User { get; private set; }
    }
}
