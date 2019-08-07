using System;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class RaidRequest
    {
        [JsonProperty("op")]
#pragma warning disable CS0649
        private readonly string _op;
#pragma warning restore CS0649


        public RaidOpcode Opcode
        {
            get
            {
                return (RaidOpcode)Enum.Parse(typeof(RaidOpcode), _op, true);
            }
        }
    }
}
