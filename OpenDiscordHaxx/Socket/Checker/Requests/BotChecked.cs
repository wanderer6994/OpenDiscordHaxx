using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class BotCheckedRequest : CheckerRequest
    {
        public BotCheckedRequest(CheckerOpcode op) : base(op)
        { }


        [JsonProperty("valid")]
        public bool Valid { get; set; }


        [JsonProperty("bot")]
        public BotInfo Bot { get; set; }
    }
}
