using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class BotCheckedRequest : CheckerRequest
    {
        public BotCheckedRequest() : base(CheckerOpcode.BotChecked)
        {
            Progress = new CheckerProgress();
        }


        [JsonProperty("valid")]
        public bool Valid { get; set; }


        [JsonProperty("bot")]
        public BotInfo Bot { get; set; }


        [JsonProperty("progress")]
        public CheckerProgress Progress { get; set; }
    }
}
