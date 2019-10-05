using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class BotStartSuccess
    {
        [JsonProperty("op")]
#pragma warning disable CS0414, IDE0051
        private readonly string _op = "raid_success";
#pragma warning restore CS0414, IDE0051


        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }


        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
