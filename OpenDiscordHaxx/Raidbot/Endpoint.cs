using WebSocketSharp.Server;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Discord;
using System.Linq;

namespace DiscordHaxx
{
    public class RaidBotEndpoint : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Send(JsonConvert.SerializeObject(new RaidBotInfoRequest() { SocketClients = Server.Bots.Where(b => b.SocketClient).Count() > 0 }));
        }

        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            BotStartSuccess status = new BotStartSuccess();

            if (Server.Bots.Count > 0)
            {
                try
                {
                    RaidBot bot = null;

                    JObject obj = JsonConvert.DeserializeObject<JObject>(e.Data);

                    switch (obj.GetValue("op").ToString())
                    {
                        case "join":
                            JoinRequest join = obj.ToObject<JoinRequest>();

                            bot = new Joiner(join.Invite, join.Threads, join.EnableAntiTrack);
                            break;
                        case "leave":
                            bot = new Leaver(obj.ToObject<LeaveRequest>());
                            break;
                        case "flood":
                            bot = new Flooder(obj.ToObject<FloodRequest>());
                            break;
                        case "friend":
                            bot = new Friender(obj.ToObject<FriendRequest>());
                            break;
                        case "react":
                            bot = new ReactionSpammer(obj.ToObject<ReactionsRequest>());
                            break;
                        case "vc":
                            bot = new VCSpammer(obj.ToObject<VCRequest>());
                            break;
                    }

                    Task.Run(() => bot.Start());

                    Server.OngoingAttacks.Add(bot.Attack);

                    status.Succeeded = true;
                    status.Message = "Bot should be starting shortly";
                }
                catch (CheckException ex)
                {
                    status.Message = ex.Issue;
                }
                catch (RateLimitException ex)
                {
                    if (ex.RetryAfter == 0)
                        status.Message = "You are CF ratelimited";
                    else
                        status.Message = ex.Message;
                }
                catch (JsonReaderException)
                {
                    status.Message = "Invalid input";
                }
            }
            else
                status.Message = "No bots are loaded";

            Send(JsonConvert.SerializeObject(status));
        }
    }
}
