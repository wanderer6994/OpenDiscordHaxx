using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class JoinRequest : Request
    {
        [JsonProperty("invite")]
        public string Invite { get; private set; }
    }
}
