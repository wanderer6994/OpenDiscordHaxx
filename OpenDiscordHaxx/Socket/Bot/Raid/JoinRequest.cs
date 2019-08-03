using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class JoinRequest : BotRequest
    {
        [JsonProperty("invite")]
        public string Invite { get; private set; }
    }
}
