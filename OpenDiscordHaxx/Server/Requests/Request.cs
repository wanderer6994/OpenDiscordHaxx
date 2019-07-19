using System;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class Request
    {
        [JsonProperty("op")]
#pragma warning disable CS0649
        private readonly string _op;
#pragma warning restore CS0649

        public Opcode Opcode
        {
            get { return (Opcode)Enum.Parse(typeof(Opcode), _op, true); }
        }
    }
}
