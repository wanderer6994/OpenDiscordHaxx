using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class CheckerRequest
    {
        public CheckerRequest(CheckerOpcode op)
        {
            Opcode = op;
        }


        [JsonProperty("op")]
        public CheckerOpcode Opcode { get; set; }
    }
}
