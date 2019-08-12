using System;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class Attack
    {
        private static int _currentAttackId = 0;

        [JsonIgnore]
        public Bot Bot { get; private set; }

        public Attack(Bot bot)
        {
            Id = _currentAttackId;
            _currentAttackId++;

            Bot = bot;
        }


        [JsonProperty("type")]
        private string _type;

        public RaidOpcode Type
        {
            get { return (RaidOpcode)Enum.Parse(typeof(RaidOpcode), _type, true); }
            set { _type = value.ToString(); }
        }


        [JsonProperty("bots")]
        public int Bots { get; set; }


        [JsonProperty("id")]
        public int Id { get; private set; }
    }
}
