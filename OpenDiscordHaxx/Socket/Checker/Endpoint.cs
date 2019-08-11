using Newtonsoft.Json;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using Discord;
using System.Collections.Generic;

namespace DiscordHaxx
{
    public class CheckerEndpoint : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Task.Run(() =>
            {
                Send(JsonConvert.SerializeObject(new CheckerStartedRequest(CheckerOpcode.Started)));

                int valid = 0;
                int invalid = 0;
                int total = Server.Bots.Count;
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


                    if (req.Valid)
                        valid++;
                    else
                    {
                        Server.Bots.Remove(client);
                        invalid++;
                    }


                    req.Progress = new CheckerProgress()
                    {
                        Valid = valid,
                        Invalid = invalid,
                        Total = total
                    };


                    Send(JsonConvert.SerializeObject(req));
                }


                Send(JsonConvert.SerializeObject(new CheckerRequest(CheckerOpcode.Done)));
            });
        }
    }
}
