using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class FriendRequest : RaidRequest
    {
        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }
    }
}
