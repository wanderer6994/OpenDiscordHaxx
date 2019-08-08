using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class ListRequest : BotRequest
    {
        public ListRequest(BotOpcode op) : base(op)
        { }


        [JsonProperty("list")]
        public List<BotInfo> List { get; set; }
    }
}
