using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class RaidBotInfo
    {
        [JsonProperty("op")]
#pragma warning disable CS0414, IDE0051
        private readonly string _op = "info";
#pragma warning restore CS0414, IDE0051


        [JsonProperty("socket_clients")]
        public bool SocketClients { get; set; }


        [JsonProperty("bots")]
        public int Bots { get; set; }
    }
}