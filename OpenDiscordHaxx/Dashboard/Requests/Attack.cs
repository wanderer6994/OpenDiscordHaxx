using System;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class Attack
    {
        private static int _currentAttackId = 0;

        [JsonIgnore]
        public RaidBot Bot { get; private set; }

        public Attack(RaidBot bot)
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


        [JsonProperty("threads")]
        public int Threads
        {
            get { return Bot.Threads; }
        }


        [JsonProperty("id")]
        public int Id { get; private set; }
    }
}
