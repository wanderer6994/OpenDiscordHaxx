using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    public class ReconEndpoint : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            switch (JsonConvert.DeserializeObject<ReconRequest>(e.Data).Opcode)
            {
                case ReconOpcode.StartRecon:

                    break;
            }
        }
    }
}
