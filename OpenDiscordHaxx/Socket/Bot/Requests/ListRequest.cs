using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class ListRequest : BotRequest
    {
        public ListRequest(BotOpcode op) : base(op)
        { }


        [JsonProperty("list")]
        public List<BotListItem> List { get; set; }
    }
}
