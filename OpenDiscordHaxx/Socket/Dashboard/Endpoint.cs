using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    public class DashboardEndpoint : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Send(new DashboardRequest<StatusUpdate>(DashboardOpcode.StatusUpdate)
                                     { Data = new StatusUpdate() { Status = Server.ServerStatus } });
            Send(new DashboardRequest<OverlookUpdate>(DashboardOpcode.OverlookUpdate)
                                     { Data = new OverlookUpdate() { Accounts = Server.Bots.Count, Attacks = Server.OngoingAttacks.Count } });
            Send(new DashboardRequest<List<Attack>>(DashboardOpcode.AttacksUpdate)
                                     { Data = Server.OngoingAttacks.ToList() });
        }


        protected override void OnMessage(MessageEventArgs e)
        {
            switch (JsonConvert.DeserializeObject<JObject>(e.Data).GetValue("op").ToObject<DashboardOpcode>())
            {
                case DashboardOpcode.KillAttack:
                    AttackKillRequest req = JsonConvert.DeserializeObject<AttackKillRequest>(e.Data);

                    foreach (var attack in Server.OngoingAttacks)
                    {
                        if (attack.Id == req.Id)
                            attack.Bot.ShouldStop = true;
                    }
                    break;
            }
        }
    }
}