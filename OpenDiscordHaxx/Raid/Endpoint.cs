using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    class RaidSuccessStatus
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }


        [JsonProperty("message")]
        public string Message { get; set; }
    }


    public class RaidBotEndpoint : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            RaidSuccessStatus status = new RaidSuccessStatus();

            if (Server.Bots.Count > 0)
            {
                try
                {
                    Bot bot = null;

                    switch (JsonConvert.DeserializeObject<RaidRequest>(e.Data).Opcode)
                    {
                        case RaidOpcode.Join:
                            bot = new Joiner(JsonConvert.DeserializeObject<JoinRequest>(e.Data));
                            break;
                        case RaidOpcode.Leave:
                            bot = new Leaver(JsonConvert.DeserializeObject<LeaveRequest>(e.Data));
                            break;
                        case RaidOpcode.Flood:
                            bot = new Flooder(JsonConvert.DeserializeObject<FloodRequest>(e.Data));
                            break;
                        case RaidOpcode.Friend:
                            bot = new Friender(JsonConvert.DeserializeObject<FriendRequest>(e.Data));
                            break;
                        case RaidOpcode.React:
                            bot = new Reactions(JsonConvert.DeserializeObject<ReactionsRequest>(e.Data));
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
                catch (JsonReaderException)
                {
                    status.Message = "Invalid input";
                }
            }
            else
            {
                status.Message = "No bots are loaded";
            }

            Send(JsonConvert.SerializeObject(status));
        }
    }
}
