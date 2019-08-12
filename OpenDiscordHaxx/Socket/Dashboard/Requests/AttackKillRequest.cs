using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class AttackKillRequest
    {
        [JsonProperty("opcode")]
        public DashboardOpcode Opcode { get; private set; }


        [JsonProperty("id")]
        public int Id { get; private set; }
    }
}
