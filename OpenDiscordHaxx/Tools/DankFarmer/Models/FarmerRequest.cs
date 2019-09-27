using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class FarmerRequest
    {
        public FarmerRequest()
        { }

        public FarmerRequest(FarmerOpcode op)
        {
            Opcode = op;
        }

        [JsonProperty("op")]
        public FarmerOpcode Opcode { get; private set; }
    }
}
