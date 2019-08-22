using Newtonsoft.Json;

namespace DiscordHaxx
{
    class ModResponse : BotRequest
    {
        public ModResponse() : base(BotOpcode.BotModify)
        { }


        [JsonProperty("at")]
        public string At { get; set; }


        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
