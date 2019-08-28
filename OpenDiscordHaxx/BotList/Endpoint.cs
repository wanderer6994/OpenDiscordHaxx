using Newtonsoft.Json;
using System.Linq;
using WebSocketSharp.Server;
using Discord;
using Newtonsoft.Json.Linq;

namespace DiscordHaxx
{
    public class BotListEndpoint : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Send(new ListRequest(ListAction.Add, Server.Bots));
        }


        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            JObject obj = JsonConvert.DeserializeObject<JObject>(e.Data);

            switch (obj.GetValue("op").ToObject<ListOpcode>())
            {
                case ListOpcode.Token:
                    TokenRequest tokenReq = obj.ToObject<TokenRequest>();
                    DiscordClient client = Server.Bots.First(c => c.User.Id == tokenReq.Id);
                    tokenReq.Token = client.Token;
                    tokenReq.At = client.User.ToString();

                    Send(tokenReq);
                    break;
                case ListOpcode.BotModify:
                    ModRequest modReq = obj.ToObject<ModRequest>();
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

                    SocketServer.Broadcast("/list", new ListRequest(ListAction.Update, modClient));
                    break;
            }
        }
    }
}
