using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class FloodRequest : BotRequest
    {
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; private set; }


        [JsonProperty("message")]
        public string Message { get; private set; }


        [JsonProperty("tts")]
#pragma warning disable CS0649
        private string _tts;
#pragma warning restore CS0649

        public bool Tts
        {
            get { return _tts == "on"; }
        }
    }
}
