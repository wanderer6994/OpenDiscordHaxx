using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class ReconRequest
    {
        public ReconRequest(ReconOpcode op)
        {
            Opcode = op;
        }


        [JsonProperty("op")]
        public ReconOpcode Opcode { get; private set; }
    }
}
