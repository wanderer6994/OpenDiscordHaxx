using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class VCRequest : RaidRequest
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        [JsonProperty("channel_id")]
        public ulong ChannelId { get; private set; }


        [JsonProperty("join")]
        public bool Join { get; private set; }
    }
}
