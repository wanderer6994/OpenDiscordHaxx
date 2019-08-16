using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class BotRequest
    {
        public BotRequest(BotOpcode op)
        {
            Opcode = op;
        }


        [JsonProperty("op")]
        public BotOpcode Opcode { get; set; }


        public static implicit operator string(BotRequest instance)
        {
            return JsonConvert.SerializeObject(instance);
        }
    }
}
