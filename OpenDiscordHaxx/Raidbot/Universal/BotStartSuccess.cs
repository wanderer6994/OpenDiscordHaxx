using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class BotStartSuccess
    {
        [JsonProperty("op")]
        private string _op = "raid_success";


        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }


        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
