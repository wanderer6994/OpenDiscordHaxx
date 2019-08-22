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
                    DiscordClient client = Server.Bots.First(c => c.User.Id == tokenReq.Id);
                    tokenReq.Token = client.Token;
                    tokenReq.At = client.User.ToString();

                    Send(tokenReq);
                    break;
                case BotOpcode.BotModify:
                    ModRequest modReq = JsonConvert.DeserializeObject<ModRequest>(e.Data);
                    DiscordClient modClient = Server.Bots.First(c => c.User.Id == modReq.Id);

                    ModResponse resp = new ModResponse { At = modClient.User.ToString() };

                    try
                    {
                        if (modClient.User.Hypesquad != modReq.Hypesquad)
                            modClient.User.SetHypesquad(modReq.Hypesquad);

                        modClient.User.Update();

                        resp.Success = true;
                    }
                    catch { }

                    Send(resp);

                    SendList();
                    break;
            }
        }

        private void SendList()
        {
            List<BotInfo> bots = new List<BotInfo>();
            foreach (var client in Server.Bots)
                bots.Add(BotInfo.FromClient(client));

            SocketServer.Broadcast("/bot", new ListRequest(bots));
        }
    }
}
