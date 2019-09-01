using System.Collections.Generic;
using Discord;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class ListRequest : BotRequest
    {
        public ListRequest(ListAction action, List<DiscordClient> bots) : base(ListOpcode.List)
        {
            List<BasicBotInfo> info = new List<BasicBotInfo>();
            foreach (var client in bots)
                info.Add(BasicBotInfo.FromClient(client));
            _bots = info;

            _action = action;
        }


        public ListRequest(ListAction action, DiscordClient bot) 
                        : this(action, new List<DiscordClient>() { bot }) { }


        [JsonProperty("bots")]
#pragma warning disable IDE0052
        private readonly List<BasicBotInfo> _bots;


        [JsonProperty("action")]
        private readonly ListAction _action;
#pragma warning restore IDE0052
    }
}
