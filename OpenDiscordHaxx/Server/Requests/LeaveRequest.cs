using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class LeaveRequest : Request
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }
    }
}
