using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class ListRequest : BotRequest
    {
        public ListRequest(List<BotInfo> bots) : base(BotOpcode.List)
        {
            _bots = bots;
        }


        [JsonProperty("bots")]
#pragma warning disable IDE0052
        private readonly List<BotInfo> _bots;
#pragma warning restore IDE0052
    }
}
