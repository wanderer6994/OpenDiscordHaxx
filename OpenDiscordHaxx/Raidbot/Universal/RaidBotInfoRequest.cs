using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class RaidBotInfoRequest
    {
        [JsonProperty("op")]
        private string _op = "info";


        [JsonProperty("socket_clients")]
        public bool SocketClients { get; set; }
    }
}
