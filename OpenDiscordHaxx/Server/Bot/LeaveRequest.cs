using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class LeaveRequest : BotRequest
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }
    }
}
