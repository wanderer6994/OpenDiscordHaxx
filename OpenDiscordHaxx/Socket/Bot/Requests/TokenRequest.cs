using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    class TokenRequest : BotRequest
    {
        public TokenRequest(BotOpcode op) : base(op)
        { }



        [JsonProperty("id")]
        public string Id { get; set; }


        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
