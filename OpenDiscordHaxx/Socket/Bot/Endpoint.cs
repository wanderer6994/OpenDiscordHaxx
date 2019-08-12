using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp.Server;
using Discord;

namespace DiscordHaxx
{
    public class BotEndpoint : WebSocketBehavior
    {
        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            switch (JsonConvert.DeserializeObject<BotRequest>(e.Data).Opcode)
            {
                case BotOpcode.List:
                    SendList();
                    break;
                case BotOpcode.Token:
                    TokenRequest tokenReq = JsonConvert.DeserializeObject<TokenRequest>(e.Data);
                    tokenReq.Token = Server.Bots.First(c => c.User.Id == tokenReq.Id).Token;

                    Send(JsonConvert.SerializeObject(tokenReq));
                    break;
                case BotOpcode.BotModification:
                    ModRequest modReq = JsonConvert.DeserializeObject<ModRequest>(e.Data);

                    DiscordClient modClient = Server.Bots.First(c => c.User.Id == modReq.Id);

                    if (modClient.User.Hypesquad != modReq.Hypesquad)
                        modClient.User.SetHypesquad(modReq.Hypesquad);

                    modClient.User.Update();

                    SendList();
                    break;
            }
        }


        private void SendList()
        {
            List<BotInfo> bots = new List<BotInfo>();
            foreach (var client in Server.Bots)
                bots.Add(BotInfo.FromClient(client));

            Send(JsonConvert.SerializeObject(new ListRequest(BotOpcode.List) { List = bots }));
        }
    }
}
