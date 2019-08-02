using System;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class BotRequest
    {
        [JsonProperty("op")]
#pragma warning disable CS0649
        private readonly string _op;
#pragma warning restore CS0649

        public BotOpcode Opcode
        {
            get { return (BotOpcode)Enum.Parse(typeof(BotOpcode), _op, true); }
        }
    }
}
