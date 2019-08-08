using Newtonsoft.Json;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using Discord;
using System.Collections.Generic;

namespace DiscordHaxx
{
    public class Checker : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Task.Run(() =>
            {
                Send(JsonConvert.SerializeObject(new CheckerRequest(CheckerOpcode.Started)));

                foreach (var client in new List<DiscordClient>(Server.Bots))
                {
                    BotCheckedRequest req = new BotCheckedRequest(CheckerOpcode.BotChecked)
                    { Bot = BotInfo.FromClient(client) };


                    try
                    {
                        client.JoinGuild("a");
                    }
                    catch (DiscordHttpException e)
                    {
                        req.Valid = e.Code == DiscordError.UnknownInvite;
                    }


                    Send(JsonConvert.SerializeObject(req));

                    
                }


                Send(JsonConvert.SerializeObject(new CheckerRequest(CheckerOpcode.Done)));
            });
        }
    }
}
