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


        [JsonProperty("threads")]
#pragma warning disable IDE0044, CS0649
        private string _threads; //string cuz the user might not put anything in there or invalid
#pragma warning restore IDE0044, CS0649


        public int Threads
        {
            get
            {
                if (!int.TryParse(_threads, out int threads) || threads < 1 || threads > 2000)
                    return 2;
                else
                    return threads;
            }
        }
    }
}
