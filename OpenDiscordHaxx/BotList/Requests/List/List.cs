using System.Collections.Generic;
using Discord;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class ListRequest : BotRequest
    {
        public ListRequest(ListAction action, List<DiscordClient> bots) : base(ListOpcode.List)
        {
            List<BotInfo> info = new List<BotInfo>();
            foreach (var client in bots)
                info.Add(BotInfo.FromClient(client));
            _bots = info;

            _action = action;
        }


        public ListRequest(ListAction action, DiscordClient bot) 
                        : this(action, new List<DiscordClient>() { bot }) { }


        [JsonProperty("bots")]
#pragma warning disable IDE0052
        private readonly List<BotInfo> _bots;


        [JsonProperty("action")]
        private readonly ListAction _action;
#pragma warning restore IDE0052
    }
}
