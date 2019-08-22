using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class FloodRequest : RaidRequest
    {
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; private set; }


        [JsonProperty("message")]
        public string Message { get; private set; }
    }
}
