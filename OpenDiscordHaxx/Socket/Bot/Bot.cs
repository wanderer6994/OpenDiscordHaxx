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
    public class Bot : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            switch (JsonConvert.DeserializeObject<BotRequest>(e.Data).Opcode)
            {
                case BotOpcode.List:
                    List<BotListItem> bots = new List<BotListItem>();
                    foreach (var client in Server.Bots)
                    {
                        BotListItem bot = new BotListItem() { At = client.User.ToString(), Id = client.User.Id.ToString() };

                        if (client.User.TwoFactorAuth)
                            bot.Verification = "Phone verified";
                        else if (client.User.EmailVerified)
                            bot.Verification = "Email verified";
                        else
                            bot.Verification = "None or locked";

                        bots.Add(bot);
                    }

                    Send(JsonConvert.SerializeObject(new ListRequest(BotOpcode.List) { List = bots }));
                    break;
                case BotOpcode.Token:
                    TokenRequest req = JsonConvert.DeserializeObject<TokenRequest>(e.Data);
                    req.Token = Server.Bots.First(client => client.User.Id == ulong.Parse(req.Id)).Token;

                    Send(JsonConvert.SerializeObject(req));
                    break;
            }
        }
    }
}
