using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp.Server;
using Discord;
using System;

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
                case BotOpcode.BotModification:
                    ModRequest modReq = JsonConvert.DeserializeObject<ModRequest>(e.Data);
                    DiscordClient modClient = Server.Bots.First(c => c.User.Id == modReq.Id);

                    try
                    {
                        if (modClient.User.Hypesquad != modReq.Hypesquad)
                            modClient.User.SetHypesquad(modReq.Hypesquad);

                        modClient.User.Update();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error on modifying account:\n{ex}");
                    }

                    SendList();
                    break;
            }
        }


        private void SendList()
        {
            List<BotInfo> bots = new List<BotInfo>();
            foreach (var client in Server.Bots)
                bots.Add(BotInfo.FromClient(client));

            Send(new ListRequest(bots));
        }
    }
}
