using System;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class Attack
    {
        [JsonProperty("type")]
        private string _type;

        public RaidOpcode Type
        {
            get { return (RaidOpcode)Enum.Parse(typeof(RaidOpcode), _type, true); }
            set { _type = value.ToString(); }
        }


        [JsonProperty("bots")]
        public int Bots { get; set; }
    }
}
