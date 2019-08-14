using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class ListRequest : BotRequest
    {
        public ListRequest(List<BotInfo> bots) : base(BotOpcode.List)
        {
            List = bots;
        }


        [JsonProperty("list")]
        public List<BotInfo> List { get; private set; }
    }
}
